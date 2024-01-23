using System.Collections.Generic;
using Acoolaum.Core.Services;
using Acoolaum.Game.Level;
using Acoolaum.Game.Model;
using Acoolaum.Game.View;
using UnityEngine;

namespace Acoolaum.Game.Services.Level
{
    public class ZoneElementsDrawService : ServiceBase, ILoaded, IUpdate
    {
        private Dictionary<ZoneModel, Dictionary<ZoneElementModel, ElementView>> _views = new();

        void ILoaded.Loaded()
        {
            var levelModel = ServiceContainer.Get<LevelModelService>();
            levelModel.OnZoneAdded += OnZoneAdded;
            levelModel.OnZoneRemoved += OnZoneRemoved;
            levelModel.OnZoneElementRemoved += OnZoneElementRemoved;
            
            var zones = levelModel.LevelModel.Zones;
            for (int i = 0; i < zones.Count; i++)
            {
                AddZoneViews(zones[i]);
            }
        }
        
        void IUpdate.Update(float updateTime)
        {
            foreach (var zone in _views)
            {
                var zonePosition = zone.Key.Position;
                foreach (var element in zone.Value)
                {
                    var elementPosition = zonePosition + element.Key.LocalPosition;
                    element.Value.SetPosition(0.01f * elementPosition);
                }
            }
        }
        
        void IService.Dispose()
        {
            foreach (var elementViews in _views)
            {
                foreach (var view in elementViews.Value)
                {
                    Object.Destroy(view.Value.gameObject);
                }
            }
            _views.Clear();
        }
        
        private void OnZoneAdded(ZoneModel zone)
        {
            AddZoneViews(zone);
        }

        private void AddZoneViews(ZoneModel zone)
        {
            var elementViews = new Dictionary<ZoneElementModel, ElementView>(zone.Elements.Count);
            _views.Add(zone, elementViews);
            for (int i = 0; i < zone.Elements.Count; i++)
            {
                AddElementView(elementViews, zone.Elements[i]);
            }
        }

        private void AddElementView(Dictionary<ZoneElementModel, ElementView> elementViews, ZoneElementModel element)
        {
            var position = element.LocalPosition + element.Zone.Position; 
            var prefab = Resources.Load<ElementView>(element.Config.ViewId);
            var view = Object.Instantiate(prefab);
            view.SetPosition(0.01f * position);
            elementViews.Add(element, view);
        }
        
        private void OnZoneRemoved(ZoneModel zone)
        {
            var elementViews = _views[zone];
            foreach (var view in elementViews)
            {
                Object.Destroy(view.Value.gameObject);
            }

            _views.Remove(zone);
        }
        
        private void OnZoneElementRemoved(ZoneElementModel element)
        {
            var elementViews = _views[element.Zone];
            var view = elementViews[element];
            Object.Destroy(view.gameObject);
            elementViews.Remove(element);
        }
    }
}