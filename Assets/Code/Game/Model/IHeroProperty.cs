using System;

namespace Acoolaum.Game.Model
{
    public interface IHeroProperty
    {
        event Action<IHeroProperty, float, float> OnPropertyChanged;

        string Name { get; }
        float Value { get; }

        bool TrySetValue(float newValue);
    }
}