using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public class HeroFlightService : ServiceBase, ILoad
    {
        private const string Flight = "flight";
        
        private LevelModelService _levelModelService;
        private HeroMovementService _heroMovementService;
        private InputService _inputService;

        void ILoad.Load()
        {
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _levelModelService.OnHeroAdded += OnHeroAdded;

            _heroMovementService = ServiceContainer.Get<HeroMovementService>();
            _inputService = ServiceContainer.Get<InputService>();
        }

        private void OnHeroAdded(HeroModel heroModel)
        {
            var property = new HeroProperty(Flight, 0f);
            heroModel.AddProperty(property);
            property.OnPropertyChanged += OnFlightPropertyChanged;
        }

        private void OnFlightPropertyChanged(IHeroProperty property, float oldValue, float newValue)
        {
            var hero = _levelModelService.LevelModel.Hero;
            if (newValue > 0f)
            {
                _heroMovementService.SetMovementStrategy(new FlightMovementStrategy(hero, _inputService));
            }
            else
            {
                _heroMovementService.SetMovementStrategy(new RunMovementStrategy(hero, _inputService));
            }
        }
    }
}