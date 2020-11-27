using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;

namespace SpaceWar
{
    class GameModel
    {
        public static readonly uint SCALE = 4;
        private RenderWindow window;
        private Sprite sprite;
        private Font font;
        private bool isMenuOpen = false; //state of pause menu
        private uint fireFrequency = 0;
        private uint pressFrequency = 0;
        private bool isEndOfGame = false;
        private uint bestScore = 0;
        private uint score = 0;
        private RectangleShape menuArea = new RectangleShape(new Vector2f(248, 400));
        private Sprite reloadIcon;
        private World world;
        private Sprite map;
        private Player player;
        private List<Bullet> bullets = new List<Bullet>();
        private List<Enemy> enemies = new List<Enemy>();
        private int enemySpawnFrequency = 0;
        private List<BlackHole> blackHoles = new List<BlackHole>();
        private int blackHoleSpawnFrequency = 0;
        private List<Booster> boosters = new List<Booster>();
        private int boosterSpawnFrequency = 0;

        private Random random = new Random();
        private Clock clock = new Clock();
        private long time;

        public GameModel()
        {
            LoadData(); //load tileset and font

            world = new World(new Sprite(sprite), font);
            map = world.Draw();

            //player
            player = new Player(new Sprite(sprite));
            world.SetPlayer(player);

            //pause menu           
            menuArea.Position = new Vector2f(124, 200);
            menuArea.FillColor = new Color(51, 51, 53);
            menuArea.OutlineThickness = 5;
            menuArea.OutlineColor = new Color(30, 30, 30);
            reloadIcon = new Sprite(sprite);
            reloadIcon.TextureRect = new IntRect(48, 16, 16, 16);
            reloadIcon.Scale = new Vector2f(reloadIcon.Scale.X * 1.5f, reloadIcon.Scale.Y * 1.5f);
            reloadIcon.Position = new Vector2f(206, 360);
        }

        static void OnClose(object sender, EventArgs e)
        {
            RenderWindow window = (RenderWindow)sender;
            window.Close();
        }

        public void LoadData()
        {
            //load tileset
            Image tileSet;
            Texture texture;
            tileSet = new Image("../../../tiles/tileset.png");
            texture = new Texture(tileSet);
            sprite = new Sprite(texture);
            sprite.Scale = new Vector2f(sprite.Scale.X * SCALE, sprite.Scale.Y * SCALE);

            //load font
            font = new Font("../../../tiles/font.ttf");
        }

        public void KeyboardControl(Player player, List<Bullet> bullets)
        {
            //control player
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

            //control pause menu
            if (pressFrequency > 0) pressFrequency--;
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape) && pressFrequency == 0)
            {
                pressFrequency = 20;
                if (!isEndOfGame) isMenuOpen = !isMenuOpen;
            }
            if (Keyboard.IsKeyPressed(Keyboard.Key.Enter) && isEndOfGame && pressFrequency == 0)
            {
                pressFrequency = 20;
                isMenuOpen = !isMenuOpen;
                score = 0;
                world.SetDistance(0);
                bullets.Clear();
                enemies.Clear();
                blackHoles.Clear();
                player.health = 5;
                player.setPos(World.WIDTH * 8 - 25, World.HEIGHT * 16 - 120);
            }
        }

        public void Draw() 
        {
            //set the speed of game
            time = clock.ElapsedTime.AsMicroseconds() / 1500;
            clock.Restart();

            for (int i = 0; i < enemies.Count; i++) window.Draw(enemies[i].Update(time));
            for (int j = 0; j < bullets.Count; j++) window.Draw(bullets[j].Update(time));
            for (int k = 0; k < blackHoles.Count; k++) window.Draw(blackHoles[k].Update(time));
            for (int k = 0; k < boosters.Count; k++) window.Draw(boosters[k].Update(time));

            window.Draw(player.Update(time));
            world.UpdateHealth(window);
            window.Draw(world.UpdateText());  //update distance traveled
        }

        public void Start()
        {
            //game window
            window = new RenderWindow(new VideoMode(World.WIDTH * 16, World.HEIGHT * 16), "Space War!");
            window.Closed += new EventHandler(OnClose);
            window.SetVerticalSyncEnabled(true);

            while (window.IsOpen)
            {
                window.DispatchEvents();
                window.Clear();

                KeyboardControl(player, bullets);
                window.Draw(map);

                if (!isMenuOpen)
                {
                    Draw();

                    //move enemies and bullets and check if hitting
                    for (int i = 0; i < enemies.Count; i++)
                    {
                        if (random.Next(0, 30) == 0) bullets.Add(enemies[i].Fire(new Sprite(sprite)));
                        for (int j = 0; j < bullets.Count && enemies.Count > 0; j++)
                        {
                            if (enemies[i].isHitting(bullets[j]) ||
                                player.isHitting(bullets[j]))
                            {
                                bullets.RemoveAt(j);
                                if (j > 0) --j;
                                if (enemies[i].GetHealth() <= 0 ||
                                    enemies[i].GetPosition().Y > World.HEIGHT * 16)
                                {
                                    enemies.RemoveAt(i);
                                    score++;
                                    if (i > 0) --i;
                                }
                            }
                        } 
                    }

                    for (int j = 0; j < bullets.Count; j++)
                    {
                        if (bullets[j].GetPosition().Y < 0 ||
                            bullets[j].GetPosition().Y > World.HEIGHT * 16)
                        {
                            bullets.RemoveAt(j);
                            if (j > 0) --j;
                        }
                    }

                    //spawn new enemies
                    if (enemySpawnFrequency > 0) enemySpawnFrequency--;
                    if (enemySpawnFrequency == 0)
                    {
                        enemySpawnFrequency = random.Next(55, 100);
                        enemies.Add(new Enemy(new Sprite(sprite), player));
                    }

                    //spawn new black hole
                    if (blackHoleSpawnFrequency > 0) blackHoleSpawnFrequency--;
                    if (blackHoleSpawnFrequency == 0)
                    {
                        blackHoleSpawnFrequency = random.Next(300, 500);
                        blackHoles.Add(new BlackHole(new Sprite(sprite)));
                    }

                    //spawn new black hole
                    if (blackHoleSpawnFrequency > 0) blackHoleSpawnFrequency--;
                    if (blackHoleSpawnFrequency == 0)
                    {
                        boosterSpawnFrequency = random.Next(200, 500);
                        boosters.Add(new Booster(new Sprite(sprite)));
                    }

                    //draw black holes
                    for (int k = 0; k < blackHoles.Count; k++)
                    {
                        if (blackHoles[k].GetPosition().Y > World.HEIGHT * 16 ||
                            player.isBlackHole(blackHoles[k]))
                        {
                            blackHoles.RemoveAt(k);
                            if (k > 0) k--;
                        }
                    }

                    //draw boosters
                    for (int k = 0; k < boosters.Count; k++)
                    {
                        if (boosters[k].GetPosition().Y > World.HEIGHT * 16 ||
                            player.isBooster(boosters[k]))
                        {
                            boosters.RemoveAt(k);
                            if (k > 0) k--;
                        }
                    }

                    if (player.health <= 0) //end of game
                    {
                        isMenuOpen = true;
                        isEndOfGame = true;
                    }
                }
                else
                {
                    window.Draw(menuArea);
                    window.Draw(reloadIcon);
                    Text distance = new Text("0", font, 25)
                    {
                        FillColor = Color.White,
                        Position = new Vector2f(150, 220)
                    };
                    uint dis = (uint)world.GetDistance();
                    distance.DisplayedString = "distance: " + dis + "\n" + "score: " + score + "\n" + "best: " + bestScore + 
                                                "\n\n\n\n\n\n\n\n" + "  Esc - resume\nEnter - restart";
                    window.Draw(distance);
                    if (isEndOfGame && score > bestScore) bestScore = score;                       
                }
                window.Display();
            }
        }
    }
}
