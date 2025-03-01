using System;
using Core;
using Core.Common;
using Core.Prank;
using CrazyPawn;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;
using Object = UnityEngine.Object;

namespace Services
{
    public class FigureControlService
    {
        [Inject] private CrazyPawnSettings _settings;
        [Inject] private DiContainer _diContainer;
        [Inject] private Board _board;
        
        private Vector3 _dragOffset;

        public event Action<FigureView> FigureDestroyed;

        public void CreateFigure(FigureView prefab, Vector3 position)
        {
            var figure = _diContainer.InstantiatePrefabForComponent<FigureView>(prefab);
            figure.Init();
            figure.transform.position = position;
            figure.DragStarted += Figure_OnDragStarted;
            figure.Dragging += Figure_OnDragging;
            figure.DragFinished += Figure_OnDragFinished;
        }
        
        public void DeleteFigure(FigureView figureView)
        {
            figureView.DragStarted -= Figure_OnDragStarted;
            figureView.Dragging -= Figure_OnDragging;
            figureView.DragFinished -= Figure_OnDragFinished;
            Object.Destroy(figureView.gameObject);
            FigureDestroyed?.Invoke(figureView);
        }

        private void Figure_OnDragFinished(FigureView figureView, PointerEventData eventData)
        {
            _dragOffset = Vector3.zero;
            
            if (figureView.IsDeleteMode)
            {
                DeleteFigure(figureView);
                
                if (_settings.IsPrank)
                    Object.Instantiate<PrankFigure>(_settings.PrankFigurePrefab).Init(figureView.transform);
            }
        }

        private void Figure_OnDragging(FigureView figureView, PointerEventData eventData)
        {
            var raycastPoint = Utils.GetRaycastPosition(eventData);
            
            MoveFigure(figureView, raycastPoint);
            Validate(figureView);
        }

        private void Figure_OnDragStarted(FigureView figureView, PointerEventData eventData)
        {
            var raycastPoint = Utils.GetRaycastPosition(eventData);
            _dragOffset = figureView.Transform.position - raycastPoint;
            
            MoveFigure(figureView, raycastPoint);
            Validate(figureView);
        }

        private void MoveFigure(FigureView figureView, Vector3 raycastPoint)
        {
            figureView.Transform.position = raycastPoint + _dragOffset;
        }

        private void Validate(FigureView figureView)
        {
            var isOutBoard = _board.Bounds.Contains(figureView.Transform.position) == false;
            figureView.SetDeleteMode(isOutBoard);
        }
    }
}