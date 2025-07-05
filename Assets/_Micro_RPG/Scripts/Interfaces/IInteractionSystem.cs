using UnityEngine;

public interface IInteractionSystem
{
    void ProcessInteraction(IInteractionTarget target, Vector3 interactorPosition);
}