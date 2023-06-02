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
        private Circle[] circles;
        private RectangleMap[] rects;

        public Map()
        {
            Generate();
        }

        private void Generate()
        {
            circles = new Circle[1];
            circles[0] = new Circle(500, 500, 300);

            rects = new RectangleMap[11];
            rects[0] = new RectangleMap(new Vector2(250,  400),  new Vector2(1200, 200));
            rects[1] = new RectangleMap(new Vector2(1250, 400),  new Vector2(200,  1000));
            rects[2] = new RectangleMap(new Vector2(1250, 1200), new Vector2(2100, 200));
            rects[3] = new RectangleMap(new Vector2(1750, 1000), new Vector2(700,  700));
            rects[4] = new RectangleMap(new Vector2(1750, 400),  new Vector2(200,  2000));
            rects[5] = new RectangleMap(new Vector2(1750, 250),  new Vector2(800, 200));
            rects[6] = new RectangleMap(new Vector2(2450, 250),  new Vector2(700, 500));
            rects[7] = new RectangleMap(new Vector2(2650, 400),  new Vector2(200, 1000));
            rects[8] = new RectangleMap(new Vector2(1750, 2300), new Vector2(1000, 200));
            rects[9] = new RectangleMap(new Vector2(2700, 2100), new Vector2(1000, 700));

            rects[10] = new RectangleMap(new Vector2(3300, 1200), new Vector2(200, 1600));
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
