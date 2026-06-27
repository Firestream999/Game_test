@echo off
:: Einfache Batch-Datei zum Erstellen einer EXE-Datei
:: Diese Version verwendet eine vorinstallierte Pygame-Version

:: Überprüfen, ob Python installiert ist
python --version >nul 2>&1
if errorlevel 1 (
    echo Python ist nicht installiert!
    echo Bitte installieren Sie Python von https://www.python.org/downloads/
    echo Empfohlen: Python 3.11 oder 3.12
    pause
    exit /b 1
)

:: Überprüfen, ob PyInstaller installiert ist
pip show pyinstaller >nul 2>&1
if errorlevel 1 (
    echo PyInstaller wird installiert...
    pip install pyinstaller
)

:: Überprüfen, ob Pygame installiert ist
pip show pygame >nul 2>&1
if errorlevel 1 (
    echo Pygame wird installiert (dies kann einige Minuten dauern)...
    echo Falls Fehler auftreten, versuchen Sie:
    echo   pip install pygame --pre
    echo oder
    echo   python -m pip install pygame --user
    pip install pygame
)

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
    echo Alternative Methode:
    echo 1. Installieren Sie PyInstaller manuell: pip install pyinstaller
    echo 2. Führen Sie dann aus: pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py
    echo.
    pause
)
