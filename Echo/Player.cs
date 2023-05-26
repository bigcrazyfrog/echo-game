﻿using System;
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
    public class Player
    {
        public Texture2D texture;
        public Vector2 size = new Vector2(50, 50);

        public string Team;
        public int id;

        public int XP;

        public Vector2 pos;
        public Vector2 speed;

        private float speedMax;

        public Player(ContentManager content, string team)
        {
            this.Team = team;

            Random rnd = new Random();
            this.id = rnd.Next(1024);
            XP = 3;

            texture = content.Load<Texture2D>("player");

            pos = new Vector2(500, 500);
            speed = new Vector2(0, 0);
            speedMax = 10;
        }

        private void Control(GraphicsDevice gd, Camera camera)
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
                BulletManager.add(gd, pos + size / 2,
                                  dir / dir.Length(), Team, id);
            }
        }

        public bool Update(GraphicsDevice gd, Map map, Camera camera)
        {
            this.Control(gd, camera);

            if (map.Collision((int)(pos.X + speed.X + 10), (int)(pos.Y + 10), (int)size.X / 2)) {
                pos += new Vector2(speed.X, 0);
            }

            if (map.Collision((int)(pos.X) + 10, (int)(pos.Y + speed.Y) + 10, (int)size.X / 2))
            {
                pos += new Vector2(0, speed.Y);
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

            return true;
        }

        public void Draw()
        {
            //_spriteBatch.Draw(lastTexture, lastPos, Color.White);
            Global._spriteBatch.Draw(texture, pos, Color.White);
        }
    }
}
