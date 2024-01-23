using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public class SpawnMovementStrategy : IHeroMovementStrategy
    {
        private readonly HeroModel _hero;
        private readonly InputService _inputService;
        private readonly HeroMovementService _heroMovementService;

        public SpawnMovementStrategy(HeroModel hero, InputService inputService, HeroMovementService heroMovementService)
        {
            _hero = hero;
            _inputService = inputService;
            _heroMovementService = heroMovementService;
        }

        void IHeroMovementStrategy.OnStart()
        {
            _hero.CurrentVerticalSpeed = 0f;
            var currentPosition = _hero.CurrentPosition;
            currentPosition.y = 200;
            _hero.CurrentPosition = currentPosition;
            var jumpCharges = _hero.Properties[HeroMovementService.BaseJumpCharges];
            jumpCharges.TrySetValue(0f);
            _inputService.ChangeInputStrategy(null);
            _hero.OnGround = false;
        }
        
        void ITick.Tick(float tickTime)
        {
            var currentPosition = _hero.CurrentPosition;
            currentPosition.y += _hero.CurrentVerticalSpeed * tickTime;
            _hero.PreviousPosition = _hero.CurrentPosition;
            _hero.CurrentPosition = currentPosition;
        }

        void IHeroMovementStrategy.OnGround(float groundPosition)
        {
            var currentPosition = _hero.CurrentPosition;

            currentPosition.y = groundPosition + 0.5f * _hero.HeroConfig.Size.y;
            var jumpCharges = _hero.Properties[HeroMovementService.BaseJumpCharges];
            jumpCharges.TrySetValue(_hero.HeroConfig.BaseParameters[HeroMovementService.BaseJumpCharges]);
            
            _hero.CurrentVerticalSpeed = 0f;
            _hero.PreviousPosition = _hero.CurrentPosition;
            _hero.CurrentPosition = currentPosition;
            
            _heroMovementService.SetMovementStrategy(new RunMovementStrategy(_hero, _inputService));
            _hero.OnGround = true;
        }

        void IHeroMovementStrategy.OnAir(float tickTime)
        {
            _hero.CurrentVerticalSpeed += _hero.HeroConfig.BaseParameters[HeroRunService.BaseGravityAcceleration] * tickTime;
            _hero.OnGround = false;
        }
        
        void IHeroMovementStrategy.OnFinish()
        {
            _inputService.ChangeInputStrategy(null);
        }
    }
}