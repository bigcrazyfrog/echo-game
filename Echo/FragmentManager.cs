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
    public class Fragment
    {
        private Texture2D texture;
        private Vector2 pos;
        private Vector2 direction;
        private int r = 1;
        private DateTime createdAt;
        private double lifeTime = 10.0d;

        public Fragment(GraphicsDevice gd, Vector2 pos, Vector2 direction)
        {
            this.pos = pos;
            this.direction = direction;

            this.createdAt = DateTime.Now;

            var rectangle = new Rectangle(0, 0, r, r);

            Color[] data = new Color[rectangle.Width * rectangle.Height];
            texture = new Texture2D(gd, rectangle.Width, rectangle.Height);

            for (int i = 0; i < data.Length; ++i)
                data[i] = new Color(255, 255, 255);

            texture.SetData(data);
        }

        public bool Update(Map map)
        {
            if (BotManager.FragmentCollision(pos))
                return false;

            if (createdAt.AddSeconds(lifeTime) < DateTime.Now)
                return false;

            if (!map.Collision((int)pos.X + (int)direction.X, (int)pos.Y + (int)direction.Y, r))
            {
                for (float i = 0.0f, j = 0.0f; 
                            Math.Abs(i) < Math.Abs(direction.X) && Math.Abs(j) < Math.Abs(direction.Y); 
                            i += direction.X / 20, j += direction.Y / 20)
                    if (!map.Collision((int)pos.X + (int)i, (int)pos.Y + (int)j, r))
                    {
                        pos += new Vector2(i, j);
                        return true;
                    }

                return false;
            }
            
            pos += direction;
            return true;
        }

        public void Draw()
        {
            Global._spriteBatch.Draw(texture, pos, Color.White);
        }
    }

    public static class FragmentManager
    {
        private static HashSet<Fragment> fragments = new HashSet<Fragment>();
        private static double speed = 23;

        public static int Count { get { return fragments.Count; } }

        public static void add(GraphicsDevice gd, Vector2 bulletPos)
        {
            var count = 200;
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
                fragments.Add(new Fragment(gd, pos[i], direction[i]));
        }

        public static void Update(Map map, Player player)
        {
            foreach (Fragment fragment in fragments)
                if (!fragment.Update(map))
                    fragments.Remove(fragment);
        }

        public static void Draw()
        {
            foreach (Fragment fragment in fragments)
                fragment.Draw();
        }

        public static void Uninitialize()
        {
            fragments = new HashSet<Fragment>();
        }
    }
}
