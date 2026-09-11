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
    // Type de casque VR détecté : "ALVR", "WiVRn", "SteamVR", "Aucun"
    private string _casqueVrDetecte = "Aucun";

    public MainWindow()
    {
        InitializeComponent();
        Loaded += MainWindow_Loaded;
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        Log("====================================================================");
        Log("      ᚢ ᚨ ᛚ ᚺ ᛖ ᛁ ᛗ   ᚡ ᚱ  -  INSTALLATEUR ÉDITION FRANÇAISE       ");
        Log("             ⚡ Conçu & Forgé par LordMadTrix ⚡                     ");
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

        // Détection du casque VR / client streaming Quest 3
        DetecterCasqueVR();
    }

    private void Log(string message)
    {
        Dispatcher.Invoke(() =>
        {
            TxtLogs.AppendText(message + Environment.NewLine);
            Scroller.ScrollToEnd();
        });
    }

    // =========================================================
    // DÉTECTION CASQUE VR / QUEST 3 (ALVR, WiVRn, SteamVR)
    // =========================================================
    private void DetecterCasqueVR()
    {
        Task.Run(() =>
        {
            string casque = "Aucun";
            string badge = "❌ Aucun client VR détecté";
            string flux = "";
            bool showALVR = true;

            // 1. Vérification ALVR (dossier AppData + registre + exe)
            string alvrAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ALVR");
            string alvrLocal = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ALVR");
            bool alvrFound = Directory.Exists(alvrAppData) || Directory.Exists(alvrLocal)
                || VerifierRegistreApp("ALVR") || TrouverExe("ALVR");

            if (alvrFound)
            {
                casque = "ALVR";
                badge = "✅ ALVR détecté (streaming Quest)";
                flux = "Quest 3 → Wi-Fi 6 → ALVR → SteamVR → Valheim VR";
                showALVR = false;
            }

            // 2. Vérification WiVRn
            if (casque == "Aucun")
            {
                string wivrnPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "WiVRn");
                bool wivrnFound = Directory.Exists(wivrnPath) || VerifierRegistreApp("WiVRn") || TrouverExe("WiVRn");
                if (wivrnFound)
                {
                    casque = "WiVRn";
                    badge = "✅ WiVRn détecté (streaming Quest open-source)";
                    flux = "Quest 3 → Wi-Fi 6 → WiVRn → OpenXR → Valheim VR";
                    showALVR = false;
                }
            }

            // 3. Vérification SteamVR natif (cabled / Index / Vive)
            if (casque == "Aucun")
            {
                try
                {
                    using var key = Registry.CurrentUser.OpenSubKey(@"Software\Valve\SteamVR");
                    if (key != null)
                    {
                        casque = "SteamVR";
                        badge = "✅ SteamVR détecté (filé / Index / Vive)";
                        flux = "Casque → USB/DP → SteamVR → Valheim VR";
                        showALVR = false;
                    }
                }
                catch { }
            }

            _casqueVrDetecte = casque;

            // Mise à jour de l'UI sur le thread principal
            Dispatcher.Invoke(() =>
            {
                TxtCasqueBadge.Text = badge;
                TxtFluxVR.Text = flux;

                // Couleur du badge selon l'état
                BadgeCasque.Background = casque == "Aucun"
                    ? System.Windows.Media.Brushes.DarkRed
                    : System.Windows.Media.Brushes.DarkGreen;

                TxtCasqueBadge.Foreground = casque == "Aucun"
                    ? System.Windows.Media.Brushes.OrangeRed
                    : System.Windows.Media.Brushes.LightGreen;

                // Activer la checkbox Quest 3 si ALVR ou WiVRn
                if (casque == "ALVR" || casque == "WiVRn")
                {
                    ChkQuest3Optimize.IsChecked = true;
                    ChkQuest3Optimize.IsEnabled = true;
                }

                // Bouton installer ALVR visible seulement si rien de trouvé
                BtnInstallALVR.Visibility = showALVR ? Visibility.Visible : Visibility.Collapsed;

                Log($"[VR] Casque détecté : {badge}");
            });
        });
    }

    // Vérifie dans la clé de désinstallation Windows si une app est installée
    private static bool VerifierRegistreApp(string nomApp)
    {
        string[] keys = {
            @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",
            @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"
        };
        foreach (var keyPath in keys)
        {
            try
            {
                using var key = Registry.LocalMachine.OpenSubKey(keyPath);
                if (key == null) continue;
                foreach (var sub in key.GetSubKeyNames())
                {
                    using var subKey = key.OpenSubKey(sub);
                    var name = subKey?.GetValue("DisplayName") as string;
                    if (name != null && name.Contains(nomApp, StringComparison.OrdinalIgnoreCase))
                        return true;
                }
            }
            catch { }
        }
        return false;
    }

    // Cherche un exécutable dans les chemins communs
    private static bool TrouverExe(string nom)
    {
        string[] paths = {
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles),
            Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
        };
        foreach (var root in paths)
        {
            if (string.IsNullOrEmpty(root)) continue;
            try
            {
                var found = Directory.EnumerateFiles(root, $"*{nom}*.exe", SearchOption.AllDirectories)
                    .Take(1).Any();
                if (found) return true;
            }
            catch { }
        }
        return false;
    }

    // Bouton : ouvrir la page de téléchargement ALVR
    private void BtnInstallALVR_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            Process.Start(new ProcessStartInfo(
                "https://github.com/alvr-org/ALVR/releases/latest") { UseShellExecute = true });
            Log("[VR] Page de téléchargement ALVR ouverte dans le navigateur.");
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Impossible d'ouvrir le navigateur :\n{ex.Message}", "Erreur", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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

                // 2. Configuration personnalisée (Confort, Visée & Profil Matériel VR)
                string cfgPath = Path.Combine(_gamePath, "BepInEx", "config", "org.bepinex.plugins.valheimvrmod.cfg");
                bool enableComfort = true;
                bool enableAimSmoothing = true;
                bool quest3Optimize = false;
                int hardwareProfile = 1; // 0 = Eco, 1 = Balanced, 2 = Ultra
                string profileName = "Équilibré (Recommandé)";

                Dispatcher.Invoke(() =>
                {
                    enableComfort = ChkComfortVignette.IsChecked == true;
                    enableAimSmoothing = ChkAimSmoothing.IsChecked == true;
                    quest3Optimize = ChkQuest3Optimize.IsChecked == true;
                    if (RadEco.IsChecked == true)
                    {
                        hardwareProfile = 0;
                        profileName = "Éco / Quest 2 (Fluide & Optimisé)";
                    }
                    else if (RadUltra.IsChecked == true)
                    {
                        hardwareProfile = 2;
                        profileName = "Ultra / Mythique (Fidélité Maximale)";
                    }
                    else
                    {
                        hardwareProfile = 1;
                        profileName = "Équilibré (Recommandé)";
                    }
                });

                if (quest3Optimize)
                    Log("[VR] Optimisation Quest 3 activée : 120Hz, H.265, bitrate 150 Mbps.");

                Log($"[i] Application du profil de performance : {profileName}");
                ConfigureModSettings(cfgPath, enableComfort, enableAimSmoothing, hardwareProfile, quest3Optimize);
                Log("[OK] Paramètres VR et optimisations matérielles enregistrés avec succès.");

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

    private void ConfigureModSettings(string cfgPath, bool comfortVignette, bool aimSmoothing, int hardwareProfile, bool quest3Optimize = false)
    {
        try
        {
            string cfgDir = Path.GetDirectoryName(cfgPath)!;
            if (!Directory.Exists(cfgDir))
            {
                Directory.CreateDirectory(cfgDir);
            }

            // Définition des valeurs selon le profil matériel choisi
            // hardwareProfile: 0 = Eco / Quest 2, 1 = Balanced, 2 = Ultra
            bool physicsSync = true;
            int targetPhysicsHz = quest3Optimize ? 120 : (hardwareProfile == 0 ? 80 : 90);
            bool shadowOpt = (hardwareProfile <= 1); // Ombres optimisées en Eco et Équilibré
            bool memoryCleanup = true;
            bool vrSharpening = true;
            bool vrBloom = true;
            bool amplifyOcclusion = (hardwareProfile >= 1); // AO désactivée en Eco pour préserver les FPS
            string buildingLOD = (hardwareProfile == 0) ? "1.5" : "1.0";

            var desiredSettings = new Dictionary<string, (string section, string value)>
            {
                { "EnableComfortVignette", ("Comfort", comfortVignette.ToString().ToLower()) },
                { "EnableBowAimSmoothing", ("Motion Control", aimSmoothing.ToString().ToLower()) },
                { "PhysicsSyncEnabled", ("Graphics", physicsSync.ToString().ToLower()) },
                { "VRTargetPhysicsHz", ("Graphics", targetPhysicsHz.ToString()) },
                { "ShadowOptimizationEnabled", ("Graphics", shadowOpt.ToString().ToLower()) },
                { "MemoryCleanupEnabled", ("General", memoryCleanup.ToString().ToLower()) },
                { "VRSharpeningEnabled", ("Graphics", vrSharpening.ToString().ToLower()) },
                { "VRAntiGlareBloomEnabled", ("Graphics", vrBloom.ToString().ToLower()) },
                { "UseAmplifyOcclusion", ("Graphics", amplifyOcclusion.ToString().ToLower()) },
                { "BuildingPieceDetailReductionFactor", ("Graphics", buildingLOD) }
            };

            if (quest3Optimize)
            {
                desiredSettings["Quest3StreamingEnabled"] = ("Quest3", "true");
                desiredSettings["Quest3RefreshRate"] = ("Quest3", "120");
                desiredSettings["Quest3VideoCodec"] = ("Quest3", "h265");
                desiredSettings["Quest3Bitrate"] = ("Quest3", "150");
                desiredSettings["Quest3SupersamplingRatio"] = ("Quest3", "1.3");
            }

            if (File.Exists(cfgPath))
            {
                var lines = File.ReadAllLines(cfgPath).ToList();
                var handledKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                for (int i = 0; i < lines.Count; i++)
                {
                    string trimmed = lines[i].Trim();
                    foreach (var kvp in desiredSettings)
                    {
                        if (trimmed.StartsWith(kvp.Key, StringComparison.OrdinalIgnoreCase) && trimmed.Contains('='))
                        {
                            lines[i] = $"{kvp.Key} = {kvp.Value.value}";
                            handledKeys.Add(kvp.Key);
                            break;
                        }
                    }
                }

                // Ajout des clés manquantes à la fin si nécessaire
                foreach (var kvp in desiredSettings)
                {
                    if (!handledKeys.Contains(kvp.Key))
                    {
                        lines.Add("");
                        lines.Add($"[{kvp.Value.section}]");
                        lines.Add($"{kvp.Key} = {kvp.Value.value}");
                    }
                }

                File.WriteAllLines(cfgPath, lines, Encoding.UTF8);
            }
            else
            {
                // Création initiale avec sections francisées / standard BepInEx
                var sb = new StringBuilder();
                sb.AppendLine("[Comfort]");
                sb.AppendLine($"EnableComfortVignette = {comfortVignette.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine("[Motion Control]");
                sb.AppendLine($"EnableBowAimSmoothing = {aimSmoothing.ToString().ToLower()}");
                sb.AppendLine();
                sb.AppendLine("[Graphics]");
                sb.AppendLine($"PhysicsSyncEnabled = {physicsSync.ToString().ToLower()}");
                sb.AppendLine($"VRTargetPhysicsHz = {targetPhysicsHz}");
                sb.AppendLine($"ShadowOptimizationEnabled = {shadowOpt.ToString().ToLower()}");
                sb.AppendLine($"VRSharpeningEnabled = {vrSharpening.ToString().ToLower()}");
                sb.AppendLine($"VRAntiGlareBloomEnabled = {vrBloom.ToString().ToLower()}");
                sb.AppendLine($"UseAmplifyOcclusion = {amplifyOcclusion.ToString().ToLower()}");
                sb.AppendLine($"BuildingPieceDetailReductionFactor = {buildingLOD}");
                sb.AppendLine();
                sb.AppendLine("[General]");
                sb.AppendLine($"MemoryCleanupEnabled = {memoryCleanup.ToString().ToLower()}");

                // Bloc d'optimisation Quest 3 si activé
                if (quest3Optimize)
                {
                    sb.AppendLine();
                    sb.AppendLine("[Quest3]");
                    sb.AppendLine("Quest3StreamingEnabled = true");
                    sb.AppendLine("Quest3RefreshRate = 120");
                    sb.AppendLine("Quest3VideoCodec = h265");
                    sb.AppendLine("Quest3Bitrate = 150");
                    sb.AppendLine("Quest3SupersamplingRatio = 1.3");
                }

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

                // Nettoyage raccourcis Bureau
                try
                {
                    string desktop = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);
                    string[] shortcuts = { "Valheim VR (FR).url", "Valheim VR (Français).url" };
                    foreach (var sc in shortcuts)
                    {
                        string p = Path.Combine(desktop, sc);
                        if (File.Exists(p)) { File.Delete(p); Log($"[OK] Raccourci Bureau '{sc}' retiré."); }
                    }
                }
                catch { }

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