using SFML.Graphics;
using SFML.System;
using System;

namespace SpaceWar
{
    class Enemy
    {
        private readonly Sprite sprite;
        private readonly Player player;

        private uint health;
        private float x, y, speed;

        private static Random random = new Random();

        private int offsetCount = 0;                     //amount of random offsets relative to the player
        private float offsetLength = 0;                  //length of the random offset
        private int randomDirection = random.Next(0, 1); //0 - left, 1 - right       

        public Enemy(Sprite sprite, Player player)
        {
            this.sprite = sprite;
            this.player = player;
            sprite.TextureRect = new IntRect(96, 0, 16, 16);
            health = 1;
            x = random.Next(0, (int)(World.WIDTH * 16 - 64));
            y = - 16 * GameModel.SCALE;
            speed = 0.2f;
        }

        public uint GetHealth() { return health; }
        public Vector2f GetPosition() { return new Vector2f(x, y); }

        public Sprite Update(float time)
        {
            y += speed * time;

            if (offsetCount > 0)
            {
                if (randomDirection == 1) x += speed * time;
                else x -= speed * time;
                if (x <= 0 || x > World.WIDTH * 16 - 64) randomDirection = -randomDirection; 
                offsetLength--;
                if (offsetLength <= 0) offsetCount--;
            }
            else
            {
                if (player.GetX() > x) x += speed * time;
                else x -= speed * time;

                if (Math.Abs(player.GetX() - x) < 5)
                {
                    offsetCount = random.Next(0, 2);
                    offsetLength = random.Next(0, 150);
                    randomDirection = random.Next(0, 2);
                }              
            }
            
            sprite.Position = new Vector2f(x, y);
            return sprite;
        }

        public Bullet Fire(Sprite bulletSprite)
        {
            bulletSprite.TextureRect = new IntRect(64, 0, 16, 16);
            return new Bullet(bulletSprite, x, y, 1, false);
        }

        public bool isHitting(Bullet bullet)
        {
            Vector2f pos = bullet.GetPosition();
            if (bullet.isFromPlayer &&
                pos.X >= x && pos.X <= x + 16 * GameModel.SCALE &&
                pos.Y >= y && pos.Y <= y + 16 * GameModel.SCALE)
            {
                health -= bullet.power;
                return true;
            }
            return false;
        }
    }
}
