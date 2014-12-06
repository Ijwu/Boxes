using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity
{
    public interface IEntity : IGameComponent, IUpdate
    {
        void Draw(SpriteBatch sb, GameTime time);
    }
}