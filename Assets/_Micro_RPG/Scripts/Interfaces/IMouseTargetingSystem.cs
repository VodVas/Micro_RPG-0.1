using UnityEngine;

public interface IMouseTargetingSystem
{
    Vector3? TargetPosition { get; }
    IClickable CurrentTarget { get; }
    bool HasValidTarget { get; }
    void SetTarget(Vector3 position, IClickable clickable = null);
    void ClearTarget();
    Vector2 GetMovementDirection(Vector3 currentPosition, Camera camera);
}