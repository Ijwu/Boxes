using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity
{
    public class EntityManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private List<IEntity> _entities;

        public EntityManager(Game game) : base(game)
        {
            _entities = new List<IEntity>();
        }

        public void AddEntity(IEntity ent)
        {
            _entities.Add(ent);
        }

        public override void Draw(GameTime gameTime)
        {
            var spriteBatch = new SpriteBatch(GraphicsDevice);
            foreach (var ent in _entities)
            {
                ent.Draw(spriteBatch, gameTime);
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