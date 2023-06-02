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
    internal class Bot : Player
    {
        private Vector2 direction;
        private int r = 10;
        public bool MustDraw = false;

        public Bot(ContentManager content, string team, Vector2 pos) : base(content, team, pos)
        {
            if (team != Global.team)
                base.texture = content.Load<Texture2D>("opponent");
        }

        public bool Collision(Vector2 bulletPos)
        {
            var dif = bulletPos - pos;
            if (dif.Length() <= size.X)
                return true;

            return false;
        }

        private void Control(GraphicsDevice gd, Camera camera)
        {
            Random rnd = new Random();
            if (rnd.Next(1, 25) == 5)
                speed = new Vector2(rnd.Next(-3, 4), rnd.Next(-3, 4));

            if (base.lastShoot.AddSeconds(base.recharge) < DateTime.Now)
            {
                base.lastShoot = DateTime.Now;
                if (rnd.Next(1, 6) == 5)
                {
                    var dir = camera.position - pos;
                    BulletManager.add(gd, pos, dir / dir.Length(), Team, id);
                    MustDraw = true;
                }
            }
        }

        public bool Update(GraphicsDevice gd, Map map, Camera camera)
        {
            this.Control(gd, camera);

            if (map.Collision((int)(pos.X + speed.X), (int)(pos.Y + speed.Y), 32))
            {
                pos += speed;
            }

            return true;
        }
    }

    internal static class BotManager
    {
        public static HashSet<Bot> bots = new HashSet<Bot>();
        private static double speed = 23;

        public static int Count { get { return bots.Count; } }

        public static void add(ContentManager content, string team, Vector2 pos)
        {
            bots.Add(new Bot(content, team, pos));
        }

        public static bool BulletCollision(Vector2 position, string team)
        {
            foreach (Bot bot in bots)
                if (bot.Collision(position) && bot.Team != team)
                {
                    bot.XP--;
                    if (bot.XP == 0)
                    {
                        bots.Remove(bot);
                        if (bots.Count == 0)
                            Global.state = "youWin";
                    }

                    return true;
                }

            return false;
        }

        public static bool FragmentCollision(Vector2 position)
        {
            foreach (Bot bot in bots)
                if (bot.Collision(position))
                {
                    bot.MustDraw = true;
                    return true;
                }   

            return false;
        }

        public static void Update(GraphicsDevice gd, Map map, Camera camera)
        {
            foreach (Bot bot in bots)
                bot.Update(gd, map, camera);
        }

        public static void Draw()
        {
            foreach (Bot bot in bots)
                if (bot.Team == Global.team)
                    bot.Draw();
                else
                    if (bot.MustDraw)
                    {
                        bot.Draw();
                        bot.MustDraw = false;
                    }
        }

        public static void Uninitialize()
        {
            bots.Clear();
        }
    }
}
