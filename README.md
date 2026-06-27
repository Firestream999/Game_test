# Dragon Fire - Pixel Art Game

Ein einfaches Pixel-Art-Spiel, in dem du einen Drachen steuerst, der mit so wenigen Schüssen wie möglich alle Häuser abfackeln muss.

## Spielbeschreibung

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

## Installation

### Voraussetzungen
- Python 3.6 oder höher
- Pygame

### Installation der Abhängigkeiten
```bash
pip install pygame
```

## Spiel starten

### Als Python-Skript
```bash
python dragon_game.py
```

### Als EXE-Datei (Windows)
1. Führe die Batch-Datei aus:
   ```bash
   build_exe.bat
   ```
2. Die EXE-Datei wird im `dist` Ordner erstellt: `dist/DragonFire.exe`

## Manuelle EXE-Erstellung

Falls die Batch-Datei nicht funktioniert, kannst du PyInstaller manuell verwenden:

```bash
# PyInstaller installieren
pip install pyinstaller

# EXE erstellen
pyinstaller --onefile --windowed --name "DragonFire" dragon_game.py
```

Die EXE-Datei findet sich dann im `dist` Ordner.

## Level

- **Level 1:** 3 Häuser - Einfach
- **Level 2:** 5 Häuser - Mittel
- **Level 3:** 7 Häuser - Schwer
- **Level 4+:** Zufällige Level mit steigender Anzahl an Häusern

## Tipps

- Versuche, mehrere Häuser mit einem Schuss zu treffen
- Bewege den Drachen strategisch, um bessere Winkel zu bekommen
- Nutze die Level-Grenzen, um Feuer zu reflektieren (in zukünftigen Versionen)

## Technische Details

- **Sprachversion:** Python 3
- **Bibliothek:** Pygame
- **Auflösung:** 800x600 Pixel
- **Pixel-Art-Skalierung:** 4x (jeder Pixel = 4x4 Bildschirmpixel)

## Lizenz

Dieses Spiel ist Open Source und kann frei verwendet, modifiziert und weitergegeben werden.

---

Viel Spaß beim Spielen! 🐉🔥
