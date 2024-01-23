using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;
using UnityEngine;

namespace Acoolaum.Game.Hero
{
    public class FlightMovementStrategy : IHeroMovementStrategy
    {
        private readonly HeroModel _hero;
        private readonly InputService _inputService;
        private float? _targetY;

        public FlightMovementStrategy(HeroModel hero, InputService inputService)
        {
            _hero = hero;
            _inputService = inputService;
        }

        void IHeroMovementStrategy.OnStart()
        {
            var inputStrategy = new DragInputStrategy();
            inputStrategy.OnDragBegin += OnDrag;
            inputStrategy.OnDrag += OnDrag;
            inputStrategy.OnDragEnd += OnEndDrag;
            _inputService.ChangeInputStrategy(inputStrategy);
        }

        private void OnDrag(Vector3 screenPosition)
        {
            var worldPoint = Camera.main.ScreenToWorldPoint(screenPosition) / 0.01f;
            _targetY = worldPoint.y;
        }
        
        private void OnEndDrag(Vector3 screenPosition)
        {
            _targetY = null;
        }

        void ITick.Tick(float tickTime)
        {
            if (_targetY.HasValue == false)
            {
                return;
            }

            var currentPosition = _hero.CurrentPosition;
            var direction =_targetY.Value - currentPosition.y;
            var absDirection = Mathf.Abs(direction);
            if (absDirection > 0f)
            {
                var maxSpeed = _hero.HeroConfig.BaseParameters["base_flight_speed"] * tickTime;
                _hero.CurrentVerticalSpeed = maxSpeed / direction * _hero.HeroConfig.BaseParameters["base_flight_speed"];
                currentPosition.y = Mathf.Lerp(currentPosition.y, _targetY.Value,
                    Mathf.Min(1f, maxSpeed / absDirection));
                _hero.PreviousPosition = _hero.CurrentPosition;
                _hero.CurrentPosition = currentPosition;
            }
        }

        void IHeroMovementStrategy.OnGround(float groundPosition)
        {
            var currentPosition = _hero.CurrentPosition;

            currentPosition.y = groundPosition + 0.5f * _hero.HeroConfig.Size.y;
            
            _hero.CurrentVerticalSpeed = 0f;
            _hero.PreviousPosition = currentPosition;
            _hero.CurrentPosition = currentPosition;
            _hero.OnGround = true;
        }

        void IHeroMovementStrategy.OnAir(float tickTime)
        {
            _hero.OnGround = false;
        }
        
        void IHeroMovementStrategy.OnFinish()
        {
            _inputService.ChangeInputStrategy(null);
        }
    }
}