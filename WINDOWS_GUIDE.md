# Windows Installationsanleitung für Dragon Fire

## Problembehebung für SSL-Zertifikatsfehler

Der Fehler `SSL: CERTIFICATE_VERIFY_FAILED` tritt auf, weil das Zertifikat für die SDL2-Bibliothek abgelaufen ist. Hier sind mehrere Lösungen:

## 🔧 Lösung 1: Einfache Installation (Empfohlen)

### Schritt 1: Python installieren
1. Laden Sie **Python 3.11 oder 3.12** von [python.org](https://www.python.org/downloads/) herunter
2. **Wichtig:** Wählen Sie während der Installation "Add Python to PATH" aus
3. Installieren Sie Python

### Schritt 2: Pygame installieren
Öffnen Sie die Eingabeaufforderung (cmd) und führen Sie aus:

```cmd
python -m pip install --upgrade pip
python -m pip install pygame
```

Falls Fehler auftreten, versuchen Sie:
```cmd
python -m pip install pygame --pre
```

### Schritt 3: PyInstaller installieren
```cmd
python -m pip install pyinstaller
```

### Schritt 4: EXE erstellen
```cmd
pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py
```

Die EXE-Datei findet sich dann im Ordner `dist/DragonFire.exe`

---

## 🔧 Lösung 2: SSL-Verifizierung umgehen (falls Lösung 1 nicht funktioniert)

Führen Sie diese Befehle in der Eingabeaufforderung aus:

```cmd
set PYTHONHTTPSVERIFY=0
python -m pip install --trusted-host pypi.org --trusted-host files.pythonhosted.org pygame
python -m pip install pyinstaller
pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py
```

---

## 🔧 Lösung 3: Vorinstallierte Pygame-Version verwenden

Manche Python-Installationen haben bereits Pygame vorinstalliert. Versuchen Sie:

```cmd
python -c "import pygame; print('Pygame ist bereits installiert')"
```

Falls Pygame installiert ist, können Sie direkt PyInstaller installieren:

```cmd
python -m pip install pyinstaller
pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py
```

---

## 🔧 Lösung 4: Alternative Python-Version

Manche Python-Versionen (besonders 3.14+) haben Probleme mit Pygame. Installieren Sie stattdessen:

1. **Python 3.11.8** oder **Python 3.12.3** herunterladen
2. Installieren mit "Add Python to PATH"
3. Dann Pygame installieren:

```cmd
python -m pip install pygame==2.5.2
python -m pip install pyinstaller
pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py
```

---

## 🔧 Lösung 5: Manuelle Installation der SDL2-Bibliotheken

1. Laden Sie die SDL2-Dateien manuell herunter:
   - [SDL2-devel-2.28.4-VC.zip](https://www.libsdl.org/release/SDL2-devel-2.28.4-VC.zip)
   - Entpacken Sie die Datei
   - Kopieren Sie die DLL-Dateien in den Python-Ordner

2. Dann installieren Sie Pygame:
```cmd
python -m pip install pygame --global-option=build_ext --global-option="-Ipfad/zur/SDL2/include" --global-option="-Lpfad/zur/SDL2/lib"
```

---

## 🎮 Spiel ohne EXE starten

Falls die EXE-Erstellung nicht funktioniert, können Sie das Spiel direkt starten:

```cmd
python dragon_game.py
```

---

## 📋 Häufige Fehler und Lösungen

### Fehler: "pyinstaller nicht gefunden"
**Lösung:**
```cmd
python -m pip install pyinstaller
```

### Fehler: "pygame nicht gefunden"
**Lösung:**
```cmd
python -m pip install pygame
```

### Fehler: "SSL: CERTIFICATE_VERIFY_FAILED"
**Lösung:**
- Verwenden Sie Lösung 2 (SSL-Verifizierung umgehen)
- Oder installieren Sie Python 3.11/3.12

### Fehler: "Microsoft Visual C++ 14.0 oder höher erforderlich"
**Lösung:**
- Installieren Sie [Microsoft Visual C++ Build Tools](https://visualstudio.microsoft.com/visual-cpp-build-tools/)
- Wählen Sie "Desktop development with C++" während der Installation

---

## 📞 Support

Falls Sie weiterhin Probleme haben:
1. Überprüfen Sie Ihre Python-Version: `python --version`
2. Überprüfen Sie Ihre pip-Version: `python -m pip --version`
3. Versuchen Sie, alle Pakete zu aktualisieren: `python -m pip install --upgrade pip setuptools wheel`
4. Starten Sie Ihren Computer neu

---

## 🎯 Schnellstart (Zusammenfassung)

```cmd
:: 1. Python 3.11 oder 3.12 installieren (mit PATH)
:: 2. Pygame installieren
python -m pip install pygame

:: 3. PyInstaller installieren
python -m pip install pyinstaller

:: 4. EXE erstellen
pyinstaller --onefile --windowed --name "DragonFire" --icon=dragon_icon.png dragon_game.py

:: 5. EXE finden
cd dist
DragonFire.exe
```

Viel Erfolg! 🐉🔥
