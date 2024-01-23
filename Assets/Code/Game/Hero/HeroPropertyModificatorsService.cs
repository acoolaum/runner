using System;
using System.Collections.Generic;
using System.Globalization;
using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using Acoolaum.Game.Level;

namespace Acoolaum.Game.Hero
{
    public class HeroPropertyModificatorsService : ServiceBase, ILoaded, ITick
    {
        private class AddModificatorElementActionRunner : IElementActionRunner
        {
            private const string AddHeroModificatorActionName = "add_hero_modificator";
            
            private readonly HeroPropertyModificatorsService _heroPropertyModificatorSystem;

            public AddModificatorElementActionRunner(HeroPropertyModificatorsService heroPropertyModificatorSystem)
            {
                _heroPropertyModificatorSystem = heroPropertyModificatorSystem;
            }

            bool IElementActionRunner.CanExecute(string actionString)
            {
                return actionString.StartsWith(AddHeroModificatorActionName, StringComparison.Ordinal);
            }

            void IElementActionRunner.Execute(ZoneElementModel elementModel, string actionString)
            {
                var parts = actionString.Split(' ');
                var targetProperty = parts[1];
                var amountString = parts[2];
                if (float.TryParse(amountString, NumberStyles.Float | NumberStyles.Integer, CultureInfo.InvariantCulture, out var amount) == false)
                {
                    throw new ArgumentException($"Can't parse amount in action '{actionString}'");
                }

                if (parts.Length > 3)
                {
                    var durationString = parts[3];
                    if (float.TryParse(durationString, NumberStyles.Float | NumberStyles.Integer, CultureInfo.InvariantCulture, out var duration) == false)
                    {
                        throw new ArgumentException($"Can't parse duration in action '{actionString}'");
                    }

                    _heroPropertyModificatorSystem.AddModificator(targetProperty, amount, duration);
                }
                else
                {
                    _heroPropertyModificatorSystem.AddModificator(targetProperty, amount);
                }
            }
        }
        
        private readonly List<HeroPropertyModificator> _appliedModificators = new();
        
        private LevelLifetimeService _lifetimeService;
        private LevelModelService _levelModelService;

        private void AddModificator(string targetProperty, float amount, float? duration = null)
        {
            var property = _levelModelService.LevelModel.Hero.Properties[targetProperty];
            var modificator = new HeroPropertyModificator(property, amount, duration);
            modificator.Start(_lifetimeService.TimeState.Time);
            _appliedModificators.Add(modificator);
        }

        void ILoaded.Loaded()
        {
            _lifetimeService = ServiceContainer.Get<LevelLifetimeService>();
            _levelModelService = ServiceContainer.Get<LevelModelService>();
            
            var executeService = ServiceContainer.Get<ElementRunActionService>();
            executeService.RegisterRunner(new AddModificatorElementActionRunner(this));
        }

        void ITick.Tick(float tickTime)
        {
            for (var i = 0; i < _appliedModificators.Count; i++)
            {
                var modificator = _appliedModificators[i];
                if (modificator.IsExpired(_lifetimeService.TimeState.Time))
                {
                    modificator.Finish();
                    _appliedModificators.RemoveAt(i);
                    i--;
                }
            }
        }

        public void RemoveAll()
        {
            for (var i = 0; i < _appliedModificators.Count; i++)
            {
                var modificator = _appliedModificators[i];
                modificator.Finish();
            }
            _appliedModificators.Clear();
        }
    }

    public class HeroPropertyModificator
    {
        private readonly IHeroProperty _targetProperty;
        private readonly float _amount;
        private readonly float? _duration;
        private float _startTime;

        public HeroPropertyModificator(IHeroProperty targetProperty, float amount, float? duration)
        {
            _targetProperty = targetProperty;
            _amount = amount;
            _duration = duration;
        }

        public void Start(float currentTime)
        {
            _startTime = currentTime;
            var value = _targetProperty.Value;
            var newValue = value + _amount;
            _targetProperty.TrySetValue(newValue);
        }

        public bool IsExpired(float currentTime)
        {
            if (_duration.HasValue == false)
            {
                return false;
            }

            return _startTime + _duration.Value <= currentTime;
        }

        public void Finish()
        {
            var value = _targetProperty.Value;
            var newValue = value - _amount;
            _targetProperty.TrySetValue(newValue);
        }
    }
}