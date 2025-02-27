using Core;
using CrazyPawn;
using Services;
using UnityEngine;
using Zenject;

namespace ContextInstallers
{
    public class DemoContextInstaller : MonoInstaller
    {
        [SerializeField] private CrazyPawnSettings _crazyPawnSettings;
        [SerializeField] private FigureSpawner _figureSpawner;

        public override void InstallBindings()
        {
            var board = BoardGenerator.Generate(_crazyPawnSettings, Vector3.zero);
            
            Container.Bind<CrazyPawnSettings>().FromInstance(_crazyPawnSettings).AsSingle();
            Container.Bind<Board>().FromInstance(board).AsSingle();
            Container.Bind<FigureSpawner>().FromInstance(_figureSpawner).AsSingle();
            Container.Bind<FigureControlService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<SocketService>().FromNew().AsSingle();
            Container.BindInterfacesAndSelfTo<ConnectionService>().FromNew().AsSingle();
        }
    }
}