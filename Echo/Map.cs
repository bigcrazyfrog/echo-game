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
    class Circle
    {
        public int radius { get; private set; }
        public int x { get; private set; }
        public int y { get; private set; }

        public Circle(int x, int y, int r)
        {
            this.x = x;
            this.y = y;
            this.radius = r;
        }

        public bool Collision(Circle other)
        {
            var dX = x - other.x;
            var dY = y - other.y;

            var len = Math.Sqrt(dX*dX + dY*dY);

            return len <= radius - other.radius;
        }
    }

    internal class Map
    {
        private bool[,] map;

        private int Widht = 10;
        private int Height = 10;
        private int Size = 10;

        private Circle[] circle;

        private Point size;

        public Map()
        {
            size = new Point();
            map = new bool[Widht, Height];

            circle = new Circle[2];
            circle[0] = new Circle(500, 500, 400);
            circle[1] = new Circle(1100, 500, 300);
        }


        public bool Collision(int x, int y, int r) {
            var c = new Circle(x + r / 2, y + r / 2, r);
            for (int i = 0; i < circle.Length; i++)
                if (circle[i].Collision(c))
                    return true;
            return false;
        }
    }
}
