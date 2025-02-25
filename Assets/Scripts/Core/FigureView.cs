using UnityEngine;

namespace Core
{
    public class FigureView:MonoBehaviour
    {
        [SerializeField] private ConnectorView[] _connectors;

        public void Init()
        {
            foreach (var connectorView in _connectors)
            {
                connectorView.Init(this);
            }
        }
    }
}