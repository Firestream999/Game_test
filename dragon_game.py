#!/usr/bin/env python3
"""
Dragon Fire Game - Pixel Art Dragon burning houses
Steuerung: Pfeiltasten zum Bewegen, Leertaste zum Schießen
Ziel: Alle Häuser mit so wenigen Schüssen wie möglich abfackeln
"""

import pygame
import sys
import random
import math
from enum import Enum

# Initialisierung
pygame.init()
pygame.display.set_caption("Dragon Fire - Pixel Art Game")

# Bildschirmgröße
SCREEN_WIDTH = 800
SCREEN_HEIGHT = 600
screen = pygame.display.set_mode((SCREEN_WIDTH, SCREEN_HEIGHT))

# Farben
BLACK = (0, 0, 0)
WHITE = (255, 255, 255)
RED = (255, 50, 50)
ORANGE = (255, 165, 0)
YELLOW = (255, 255, 0)
GREEN = (50, 200, 50)
BLUE = (50, 100, 200)
BROWN = (139, 69, 19)
GRAY = (100, 100, 100)
DARK_GRAY = (50, 50, 50)
LIGHT_GRAY = (200, 200, 200)

# Pixel-Art Skalierung
PIXEL_SCALE = 4

def draw_pixel_rect(surface, color, x, y, width, height):
    """Zeichnet ein Pixel-Art Rechteck"""
    pygame.draw.rect(surface, color, (x * PIXEL_SCALE, y * PIXEL_SCALE, 
                                      width * PIXEL_SCALE, height * PIXEL_SCALE))

class GameState(Enum):
    MENU = 1
    PLAYING = 2
    LEVEL_COMPLETE = 3
    GAME_OVER = 4

class Dragon:
    def __init__(self, x, y):
        self.x = x
        self.y = y
        self.width = 8
        self.height = 6
        self.speed = 3
        self.color = ORANGE
        self.body_color = RED
        self.wing_color = YELLOW
        self.eye_color = WHITE
        self.flame_timer = 0
        self.flame_active = False
        self.direction = 0  # 0 = rechts, 1 = links
        
    def draw(self, surface):
        # Körper
        draw_pixel_rect(surface, self.body_color, self.x, self.y, self.width, self.height)
        
        # Flügel
        if self.direction == 0:  # rechts
            draw_pixel_rect(surface, self.wing_color, self.x + self.width, self.y + 1, 3, 2)
            draw_pixel_rect(surface, self.wing_color, self.x + self.width, self.y + 3, 3, 2)
        else:  # links
            draw_pixel_rect(surface, self.wing_color, self.x - 3, self.y + 1, 3, 2)
            draw_pixel_rect(surface, self.wing_color, self.x - 3, self.y + 3, 3, 2)
        
        # Kopf
        draw_pixel_rect(surface, self.body_color, self.x + self.width, self.y, 4, 3)
        
        # Auge
        draw_pixel_rect(surface, self.eye_color, self.x + self.width + 2, self.y + 1, 2, 1)
        
        # Schwanz
        draw_pixel_rect(surface, self.body_color, self.x - 2, self.y + 2, 2, 2)
        
        # Beine
        draw_pixel_rect(surface, self.body_color, self.x + 2, self.y + self.height, 2, 2)
        draw_pixel_rect(surface, self.body_color, self.x + 4, self.y + self.height, 2, 2)
        
        # Feuer-Animation
        if self.flame_active and self.flame_timer < 10:
            flame_x = self.x + self.width + 4
            flame_y = self.y + 1
            flame_height = 3
            flame_width = 2 + (self.flame_timer // 2)
            
            # Feuerfarben
            colors = [RED, ORANGE, YELLOW]
            for i in range(flame_width):
                color = colors[min(i, len(colors) - 1)]
                draw_pixel_rect(surface, color, flame_x + i, flame_y, 1, flame_height)
                if flame_height > 1:
                    draw_pixel_rect(surface, color, flame_x + i, flame_y + flame_height, 1, 1)
        
    def move(self, dx, dy):
        self.x += dx * self.speed
        self.y += dy * self.speed
        
        # Bildschirmgrenzen
        self.x = max(0, min(self.x, SCREEN_WIDTH // PIXEL_SCALE - self.width))
        self.y = max(0, min(self.y, SCREEN_HEIGHT // PIXEL_SCALE - self.height))
        
        # Richtung aktualisieren
        if dx < 0:
            self.direction = 1
        elif dx > 0:
            self.direction = 0
        
    def shoot_flame(self):
        if not self.flame_active:
            self.flame_active = True
            self.flame_timer = 0
            return True
        return False
    
    def update(self):
        if self.flame_active:
            self.flame_timer += 1
            if self.flame_timer >= 15:
                self.flame_active = False

class House:
    def __init__(self, x, y, width, height):
        self.x = x
        self.y = y
        self.width = width
        self.height = height
        self.color = BROWN
        self.roof_color = RED
        self.burning = False
        self.burn_timer = 0
        self.burned = False
        
    def draw(self, surface):
        if self.burned:
            # Verbranntes Haus
            draw_pixel_rect(surface, DARK_GRAY, self.x, self.y, self.width, self.height)
            draw_pixel_rect(surface, BLACK, self.x, self.y - 2, self.width + 4, 2)
            return
            
        if self.burning:
            # Brennendes Haus
            burn_intensity = min(self.burn_timer // 5, 3)
            colors = [self.color, ORANGE, RED, YELLOW]
            house_color = colors[min(burn_intensity, len(colors) - 1)]
            
            draw_pixel_rect(surface, house_color, self.x, self.y, self.width, self.height)
            
            # Dach
            draw_pixel_rect(surface, self.roof_color, self.x - 1, self.y - 2, self.width + 2, 2)
            
            # Flammen
            if self.burn_timer % 3 == 0:
                flame_x = self.x + random.randint(0, self.width - 1)
                flame_y = self.y - 3 - (self.burn_timer // 10)
                flame_height = 2 + (self.burn_timer // 15)
                
                for i in range(flame_height):
                    flame_color = [RED, ORANGE, YELLOW][random.randint(0, 2)]
                    draw_pixel_rect(surface, flame_color, flame_x, flame_y - i, 1, 1)
        else:
            # Normales Haus
            draw_pixel_rect(surface, self.color, self.x, self.y, self.width, self.height)
            
            # Dach
            draw_pixel_rect(surface, self.roof_color, self.x - 1, self.y - 2, self.width + 2, 2)
            
            # Fenster
            draw_pixel_rect(surface, BLUE, self.x + 2, self.y + 2, 2, 2)
            
            # Tür
            draw_pixel_rect(surface, BROWN, self.x + self.width // 2 - 1, self.y + self.height - 2, 2, 2)
    
    def burn(self):
        if not self.burning and not self.burned:
            self.burning = True
            self.burn_timer = 0
            return True
        return False
    
    def update(self):
        if self.burning:
            self.burn_timer += 1
            if self.burn_timer >= 60:  # 1 Sekunde brennen
                self.burning = False
                self.burned = True

class FlameProjectile:
    def __init__(self, x, y, direction):
        self.x = x
        self.y = y
        self.width = 2
        self.height = 2
        self.speed = 8
        self.direction = direction  # 0 = rechts, 1 = links
        self.active = True
        self.color = ORANGE
        
    def draw(self, surface):
        if self.active:
            # Feuerball
            draw_pixel_rect(surface, RED, self.x, self.y, self.width, self.height)
            draw_pixel_rect(surface, ORANGE, self.x + 1, self.y, self.width, self.height)
            draw_pixel_rect(surface, YELLOW, self.x, self.y + 1, self.width, self.height)
    
    def update(self):
        if self.active:
            if self.direction == 0:  # rechts
                self.x += self.speed
            else:  # links
                self.x -= self.speed
            
            # Deaktivieren wenn außerhalb des Bildschirms
            if self.x < 0 or self.x > SCREEN_WIDTH // PIXEL_SCALE:
                self.active = False

class Level:
    def __init__(self, level_num):
        self.level_num = level_num
        self.houses = []
        self.dragon = Dragon(50, SCREEN_HEIGHT // PIXEL_SCALE // 2)
        self.projectiles = []
        self.shots_fired = 0
        self.houses_burned = 0
        self.houses_total = 0
        self.completed = False
        
        self.create_level()
        
    def create_level(self):
        # Level-Designs
        if self.level_num == 1:
            # Einfaches Level - 3 Häuser
            self.houses = [
                House(200, 200, 10, 8),
                House(300, 250, 12, 10),
                House(400, 200, 8, 6)
            ]
        elif self.level_num == 2:
            # Mittleres Level - 5 Häuser
            self.houses = [
                House(150, 150, 8, 6),
                House(250, 200, 10, 8),
                House(350, 150, 12, 10),
                House(450, 200, 8, 6),
                House(550, 150, 10, 8)
            ]
        elif self.level_num == 3:
            # Schweres Level - 7 Häuser
            self.houses = [
                House(100, 100, 8, 6),
                House(200, 200, 10, 8),
                House(300, 100, 12, 10),
                House(400, 200, 8, 6),
                House(500, 100, 10, 8),
                House(600, 200, 12, 10),
                House(700, 100, 8, 6)
            ]
        else:
            # Zufälliges Level
            for i in range(5 + self.level_num * 2):
                x = random.randint(50, SCREEN_WIDTH // PIXEL_SCALE - 20)
                y = random.randint(50, SCREEN_HEIGHT // PIXEL_SCALE - 20)
                width = random.randint(6, 12)
                height = random.randint(6, 10)
                self.houses.append(House(x, y, width, height))
        
        self.houses_total = len(self.houses)
        
    def update(self):
        # Drache aktualisieren
        self.dragon.update()
        
        # Projektile aktualisieren
        for projectile in self.projectiles[:]:
            projectile.update()
            if not projectile.active:
                self.projectiles.remove(projectile)
        
        # Häuser aktualisieren
        for house in self.houses:
            house.update()
        
        # Kollisionen prüfen
        for projectile in self.projectiles[:]:
            for house in self.houses:
                if (projectile.x < house.x + house.width and 
                    projectile.x + projectile.width > house.x and
                    projectile.y < house.y + house.height and 
                    projectile.y + projectile.height > house.y):
                    
                    if house.burn():
                        self.houses_burned += 1
                    
                    projectile.active = False
                    if projectile in self.projectiles:
                        self.projectiles.remove(projectile)
                    break
        
        # Level abgeschlossen?
        if self.houses_burned >= self.houses_total:
            self.completed = True
    
    def draw(self, surface):
        # Hintergrund
        surface.fill(BLACK)
        
        # Sterne im Hintergrund
        for i in range(50):
            star_x = random.randint(0, SCREEN_WIDTH // PIXEL_SCALE)
            star_y = random.randint(0, SCREEN_HEIGHT // PIXEL_SCALE)
            if random.random() > 0.7:
                draw_pixel_rect(surface, WHITE, star_x, star_y, 1, 1)
        
        # Häuser zeichnen
        for house in self.houses:
            house.draw(surface)
        
        # Projektile zeichnen
        for projectile in self.projectiles:
            projectile.draw(surface)
        
        # Drache zeichnen
        self.dragon.draw(surface)
        
        # UI zeichnen
        self.draw_ui(surface)
    
    def draw_ui(self, surface):
        # Schüsse-Anzeige
        text = f"Schüsse: {self.shots_fired}"
        font = pygame.font.SysFont('Arial', 16)
        text_surface = font.render(text, True, WHITE)
        surface.blit(text_surface, (10, 10))
        
        # Häuser-Anzeige
        text = f"Häuser: {self.houses_burned}/{self.houses_total}"
        text_surface = font.render(text, True, WHITE)
        surface.blit(text_surface, (10, 30))
        
        # Level-Anzeige
        text = f"Level: {self.level_num}"
        text_surface = font.render(text, True, WHITE)
        surface.blit(text_surface, (10, 50))
    
    def fire_flame(self):
        direction = self.dragon.direction
        if direction == 0:  # rechts
            projectile = FlameProjectile(
                self.dragon.x + self.dragon.width + 2, 
                self.dragon.y + 2, 
                direction
            )
        else:  # links
            projectile = FlameProjectile(
                self.dragon.x - 2, 
                self.dragon.y + 2, 
                direction
            )
        
        self.projectiles.append(projectile)
        self.shots_fired += 1
        return True

class Game:
    def __init__(self):
        self.state = GameState.MENU
        self.current_level = 1
        self.level = None
        self.clock = pygame.time.Clock()
        self.font_large = pygame.font.SysFont('Arial', 32)
        self.font_medium = pygame.font.SysFont('Arial', 24)
        self.font_small = pygame.font.SysFont('Arial', 16)
        self.best_shots = {}  # Beste Schusszahl pro Level
        
    def start_level(self, level_num):
        self.current_level = level_num
        self.level = Level(level_num)
        self.state = GameState.PLAYING
        
    def handle_events(self):
        for event in pygame.event.get():
            if event.type == pygame.QUIT:
                return False
            
            if event.type == pygame.KEYDOWN:
                if self.state == GameState.MENU:
                    if event.key == pygame.K_RETURN:
                        self.start_level(1)
                    elif event.key == pygame.K_1:
                        self.start_level(1)
                    elif event.key == pygame.K_2:
                        self.start_level(2)
                    elif event.key == pygame.K_3:
                        self.start_level(3)
                    elif event.key == pygame.K_ESCAPE:
                        return False
                        
                elif self.state == GameState.PLAYING:
                    if event.key == pygame.K_SPACE:
                        # Drache schießt
                        if self.level.dragon.shoot_flame():
                            self.level.fire_flame()
                    elif event.key == pygame.K_r:
                        # Level neu starten
                        self.start_level(self.current_level)
                    elif event.key == pygame.K_ESCAPE:
                        self.state = GameState.MENU
                        
                elif self.state == GameState.LEVEL_COMPLETE:
                    if event.key == pygame.K_RETURN:
                        # Nächstes Level
                        self.start_level(self.current_level + 1)
                    elif event.key == pygame.K_m:
                        # Zurück zum Menü
                        self.state = GameState.MENU
                    elif event.key == pygame.K_ESCAPE:
                        self.state = GameState.MENU
                        
                elif self.state == GameState.GAME_OVER:
                    if event.key == pygame.K_RETURN:
                        # Level neu starten
                        self.start_level(self.current_level)
                    elif event.key == pygame.K_m:
                        # Zurück zum Menü
                        self.state = GameState.MENU
                    elif event.key == pygame.K_ESCAPE:
                        return False
        
        return True
    
    def handle_movement(self):
        if self.state == GameState.PLAYING:
            keys = pygame.key.get_pressed()
            dx, dy = 0, 0
            
            if keys[pygame.K_LEFT] or keys[pygame.K_a]:
                dx -= 1
            if keys[pygame.K_RIGHT] or keys[pygame.K_d]:
                dx += 1
            if keys[pygame.K_UP] or keys[pygame.K_w]:
                dy -= 1
            if keys[pygame.K_DOWN] or keys[pygame.K_s]:
                dy += 1
            
            self.level.dragon.move(dx, dy)
    
    def update(self):
        if self.state == GameState.PLAYING:
            self.level.update()
            
            # Level abgeschlossen?
            if self.level.completed:
                # Beste Schusszahl speichern
                if self.current_level not in self.best_shots or \
                   self.level.shots_fired < self.best_shots[self.current_level]:
                    self.best_shots[self.current_level] = self.level.shots_fired
                
                self.state = GameState.LEVEL_COMPLETE
    
    def draw(self):
        if self.state == GameState.MENU:
            self.draw_menu()
        elif self.state == GameState.PLAYING:
            self.level.draw(screen)
        elif self.state == GameState.LEVEL_COMPLETE:
            self.draw_level_complete()
        elif self.state == GameState.GAME_OVER:
            self.draw_game_over()
        
        pygame.display.flip()
    
    def draw_menu(self):
        screen.fill(BLACK)
        
        # Titel
        title = self.font_large.render("DRAGON FIRE", True, RED)
        screen.blit(title, (SCREEN_WIDTH // 2 - title.get_width() // 2, 100))
        
        # Untertitel
        subtitle = self.font_medium.render("Pixel Art Dragon Game", True, ORANGE)
        screen.blit(subtitle, (SCREEN_WIDTH // 2 - subtitle.get_width() // 2, 150))
        
        # Anleitung
        instructions = [
            "Steuerung:",
            "Pfeiltasten/WASD - Bewegen",
            "Leertaste - Feuer speien",
            "R - Level neu starten",
            "ESC - Menü",
            "",
            "Ziel: Alle Häuser mit so wenigen Schüssen wie möglich abfackeln!"
        ]
        
        y_offset = 250
        for line in instructions:
            text = self.font_small.render(line, True, WHITE)
            screen.blit(text, (SCREEN_WIDTH // 2 - text.get_width() // 2, y_offset))
            y_offset += 25
        
        # Level-Auswahl
        level_text = self.font_medium.render("Level auswählen:", True, YELLOW)
        screen.blit(level_text, (SCREEN_WIDTH // 2 - level_text.get_width() // 2, 350))
        
        level_buttons = [
            ("1 - Level 1 (Einfach)", 1),
            ("2 - Level 2 (Mittel)", 2),
            ("3 - Level 3 (Schwer)", 3)
        ]
        
        y_offset = 380
        for text, level in level_buttons:
            rendered = self.font_small.render(text, True, WHITE)
            screen.blit(rendered, (SCREEN_WIDTH // 2 - rendered.get_width() // 2, y_offset))
            y_offset += 30
        
        # Enter zum Starten
        start_text = self.font_small.render("ENTER - Level 1 starten", True, GREEN)
        screen.blit(start_text, (SCREEN_WIDTH // 2 - start_text.get_width() // 2, 450))
    
    def draw_level_complete(self):
        self.level.draw(screen)
        
        # Halbtransparenter Overlay
        overlay = pygame.Surface((SCREEN_WIDTH, SCREEN_HEIGHT))
        overlay.fill(BLACK)
        overlay.set_alpha(180)
        screen.blit(overlay, (0, 0))
        
        # Level abgeschlossen Text
        complete_text = self.font_large.render(f"LEVEL {self.current_level} ABGESCHLOSSEN!", True, GREEN)
        screen.blit(complete_text, (SCREEN_WIDTH // 2 - complete_text.get_width() // 2, 200))
        
        # Statistik
        stats = [
            f"Schüsse: {self.level.shots_fired}",
            f"Häuser verbrannt: {self.level.houses_burned}/{self.level.houses_total}"
        ]
        
        if self.current_level in self.best_shots:
            stats.append(f"Beste Schüsse: {self.best_shots[self.current_level]}")
        
        y_offset = 280
        for line in stats:
            text = self.font_medium.render(line, True, WHITE)
            screen.blit(text, (SCREEN_WIDTH // 2 - text.get_width() // 2, y_offset))
            y_offset += 35
        
        # Fortsetzungsoptionen
        continue_text = self.font_medium.render("ENTER - Nächstes Level", True, GREEN)
        screen.blit(continue_text, (SCREEN_WIDTH // 2 - continue_text.get_width() // 2, 380))
        
        menu_text = self.font_medium.render("M - Menü", True, YELLOW)
        screen.blit(menu_text, (SCREEN_WIDTH // 2 - menu_text.get_width() // 2, 420))
    
    def draw_game_over(self):
        screen.fill(BLACK)
        
        game_over_text = self.font_large.render("GAME OVER", True, RED)
        screen.blit(game_over_text, (SCREEN_WIDTH // 2 - game_over_text.get_width() // 2, 200))
        
        retry_text = self.font_medium.render("ENTER - Nochmal versuchen", True, GREEN)
        screen.blit(retry_text, (SCREEN_WIDTH // 2 - retry_text.get_width() // 2, 300))
        
        menu_text = self.font_medium.render("M - Menü", True, YELLOW)
        screen.blit(menu_text, (SCREEN_WIDTH // 2 - menu_text.get_width() // 2, 350))
    
    def run(self):
        running = True
        
        while running:
            # Events
            running = self.handle_events()
            
            # Bewegung
            self.handle_movement()
            
            # Update
            self.update()
            
            # Zeichnen
            self.draw()
            
            # FPS begrenzen
            self.clock.tick(60)
        
        pygame.quit()
        sys.exit()

# Spiel starten
if __name__ == "__main__":
    game = Game()
    game.run()
