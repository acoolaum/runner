using System;
using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public class HeroHealthService : ServiceBase, ILoad
    {
        public event Action HeroDied;
        
        public const string Health = "health";
        public const string MaxHealth = "max_health";
        
        private LevelModelService _levelModelService;
        private LevelLifetimeService _levelLifetimeService;

        private class HealthChecker : IHeroPropertyValueChecker
        {
            private readonly IHeroProperty _maxHealthProperty;
            
            public HealthChecker(IHeroProperty maxHealthProperty)
            {
                _maxHealthProperty = maxHealthProperty;
            }

            bool IHeroPropertyValueChecker.Check(float newHealthValue)
            {
                return _maxHealthProperty.Value >= newHealthValue;
            }
        }
        
        private class ModifyHealthElementActionRunner : IElementActionRunner
        {
            private const string AddHealthActionName = "add_health";
            private const string RemoveHealthActionName = "remove_health";
            
            private readonly HeroHealthService _heroHealthSystem;

            public ModifyHealthElementActionRunner(HeroHealthService heroHealthSystem)
            {
                _heroHealthSystem = heroHealthSystem;
            }

            bool IElementActionRunner.CanExecute(string actionString)
            {
                return actionString.StartsWith(AddHealthActionName, StringComparison.Ordinal) ||
                       actionString.StartsWith(RemoveHealthActionName, StringComparison.Ordinal);
            }

            void IElementActionRunner.Execute(ZoneElementModel elementModel, string actionString)
            {
                var parts = actionString.Split(' ');
                var amountString = parts[1];
                if (float.TryParse(amountString, out var health) == false)
                {
                    throw new ArgumentException($"Can't parse health amount in action '{amountString}'");
                }

                if (actionString.StartsWith(AddHealthActionName, StringComparison.Ordinal))
                {
                    _heroHealthSystem.AddHealth(health);
                }
                else
                {
                    _heroHealthSystem.RemoveHealth(health);
                }
            }
        }
        
        public float GetHealth()
        {
            var hero = _levelModelService.LevelModel.Hero;
            return hero.Properties[Health].Value;
        }
        
        public void RemoveHealth(float health)
        {
            var hero = _levelModelService.LevelModel.Hero;
            var healthProperty = hero.Properties[Health];
            var newHealth = healthProperty.Value - health;
            healthProperty.TrySetValue(newHealth);
            if (newHealth <= 0f)
            {
                _levelLifetimeService.Pause(true);
                HeroDied?.Invoke();
            }
        }

        public void AddHealth(float health)
        {
            var hero = _levelModelService.LevelModel.Hero;
            var healthProperty = hero.Properties[Health];
            var newHealth = healthProperty.Value + health;
            healthProperty.TrySetValue(newHealth);
        }

        void ILoad.Load()
        {
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            _levelModelService.OnHeroAdded += OnHeroAdded;

            _levelLifetimeService = ServiceContainer.Get<LevelLifetimeService>();
            
            var executeService = ServiceContainer.Get<ElementRunActionService>();
            executeService.RegisterRunner(new ModifyHealthElementActionRunner(this));
        }

        private void OnHeroAdded(HeroModel hero)
        {
            var maxHealthProperty = new HeroProperty(MaxHealth, hero.HeroConfig.BaseParameters[MaxHealth]);
            hero.AddProperty(maxHealthProperty);
            var healthProperty = new HeroProperty(Health, hero.HeroConfig.BaseParameters[Health], new HealthChecker(maxHealthProperty));
            hero.AddProperty(healthProperty);
        }
    }
}