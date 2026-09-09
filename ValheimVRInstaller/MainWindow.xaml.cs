using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;

namespace ValheimVRInstaller;

public partial class MainWindow : Window
{
    private string _gamePath = string.Empty;

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Log("====================================================================");
        Log("      ᚢ ᚨ ᛚ ᚺ ᛖ ᛁ ᛗ   ᚡ ᚱ  -  INSTALLATEUR ÉDITION FRANÇAISE       ");
        Log("====================================================================");
        Log("Recherche de l'installation de Valheim sur votre système...");

        _gamePath = FindValheimPath();
        if (!string.IsNullOrEmpty(_gamePath) && File.Exists(Path.Combine(_gamePath, "valheim.exe")))
        {
            TxtGamePath.Text = _gamePath;
            Log($"[OK] Valheim détecté : {_gamePath}");
            Log("Prêt pour l'installation du mod VR en Français.");
        }
        else
        {
            Log("[!] Répertoire Valheim non détecté automatiquement.");
            Log("Veuillez cliquer sur 'Parcourir...' pour sélectionner votre dossier Valheim.");
        }
    }

    private void Log(string message)
    {
        Dispatcher.Invoke(() =>
        {
            TxtLogs.AppendText(message + Environment.NewLine);
            Scroller.ScrollToEnd();
        });
    }

    private string FindValheimPath()
    {
        string[] candidates =
        {
            @"D:\SteamLibrary\steamapps\common\Valheim",
            @"C:\Program Files (x86)\Steam\steamapps\common\Valheim",
            @"C:\Steam\steamapps\common\Valheim",
            @"C:\SteamLibrary\steamapps\common\Valheim",
            @"E:\SteamLibrary\steamapps\common\Valheim",
            @"F:\SteamLibrary\steamapps\common\Valheim"
        };

        foreach (var p in candidates)
        {
            if (File.Exists(Path.Combine(p, "valheim.exe")) && Directory.Exists(Path.Combine(p, "valheim_Data")))
            {
                return p;
            }
        }

        // Lecture registre Steam
        try
        {
            using var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam");
            if (key?.GetValue("SteamPath") is string steamPath)
            {
                steamPath = steamPath.Replace('/', '\\');
                string vdf = Path.Combine(steamPath, "steamapps", "libraryfolders.vdf");
                if (File.Exists(vdf))
                {
                    var lines = File.ReadAllLines(vdf);
                    foreach (var line in lines)
                    {
                        var trimmed = line.Trim();
                        if (trimmed.StartsWith("\"path\""))
                        {
                            var parts = trimmed.Split('"');
                            if (parts.Length >= 4)
                            {
                                string libPath = parts[3].Replace(@"\\", @"\");
                                string valheimCandidate = Path.Combine(libPath, "steamapps", "common", "Valheim");
                                if (File.Exists(Path.Combine(valheimCandidate, "valheim.exe")))
                                {
                                    return valheimCandidate;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch
        {
            // Ignore les erreurs de registre
        }

        return string.Empty;
    }

    private void BtnBrowse_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFolderDialog
        {
            Title = "Sélectionnez le dossier du jeu Valheim (contenant valheim.exe)",
            Multiselect = false
        };

        if (dialog.ShowDialog() == true)
        {
            string selected = dialog.FolderName;
            if (File.Exists(Path.Combine(selected, "valheim.exe")))
            {
                _gamePath = selected;
                TxtGamePath.Text = _gamePath;
                Log($"[OK] Dossier sélectionné manuellement : {_gamePath}");
            }
            else
            {
                MessageBox.Show(
                    "Le dossier sélectionné ne contient pas 'valheim.exe'.\nAssurez-vous de sélectionner le répertoire principal de Valheim.",
                    "Dossier invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
        }
    }

    private async void BtnInstall_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_gamePath) || !File.Exists(Path.Combine(_gamePath, "valheim.exe")))
        {
            MessageBox.Show(
                "Veuillez sélectionner un dossier de jeu Valheim valide avant d'installer.",
                "Chemin introuvable",
                MessageBoxButton.OK,
                MessageBoxImage.Exclamation
            );
            return;
        }

        // Vérification processus en cours
        var procs = Process.GetProcessesByName("valheim");
        if (procs.Length > 0)
        {
            var res = MessageBox.Show(
                "Valheim est actuellement en cours d'exécution. Voulez-vous le fermer pour poursuivre l'installation ?",
                "Valheim en cours",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (res == MessageBoxResult.Yes)
            {
                foreach (var p in procs)
                {
                    try { p.Kill(); p.WaitForExit(3000); } catch { }
                }
                Log("[i] Valheim a été fermé.");
            }
            else
            {
                Log("[X] Installation annulée : le jeu doit être fermé.");
                return;
            }
        }

        BtnInstall.IsEnabled = false;
        PrgProgress.Value = 0;

        bool success = false;
        try
        {
            await Task.Run(() =>
            {
                Log("\n[>>>] Début de l'installation de Valheim VR Édition Française...");

                // 1. Extraction du payload embarqué
                var assembly = Assembly.GetExecutingAssembly();
                using var stream = assembly.GetManifestResourceStream("ValheimVRInstaller.payload.zip");

                if (stream == null)
                {
                    throw new InvalidOperationException("Archive payload.zip introuvable dans les ressources de l'exécutable !");
                }

                using var archive = new ZipArchive(stream, ZipArchiveMode.Read);
                int totalEntries = archive.Entries.Count;
                int current = 0;

                Log($"Extraction des fichiers ({totalEntries} composants)...");

                foreach (var entry in archive.Entries)
                {
                    current++;
                    string destinationPath = Path.Combine(_gamePath, entry.FullName);

                    // Si c'est un dossier
                    if (string.IsNullOrEmpty(entry.Name))
                    {
                        Directory.CreateDirectory(destinationPath);
                        continue;
                    }

                    string dir = Path.GetDirectoryName(destinationPath)!;
                    if (!Directory.Exists(dir))
                    {
                        Directory.CreateDirectory(dir);
                    }

                    entry.ExtractToFile(destinationPath, overwrite: true);

                    int progress = (int)((double)current / totalEntries * 80.0);
                    Dispatcher.Invoke(() => PrgProgress.Value = progress);
                }

                Log("[OK] Fichiers du mod et de BepInEx déployés avec succès.");

                // 2. Configuration personnalisée (Confort & Visée)
                string cfgPath = Path.Combine(_gamePath, "BepInEx", "config", "org.bepinex.plugins.valheimvrmod.cfg");
                bool enableComfort = true;
                bool enableAimSmoothing = true;

                Dispatcher.Invoke(() =>
                {
                    enableComfort = ChkComfortVignette.IsChecked == true;
                    enableAimSmoothing = ChkAimSmoothing.IsChecked == true;
                });

                ConfigureModSettings(cfgPath, enableComfort, enableAimSmoothing);
                Log("[OK] Paramètres VR personnalisés enregistrés.");

                Dispatcher.Invoke(() => PrgProgress.Value = 90);

                // 3. Raccourci Bureau
                bool createShortcut = true;
                Dispatcher.Invoke(() => createShortcut = ChkDesktopShortcut.IsChecked == true);

                if (createShortcut)
                {
                    CreateDesktopShortcut(_gamePath);
                    Log("[OK] Raccourci Bureau 'Valheim VR (FR)' créé.");
                }

                Dispatcher.Invoke(() => PrgProgress.Value = 100);
                success = true;
            });
        }
        catch (Exception ex)
        {
            Log($"[ERREUR] Échec de l'installation : {ex.Message}");
            MessageBox.Show($"Une erreur est survenue pendant l'installation :\n{ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        finally
        {
            BtnInstall.IsEnabled = true;
        }

        if (success)
        {
            Log("\n====================================================================");
            Log("  🎉 FÉLICITATIONS ! VALHEIM VR EST INSTALLÉ ET CONFIGURÉ !");
            Log("====================================================================");
            Log("1. Allumez votre casque VR (Quest Link, Index, Vive, etc.).");
            Log("2. Démarrez SteamVR.");
            Log("3. Cliquez sur le bouton '🥽 LANCER EN VR' ci-dessous ou sur le raccourci Bureau.");

            BtnLaunch.Visibility = Visibility.Visible;
            MessageBox.Show(
                "Valheim VR (Édition Française) a été installé avec succès !\n\nVous pouvez maintenant lancer le jeu en VR.",
                "Installation réussie !",
                MessageBoxButton.OK,
                MessageBoxImage.Information
            );
        }
    }

    private void ConfigureModSettings(string cfgPath, bool comfortVignette, bool aimSmoothing)
    {
        try
        {
            string cfgDir = Path.GetDirectoryName(cfgPath)!;
            if (!Directory.Exists(cfgDir))
            {
                Directory.CreateDirectory(cfgDir);
            }

            if (File.Exists(cfgPath))
            {
                var lines = File.ReadAllLines(cfgPath).ToList();
                for (int i = 0; i < lines.Count; i++)
                {
                    if (lines[i].Trim().StartsWith("ComfortVignetteEnabled"))
                    {
                        lines[i] = $"ComfortVignetteEnabled = {comfortVignette.ToString().ToLower()}";
                    }
                    else if (lines[i].Trim().StartsWith("BowAimSmoothing"))
                    {
                        lines[i] = $"BowAimSmoothing = {aimSmoothing.ToString().ToLower()}";
                    }
                }
                File.WriteAllLines(cfgPath, lines, Encoding.UTF8);
            }
            else
            {
                // Création initiale avec sections francisées
                var sb = new StringBuilder();
                sb.AppendLine("[Confort]");
                sb.AppendLine($"ComfortVignetteEnabled = {comfortVignette.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine("[Commandes]");
                sb.AppendLine($"BowAimSmoothing = {aimSmoothing.ToString().ToLower()}");
                File.WriteAllText(cfgPath, sb.ToString(), Encoding.UTF8);
            }
        }
        catch (Exception ex)
        {
            Log($"[!] Note config: {ex.Message}");
        }
    }

    private void CreateDesktopShortcut(string gamePath)
    {
        try
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
            string shortcutPath = Path.Combine(desktop, "Valheim VR (FR).url");

            string content = string.Join(Environment.NewLine, new[]
            {
                "[InternetShortcut]",
                "URL=steam://rungameid/892970",
                "IconIndex=0",
                $"IconFile={Path.Combine(gamePath, "valheim.exe")}"
            });

            File.WriteAllText(shortcutPath, content, Encoding.ASCII);
        }
        catch (Exception ex)
        {
            Log($"[!] Impossible de créer le raccourci Bureau : {ex.Message}");
        }
    }

    private async void BtnUninstall_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrEmpty(_gamePath) || !File.Exists(Path.Combine(_gamePath, "valheim.exe")))
        {
            MessageBox.Show("Veuillez sélectionner un dossier de jeu Valheim valide.", "Erreur", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        var confirm = MessageBox.Show(
            "Voulez-vous désactiver Valheim VR et restaurer Valheim en version originale (Écran Plat Vanilla) ?\nVos sauvegardes et personnages ne seront absolument pas touchés.",
            "Restaurer Vanilla",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question
        );

        if (confirm != MessageBoxResult.Yes) return;

        Log("\n[>>>] Désactivation de Valheim VR et restauration Vanilla...");

        await Task.Run(() =>
        {
            try
            {
                // Désactivation du loader Doorstop
                string winhttp = Path.Combine(_gamePath, "winhttp.dll");
                string winhttpDisabled = Path.Combine(_gamePath, "winhttp.dll.disabled");
                if (File.Exists(winhttp))
                {
                    if (File.Exists(winhttpDisabled)) File.Delete(winhttpDisabled);
                    File.Move(winhttp, winhttpDisabled);
                    Log("[OK] Doorstop désactivé (winhttp.dll -> winhttp.dll.disabled).");
                }

                // Suppression de ValheimVRMod.dll
                string modDll = Path.Combine(_gamePath, "BepInEx", "plugins", "ValheimVRMod.dll");
                if (File.Exists(modDll))
                {
                    File.Delete(modDll);
                    Log("[OK] ValheimVRMod.dll retiré de BepInEx\\plugins.");
                }

                // Nettoyage UnitySubsystems
                string subsys = Path.Combine(_gamePath, "valheim_Data", "UnitySubsystems", "XRSDKOpenVR");
                if (Directory.Exists(subsys))
                {
                    Directory.Delete(subsys, true);
                    Log("[OK] XRSDKOpenVR retiré de valheim_Data\\UnitySubsystems.");
                }

                Log("[OK] Valheim est restauré en mode écran plat d'origine !");
            }
            catch (Exception ex)
            {
                Log($"[ERREUR] Restauration partielle : {ex.Message}");
            }
        });

        MessageBox.Show(
            "Valheim a été restauré en version Vanilla écran plat !\nVous pouvez relancer le jeu normalement via Steam.",
            "Restauration terminée",
            MessageBoxButton.OK,
            MessageBoxImage.Information
        );
    }

    private void BtnLaunch_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Log("\n[>>>] Lancement de Valheim via Steam...");
            Process.Start(new ProcessStartInfo("steam://rungameid/892970") { UseShellExecute = true });
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erreur lors du lancement de Steam :\n{ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}