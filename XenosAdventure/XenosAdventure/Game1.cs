using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XenosAdventure
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int tileSize = 32;
        const int width = 1366;
        const int height = 768;

        TileMap adventureMap;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferWidth = width;
            graphics.PreferredBackBufferHeight = height;
            graphics.ApplyChanges();

            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            // Floor
            List<TileType> tileSetup = new List<TileType>();
            TileType type = new TileType(new Color(226, 114, 91), 10, 50);
            type.Movable = true;
            tileSetup.Add(type);
            type = new TileType(new Color(179, 82, 62), 6, 50);
            type.Movable = true;
            tileSetup.Add(type);
            
            // Walls
            //TileType walls = new TileType(new Color(101, 0, 11), 7, 40);
            TileType walls = new TileType(new Color(255, 255, 0), 7, 40);
            walls.Unshaded = true;
            tileSetup.Add(walls);

            //walls = new TileType(new Color(150, 0, 24), 1, 30);
            walls = new TileType(new Color(255, 255, 0), 1, 30);
            walls.Unshaded = true;
            tileSetup.Add(walls);

            // Lava
            type = new TileType(new Color(230, 32, 32), 3, 30);
            type.Unshaded = true;
            type.Flood = true;
            tileSetup.Add(type);

            

            adventureMap = new TileMap(this, tileSetup, tileSize);
            Components.Add(adventureMap);
            

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            

            
            //lightSources.Add(new PointLight(new Vector2(900, 200), new Color(.75f, .93f, .92f), 100));
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                this.Exit();

            base.Update(gameTime);
        }

        

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            

            base.Draw(gameTime);
        }
    }
}
