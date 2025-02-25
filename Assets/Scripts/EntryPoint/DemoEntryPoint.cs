using System;
using Services;
using UnityEngine;
using Zenject;

namespace EntryPoint
{
    public class DemoEntryPoint : MonoBehaviour
    {
        [Inject] private BoardGenerator _boardGenerator;

        private void Awake()
        {
            _boardGenerator.Generate(Vector3.zero);
        }
    }
}