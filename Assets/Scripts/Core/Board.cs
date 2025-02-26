using UnityEngine;

namespace Core
{
    public class Board : MonoBehaviour
    {
        public Transform _transform;
        private float _size;
        private Bounds _bounds;

        public Transform Transform => _transform;
        public Bounds Bounds => _bounds;

        public void Init(Vector3 position, float size)
        {
            _size = size;
            _bounds = new Bounds(transform.position, new Vector3(size, 1f, size));
            _transform = transform;
            _transform.position = position;
        }
    }
}