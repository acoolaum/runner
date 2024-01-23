using Acoolaum.Core.Services;
using Acoolaum.Game.Hero;
using Acoolaum.Game.Level;
using Acoolaum.Game.Ui;
using UnityEngine;

namespace Acoolaum.Game.Services.Level
{
    public class GameStarter : MonoBehaviour
    {
        [SerializeField] private string _levelName;
        [SerializeField] private Canvas _ui;
        [SerializeField] private Camera _uiCamera;
        
        private ServiceContainer _game;
        private ServiceContainer _level;
        private LevelLifetimeService _levelLifetime;

        void Start()
        {
            _game = new ServiceContainer("game");
            _game.Add<ILevelConfigProviderService>(new LevelConfigProviderService());
            _game.Add(new InputService(_ui));
            _game.Initialize();
            
            _level = new ServiceContainer("level", _game);
            _level.Add(new LevelModelService());
            
            _level.Add(new ElementRunActionService());
            _level.Add(new EffectSpawnService());
            
            _level.Add(new HeroHealthService());
            _level.Add(new HeroRunService());
            _level.Add(new HeroFlightService());
            _level.Add(new HeroMovementService());
            _level.Add(new HeroHitService());
            _level.Add(new HeroInteractService());
            _level.Add(new HeroPropertyModificatorsService());
            
            _level.Add(new LevelCreateService(_levelName));
            
            _level.Add(new ZoneMoveService());
            
            _level.Add(new CollisionService());
            _level.Add(new ZoneGenerateService());

            _level.Add(new ZoneElementsDrawService());
            _level.Add(new HeroDrawService());
            
            _level.Add(new HeroBarPresenter(_ui));
            _level.Add(new GameOverWindowPresenter(_ui, GameRestart));
            
            _levelLifetime = new LevelLifetimeService(); 
            _level.Add(_levelLifetime);
            _level.Initialize();
            
            _levelLifetime.Load();
            _levelLifetime.Loaded();
        }

        private void GameRestart()
        {
            _level.Dispose();
            _levelLifetime = null;
            _game.Dispose();
            
            Start();
        }

        private void Update()
        {
            _levelLifetime?.Tick(Time.deltaTime);
            _levelLifetime?.Update(Time.deltaTime);
        }
    }
}