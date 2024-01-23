using System;
using Acoolaum.Core.Services;
using Acoolaum.Game.Hero;
using Acoolaum.Game.Level;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Acoolaum.Game.Ui
{
    public class GameOverWindowPresenter : ServiceBase, ILoad
    {
        private GameOverWindow _gameOverWindow;
        private readonly Canvas _canvas;
        private Action _restartGameAction;

        public GameOverWindowPresenter(Canvas canvas, Action restartGameAction)
        {
            _canvas = canvas;
            _restartGameAction = restartGameAction;
        }
        
        private void OnButtonClicked()
        {
            _restartGameAction?.Invoke();
        }

        void ILoad.Load()
        {
            ServiceContainer.Get<HeroHealthService>().HeroDied += OnHeroDie;
        }

        void IService.Dispose()
        {
            if (_gameOverWindow != null)
            {
                Object.Destroy(_gameOverWindow.gameObject);
                _restartGameAction = null;
            }
        }

        private void OnHeroDie()
        {
            var prefab = Resources.Load<GameOverWindow>("UI/GameOverWindow");
            _gameOverWindow = Object.Instantiate(prefab, _canvas.transform);
            _gameOverWindow.OnButtonClicked += OnButtonClicked;
        }
    }
}