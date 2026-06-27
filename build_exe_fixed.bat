@echo off
:: Batch-Datei zum Erstellen einer EXE-Datei aus dem Dragon Fire Spiel
:: Benötigt PyInstaller und Pygame

:: Überprüfen, ob Python installiert ist
python --version >nul 2>&1
if errorlevel 1 (
    echo Python ist nicht installiert!
    echo Bitte installieren Sie Python von https://www.python.org/downloads/
    pause
    exit /b 1
)

:: SSL-Zertifikatsproblem umgehen (für Pygame Installation)
set PYTHONHTTPSVERIFY=0

:: Pygame installieren (mit SSL-Verifizierung deaktiviert)
echo Pygame wird installiert (dies kann einige Minuten dauern)...
pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org --trusted-host www.libsdl.org pygame

:: PyInstaller installieren
echo PyInstaller wird installiert...
pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org pyinstaller

:: Icon erstellen, falls nicht vorhanden
if not exist dragon_icon.png (
    echo Icon wird erstellt...
    python dragon_icon.py
)

echo.
echo ============================================
echo Dragon Fire - EXE wird erstellt...
echo ============================================
echo.

:: EXE erstellen mit Icon
pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py

:: Überprüfen, ob die EXE erstellt wurde
if exist dist\DragonFire.exe (
    echo.
    echo ============================================
    echo EXE erfolgreich erstellt!
    echo Die Datei befindet sich im Ordner: dist\DragonFire.exe
    echo ============================================
    echo.
    echo Sie können die EXE-Datei jetzt ausführen oder weitergeben.
    echo.
    pause
) else (
    echo.
    echo ============================================
    echo Fehler beim Erstellen der EXE!
    echo ============================================
    echo.
    echo Mögliche Lösungen:
    echo 1. Stellen Sie sicher, dass Sie eine stabile Internetverbindung haben
    echo 2. Versuchen Sie: pip install --upgrade pip
    echo 3. Installieren Sie PyInstaller manuell: pip install pyinstaller
    echo 4. Installieren Sie Pygame manuell: pip install pygame
    echo 5. Verwenden Sie Python 3.11 oder 3.12 (neuere Versionen können Probleme haben)
    echo.
    pause
)
