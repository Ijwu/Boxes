using System;
using Microsoft.Xna.Framework;

namespace Boxes.Collision
{
    public delegate void CollisionEvent(object sender, CollisionEventArgs args);

    public class CollisionEventArgs : EventArgs
    {
        public Rectangle Other;
    }
}