using System;
using Acoolaum.Core.Services;
using Acoolaum.Game.Model;

namespace Acoolaum.Game.Level
{
    public class LevelModelService : IService
    {
        public event Action<HeroModel> OnHeroAdded;
        public event Action<ZoneModel> OnZoneAdded;
        public event Action<ZoneModel> OnZoneRemoved;
        public event Action<ZoneElementModel> OnZoneElementRemoved;
        
        public LevelModel LevelModel { get; private set; }
        public void Set(LevelModel levelModel)
        {
            LevelModel = levelModel;
        }

        public void AddHero(HeroModel heroModel)
        {
            LevelModel.AddHero(heroModel);
            OnHeroAdded?.Invoke(heroModel);
        }

        public void AddZone(ZoneModel zone)
        {
            LevelModel.AddZone(zone);
            OnZoneAdded?.Invoke(zone);
        }
        
        public void RemoveZone(ZoneModel zone)
        {
            LevelModel.RemoveZone(zone);
            OnZoneRemoved?.Invoke(zone);
        }
        
        public void RemoveElement(ZoneElementModel element)
        {
            LevelModel.RemoveElement(element);
            OnZoneElementRemoved?.Invoke(element);
        }
    }
}