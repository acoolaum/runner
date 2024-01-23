using System;
using Acoolaum.Core.Components;
using UnityEngine;
using UnityEngine.EventSystems;
using Object = UnityEngine.Object;

namespace Acoolaum.Core.Services
{
    public class InputService : IService
    {
        private IInputStrategy _currentInputStrategy;
        private readonly Canvas _canvas;
        private readonly InputControllerBridge _bridge;

        public InputService(Canvas canvas)
        {
            _canvas = canvas;
            _currentInputStrategy = new EmptyInputStrategy();

            var prefab = Resources.Load<InputControllerBridge>("UI/InputController");
            _bridge = Object.Instantiate(prefab, _canvas.transform);
        }

        public void ChangeInputStrategy(IInputStrategy inputStrategy)
        {
            if (_currentInputStrategy != null)
            {
                _currentInputStrategy.Dispose();
            }

            if (inputStrategy != null)
            {
                _currentInputStrategy = inputStrategy;
                _currentInputStrategy.Initialize(_bridge);    
            }
        }

        public void Cancel()
        {
            _currentInputStrategy?.Cancel();
        }

        void IService.Dispose()
        {
            if (_bridge != null)
            {
                Object.Destroy(_bridge.gameObject);
            }
        }
    }

    public interface IInputStrategy : IDisposable
    {
        void Initialize(InputControllerBridge bridge);
        void Cancel();
    }
    
    public interface ISwipeInputStrategy : IInputStrategy, IPointerDownHandler, IBeginDragHandler, IDragHandler,
        IEndDragHandler, IPointerUpHandler
    {
    }
    

    public class EmptyInputStrategy : IInputStrategy
    {
        void IInputStrategy.Initialize(InputControllerBridge bridge)
        {
        }

        void IInputStrategy.Cancel()
        {
        }

        void IDisposable.Dispose()
        {
        }
    }
}