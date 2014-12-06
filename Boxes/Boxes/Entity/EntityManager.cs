using System.Collections.Generic;
using Boxes.Collision;
using Boxes.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity
{
    public class EntityManager : IComponentService
    {
        private List<IEntity> _entities;
        private SpriteBatch _sb;
        private CollisionService _cs;
        private Game _game;
        public event EntityAddedEvent EntityAdded;

        public EntityManager(Game game)
        {
            _game = game;
            _entities = new List<IEntity>();
            _sb = new SpriteBatch(game.GraphicsDevice);
        }

        public void AddEntity(IEntity ent)
        {
            _entities.Add(ent);
            if (EntityAdded != null)
                EntityAdded(this, new EntityAddedEventArgs(ent));
        }

        public void Initialize()
        {
            _cs = new CollisionService(_game);
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
            _cs.Update(gameTime);
        }

        public List<IEntity> GetEntities()
        {
            return _entities;
        }
    }
}