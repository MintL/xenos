using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XenosAdventure
{
    class Player : Character
    {
        public Color Color { get; set; }
        public Color LightColor { get; set; }

        public Player(Texture2D block)
            : base(block, new Vector2(5, 5))
        {
            Color = Color.Red;
            LightColor = Color.White;
        }

        protected override void BuildCharacter()
        {
            Size = new Vector2(1, 1);
            GridSize = new Vector2(4, 4);
            Grid = new int[,] 
            { 
                { 3, 3, 3, 3 }, 
                { 3, 3, 3, 3 }, 
                { 3, 3, 3, 3 }, 
                { 3, 3, 3, 3 }
            };
            
        }
    }
}
