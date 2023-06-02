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
    public class Bullet
    {
        private Texture2D texture;
        public Vector2 pos;
        private Vector2 direction;

        private int r = 10;
        public int id;

        public string Team;

        public Bullet(GraphicsDevice gd, Vector2 pos, Vector2 direction, string team, int id)
        {
            this.pos = pos;
            this.direction = direction;

            this.Team = team;
            this.id = id;

            var rectangle = new Rectangle(0, 0, r, r);

            Color[] data = new Color[rectangle.Width * rectangle.Height];
            texture = new Texture2D(gd, rectangle.Width, rectangle.Height);

            Color color = new Color(255, 255, 255);
            if (team != Global.team)
                color = new Color(255, 0, 0);

            for (int i = 0; i < data.Length; ++i)
                data[i] = color;

            texture.SetData(data);
        }

        public bool Update(Map map, Player player)
        {
            if (BotManager.BulletCollision(pos, Team))
                return false;

            if (Team != Global.team && player.BulletCollision(pos))
                return false;

            if (!map.Collision((int)pos.X + (int)direction.X, (int)pos.Y + (int)direction.Y, r))
                return false;

            pos += direction;
            return true;
        }

        public void Draw()
        {
            Global._spriteBatch.Draw(texture, pos, Color.White);
        }
    }

    public static class BulletManager
    {
        private static HashSet<Bullet> bullets = new HashSet<Bullet>();

        private static int speed = 10;

        public static int Count { get { return bullets.Count; } }

        public static void add(GraphicsDevice gd, Vector2 pos, Vector2 direction, string team, int id)
        {
            bullets.Add(new Bullet(gd, pos, direction * speed, team, id));
        }

        public static void Update(GraphicsDevice gd, Map map, Player player)
        {
            foreach (Bullet bullet in bullets)
                if (!bullet.Update(map, player))
                {
                    if (bullet.Team == Global.team)
                        FragmentManager.add(gd, bullet.pos);
                    bullets.Remove(bullet);
                }
        }

        public static void Draw()
        {
            foreach (Bullet bullet in bullets)
                bullet.Draw();
        }

        public static void Uninitialize()
        {
            bullets = new HashSet<Bullet>();
        }
    }
}
