@echo off
:: Batch-Datei zum Kompilieren des Dragon Fire Spiels in C#
:: Benötigt .NET 6.0 SDK (wird automatisch installiert, falls nicht vorhanden)

echo ============================================
echo Dragon Fire - C# Version wird kompiliert...
echo ============================================
echo.

:: Überprüfen, ob .NET 6.0 SDK installiert ist
dotnet --list-sdks 2>nul | findstr "6.0" >nul
if errorlevel 1 (
    echo .NET 6.0 SDK wird installiert...
    echo Dies kann einige Minuten dauern...
    echo.
    
    :: Versuchen, .NET 6.0 SDK zu installieren
    powershell -Command "[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -Uri 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'; .\dotnet-install.ps1 -Channel 6.0 -InstallDir 'C:\Program Files\dotnet'" >nul 2>&1
    
    :: Überprüfen, ob Installation erfolgreich war
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
)

echo .NET 6.0 SDK gefunden!
echo.

:: Projekt erstellen
echo Projekt wird erstellt...
dotnet build DragonFire.csproj -c Release -o bin/Release

if errorlevel 1 (
    echo.
    echo Fehler beim Erstellen des Projekts!
    echo.
    echo Mögliche Lösungen:
    echo 1. Stellen Sie sicher, dass .NET 6.0 SDK installiert ist
    echo 2. Führen Sie: dotnet --list-sdks aus, um installierte SDKs anzuzeigen
    echo 3. Installieren Sie .NET 6.0 SDK von: https://dotnet.microsoft.com/download/dotnet/6.0
    echo.
    pause
    exit /b 1
)

echo.
echo ============================================
echo EXE erfolgreich erstellt!
echo ============================================
echo.

:: EXE-Datei finden und anzeigen
if exist bin\Release\net6.0-windows\DragonFire.exe (
    echo Die EXE-Datei befindet sich in: bin\Release\net6.0-windows\DragonFire.exe
    echo.
    echo Sie können die EXE-Datei jetzt ausführen oder weitergeben.
    echo.
    echo Möchten Sie die EXE-Datei jetzt starten? (J/N)
    set /p choice=
    if /i "%choice%"=="J" (
        start bin\Release\net6.0-windows\DragonFire.exe
    )
) else (
    echo.
    echo EXE-Datei wurde nicht gefunden!
    echo.
    echo Bitte überprüfen Sie den Ordner: bin\Release\net6.0-windows\
)

echo.
pause
