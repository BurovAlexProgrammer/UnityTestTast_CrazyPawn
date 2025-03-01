using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Common;
using CrazyPawn;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Services
{
    public class SocketService : IInitializable, ITickable
    {
        [Inject] private DiContainer _diContainer;
        [Inject] private CrazyPawnSettings _settings;
        [Inject] private ConnectionService _connectionService;
        [Inject] private FigureControlService _figureControl;

        private List<SocketView> _socketsList;
        
        private LineRenderer _dragConnectionLineRenderer;
        private SocketView _startDragSocket;
        private int _selectionsCount;
        private bool _isConnectionDragging;

        public void Initialize()
        {
            _socketsList = new List<SocketView>(_settings.InitialPawnCount * 4);
            _figureControl.FigureDestroyed += OnFigureDestroyed;
            _dragConnectionLineRenderer = _diContainer.InstantiatePrefabForComponent<LineRenderer>(_settings.ConnectionLinePrefab);
            _dragConnectionLineRenderer.widthMultiplier = 0.1f;
            _dragConnectionLineRenderer.gameObject.SetActive(false);
        }

        public void Tick()
        {
            if (_isConnectionDragging)
            {
                var cursorPosition = Input.mousePosition;//Not for mobile
                
                _dragConnectionLineRenderer.SetPositions(new []
                {
                    _startDragSocket.Transform.position,
                    Utils.GetRaycastPosition(cursorPosition, _startDragSocket.Transform.position.y)
                });
            }
        }

        private void OnFigureDestroyed(FigureView figureView)
        {
            var dependedSockets = _socketsList
                .Where(x => x.ParentFigureView == figureView)
                .ToArray();

            foreach (var socket in dependedSockets)
            {
                socket.SelectionChanged -= ConnectorView_OnSelectionChanged;
                socket.DragStarted -= OnSocketDragStarted;
                socket.Dragging -= OnSocketDragging;
                socket.DragFinished -= OnSocketDragFinished;
                Object.Destroy(socket.gameObject);
                _socketsList.Remove(socket);
            }
        }

        public void RegisterConnector(SocketView socketView)
        {
            socketView.gameObject.name = $"Socket{_socketsList.Count}";
            _socketsList.Add(socketView);
            socketView.SelectionChanged += ConnectorView_OnSelectionChanged;
            socketView.DragStarted += OnSocketDragStarted;
            socketView.Dragging += OnSocketDragging;
            socketView.DragFinished += OnSocketDragFinished;
        }

        private void OnSocketDragging(SocketView arg1, PointerEventData arg2)
        {
            _isConnectionDragging = true;
            _dragConnectionLineRenderer.gameObject.SetActive(true);
        }

        private void OnSocketDragStarted(SocketView socket, PointerEventData eventData)
        {
            _startDragSocket = socket;
            _startDragSocket.SetSelection(true);
        }

        private void OnSocketDragFinished(SocketView socket, PointerEventData eventData)
        {
            _dragConnectionLineRenderer.gameObject.SetActive(false);
            var endDragSocket = eventData.pointerEnter.GetComponent<SocketView>();
            
            if (endDragSocket != null && endDragSocket != _startDragSocket)
            {
                endDragSocket.SetSelection(true);
            }

            _startDragSocket = null;
            _isConnectionDragging = false;
        }

        private void ConnectorView_OnSelectionChanged(SocketView socketView, bool selected)
        {
            _selectionsCount = _socketsList.Count(x => x.IsSelected);

            if (_selectionsCount == 2)
            {
                TryToConnect();

                foreach (var connector in _socketsList)
                {
                    connector.SetSelection(false);
                }
            }
        }

        private void TryToConnect()
        {
            var selectedSockets = _socketsList.Where(x => x.IsSelected).ToArray();

            if (selectedSockets[0].ParentFigureView == selectedSockets[1].ParentFigureView)
            {
                Debug.LogWarning("Selected sockets' parents are the same. Connection skipped. ");
                return;
            }

            _connectionService.CreateConnection(selectedSockets[0], selectedSockets[1]);
        }
    }
}