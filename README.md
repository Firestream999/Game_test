# Dragon Fire - Pixel Art Game

Ein einfaches Pixel-Art-Spiel, in dem du einen Drachen steuerst, der mit so wenigen Schüssen wie möglich alle Häuser abfackeln muss.

## 🎮 Spielbeschreibung

- **Steuerung:**
  - Pfeiltasten oder WASD: Drachen bewegen
  - Leertaste: Feuer speien
  - R: Level neu starten
  - ESC: Zum Menü zurückkehren

- **Ziel:** Alle Häuser in jedem Level mit möglichst wenigen Schüssen verbrennen

- **Features:**
  - 3 verschiedene Level mit steigendem Schwierigkeitsgrad
  - Pixel-Art-Grafik
  - Highscore-System für jedes Level
  - Einfache Steuerung

## 📥 Installation

### Voraussetzungen
- **Python 3.11 oder 3.12** (empfohlen für beste Kompatibilität)
- Pygame
- PyInstaller (für EXE-Erstellung)

### Installation der Abhängigkeiten

#### Standard-Installation:
```bash
pip install pygame pyinstaller
```

#### Falls SSL-Fehler auftreten (Windows):
```bash
# SSL-Verifizierung temporär deaktivieren
set PYTHONHTTPSVERIFY=0
pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org pygame pyinstaller
```

## 🚀 Spiel starten

### Als Python-Skript
```bash
python dragon_game.py
```

### Als EXE-Datei (Windows)

#### Methode 1: Einfache Batch-Datei
1. Doppelklick auf `build_exe_simple.bat`
2. Die EXE-Datei wird im `dist` Ordner erstellt: `dist/DragonFire.exe`

#### Methode 2: Manuelle EXE-Erstellung
```bash
# 1. Pygame installieren
pip install pygame

# 2. PyInstaller installieren  
pip install pyinstaller

# 3. EXE erstellen
pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py
```

Die EXE-Datei findet sich dann im `dist/DragonFire.exe`

## 📖 Detaillierte Windows-Anleitung

Falls Sie Probleme haben, lesen Sie bitte die **[Windows Installationsanleitung](WINDOWS_GUIDE.md)** für:
- Lösung für SSL-Zertifikatsfehler
- Alternative Installationsmethoden
- Problembehebung für häufige Fehler
- Schritt-für-Schritt Anleitung

## 🎯 Level

- **Level 1:** 3 Häuser - Einfach
- **Level 2:** 5 Häuser - Mittel
- **Level 3:** 7 Häuser - Schwer
- **Level 4+:** Zufällige Level mit steigender Anzahl an Häusern

## 💡 Tipps

- Versuche, mehrere Häuser mit einem Schuss zu treffen
- Bewege den Drachen strategisch, um bessere Winkel zu bekommen
- Nutze die Level-Grenzen zu deinem Vorteil

## 🔧 Technische Details

- **Sprachversion:** Python 3.11+ (empfohlen)
- **Bibliothek:** Pygame 2.5+
- **Auflösung:** 800x600 Pixel
- **Pixel-Art-Skalierung:** 4x (jeder Pixel = 4x4 Bildschirmpixel)

## 📜 Dateien im Projekt

- `dragon_game.py` - Hauptspiel
- `dragon_icon.py` - Skript zum Erstellen des Icons
- `dragon_icon.png` - Spiel-Icon
- `build_exe.bat` - Batch-Datei für EXE-Erstellung
- `build_exe_simple.bat` - Einfachere Batch-Datei
- `build_exe_fixed.bat` - Batch-Datei mit SSL-Fix
- `requirements.txt` - Python Abhängigkeiten
- `WINDOWS_GUIDE.md` - Detaillierte Windows-Anleitung

## 📜 Lizenz

Dieses Spiel ist Open Source und kann frei verwendet, modifiziert und weitergegeben werden.

---

## ❓ Häufige Probleme und Lösungen

### Problem: SSL-Zertifikatsfehler
**Lösung:** Verwenden Sie `build_exe_fixed.bat` oder lesen Sie `WINDOWS_GUIDE.md`

### Problem: PyInstaller nicht gefunden
**Lösung:** `pip install pyinstaller`

### Problem: Pygame nicht gefunden
**Lösung:** `pip install pygame`

### Problem: Microsoft Visual C++ fehlend
**Lösung:** Installieren Sie [Microsoft Visual C++ Build Tools](https://visualstudio.microsoft.com/visual-cpp-build-tools/)

---

Viel Spaß beim Spielen! 🐉🔥
