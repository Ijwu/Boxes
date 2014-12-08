using System;
using System.Diagnostics;
using System.Linq;
using Boxes.Compenents;
using Boxes.Entity;
using Boxes.Entity.Implementations;
using Boxes.Extensions;
using Boxes.Modifiers;
using Boxes.Objective;
using Boxes.Services;
using Boxes.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

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
        private ModifierTimer _modifierTimer = new ModifierTimer(4000);
        private Modifier _currentGravity = Modifier.GravityDown;
        private Vector2 _clickPower = new Vector2(3, 10);
        private bool _hasObjective;
        private bool _flashScreen;
        private int _maxBoxes = 1;
        private Texture2D _arrow;
        private Random _random = new Random();
        private double _difficulty = 1;
        private Texture2D _boxTexture;
        private bool _gameLost;
        private double _score;
        private int _toAdd;
        private Color _flashColor;
        private bool _instructionScreen = true;
        private bool _paused = false;

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
            _modifierTimer.Elapsed += AdjustDifficulty;
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
            _boxTexture = _assetService.LoadContent<Texture2D>("box");
            _arrow = _assetService.LoadContent<Texture2D>("arrow");

            _entityManager.AddEntity(new Box(new Color(_random.Next(100, 256), _random.Next(100, 256), _random.Next(100, 256)), new Vector2(_random.Next(0, 1280), _random.Next(0, 768)), _boxTexture) { Gravity = new Vector2(0, 1) });   
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void OnDeactivated(object sender, EventArgs args)
        {
            base.OnDeactivated(sender, args);
            _paused = true;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            Input.Input.Update();

            if (Input.Input.IsKeyTapped(Keys.Escape))
            {
                _paused = true;
            }

            if (_paused)
            {
                if (Input.Input.IsKeyTapped(Keys.Enter))
                {
                    _paused = false;
                }
                return;
            }

            if (_instructionScreen)
            {
                if (Input.Input.IsKeyTapped(Keys.Enter))
                {
                    _instructionScreen = false;
                }
                return;
            }

            if (_gameLost)
            {
                if (Input.Input.IsKeyTapped(Keys.Enter))
                {
                    _entityManager.GetEntities().Clear();
                    _score = 0;
                    _difficulty = 1;
                    _modifierTimer = new ModifierTimer(4000);
                    _currentGravity = Modifier.GravityDown;
                    _clickPower = new Vector2(10, 10);
                    _hasObjective = false;
                    _flashScreen = false;
                    _maxBoxes = 1;
                    _toAdd = 0;
                    _gameLost = false;
                    _entityManager.AddEntity(new Box(new Color(_random.Next(100, 256), _random.Next(100, 256), _random.Next(100, 256)), new Vector2(_random.Next(0, 1280), _random.Next(0, 768)), _boxTexture) { Gravity = new Vector2(0, 1) });   
                }
                return;
            }

            this.UpdateableServices.Update(gameTime);

            if (Input.Input.MouseLeftClick || Input.Input.MouseRightClick)
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
            if (!_hasObjective)
            {
                var objective = CreateObjectiveInRandomArea();
                objective.Elapsed += OnObjectiveAreaElapsed;
                lock(_entityManager.GetEntities())
                {
                    switch (_currentGravity)
                    {
                        case Modifier.GravityUp:
                            _entityManager.AddEntity(objective);
                            _hasObjective = true;
                            break;
                        case Modifier.GravityDown:
                            _entityManager.AddEntity(objective);
                            _hasObjective = true;
                            break;
                        case Modifier.GravityLeft:
                            _entityManager.AddEntity(objective);
                            _hasObjective = true;
                            break;
                        case Modifier.GravityRight:
                            _entityManager.AddEntity(objective);
                            _hasObjective = true;
                            break;
                        case Modifier.NoGravity:
                            _entityManager.AddEntity(objective);
                            _hasObjective = true;
                            break;
                    }
                }
            }
            Vector2 grav = new Vector2();
            switch (_currentGravity)
            {
                case Modifier.GravityUp:
                    grav = new Vector2(0, -1);
                    break;
                case Modifier.GravityDown:
                    grav = new Vector2(0, 1);
                    break;
                case Modifier.GravityLeft:
                    grav = new Vector2(-1, 0);
                    break;
                case Modifier.GravityRight:
                    grav = new Vector2(1, 0);
                    break;
                case Modifier.NoGravity:
                    grav = new Vector2(0);
                    break;
            }

            for (int i = 0; i < _toAdd; i++)
            {
                var objectives = _entityManager.GetEntities().Where(x => x is ObjectiveArea).ToList();
                var pos = new Vector2(_random.Next(0, 1280), _random.Next(0, 768));
                while (objectives.Any(x => x.GetBoundingBox().Contains((int)pos.X, (int)pos.Y)))
                {
                    pos = new Vector2(_random.Next(0, 1280), _random.Next(0, 768));    
                }
                _entityManager.AddEntity(new Box(new Color(_random.Next(100, 256), _random.Next(100, 256), _random.Next(100, 256)), pos, _boxTexture)
                {
                    Gravity = grav
                });   
            }
            _toAdd = 0;

            base.Update(gameTime);
        }

        private ObjectiveArea CreateObjectiveInRandomArea()
        {
            var bounds = GraphicsDevice.Viewport.Bounds;
            int rand = _random.Next(0, 8);
            ObjectiveArea objective = null;
            double ttl = 1500 - (2000*_difficulty*10/100);
            switch (rand)
            {
                case 0:
                    objective = new ObjectiveArea(new Rectangle(0, bounds.Height / 2, bounds.Width, bounds.Height / 2),
                    ttl, _boxTexture);
                    break;
                case 1:
                    objective = new ObjectiveArea(new Rectangle(0, 0, bounds.Width, bounds.Height / 2),
                    ttl, _boxTexture);
                    break;
                case 2:
                    objective = new ObjectiveArea(new Rectangle(bounds.Width/2, 0, bounds.Width/2, bounds.Height),
                    ttl, _boxTexture);
                    break;
                case 3:
                    objective = new ObjectiveArea(new Rectangle(0, 0, bounds.Width/2, bounds.Height),
                    ttl, _boxTexture);
                    break;
                case 4:
                    objective = new ObjectiveArea(new Rectangle(0, 0, bounds.Width / 2, bounds.Height / 2),
                    ttl, _boxTexture);
                    break;
                case 5:
                    objective = new ObjectiveArea(new Rectangle(0, bounds.Height / 2, bounds.Width / 2, bounds.Height / 2),
                    ttl, _boxTexture);
                    break;
                case 6:
                    objective = new ObjectiveArea(new Rectangle(bounds.Width/2, bounds.Height / 2, bounds.Width/2, bounds.Height / 2),
                    ttl, _boxTexture);
                    break;
                case 7:
                    objective = new ObjectiveArea(new Rectangle(bounds.Width/2, 0, bounds.Width/2, bounds.Height / 2),
                    ttl, _boxTexture);
                    break;
            }
            return objective;
        }

        private void OnObjectiveAreaElapsed(object sender, ObjectiveAreaElapsedArgs args)
        {
            (sender as ObjectiveArea).Dispose();
            _hasObjective = false;

            var entities = _entityManager.GetEntities();
            ObjectiveArea thisArea = sender as ObjectiveArea;

            entities.ForEach(x =>
            {
                if (!x.GetBoundingBox().Intersects(thisArea.GetBoundingBox()))
                {
                    x.Dispose();
                }
            });
            var entCount = entities.Count(x => !x.Disposing);

            if (entCount == 0)
            {
                _gameLost = true;
            }

            var all = _entityManager.GetEntities().Count;
            var penalty = (all - entCount);
            penalty = penalty * 3;

            _score += entCount - penalty;

            _toAdd = (int)(5 + (300*_difficulty/100) + entCount*_difficulty/100);

            _flashColor = new Color(_random.Next(100, 200), _random.Next(100, 200), _random.Next(100, 200)) * .5f;

            _maxBoxes = Math.Max(entCount, _maxBoxes);
        }

        private void AdjustDifficulty(object sender, ModifierElapsedEventArgs args)
        {
            _difficulty += .1333337;
            var range = _modifierTimer.GetTimeRange();
            _modifierTimer.AdjustTimeRange(Math.Max(range.X - (3*_difficulty / 100 * range.X), 2000));
            _clickPower = new Vector2((float)(10+100*_difficulty/100), (float)(10+100*_difficulty/100)); 
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
                    _entityManager.GetEntities().ForEach(x =>
                    {
                        x.Gravity = new Vector2(0, 2);
                        x.Friction = Vector2.Zero;
                    });
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.GravityUp:
                    _entityManager.GetEntities().ForEach(x =>
                    {
                        x.Gravity = new Vector2(0, -2);
                        x.Friction = Vector2.Zero;
                    });
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.GravityLeft:
                    _entityManager.GetEntities().ForEach(x =>
                    {
                        x.Gravity = new Vector2(-2, 0);
                        x.Friction = Vector2.Zero;
                    });
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.GravityRight:
                    _entityManager.GetEntities().ForEach(x =>
                    {
                        x.Gravity = new Vector2(2, 0);
                        x.Friction = Vector2.Zero;
                    });
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.RandomizeGravity:
                    _entityManager.GetEntities().ForEach(x =>
                    {
                        x.Gravity = new Vector2(_random.Next(0, 4), _random.Next(0, 4));
                        x.Friction = Vector2.Zero;
                    });
                    _currentGravity = args.Modifier;
                    break;
                case Modifier.NoGravity:
                    _entityManager.GetEntities().ForEach(x =>
                    {
                        x.Gravity = new Vector2(0);
                        x.Friction = new Vector2(1, 1);
                    });                 
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
            if (_flashScreen)
            {
                GraphicsDevice.Clear(_flashColor);
                _flashScreen = false;
            }
            else
            {
                GraphicsDevice.Clear(_flashColor);
            }

            this.UpdateableServices.Draw(gameTime);

            _spriteBatch.Begin();
            var pos = GraphicsDevice.Viewport.Bounds.Center.ToVector2();
            var origin = new Vector2(_arrow.Width/2, _arrow.Height/2);
            switch (_currentGravity)
            {
                case Modifier.GravityDown:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White * .1f, (float)(Math.PI / 2), origin, 1.0f, SpriteEffects.None, 0f);
                    break;
                case Modifier.GravityLeft:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White * .1f, (float)Math.PI, origin, 1.0f, SpriteEffects.None, 0f);
                    break;
                case Modifier.GravityRight:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White * .1f, 0, origin, 1.0f, SpriteEffects.None, 0f);
                    break;
                case Modifier.GravityUp:
                    _spriteBatch.Draw(_arrow, pos, null, Color.White * .1f, (float)(1.5 * Math.PI), origin, 1.0f, SpriteEffects.None, 0f);
                    break;
            }
            _spriteBatch.DrawString(_font, ((int)_score).ToString(), new Vector2(40, 10), Color.White);

            if (_paused)
            {
                var bounds = GraphicsDevice.Viewport.Bounds;
                var center = new Vector2(bounds.Center.X, bounds.Center.Y);
                _spriteBatch.DrawString(_font, "Paused.", center - _font.MeasureString("Paused."), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "Press Enter to unpause.", center - _font.MeasureString("Press Enter to unpause.") - new Vector2(0, 40), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }

            if (_gameLost)
            {
                var bounds = GraphicsDevice.Viewport.Bounds;
                var center = new Vector2(bounds.Center.X, bounds.Center.Y);
                _spriteBatch.DrawString(_font, "Game over!", center-_font.MeasureString("Game over!")-new Vector2(0,40),Color.White,0f,Vector2.Zero,2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "You lost all your boxes.", center - _font.MeasureString("You lose all your boxes."), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "Press Enter to restart.", center - _font.MeasureString("Press Enter to restart.") + new Vector2(0, 40), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, string.Format("Final Score: {0}", (int)_score), center - _font.MeasureString(string.Format("Final Score: {0}", (int)_score)) + new Vector2(0, 80), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }

            if (_instructionScreen)
            {
                var bounds = GraphicsDevice.Viewport.Bounds;
                var center = new Vector2(bounds.Center.X, bounds.Center.Y);
                _spriteBatch.DrawString(_font, "Thanks for playing Boxes!", center - _font.MeasureString("Thanks for playing Boxes!") - new Vector2(0, 120), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "Click the screen to pull the boxes to your cursor.", center - _font.MeasureString("Click the screen to pull the boxes to your cursor.") - new Vector2(0, 80), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "Get the boxes into the highlighted zone.", center - _font.MeasureString("Get the boxes into the highlighted zone.") - new Vector2(0, 40), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "You get points for each box that made it", center - _font.MeasureString("You get points for each box that made it"), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "and lose points for each that didn't.", center - _font.MeasureString("and lose points for each that didn't.") + new Vector2(0, 40), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "Boxes that don't make it disappear.", center - _font.MeasureString("Boxes that don't make it disappear.") + new Vector2(0, 80), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "Don't lose all your boxes!", center - _font.MeasureString("Don't lose all your boxes!") + new Vector2(0, 120), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
                _spriteBatch.DrawString(_font, "Press Enter to start.", center - _font.MeasureString("Press Enter to start.") + new Vector2(0, 160), Color.White, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
