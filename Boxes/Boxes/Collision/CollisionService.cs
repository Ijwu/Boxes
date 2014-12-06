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
        private Chunk[,] _chunks;
        private const int GridSpacing = 64;

        public CollisionService(Game game)
        {
            _game = game as Boxes;

            _chunks = new Chunk[game.GraphicsDevice.Viewport.Width / GridSpacing, game.GraphicsDevice.Viewport.Height/GridSpacing];
            for (int i = 0; i < game.GraphicsDevice.Viewport.Width; i += GridSpacing)
            {
                for (int j = 0; j < game.GraphicsDevice.Viewport.Height; j += GridSpacing)
                {
                    _chunks[i/GridSpacing, j/GridSpacing] = new Chunk(i, j, GridSpacing);
                }
            }

            var ent = _game.UpdateableServices.GetService(typeof (EntityManager)) as EntityManager;
            ent.EntityAdded += OnEntityAdded;
        }

        private void OnEntityAdded(object sender, EntityAddedEventArgs args)
        {
            var box = args.Entity.GetBoundingBox();
            args.Entity.Chunk = _chunks[box.X/GridSpacing, box.Y/GridSpacing];
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
            var et = _game.UpdateableServices.GetService(typeof (EntityManager)) as EntityManager;
            var ents = et.GetEntities();
            foreach (var chunk in _chunks)
            {
                chunk.Entities = new List<IEntity>();
                foreach (var entity in ents)
                {
                    if (Collides(chunk.Area, entity.GetBoundingBox()))
                        chunk.Entities.Add(entity);
                }
            }
        }
    }
}