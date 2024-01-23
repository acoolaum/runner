using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;
using UnityEngine;

namespace Acoolaum.Game.Hero
{
    public class HeroRunService : ServiceBase, ILoad
    {
        const string BaseRunSpeed = "base_run_speed";
        const string RunSpeedMultiplier = "run_speed_mult";
        public const string BaseGravityAcceleration = "base_gravity_acceleration";
        
        private LevelModelService _levelModelService;

        public float GetHeroHorizontalSpeed()
        {
            var properties = _levelModelService.LevelModel.Hero.Properties;
            return properties[BaseRunSpeed].Value * Mathf.Max(0.1f, Mathf.Min(2f, 1 + properties[RunSpeedMultiplier].Value));
        }

        void ILoad.Load()
        {
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _levelModelService.OnHeroAdded += OnHeroAdded;
        }

        private void OnHeroAdded(HeroModel heroModel)
        {
            var hero = _levelModelService.LevelModel.Hero;
            var baseRunSpeed = new HeroProperty(BaseRunSpeed, heroModel.HeroConfig.BaseParameters[BaseRunSpeed]);
            hero.AddProperty(baseRunSpeed);
            var runSpeedMultiplier = new HeroProperty(RunSpeedMultiplier, 0f);
            hero.AddProperty(runSpeedMultiplier);
        }
    }
}