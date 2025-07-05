using UnityEngine;

public interface IInteractionValidator
{
    bool CanInteract(Vector3 interactorPosition, Vector3 targetPosition, float maxDistance);
}