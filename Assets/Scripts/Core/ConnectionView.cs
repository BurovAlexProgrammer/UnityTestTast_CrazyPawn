using UnityEngine;

namespace Core
{
    public class ConnectionView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;

        private SocketView _socketViewStart;
        private SocketView _socketViewEnd;

        public LineRenderer LineRenderer => _lineRenderer;

        public void Init(SocketView socketViewStart, SocketView socketViewEnd)
        {
            _socketViewEnd = socketViewEnd;
            _socketViewStart = socketViewStart;
            _lineRenderer.positionCount = 2;
            _lineRenderer.useWorldSpace = true;
            _lineRenderer.widthMultiplier = 0.1f;
        }

        public void Destroy()
        {
            GameObject.Destroy(gameObject);
        }
        
        public void Update()
        {
            _lineRenderer.SetPositions(new []
            {
                _socketViewStart.Transform.position,
                _socketViewEnd.Transform.position
            });
        }

        public bool IsParentFigure(FigureView figureView)
        {
            return _socketViewStart.ParentFigureView == figureView || _socketViewEnd.ParentFigureView == figureView;
        }
    }
}