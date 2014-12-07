using System;
using System.Diagnostics;
using Boxes.Compenents;
using Boxes.Entity;
using Boxes.Entity.Implementations;
using Boxes.Extensions;
using Boxes.Modifiers;
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
        private ClickMoveMode _currentMode = ClickMoveMode.Pull;
        private ModifierTimer _modifierTimer = new ModifierTimer(2000);
        private Vector2 _clickPower = new Vector2(3, 10);
        private Texture2D _arrow;
        private Random _random = new Random();
        private Modifier _currentGravity;

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
            _modifierTimer.Elapsed += OnModifierTimerElapsed;
            _modifierTimer.Run();
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
            _arrow = _assetService.LoadContent<Texture2D>("arrow");

            for (int i = 0; i < 500; i++)
            {
                _entityManager.AddEntity(new Box(new Color(_random.Next(100, 256), _random.Next(100, 256), _random.Next(100, 256)), new Vector2(_random.Next(0, 1280), _random.Next(0, 768)), tex) { Gravity = new Vector2(0, 1), Friction = new Vector2(.1f)});   
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
            Input.Input.Update();
            this.UpdateableServices.Update(gameTime);

            if (Input.Input.MouseLeftClick)
            {
                var mpos = new Vector2(Input.Input.MouseX, Input.Input.MouseY);
                switch (_currentMode)
                {
                    case ClickMoveMode.Pull:
                        _entityManager.GetEntities()
                            .ForEach(x => x.Push(Vector2Ext.FromAngle(Vector2Ext.GetAngle(x.Position, mpos), _clickPower.X,_clickPower.Y)));
                        break;
                    case ClickMoveMode.Push:
                        _entityManager.GetEntities()
                            .ForEach(x => x.Push(Vector2Ext.FromAngle(Vector2Ext.GetAngle(x.Position, mpos), _clickPower.X, _clickPower.Y) * -1));
                        break;
                }
            }

            base.Update(gameTime);
        }

        private void OnModifierTimerElapsed(object sender, ModifierElapsedEventArgs args)
        {
            switch (args.Modifier)
            {
                case Modifier.Pull:
                    _currentMode = ClickMoveMode.Pull;
                    break;
                case Modifier.Push:
                    _currentMode = ClickMoveMode.Push;
                    break;
                case Modifier.GravityDown:
                    _entityManager.GetEntities().ForEach(x => x.Gravity = new Vector2(0, 1));
                    _clickPower = new Vector2(3, 10);
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.GravityUp:
                    _entityManager.GetEntities().ForEach(x => x.Gravity = new Vector2(0, -1));
                    _clickPower = new Vector2(3, 10);
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.GravityLeft:
                    _entityManager.GetEntities().ForEach(x => x.Gravity = new Vector2(-1, 0));
                    _clickPower = new Vector2(5, 3);
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.GravityRight:
                    _entityManager.GetEntities().ForEach(x => x.Gravity = new Vector2(1, 0));
                    _clickPower = new Vector2(5, 3);
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.RandomizeGravity:
                    _entityManager.GetEntities().ForEach(x => x.Gravity = new Vector2(_random.Next(0,2),_random.Next(0,2)));
                    _clickPower = new Vector2(5, 5);
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.NoGravity:
                    _entityManager.GetEntities().ForEach(x => x.Gravity = new Vector2(0));
                    _clickPower = new Vector2(2,2);
                    _currentGravity = args.Modifier;
                    break;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            this.UpdateableServices.Draw(gameTime);

            _spriteBatch.Begin();
            var pos = GraphicsDevice.Viewport.Bounds.Center.ToVector2() - new Point(_arrow.Width, _arrow.Height).ToVector2();
            switch (_currentGravity)
            {
                case Modifier.GravityDown:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White, (float)(1.5*Math.PI), new Vector2(_arrow.Width / 2, _arrow.Height / 2), 1.0f, SpriteEffects.None, 0f);
                    break;
                case Modifier.GravityLeft:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White, (float)Math.PI, new Vector2(_arrow.Width / 2, _arrow.Height / 2), 1.0f, SpriteEffects.None, 0f);
                    break;
                case Modifier.GravityRight:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White, 0, new Vector2(_arrow.Width / 2, _arrow.Height / 2), 1.0f, SpriteEffects.None, 0f);
                    break;
                case Modifier.GravityUp:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White, (float)(Math.PI/2), new Vector2(_arrow.Width / 2, _arrow.Height / 2), 1.0f, SpriteEffects.None, 0f);
                    break;
                default:
                    break;
            }
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
