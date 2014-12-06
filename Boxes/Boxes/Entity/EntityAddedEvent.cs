using System;

namespace Boxes.Entity
{
    public delegate void EntityAddedEvent(object sender, EntityAddedEventArgs args);

    public class EntityAddedEventArgs : EventArgs
    {
        public IEntity Entity;

        public EntityAddedEventArgs(IEntity entity)
        {
            Entity = entity;
        }
    }
}