﻿using SFML.Graphics;
using SFML.System;
using System;

namespace SpaceWar
{
    class BlackHole
    {
        private readonly Sprite sprite;

        private float x, y, speed;
        private static Random random = new Random();

        private bool isFirstTile = false;
        private uint changeTileFrequency = 0;

        public BlackHole(Sprite sprite)
        {
            this.sprite = sprite;
            sprite.TextureRect = new IntRect(80, 0, 16, 16);
            x = random.Next(0, (int)(World.WIDTH * 16 - 64));
            y = -16 * GameModel.SCALE;
            speed = 0.2f;
        }

        public Vector2f GetPosition() { return new Vector2f(x, y); }

        public Sprite Update(float time)
        {
            y += speed * time;

            if (changeTileFrequency > 0) changeTileFrequency--;
            if (changeTileFrequency == 0)
            {
                changeTileFrequency = 50;
                if (isFirstTile) sprite.TextureRect = new IntRect(80, 0, 16, 16);
                else sprite.TextureRect = new IntRect(80, 16, 16, 16);
                isFirstTile = !isFirstTile;
            }

            sprite.Position = new Vector2f(x, y);
            return sprite;
        }
    }
}
