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
    public class Camera
    {
        public Vector2 position;
        public Matrix transform { get; private set; }
        public float delay { get; set; } = 0.1f;

        public float Zoom { get; private set; } = 1.0f;

        public Camera(Vector2 pos)
        {
            this.position = pos;
        }

        public void Update(Vector2 pos)
        {
            float d = delay;

            position = position + (pos - position) * d;

            transform = Matrix.CreateTranslation(((int)-position.X ) + Global.Screen.X / 2,
                                                 ((int)-position.Y ) + Global.Screen.Y / 2, 0) *
                                                 Matrix.CreateScale(new Vector3(Zoom, Zoom, 1));
        }
    }
}
