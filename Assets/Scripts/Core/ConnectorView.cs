using UnityEngine;

namespace Core
{
    public class ConnectorView:MonoBehaviour
    {
        private FigureView _parentFigureView;

        public void Init(FigureView parentFigureView)
        {
            _parentFigureView = parentFigureView;
        }
    }
}