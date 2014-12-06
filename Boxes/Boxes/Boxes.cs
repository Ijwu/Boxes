using System;
using System.Collections.Generic;
using System.Linq;
using Boxes.Collision;
using Boxes.Entity;
using Boxes.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Boxes
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Boxes : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private AssetService _assetService;
        private EntityManager _entityManager;
        public UpdateableServicesCollection UpdateableServices;

        public Boxes()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _assetService = new AssetService(this);
            
            UpdateableServices = new UpdateableServicesCollection();
            this.IsMouseVisible = true;

            _graphics.PreferredBackBufferHeight = 768;
            _graphics.PreferredBackBufferWidth = 1280;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
            this.Services.AddService(typeof(AssetService), _assetService);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            _entityManager = new EntityManager(this);
            this.UpdateableServices.AddService(typeof(EntityManager),_entityManager);
            _entityManager.Initialize();

            for (int i = 1; i < 1000; i++)
            {
                var box = new Box(Color.Red, new Vector2(i, 50), _assetService.LoadContent<Texture2D>("box"))
                {
                    Width = 10,
                    Height = 10,
                    Gravity = new Vector2(0, 1)
                };
                _entityManager.AddEntity(box);   
            }
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            this.UpdateableServices.Update(gameTime);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.UpdateableServices.Draw(gameTime);

            base.Draw(gameTime);
        }
    }
}
