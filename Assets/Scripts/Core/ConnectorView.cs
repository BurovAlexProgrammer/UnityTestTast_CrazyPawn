using UnityEngine;
using UnityEngine.EventSystems;

namespace Core
{
    public class ConnectorView : MonoBehaviour
    {
        [SerializeField] private DragDropHandler _dragDropHandler;

        private FigureView _parentFigureView;

        public void Init(FigureView parentFigureView)
        {
            _dragDropHandler.DragStarted += OnDragStarted;
            _dragDropHandler.Dragging += OnDragging;
            _dragDropHandler.DragFinished += OnDragFinished;
            _parentFigureView = parentFigureView;
        }


        private void OnDragFinished(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        private void OnDragging(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }

        private void OnDragStarted(PointerEventData eventData)
        {
            throw new System.NotImplementedException();
        }
    }
}