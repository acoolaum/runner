using Acoolaum.Core.Services;
using Acoolaum.Game.Hero;
using Acoolaum.Game.Level;
using Acoolaum.Game.Model;
using Acoolaum.Game.Ui;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Acoolaum.Game.Services.Level
{
    public class HeroBarPresenter : ServiceBase, ILoaded
    {
        private readonly Canvas _canvas;
        private HeroBar _heroBar;

        public HeroBarPresenter(Canvas canvas)
        {
            _canvas = canvas;
        }

        void ILoaded.Loaded()
        {
            var levelModel = ServiceContainer.Get<LevelModelService>();
            levelModel.OnHeroAdded += OnHeroAdded;

            var hero = levelModel.LevelModel.Hero;
            if (hero != null)
            {
                AddHeroView(hero);
            }
        }

        void IService.Dispose()
        {
            if (_heroBar != null)
            {
                Object.Destroy(_heroBar.gameObject);
                _heroBar = null;
            }
        }

        private void OnHeroAdded(HeroModel hero)
        {
            AddHeroView(hero);
        }

        private void AddHeroView(HeroModel hero)
        {
            var heroHealth = hero.Properties[HeroHealthService.Health];
            heroHealth.OnPropertyChanged += OnHeroHealthChanged;
            var prefab = Resources.Load<HeroBar>("UI/HeroBar");
            _heroBar = Object.Instantiate(prefab, _canvas.transform);
            _heroBar.ShowName(hero.HeroConfig.Name);
            _heroBar.Init(hero.Properties[HeroHealthService.MaxHealth].Value);
            _heroBar.ShowHealth(heroHealth.Value);
        }

        private void OnHeroHealthChanged(IHeroProperty property, float from, float newValue)
        {
            _heroBar.ShowHealth(newValue);
        }
    }
}