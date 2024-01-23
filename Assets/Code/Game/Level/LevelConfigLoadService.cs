using System.Collections.Generic;
using Acoolaum.Core.Services;
using Acoolaum.Game.Config;
using UnityEngine;

namespace Acoolaum.Game.Level
{
    public class LevelConfigProviderService : ILevelConfigProviderService
    {
        LevelConfig ILevelConfigProviderService.Load(string levelName)
        {
            var levelConfig = new LevelConfig
            {
                Name = levelName,
                HeroConfig = new HeroConfig
                {
                    Name = "Pink Man", 
                    ViewId = "hero/pink_man",
                    Size = new (26f, 30f),
                    BaseParameters = new()
                    {
                        { "health", 3 },
                        { "max_health", 3 },
                        { "base_run_speed", 100 },
                        { "base_jump_speed", 250 },
                        { "base_jump_charges", 2 },
                        { "base_flight_speed", 100 },
                        { "base_gravity_acceleration", -450},
                    }
                }
            };
            var environmentConfig = new LevelEnvironmentConfig();
            
            var grassStart = environmentConfig.AddElementType("terrain_grass_start", "terrain/terrain_grass_start", true, 16, 48);
            environmentConfig.AddElementType("terrain_grass_middle", "terrain/terrain_grass_start", true, 16, 48);
            environmentConfig.AddElementType("terrain_grass_end", "terrain/terrain_grass_start", true, 16, 48);
            var block = environmentConfig.AddElementType("block", "block/block", true, 16, 16);
            var apple = environmentConfig.AddElementType("fruit_apple", "fruits/apple", false, 12, 12, new []
            {
                "add_health 1",
                "spawn_effect effect/collected"
            });
            var arrowSpeedUp = environmentConfig.AddElementType("arrow_speed_up", "arrow/arrow_speed_up", false, 12, 12, new []
            {
                "add_hero_modificator run_speed_mult 0.5 5",
                "spawn_effect effect/collected"
            });
            var arrowSlowDown = environmentConfig.AddElementType("arrow_slow_down", "arrow/arrow_slow_down", false, 12, 12, new []
            {
                "add_hero_modificator run_speed_mult -0.5 5",
                "spawn_effect effect/collected"
            });
            var pineapple = environmentConfig.AddElementType("fruit_pineapple", "fruits/pineapple", false, 12, 12, new []
            {
                "add_hero_modificator run_speed_mult 0.5 4",
                "add_hero_modificator flight 1 4",
                "spawn_effect effect/collected"
            });
            var frog = environmentConfig.AddElementType("enemy_frog", "enemy/frog", false, 12, 12, new [] {"remove_health 1"});

            environmentConfig.Zones = new List<ZoneGenerationConfig>();
            var zoneSize = 2048; 
            for (int i = 0; i < 10; i++)
            {
                var zone = new ZoneGenerationConfig()
                {
                    ZoneSize = zoneSize,
                    Elements = new List<ZoneGenerationElement>()
                };
                var elements = zone.Elements;
                for (float f = 0 - grassStart.Size.x / 2f; f < zoneSize + grassStart.Size.x / 2f; f += grassStart.Size.x)
                {
                    elements.AddElement(grassStart.Id, f - zoneSize / 2f , - grassStart.Size.y / 2f - 15);
                }
                
                elements.AddHorizontalBlockLine(block.Id, 200f + 8 * block.Size.x, 2 * block.Size.y - block.Size.y / 2f - 15, 4, block.Size.x);
                elements.AddHorizontalBlockLine(block.Id, 200f + 13 * block.Size.x, 3 * block.Size.y - block.Size.y / 2f - 15, 10, block.Size.x);

                elements.AddElement(apple.Id, 200f + 20 * block.Size.x, 7 * block.Size.y - block.Size.y / 2f - 15);
                
                elements.AddHorizontalBlockLine(block.Id, 650f + 8 * block.Size.x, block.Size.y / 2f + 20f, 4, block.Size.x);
                elements.AddHorizontalBlockLine(block.Id, 650f + 13 * block.Size.x, 4 * block.Size.y - block.Size.y / 2f - 15, 10, block.Size.x);
                
                
                elements.AddElement(arrowSlowDown.Id, 650f + 16 * block.Size.x, 0f);
                //
                elements.AddElement(pineapple.Id, 1000, 6 * block.Size.y - block.Size.y / 2f - 1);
                
                elements.AddHorizontalBlockLine(block.Id, -800, 9f * block.Size.y - block.Size.y / 2f - 15, 25, block.Size.x);
                elements.AddElement(frog.Id, -900, 0);
                elements.AddElement(arrowSpeedUp.Id, -800 + 16 * block.Size.x, 11f * block.Size.y - block.Size.y / 2f - 15);
                
                elements.AddVerticalBlockLine(block.Id, -800 + 5f * block.Size.x, 10f * block.Size.y - block.Size.y / 2f - 15, 3, block.Size.y);
                elements.AddVerticalBlockLine(block.Id, -800 + 5f * block.Size.x, 15f * block.Size.y - block.Size.y / 2f - 15, 2, block.Size.y);
                elements.AddVerticalBlockLine(block.Id, -800 + 15f * block.Size.x, 14f * block.Size.y - block.Size.y / 2f - 15, 6, block.Size.y);
                
                elements.AddElement(frog.Id, -600, 0);
                
                environmentConfig.Zones.Add(zone);
            }
            
            levelConfig.Environment = environmentConfig;
            return levelConfig;
        }
    }

    public static class EnvironmentConfigHelper
    {
        public static ElementTypeConfig AddElementType(this LevelEnvironmentConfig config, string id, string viewId, bool isGround, float width, float height)
        {
            if (config.ElementsTypes == null)
            {
                config.ElementsTypes = new List<ElementTypeConfig>();
            }

            var elementType = new ElementTypeConfig
            {
                Id = id,
                ViewId = viewId,
                IsGround = isGround,
                Size = new Vector2(width, height)
            };
            
            config.ElementsTypes.Add(elementType);

            return elementType;
        }
        
        public static ElementTypeConfig AddElementType(this LevelEnvironmentConfig config, string id, string viewId, bool isGround, float width, float height, string [] actions)
        {
            var element = AddElementType(config, id, viewId, isGround, width, height);
            element.Actions = actions;
            return element;
        }

        public static ZoneGenerationElement AddElement(this List<ZoneGenerationElement> elements, string elementType, float x, float y)
        {
            var element = new ZoneGenerationElement()
            {
                Type = elementType,
                Position = new Vector2(x, y)
            };
            elements.Add(element);
            return element;
        }

        public static void AddHorizontalBlockLine(this List<ZoneGenerationElement> elements, string elementType, float x, float y,
            int amount, float offset)
        {
            for (int i = 0; i < amount; i++)
            {
                elements.AddElement(elementType, x + i * offset, y);
            }
        }

        public static void AddVerticalBlockLine(this List<ZoneGenerationElement> elements, string elementType, float x, float y,
            int amount, float offset)
        {
            for (int i = 0; i < amount; i++)
            {
                elements.AddElement(elementType, x, y  + i * offset);
            }
        }
    }

    public interface ILevelConfigProviderService : IService
    {
        LevelConfig Load(string levelName);
    }
}