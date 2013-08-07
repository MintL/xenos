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
        public int Range { get; set; }

        public PointLight(Vector2 position, Color color, int range)
        {
            Position = position;
            Color = color;
            Range = range;
        }

        public float CalculatePower(Vector2 position)
        {
            Vector2 diff = Position - position;
            return MathHelper.Clamp(1 / ((float)Math.Pow(diff.Length(), 1.8f)) * Range * 50, 0, 3f);
        }

    }
}
