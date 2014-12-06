using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Boxes.Services
{
    public class UpdateableServicesCollection
    {
        private Dictionary<Type, IComponentService> _services;

        public UpdateableServicesCollection()
        {
            _services = new Dictionary<Type, IComponentService>();
        }

        public void AddService(Type t, IComponentService componentService)
        {
            if (_services.ContainsKey(t))
            {
                throw new InvalidOperationException("You may not register more than one service of the same type.");
            }
            _services.Add(t, componentService);
        }

        public IComponentService GetService(Type t)
        {
            if (_services.ContainsKey(t))
            {
                return _services[t];
            }
            throw new InvalidOperationException("That service has not been registered.");
        }

        public void Update(GameTime time)
        {
            foreach (var componentService in _services.Values)
            {
                componentService.Update(time);
            }
        }

        public void Draw(GameTime time)
        {
            foreach (var componentService in _services.Values)
            {
                componentService.Draw(time);
            }
        }
    }
}