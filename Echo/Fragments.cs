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
    class Fragment
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private Texture2D texture;
        private Vector2 pos;
        private Vector2 direction;
        private int r = 1;

        public Fragment(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch, Vector2 pos, Vector2 direction)
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
            if (!map.Collision((int)pos.X, (int)pos.Y, r))
                return false;
            
            pos += direction;
            return true;
        }

        public void Draw()
        {
            _spriteBatch.Begin();
            _spriteBatch.Draw(texture, pos, Color.White);
            _spriteBatch.End();
        }
    }

    internal class Fragments
    {
        private GraphicsDevice _graphicsDevice;
        private SpriteBatch _spriteBatch;

        private HashSet<Fragment> fragments;
        private double speed = 23;

        public int Count { get { return fragments.Count; } }

        public Fragments(GraphicsDevice graphicsDevice, SpriteBatch spriteBatch)
        {
            this._graphicsDevice = graphicsDevice;
            this._spriteBatch = spriteBatch;

            fragments = new HashSet<Fragment>();
        }

        public void add(Vector2 bulletPos)
        {
            var count = 100;
            var pos = new Vector2[count];

            for (int i = 0; i < pos.Length; i++)
                pos[i] = bulletPos;

            var direction = new Vector2[count];
            for (int i = 0; i < direction.Length; i++)
            {
                double angle = i * (2 * Math.PI / (double)direction.Length);

                double dx = speed * Math.Cos(angle);
                double dy = speed * Math.Sin(angle);

                direction[i] = new Vector2((float)dx, (float)dy);
            }
            for (int i = 0; i < count; i++)
                fragments.Add(new Fragment(_graphicsDevice, _spriteBatch, pos[i], direction[i]));
        }

        public void Update(Map map, Player player)
        {
            foreach (Fragment fragment in fragments)
                if (!fragment.Update(map))
                    fragments.Remove(fragment);
        }

        public void Draw()
        {
            foreach (Fragment fragment in fragments)
                fragment.Draw();
        }
    }
}
