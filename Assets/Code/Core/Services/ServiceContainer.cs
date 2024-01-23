using System;
using System.Collections.Generic;

namespace Acoolaum.Core.Services
{
    public class ServiceContainer
    {
        private readonly Dictionary<Type, IService> _services = new ();
        private readonly string _name;
        private readonly ServiceContainer _parent;

        public ServiceContainer(string name, ServiceContainer parent = null)
        {
            _name = name;
            _parent = parent;
        }

        public void Add<TService>(TService service) where TService: IService
        {
            var type = typeof(TService);
            
            if (_services.ContainsKey(type))
            {
                throw new ArgumentException($"Service with type {type} already registered");
            }
            
            _services.Add(type, service);
        }
        
        public TService Get<TService>() where TService : IService
        {
            var type = typeof(TService);
            if (_services.TryGetValue(type, out var service) == false)
            {
                if (_parent == null)
                {
                    throw new ArgumentException($"Service with type {type} not registered");    
                }

                return _parent.Get<TService>();
            }

            return (TService)service;
        }
        
        public IEnumerable<TInterface> GetAll<TInterface>() where TInterface : class, IService
        {
            foreach (var service in _services)
            {
                if (service.Value is TInterface result)
                {
                    yield return result;
                }
            }
        }

        public void Initialize()
        {
            foreach (var service in _services)
            {
                service.Value.Initialize(this);
            }
        }
        
        public void Dispose()
        {
            foreach (var service in _services)
            {
                service.Value.Dispose();
            }
        }
    }
}