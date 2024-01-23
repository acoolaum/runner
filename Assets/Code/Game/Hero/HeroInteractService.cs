using System.Collections.Generic;
using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public class HeroInteractService : ServiceBase, ILoaded
    {
        private CollisionService _collisionsService;
        private LevelModelService _levelModelService;
        private HeroHitService _heroHitService;
        private ElementRunActionService _elementRunActionSystem;

        void ILoaded.Loaded()
        {
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _collisionsService = ServiceContainer.Get<CollisionService>();
            _collisionsService.CollisionDetected += OnCollisionDetected;
            _heroHitService = ServiceContainer.Get<HeroHitService>();
            _elementRunActionSystem = ServiceContainer.Get<ElementRunActionService>();
        }

        private void OnCollisionDetected(IReadOnlyList<ZoneElementModel> elements)
        {
            var hero = _levelModelService.LevelModel.Hero;
            var heroBottomPosition = hero.PreviousPosition.y - 0.5f * hero.HeroConfig.Size.y;
            
            for (int i = 0; i < elements.Count; i++)
            {
                var element = elements[i];
                if (element.Config.IsGround)
                {
                    if (heroBottomPosition < element.Position.y + 0.5f * element.Config.Size.y)
                    {
                        _heroHitService.Hit(element);
                        break;
                    }
                }
                else
                {
                    if (element.Config.Actions != null)
                    {
                        _elementRunActionSystem.Execute(element, element.Config.Actions);
                        _levelModelService.RemoveElement(element);
                    }
                }
            }
        }
    }
}