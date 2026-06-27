@echo off
:: Batch-Datei zum Erstellen einer EXE-Datei aus dem Dragon Fire Spiel
:: Benötigt PyInstaller (wird automatisch installiert, falls nicht vorhanden)

:: Überprüfen, ob Python installiert ist
python --version >nul 2>&1
if errorlevel 1 (
    echo Python ist nicht installiert!
    echo Bitte installieren Sie Python von https://www.python.org/downloads/
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
    echo Pygame wird installiert...
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
    pause
)
