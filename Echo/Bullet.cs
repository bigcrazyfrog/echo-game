using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Echo
{
    class Bullet
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        public Vector2 pos;
        private Vector2 direction;
        private int r = 10;

        public Bullet(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 pos, Vector2 direction)
        {
            this._graphicsDevice = graphicsDevice;
            this._spriteBatch = spriteBatch;

            this.pos = pos;
            this.direction = direction;

            var rectangle = new Rectangle(0, 0, r, r);

            Color[] data = new Color[rectangle.Width * rectangle.Height];
            texture = new Texture2D(graphicsDevice, rectangle.Width, rectangle.Height);

            for (int i = 0; i < data.Length; ++i)
                data[i] = new Color(255, 255, 255);

            texture.SetData(data);
        }

        public bool Update(Map map)
        {
            if (!map.Collision((int)pos.X + (int)direction.X, (int)pos.Y + (int)direction.Y, r))
                return false;

            pos += direction;
            return true;
        }

        public void Draw()
        {
            _spriteBatch.Draw(texture, pos, Color.White);
        }
    }

    internal class BulletManager
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private HashSet<Bullet> bullets;

        private int speed = 10;

        public int Count { get { return bullets.Count; } }

        public BulletManager(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this._graphicsDevice = graphicsDevice;
            this._spriteBatch = spriteBatch;

            bullets = new HashSet<Bullet>();
        }

        public void add(Vector2 pos, Vector2 direction)
        {           
             bullets.Add(new Bullet(_graphicsDevice, _spriteBatch, pos, direction * speed));
        }

        public void Update(Map map, ref Fragments fragments)
        {
            foreach (Bullet bullet in bullets)
                if (!bullet.Update(map))
                {
                    fragments.add(bullet.pos);
                    bullets.Remove(bullet);
                }
        }

        public void Draw()
        {
            foreach (Bullet bullet in bullets)
                bullet.Draw();
        }
    }
}
