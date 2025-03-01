using System;
using CrazyPawn;
using Services;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Core
{
    public class SocketView : MonoBehaviour
    {
        [Inject] private CrazyPawnSettings _settings;
        [Inject] private SocketService _socketService;
        [Inject] private InputService _inputService;

        [SerializeField] private SelectableHandler _selectableHandler;
        [SerializeField] private MeshRenderer _meshRenderer;

        public event Action<SocketView, bool> SelectionChanged;
        public event Action<SocketView, PointerEventData> DragStarted;
        public event Action<SocketView, PointerEventData> Dragging;
        public event Action<SocketView, PointerEventData> DragFinished;

        private FigureView _parentFigureView;
        private GameObject _gameObject;
        private Transform _transform;
        private Material _initialMaterial;
        private bool _isDragStarted;

        public FigureView ParentFigureView => _parentFigureView;
        public bool IsSelected => _selectableHandler.Selected;
        public Transform Transform => _transform;

        public void Init(FigureView parentFigureView)
        {
            _gameObject = gameObject;
            _transform = transform;
            _initialMaterial = _meshRenderer.material;
            _selectableHandler.SelectionChanged += OnSelectionChanged;
            _parentFigureView = parentFigureView;
            _socketService.RegisterConnector(this);
            _inputService.DragStarted += InputService_OnDragStarted;
            _inputService.Dragging += InputService_OnDragging;
            _inputService.DragFinished += InputService_OnDragFinished;
        }

        public void SetDeleteMode(bool isDeleteMode)
        {
            _meshRenderer.material = isDeleteMode ? _settings.DeleteMaterial : _initialMaterial;
        }

        public void SetSelection(bool selected)
        {
            _selectableHandler.SetSelection(selected);
            Refresh();
        }

        private void InputService_OnDragStarted(GameObject obj, PointerEventData eventData)
        {
            if (obj != _gameObject) return;
            
            _isDragStarted = true;
            DragStarted?.Invoke(this, eventData);
        }

        private void InputService_OnDragging(GameObject obj, PointerEventData eventData)
        {
            if (_isDragStarted)
                Dragging?.Invoke(this, eventData);
        }

        private void InputService_OnDragFinished(GameObject obj, PointerEventData eventData)
        {
            if (_isDragStarted)
            {
                _isDragStarted = false;
                DragFinished?.Invoke(this, eventData);
            }
        }

        private void OnSelectionChanged(bool selected)
        {
            SelectionChanged?.Invoke(this, selected);
            Refresh();
        }

        private void Refresh()
        {
            _meshRenderer.material = IsSelected ? _settings.ActiveConnectorMaterial : _initialMaterial;
        }
    }
}