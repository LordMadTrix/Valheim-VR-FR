@echo off
title Desinstallation de Valheim VR (VHVR)
chcp 65001 >nul
cd /d "%~dp0"

powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Desinstaller-ValheimVR-FR.ps1"

exit /b %ERRORLEVEL%
