using System.Collections.Generic;
using UnityEngine;

namespace Acoolaum.Game.Config
{
    public class LevelConfig
    {
        public string Name;
        
        public HeroConfig HeroConfig;
        public LevelEnvironmentConfig Environment;
    }

    public class LevelEnvironmentConfig
    {
        public List<ElementTypeConfig> ElementsTypes;
        public List<ZoneGenerationConfig> Zones;
    }

    public class HeroConfig
    {
        public string Name;
        public string ViewId;
        public Vector2 Size;
        public Dictionary<string, int> BaseParameters;
    }

    public class ElementTypeConfig
    {
        public string Id;
        public Vector2 Size;
        public string ViewId;

        public bool IsGround;
        public string[] Actions;
    }
    
    public class ZoneGenerationConfig
    {
        public float ZoneSize;
        public List<ZoneGenerationElement> Elements;
    }

    public class ZoneGenerationElement
    {
        public string Type;
        public Vector2 Position;
    }
}