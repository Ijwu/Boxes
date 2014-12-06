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
        private List<Chunk> _chunks; 

        public CollisionService(Game game)
        {
            _game = game as Boxes;
            _chunks = new List<Chunk>();
        }

        public bool Collides(Rectangle box1, Rectangle box2)
        {
            return box1.Intersects(box2);
        }

        public List<Tuple<Rectangle, Rectangle>> CollidesList(List<Rectangle> rects)
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
            var et = _game.UpdateableServices.GetService(typeof (EntityManager)) as EntityManager;
        }
    }
}