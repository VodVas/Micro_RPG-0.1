//using UnityEngine;

//[RequireComponent(typeof(Collider))]
//public class EnemyView : MonoBehaviour, IInteractionTarget
//{
//    [SerializeField] private int _priority = 10;
//    [SerializeField] private float _deathDelay = 0.5f;

//    private EnemyModel _model;
//    private IHealthComponent _health;

//    public int Priority => _priority;
//    public bool CanInteract => _health?.IsAlive ?? false;
//    public InteractionType InteractionType => InteractionType.Combat;

//    private void Awake()
//    {
//        if (_health == null)
//        {
//            _model = new EnemyModel(100f);
//            _health = _model;
//            _health.Died += HandleDeath;
//        }
//    }

//    private void Update()
//    {
//        Debug.Log(_health);
//    }

//    private void OnDisable()
//    {
//        if (_health != null)
//        {
//            _health.Died -= HandleDeath;
//        }
//    }

//    public void Initialize(EnemyModel model)
//    {
//        _model = model;
//        _health = model;

//        _health.Died += HandleDeath;
//    }

//    public void OnClick(Vector3 clickPosition)
//    {
//        // Handled by interaction system
//    }

//    private void HandleDeath()
//    {
//        //GetComponent<Collider>().enabled = false;
//        Destroy(gameObject, _deathDelay);
//    }
//}

using UnityEngine;
using Zenject;

[RequireComponent(typeof(Collider))]
public class EnemyView : MonoBehaviour, IInteractionTarget, IPoolableView<EnemyModel>
{
    [SerializeField] private int _priority = 10;
    [SerializeField] private float _deathDelay = 0.5f;

    private EnemyModel _model;
    private IHealthComponent _health;
    private MemoryPool<EnemyModel, Vector3, EnemyView> _pool;
    private bool _isPooled;

    public int Priority => _priority;
    public bool CanInteract => _health?.IsAlive ?? false;
    public InteractionType InteractionType => InteractionType.Combat;

    private void Awake() //Test
    {
        if (_health == null)
        {
            _model = new EnemyModel(100f);
            _health = _model;
            _health.Died += HandleDeath;
        }
    }

    public void SetPool(MemoryPool<EnemyModel, Vector3, EnemyView> pool)
    {
        _pool = pool;
        _isPooled = pool != null;
    }

    public void Initialize(EnemyModel model)
    {
        Cleanup();

        _model = model;
        _health = model;
        _health.Died += HandleDeath;
    }

    public void Cleanup()
    {
        if (_health != null)
        {
            _health.Died -= HandleDeath;
            _health = null;
        }
        _model = null;
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

    private void HandleDeath()
    {
        GetComponent<Collider>().enabled = false;

        if (_isPooled)
        {
            StartCoroutine(DespawnAfterDelay());
        }
        else
        {
            Destroy(gameObject, _deathDelay);
        }
    }

    private System.Collections.IEnumerator DespawnAfterDelay()
    {
        yield return new WaitForSeconds(_deathDelay);
        _pool?.Despawn(this);
    }

    private void OnDestroy()
    {
        Cleanup();
    }
}