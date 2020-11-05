using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceWar
{
    class Enemy
    {
        private readonly Sprite sprite;
        private readonly Player player;

        private uint health;
        private float x, y, speed;

        public Enemy(Sprite sprite, Player player)
        {
            this.sprite = sprite;
            this.player = player;
            sprite.TextureRect = new IntRect(96, 0, 16, 16);
            health = 3;
            Random random = new Random();
            x = random.Next(0, (int)(World.WIDTH * 16 - 64));
            y = - 16 * Program.SCALE;
            speed = 0.2f;
        }

        public Sprite Update(float time)
        {
            y += speed * time;

            if (player.GetX() > x) x += speed * time;
            else x -= speed * time;

            sprite.Position = new Vector2f(x, y);
            return sprite;
        }

        public Bullet Fire(Sprite bulletSprite)
        {
            return new Bullet(bulletSprite, x, y, 1, false);
        }

        public bool isHitting(Bullet bullet)
        {
            Vector2f pos = bullet.GetPosition();
            if (pos.X >= x && pos.X <= x + 16 * Program.SCALE &&
                pos.Y >= y && pos.Y <= y + 16 * Program.SCALE)
            {
                health -= bullet.power;
                return true;
            }
            return false;
        }

        public uint GetHealth() { return health; }
    }
}
