using System.Collections.Generic;
using Acoolaum.Game.Config;

namespace Acoolaum.Game.Model
{
    public class LevelModel
    {
        public string Name { get; }
        public LevelConfig Config { get; }
        public HeroModel Hero { get; private set; }
        public IReadOnlyList<ZoneModel> Zones => _zones;

        private readonly List<ZoneModel> _zones;

        public LevelModel(LevelConfig levelConfig)
        {
            Name = levelConfig.Name;
            Config = levelConfig;
            _zones = new List<ZoneModel>();
        }

        public void AddHero(HeroModel hero)
        {
            Hero = hero;
        }

        public void AddZone(ZoneModel zone)
        {
            _zones.Add(zone);
        }

        public void RemoveZone(ZoneModel zone)
        {
            _zones.Remove(zone);
        }

        public void RemoveElement(ZoneElementModel element)
        {
            element.Zone.Elements.Remove(element);
        }
    }
}