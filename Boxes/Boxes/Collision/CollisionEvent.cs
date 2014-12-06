using System;
using Boxes.Entity;
using Microsoft.Xna.Framework;

namespace Boxes.Collision
{
    public delegate void CollisionEvent(object sender, CollisionEventArgs args);

    public class CollisionEventArgs : EventArgs
    {
        public IEntity Other;

        public CollisionEventArgs(IEntity other)
        {
            Other = other;
        }
    }
}