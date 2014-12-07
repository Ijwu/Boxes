using Boxes.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity
{
    public interface IEntity : IGameComponent, IUpdate
    {
        bool Disposing { get; set; }
        Vector2 Position { get; set;}
        Vector2 Velocity { get; set; }
        Vector2 Gravity { get; set; }
        void Draw(SpriteBatch sb, GameTime time);
        Rectangle GetBoundingBox();
        void Push(Vector2 vel);
    }
}