using System;
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


        static void Main(string[] args)
        {
            RenderWindow window = new RenderWindow(new VideoMode(World.WIDTH * 16, World.HEIGHT * 16), "Space War!");
            window.Closed += new EventHandler(OnClose);
            window.SetVerticalSyncEnabled(true);

            World world = new World("../../../tiles/tileset.png");
            Sprite map = world.Draw();
            View mapView = window.DefaultView;
            mapView.Rotate(180);
            Vector2f mapOffset = new Vector2f(0, 0.6f);
            float off = 0;

            Player player = new Player("../../../tiles/tileset.png");

            Clock clock = new Clock();
            
            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();
                
                long time = clock.ElapsedTime.AsMicroseconds(); //дать прошедшее время в микросекундах
                clock.Restart(); //перезагружает время
                time /= 1500; //скорость игры

                mapView.Move(mapOffset);
                window.SetView(mapView);
                window.Draw(map);
                window.SetView(window.DefaultView);

                off += mapOffset.Y;
                if (off > 2000)
                {
                    mapView.Move(new Vector2f(0, -1800));
                    off = 0;
                }

                player.direction = 0;
                if (Keyboard.IsKeyPressed(Keyboard.Key.Left)) player.direction = 1;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Right)) player.direction = 2;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Up)) player.direction = 3;
                else if (Keyboard.IsKeyPressed(Keyboard.Key.Down)) player.direction = 4;

                window.Draw(player.Update(time));

                window.Display();
            }
        }
    }
}
