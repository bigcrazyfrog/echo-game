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
    public static class MouseManager
    {
        public static bool LeftClicked = false;

        private static MouseState ms = new MouseState(), oms;

        public static Vector2 MousePosition = new Vector2();

        public static void Update()
        {
            oms = ms;
            ms = Mouse.GetState();
            LeftClicked = ms.LeftButton != ButtonState.Pressed && oms.LeftButton == ButtonState.Pressed;

            if (LeftClicked) MousePosition = new Vector2(ms.X, ms.Y) ;
        }
    }
}
