﻿using Core;
using CrazyPawn;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Services
{
    public class FigureControlService
    {
        [Inject] private CrazyPawnSettings _settings;
        [Inject] private DiContainer _diContainer;
        [Inject] private Board _board;
        private Vector3 _dragOffset;

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
        }

        private void Figure_OnDragFinished(FigureView figureView, PointerEventData eventData)
        {
            _dragOffset = Vector3.zero;
            
            if (figureView.IsDeleteMode)
            {
                DeleteFigure(figureView);
            }
        }

        private void Figure_OnDragging(FigureView figureView, PointerEventData eventData)
        {
            var raycastPoint = GetRaycastPosition(figureView, eventData);
            
            MoveFigure(figureView, raycastPoint);
            Validate(figureView);
        }

        private void Figure_OnDragStarted(FigureView figureView, PointerEventData eventData)
        {
            var raycastPoint = GetRaycastPosition(figureView, eventData);
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
        
        private static Vector3 GetRaycastPosition(FigureView figureView, PointerEventData eventData)
        {
            // var delta = eventData.delta * _settings.DragSensitivity;
            // figureView.Transform.Translate(delta.x, 0f, delta.y);
            var screenPosition = eventData.position;
            var ray = Camera.main.ScreenPointToRay(screenPosition);
            var offset = -ray.origin.y / ray.direction.y;
            var raycastPoint = ray.origin + ray.direction * offset;

            return raycastPoint;
        }
    }
}