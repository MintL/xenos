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
        int widthInTiles, heightInTiles;
        int tileSize;
        SpriteBatch spriteBatch;

        List<TileType> tileSetup;
        Random random = new Random();

        int[,] grid;
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

            widthInTiles = width / tileSize + 1;
            heightInTiles = height / tileSize + 1;

            this.tileSetup = tileSetup;
            this.tileSize = tileSize;

            spriteBatch = (SpriteBatch)Game.Services.GetService(typeof(SpriteBatch));

            Console.WriteLine("XENOS'S ADVENTURE");
            

            // Calculate tiles based on weights
            int totalWeight = 0;
            foreach (TileType type in tileSetup)
            {
                totalWeight += type.Weight;
            }

            grid = new int[widthInTiles, heightInTiles];
            for (int i = 0; i < widthInTiles; i++)
            {
                for (int j = 0; j < heightInTiles; j++)
                {
                    int rand = random.Next(totalWeight);
                    int weightOffset = 0, repeatWeightOffset = 0;

                    // Repeat
                    if (i > 0 && random.Next(100) < tileSetup[grid[i - 1, j]].RepeatWeight)  
                    {
                        grid[i, j] = grid[i - 1, j];
                    }
                    else if (j > 0 && random.Next(100) < tileSetup[grid[i, j - 1]].RepeatWeight)
                    {
                        grid[i, j] = grid[i, j - 1];
                    }
                    else
                    {
                        for (int typeIndex = 0; typeIndex < tileSetup.Count; typeIndex++)
                        {
                            if (!tileSetup[typeIndex].Flood)
                            {
                                weightOffset += tileSetup[typeIndex].Weight;
                                repeatWeightOffset += tileSetup[typeIndex].RepeatWeight;
                                if (rand < weightOffset)
                                {
                                    grid[i, j] = typeIndex;
                                    break;
                                }
                            }
                        }
                    }
                }
                
            }

            // Generate floods
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
            int x = 0, y = random.Next(heightInTiles / 4, 3 * heightInTiles / 4);

            grid[x, y] = typeIndex;
            while (x < widthInTiles - 1 && y < heightInTiles - 1 && y > 0)
            {
                int rand = random.Next(5);
                // Left
                if (rand == 0 && x > 0 && grid[x - 1, y] != typeIndex)
                {
                    x -= 1;
                    grid[x, y] = typeIndex;
                }
                // Up
                else if (rand == 1 && y > 0 && grid[x, y - 1] != typeIndex)
                {
                    y -= 1;
                    grid[x, y] = typeIndex;
                }
                // Right
                else if (rand == 2 || rand == 3 && grid[x + 1, y] != typeIndex)
                {
                    x += 1;
                    grid[x, y] = typeIndex;
                }
                // Down
                else if (rand == 4 && grid[x, y + 1] != typeIndex)
                {
                    y += 1;
                    grid[x, y] = typeIndex;
                }
                else if ((x == 0 || grid[x - 1, y] == typeIndex) &&
                    (grid[x, y - 1] == typeIndex) &&
                    (grid[x + 1, y] == typeIndex) &&
                    (grid[x, y + 1] == typeIndex))
                {
                    x += 1;
                    y += 1;
                    grid[x, y] = typeIndex;
                }
                //Console.WriteLine(rand + ", " + x + ", " + y);
                //Console.WriteLine("tile: " + grid[x, y - 1] + ", " + grid[x + 1, y] + ", " + grid[x, y + 1]);
                
            }
        }

        protected override void LoadContent()
        {
            tile = Game.Content.Load<Texture2D>("tile");
            lightSources.Add(new PointLight(new Vector2(width/2, height/2), new Color(.9f, .5f, .5f), 200));
            
            player = new Player();
            playerLight = new PointLight(player.Position * tileSize + Vector2.One * (tileSize / 2), player.LightColor, 50);
            
            lightSources.Add(playerLight);
        }

        public override void Update(GameTime gameTime)
        {
            playerLight.Position = player.Position * tileSize + Vector2.One * (tileSize / 2);

            playerMovementTime += gameTime.ElapsedGameTime;
            
            if (playerMovementTime.TotalMilliseconds > 150)
            {
                playerMovementTime = TimeSpan.Zero;

                int x = (int)player.Position.X, y = (int)player.Position.Y;
                KeyboardState keystate = Keyboard.GetState();
                if (keystate.IsKeyDown(Keys.Up) && y > 0 && tileSetup[grid[x, y - 1]].Movable)
                {
                    player.Move(new Vector2(0, -1));
                }
                else if (keystate.IsKeyDown(Keys.Down) && y < heightInTiles - 1 && tileSetup[grid[x, y + 1]].Movable)
                {
                    player.Move(new Vector2(0, 1));
                }
                else if (keystate.IsKeyDown(Keys.Left) && x > 0 && tileSetup[grid[x - 1, y]].Movable)
                {
                    player.Move(new Vector2(-1, 0));
                }
                else if (keystate.IsKeyDown(Keys.Right) && x < widthInTiles - 1 && tileSetup[grid[x + 1, y]].Movable)
                {
                    player.Move(new Vector2(1, 0));
                }
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

            foreach (var tile in tiles)
            {
                TileType type = tileSetup[grid[(int)tile.X / tileSize, (int)tile.Y / tileSize]];
                if (!type.Movable && !type.Unshaded) 
                    return true;
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
                Vector2 lightDiff = position - light.Position;
                if (!IsInShadow(light.Position, position))
                {
                    shadedColor += color.ToVector3() * light.Color.ToVector3() * light.CalculatePower(position);
                }
            }

            // Bloom
            int x = (int)position.X / tileSize, y = (int)position.Y / tileSize;
            if (x > 0 && tileSetup[grid[x - 1, y]].Unshaded)
                shadedColor += tileSetup[grid[x - 1, y]].Color.ToVector3() * 0.4f;
            if (x < widthInTiles - 1 && tileSetup[grid[x + 1, y]].Unshaded)
                shadedColor += tileSetup[grid[x + 1, y]].Color.ToVector3() * 0.4f;
            if (y > 0 && tileSetup[grid[x, y - 1]].Unshaded)
                shadedColor += tileSetup[grid[x, y - 1]].Color.ToVector3() * 0.4f;
            if (y < heightInTiles - 1 && tileSetup[grid[x, y + 1]].Unshaded)
                shadedColor += tileSetup[grid[x, y + 1]].Color.ToVector3() * 0.4f;

            return new Color(shadedColor.X, shadedColor.Y, shadedColor.Z);
        }

        ///// <summary>
        ///// Helper for the bloom function
        ///// </summary>
        ///// <returns></returns>
        //private bool IsBright(Color color)
        //{
        //    int sum = color.R + color.G + color.B;
        //    if (sum > 600)
        //}

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int i = 0; i < widthInTiles; i++)
            {
                for (int j = 0; j < heightInTiles; j++)
                {
                    Vector2 position = new Vector2(i * tileSize, j * tileSize);
                    if (tileSetup[grid[i, j]].Unshaded)
                    {
                        spriteBatch.Draw(tile, position, tileSetup[grid[i, j]].Color);
                    }
                    else
                    {
                        spriteBatch.Draw(tile, position,
                            GetShadedColor(position + Vector2.One * (tileSize / 2), tileSetup[grid[i, j]].Color));
                    }
                }
            }

            // Player
            spriteBatch.Draw(tile, player.Position * tileSize, player.Color);

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
