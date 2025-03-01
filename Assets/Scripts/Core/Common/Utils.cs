using UnityEngine;
using UnityEngine.EventSystems;

namespace Core.Common
{
    public class Utils
    {
        //AI generated
        public static Vector3 GetRaycastPosition(PointerEventData eventData, float targetY = 0f)
        {
            var screenPosition = eventData.position;

            return GetRaycastPosition(screenPosition, targetY);
        }
        
        public static Vector3 GetRaycastPosition(Vector2 screenPosition, float targetY = 0f)
        {
            var ray = Camera.main.ScreenPointToRay(screenPosition);
            var planeOffset = (targetY - ray.origin.y) / ray.direction.y;
            var raycastPoint = ray.origin + ray.direction * planeOffset;

            return raycastPoint;
        }
    }
}