using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XenosAdventure
{
    class PointLight
    {
        public Vector2 Position { get; set; }
        public Color Color { get; set; }
        public int Power { get; set; }

        public PointLight(Vector2 position, Color color, int power)
        {
            Position = position;
            Color = color;
            Power = power;
        }

        public float CalculatePower(Vector2 position)
        {
            Vector2 diff = Position - position;
            return MathHelper.Clamp(1 / ((float)Math.Pow(diff.Length(), 3f)) * Power, 0, 3f);
        }

    }
}
