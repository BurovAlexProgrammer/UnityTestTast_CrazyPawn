using System;
using Core;
using Services;
using UnityEngine;
using Zenject;

namespace EntryPoint
{
    public class DemoEntryPoint : MonoBehaviour
    {
        [Inject] private BoardGenerator _boardGenerator;
        [Inject] private FigureSpawner _figureSpawner;
        [Inject] private DiContainer _diContainer;

        private void Awake()
        {
            var board = _boardGenerator.Generate(Vector3.zero);
            _figureSpawner.SpawnFigures(board.Transform.position);
        }
    }
}