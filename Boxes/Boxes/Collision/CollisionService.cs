using System;
using System.Collections.Generic;
using System.Linq;
using Boxes.Entity;
using Boxes.Services;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Collision
{
    public class CollisionService
    {
        private Boxes _game;
        private const int GridSpacing = 64;

        public CollisionService(Game game)
        {
            _game = game as Boxes;

            var ent = _game.UpdateableServices.GetService(typeof (EntityManager)) as EntityManager;
            ent.EntityAdded += OnEntityAdded;
        }

        private void OnEntityAdded(object sender, EntityAddedEventArgs args)
        {
            
        }

        public static bool Collides(Rectangle box1, Rectangle box2)
        {
            return box1.Intersects(box2);
        }

        public static List<Tuple<Rectangle, Rectangle>> CollidesList(List<Rectangle> rects)
        {
            return  (from x in rects
                    from y in rects
                    where x.Intersects(y)
                    select new Tuple<Rectangle, Rectangle>(x, y)).ToList();
        }

        public bool CollidesWithScreenBounds(Rectangle box)
        {
            var bounds = _game.GraphicsDevice.Viewport.Bounds;
            return (box.Left < bounds.Left || box.Right > bounds.Right || box.Bottom < bounds.Bottom ||
                    box.Top > bounds.Top);
        }

        public void Update(GameTime time)
        {
        }
    }
}