using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpaceWar
{
    class Player
    {
        private readonly Sprite sprite;

        public uint direction, health;
        private float x, y, dx, dy, speed;
        
        public Player(Sprite sprite)
        {
            this.sprite = sprite;
            sprite.TextureRect = new IntRect(0, 16, 16, 32);
            direction = 0;
            health = 5;
            x = World.WIDTH * 8 - 25;
            y = World.HEIGHT * 16 - 120;
            speed = 0.4f;
        }

        public float GetX() { return x; }

        public Sprite Update(float time)
        {
            switch (direction)
            {
                case 0:
                    break;
                case 1: //left
                    dx = -speed;
                    dy = 0;
                    break;
                case 2: //right
                    dx = speed;
                    dy = 0;
                    break;
                case 3: //up
                    dx = 0;
                    dy = -speed;
                    break;
                case 4: //down
                    dx = 0;
                    dy = speed;
                    break;
            }
            if (x + dx * time > 0 && x + dx * time < World.WIDTH * 16 - 64) 
                x += dx * time;
            if (y + dy * time > 0 && y + dy * time < World.HEIGHT * 16 - 96) 
                y += dy * time;

            dx = 0;
            dy = 0;

            sprite.Position = new Vector2f(x, y);
            return sprite;
        }

        public Bullet Fire(Sprite bulletSprite)
        {
            return new Bullet(bulletSprite, x, y, 1, true);
        }
    }
}
