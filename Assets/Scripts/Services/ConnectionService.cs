using System;
using System.Collections.Generic;
using System.Linq;
using Core;
using Core.Prank;
using CrazyPawn;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Services
{
    public class ConnectionService: IInitializable, IDisposable
    {
        [Inject] private CrazyPawnSettings _settings;
        [Inject] private DiContainer _diContainer;
        [Inject] private FigureControlService _figureControl;

        private List<ConnectionView> _connectionViews = new(4);

        public void Initialize()
        {
            _figureControl.FigureDestroyed += OnFigureDestroyed;
        }

        public void Dispose()
        {
            _figureControl.FigureDestroyed -= OnFigureDestroyed;
        }

        public void CreateConnection(SocketView socketViewStart, SocketView socketViewEnd)
        {
            var newConnection = _diContainer.InstantiatePrefabForComponent<ConnectionView>(_settings.ConnectionViewPrefab);
            newConnection.Init(socketViewStart, socketViewEnd);
            _connectionViews.Add(newConnection);
        }

        private void OnFigureDestroyed(FigureView figureView)
        {
            var dependConnections = _connectionViews
                .Where(x => x.IsParentFigure(figureView))
                .ToArray();

            foreach (var connection in dependConnections)
            {
                if (_settings.IsPrank)
                {
                    var prankLine = Object.Instantiate<PrankLine>(_settings.PrankLinePrefab);
                    prankLine.Init(connection.LineRenderer);
                }
                
                connection.Destroy();
                _connectionViews.Remove(connection);
            }
        }
    }
}