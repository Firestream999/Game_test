#!/usr/bin/env python3
"""
Einfaches Skript zum Erstellen eines Icons für das Spiel
"""

import pygame
import sys

# Icon erstellen
size = 32
icon = pygame.Surface((size, size), pygame.SRCALPHA)

# Drachen-Icon zeichnen
pygame.draw.rect(icon, (255, 100, 0), (8, 8, 16, 12))  # Körper
pygame.draw.rect(icon, (255, 200, 0), (20, 10, 8, 4))  # Kopf
pygame.draw.rect(icon, (255, 50, 0), (4, 12, 4, 8))   # Flügel
pygame.draw.rect(icon, (255, 255, 0), (24, 6, 4, 4))  # Feuer

# Icon speichern
pygame.image.save(icon, "dragon_icon.png")
print("Icon wurde als dragon_icon.png gespeichert")
