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
        
        [SerializeField] private DragDropHandler _dragDropHandler;
        [SerializeField] private SelectableHandler _selectableHandler;
        [SerializeField] private MeshRenderer _meshRenderer;

        public event Action<SocketView, bool> SelectionChanged;

        private FigureView _parentFigureView;
        private Material _initialMaterial;
        private Transform _transform;

        public FigureView ParentFigureView => _parentFigureView;
        public bool IsSelected => _selectableHandler.Selected;
        public Transform Transform => _transform;

        public void Init(FigureView parentFigureView)
        {
            _transform = transform;
            _initialMaterial = _meshRenderer.material;
            _selectableHandler.SelectionChanged += OnSelectionChanged;
            _dragDropHandler.DragStarted += OnDragStarted;
            _dragDropHandler.Dragging += OnDragging;
            _dragDropHandler.DragFinished += OnDragFinished;
            _parentFigureView = parentFigureView;
            _socketService.RegisterConnector(this);
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
        
        private void OnSelectionChanged(bool selected)
        {
            SelectionChanged?.Invoke(this, selected);
            Refresh();
        }

        private void Refresh()
        {
            _meshRenderer.material = IsSelected ? _settings.ActiveConnectorMaterial : _initialMaterial;
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