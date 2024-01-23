using System.Collections.Generic;
using Acoolaum.Game.Config;
using UnityEngine;

namespace Acoolaum.Game.Model
{
    public class HeroModel
    {
        public Vector2 CurrentPosition;
        public Vector2 PreviousPosition;
        
        public float CurrentVerticalSpeed;
        public bool OnGround;
        
        public readonly Dictionary<string, IHeroProperty> Properties = new ();

        public HeroConfig HeroConfig { get; }

        public HeroModel(HeroConfig heroConfig)
        {
            HeroConfig = heroConfig;
            CurrentPosition = new Vector2(-130, 0);
        }

        public void AddProperty(IHeroProperty property)
        {
            Properties.Add(property.Name, property);
        }
    }

    public class ZoneModel
    {
        public Vector2 Position;
        public ZoneGenerationConfig Config;
        public List<ZoneElementModel> Elements;
    }

    public class ZoneElementModel
    {
        public Vector2 Position => Zone.Position + LocalPosition;
        
        public Vector2 LocalPosition;
        public ElementTypeConfig Config;
        
        public ZoneModel Zone;
    }
}