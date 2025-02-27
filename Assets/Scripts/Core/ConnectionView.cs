using UnityEngine;

namespace Core
{
    public class ConnectionView : MonoBehaviour
    {
        [SerializeField] private LineRenderer _lineRenderer;
        
        private SocketView _socketViewStart;
        private SocketView _socketViewEnd;

        public void Init(SocketView socketViewStart, SocketView socketViewEnd)
        {
            _socketViewEnd = socketViewEnd;
            _socketViewStart = socketViewStart;
            _lineRenderer.positionCount = 2;
            _lineRenderer.useWorldSpace = true;
            UpdateLine();
        }
        
        public void UpdateLine()
        {
            _lineRenderer.SetPositions(new []
            {
                _socketViewStart.Transform.position,
                _socketViewEnd.Transform.position
            });
        }
    }
}