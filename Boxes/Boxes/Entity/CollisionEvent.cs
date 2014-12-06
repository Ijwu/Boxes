using System;
using Microsoft.Xna.Framework;

namespace Boxes.Entity
{
    public delegate void CollisionEvent(object sender, CollisionEventArgs args);

    public class CollisionEventArgs : EventArgs
    {
        public Rectangle Other;
    }
}