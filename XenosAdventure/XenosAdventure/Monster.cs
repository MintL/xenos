using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace XenosAdventure
{
    class Monster : Character
    {
        public Monster(Texture2D block)
            : base(block, new Vector2(5, 5))
        {
        }

        protected override void BuildCharacter()
        {
            Size = new Vector2(3, 2);
            GridSize = new Vector2(12, 8);
            Grid = new int[,] 
            { 
                { 0, 1, 1, 0, 0, 1, 1, 0 }, 
                { 1, 2, 0, 0, 0, 0, 2, 1 }, 
                { 0, 0, 1, 0, 0, 1, 0, 0 }, 
                { 0, 0, 0, 2, 2, 0, 0, 0 },
                { 0, 0, 2, 1, 1, 2, 0, 0 },
                { 0, 2, 1, 1, 1, 1, 2, 0 },
                { 0, 2, 1, 1, 1, 1, 2, 0 },
                { 0, 0, 1, 3, 3, 1, 0, 0 },
                { 0, 1, 3, 4, 4, 3, 1, 0 },
                { 0, 1, 3, 4, 4, 3, 1, 0 },
                { 0, 0, 1, 3, 3, 1, 0, 0 },
                { 0, 0, 0, 1, 1, 0, 0, 0 } 
            };
        }

    }
}
