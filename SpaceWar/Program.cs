using System;
using System.Collections.Generic;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace SpaceWar
{
    class Program
    {
        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public static readonly uint SCALE = 4;

        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(World.WIDTH * 16, World.HEIGHT * 16), "Space War!");
            window.Closed += new EventHandler(OnClose);
            window.SetVerticalSyncEnabled(true);

            Image tileSet;
            Texture texture;
            Sprite sprite;

            //load sprite from image
            tileSet = new Image("../../../tiles/tileset.png");
            texture = new Texture(tileSet);
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(sprite.Scale.X * SCALE, sprite.Scale.Y * SCALE);

            //load font
            Font font = new Font("../../../tiles/font.ttf");

            //background
            World world = new World(new Sprite(sprite), font);
            Sprite map = world.Draw();
            View mapView = window.DefaultView;
            mapView.Rotate(180);
            Vector2f mapOffset = new Vector2f(0, 0.6f);
            float off = 0;

            //player
            Player player = new Player(new Sprite(sprite));
            world.SetPlayer(player);

            //bullets
            List<Bullet> bullets = new List<Bullet>();
            uint fireFrequency = 0;

            //enemies
            List<Enemy> enemies = new List<Enemy>();
            int enemySpawnFrequency = 0;

            Random random = new Random();

            Clock clock = new Clock();
            
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                
                //set the speed of game
                long time = clock.ElapsedTime.AsMicroseconds(); 
                clock.Restart(); 
                time /= 1500; 

                //animate the background
                mapView.Move(mapOffset);
                window.SetView(mapView);
                window.Draw(map);
                window.SetView(window.DefaultView);

                //move view of space to start position if it ends
                off += mapOffset.Y;
                if (off > 1750)
                {
                    mapView.Move(new Vector2f(0, -1750));
                    off = 0;
                }

                //control the player
                player.direction = 0;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) player.direction = 1;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) player.direction = 2;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) player.direction = 3;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Down)) player.direction = 4;

                if (fireFrequency > 0) fireFrequency--;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Space) && fireFrequency == 0) 
                {
                    fireFrequency = 20;
                    bullets.Add(player.Fire(new Sprite(sprite))); 
                }

                //move the bullets
                foreach (Bullet bullet in bullets) 
                {
                    window.Draw(bullet.Update(time));
                }

                //move the enemies
                foreach (Enemy enemy in enemies)
                {
                    window.Draw(enemy.Update(time));
                }

                //spawn new enemies
                if (enemySpawnFrequency > 0) enemySpawnFrequency--;
                if (enemySpawnFrequency == 0)
                {
                    enemySpawnFrequency = random.Next(50, 500);
                    enemies.Add(new Enemy(new Sprite(sprite), player));
                }

                window.Draw(player.Update(time)); //move the player
                window.Draw(world.UpdateText()); //update distance traveled
                world.UpdateHealth(window);

                window.Display();
            }
        }
    }
}
