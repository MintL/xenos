using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XenosAdventure
{
    class Player
    {
        public Vector2 Position { get; protected set; }
        public Color Color { get; set; }
        public Color LightColor { get; set; }

        public Player()
        {
            Position = new Vector2(0, 0);
            //Color = new Color(237, 236, 152);
            Color = Color.Red;
            LightColor = Color.White;
        }


        public void Move(Vector2 offset)
        {
            Position += offset; 
        }
    }
}
