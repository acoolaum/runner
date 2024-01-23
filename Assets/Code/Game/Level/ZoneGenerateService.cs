using System.Collections.Generic;
using Acoolaum.Core.Services;
using Acoolaum.Game.Model;
using UnityEngine;

namespace Acoolaum.Game.Level
{
    public class ZoneGenerateService : ServiceBase, ILoaded, ITick
    {
        private LevelModelService _levelModel;

        void ILoaded.Loaded()
        {
            _levelModel = ServiceContainer.Get<LevelModelService>();
            Generate();
        }

        void ITick.Tick(float tickTime)
        {
            Generate();
            CleanUp();
        }

        void Generate()
        {
            var zones = _levelModel.LevelModel.Zones;

            while (zones.Count == 0 || zones[zones.Count - 1].Position.x < 0)
            {
                float? newZoneOffset = null;
                if (zones.Count > 0)
                {
                    var lastZone = zones[zones.Count - 1];
                    newZoneOffset = lastZone.Position.x + lastZone.Config.ZoneSize / 2f;
                }
                var newZone = GenerateNew(newZoneOffset);
                _levelModel.AddZone(newZone);
                zones = _levelModel.LevelModel.Zones;
            }
        }

        void CleanUp()
        {
            var zones = _levelModel.LevelModel.Zones;
            for (int i = 0; i < zones.Count - 1; i++)
            {
                var zone = zones[i];
                if (zone.Position.x + zone.Config.ZoneSize < -1024)
                {
                    _levelModel.RemoveZone(zone);
                    i--;
                }
            }
        }

        private ZoneModel GenerateNew(float? offset)
        {
            var environmentConfig = _levelModel.LevelModel.Config.Environment;
            var c = environmentConfig.Zones[0];
            var zoneModel = new ZoneModel
            {
                Config = c,
                Position = offset == null ? new Vector2(0, 0) : new Vector2(offset.Value + c.ZoneSize / 2f, 0f),
                Elements = new List<ZoneElementModel>(c.Elements.Count)
            };

            for (var i = 0; i < c.Elements.Count; i++)
            {
                var generationElement = c.Elements[i];
                var elementTypeConfig = environmentConfig.ElementsTypes.Find(e => e.Id == generationElement.Type);
                var element = new ZoneElementModel
                {
                    LocalPosition = generationElement.Position,
                    Zone = zoneModel,
                    Config = elementTypeConfig
                };
                zoneModel.Elements.Add(element);
            }

            return zoneModel;
        }
    }
}