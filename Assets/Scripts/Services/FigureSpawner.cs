﻿using Core;
using CrazyPawn;
using UnityEngine;
using Zenject;

namespace Services
{
    public class FigureSpawner: MonoBehaviour
    {
        [Inject] private CrazyPawnSettings _crazyPawnSettings;
        [Inject] private DiContainer _diContainer;

        [SerializeField] private FigureView _figurePrefab;

        public void SpawnFigures(Vector3 center)
        {
            var spawnRadius = _crazyPawnSettings.InitialZoneRadius;
            var spawnCount = _crazyPawnSettings.InitialPawnCount;

            for (var i = 0; i < spawnCount; i++)
            {
                var figure = _diContainer.InstantiatePrefabForComponent<FigureView>(_figurePrefab);
                figure.Init();
                figure.transform.position = GetRandomPointInRadius(center, spawnRadius);
            }
        }
        
        Vector3 GetRandomPointInRadius(Vector3 center, float radius)
        {
            Vector2 randomPoint2D = Random.insideUnitCircle * radius;
            return new Vector3(center.x + randomPoint2D.x, center.y, center.z + randomPoint2D.y);
        }
    }
}