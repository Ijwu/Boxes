using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Compenents
{
    public class FrameRateCounter : DrawableGameComponent
    {
        private ContentManager _content;
        private SpriteBatch _spriteBatch;
        private SpriteFont _spriteFont;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;


        public FrameRateCounter(Game game)
            : base(game)
        {
            _content = new ContentManager(game.Services);
        }


        protected void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                _spriteBatch = new SpriteBatch(GraphicsDevice);
                _spriteFont = _content.Load<SpriteFont>("Font");
            }
        }


        protected void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent)
                _content.Unload();
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

            string fps = string.Format("fps: {0}", frameRate);

            _spriteBatch.Begin();

            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(33, 33), Color.Black);
            _spriteBatch.DrawString(_spriteFont, fps, new Vector2(32, 32), Color.White);

            _spriteBatch.End();
        }
    }
}