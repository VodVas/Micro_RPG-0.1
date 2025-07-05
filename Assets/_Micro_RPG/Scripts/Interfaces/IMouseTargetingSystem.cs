using UnityEngine;

public interface IMouseTargetingSystem
{
    void SetTarget(Vector3 position, IInteractionTarget target = null);
    Vector2 GetMovementDirection(Vector3 playerPosition, Camera camera);
    IInteractionTarget CurrentTarget { get; }
}