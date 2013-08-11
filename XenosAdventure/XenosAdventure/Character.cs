using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XenosAdventure
{
    class Character
    {
        /// <summary>
        /// Absolute block position
        /// </summary>
        public Vector2 Position { get; set; }
        /// <summary>
        /// The number of large blocks the character occupies
        /// </summary>
        public Vector2 Size { get; set; }
        /// <summary>
        /// Size of the grid
        /// </summary>
        public Vector2 GridSize { get; set; }
        public int[,] Grid;
        public int Rotation { get; set; }

        private Texture2D block;

        private static Color[] colorPalette = { Color.Transparent, Color.Black, new Color(77, 77, 77), new Color(0, 60, 255), new Color(0, 34, 145) };
        private static int blockSize = 8;

        public Character(Texture2D block, Vector2 position)
        {
            this.block = block;
            Position = position;
            Rotation = 0;
            BuildCharacter();
        }

        public Character(Texture2D block, Vector2 position, Vector2 size, Vector2 gridSize, int[,] grid)
            : this(block, position)
        {
            Size = size;
            GridSize = gridSize;
            Grid = grid;
        }

        /// <summary>
        /// Set Size, GridSize, and Grid to work
        /// </summary>
        protected virtual void BuildCharacter()
        {
            Size = Vector2.Zero;
            GridSize = Vector2.Zero;
            Grid = new int[(int)GridSize.X, (int)GridSize.Y];
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 halfScreen, Vector2 centerPosition, int tileSize)
        {
            Vector2 offset = halfScreen + (Position - centerPosition) * tileSize + (Size * tileSize - GridSize * blockSize) / 2;

            int sizeX = (int)Size.X / 2;
            int sizeY = (int)Size.Y / 2;
            switch (Rotation)
            {
                // Right
                case 0:
                    offset -= new Vector2(sizeX, sizeY) * tileSize;
                    break;
                // Down
                case 1:
                    offset -= new Vector2(sizeY, sizeX) * tileSize;
                    break;
                // Left
                case 2:
                    offset -= new Vector2(-sizeX, sizeY) * tileSize;
                    break;
                // Up
                default:
                    offset -= new Vector2(sizeY, -sizeX) * tileSize;
                    break;
            }

            for (int x = 0; x < GridSize.X; x++)
            {
                for (int y = 0; y < GridSize.Y; y++)
                {
                    Vector2 position;
                    switch (Rotation)
                    {
                        case 0:
                            position = offset + new Vector2(x * blockSize, y * blockSize);
                            break;
                        case 1:
                            position = offset + new Vector2(y * blockSize, x * blockSize);
                            break;
                        case 2:
                            position = offset + new Vector2(-x * blockSize, y * blockSize);
                            break;
                        default:
                            position = offset + new Vector2(y * blockSize, -x * blockSize);
                            break;
                    }
                    spriteBatch.Draw(block, new Rectangle((int)position.X, (int)position.Y, blockSize, blockSize), colorPalette[Grid[x, y]]);
                }
            }
        }

        public List<Vector2> GetOccupiedBlocks(Vector2 newPosition, int newRotation)
        {
            Vector2 offset = newPosition;

            int sizeX = (int)Size.X / 2;
            int sizeY = (int)Size.Y / 2;
            switch (newRotation)
            {
                // Right
                case 0:
                    offset -= new Vector2(sizeX, sizeY);
                    break;
                // Down
                case 1:
                    offset -= new Vector2(sizeY, sizeX);
                    break;
                // Left
                case 2:
                    offset -= new Vector2(-sizeX, sizeY);
                    break;
                // Up
                default:
                    offset -= new Vector2(sizeY, -sizeX);
                    break;
            }

            List<Vector2> blocks = new List<Vector2>();
            for (int x = 0; x < Size.X; x++)
            {
                for (int y = 0; y < Size.Y; y++)
                {
                    Vector2 position;
                    switch (newRotation)
                    {
                        case 0:
                            position = offset + new Vector2(x, y);
                            break;
                        case 1:
                            position = offset + new Vector2(y, x);
                            break;
                        case 2:
                            position = offset + new Vector2(-x, y);
                            break;
                        default:
                            position = offset + new Vector2(y, -x);
                            break;
                    }

                    blocks.Add(position);
                }
            }

            return blocks;
        }
    }

    
}
