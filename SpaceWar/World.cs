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

        private Sprite backSprite;
        private Sprite healthSprite;
        private Text distanceText;
        private float distance;
        private Player player;

        public World(Sprite sprite, Font font)
        {
            this.backSprite = sprite;
            this.healthSprite = new Sprite(sprite);
            healthSprite.TextureRect = new IntRect(64, 16, 16, 16);
            healthSprite.Scale = new Vector2f(healthSprite.Scale.X * 0.7f, healthSprite.Scale.Y * 0.7f);
            distance = 0;
            distanceText = new Text("0", font, 30)
            {
                FillColor = Color.White,
                Position = new Vector2f(10, 0)
            };
        }

        public void SetPlayer(Player player)
        {
            this.player = player;
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
                    if (GetRandomBool(random)) backSprite.TextureRect = new IntRect(0, 0, 16, 16);
                    else backSprite.TextureRect = new IntRect(16, 0, 16, 16);

                    backSprite.Position = new Vector2f(i * Program.SCALE*16, k * Program.SCALE *16);
                    k++;
                    renderTexture.Draw(backSprite);
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

        public void UpdateHealth(RenderWindow window)
        {
            for (uint i = player.health; i >= 1; i--)
            {
                healthSprite.Position = new Vector2f(i*16 + WIDTH * 12, 0);
                window.Draw(healthSprite);
            }
        }

        private static bool GetRandomBool(Random r, int truePercentage = 50)
        {
            return r.NextDouble() < truePercentage / 100.0;
        }
    }
}
