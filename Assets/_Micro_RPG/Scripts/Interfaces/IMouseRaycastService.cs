using UnityEngine;

public interface IMouseRaycastService
{
    RaycastResult PerformRaycast(Vector2 screenPosition, Camera camera);
}