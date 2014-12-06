﻿using Boxes.Collision;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Boxes.Entity
{
    public interface IEntity : IGameComponent, IUpdate
    {
        bool Disposing { get; set; }
        void Draw(SpriteBatch sb, GameTime time);
        Rectangle GetBoundingBox();
        event CollisionEvent Collides;
        void InvokeCollides(object sender, CollisionEventArgs args);
    }
}