using SFML.Graphics;
using SFML.System;


namespace SpaceWar
{
    class Player
    {
        private readonly Sprite sprite;

        public uint direction, health;
        private float x, y, dx, dy, speed;

        private bool player_isFirstTile = false;
        private uint player_changeTileFrequency = 0;
        private uint bullet_timerBoosted = 0;

        public Player(Sprite sprite)
        {
            this.sprite = sprite;
            sprite.TextureRect = new IntRect(0, 16, 16, 32);
            direction = 0;
            health = 6;
            x = World.WIDTH * 8 - 25;
            y = World.HEIGHT * 16 - 120;
            speed = 0.4f;
        }

        public float GetX() { return x; }

        public void setPos(float x, float y)
        {
            this.x = x;
            this.y = y;
        }

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

            if (player_changeTileFrequency > 0) player_changeTileFrequency--;
            if (player_changeTileFrequency == 0)
            {
                player_changeTileFrequency = 50;
                if (player_isFirstTile) sprite.TextureRect = new IntRect(0, 16, 16, 32);
                else sprite.TextureRect = new IntRect(16, 16, 16, 32);
                player_isFirstTile = !player_isFirstTile;
            }

            if (bullet_timerBoosted > 0) bullet_timerBoosted--;

            sprite.Position = new Vector2f(x, y);
            return sprite;
        }

        public Bullet Fire(Sprite bulletSprite)
        {
            if (bullet_timerBoosted > 0)
                bulletSprite.TextureRect = new IntRect(48, 0, 16, 16);
            else
                bulletSprite.TextureRect = new IntRect(64, 0, 16, 16);
            return new Bullet(bulletSprite, x, y, 1, true);
        }

        public bool isHitting(Bullet bullet)
        {
            Vector2f pos = bullet.GetPosition();
            if (!bullet.isFromPlayer && 
                pos.X >= x && pos.X <= x + 16 * GameModel.SCALE &&
                pos.Y >= y && pos.Y <= y + 16 * GameModel.SCALE)
            {
                health -= bullet.power;
                return true;
            }
            return false;
        }

        public bool isBlackHole(BlackHole blackHole)
        {
            Vector2f pos = blackHole.GetPosition();
            if (pos.X + 8 * GameModel.SCALE >= x && pos.X + 8 * GameModel.SCALE <= x + 16 * GameModel.SCALE &&
                pos.Y + 8 * GameModel.SCALE >= y && pos.Y + 8 * GameModel.SCALE <= y + 16 * GameModel.SCALE)
            {
                health = 0;
                return true;
            }
            return false;
        }

        public bool isBooster(Booster booster)
        {
            Vector2f pos = booster.GetPosition();
            if (pos.X + 8 * GameModel.SCALE >= x && pos.X + 8 * GameModel.SCALE <= x + 16 * GameModel.SCALE &&
                pos.Y + 8 * GameModel.SCALE >= y && pos.Y + 8 * GameModel.SCALE <= y + 16 * GameModel.SCALE)
            {
                health++;
                bullet_timerBoosted = 200;
                return true;
            }
            return false;
        }
    }
}
