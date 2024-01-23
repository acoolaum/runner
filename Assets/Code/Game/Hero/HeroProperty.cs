using System;
using Acoolaum.Game.Model;

namespace Acoolaum.Game.Hero
{
    public class HeroProperty :IHeroProperty
    {
        public event Action<IHeroProperty, float, float> OnPropertyChanged;

        public string Name { get; }

        public float Value { get; private set; }

        private readonly IHeroPropertyValueChecker _checker;
        

        public HeroProperty(string name, float value, IHeroPropertyValueChecker checker = null)
        {
            Name = name;
            Value = value;
            _checker = checker;
        }

        bool IHeroProperty.TrySetValue(float newValue)
        {
            if (_checker != null && _checker.Check(newValue) == false)
            {
                return false;
            }

            if (Math.Abs(Value - newValue) > 0.0001f)
            {
                var oldValue = Value;
                Value = newValue;
                OnPropertyChanged?.Invoke(this, oldValue, newValue);
            }
            
            return true;
        }
    }

    public interface IHeroPropertyValueChecker
    {
        bool Check(float newValue);
    }
}