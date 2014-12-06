using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Boxes.Entity;
using Microsoft.Xna.Framework;

namespace Boxes.Collision
{
    public class CollisionService
    {
        private Boxes _game;

        public CollisionService(Game game)
        {
            _game = game as Boxes;

            var ent = _game.UpdateableServices.GetService(typeof (EntityManager)) as EntityManager;
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

        public void Update(GameTime time)
        {
            Task.Factory.StartNew(RunCollisionChecks);
        }

        private void RunCollisionChecks()
        {
            var manager = _game.UpdateableServices.GetService(typeof(EntityManager)) as EntityManager;
            List<IEntity> ents = manager.GetEntities();
            ents.Sort((x, y) => x.GetBoundingBox().X.CompareTo(y.GetBoundingBox().Y));

            var entsQueue = new Queue<IEntity>(ents);

            for (int i = 0; i < entsQueue.Count; i++)
            {
                var ent = entsQueue.Dequeue();

                var box = ent.GetBoundingBox();
                foreach (var entity in entsQueue)
                {
                    var otherBox = entity.GetBoundingBox();
                    if (box.Right > otherBox.X)
                    {
                        if (box.Bottom > otherBox.Y)
                        {
                            if (otherBox.Bottom > box.Y)
                            {
                                ent.InvokeCollides(this, new CollisionEventArgs(entity));
                            }
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}