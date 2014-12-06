using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity
{
    public class EntityManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private List<IEntity> _entities;
        private SpriteBatch _sb;

        public EntityManager(Game game) : base(game)
        {
            _entities = new List<IEntity>();
            _sb = new SpriteBatch(game.GraphicsDevice);
        }

        public void AddEntity(IEntity ent)
        {
            _entities.Add(ent);
        }

        public override void Draw(GameTime gameTime)
        {
            foreach (var ent in _entities)
            {
                ent.Draw(_sb, gameTime);
            }
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var ent in _entities)
            {
                ent.Update(gameTime);
            }
        }
    }
}