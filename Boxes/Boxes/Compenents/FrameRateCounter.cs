using System;
using Boxes.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Compenents
{
    public class FrameRateCounter : DrawableGameComponent
    {
        private Game _game;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FrameRateCounter(Game game)
            : base(game)
        {
            _game = game;
        }

        public override void Initialize()
        {
            base.Initialize();
            _spriteFont =
                (_game.Services.GetService(typeof (AssetService)) as AssetService).LoadContent<SpriteFont>("font");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = frameRate.ToString();

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(1280,768)-_spriteFont.MeasureString(fps)-_spriteFont.MeasureString(fps), Color.White);

            _spriteBatch.End();
        }
    }
}