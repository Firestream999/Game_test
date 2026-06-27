@echo off
:: Batch-Datei zum Kompilieren des Dragon Fire Spiels in C#
:: Benötigt .NET 6.0 SDK

setlocal enabledelayedexpansion

echo ============================================
echo Dragon Fire - C# Version wird kompiliert...
echo ============================================
echo.

:: Überprüfen, ob .NET 6.0 SDK installiert ist
dotnet --list-sdks 2>nul | findstr "6.0" >nul
if errorlevel 1 (
    echo .NET 6.0 SDK wird überprüft...
    
    :: Versuchen, .NET 6.0 SDK zu installieren
    echo Installation von .NET 6.0 SDK...
    echo Dies kann einige Minuten dauern...
    echo.
    
    powershell -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1' -UseBasicParsing; .\dotnet-install.ps1 -Channel 6.0 -InstallDir '%%ProgramFiles%%\dotnet' -NoPath" 2>nul
    
    :: Überprüfen, ob Installation erfolgreich war
    timeout /t 5 >nul
    where dotnet >nul 2>&1
    if errorlevel 1 (
        echo.
        echo Fehler: .NET 6.0 SDK konnte nicht automatisch installiert werden.
        echo.
        echo Bitte installieren Sie .NET 6.0 SDK manuell von:
        echo https://dotnet.microsoft.com/download/dotnet/6.0
        echo.
        echo Nach der Installation führen Sie diese Batch-Datei erneut aus.
        pause
        exit /b 1
    )
    
    echo .NET 6.0 SDK wurde erfolgreich installiert!
    echo.
)

echo .NET 6.0 SDK gefunden!
echo.

:: Projekt erstellen
echo Projekt wird erstellt...
echo.

:: Erstelle den Release-Ordner
dotnet publish DragonFire.csproj -c Release -o bin/Release/net6.0-windows --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=true

if errorlevel 1 (
    echo.
    echo Fehler beim Erstellen des Projekts!
    echo.
    echo Versuche alternative Methode...
    
    :: Alternative Methode ohne Single File
    dotnet build DragonFire.csproj -c Release -o bin/Release
    
    if errorlevel 1 (
        echo.
        echo Fehler: Projekt konnte nicht erstellt werden.
        echo.
        echo Mögliche Lösungen:
        echo 1. Stellen Sie sicher, dass .NET 6.0 SDK installiert ist
        echo 2. Führen Sie: dotnet --list-sdks aus, um installierte SDKs anzuzeigen
        echo 3. Installieren Sie .NET 6.0 SDK von: https://dotnet.microsoft.com/download/dotnet/6.0
        echo.
        pause
        exit /b 1
    )
)

echo.
echo ============================================
echo EXE erfolgreich erstellt!
echo ============================================
echo.

:: Pfad zur EXE-Datei
set EXE_PATH=bin\Release\net6.0-windows\DragonFire.exe

if exist "%EXE_PATH%" (
    echo Die EXE-Datei wurde erstellt in:
    echo.
    echo   %~dp0%EXE_PATH%
    echo.
    echo = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = = =
    echo.
    
    :: Datei-Explorer öffnen und EXE anzeigen
    explorer /select,"%~dp0%EXE_PATH%"
    
    echo.
    echo Möchten Sie die EXE-Datei jetzt starten? (J/N)
    set /p choice=
    if /i "%choice%"=="J" (
        start "" "%~dp0%EXE_PATH%"
    )
) else (
    echo.
    echo EXE-Datei wurde nicht gefunden!
    echo.
    echo Mögliche Pfade:
    echo   - bin\Release\net6.0-windows\DragonFire.exe
    echo   - bin\Release\net6.0\win-x64\publish\DragonFire.exe
    echo   - bin\Release\net6.0\win-x86\publish\DragonFire.exe
    echo.
    echo Bitte überprüfen Sie diese Ordner.
    
    :: Zeige alle Dateien im Release-Ordner an
    if exist bin\Release\ (
        echo.
        echo Inhalte des Release-Ordners:
        dir bin\Release\ /s /b
    )
)

echo.
pause
