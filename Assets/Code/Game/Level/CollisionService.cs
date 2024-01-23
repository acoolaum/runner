using System;
using System.Collections.Generic;
using Acoolaum.Core.Services;
using Acoolaum.Game.Hero;
using Acoolaum.Game.Model;
using UnityEngine;

namespace Acoolaum.Game.Level
{
    public class CollisionService : ServiceBase, ILoaded, ITick
    {
        public event Action<IReadOnlyList<ZoneElementModel>> CollisionDetected;

        private readonly List<ZoneElementModel> _collisionsBuffer = new();
        private readonly List<ZoneElementModel> _groundBuffer = new();

        private LevelModelService _levelModelService;
        private HeroRunService _heroRunService;
        private HeroMovementService _heroMovementService;

        public void ClearGroundBuffer()
        {
            _groundBuffer.Clear();
        }
        
        void ILoaded.Loaded()
        {
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _heroRunService = ServiceContainer.Get<HeroRunService>();
            _heroMovementService = ServiceContainer.Get<HeroMovementService>();
        }

        void ITick.Tick(float tickTime)
        {
            var levelModel = _levelModelService.LevelModel;
            var zones = levelModel.Zones;
            var heroHorizontalSpeed = _heroRunService.GetHeroHorizontalSpeed();
            var hero = levelModel.Hero;
            var heroPosition = hero.CurrentPosition;
            var heroHalfWidth = hero.HeroConfig.Size.x / 2f;
            _collisionsBuffer.Clear();
            _groundBuffer.Clear();
            for (var i = 0; i < zones.Count; i++)
            {
                var zone = zones[i];
                var zoneHalfWidth = zone.Config.ZoneSize / 2f;
                if (heroPosition.x - heroHalfWidth >
                    zone.Position.x + zoneHalfWidth)
                {
                    continue;
                }

                if (heroPosition.x + heroHalfWidth <
                    zone.Position.x - zoneHalfWidth)
                {
                    break;
                }

                for (var f = 0; f < zone.Elements.Count; f++)
                {
                    var element = zone.Elements[f];
                    if (HasOverlap(hero.CurrentPosition, hero.HeroConfig.Size, element.Position, element.Config.Size))
                    {
                        if (HasOverlap(hero.PreviousPosition, hero.HeroConfig.Size, element.Position + heroHorizontalSpeed * tickTime * Vector2.right,
                                element.Config.Size) == false)
                        {
                            _collisionsBuffer.Add(element);
                        }
                    } 
                    if (StayOn(hero, element))
                    {
                        _groundBuffer.Add(element);
                    }
                }
            }
            
            CollisionDetected?.Invoke(_collisionsBuffer);

            if (_groundBuffer.Count > 0 && hero.CurrentVerticalSpeed <= 0f)
            {
                var maxY = float.MinValue;
                for (int i = 0; i < _groundBuffer.Count; i++)
                {
                    var element = _groundBuffer[i];
                    var elementTop = element.Position.y + element.Config.Size.y / 2f;
                    if (maxY < elementTop)
                    {
                        maxY = elementTop;
                    }
                }

                _heroMovementService.OnGround(maxY);
            }
            else
            {
                _heroMovementService.OnAir(tickTime);
            }
        }

        private bool HasOverlap(Vector2 heroPosition, Vector2 heroSize, Vector2 elementPosition, Vector2 elementSize)
        {
            var distance = heroPosition - elementPosition;
            var halfSizeSum = 0.5f * (heroSize + elementSize);
            if (Math.Abs(distance.x) < halfSizeSum.x && Mathf.Abs(distance.y) < halfSizeSum.y)
            {
                return true;
            }

            return false;
        }

        private bool StayOn(HeroModel heroModel, ZoneElementModel element)
        {
            if (element.Config.IsGround == false)
            {
                return false;
            }
            
            var distance = heroModel.CurrentPosition - element.Position;
            var halfSizeSum = 0.5f * (heroModel.HeroConfig.Size + element.Config.Size);
            
            if (Mathf.Abs(distance.x) > halfSizeSum.x)
            {
                return false;
            }

            var bottomHeroPosition = heroModel.CurrentPosition.y - 0.5f * heroModel.HeroConfig.Size.y - 1f;
            if (Mathf.Abs(bottomHeroPosition - element.Position.y) > 0.5f * element.Config.Size.y)
            {
                return false;
            }

            return true;
        }
    }
}