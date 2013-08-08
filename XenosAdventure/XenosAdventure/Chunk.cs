using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XenosAdventure
{
    class Chunk
    {
        public int[,] Grid { get; protected set; }
        public Point ChunkPosition { get; set; }

        private int chunkSize;
        private Random random;

        public Chunk(Point position)
        {
            ChunkPosition = position;
        }

        public int GetGridValue(Vector2 absolutePosition)
        {
            return Grid[(int)absolutePosition.X - ChunkPosition.X * chunkSize,
                (int)absolutePosition.Y - ChunkPosition.Y * chunkSize];
        }

        public void Generate(List<TileType> tileSetup, Random random, int chunkSize)
        {
            this.chunkSize = chunkSize;
            this.random = random;

            // Calculate tiles based on weights
            int totalWeight = 0;
            foreach (TileType type in tileSetup)
            {
                totalWeight += type.Weight;
            }

            Grid = new int[chunkSize, chunkSize];
            for (int i = 0; i < chunkSize; i++)
            {
                for (int j = 0; j < chunkSize; j++)
                {
                    int rand = random.Next(totalWeight);
                    int weightOffset = 0; //repeatWeightOffset = 0;

                    // Repeat
                    if (i > 0 && random.Next(100) < tileSetup[Grid[i - 1, j]].RepeatWeight)
                    {
                        Grid[i, j] = Grid[i - 1, j];
                    }
                    else if (j > 0 && random.Next(100) < tileSetup[Grid[i, j - 1]].RepeatWeight)
                    {
                        Grid[i, j] = Grid[i, j - 1];
                    }
                    else
                    {
                        for (int typeIndex = 0; typeIndex < tileSetup.Count; typeIndex++)
                        {
                            if (!tileSetup[typeIndex].Flood)
                            {
                                weightOffset += tileSetup[typeIndex].Weight;
                                //repeatWeightOffset += tileSetup[typeIndex].RepeatWeight;
                                if (rand < weightOffset)
                                {
                                    Grid[i, j] = typeIndex;
                                    break;
                                }
                            }
                        }
                    }
                }

            }
            //// Generate floods
            for (int typeIndex = 0; typeIndex < tileSetup.Count; typeIndex++)
            {
                if (tileSetup[typeIndex].Flood)
                {
                    FloodGeneration(typeIndex);
                }
            }

        }


        private void FloodGeneration(int typeIndex)
        {
            int x = random.Next(chunkSize / 4, 3 * chunkSize / 4);
            int y = random.Next(chunkSize / 4, 3 * chunkSize / 4);
            int oldx = x, oldy = y;

            Grid[x, y] = typeIndex;
            int length = 0;
            while (x > 0 && x < chunkSize - 1 && y > 0 && y < chunkSize - 1 && length < 50)
            {
                int rand = random.Next(4);
                // Left
                if (rand == 0 && x > 0 && Grid[x - 1, y] != typeIndex)
                {
                    x -= 1;
                }
                // Up
                else if (rand == 1 && y > 0 && Grid[x, y - 1] != typeIndex)
                {
                    y -= 1;
                }
                // Right
                else if (rand == 2 && Grid[x + 1, y] != typeIndex)
                {
                    x += 1;
                }
                // Down
                else if (rand == 3 && Grid[x, y + 1] != typeIndex)
                {
                    y += 1;
                }
                else if ((x == 0 || Grid[x - 1, y] == typeIndex) &&
                    (Grid[x, y - 1] == typeIndex) &&
                    (Grid[x + 1, y] == typeIndex) &&
                    (Grid[x, y + 1] == typeIndex))
                {
                    x += 1;
                    y += 1;
                }

                if (oldx != x || oldy != y)
                {
                    Grid[x, y] = typeIndex;
                    oldx = x; 
                    oldy = y;
                    length++;
                }

            }
        }

    }


}
