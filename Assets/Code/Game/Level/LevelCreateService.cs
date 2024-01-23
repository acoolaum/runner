using Acoolaum.Core.Services;
using Acoolaum.Game.Model;

namespace Acoolaum.Game.Level
{
    public class LevelCreateService : ILoad
    {
        private readonly string _levelName;
        private ILevelConfigProviderService _levelConfigProvider;
        private LevelModelService _levelModelService;

        public LevelCreateService(string levelName)
        {
            _levelName = levelName;
        }

        void IService.Initialize(ServiceContainer serviceContainer)
        {
            _levelConfigProvider = serviceContainer.Get<ILevelConfigProviderService>();
            _levelModelService = serviceContainer.Get<LevelModelService>();
        }

        void ILoad.Load()
        {
            var config = _levelConfigProvider.Load(_levelName);
            var model = new LevelModel(config);
            _levelModelService.Set(model);
            _levelModelService.AddHero(new HeroModel(config.HeroConfig));
        }
    }
}