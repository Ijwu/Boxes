using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Collision
{
    public class CollisionService
    {
        private Game _game;

        public CollisionService(Game game)
        {
            _game = game;
        }

        public bool Collides(Rectangle box1, Rectangle box2)
        {
            return box1.Intersects(box2);
        }

        public bool CollidesWithScreenBounds(Rectangle box)
        {
            return box.Intersects(_game.GraphicsDevice.Viewport.Bounds);
        }
    }
}