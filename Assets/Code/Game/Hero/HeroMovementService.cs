using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public class HeroMovementService : ServiceBase, ILoad, ITick
    {
        public const string BaseJumpSpeed = "base_jump_speed";
        public const string BaseJumpCharges = "base_jump_charges";
        
        private LevelModelService _levelModelService;
        private IHeroMovementStrategy _movementStrategy;
        private InputService _inputService;

        public void OnGround(float groundPosition)
        {
            _movementStrategy?.OnGround(groundPosition);
        }

        public void OnAir(float tickTime)
        {
            _movementStrategy?.OnAir(tickTime);
        }

        public void DropFromHaven()
        {
            var hero = _levelModelService.LevelModel.Hero;
            SetMovementStrategy(new SpawnMovementStrategy(hero, _inputService, this));
        }
        
        void ILoad.Load()
        {
            _inputService = ServiceContainer.Get<InputService>();
            
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _levelModelService.OnHeroAdded += OnHeroAdded;
        }

        void OnHeroAdded(HeroModel heroModel)
        {
            var hero = _levelModelService.LevelModel.Hero;
            var baseJumpSpeed = new HeroProperty(BaseJumpSpeed, heroModel.HeroConfig.BaseParameters[BaseJumpSpeed]);
            hero.AddProperty(baseJumpSpeed);
            
            var jumpCharges = new HeroProperty(BaseJumpCharges, heroModel.HeroConfig.BaseParameters[BaseJumpCharges]);
            hero.AddProperty(jumpCharges);
            
            SetMovementStrategy(new RunMovementStrategy(hero, _inputService));
        }

        public void SetMovementStrategy(IHeroMovementStrategy movementStrategy)
        {
            _movementStrategy?.OnFinish();
            
            _movementStrategy = movementStrategy;
            _movementStrategy.OnStart();
        }

        void ITick.Tick(float tickTime)
        {
            _movementStrategy?.Tick(tickTime);
        }
    }
}