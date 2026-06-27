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

## 📥 Installation & Kompilierung

### Voraussetzungen
- **Windows 7/8/10/11**
- **.NET 6.0 SDK** (wird automatisch installiert, falls nicht vorhanden)

### Methode 1: Einfache Kompilierung (Empfohlen)
1. Doppelklick auf `build_exe.bat`
2. Die EXE-Datei wird im Ordner `bin/Release/net6.0-windows/DragonFire.exe` erstellt

### Methode 2: Manuelle Kompilierung
1. Öffnen Sie die Eingabeaufforderung (cmd) in diesem Ordner
2. Führen Sie aus:
   ```cmd
   dotnet build DragonFire.csproj -c Release -o bin/Release
   ```
3. Die EXE-Datei findet sich in: `bin/Release/net6.0-windows/DragonFire.exe`

### Methode 3: Direktes Ausführen (ohne EXE)
1. Öffnen Sie die Eingabeaufforderung (cmd) in diesem Ordner
2. Führen Sie aus:
   ```cmd
   dotnet run
   ```

## 🚀 Schnellstart

### Wenn .NET 6.0 bereits installiert ist:
```cmd
:: 1. Projekt erstellen
dotnet build DragonFire.csproj -c Release

:: 2. EXE ausführen
start bin/Release/net6.0-windows/DragonFire.exe
```

### Wenn .NET 6.0 nicht installiert ist:
1. Doppelklick auf `build_exe.bat`
2. Das Skript installiert .NET 6.0 automatisch
3. Die EXE wird erstellt und kann ausgeführt werden

## 📁 Dateistruktur

- `Program.cs` - Hauptspielcode
- `DragonFire.csproj` - Projektdatei
- `build_exe.bat` - Batch-Datei zur EXE-Erstellung
- `README.md` - Diese Anleitung

## 🎯 Level

- **Level 1:** 3 Häuser - Einfach
- **Level 2:** 5 Häuser - Mittel
- **Level 3:** 7 Häuser - Schwer
- **Level 4+:** Zufällige Level mit steigender Anzahl an Häusern

## 💡 Tipps

- Versuche, mehrere Häuser mit einem Schuss zu treffen
- Bewege den Drachen strategisch, um bessere Winkel zu bekommen
- Nutze die Level-Grenzen zu deinem Vorteil

## 🔧 Problembehebung

### Problem: .NET 6.0 SDK nicht gefunden
**Lösung:**
1. Installieren Sie .NET 6.0 SDK manuell von: [https://dotnet.microsoft.com/download/dotnet/6.0](https://dotnet.microsoft.com/download/dotnet/6.0)
2. Wählen Sie "Desktop development" während der Installation
3. Starten Sie Ihren PC neu

### Problem: Kompilierungsfehler
**Lösung:**
1. Stellen Sie sicher, dass Sie die neueste Version von .NET 6.0 SDK haben
2. Führen Sie `dotnet --list-sdks` aus, um installierte SDKs anzuzeigen
3. Installieren Sie das fehlende SDK

### Problem: EXE-Datei wird nicht erstellt
**Lösung:**
1. Überprüfen Sie, ob der Ordner `bin/Release/net6.0-windows/` existiert
2. Falls nicht, führen Sie `dotnet build DragonFire.csproj -c Release` manuell aus
3. Überprüfen Sie die Fehlermeldungen

## 📜 Technische Details

- **Sprache:** C# 10.0
- **Framework:** .NET 6.0 Windows Forms
- **Auflösung:** 800x600 Pixel
- **Pixel-Art-Skalierung:** 4x (jeder Pixel = 4x4 Bildschirmpixel)
- **Zielplattform:** Windows x86/x64

## 📜 Lizenz

Dieses Spiel ist Open Source und kann frei verwendet, modifiziert und weitergegeben werden.

---

Viel Spaß beim Spielen! 🐉🔥
