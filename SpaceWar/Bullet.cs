using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceWar
{
    class Bullet
    {
        private readonly Sprite sprite;
        private float x, y, speed;
        private bool isFromPlayer;

        public Bullet(Sprite sprite, float x, float y, bool isFromPlayer)
        {
            this.sprite = sprite;
            sprite.TextureRect = new IntRect(96, 16, 16, 32);
            speed = 0.6f;
            this.x = x;
            this.y = y;
            this.isFromPlayer = isFromPlayer;
        }

        public Sprite Update(float time)
        {
            if (isFromPlayer) y -= speed * time;
            else y += speed * time;
            sprite.Position = new Vector2f(x, y);
            return sprite;
        }
    }
}
