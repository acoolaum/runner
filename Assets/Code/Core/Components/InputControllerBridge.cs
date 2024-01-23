using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Acoolaum.Core.Components
{
    public class InputControllerBridge : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler,
        IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> OnPointerDown;
        public event Action<PointerEventData> OnBeginDrag;
        public event Action<PointerEventData> OnDrag;
        public event Action<PointerEventData> OnEndDrag;
        public event Action<PointerEventData> OnPointerUp;
        

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {
            OnPointerDown?.Invoke(eventData);
        }

        void IPointerUpHandler.OnPointerUp(PointerEventData eventData)
        {
            OnPointerUp?.Invoke(eventData);
        }

        void IBeginDragHandler.OnBeginDrag(PointerEventData eventData)
        {
            OnBeginDrag?.Invoke(eventData);
        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
            OnDrag?.Invoke(eventData);
        }

        void IEndDragHandler.OnEndDrag(PointerEventData eventData)
        {
            OnEndDrag?.Invoke(eventData);
        }
    }
}