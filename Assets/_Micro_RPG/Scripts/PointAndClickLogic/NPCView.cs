//using UnityEngine;

//[RequireComponent(typeof(Collider))]
//public class NPCView : MonoBehaviour, IInteractionTarget
//{
//    [SerializeField] private int _priority = 5;

//    private NPCModel _model;
//    private IInteractionValidator _interactionValidator;
//    private Transform _playerTransform;

//    public int Priority => _priority;
//    public bool CanInteract => _interactionValidator?.CanInteract(
//        _playerTransform.position,
//        transform.position,
//        _model?.InteractionDistance ?? 0) ?? false;
//    public InteractionType InteractionType => InteractionType.Dialogue;

//    public void Initialize(NPCModel model, IInteractionValidator validator, Transform playerTransform)
//    {
//        _model = model;
//        _interactionValidator = validator;
//        _playerTransform = playerTransform;
//    }

//    public void OnClick(Vector3 clickPosition)
//    {
//        // Handled by interaction system
//    }
//}


using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class NPCView : MonoBehaviour, IInteractionTarget, IPoolableView<NPCModel>
{
    [SerializeField] private int _priority = 5;

    private NPCModel _model;
    private IInteractionValidator _interactionValidator;
    private Transform _playerTransform;
    private MemoryPool<NPCModel, Vector3, NPCView> _pool;
    private bool _isPooled;

    public int Priority => _priority;
    public bool CanInteract => _interactionValidator?.CanInteract(
        _playerTransform.position,
        transform.position,
        _model?.InteractionDistance ?? 0) ?? false;
    public InteractionType InteractionType => InteractionType.Dialogue;

    private void Awake() // Test
    {
        if (_model == null)
        {
            _model = new NPCModel("temp_npc", 3f);

            _interactionValidator = new InteractionValidator();

            var player = GameObject.FindWithTag("Player");

            _playerTransform = player?.transform;

            if (_playerTransform == null)
            {
                Debug.LogWarning($"NPC {gameObject.name} couldn't find player transform!");
            }
        }
    }

    //public void SetPool(MemoryPool<NPCModel, Vector3, NPCView> pool)
    //{
    //    _pool = pool;
    //    _isPooled = pool != null;
    //}

    public void Initialize(NPCModel model)
    {
        _model = model;
    }

    public void SetDependencies(IInteractionValidator validator, Transform playerTransform)
    {
        _interactionValidator = validator;
        _playerTransform = playerTransform;
    }

    public void Cleanup()
    {
        _model = null;
        _interactionValidator = null;
        _playerTransform = null;
    }

    public void OnSpawned()
    {
        gameObject.SetActive(true);
        GetComponent<Collider>().enabled = true;
    }

    public void OnDespawned()
    {
        Cleanup();
        gameObject.SetActive(false);
    }

    public void OnClick(Vector3 clickPosition)
    {
        // Handled by interaction system
    }

    //public void DespawnToPool()
    //{
    //    if (_isPooled && _pool != null)
    //    {
    //        _pool.Despawn(this);
    //    }
    //    else
    //    {
    //        Destroy(gameObject);
    //    }
    //}

    private void OnDestroy()
    {
        Cleanup();
    }
}