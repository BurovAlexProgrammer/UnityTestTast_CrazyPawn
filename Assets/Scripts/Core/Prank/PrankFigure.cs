using System.Linq;
using UnityEngine;

namespace Core.Prank
{
    public class PrankFigure : MonoBehaviour
    {
        [SerializeField] private Collider[] _colliders;
        
        private float _timer;
        private Transform _transform;
        private Transform[] _childs;

        public void Init(Transform originTransform)
        {
            _transform = transform;
            _childs = _colliders.Select(x => x.transform).ToArray();
            _timer = 3f;
            _transform.position = originTransform.position;
        }

        private void Start()
        {
            Explode();
        }

        private void Explode()
        {
            foreach (var partCollider in _colliders)
            {
                partCollider.attachedRigidbody.AddExplosionForce(20f, _transform.position, 20f, 10f, ForceMode.Impulse);
            }
        }

        //Not optimized, just for fun (Better way is Dotween animation)
        private void Update()
        {
            var scaleChange = 0.997f;
            _timer -= Time.deltaTime;
            
            foreach (var child in _childs)
            {
                child.localScale *= scaleChange;
            }
            
            if (_timer <= 0f) Destroy(gameObject);
        }
    }
}