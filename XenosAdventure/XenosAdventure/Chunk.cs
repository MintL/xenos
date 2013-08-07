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

        public Chunk(Point position)
        {
            ChunkPosition = position;
        }

        public void Generate(List<TileType> tileSetup, Random random, int chunkSize)
        {
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
        }
    }
}
