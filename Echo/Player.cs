using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Echo
{
    internal class Player
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private Texture2D lastTexture;
        private Vector2 size = new Vector2(64, 64);

        public Vector2 pos;
        private Vector2 lastPos;
        private Vector2 speed;

        private float speedMax;

        public Player(ContentManager content, GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this._graphicsDevice = graphicsDevice;
            this._spriteBatch = spriteBatch;

            texture = content.Load<Texture2D>("player");
            lastTexture = content.Load<Texture2D>("last_player");

            pos = new Vector2(500, 500);
            speed = new Vector2(0, 0);
            speedMax = 10;
        }

        private void Control(BulletManager bulletManager, Camera camera)
        {
            if (KeyboardManager.State.IsKeyDown(Keys.Left))
                speed.X = -speedMax;
            if (KeyboardManager.State.IsKeyDown(Keys.Right))
                speed.X = speedMax;
            if (KeyboardManager.State.IsKeyDown(Keys.Up))
                speed.Y = -speedMax;
            if (KeyboardManager.State.IsKeyDown(Keys.Down))
                speed.Y = speedMax;

            if (MouseManager.LeftClicked)
            {
                Vector2 dir = MouseManager.MousePosition - Global.Screen / 2 - size / 2;
                bulletManager.add(camera.position + size / 2,
                                  dir / dir.Length());
            }
        }

        public void Update(Map map, BulletManager bulletManager, Camera camera)
        {
            this.Control(bulletManager, camera);

            lastPos = pos;

            if (map.Collision((int)(pos.X + speed.X), (int)(pos.Y + speed.Y), 32)) {
                pos += speed;
            }

            if (speed.X != 0)
                if (speed.X > 0)
                    speed.X -= 1;
                else 
                    speed.X += 1;

            if (speed.Y != 0)
                if (speed.Y > 0)
                    speed.Y -= 1;
                else
                    speed.Y += 1;
        }

        public void Draw()
        {
            //_spriteBatch.Draw(lastTexture, lastPos, Color.White);
            _spriteBatch.Draw(texture, Global.Screen / 2, Color.White);
        }
    }
}
