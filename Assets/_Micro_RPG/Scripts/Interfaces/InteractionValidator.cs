using UnityEngine;

public class InteractionValidator : IInteractionValidator
{
    public bool CanInteract(Vector3 interactorPosition, Vector3 targetPosition, float maxDistance)
    {
        return Vector3.Distance(interactorPosition, targetPosition) <= maxDistance;
    }
}