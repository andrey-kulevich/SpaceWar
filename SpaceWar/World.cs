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

        private Sprite sprite;
        private Text distanceText;
        private float distance;
        //private Text playerHealth;

        public World(Sprite sprite, Font font)
        {
            this.sprite = sprite;
            distance = 0;
            distanceText = new Text("0", font, 30);
            distanceText.FillColor = Color.White;
            distanceText.Position = new Vector2f(10, 0);
            
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

                    sprite.Position = new Vector2f(i * Program.SCALE*16, k * Program.SCALE *16);
                    k++;
                    renderTexture.Draw(sprite);
                }
                k = 0;
            }
            return new Sprite(renderTexture.Texture);
        }

        public Text UpdateText()
        {
            uint dis = (uint)distance;
            distanceText.DisplayedString = dis.ToString();
            distance += 0.4f;
            return distanceText;
        }

        private static bool GetRandomBool(Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }
    }
}
