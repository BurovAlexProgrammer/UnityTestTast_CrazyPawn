using System;
using Core;
using Services;
using UnityEngine;
using Zenject;

namespace EntryPoint
{
    public class DemoEntryPoint : MonoBehaviour
    {
        [Inject] private Board _board;
        [Inject] private FigureSpawner _figureSpawner;

        private void Awake()
        {
            _figureSpawner.SpawnFigures(_board.Transform.position);
        }
    }
}