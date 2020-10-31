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
        private readonly Image player;
        private readonly Texture texture;
        private readonly Sprite sprite;

        public uint direction;
        private float x, y, dx, dy, speed;

        public Player(string filename)
        {
            player = new Image(filename);
            texture = new Texture(player);
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(sprite.Scale.X * World.SCALE, sprite.Scale.Y * World.SCALE);
            sprite.TextureRect = new IntRect(0, 16, 16, 32);
            direction = 0;
            x = World.WIDTH * 8 - 25;
            y = World.HEIGHT * 16 - 120;
            speed = 0.4f;
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
            if (x + dx * time < World.WIDTH * 16)
                x += dx * time;
            if (y + dy * time < World.HEIGHT * 16)
                y += dy * time;

            dx = 0;
            dy = 0;

            sprite.Position = new Vector2f(x, y);
            return sprite;
        }
    }
}
