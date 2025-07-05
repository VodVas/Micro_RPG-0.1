public interface IInteractionTarget : IClickable
{
    //int Priority { get; }
    bool CanInteract { get; }
    InteractionType InteractionType { get; }
}