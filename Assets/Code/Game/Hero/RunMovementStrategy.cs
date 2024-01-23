using System.Collections.Generic;
using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;
using UnityEngine;

namespace Acoolaum.Game.Hero
{
    public class RunMovementStrategy : IHeroMovementStrategy
    {
        private readonly HeroModel _hero;
        private readonly InputService _inputService;

        public RunMovementStrategy(HeroModel hero, InputService inputService)
        {
            _hero = hero;
            _inputService = inputService;
        }

        void IHeroMovementStrategy.OnStart()
        {
            var swipeInputStrategy = new SwipeInputStrategy();
            swipeInputStrategy.SwipeCompleted += OnSwipeCompleted;
            _inputService.ChangeInputStrategy(swipeInputStrategy);
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
        
        private void OnSwipeCompleted(List<Vector3> swipePositions)
        {
            if (swipePositions.Count < 2)
            {
                return;
            }

            var hero = _hero;
            var verticalDirection = swipePositions[swipePositions.Count - 1].y - swipePositions[0].y;
            var horizontalDirection = swipePositions[swipePositions.Count - 1].x - swipePositions[0].x;
            if (verticalDirection > 40f && Mathf.Abs(horizontalDirection) / verticalDirection < 1.2f)
            {
                var jumpCharges = hero.Properties[HeroMovementService.BaseJumpCharges];
                var jumpChargesAmount = jumpCharges.Value;
                if (jumpChargesAmount > 0)
                {
                    jumpChargesAmount--;
                    jumpCharges.TrySetValue(jumpChargesAmount);
                    hero.CurrentVerticalSpeed += hero.Properties[HeroMovementService.BaseJumpSpeed].Value;
                }
            }
        }
    }
}