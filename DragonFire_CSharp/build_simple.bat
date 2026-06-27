@echo off
:: EINFACHE Batch-Datei zum Kompilieren des Dragon Fire Spiels
:: Diese Version erstellt die EXE im aktuellen Ordner

setlocal

echo Dragon Fire - Kompiliere Spiel...
echo =================================
echo.

:: Überprüfen, ob dotnet verfügbar ist
where dotnet >nul 2>&1
if errorlevel 1 (
    echo .NET SDK nicht gefunden!
    echo Bitte installieren Sie .NET 6.0 SDK von:
    echo https://dotnet.microsoft.com/download/dotnet/6.0
    echo.
    echo Nach der Installation führen Sie diese Datei erneut aus.
    pause
    exit /b 1
)

echo .NET SDK gefunden - starte Kompilierung...
echo.

:: Projekt erstellen (dies erstellt die EXE in bin/Release/net6.0-windows/)
dotnet publish -c Release -o .\publish --self-contained true -p:PublishSingleFile=true -p:PublishTrimmed=false

if errorlevel 1 (
    echo.
    echo Fehler bei der Kompilierung!
    echo Versuche alternative Methode...
    
    :: Alternative: Erstelle normale EXE
    dotnet build -c Release
    
    if errorlevel 1 (
        echo.
        echo Fehler: Kompilierung fehlgeschlagen
        echo Bitte überprüfen Sie, ob .NET 6.0 SDK installiert ist
        pause
        exit /b 1
    )
    
    :: Kopiere die EXE in den aktuellen Ordner
    if exist bin\Release\net6.0-windows\DragonFire.exe (
        copy bin\Release\net6.0-windows\DragonFire.exe .\DragonFire.exe >nul
        echo EXE wurde in den aktuellen Ordner kopiert!
    )
) else (
    echo.
    echo ============================================
    echo EXE ERFOLGREICH ERSTELLT!
    echo ============================================
)

echo.

:: Überprüfen, wo die EXE ist
if exist .\publish\DragonFire.exe (
    echo Die EXE-Datei befindet sich in: .\publish\DragonFire.exe
    echo.
    explorer /select,".\publish\DragonFire.exe"
    
) else if exist .\DragonFire.exe (
    echo Die EXE-Datei befindet sich in: .\DragonFire.exe
    echo.
    explorer /select,".\DragonFire.exe"
    
) else if exist bin\Release\net6.0-windows\DragonFire.exe (
    echo Die EXE-Datei befindet sich in: bin\Release\net6.0-windows\DragonFire.exe
    echo.
    explorer /select,"bin\Release\net6.0-windows\DragonFire.exe"
    
) else (
    echo EXE-Datei wurde nicht gefunden!
    echo Bitte suchen Sie nach "DragonFire.exe" in diesem Ordner und seinen Unterordnern.
)

echo.
echo Möchten Sie die EXE jetzt starten? (J/N)
set /p choice=
if /i "%choice%"=="J" (
    if exist .\publish\DragonFire.exe (
        start .\publish\DragonFire.exe
    ) else if exist .\DragonFire.exe (
        start .\DragonFire.exe
    ) else if exist bin\Release\net6.0-windows\DragonFire.exe (
        start bin\Release\net6.0-windows\DragonFire.exe
    )
)

echo.
pause
