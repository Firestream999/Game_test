using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DragonFire
{
    public class DragonFireGame : Form
    {
        // Spielkonstanten
        private const int PIXEL_SCALE = 4;
        private const int SCREEN_WIDTH = 800;
        private const int SCREEN_HEIGHT = 600;
        private const int GAME_WIDTH = SCREEN_WIDTH / PIXEL_SCALE;
        private const int GAME_HEIGHT = SCREEN_HEIGHT / PIXEL_SCALE;

        // Spielzustand
        private enum GameState { Menu, Playing, LevelComplete, GameOver }
        private GameState currentState = GameState.Menu;

        // Spieler (Drache)
        private Dragon dragon;

        // Level
        private Level currentLevel;
        private int currentLevelNumber = 1;

        // Eingabe
        private bool leftKeyDown = false;
        private bool rightKeyDown = false;
        private bool upKeyDown = false;
        private bool downKeyDown = false;
        private bool spaceKeyDown = false;

        // Timer für Spielschleife
        private Timer gameTimer;

        // Besten Schusszahlen
        private Dictionary<int, int> bestShots = new Dictionary<int, int>();

        // Double Buffering für flüssige Grafik
        private Bitmap doubleBuffer;
        private Graphics doubleBufferGraphics;

        public DragonFireGame()
        {
            this.Text = "Dragon Fire - Pixel Art Game";
            this.ClientSize = new Size(SCREEN_WIDTH, SCREEN_HEIGHT);
            this.DoubleBuffered = true;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            // Double Buffer erstellen
            doubleBuffer = new Bitmap(SCREEN_WIDTH, SCREEN_HEIGHT);
            doubleBufferGraphics = Graphics.FromImage(doubleBuffer);

            // Timer für Spielschleife
            gameTimer = new Timer();
            gameTimer.Interval = 16; // ~60 FPS
            gameTimer.Tick += GameLoop;
            gameTimer.Start();

            // Tastaturereignisse
            this.KeyDown += OnKeyDown;
            this.KeyUp += OnKeyUp;

            // Spiel initialisieren
            InitializeGame();
        }

        private void InitializeGame()
        {
            dragon = new Dragon(50, GAME_HEIGHT / 2);
            currentLevel = new Level(currentLevelNumber);
        }

        private void StartLevel(int levelNumber)
        {
            currentLevelNumber = levelNumber;
            dragon = new Dragon(50, GAME_HEIGHT / 2);
            currentLevel = new Level(levelNumber);
            currentState = GameState.Playing;
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    leftKeyDown = true;
                    break;
                case Keys.Right:
                case Keys.D:
                    rightKeyDown = true;
                    break;
                case Keys.Up:
                case Keys.W:
                    upKeyDown = true;
                    break;
                case Keys.Down:
                case Keys.S:
                    downKeyDown = true;
                    break;
                case Keys.Space:
                    if (!spaceKeyDown && currentState == GameState.Playing)
                    {
                        spaceKeyDown = true;
                        if (dragon.ShootFlame())
                        {
                            currentLevel.FireFlame(dragon.X + dragon.Width, dragon.Y + 2, dragon.Direction);
                        }
                    }
                    break;
                case Keys.R:
                    if (currentState == GameState.Playing)
                    {
                        StartLevel(currentLevelNumber);
                    }
                    break;
                case Keys.Enter:
                    if (currentState == GameState.Menu)
                    {
                        StartLevel(1);
                    }
                    else if (currentState == GameState.LevelComplete)
                    {
                        StartLevel(currentLevelNumber + 1);
                    }
                    else if (currentState == GameState.GameOver)
                    {
                        StartLevel(currentLevelNumber);
                    }
                    break;
                case Keys.Escape:
                    if (currentState == GameState.Playing)
                    {
                        currentState = GameState.Menu;
                    }
                    else if (currentState == GameState.Menu || currentState == GameState.LevelComplete || currentState == GameState.GameOver)
                    {
                        this.Close();
                    }
                    break;
                case Keys.D1:
                    if (currentState == GameState.Menu)
                        StartLevel(1);
                    break;
                case Keys.D2:
                    if (currentState == GameState.Menu)
                        StartLevel(2);
                    break;
                case Keys.D3:
                    if (currentState == GameState.Menu)
                        StartLevel(3);
                    break;
                case Keys.M:
                    if (currentState == GameState.LevelComplete || currentState == GameState.GameOver)
                        currentState = GameState.Menu;
                    break;
            }
        }

        private void OnKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                case Keys.A:
                    leftKeyDown = false;
                    break;
                case Keys.Right:
                case Keys.D:
                    rightKeyDown = false;
                    break;
                case Keys.Up:
                case Keys.W:
                    upKeyDown = false;
                    break;
                case Keys.Down:
                case Keys.S:
                    downKeyDown = false;
                    break;
                case Keys.Space:
                    spaceKeyDown = false;
                    break;
            }
        }

        private void GameLoop(object sender, EventArgs e)
        {
            // Eingabe verarbeiten
            HandleInput();

            // Spiel aktualisieren
            UpdateGame();

            // Bildschirm neu zeichnen
            this.Invalidate();
        }

        private void HandleInput()
        {
            if (currentState == GameState.Playing)
            {
                int dx = 0, dy = 0;
                if (leftKeyDown) dx -= 1;
                if (rightKeyDown) dx += 1;
                if (upKeyDown) dy -= 1;
                if (downKeyDown) dy += 1;
                
                dragon.Move(dx, dy);
            }
        }

        private void UpdateGame()
        {
            if (currentState == GameState.Playing)
            {
                dragon.Update();
                currentLevel.Update();

                // Kollisionen prüfen
                CheckCollisions();

                // Level abgeschlossen?
                if (currentLevel.Completed)
                {
                    // Beste Schusszahl speichern
                    if (!bestShots.ContainsKey(currentLevelNumber) || 
                        currentLevel.ShotsFired < bestShots[currentLevelNumber])
                    {
                        bestShots[currentLevelNumber] = currentLevel.ShotsFired;
                    }
                    currentState = GameState.LevelComplete;
                }
            }
        }

        private void CheckCollisions()
        {
            foreach (var projectile in currentLevel.Projectiles)
            {
                if (!projectile.Active) continue;

                foreach (var house in currentLevel.Houses)
                {
                    if (house.Burning || house.Burned) continue;

                    // Kollision prüfen
                    if (projectile.X < house.X + house.Width &&
                        projectile.X + projectile.Width > house.X &&
                        projectile.Y < house.Y + house.Height &&
                        projectile.Y + projectile.Height > house.Y)
                    {
                        house.StartBurning();
                        projectile.Active = false;
                        currentLevel.HousesBurned++;
                        break;
                    }
                }
            }

            // Projektile bereinigen
            currentLevel.Projectiles.RemoveAll(p => !p.Active);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // Double Buffer verwenden
            doubleBufferGraphics.Clear(Color.Black);

            // Je nach Zustand zeichnen
            switch (currentState)
            {
                case GameState.Menu:
                    DrawMenu(doubleBufferGraphics);
                    break;
                case GameState.Playing:
                    currentLevel.Draw(doubleBufferGraphics, dragon);
                    DrawUI(doubleBufferGraphics);
                    break;
                case GameState.LevelComplete:
                    currentLevel.Draw(doubleBufferGraphics, dragon);
                    DrawLevelComplete(doubleBufferGraphics);
                    break;
                case GameState.GameOver:
                    DrawGameOver(doubleBufferGraphics);
                    break;
            }

            // Auf den Bildschirm zeichnen
            e.Graphics.DrawImage(doubleBuffer, 0, 0);
        }

        private void DrawMenu(Graphics g)
        {
            // Hintergrund
            DrawPixelStars(g);

            // Titel
            Font titleFont = new Font("Arial", 32, FontStyle.Bold);
            string title = "DRAGON FIRE";
            SizeF titleSize = g.MeasureString(title, titleFont);
            g.DrawString(title, titleFont, Brushes.Red, 
                (SCREEN_WIDTH - titleSize.Width) / 2, 100);

            // Untertitel
            Font subtitleFont = new Font("Arial", 24);
            string subtitle = "Pixel Art Dragon Game";
            SizeF subtitleSize = g.MeasureString(subtitle, subtitleFont);
            g.DrawString(subtitle, subtitleFont, Brushes.Orange, 
                (SCREEN_WIDTH - subtitleSize.Width) / 2, 150);

            // Anleitung
            Font smallFont = new Font("Arial", 16);
            string[] instructions = {
                "Steuerung:",
                "Pfeiltasten/WASD - Bewegen",
                "Leertaste - Feuer speien",
                "R - Level neu starten",
                "ESC - Menü",
                "",
                "Ziel: Alle Häuser mit so wenigen Schüssen wie möglich abfackeln!"
            };

            float yOffset = 250;
            foreach (string line in instructions)
            {
                SizeF size = g.MeasureString(line, smallFont);
                g.DrawString(line, smallFont, Brushes.White, 
                    (SCREEN_WIDTH - size.Width) / 2, yOffset);
                yOffset += 25;
            }

            // Level-Auswahl
            Font mediumFont = new Font("Arial", 20, FontStyle.Bold);
            string levelText = "Level auswählen:";
            SizeF levelSize = g.MeasureString(levelText, mediumFont);
            g.DrawString(levelText, mediumFont, Brushes.Yellow, 
                (SCREEN_WIDTH - levelSize.Width) / 2, 350);

            string[] levelButtons = { "1 - Level 1 (Einfach)", "2 - Level 2 (Mittel)", "3 - Level 3 (Schwer)" };
            yOffset = 380;
            foreach (string text in levelButtons)
            {
                SizeF size = g.MeasureString(text, smallFont);
                g.DrawString(text, smallFont, Brushes.White, 
                    (SCREEN_WIDTH - size.Width) / 2, yOffset);
                yOffset += 30;
            }

            // Start-Hinweis
            string startText = "ENTER - Level 1 starten";
            SizeF startSize = g.MeasureString(startText, smallFont);
            g.DrawString(startText, smallFont, Brushes.Green, 
                (SCREEN_WIDTH - startSize.Width) / 2, 450);

            // Cleanup
            titleFont.Dispose();
            subtitleFont.Dispose();
            smallFont.Dispose();
            mediumFont.Dispose();
        }

        private void DrawLevelComplete(Graphics g)
        {
            // Halbtransparenter Overlay
            using (Brush overlayBrush = new SolidBrush(Color.FromArgb(180, 0, 0, 0)))
            {
                g.FillRectangle(overlayBrush, 0, 0, SCREEN_WIDTH, SCREEN_HEIGHT);
            }

            // Level abgeschlossen Text
            Font largeFont = new Font("Arial", 32, FontStyle.Bold);
            string completeText = "LEVEL " + currentLevelNumber + " ABGESCHLOSSEN!";
            SizeF completeSize = g.MeasureString(completeText, largeFont);
            g.DrawString(completeText, largeFont, Brushes.Green, 
                (SCREEN_WIDTH - completeSize.Width) / 2, 200);

            // Statistik
            Font mediumFont = new Font("Arial", 20);
            string[] stats = {
                "Schüsse: " + currentLevel.ShotsFired,
                "Häuser verbrannt: " + currentLevel.HousesBurned + "/" + currentLevel.HousesTotal
            };

            float yOffset = 280;
            foreach (string line in stats)
            {
                SizeF size = g.MeasureString(line, mediumFont);
                g.DrawString(line, mediumFont, Brushes.White, 
                    (SCREEN_WIDTH - size.Width) / 2, yOffset);
                yOffset += 35;
            }

            // Beste Schüsse
            if (bestShots.ContainsKey(currentLevelNumber))
            {
                string bestText = "Beste Schüsse: " + bestShots[currentLevelNumber];
                SizeF bestSize = g.MeasureString(bestText, mediumFont);
                g.DrawString(bestText, mediumFont, Brushes.LightBlue, 
                    (SCREEN_WIDTH - bestSize.Width) / 2, yOffset);
                yOffset += 35;
            }

            // Fortsetzungsoptionen
            string continueText = "ENTER - Nächstes Level";
            SizeF continueSize = g.MeasureString(continueText, mediumFont);
            g.DrawString(continueText, mediumFont, Brushes.Green, 
                (SCREEN_WIDTH - continueSize.Width) / 2, 380);

            string menuText = "M - Menü";
            SizeF menuSize = g.MeasureString(menuText, mediumFont);
            g.DrawString(menuText, mediumFont, Brushes.Yellow, 
                (SCREEN_WIDTH - menuSize.Width) / 2, 420);

            // Cleanup
            largeFont.Dispose();
            mediumFont.Dispose();
        }

        private void DrawGameOver(Graphics g)
        {
            g.Clear(Color.Black);

            Font largeFont = new Font("Arial", 32, FontStyle.Bold);
            string gameOverText = "GAME OVER";
            SizeF gameOverSize = g.MeasureString(gameOverText, largeFont);
            g.DrawString(gameOverText, largeFont, Brushes.Red, 
                (SCREEN_WIDTH - gameOverSize.Width) / 2, 200);

            Font mediumFont = new Font("Arial", 20);
            string retryText = "ENTER - Nochmal versuchen";
            SizeF retrySize = g.MeasureString(retryText, mediumFont);
            g.DrawString(retryText, mediumFont, Brushes.Green, 
                (SCREEN_WIDTH - retrySize.Width) / 2, 300);

            string menuText = "M - Menü";
            SizeF menuSize = g.MeasureString(menuText, mediumFont);
            g.DrawString(menuText, mediumFont, Brushes.Yellow, 
                (SCREEN_WIDTH - menuSize.Width) / 2, 350);

            // Cleanup
            largeFont.Dispose();
            mediumFont.Dispose();
        }

        private void DrawUI(Graphics g)
        {
            Font font = new Font("Arial", 16);

            // Schüsse-Anzeige
            string shotsText = "Schüsse: " + currentLevel.ShotsFired;
            g.DrawString(shotsText, font, Brushes.White, 10, 10);

            // Häuser-Anzeige
            string housesText = "Häuser: " + currentLevel.HousesBurned + "/" + currentLevel.HousesTotal;
            g.DrawString(housesText, font, Brushes.White, 10, 30);

            // Level-Anzeige
            string levelText = "Level: " + currentLevelNumber;
            g.DrawString(levelText, font, Brushes.White, 10, 50);

            font.Dispose();
        }

        private void DrawPixelStars(Graphics g)
        {
            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                int starX = rand.Next(0, GAME_WIDTH);
                int starY = rand.Next(0, GAME_HEIGHT);
                if (rand.NextDouble() > 0.7)
                {
                    g.FillRectangle(Brushes.White, 
                        starX * PIXEL_SCALE, starY * PIXEL_SCALE, 
                        PIXEL_SCALE, PIXEL_SCALE);
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            gameTimer.Stop();
            doubleBufferGraphics.Dispose();
            doubleBuffer.Dispose();
            base.OnFormClosing(e);
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DragonFireGame());
        }
    }

    // Drachen-Klasse
    public class Dragon
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; } = 8;
        public int Height { get; } = 6;
        public int Speed { get; } = 3;
        public int Direction { get; set; } = 0; // 0 = rechts, 1 = links
        public bool FlameActive { get; private set; } = false;
        public int FlameTimer { get; private set; } = 0;

        private Color bodyColor = Color.FromArgb(255, 100, 0);
        private Color wingColor = Color.Yellow;
        private Color eyeColor = Color.White;

        public Dragon(int x, int y)
        {
            X = x;
            Y = y;
        }

        public void Move(int dx, int dy)
        {
            X += dx * Speed;
            Y += dy * Speed;

            // Bildschirmgrenzen
            X = Math.Max(0, Math.Min(X, DragonFireGame.GAME_WIDTH - Width));
            Y = Math.Max(0, Math.Min(Y, DragonFireGame.GAME_HEIGHT - Height));

            // Richtung aktualisieren
            if (dx < 0) Direction = 1;
            else if (dx > 0) Direction = 0;
        }

        public bool ShootFlame()
        {
            if (!FlameActive)
            {
                FlameActive = true;
                FlameTimer = 0;
                return true;
            }
            return false;
        }

        public void Update()
        {
            if (FlameActive)
            {
                FlameTimer++;
                if (FlameTimer >= 15)
                {
                    FlameActive = false;
                }
            }
        }

        public void Draw(Graphics g, int pixelScale)
        {
            // Körper
            DrawPixelRect(g, bodyColor, X, Y, Width, Height, pixelScale);

            // Flügel
            if (Direction == 0) // rechts
            {
                DrawPixelRect(g, wingColor, X + Width, Y + 1, 3, 2, pixelScale);
                DrawPixelRect(g, wingColor, X + Width, Y + 3, 3, 2, pixelScale);
            }
            else // links
            {
                DrawPixelRect(g, wingColor, X - 3, Y + 1, 3, 2, pixelScale);
                DrawPixelRect(g, wingColor, X - 3, Y + 3, 3, 2, pixelScale);
            }

            // Kopf
            DrawPixelRect(g, bodyColor, X + Width, Y, 4, 3, pixelScale);

            // Auge
            DrawPixelRect(g, eyeColor, X + Width + 2, Y + 1, 2, 1, pixelScale);

            // Schwanz
            DrawPixelRect(g, bodyColor, X - 2, Y + 2, 2, 2, pixelScale);

            // Beine
            DrawPixelRect(g, bodyColor, X + 2, Y + Height, 2, 2, pixelScale);
            DrawPixelRect(g, bodyColor, X + 4, Y + Height, 2, 2, pixelScale);

            // Feuer-Animation
            if (FlameActive && FlameTimer < 10)
            {
                int flameX = Direction == 0 ? X + Width + 4 : X - 4;
                int flameY = Y + 1;
                int flameHeight = 3;
                int flameWidth = 2 + (FlameTimer / 2);

                Color[] colors = { Color.Red, Color.Orange, Color.Yellow };
                for (int i = 0; i < flameWidth; i++)
                {
                    Color color = colors[Math.Min(i, colors.Length - 1)];
                    DrawPixelRect(g, color, flameX + (Direction == 0 ? i : -i), flameY, 1, flameHeight, pixelScale);
                    if (flameHeight > 1)
                    {
                        DrawPixelRect(g, color, flameX + (Direction == 0 ? i : -i), flameY + flameHeight, 1, 1, pixelScale);
                    }
                }
            }
        }

        private void DrawPixelRect(Graphics g, Color color, int x, int y, int width, int height, int scale)
        {
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x * scale, y * scale, width * scale, height * scale);
            }
        }
    }

    // Haus-Klasse
    public class House
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Burning { get; private set; } = false;
        public bool Burned { get; private set; } = false;
        public int BurnTimer { get; private set; } = 0;

        private Color wallColor = Color.FromArgb(139, 69, 19); // Brown
        private Color roofColor = Color.Red;
        private Random rand = new Random();

        public House(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public void StartBurning()
        {
            if (!Burning && !Burned)
            {
                Burning = true;
                BurnTimer = 0;
            }
        }

        public void Update()
        {
            if (Burning)
            {
                BurnTimer++;
                if (BurnTimer >= 60) // 1 Sekunde brennen
                {
                    Burning = false;
                    Burned = true;
                }
            }
        }

        public void Draw(Graphics g, int pixelScale)
        {
            if (Burned)
            {
                // Verbranntes Haus
                DrawPixelRect(g, Color.DarkGray, X, Y, Width, Height, pixelScale);
                DrawPixelRect(g, Color.Black, X, Y - 2, Width + 4, 2, pixelScale);
                return;
            }

            if (Burning)
            {
                // Brennendes Haus
                int burnIntensity = Math.Min(BurnTimer / 5, 3);
                Color[] colors = { wallColor, Color.Orange, Color.Red, Color.Yellow };
                Color houseColor = colors[Math.Min(burnIntensity, colors.Length - 1)];

                DrawPixelRect(g, houseColor, X, Y, Width, Height, pixelScale);
                DrawPixelRect(g, roofColor, X - 1, Y - 2, Width + 2, 2, pixelScale);

                // Flammen
                if (BurnTimer % 3 == 0)
                {
                    int flameX = X + rand.Next(0, Width);
                    int flameY = Y - 3 - (BurnTimer / 10);
                    int flameHeight = 2 + (BurnTimer / 15);

                    for (int i = 0; i < flameHeight; i++)
                    {
                        Color[] flameColors = { Color.Red, Color.Orange, Color.Yellow };
                        Color flameColor = flameColors[rand.Next(0, flameColors.Length)];
                        DrawPixelRect(g, flameColor, flameX, flameY - i, 1, 1, pixelScale);
                    }
                }
            }
            else
            {
                // Normales Haus
                DrawPixelRect(g, wallColor, X, Y, Width, Height, pixelScale);
                DrawPixelRect(g, roofColor, X - 1, Y - 2, Width + 2, 2, pixelScale);

                // Fenster
                DrawPixelRect(g, Color.Blue, X + 2, Y + 2, 2, 2, pixelScale);

                // Tür
                DrawPixelRect(g, Color.FromArgb(101, 67, 33), X + Width / 2 - 1, Y + Height - 2, 2, 2, pixelScale);
            }
        }

        private void DrawPixelRect(Graphics g, Color color, int x, int y, int width, int height, int scale)
        {
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x * scale, y * scale, width * scale, height * scale);
            }
        }
    }

    // Feuer-Projektil-Klasse
    public class FlameProjectile
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; } = 2;
        public int Height { get; } = 2;
        public int Speed { get; } = 8;
        public int Direction { get; set; } // 0 = rechts, 1 = links
        public bool Active { get; set; } = true;

        public FlameProjectile(int x, int y, int direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        public void Update()
        {
            if (Active)
            {
                if (Direction == 0) // rechts
                    X += Speed;
                else // links
                    X -= Speed;

                // Deaktivieren wenn außerhalb des Bildschirms
                if (X < 0 || X > DragonFireGame.GAME_WIDTH)
                    Active = false;
            }
        }

        public void Draw(Graphics g, int pixelScale)
        {
            if (Active)
            {
                // Feuerball
                DrawPixelRect(g, Color.Red, X, Y, Width, Height, pixelScale);
                DrawPixelRect(g, Color.Orange, X + 1, Y, Width, Height, pixelScale);
                DrawPixelRect(g, Color.Yellow, X, Y + 1, Width, Height, pixelScale);
            }
        }

        private void DrawPixelRect(Graphics g, Color color, int x, int y, int width, int height, int scale)
        {
            using (Brush brush = new SolidBrush(color))
            {
                g.FillRectangle(brush, x * scale, y * scale, width * scale, height * scale);
            }
        }
    }

    // Level-Klasse
    public class Level
    {
        public List<House> Houses { get; } = new List<House>();
        public List<FlameProjectile> Projectiles { get; } = new List<FlameProjectile>();
        public int ShotsFired { get; set; } = 0;
        public int HousesBurned { get; set; } = 0;
        public int HousesTotal { get; private set; }
        public bool Completed => HousesBurned >= HousesTotal;

        private int levelNumber;

        public Level(int levelNumber)
        {
            this.levelNumber = levelNumber;
            CreateLevel();
        }

        private void CreateLevel()
        {
            Houses.Clear();
            Projectiles.Clear();
            ShotsFired = 0;
            HousesBurned = 0;

            if (levelNumber == 1)
            {
                // Level 1 - 3 Häuser
                Houses.Add(new House(200, 200, 10, 8));
                Houses.Add(new House(300, 250, 12, 10));
                Houses.Add(new House(400, 200, 8, 6));
            }
            else if (levelNumber == 2)
            {
                // Level 2 - 5 Häuser
                Houses.Add(new House(150, 150, 8, 6));
                Houses.Add(new House(250, 200, 10, 8));
                Houses.Add(new House(350, 150, 12, 10));
                Houses.Add(new House(450, 200, 8, 6));
                Houses.Add(new House(550, 150, 10, 8));
            }
            else if (levelNumber == 3)
            {
                // Level 3 - 7 Häuser
                Houses.Add(new House(100, 100, 8, 6));
                Houses.Add(new House(200, 200, 10, 8));
                Houses.Add(new House(300, 100, 12, 10));
                Houses.Add(new House(400, 200, 8, 6));
                Houses.Add(new House(500, 100, 10, 8));
                Houses.Add(new House(600, 200, 12, 10));
                Houses.Add(new House(700, 100, 8, 6));
            }
            else
            {
                // Zufälliges Level
                Random rand = new Random();
                for (int i = 0; i < 5 + levelNumber * 2; i++)
                {
                    int x = rand.Next(50, DragonFireGame.GAME_WIDTH - 20);
                    int y = rand.Next(50, DragonFireGame.GAME_HEIGHT - 20);
                    int width = rand.Next(6, 12);
                    int height = rand.Next(6, 10);
                    Houses.Add(new House(x, y, width, height));
                }
            }

            HousesTotal = Houses.Count;
        }

        public void FireFlame(int x, int y, int direction)
        {
            Projectiles.Add(new FlameProjectile(x, y, direction));
            ShotsFired++;
        }

        public void Update()
        {
            foreach (var projectile in Projectiles)
            {
                projectile.Update();
            }

            foreach (var house in Houses)
            {
                house.Update();
            }
        }

        public void Draw(Graphics g, Dragon dragon)
        {
            // Hintergrund
            g.Clear(Color.Black);

            // Sterne im Hintergrund
            Random rand = new Random();
            for (int i = 0; i < 50; i++)
            {
                int starX = rand.Next(0, DragonFireGame.GAME_WIDTH);
                int starY = rand.Next(0, DragonFireGame.GAME_HEIGHT);
                if (rand.NextDouble() > 0.7)
                {
                    g.FillRectangle(Brushes.White, 
                        starX * DragonFireGame.PIXEL_SCALE, 
                        starY * DragonFireGame.PIXEL_SCALE, 
                        DragonFireGame.PIXEL_SCALE, 
                        DragonFireGame.PIXEL_SCALE);
                }
            }

            // Häuser zeichnen
            foreach (var house in Houses)
            {
                house.Draw(g, DragonFireGame.PIXEL_SCALE);
            }

            // Projektile zeichnen
            foreach (var projectile in Projectiles)
            {
                projectile.Draw(g, DragonFireGame.PIXEL_SCALE);
            }

            // Drache zeichnen
            dragon.Draw(g, DragonFireGame.PIXEL_SCALE);
        }
    }

    // Statische Klasse für Spielkonstanten
    public static class DragonFireGame
    {
        public const int PIXEL_SCALE = 4;
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 600;
        public const int GAME_WIDTH = SCREEN_WIDTH / PIXEL_SCALE;
        public const int GAME_HEIGHT = SCREEN_HEIGHT / PIXEL_SCALE;
    }
}
