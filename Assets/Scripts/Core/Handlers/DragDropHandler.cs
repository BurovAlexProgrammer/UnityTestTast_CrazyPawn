using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class DragDropHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public event Action<PointerEventData> DragStarted; 
        public event Action<PointerEventData> Dragging; 
        public event Action<PointerEventData> DragFinished;

        public void OnBeginDrag(PointerEventData eventData)
        {
            DragStarted?.Invoke(eventData);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Dragging?.Invoke(eventData);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            DragFinished?.Invoke(eventData);
        }
        
        private void OnDestroy()
        {
            DragStarted = null;
            Dragging = null;
            DragFinished = null;
        }
    }
}

