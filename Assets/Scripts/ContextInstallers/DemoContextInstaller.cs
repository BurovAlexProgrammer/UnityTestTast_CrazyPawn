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
            Container.Bind<CrazyPawnSettings>().FromInstance(_crazyPawnSettings).AsSingle();
            Container.Bind<BoardGenerator>().FromNew().AsSingle();
            Container.Bind<FigureSpawner>().FromInstance(_figureSpawner).AsSingle();
        }
    }
}