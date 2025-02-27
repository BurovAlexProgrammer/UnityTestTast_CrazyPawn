using System.Collections.Generic;
using System.Linq;
using Core;
using CrazyPawn;
using UnityEngine;
using Zenject;

namespace Services
{
    public class SocketService : IInitializable
    {
        [Inject] private CrazyPawnSettings _settings;
        // [Inject] private ConnectionService _connectionService;

        private List<SocketView> _socketsList;
        
        private int _selectionsCount;

        public void Initialize()
        {
            _socketsList = new List<SocketView>(_settings.InitialPawnCount * 4);
        }
        
        public void RegisterConnector(SocketView socketView)
        {
            socketView.gameObject.name = $"Socket{_socketsList.Count}";
            _socketsList.Add(socketView);
            socketView.SelectionChanged += ConnectorView_OnSelectionChanged;
        }

        private void ConnectorView_OnSelectionChanged(SocketView socketView, bool selected)
        {
            return;
            _selectionsCount = selected ? _selectionsCount + 1 : _selectionsCount - 1;

            if (_selectionsCount == 2)
            {
                TryToConnect();

                foreach (var connector in _socketsList)
                {
                    connector.SetSelection(false);
                }
            }
            Debug.Log($"Selected: {selected}");
        }

        private void TryToConnect()
        {
            var selectedSockets = _socketsList.Where(x => x.IsSelected).ToArray();

            if (selectedSockets[0].ParentFigureView == selectedSockets[1].ParentFigureView)
            {
                Debug.LogWarning("Selected sockets' parents are the same. Connection skipped. ");
                return;
            }

            // _connectionService.CreateConnection(selectedSockets[0], selectedSockets[1]);
        }
    }
}