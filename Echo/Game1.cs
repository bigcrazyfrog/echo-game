using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// mgcb-editor

namespace Echo
{
    public class Game1 : Game
    {
        private Player player;
        private Map map;

        private Texture2D rectTexture;
        private Texture2D gameOverTexture;
        private Texture2D youWinTexture;
        private Camera camera;

        public Game1()
        {
            Global._graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            camera = new Camera(new Vector2(300, 300));

            Global._graphics.PreferredBackBufferWidth = (int)Global.Screen.X;
            Global._graphics.PreferredBackBufferHeight = (int)Global.Screen.Y;
            Global._graphics.ApplyChanges();

            Global._spriteBatch = new SpriteBatch(GraphicsDevice);

            Global.team = "white";
            Global.state = "game";

            base.Initialize();
        }

        protected override void LoadContent()
        {
            player = new Player(Content, Global.team, new Vector2(500, 500));
            map = new Map();
            
            //BotManager.add(Content, "white");
            BotManager.add(Content, "Red", new Vector2(1000, 500));

            // TODO: use this.Content to load your game content here
            var rectangle = new Rectangle(0, 0, (int)Global.Screen.X, (int)Global.Screen.Y);

            Color[] data = new Color[rectangle.Width * rectangle.Height];
            rectTexture = new Texture2D(GraphicsDevice, rectangle.Width, rectangle.Height);

            for (int i = 0; i < data.Length; i++)
                data[i] = new Color(0, 0, 0, 20);

            rectTexture.SetData(data);
            var color = new Color(6, 6, 6, 255);

            gameOverTexture = Content.Load<Texture2D>("game_over");
            youWinTexture = Content.Load<Texture2D>("you_win");

            GraphicsDevice.Clear(color);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

                MouseManager.Update();
                KeyboardManager.Update();

                BotManager.Update(GraphicsDevice, map, camera);
                if (Global.state == "game")
                {
                    camera.Update(player.pos);

                    player.Update(GraphicsDevice, map, camera);
                }
                FragmentManager.Update(map, player);
                BulletManager.Update(GraphicsDevice, map, player);

                if (Global.state != "game")
                    if (Keyboard.GetState().IsKeyDown(Keys.Space)) 
                    {
                        Uninitialize();
                        Initialize();

                    }
                // TODO: Add your update logic here

                base.Update(gameTime);
            
        }

        protected override void Draw(GameTime gameTime)
        {
            var color = new Color(6, 6, 6, 255);

            GraphicsDevice.Clear(color);
            // отрисовка спрайта
            /*
            Global._spriteBatch.Begin();
            Global._spriteBatch.Draw(rectTexture, new Vector2(0, 0), Color.White);
            Global._spriteBatch.End();*/
            Global._spriteBatch.Begin(transformMatrix: camera.transform);

            
            FragmentManager.Draw();
            BulletManager.Draw();
            BotManager.Draw();
            if (Global.state == "game")
                player.Draw();
            Global._spriteBatch.End();

            if (Global.state == "gameOver")
            {
                Global._spriteBatch.Begin();
                Global._spriteBatch.Draw(gameOverTexture,
                                         new Vector2(Global.Screen.X / 2 - gameOverTexture.Width / 2,
                                                     Global.Screen.Y / 2 - gameOverTexture.Height / 2), 
                                         Color.White);
                Global._spriteBatch.End();
            }

            if (Global.state == "youWin")
            {
                Global._spriteBatch.Begin();
                Global._spriteBatch.Draw(youWinTexture,
                                         new Vector2(Global.Screen.X / 2 - youWinTexture.Width / 2,
                                                     Global.Screen.Y / 2 - youWinTexture.Height / 2),
                                         Color.White);
                Global._spriteBatch.End();
            }

            base.Draw(gameTime);
        }

        protected void Uninitialize()
        {
            BulletManager.Uninitialize();
            FragmentManager.Uninitialize();
            BotManager.Uninitialize();
          }
     }

    public static class Global
    {
        public static Vector2 Screen = new Vector2(1400, 850);
        public static GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;

        public static string state = "game";

        public static string team;
    }
}