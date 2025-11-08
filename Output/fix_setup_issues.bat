@echo off
:: ---------------------------
:: ByeByeDPI Setup Helper
:: ---------------------------

echo ----------------------------------------
echo ByeByeDPI Setup Helper
echo ----------------------------------------
echo.

set "SCRIPT_DIR=%~dp0"

>nul 2>&1 "%SYSTEMROOT%\system32\cacls.exe" "%SYSTEMROOT%\system32\config\system"
if '%errorlevel%' NEQ '0' (
    echo Please run this script as Administrator!
    pause
    exit /b
)

echo Please extract all files from the .zip archive to a regular folder.
echo Do NOT run the installer directly from inside the compressed archive.
pause

echo If installation fails with an error about WinDivert64.sys being in use, stop the driver first with using cmd (as an Admin): sc stop WinDivert
echo (This script will do this step automaticly for you)
pause

if exist "%SCRIPT_DIR%setup.exe" (
    echo Checking WinDivert driver...
    
    sc query WinDivert | find "RUNNING" >nul
    if %errorlevel%==0 (
        echo WinDivert driver is currently running. Stopping it first...
        sc stop WinDivert
        timeout /t 2 >nul
    )

    echo Starting ByeByeDPI installer...
    start "" "%SCRIPT_DIR%setup.exe"
) else (
    echo Error: setup.exe not found!
    pause
    exit /b
)

    echo NOTE: If you see the message:
    echo "You cannot start application ByeByeDPI from this location because it is already installed from a different location."
    echo It means the application is already installed from another folder.
    echo You can either:
    echo   1. Start it from the original installation location.
    echo   2. Uninstall the old version and reinstall from this folder.
    pause

echo.
echo Installation steps completed.
pause
