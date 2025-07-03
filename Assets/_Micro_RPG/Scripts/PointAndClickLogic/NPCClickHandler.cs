using UnityEngine;

public class NPCClickHandler : MonoBehaviour, IInteractable
{
    [SerializeField] private float _interactionDistance = 3f;
    [SerializeField] private int _priority = 5;
    [SerializeField] private Transform _player;

    public bool CanInteract { get; private set; }
    public int Priority => _priority;

    private void Update()
    {
        if (_player == null) return;

        CanInteract = Vector3.Distance(transform.position, _player.position) <= _interactionDistance;
    }

    public void OnClick(Vector3 clickPosition)
    {
        if (!CanInteract) return;

        Interact();
    }

    public void Interact()
    {
        Debug.Log("Starting conversation with NPC");
    }
}