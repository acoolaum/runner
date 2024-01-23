using System;
using Acoolaum.Core.Components;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Acoolaum.Core.Services
{
    public class DragInputStrategy : ISwipeInputStrategy
    {
        public event Action<Vector3> OnDragBegin;
        public event Action<Vector3> OnDrag;
        public event Action<Vector3> OnDragEnd;
        private InputControllerBridge _bridge;
        private bool _dragging;

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
            _dragging = false;
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
            _dragging = true;
            OnDragBegin?.Invoke(eventData.position);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            if (_dragging == false)
            {
                return;
            }
            OnDrag?.Invoke(eventData.position);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            if (_dragging == false)
            {
                return;
            }
            OnDrag?.Invoke(eventData.position);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            if (_dragging == false)
            {
                return;
            }
            OnDragEnd?.Invoke(eventData.position);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            if (_dragging == false)
            {
                return;
            }

            if (eventData.dragging == false)
            {
                OnDragEnd?.Invoke(eventData.position);                
            }
        }
    }
}