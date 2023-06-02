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
            camera = new Camera(new Vector2(300, 300));
            player = new Player(Content, Global.team, new Vector2(500, 500));
            map = new Map();
            
            BotManager.add(Content, "Red", new Vector2(1850, 1100));
            /*
            BotManager.add(Content, "Red", new Vector2(2550, 350));
            BotManager.add(Content, "Red", new Vector2(2550, 350));
            */
            BotManager.add(Content, "Red", new Vector2(2950, 2300));
            BotManager.add(Content, "Red", new Vector2(2950, 2300));

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

                if (Global.state != "game" && Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    Uninitialize();
                    Initialize();
                }

                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(6, 6, 6, 255));

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