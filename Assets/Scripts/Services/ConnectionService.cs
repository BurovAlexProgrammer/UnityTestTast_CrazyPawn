using System.Collections.Generic;
using Core;
using CrazyPawn;
using Zenject;

namespace Services
{
    public class ConnectionService: ITickable
    {
        [Inject] private CrazyPawnSettings _settings;
        [Inject] private DiContainer _diContainer;

        private List<ConnectionView> _connectionViews = new(4);
        
        public void CreateConnection(SocketView socketViewStart, SocketView socketViewEnd)
        {
            var newConnection = _diContainer.InstantiatePrefabForComponent<ConnectionView>(_settings.ConnectionViewPrefab);
            newConnection.Init(socketViewStart, socketViewEnd);
            _connectionViews.Add(newConnection);
        }

        public void Tick()
        {
            foreach (var connectionView in _connectionViews)
            {
                connectionView.UpdateLine();
            }
        }
    }
}