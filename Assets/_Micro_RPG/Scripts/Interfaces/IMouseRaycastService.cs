using UnityEngine;

public interface IMouseRaycastService
{
    MouseRaycastResult PerformRaycast(Vector3 screenPosition, Camera camera);
}