# Dragon Fire - C# Version

Ein Pixel-Art-Drachenspiel in C# mit Windows Forms. Funktioniert auf jedem Windows-PC ohne Python-Probleme!

## 🎮 Spielbeschreibung

- **Steuerung:**
  - Pfeiltasten oder WASD: Drachen bewegen
  - Leertaste: Feuer speien
  - R: Level neu starten
  - ESC: Zum Menü zurückkehren
  - ENTER: Level starten / bestätigen
  - 1/2/3: Level auswählen (im Menü)
  - M: Zum Menü (nach Level abschließen)

- **Ziel:** Alle Häuser in jedem Level mit möglichst wenigen Schüssen verbrennen

- **Features:**
  - 3 verschiedene Level mit steigendem Schwierigkeitsgrad
  - Pixel-Art-Grafik (4x Skalierung)
  - Highscore-System für jedes Level
  - Einfache Steuerung

## 🚀 Schnellstart (3 Schritte!)

### 1️⃣ Doppelklick auf `build_simple.bat`
- Installiert automatisch .NET 6.0 (falls nicht vorhanden)
- Kompiliert das Spiel

### 2️⃣ Warten bis "EXE ERFOLGREICH ERSTELLT!" erscheint

### 3️⃣ EXE finden und starten
Die EXE-Datei befindet sich in einem dieser Ordner:
- **`./publish/DragonFire.exe`** (bevorzugt)
- **`./DragonFire.exe`** (falls kopiert)
- **`bin/Release/net6.0-windows/DragonFire.exe`** (alternativ)

---

## 📁 Wo ist die EXE-Datei?

Nach erfolgreicher Kompilierung:

| Methode | EXE-Pfad | Beschreibung |
|---------|----------|--------------|
| `build_simple.bat` | `./publish/DragonFire.exe` | Selbstständige EXE (empfohlen) |
| `build_exe.bat` | `bin/Release/net6.0-windows/DragonFire.exe` | Standard-EXE |
| Manuell | `bin/Release/net6.0-windows/DragonFire.exe` | Nach `dotnet build` |

**💡 Tipp:** Die Batch-Dateien öffnen automatisch den Datei-Explorer und zeigen die EXE an!

---

## 📥 Alternative Installationsmethoden

### Methode 1: Mit `build_simple.bat` (empfohlen)
```cmd
:: Einfach Doppelklick auf build_simple.bat
:: Die EXE wird in ./publish/ erstellt
```

### Methode 2: Mit `build_exe.bat`
```cmd
:: Doppelklick auf build_exe.bat
:: Die EXE wird in bin/Release/net6.0-windows/ erstellt
```

### Methode 3: Manuell mit dotnet CLI
```cmd
:: 1. .NET 6.0 SDK installieren (falls nicht vorhanden)
::    https://dotnet.microsoft.com/download/dotnet/6.0

:: 2. Projekt erstellen
cd DragonFire_CSharp
dotnet publish -c Release -o .\publish --self-contained true

:: 3. EXE starten
start .\publish\DragonFire.exe
```

### Methode 4: Direktes Ausführen (ohne EXE)
```cmd
cd DragonFire_CSharp
dotnet run
```

---

## 🎯 Level

- **Level 1:** 3 Häuser - Einfach
- **Level 2:** 5 Häuser - Mittel
- **Level 3:** 7 Häuser - Schwer
- **Level 4+:** Zufällige Level mit steigender Anzahl an Häusern

---

## 💡 Tipps

- Versuche, mehrere Häuser mit einem Schuss zu treffen
- Bewege den Drachen strategisch, um bessere Winkel zu bekommen
- Nutze die Level-Grenzen zu deinem Vorteil

---

## 🔧 Problembehebung

### ❌ Problem: ".NET SDK nicht gefunden"
**✅ Lösung:**
1. Installieren Sie .NET 6.0 SDK von: [https://dotnet.microsoft.com/download/dotnet/6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Wählen Sie "Desktop development" während der Installation
3. Starten Sie Ihren PC neu
4. Führen Sie die Batch-Datei erneut aus

### ❌ Problem: "Kompilierung fehlgeschlagen"
**✅ Lösung:**
1. Führen Sie `dotnet --list-sdks` aus, um installierte SDKs anzuzeigen
2. Installieren Sie .NET 6.0 SDK, falls nicht vorhanden
3. Stellen Sie sicher, dass Sie die neueste Version haben

### ❌ Problem: "EXE-Datei nicht gefunden"
**✅ Lösung:**
1. Suchen Sie nach `DragonFire.exe` in allen Unterordnern
2. Die EXE könnte in einem dieser Pfade sein:
   - `./publish/DragonFire.exe`
   - `./DragonFire.exe`
   - `bin/Release/net6.0-windows/DragonFire.exe`
   - `bin/Release/net6.0/win-x64/publish/DragonFire.exe`

### ❌ Problem: "Die Batch-Datei funktioniert nicht"
**✅ Lösung:**
1. Öffnen Sie die Eingabeaufforderung (cmd) manuell
2. Navigieren Sie zum DragonFire_CSharp Ordner
3. Führen Sie die Befehle aus der Batch-Datei manuell aus

---

## 📜 Technische Details

- **Sprache:** C# 10.0
- **Framework:** .NET 6.0 Windows Forms
- **Auflösung:** 800x600 Pixel
- **Pixel-Art-Skalierung:** 4x (jeder Pixel = 4x4 Bildschirmpixel)
- **Zielplattform:** Windows x86/x64
- **EXE-Größe:** ~5-10 MB (selbstständig)

---

## 📁 Dateien im Projekt

```
DragonFire_CSharp/
├── Program.cs          # Hauptspielcode
├── DragonFire.csproj   # Projektdatei
├── build_simple.bat    # Einfache Batch-Datei (empfohlen)
├── build_exe.bat       # Alternative Batch-Datei
├── ANLEITUNG.txt       # Einfache Textanleitung
└── README.md           # Diese Datei
```

---

## 📜 Lizenz

Dieses Spiel ist Open Source und kann frei verwendet, modifiziert und weitergegeben werden.

---

## 🎉 Viel Spaß beim Spielen! 🐉🔥

**Falls Sie Probleme haben, lesen Sie bitte die ANLEITUNG.txt Datei!**
