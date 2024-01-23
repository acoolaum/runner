using Acoolaum.Core.Services;
using Acoolaum.Game.Hero;
using UnityEngine;

namespace Acoolaum.Game.Level
{
    public class ZoneMoveService : ServiceBase, ILoaded, ITick
    {
        private LevelModelService _levelModelService;
        private HeroRunService _heroRunService;

        void ILoaded.Loaded()
        {
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _heroRunService = ServiceContainer.Get<HeroRunService>();
        }

        void ITick.Tick(float deltaTime)
        {
            var levelModel = _levelModelService.LevelModel;
            var zones = levelModel.Zones;
            var horizontalSpeed = _heroRunService.GetHeroHorizontalSpeed();
            foreach (var zone in zones)
            {
                zone.Position += horizontalSpeed * deltaTime * Vector2.left;
            }
        }
    }
}