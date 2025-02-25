using UnityEngine;

namespace Core
{
    public class Board : MonoBehaviour
    {
        public Transform _transform;

        public Transform Transform => _transform;

        public void Init(Vector3 position, float size)
        {
            _transform = transform;
            _transform.position = position;
        }
    }
}