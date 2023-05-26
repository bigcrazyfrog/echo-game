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
    public class Circle
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

    public class RectangleMap
    {
        public Rectangle rect;

        public RectangleMap(Vector2 pos, Vector2 size)
        {
            rect = new Rectangle((int)pos.X, (int)pos.Y, (int)size.X, (int)size.Y);
        }

        public bool Collision(Rectangle other)
        {
            return rect.Intersects(other);
        }

        public bool Collision(Circle circle)
        {
            return rect.Contains(circle.x - circle.radius, circle.y) &&
                   rect.Contains(circle.x, circle.y - circle.radius) &&
                   rect.Contains(circle.x + circle.radius, circle.y) &&
                   rect.Contains(circle.x, circle.y + circle.radius);
        }
    }

    public class Map
    {
        private bool[,] map;

        private int Widht = 10;
        private int Height = 10;
        private int Size = 10;

        private Circle[] circles;
        private RectangleMap[] rects;

        private Point size;

        public Map()
        {
            size = new Point();
            map = new bool[Widht, Height];

            circles = new Circle[1];
            circles[0] = new Circle(500, 500, 300);
            /*
            circles = new Circle[2];
            circles[0] = new Circle(500, 500, 300);
            circles[1] = new Circle(900, 500, 200);
            
            rects = new RectangleMap[1];
            rects[0] = new RectangleMap(new Vector2(900, 500), new Vector2(500, 500));*/
            Generate();
        }

        private void Generate()
        {
            rects = new RectangleMap[5];
            rects[0] = new RectangleMap(new Vector2(250, 400), new Vector2(5000, 200));
            for (int i = 1; i < rects.Length; i++)
                rects[i] = new RectangleMap(new Vector2(200 + i * 1000, 200), new Vector2(600, 600));
        }

        public bool Collision(int x, int y, int r) {
            var c = new Circle(x + r / 2, y + r / 2, r);
            for (int i = 0; i < circles.Length; i++)
                if (circles[i].Collision(c))
                    return true;
            
            for (int i = 0; i < rects.Length; i++)
                if (rects[i].Collision(c))
                    return true;

            return false;
        }
    }
}
