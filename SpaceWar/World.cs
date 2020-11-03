using System;
using System.Collections.Generic;
using System.Text;
using SFML;
using SFML.Graphics;
using SFML.System;

namespace SpaceWar
{
    class World
    {
        public static readonly uint WIDTH = 31;
        public static readonly uint HEIGHT = 50;
        public static readonly uint MAP_LEN = 160;
        public static readonly uint SCALE = 4;

        private Image tileSet;
        private Texture texture;
        private Sprite sprite;

        //private uint[,] tileMap = new uint[WIDTH, MAP_LEN];

        public World(String filename)
        {
            tileSet = new Image(filename);
            texture = new Texture(tileSet);
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(sprite.Scale.X * SCALE, sprite.Scale.Y * SCALE);
        }

        public Sprite Draw()
        {
            RenderTexture renderTexture = new RenderTexture(WIDTH*16, MAP_LEN*16);
            Random random = new Random();
            uint k = 0;
            for (uint i = 0; i < WIDTH; i++)
            {
                for (uint j = 0; j < MAP_LEN; j++)
                {
                    if (GetRandomBool(random)) sprite.TextureRect = new IntRect(0, 0, 16, 16);
                    else sprite.TextureRect = new IntRect(16, 0, 16, 16);

                    sprite.Position = new Vector2f(i * SCALE*16, k * SCALE*16);
                    k++;
                    renderTexture.Draw(sprite);
                }
                k = 0;
            }
            return new Sprite(renderTexture.Texture);
        }

        private static bool GetRandomBool(Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }
    }
}
