using System.Collections.Generic;
using System.Linq;
using Acoolaum.Core.Services;

namespace Acoolaum.Game.Level
{
    public class LevelLifetimeService : IService
    {
        public class LevelTimeState
        {
            public float Time;
            public float TickTime;
            public float UpdateTime;
            public bool IsPaused;
        }

        public LevelTimeState TimeState { get; private set; }

        private List<ILoad> _loadServices;
        private List<ILoaded> _loadedServices;
        private List<IUpdate> _updateServices;
        private List<ITick> _tickServices;

        void IService.Initialize(ServiceContainer serviceContainer)
        {
            _loadServices = serviceContainer.GetAll<ILoad>().ToList();
            _loadedServices = serviceContainer.GetAll<ILoaded>().ToList();
            _tickServices = serviceContainer.GetAll<ITick>().ToList();
            _updateServices = serviceContainer.GetAll<IUpdate>().ToList();
        }

        public void Load()
        {
            TimeState = new LevelTimeState();
            for (var i = 0; i < _loadServices.Count; i++)
            {
                var loadService = _loadServices[i];
                loadService.Load();
            }
        }

        public void Loaded()
        {
            for (var i = 0; i < _loadedServices.Count; i++)
            {
                var loadedService = _loadedServices[i];
                loadedService.Loaded();
            }
        }

        public void Pause(bool isPaused)
        {
            TimeState.IsPaused = isPaused;
        }

        public void Tick(float tickTime)
        {
            if (TimeState.IsPaused)
            {
                return;
            }

            TimeState.TickTime = tickTime;
            TimeState.Time += tickTime;
            
            for (var i = 0; i < _tickServices.Count; i++)
            {
                var tickService = _tickServices[i];
                tickService.Tick(TimeState.TickTime);
            }
        }

        public void Update(float updateTime)
        {
            TimeState.UpdateTime += updateTime;
            for (var i = 0; i < _updateServices.Count; i++)
            {
                var updateService = _updateServices[i];
                updateService.Update(TimeState.UpdateTime);
            }
        }
    }

    public interface ITick : IService
    {
        public void Tick(float tickTime);
    }
    
    public interface IUpdate : IService
    {
        public void Update(float updateTime);
    }

    public interface ILoad : IService
    {
        public void Load();
    }
    
    public interface ILoaded : IService
    {
        public void Loaded();
    }
}