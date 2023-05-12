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

        private Vector2 Size;
        private Texture2D rectTexture;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            Size = new Vector2(1420, 850);

            _graphics.PreferredBackBufferWidth = (int)Size.X;
            _graphics.PreferredBackBufferHeight = (int)Size.Y;
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
            var rectangle = new Rectangle(0, 0, (int)Size.X, (int)Size.X);

            Color[] data = new Color[rectangle.Width * rectangle.Height];
            rectTexture = new Texture2D(GraphicsDevice, rectangle.Width, rectangle.Height);

            for (int i = 0; i < data.Length; i++)
                data[i] = new Color(0, 0, 0, 15);

            rectTexture.SetData(data);

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            player.Update(map, bulletManager);
            fragments.Update(map, player);
            bulletManager.Update(map, ref fragments);
            // TODO: Add your update logic here



            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            var color = new Color(0, 0, 0, 255);

            // GraphicsDevice.Clear(color);
            // отрисовка спрайта

            _spriteBatch.Begin();
            _spriteBatch.Draw(rectTexture, new Vector2(0, 0), Color.White);
            _spriteBatch.End();
            

            player.Draw();
            fragments.Draw();
            bulletManager.Draw();

            base.Draw(gameTime);
        }
    }
}