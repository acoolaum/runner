using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public class HeroHitService : ServiceBase, ILoaded
    {
        private LevelModelService _levelModelService;
        private HeroHealthService _heroHealthService;
        private HeroMovementService _heroMovementService;
        private HeroPropertyModificatorsService _heroPropertyModificatorsService;
        private CollisionService _collisionService;

        void ILoaded.Loaded()
        {
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _heroHealthService = ServiceContainer.Get<HeroHealthService>();
            _heroMovementService = ServiceContainer.Get<HeroMovementService>();
            _heroPropertyModificatorsService = ServiceContainer.Get<HeroPropertyModificatorsService>();
            _collisionService = ServiceContainer.Get<CollisionService>();
        }

        public void Hit(ZoneElementModel element)
        {
            _heroHealthService.RemoveHealth(1f);
            _collisionService.ClearGroundBuffer();
            _heroPropertyModificatorsService.RemoveAll();
            if (_heroHealthService.GetHealth() > 0f)
            {
                _heroMovementService.DropFromHaven();                
            }
        }
    }
}