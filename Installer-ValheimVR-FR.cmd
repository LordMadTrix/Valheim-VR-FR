@echo off
title Installation de Valheim VR (VHVR) - Francais
chcp 65001 >nul
cd /d "%~dp0"

echo ====================================================================
echo           LANCEMENT DE L'INSTALLATEUR VALHEIM VR (FR)
echo ====================================================================
echo.

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Installer-ValheimVR-FR.ps1"

exit /b %ERRORLEVEL%
