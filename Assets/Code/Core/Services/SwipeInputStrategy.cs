using System;
using System.Collections.Generic;
using Acoolaum.Core.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Acoolaum.Core.Services
{
    public class SwipeInputStrategy : ISwipeInputStrategy
    {
        public event Action<List<Vector3>> SwipeCompleted;
        private InputControllerBridge _bridge;
        private readonly List<Vector3> _swipePositions = new (100);

        void IInputStrategy.Initialize(InputControllerBridge bridge)
        {
            _bridge = bridge;
            
            var swipe = (ISwipeInputStrategy)this;
            bridge.OnPointerDown += swipe.OnPointerDown;
            bridge.OnBeginDrag += swipe.OnBeginDrag;
            bridge.OnDrag += swipe.OnDrag;
            bridge.OnEndDrag += swipe.OnEndDrag;
            bridge.OnPointerUp += swipe.OnPointerUp;
        }
        
        void IInputStrategy.Cancel()
        {
            _swipePositions.Clear();
        }
        
        void IDisposable.Dispose()
        {
            var swipe = (ISwipeInputStrategy)this;
            _bridge.OnPointerDown -= swipe.OnPointerDown;
            _bridge.OnBeginDrag -= swipe.OnBeginDrag;
            _bridge.OnDrag -= swipe.OnDrag;
            _bridge.OnEndDrag -= swipe.OnEndDrag;
            _bridge.OnPointerUp -= swipe.OnPointerUp;
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            eventData.useDragThreshold = false;
            _swipePositions.Clear();
            _swipePositions.Add(eventData.position);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (_swipePositions.Count == 0)
            {
                return;
            }
            _swipePositions.Add(eventData.position);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (_swipePositions.Count == 0)
            {
                return;
            }
            _swipePositions.Add(eventData.position);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (_swipePositions.Count == 0)
            {
                return;
            }
            _swipePositions.Add(eventData.position);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (_swipePositions.Count == 0)
            {
                return;
            }
            _swipePositions.Add(eventData.position);
            SwipeCompleted?.Invoke(_swipePositions);
        }
    }
}