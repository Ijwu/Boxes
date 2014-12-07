using System.Diagnostics;
using Boxes.Compenents;
using Boxes.Entity;
using Boxes.Entity.Implementations;
using Boxes.Extensions;
using Boxes.Services;
using Boxes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
        private SpriteFont _font;
        private FrameRateCounter _fps;

        public Boxes()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            _assetService = new AssetService(this);
            _fps = new FrameRateCounter(this);

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
            _fps.Initialize();
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
            this.Components.Add(_fps);
            _entityManager.Initialize();

            _font = _assetService.LoadContent<SpriteFont>("font");
            var tex = _assetService.LoadContent<Texture2D>("box");

            _entityManager.AddEntity(new Box(Color.White, new Vector2(500), tex){Gravity = new Vector2(0,3), Friction = new Vector2(.001f)});
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
            Input.Input.Update();
            this.UpdateableServices.Update(gameTime);

            if (Input.Input.MouseLeftClick)
            {
                var mpos = new Vector2(Input.Input.MouseX, Input.Input.MouseY);
                _entityManager.GetEntities().ForEach(x => x.Push(Vector2Ext.FromAngle(Vector2Ext.GetAngle(x.Position,mpos))));
                //vec2d.fromangle(getAngle(thing.position, vec2d(event.pos)), MOUSEPOWER)) #Push every square towards where you clicked with the power of MOUSEPOWER
            }

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
