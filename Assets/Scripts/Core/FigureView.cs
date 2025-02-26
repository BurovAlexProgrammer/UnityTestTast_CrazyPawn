using System;
using CrazyPawn;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core
{
    public class FigureView:MonoBehaviour
    {
        [Inject] private CrazyPawnSettings _settings;
        
        [SerializeField] private DragDropHandler _dragDropHandler;
        [SerializeField] private ConnectorView[] _connectors;
        [SerializeField] private MeshRenderer _meshRenderer;

        private Transform _transform;
        private Material _initMaterial;
        private bool _isDeleteMode;

        public event Action<FigureView, PointerEventData> DragStarted;
        public event Action<FigureView, PointerEventData> Dragging;
        public event Action<FigureView, PointerEventData> DragFinished;

        public Transform Transform => _transform;
        public bool IsDeleteMode => _isDeleteMode;

        public void Init()
        {
            _initMaterial = _meshRenderer.material;
            _transform = transform;
            _dragDropHandler.DragStarted += eventData => DragStarted?.Invoke(this, eventData);;
            _dragDropHandler.Dragging += eventData => Dragging?.Invoke(this, eventData);
            _dragDropHandler.DragFinished += eventData => DragFinished?.Invoke(this, eventData);
            
            foreach (var connectorView in _connectors)
            {
                connectorView.Init(this);
            }
        }

        public void SetDeleteMode(bool isDeleteMode)
        {
            _meshRenderer.material = isDeleteMode ? _settings.DeleteMaterial : _initMaterial;
            _isDeleteMode = isDeleteMode;
        }
    }
}