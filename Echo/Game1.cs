using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

// mgcb-editor

namespace Echo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Player player;
        private Map map;
        private Fragments fragments;
        private BulletManager bulletManager;

        private Texture2D rectTexture;
        private Camera camera;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            camera = new Camera(new Vector2(300, 300));

            _graphics.PreferredBackBufferWidth = (int)Global.Screen.X;
            _graphics.PreferredBackBufferHeight = (int)Global.Screen.Y;
            _graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            player = new Player(Content, GraphicsDevice, _spriteBatch);
            map = new Map();
            fragments = new Fragments(GraphicsDevice, _spriteBatch);
            bulletManager = new BulletManager(GraphicsDevice, _spriteBatch);

            // TODO: use this.Content to load your game content here
            var rectangle = new Rectangle(0, 0, (int)Global.Screen.X, (int)Global.Screen.Y);

            Color[] data = new Color[rectangle.Width * rectangle.Height];
            rectTexture = new Texture2D(GraphicsDevice, rectangle.Width, rectangle.Height);

            for (int i = 0; i < data.Length; i++)
                data[i] = new Color(0, 0, 0, 20);

            rectTexture.SetData(data);
            var color = new Color(6, 6, 6, 255);

            GraphicsDevice.Clear(color);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            MouseManager.Update();
            KeyboardManager.Update();

            camera.Update(player.pos);

            player.Update(map, bulletManager, camera);
            fragments.Update(map, player);
            bulletManager.Update(map, ref fragments);
            // TODO: Add your update logic here



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var color = new Color(6, 6, 6, 255);

            // GraphicsDevice.Clear(color);
            // отрисовка спрайта
            _spriteBatch.Begin();
            _spriteBatch.Draw(rectTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.End();

            _spriteBatch.Begin(transformMatrix: camera.transform);
            fragments.Draw();
            bulletManager.Draw();
            _spriteBatch.End();

            _spriteBatch.Begin();
            player.Draw();
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }

    public static class Global
    {
        public static readonly Vector2 Screen = new Vector2(1400, 850);

    }
}