using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace XenosAdventure
{
    class TileMap : DrawableGameComponent
    {
        int width, height;
        Vector2 halfScreen;
        int widthInTiles, heightInTiles;
        int tileSize;
        int chunkSize = 32;
        SpriteBatch spriteBatch;

        List<TileType> tileSetup;
        Random random = new Random();

        //int[,] grid;
        List<Chunk> chunks = new List<Chunk>();

        List<PointLight> lightSources = new List<PointLight>();
        Texture2D tile;
        Player player;
        PointLight playerLight;
        TimeSpan playerMovementTime;

        public TileMap(Game game, List<TileType> tileSetup, int tileSize)
            : base(game)
        {
            width = Game.GraphicsDevice.Viewport.Width;
            height = Game.GraphicsDevice.Viewport.Height;
            halfScreen = new Vector2(width, height) / 2 - Vector2.One * tileSize / 2;

            widthInTiles = width / tileSize + 1;
            heightInTiles = height / tileSize + 1;

            this.tileSetup = tileSetup;
            this.tileSize = tileSize;

            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            Console.WriteLine("XENOS'S ADVENTURE");

            Chunk chunk = new Chunk(new Point(0, 0));
            chunk.Generate(tileSetup, random, chunkSize);
            chunks.Add(chunk);

            chunk = new Chunk(new Point(1, 0));
            chunk.Generate(tileSetup, random, chunkSize);
            chunks.Add(chunk);

            chunk = new Chunk(new Point(-1, 0));
            chunk.Generate(tileSetup, random, chunkSize);
            chunks.Add(chunk);

            //// Calculate tiles based on weights
            //int totalWeight = 0;
            //foreach (TileType type in tileSetup)
            //{
            //    totalWeight += type.Weight;
            //}

            //grid = new int[widthInTiles, heightInTiles];
            //for (int i = 0; i < widthInTiles; i++)
            //{
            //    for (int j = 0; j < heightInTiles; j++)
            //    {
            //        int rand = random.Next(totalWeight);
            //        int weightOffset = 0, repeatWeightOffset = 0;

            //        // Repeat
            //        if (i > 0 && random.Next(100) < tileSetup[grid[i - 1, j]].RepeatWeight)  
            //        {
            //            grid[i, j] = grid[i - 1, j];
            //        }
            //        else if (j > 0 && random.Next(100) < tileSetup[grid[i, j - 1]].RepeatWeight)
            //        {
            //            grid[i, j] = grid[i, j - 1];
            //        }
            //        else
            //        {
            //            for (int typeIndex = 0; typeIndex < tileSetup.Count; typeIndex++)
            //            {
            //                if (!tileSetup[typeIndex].Flood)
            //                {
            //                    weightOffset += tileSetup[typeIndex].Weight;
            //                    repeatWeightOffset += tileSetup[typeIndex].RepeatWeight;
            //                    if (rand < weightOffset)
            //                    {
            //                        grid[i, j] = typeIndex;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //    }
                
            //}

            //// Generate floods
            //for (int typeIndex = 0; typeIndex < tileSetup.Count; typeIndex++)
            //{
            //    if (tileSetup[typeIndex].Flood)
            //    {
            //        FloodGeneration(typeIndex);
            //    }
            //}
        }

        //private void FloodGeneration(int typeIndex)
        //{
        //    int x = 0, y = random.Next(heightInTiles / 4, 3 * heightInTiles / 4);

        //    grid[x, y] = typeIndex;
        //    while (x < widthInTiles - 1 && y < heightInTiles - 1 && y > 0)
        //    {
        //        int rand = random.Next(5);
        //        // Left
        //        if (rand == 0 && x > 0 && grid[x - 1, y] != typeIndex)
        //        {
        //            x -= 1;
        //            grid[x, y] = typeIndex;
        //        }
        //        // Up
        //        else if (rand == 1 && y > 0 && grid[x, y - 1] != typeIndex)
        //        {
        //            y -= 1;
        //            grid[x, y] = typeIndex;
        //        }
        //        // Right
        //        else if (rand == 2 || rand == 3 && grid[x + 1, y] != typeIndex)
        //        {
        //            x += 1;
        //            grid[x, y] = typeIndex;
        //        }
        //        // Down
        //        else if (rand == 4 && grid[x, y + 1] != typeIndex)
        //        {
        //            y += 1;
        //            grid[x, y] = typeIndex;
        //        }
        //        else if ((x == 0 || grid[x - 1, y] == typeIndex) &&
        //            (grid[x, y - 1] == typeIndex) &&
        //            (grid[x + 1, y] == typeIndex) &&
        //            (grid[x, y + 1] == typeIndex))
        //        {
        //            x += 1;
        //            y += 1;
        //            grid[x, y] = typeIndex;
        //        }
                
        //    }
        //}

        protected override void LoadContent()
        {
            tile = Game.Content.Load<Texture2D>("tile");
            //lightSources.Add(new PointLight(new Vector2(width/2, height/2), new Color(.9f, .5f, .5f), 200));
            
            player = new Player();
            playerLight = new PointLight(player.Position, player.LightColor, 50);
            
            lightSources.Add(playerLight);
        }

        public override void Update(GameTime gameTime)
        {
            playerMovementTime += gameTime.ElapsedGameTime;

            if (playerMovementTime.TotalMilliseconds > 150)
            {
                playerMovementTime = TimeSpan.Zero;
                KeyboardState keystate = Keyboard.GetState();

                Vector2 translation = Vector2.Zero;
                if (keystate.IsKeyDown(Keys.Up))// && y > 0 && tileSetup[grid[x, y - 1]].Movable)
                {
                    translation = new Vector2(0, -1);
                }
                else if (keystate.IsKeyDown(Keys.Down))// && y < heightInTiles - 1 && tileSetup[grid[x, y + 1]].Movable)
                {
                    translation = new Vector2(0, 1);
                }
                else if (keystate.IsKeyDown(Keys.Left))// && x > 0 && tileSetup[grid[x - 1, y]].Movable)
                {
                    translation = new Vector2(-1, 0);
                }
                else if (keystate.IsKeyDown(Keys.Right))// && x < widthInTiles - 1 && tileSetup[grid[x + 1, y]].Movable)
                {
                    translation = new Vector2(1, 0);
                }
                Vector2 newPosition = translation + player.Position;
                Chunk chunk = GetChunk(newPosition);
                if (chunk != null && tileSetup[chunk.GetGridValue(newPosition)].Movable)
                {
                    player.Position = newPosition;
                }

                playerLight.Position = player.Position;
            }
        }

        /// <summary>
        /// http://lifc.univ-fcomte.fr/~dedu/projects/bresenham/index.html
        /// </summary>
        /// <param name="lightPosition"></param>
        /// <param name="position"></param>
        /// <returns></returns>
        private bool IsInShadow(Vector2 lightPosition, Vector2 position)
        {
            List<Vector2> tiles = new List<Vector2>();

            #region Algorithm
            int ystep = 1, xstep = 1;
            int error;
            int errorprev;
            int y = (int)lightPosition.Y, x = (int)lightPosition.X;
            int ddy, ddx;
            int dx = (int)position.X - x;
            int dy = (int)position.Y - y;

            if (dy < 0)
            {
                ystep = -1;
                dy = -dy;
            }
            if (dx < 0)
            {
                xstep = -1;
                dx = -dx;
            }

            ddy = 2 * dy;
            ddx = 2 * dx;

            if (ddx >= ddy)
            {
                errorprev = error = dx;
                for (int i = 0; i < dx; i++)
                {
                    x += xstep;
                    error += ddy;
                    if (error > ddx)
                    {
                        y += ystep;
                        error -= ddx;
                        if (error + errorprev < ddx)
                            tiles.Add(new Vector2(x, y - ystep));
                        else if (error + errorprev > ddx)
                            tiles.Add(new Vector2(x - xstep, y));
                        //else
                        //{
                        //    tiles.Add(new Vector2(x, y - ystep));
                        //    tiles.Add(new Vector2(x - xstep, y));
                        //}
                    }
                    tiles.Add(new Vector2(x, y));
                    errorprev = error;
                }
            }
            else
            {
                errorprev = error = dy;
                for (int i = 0; i < dy; i++)
                {
                    y += ystep;
                    error += ddx;
                    if (error > ddy)
                    {
                        x += xstep;
                        error -= ddy;

                        if (error + errorprev < ddy)
                            tiles.Add(new Vector2(x - xstep, y));
                        else if (error + errorprev > ddy)
                            tiles.Add(new Vector2(x, y - ystep));
                        //else
                        //{
                        //    tiles.Add(new Vector2(x - xstep, y));
                        //    tiles.Add(new Vector2(x, y - ystep));
                        //}
                    }
                    tiles.Add(new Vector2(x, y));
                    errorprev = error;
                }
            }
            #endregion

            foreach (var tile in tiles)
            {
                Chunk chunk = GetChunk(tile);
                if (chunk != null)
                {
                    TileType type = tileSetup[chunk.Grid[(int)tile.X - chunk.ChunkPosition.X * chunkSize, (int)tile.Y - chunk.ChunkPosition.Y * chunkSize]];
                    if (!type.Movable && !type.Unshaded)
                        return true;
                }
            }

            return false;
        }

        private Color GetShadedColor(Vector2 position, Color color)
        {
            // Ambient
            Vector3 shadedColor = color.ToVector3() * 0.03f;
            
            // Diffuse
            foreach (PointLight light in lightSources)
            {
                if (!IsInShadow(light.Position, position))
                {
                    shadedColor += color.ToVector3() * light.Color.ToVector3() * light.CalculatePower(position);
                }
            }

            // Bloom
            //int x = (int)position.X / tileSize, y = (int)position.Y / tileSize;
            //if (x > 0 && tileSetup[grid[x - 1, y]].Unshaded)
            //    shadedColor += tileSetup[grid[x - 1, y]].Color.ToVector3() * 0.4f;
            //if (x < widthInTiles - 1 && tileSetup[grid[x + 1, y]].Unshaded)
            //    shadedColor += tileSetup[grid[x + 1, y]].Color.ToVector3() * 0.4f;
            //if (y > 0 && tileSetup[grid[x, y - 1]].Unshaded)
            //    shadedColor += tileSetup[grid[x, y - 1]].Color.ToVector3() * 0.4f;
            //if (y < heightInTiles - 1 && tileSetup[grid[x, y + 1]].Unshaded)
            //    shadedColor += tileSetup[grid[x, y + 1]].Color.ToVector3() * 0.4f;

            return new Color(shadedColor.X, shadedColor.Y, shadedColor.Z);
        }

        private Chunk GetChunk(Vector2 blockPosition)
        {
            return chunks.Find(c => c.ChunkPosition.Equals(GetChunkPosition(blockPosition)));
        }

        private Point GetChunkPosition(Vector2 blockPosition)
        {
            int x, y;
            if (blockPosition.X < 0)
            {
                x = ((int)blockPosition.X + 1) / chunkSize;
                x -= 1;
            }
            else
            {
                x = (int)blockPosition.X / chunkSize;
            }
            if (blockPosition.Y < 0)
            {
                y = ((int)blockPosition.Y + 1) / chunkSize;
                y -= 1;
            }
            else
            {
                y = (int)blockPosition.Y / chunkSize;
            }
            return new Point(x, y);
        }

        public override void Draw(GameTime gameTime)
        {
            Vector2 startBlock = player.Position - new Vector2(widthInTiles, heightInTiles) / 2;
            Vector2 endBlock = player.Position + new Vector2(widthInTiles, heightInTiles) / 2;

            // Find visible chunks
            List<Chunk> visibleChunks = new List<Chunk>();
            Point startChunk = GetChunkPosition(startBlock);
            Point endChunk = GetChunkPosition(endBlock);

            for (int chunkX = startChunk.X; chunkX <= endChunk.X; chunkX++)
            {
                for (int chunkY = startChunk.Y; chunkY <= endChunk.Y; chunkY++)
                {
                    //Console.WriteLine(chunkX + ", " + chunkY);
                    Chunk chunk = chunks.Find(c => c.ChunkPosition.Equals(new Point(chunkX, chunkY)));
                    if (chunk != null)
                    {
                        visibleChunks.Add(chunk);
                    }
                }
            }

            spriteBatch.Begin();
            foreach (var chunk in visibleChunks)
            {
                //Console.WriteLine(chunk.ChunkPosition);
                int[,] grid = chunk.Grid;

                #region FindMinMax
                int xmin, ymin;
                Vector2 A;
                A.X = chunk.ChunkPosition.X * chunkSize;
                A.Y = chunk.ChunkPosition.Y * chunkSize;
                Vector2 B = startBlock;
                if (A.X < B.X)
                    xmin = (int)B.X - (int)A.X;
                else
                    xmin = 0;

                if (A.Y < B.Y)
                    ymin = (int)B.Y - (int)A.Y + 1;
                else
                    ymin = 0;

                int xmax, ymax;
                Vector2 C;
                C.X = chunk.ChunkPosition.X * chunkSize + chunkSize - 1;
                C.Y = chunk.ChunkPosition.Y * chunkSize + chunkSize - 1;
                Vector2 D = endBlock;
                if (C.X > D.X)
                    xmax = (int)D.X - (int)A.X + 1;
                else
                    xmax = chunkSize;

                if (C.Y > D.Y)
                    ymax = (int)D.Y - (int)A.Y + 1;
                else
                    ymax = chunkSize;
                #endregion

                for (int x = xmin; x < xmax; x++)
                {
                    for (int y = ymin; y < ymax; y++)
                    {
                        Vector2 position = new Vector2(x * tileSize, y * tileSize) - player.Position * tileSize + halfScreen +
                            new Vector2(chunk.ChunkPosition.X, chunk.ChunkPosition.Y) * chunkSize * tileSize;
                        if (tileSetup[grid[x, y]].Unshaded)
                        {
                            spriteBatch.Draw(tile, position, tileSetup[grid[x, y]].Color);
                        }
                        else
                        {
                            spriteBatch.Draw(tile, position, GetShadedColor(
                                    new Vector2(x, y) + new Vector2(chunk.ChunkPosition.X, chunk.ChunkPosition.Y) * chunkSize,
                                    tileSetup[grid[x, y]].Color
                            ));
                        }
                    }
                }
            }

            // Player
            spriteBatch.Draw(tile, halfScreen, player.Color);

            spriteBatch.End();
        }
    }

    class TileType
    {
        public Color Color { get; set; }
        public int Weight { get; set; }
        public int RepeatWeight { get; set; }
        public bool Flood { get; set; }
        public bool Movable { get; set; }
        public bool Unshaded { get; set; }

        public TileType(Color color, int weight, int repeatWeight)
        {
            Color = color;
            Weight = weight;
            RepeatWeight = repeatWeight;
            Flood = false;
            Movable = false;
            Unshaded = false;
        }
    }

}
