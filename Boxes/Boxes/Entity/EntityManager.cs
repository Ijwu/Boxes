using System.Collections.Generic;
using Boxes.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity
{
    public class EntityManager : IComponentService
    {
        private List<IEntity> _entities;
        private SpriteBatch _sb;

        public EntityManager(Game game)
        {
            _entities = new List<IEntity>();
            _sb = new SpriteBatch(game.GraphicsDevice);
        }

        public void AddEntity(IEntity ent)
        {
            _entities.Add(ent);
        }

        public void Draw(GameTime gameTime)
        {
            _sb.Begin();
            foreach (var ent in _entities)
            {
                ent.Draw(_sb, gameTime);
            }
            _sb.End();
        }

        public void Update(GameTime gameTime)
        {
            foreach (var ent in _entities)
            {
                ent.Update(gameTime);
            }
        }

        public List<IEntity> GetEntities()
        {
            return _entities;
        }
    }
}