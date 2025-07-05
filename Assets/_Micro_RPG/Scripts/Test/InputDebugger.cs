using UnityEngine;

public class InputDebugger : MonoBehaviour
{
   
}

//using UnityEngine;
//using UnityEngine.InputSystem;
//using System;
//using System.Collections.Generic;

//public sealed class MouseInputProvider : InputProviderBase, IGroundClickHandler
//{
//    private class RaycastHitComparer : IComparer<RaycastHit>
//    {
//        public int Compare(RaycastHit x, RaycastHit y)
//        {
//            int distanceComparison = x.distance.CompareTo(y.distance);

//            if (distanceComparison != 0) return distanceComparison;

//            var xClickable = x.collider?.GetComponent<IClickable>();
//            var yClickable = y.collider?.GetComponent<IClickable>();

//            if (xClickable != null && yClickable != null)
//            {
//                return yClickable.Priority.CompareTo(xClickable.Priority);
//            }

//            return 0;
//        }
//    }

//    const float MaxRaycastDistance = 100f;

//    private readonly Camera _camera;
//    private readonly Transform _player;
//    private readonly LayerMask _clickableLayers;
//    private readonly LayerMask _groundLayer;
//    private readonly float _stoppingDistance;
//    private readonly float _rotationSpeed;
//    private Vector3? _targetPosition;
//    private IClickable _currentTarget;
//    private readonly RaycastHit[] _raycastHits = new RaycastHit[10];
//    private readonly Mouse _mouse;
//    private readonly PlayerInputActions _inputActions;

//    public MouseInputProvider(Transform player,
//        PlayerInputActions inputActions,
//        Camera camera,
//        LayerMask clickableLayers,
//        LayerMask groundLayer,
//        float stoppingDistance = 0.1f,
//        float rotationSpeed = 720f)
//    {
//        _player = player ?? throw new ArgumentNullException(nameof(player));
//        _inputActions = inputActions ?? throw new ArgumentNullException(nameof(inputActions));
//        _camera = camera ?? throw new ArgumentNullException(nameof(camera));
//        _mouse = Mouse.current ?? throw new InvalidOperationException("Mouse input device not found!");
//        _clickableLayers = clickableLayers;
//        _groundLayer = groundLayer;
//        _stoppingDistance = stoppingDistance;
//        _rotationSpeed = rotationSpeed;

//        _inputActions.Enable();
//    }

//    protected override void UpdateInput()
//    {
//        HandleMouseClick();
//        UpdateMovement();
//    }

//    private void HandleMouseClick()
//    {
//        if (_mouse == null) return;
//        if (!_mouse.leftButton.wasPressedThisFrame) return;

//        var mousePosition = _mouse.position.ReadValue();
//        var ray = _camera.ScreenPointToRay(mousePosition);

//        Debug.DrawRay(ray.origin, ray.direction * MaxRaycastDistance, Color.red, 2f);

//        int hitCount = Physics.RaycastNonAlloc(ray, _raycastHits, MaxRaycastDistance, _clickableLayers);

//        if (hitCount > 0)
//        {
//            Array.Sort(_raycastHits, 0, hitCount, new RaycastHitComparer());

//            for (int i = 0; i < hitCount; i++)
//            {
//                var clickable = _raycastHits[i].collider.GetComponent<IClickable>();
//                if (clickable != null)
//                {
//                    _currentTarget = clickable;
//                    _targetPosition = _raycastHits[i].point;
//                    clickable.OnClick(_raycastHits[i].point);

//                    if (clickable is IAttackable attackable && attackable.IsAlive)
//                    {
//                        SetAttack(true);
//                    }
//                    return;
//                }
//            }
//        }

//        if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, _groundLayer))
//        {
//            OnGroundClick(groundHit.point);
//        }
//    }

//    public void OnGroundClick(Vector3 worldPosition)
//    {
//        _targetPosition = worldPosition;
//        _currentTarget = null;
//        SetAttack(false);
//    }

//    private void UpdateMovement()
//    {
//        if (!_targetPosition.HasValue)
//        {
//            SetMovement(Vector2.zero);
//            return;
//        }

//        var playerPosition = GetPlayerPosition();
//        var directionToTarget = _targetPosition.Value - playerPosition;
//        directionToTarget.y = 0;

//        var distance = directionToTarget.magnitude;

//        if (distance <= _stoppingDistance)
//        {
//            SetMovement(Vector2.zero);

//            if (_currentTarget == null)
//            {
//                _targetPosition = null;
//            }
//            return;
//        }

//        var cameraForward = _camera.transform.forward;
//        var cameraRight = _camera.transform.right;
//        cameraForward.y = 0;
//        cameraRight.y = 0;
//        cameraForward.Normalize();
//        cameraRight.Normalize();

//        var direction = directionToTarget.normalized;
//        var movement = new Vector2(Vector3.Dot(direction, cameraRight), Vector3.Dot(direction, cameraForward));

//        SetMovement(movement.normalized);
//    }

//    private Vector3 GetPlayerPosition()
//    {
//        return _player != null ? _player.position : throw new ArgumentNullException("Player not found!");
//    }

//    public override void Dispose()
//    {
//        _inputActions?.Dispose();
//    }
//}







/*using System;
using UnityEngine;
using Zenject;

public class PlayerController : ITickable, IFixedTickable, IDisposable
{
    private readonly PlayerModel _model;
    private readonly PlayerView _view;
    private readonly PlayerConfig _config;
    private readonly IInputProvider _inputProvider;

    public PlayerController(
        PlayerModel model,
        PlayerView view,
        PlayerConfig config,
        IInputProvider inputProvider)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _view = view ?? throw new ArgumentNullException(nameof(view));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _inputProvider = inputProvider ?? throw new ArgumentNullException(nameof(inputProvider));
    }

    public void Tick()
    {
        _inputProvider.Tick();

        Move();
        Attack();
    }

    public void FixedTick()
    {
        if (_view == null) return;

        _model.ApplyGravity(Time.fixedDeltaTime, _view.IsGrounded);
    }

    public void Dispose()
    {
        (_inputProvider as IDisposable)?.Dispose();
    }

    private void Move()
    {
        Vector2 input = _inputProvider.GetMovement();

        _model.CalculateMovement(input, Time.deltaTime);
        _model.CalculateRotation(input);

        if (_view != null)
        {
            _view.ApplyMovement(_model.Velocity);
            _view.ApplyRotation(_model.TargetRotation, _config.RotationSpeed);
            _view.SetRunningState(_model.IsRunning);
        }
    }

    private void Attack()
    {
        if (_inputProvider.GetAttack() && _model.TryAttack())
        {
            _view.TriggerAttackAnimation();
        }
    }
}*/


/*using UnityEngine;

public class PlayerModel
{
    private readonly MovementSystem _movement;
    private readonly AttackSystem _attack;

    public Vector3 Velocity { get; private set; }
    public Quaternion TargetRotation => _movement.TargetRotation;
    public bool IsRunning => _movement.IsRunning;

    public PlayerModel(PlayerConfig config, MovementSystem movement, AttackSystem attack)
    {
        _movement = movement ?? throw new System.ArgumentNullException(nameof(movement));
        _attack = attack ?? throw new System.ArgumentNullException(nameof(attack));

        Velocity = Vector3.zero;
    }

    public void CalculateMovement(Vector2 input, float deltaTime)
    {
        _movement.CalculateMovement(input, deltaTime);
        Velocity = _movement.Velocity;
    }

    public bool TryAttack() => _attack.TryAttack();

    public void CalculateRotation(Vector2 input) => _movement.CalculateRotation(input);

    public void ApplyGravity(float deltaTime, bool isGrounded)
    {
        _movement.ApplyGravity(deltaTime, isGrounded);
        Velocity = _movement.Velocity;
    }
}*/

/*// Interfaces
public interface IClickable
{
    void OnClick(Vector3 clickPosition);
}

public interface IInteractionTarget : IClickable
{
    int Priority { get; }
    bool CanInteract { get; }
    InteractionType InteractionType { get; }
}

public enum InteractionType
{
    Combat,
    Dialogue,
    Pickup
}

public interface IHealthComponent
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    bool IsAlive { get; }
    void TakeDamage(float damage);
    event System.Action<float> HealthChanged;
    event System.Action Died;
}

public interface IInteractionValidator
{
    bool CanInteract(Vector3 interactorPosition, Vector3 targetPosition, float maxDistance);
}

// Models
public class EnemyModel : IHealthComponent
{
    private float _currentHealth;
    private readonly float _maxHealth;

    public float CurrentHealth => _currentHealth;
    public float MaxHealth => _maxHealth;
    public bool IsAlive => _currentHealth > 0;

    public event System.Action<float> HealthChanged;
    public event System.Action Died;

    public EnemyModel(float maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        HealthChanged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            Died?.Invoke();
        }
    }
}

public class NPCModel
{
    private readonly string _npcId;
    private readonly float _interactionDistance;

    public string NpcId => _npcId;
    public float InteractionDistance => _interactionDistance;

    public NPCModel(string npcId, float interactionDistance)
    {
        _npcId = npcId;
        _interactionDistance = interactionDistance;
    }
}

// Views
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class EnemyView : MonoBehaviour, IInteractionTarget
{
    [SerializeField] private int _priority = 10;
    [SerializeField] private float _deathDelay = 0.5f;

    private EnemyModel _model;
    private IHealthComponent _health;

    public int Priority => _priority;
    public bool CanInteract => _health?.IsAlive ?? false;
    public InteractionType InteractionType => InteractionType.Combat;

    public void Initialize(EnemyModel model)
    {
        _model = model;
        _health = model;

        _health.Died += HandleDeath;
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.Died -= HandleDeath;
        }
    }

    public void OnClick(Vector3 clickPosition)
    {
        // Handled by interaction system
    }

    private void HandleDeath()
    {
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, _deathDelay);
    }
}

[RequireComponent(typeof(Collider))]
public class NPCView : MonoBehaviour, IInteractionTarget
{
    [SerializeField] private int _priority = 5;

    private NPCModel _model;
    private IInteractionValidator _interactionValidator;
    private Transform _playerTransform;

    public int Priority => _priority;
    public bool CanInteract => _interactionValidator?.CanInteract(
        _playerTransform.position, 
        transform.position, 
        _model?.InteractionDistance ?? 0) ?? false;
    public InteractionType InteractionType => InteractionType.Dialogue;

    public void Initialize(NPCModel model, IInteractionValidator validator, Transform playerTransform)
    {
        _model = model;
        _interactionValidator = validator;
        _playerTransform = playerTransform;
    }

    public void OnClick(Vector3 clickPosition)
    {
        // Handled by interaction system
    }
}

// Systems
public class HealthSystem
{
    public void ApplyDamage(IHealthComponent target, float damage)
    {
        target?.TakeDamage(damage);
    }
}

public class InteractionValidator : IInteractionValidator
{
    public bool CanInteract(Vector3 interactorPosition, Vector3 targetPosition, float maxDistance)
    {
        return Vector3.Distance(interactorPosition, targetPosition) <= maxDistance;
    }
}

// Enhanced Mouse System
public struct RaycastResult
{
    public bool HasHit { get; set; }
    public Vector3 HitPoint { get; set; }
    public bool IsGround { get; set; }
    public IInteractionTarget[] Targets { get; set; }
}

public interface IMouseRaycastService
{
    RaycastResult PerformRaycast(Vector2 screenPosition, Camera camera);
}

public class MouseRaycastService : IMouseRaycastService
{
    private readonly LayerMask _interactableLayers;
    private readonly LayerMask _groundLayer;
    private readonly RaycastHit[] _hits = new RaycastHit[10];

    public MouseRaycastService(LayerMask interactableLayers, LayerMask groundLayer)
    {
        _interactableLayers = interactableLayers;
        _groundLayer = groundLayer;
    }

    public RaycastResult PerformRaycast(Vector2 screenPosition, Camera camera)
    {
        var ray = camera.ScreenPointToRay(screenPosition);
        var result = new RaycastResult();

        // Check for interactables
        int hitCount = Physics.RaycastNonAlloc(ray, _hits, 100f, _interactableLayers);
        
        if (hitCount > 0)
        {
            var targets = new IInteractionTarget[hitCount];
            int validTargets = 0;

            for (int i = 0; i < hitCount; i++)
            {
                var target = _hits[i].collider.GetComponent<IInteractionTarget>();
                if (target != null && target.CanInteract)
                {
                    targets[validTargets++] = target;
                }
            }

            if (validTargets > 0)
            {
                System.Array.Resize(ref targets, validTargets);
                System.Array.Sort(targets, (a, b) => b.Priority.CompareTo(a.Priority));
                
                result.HasHit = true;
                result.HitPoint = _hits[0].point;
                result.Targets = targets;
                return result;
            }
        }

        // Check for ground
        if (Physics.Raycast(ray, out RaycastHit groundHit, 100f, _groundLayer))
        {
            result.HasHit = true;
            result.IsGround = true;
            result.HitPoint = groundHit.point;
            result.Targets = System.Array.Empty<IInteractionTarget>();
        }

        return result;
    }
}

// Enhanced Mouse Targeting System
public interface IMouseTargetingSystem
{
    void SetTarget(Vector3 position, IInteractionTarget target = null);
    Vector2 GetMovementDirection(Vector3 playerPosition, Camera camera);
    IInteractionTarget CurrentTarget { get; }
}

public class MouseTargetingSystem : IMouseTargetingSystem
{
    private readonly float _stoppingDistance;
    private Vector3? _targetPosition;
    private IInteractionTarget _currentTarget;

    public IInteractionTarget CurrentTarget => _currentTarget;

    public MouseTargetingSystem(float stoppingDistance)
    {
        _stoppingDistance = stoppingDistance;
    }

    public void SetTarget(Vector3 position, IInteractionTarget target = null)
    {
        _targetPosition = position;
        _currentTarget = target;
    }

    public Vector2 GetMovementDirection(Vector3 playerPosition, Camera camera)
    {
        if (!_targetPosition.HasValue) return Vector2.zero;

        var direction = _targetPosition.Value - playerPosition;
        direction.y = 0;

        if (direction.magnitude <= _stoppingDistance)
        {
            _targetPosition = null;
            return Vector2.zero;
        }

        var normalized = direction.normalized;
        return new Vector2(normalized.x, normalized.z);
    }
}

// Interaction System
public interface IInteractionSystem
{
    void ProcessInteraction(IInteractionTarget target, Vector3 interactorPosition);
}

public class InteractionSystem : IInteractionSystem
{
    private readonly HealthSystem _healthSystem;
    private readonly IAttacker _playerAttacker;
    private readonly float _attackDamage;

    public InteractionSystem(HealthSystem healthSystem, IAttacker playerAttacker, float attackDamage)
    {
        _healthSystem = healthSystem;
        _playerAttacker = playerAttacker;
        _attackDamage = attackDamage;
    }

    public void ProcessInteraction(IInteractionTarget target, Vector3 interactorPosition)
    {
        if (target == null || !target.CanInteract) return;

        switch (target.InteractionType)
        {
            case InteractionType.Combat:
                if (_playerAttacker.TryAttack() && target is EnemyView enemyView)
                {
                    var enemy = enemyView.GetComponent<IHealthComponent>();
                    _healthSystem.ApplyDamage(enemy, _attackDamage);
                }
                break;

            case InteractionType.Dialogue:
                Debug.Log($"Starting dialogue with NPC");
                break;

            case InteractionType.Pickup:
                Debug.Log($"Picking up item");
                break;
        }
    }
}

// Updated MouseInputProvider
public sealed class MouseInputProvider : InputProviderBase
{
    private readonly Transform _player;
    private readonly Camera _camera;
    private readonly Mouse _mouse;
    private readonly IMouseTargetingSystem _targetingSystem;
    private readonly IMouseRaycastService _raycastService;
    private readonly IInteractionSystem _interactionSystem;
    private readonly PlayerInputActions _inputActions;

    public MouseInputProvider(
        Transform player,
        Camera camera,
        IMouseTargetingSystem targetingSystem,
        IMouseRaycastService raycastService,
        IInteractionSystem interactionSystem,
        PlayerInputActions inputActions)
    {
        _player = player ?? throw new System.ArgumentNullException(nameof(player));
        _camera = camera ?? throw new System.ArgumentNullException(nameof(camera));
        _targetingSystem = targetingSystem ?? throw new System.ArgumentNullException(nameof(targetingSystem));
        _raycastService = raycastService ?? throw new System.ArgumentNullException(nameof(raycastService));
        _interactionSystem = interactionSystem ?? throw new System.ArgumentNullException(nameof(interactionSystem));
        _inputActions = inputActions ?? throw new System.ArgumentNullException(nameof(inputActions));
        _mouse = Mouse.current;

        _inputActions.Enable();
    }

    protected override void UpdateInput()
    {
        HandleMouseClick();
        UpdateMovement();
        UpdateAttackState();
    }

    private void HandleMouseClick()
    {
        if (_mouse == null || !_mouse.leftButton.wasPressedThisFrame) return;

        var mousePosition = _mouse.position.ReadValue();
        var raycastResult = _raycastService.PerformRaycast(mousePosition, _camera);

        if (!raycastResult.HasHit) return;

        if (raycastResult.Targets != null && raycastResult.Targets.Length > 0)
        {
            var target = raycastResult.Targets[0];
            _targetingSystem.SetTarget(raycastResult.HitPoint, target);
            _interactionSystem.ProcessInteraction(target, _player.position);
        }
        else if (raycastResult.IsGround)
        {
            _targetingSystem.SetTarget(raycastResult.HitPoint);
        }
    }

    private void UpdateMovement()
    {
        var movement = _targetingSystem.GetMovementDirection(_player.position, _camera);
        SetMovement(movement);
    }

    private void UpdateAttackState()
    {
        var currentTarget = _targetingSystem.CurrentTarget;
        SetAttack(currentTarget != null && currentTarget.InteractionType == InteractionType.Combat);
    }

    public override void Dispose()
    {
        _inputActions?.Dispose();
    }
}

// Updated GameInstaller
public class GameInstaller : MonoInstaller
{
    [SerializeField] private PlayerConfig _config;
    [SerializeField] private PlayerView _playerViewPrefab;
    [SerializeField] private InputActionReference _touchMoveAction;
    [SerializeField] private InputActionReference _touchAttackAction;

    [Header("Mouse Control Settings")]
    [SerializeField] private Camera _gameCamera;
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _interactableLayers = -1;
    [SerializeField] private LayerMask _groundLayer = 1 << 0;
    [SerializeField] private float _mouseStoppingDistance = 0.5f;
    [SerializeField] private float _attackDamage = 25f;

    public override void InstallBindings()
    {
        var keyboardActions = new PlayerInputActions();

        // Core bindings
        Container.BindInstance(_config);
        Container.Bind<PlayerModel>().AsSingle().NonLazy();
        Container.Bind<MovementSystem>().AsSingle().WithArguments(_config.MoveSpeed, _config.Gravity, _config.GroundOffset);
        Container.Bind<AttackSystem>().AsSingle().WithArguments(_config.AttackCooldown);

        Container.Bind<PlayerView>()
            .FromInstance(_playerViewPrefab)
            .AsSingle()
            .OnInstantiated<PlayerView>((ctx, view) => view.Initialize());

        Container.BindInterfacesAndSelfTo<PlayerController>().AsSingle().NonLazy();

        // Interaction systems
        Container.Bind<HealthSystem>().AsSingle();
        Container.Bind<IInteractionValidator>().To<InteractionValidator>().AsSingle();
        
        Container.Bind<IInteractionSystem>()
            .To<InteractionSystem>()
            .AsSingle()
            .WithArguments(_attackDamage);

        // Mouse systems
        Container.Bind<IMouseTargetingSystem>()
            .To<MouseTargetingSystem>()
            .AsSingle()
            .WithArguments(_mouseStoppingDistance);

        Container.Bind<IMouseRaycastService>()
            .To<MouseRaycastService>()
            .AsSingle()
            .WithArguments(_interactableLayers, _groundLayer);

        // Input providers
        var touchProvider = new TouchInputProvider(_touchMoveAction, _touchAttackAction);
        var keyboardProvider = new KeyboardInputProvider(keyboardActions);

        Container.Bind<MouseInputProvider>()
            .AsSingle()
            .WithArguments(_player, _gameCamera, keyboardActions);

        Container.Bind<IInputProvider>()
            .To<CompositeInputProvider>()
            .FromMethod(ctx =>
            {
                var mouseProvider = ctx.Container.Resolve<MouseInputProvider>();
                return new CompositeInputProvider(new IInputProvider[]
                {
                    mouseProvider,
                    touchProvider,
                    keyboardProvider
                });
            })
            .AsSingle();

        // Interface bindings
        Container.Bind<IMovable>().To<PlayerModel>().FromResolve();
        Container.Bind<IMovementApplier>().To<PlayerView>().FromResolve();
        Container.Bind<IAttacker>().To<PlayerModel>().FromResolve();
        Container.Bind<IGroundedStateProvider>().To<PlayerView>().FromResolve();
        Container.Bind<IAttackAnimationHandler>().To<PlayerView>().FromResolve();
    }
}

// Factory for creating enemies and NPCs
public interface ICharacterFactory
{
    EnemyView CreateEnemy(Vector3 position, float maxHealth);
    NPCView CreateNPC(Vector3 position, string npcId, float interactionDistance);
}

public class CharacterFactory : ICharacterFactory
{
    private readonly DiContainer _container;
    private readonly EnemyView _enemyPrefab;
    private readonly NPCView _npcPrefab;
    private readonly IInteractionValidator _interactionValidator;
    private readonly Transform _playerTransform;

    public CharacterFactory(
        DiContainer container,
        EnemyView enemyPrefab,
        NPCView npcPrefab,
        IInteractionValidator interactionValidator,
        Transform playerTransform)
    {
        _container = container;
        _enemyPrefab = enemyPrefab;
        _npcPrefab = npcPrefab;
        _interactionValidator = interactionValidator;
        _playerTransform = playerTransform;
    }

    public EnemyView CreateEnemy(Vector3 position, float maxHealth)
    {
        var model = new EnemyModel(maxHealth);
        var view = _container.InstantiatePrefabForComponent<EnemyView>(_enemyPrefab, position, Quaternion.identity, null);
        view.Initialize(model);
        return view;
    }

    public NPCView CreateNPC(Vector3 position, string npcId, float interactionDistance)
    {
        var model = new NPCModel(npcId, interactionDistance);
        var view = _container.InstantiatePrefabForComponent<NPCView>(_npcPrefab, position, Quaternion.identity, null);
        view.Initialize(model, _interactionValidator, _playerTransform);
        return view;
    }
}
*/


/*Реализация решения
WaypointData.cs
csharp

Copy
using System;
using UnityEngine;

[Serializable]
public struct WaypointData
{
    public Transform[] Waypoints;
    public PatrolMode Mode;
    public float MoveSpeed;
    public float RotationSpeed;
    public float StoppingDistance;
    public OrientationMode Orientation;
    public Vector3 CustomUpVector;
    public bool UseCustomUpVector;
    public float BankAngle;
    public float BankSpeed;
    public float SurfaceCheckDistance;
    public LayerMask SurfaceLayerMask;
    public Vector3 SurfaceCheckDirection;
}

public enum PatrolMode
{
    Loop,
    PingPong,
    Once
}

public enum OrientationMode
{
    None,
    LookAtDirection,
    FreeRotation,
    SurfaceAlign
}
IWaypointModel.cs
csharp

Copy
using UnityEngine;

public interface IWaypointModel
{
    int CurrentWaypointIndex { get; }
    bool IsMovingForward { get; }
    bool IsActive { get; }
    float ProgressToNextWaypoint { get; }
    Vector3 TargetPosition { get; }
    Quaternion TargetRotation { get; }
    Vector3 CurrentVelocity { get; }
    
    void UpdateMovement(float deltaTime);
    void SetActive(bool active);
    void ResetToStart();
    void TeleportToWaypoint(int index);
    void SetMoveSpeed(float speed);
    void SetRotationSpeed(float speed);
    void SetOrientationMode(OrientationMode mode);
    void SetCustomUpVector(Vector3 upVector);
}
WaypointModel.cs
csharp

Copy
using UnityEngine;

public class WaypointModel : IWaypointModel
{
    private readonly WaypointData _data;
    private readonly Transform _transform;
    private readonly MovementCalculationSystem _movementSystem;
    private readonly OrientationCalculationSystem _orientationSystem;
    
    private int _currentWaypointIndex;
    private bool _isMovingForward = true;
    private bool _isActive = true;
    private float _progressToNextWaypoint;
    private Vector3 _currentVelocity;
    private Quaternion _targetRotation;
    
    public int CurrentWaypointIndex => _currentWaypointIndex;
    public bool IsMovingForward => _isMovingForward;
    public bool IsActive => _isActive;
    public float ProgressToNextWaypoint => _progressToNextWaypoint;
    public Vector3 TargetPosition => GetCurrentTargetPosition();
    public Quaternion TargetRotation => _targetRotation;
    public Vector3 CurrentVelocity => _currentVelocity;
    
    public WaypointModel(WaypointData data, Transform transform, 
        MovementCalculationSystem movementSystem, OrientationCalculationSystem orientationSystem)
    {
        _data = data;
        _transform = transform;
        _movementSystem = movementSystem;
        _orientationSystem = orientationSystem;
        _targetRotation = transform.rotation;
        
        if (_data.Waypoints.Length > 0 && _data.Waypoints[0] != null)
        {
            _transform.position = _data.Waypoints[0].position;
        }
    }
    
    public void UpdateMovement(float deltaTime)
    {
        if (!_isActive || _data.Waypoints.Length == 0) 
        {
            _currentVelocity = Vector3.zero;
            return;
        }
        
        var targetPos = GetCurrentTargetPosition();
        if (targetPos == Vector3.zero) return;
        
        var moveResult = _movementSystem.CalculateMovement(
            _transform.position, targetPos, _data.MoveSpeed, 
            _data.StoppingDistance, deltaTime);
        
        _currentVelocity = moveResult.Velocity;
        _progressToNextWaypoint = moveResult.Progress;
        
        if (moveResult.ReachedTarget)
        {
            OnReachedWaypoint();
        }
        else if (moveResult.Direction != Vector3.zero)
        {
            _targetRotation = _orientationSystem.CalculateOrientation(
                _transform, moveResult.Direction, _data, deltaTime);
        }
    }
    
    public void SetActive(bool active) => _isActive = active;
    
    public void ResetToStart()
    {
        _currentWaypointIndex = 0;
        _isMovingForward = true;
        _isActive = true;
        _progressToNextWaypoint = 0f;
        
        if (_data.Waypoints.Length > 0 && _data.Waypoints[0] != null)
        {
            _transform.position = _data.Waypoints[0].position;
        }
    }
    
    public void TeleportToWaypoint(int index)
    {
        if (index < 0 || index >= _data.Waypoints.Length) return;
        
        _currentWaypointIndex = index;
        if (_data.Waypoints[index] != null)
        {
            _transform.position = _data.Waypoints[index].position;
        }
    }
    
    public void SetMoveSpeed(float speed) => _movementSystem.SetMoveSpeed(speed);
    public void SetRotationSpeed(float speed) => _orientationSystem.SetRotationSpeed(speed);
    public void SetOrientationMode(OrientationMode mode) => _orientationSystem.SetOrientationMode(mode);
    public void SetCustomUpVector(Vector3 upVector) => _orientationSystem.SetCustomUpVector(upVector);
    
    private Vector3 GetCurrentTargetPosition()
    {
        if (_currentWaypointIndex >= _data.Waypoints.Length) return Vector3.zero;
        var waypoint = _data.Waypoints[_currentWaypointIndex];
        return waypoint != null ? waypoint.position : Vector3.zero;
    }
    
    private void OnReachedWaypoint()
    {
        switch (_data.Mode)
        {
            case PatrolMode.Loop:
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _data.Waypoints.Length;
                break;
                
            case PatrolMode.PingPong:
                HandlePingPongMode();
                break;
                
            case PatrolMode.Once:
                if (_currentWaypointIndex < _data.Waypoints.Length - 1)
                {
                    _currentWaypointIndex++;
                }
                else
                {
                    _isActive = false;
                }
                break;
        }
    }
    
    private void HandlePingPongMode()
    {
        if (_isMovingForward)
        {
            if (_currentWaypointIndex < _data.Waypoints.Length - 1)
            {
                _currentWaypointIndex++;
            }
            else
            {
                _isMovingForward = false;
                _currentWaypointIndex = Mathf.Max(0, _currentWaypointIndex - 1);
            }
        }
        else
        {
            if (_currentWaypointIndex > 0)
            {
                _currentWaypointIndex--;
            }
            else
            {
                _isMovingForward = true;
                _currentWaypointIndex = Mathf.Min(_data.Waypoints.Length - 1, _currentWaypointIndex + 1);
            }
        }
    }
}
MovementCalculationSystem.cs
csharp

Copy
using UnityEngine;

public struct MovementResult
{
    public Vector3 Velocity;
    public Vector3 Direction;
    public float Progress;
    public bool ReachedTarget;
}

public class MovementCalculationSystem
{
    private float _moveSpeed;
    
    public MovementCalculationSystem(float moveSpeed)
    {
        _moveSpeed = moveSpeed;
    }
    
    public MovementResult CalculateMovement(Vector3 currentPos, Vector3 targetPos, 
        float speed, float stoppingDistance, float deltaTime)
    {
        var direction = targetPos - currentPos;
        var distance = direction.magnitude;
        
        if (distance <= stoppingDistance)
        {
            return new MovementResult
            {
                Velocity = Vector3.zero,
                Direction = Vector3.zero,
                Progress = 1f,
                ReachedTarget = true
            };
        }
        
        var normalizedDir = direction / distance;
        var moveDistance = speed * deltaTime;
        
        if (moveDistance > distance)
        {
            moveDistance = distance;
        }
        
        return new MovementResult
        {
            Velocity = normalizedDir * moveDistance,
            Direction = normalizedDir,
            Progress = 1f - (distance / Vector3.Distance(currentPos, targetPos)),
            ReachedTarget = false
        };
    }
    
    public void SetMoveSpeed(float speed) => _moveSpeed = Mathf.Max(0f, speed);
}
OrientationCalculationSystem.cs
csharp

Copy
using UnityEngine;

public class OrientationCalculationSystem
{
    private float _rotationSpeed;
    private OrientationMode _mode;
    private Vector3 _customUpVector = Vector3.up;
    private bool _useCustomUpVector;
    private float _currentBankAngle;
    private Vector3 _lastDirection;
    private Vector3 _currentSurfaceNormal = Vector3.up;
    
    public OrientationCalculationSystem(float rotationSpeed, OrientationMode mode)
    {
        _rotationSpeed = rotationSpeed;
        _mode = mode;
    }
    
    public Quaternion CalculateOrientation(Transform transform, Vector3 direction, 
        WaypointData data, float deltaTime)
    {
        if (_mode == OrientationMode.None || direction == Vector3.zero)
            return transform.rotation;
        
        Quaternion targetRotation = transform.rotation;
        
        switch (_mode)
        {
            case OrientationMode.LookAtDirection:
                targetRotation = CalculateLookAtRotation(direction, data);
                break;
                
            case OrientationMode.FreeRotation:
                targetRotation = CalculateFreeRotation(direction);
                break;
                
            case OrientationMode.SurfaceAlign:
                targetRotation = CalculateSurfaceAlignedRotation(transform, direction, data);
                break;
        }
        
        _lastDirection = direction;
        
        return Quaternion.Slerp(transform.rotation, targetRotation, 
            _rotationSpeed * deltaTime / 360f);
    }
    
    private Quaternion CalculateLookAtRotation(Vector3 direction, WaypointData data)
    {
        var upVector = _useCustomUpVector ? _customUpVector : Vector3.up;
        var rotation = Quaternion.LookRotation(direction, upVector);
        
        if (data.BankAngle > 0)
        {
            var turnIntensity = Vector3.SignedAngle(_lastDirection, direction, Vector3.up);
            var targetBank = Mathf.Clamp(turnIntensity * data.BankAngle / 90f, 
                -data.BankAngle, data.BankAngle);
            
            _currentBankAngle = Mathf.Lerp(_currentBankAngle, targetBank, 
                data.BankSpeed * Time.deltaTime);
            
            rotation *= Quaternion.Euler(0, 0, -_currentBankAngle);
        }
        
        return rotation;
    }
    
    private Quaternion CalculateFreeRotation(Vector3 direction)
    {
        var movementDelta = direction - _lastDirection;
        var upVector = Vector3.up;
        
        if (movementDelta.magnitude > 0.01f)
        {
            var right = Vector3.Cross(direction, _lastDirection).normalized;
            if (right.magnitude > 0.01f)
            {
                upVector = Vector3.Cross(right, direction).normalized;
            }
        }
        else if (_useCustomUpVector)
        {
            upVector = _customUpVector;
        }
        
        return Quaternion.LookRotation(direction, upVector);
    }
    
    private Quaternion CalculateSurfaceAlignedRotation(Transform transform, 
        Vector3 direction, WaypointData data)
    {
        if (Physics.Raycast(transform.position, data.SurfaceCheckDirection, 
            out RaycastHit hit, data.SurfaceCheckDistance, data.SurfaceLayerMask))
        {
            _currentSurfaceNormal = hit.normal;
        }
        
        var projectedDir = Vector3.ProjectOnPlane(direction, _currentSurfaceNormal).normalized;
        
        return projectedDir.magnitude > 0.01f 
            ? Quaternion.LookRotation(projectedDir, _currentSurfaceNormal) 
            : transform.rotation;
    }
    
    public void SetRotationSpeed(float speed) => _rotationSpeed = Mathf.Max(0f, speed);
    public void SetOrientationMode(OrientationMode mode) => _mode = mode;
    
    public void SetCustomUpVector(Vector3 upVector)
    {
        _customUpVector = upVector.normalized;
        _useCustomUpVector = true;
    }
}
IWaypointView.cs
csharp

Copy
using UnityEngine;

public interface IWaypointView
{
    void ApplyMovement(Vector3 velocity);
    void ApplyRotation(Quaternion rotation, float rotationSpeed);
    void SetDebugInfo(bool show);
}
WaypointView.cs
csharp

Copy
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class WaypointView : MonoBehaviour, IWaypointView
{
    [Header("Debug Settings")]
    [SerializeField] private bool _showDebugInfo = true;
    [SerializeField] private Color _pathColor = Color.yellow;
    [SerializeField] private float _waypointGizmoSize = 0.5f;
    
    private CharacterController _controller;
    private WaypointData _data;
    private IWaypointModel _model;
    
    public void Initialize(WaypointData data, IWaypointModel model)
    {
        _controller = GetComponent<CharacterController>();
        _data = data;
        _model = model;
    }
    
    public void ApplyMovement(Vector3 velocity)
    {
        if (_controller != null && _controller.enabled)
        {
            _controller.Move(velocity);
        }
    }
    
    public void ApplyRotation(Quaternion rotation, float rotationSpeed)
    {
        transform.rotation = rotation;
    }
    
    public void SetDebugInfo(bool show) => _showDebugInfo = show;
    
    private void OnDrawGizmos()
    {
        if (!_showDebugInfo || _data.Waypoints == null || _data.Waypoints.Length == 0) return;
        
        Gizmos.color = _pathColor;
        for (int i = 0; i < _data.Waypoints.Length; i++)
        {
            if (_data.Waypoints[i] == null) continue;
            
            Gizmos.DrawWireSphere(_data.Waypoints[i].position, _waypointGizmoSize);
            
            int nextIndex = GetNextIndex(i);
            if (nextIndex != -1 && _data.Waypoints[nextIndex] != null)
            {
                Gizmos.DrawLine(_data.Waypoints[i].position, _data.Waypoints[nextIndex].position);
            }
        }
        
        if (Application.isPlaying && _model != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(_model.TargetPosition, _waypointGizmoSize * 1.5f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, _model.TargetPosition);
        }
    }
    
    private int GetNextIndex(int currentIndex)
    {
        switch (_data.Mode)
        {
            case PatrolMode.Loop:
                return (currentIndex + 1) % _data.Waypoints.Length;
                
            case PatrolMode.PingPong:
                if (_model != null)
                {
                    if (_model.IsMovingForward && currentIndex < _data.Waypoints.Length - 1)
                        return currentIndex + 1;
                    else if (!_model.IsMovingForward && currentIndex > 0)
                        return currentIndex - 1;
                }
                return -1;
                
            case PatrolMode.Once:
                return currentIndex < _data.Waypoints.Length - 1 ? currentIndex + 1 : -1;
                
            default:
                return -1;
        }
    }
}
WaypointPresenter.cs
csharp

Copy
using UnityEngine;
using Zenject;

public class WaypointPresenter : ITickable
{
    private readonly IWaypointModel _model;
    private readonly IWaypointView _view;
    private readonly WaypointData _data;
    
    public WaypointPresenter(IWaypointModel model, IWaypointView view, WaypointData data)
    {
        _model = model;
        _view = view;
        _data = data;
    }
    
    public void Tick()
    {
        _model.UpdateMovement(Time.deltaTime);
        
        _view.ApplyMovement(_model.CurrentVelocity);
        _view.ApplyRotation(_model.TargetRotation, _data.RotationSpeed);
    }
}
IWaypointOrientationModifier.cs
csharp

Copy
public interface IWaypointOrientationModifier
{
    void ModifyOrientation(IWaypointModel model, OrientationMode mode);
}
IWaypointUpVectorModifier.cs
csharp

Copy
using UnityEngine;

public interface IWaypointUpVectorModifier
{
    void ModifyUpVector(IWaypointModel model, Vector3 upVector);
}
WaypointOrientationTrigger.cs
csharp

Copy
using UnityEngine;

public class WaypointOrientationTrigger : MonoBehaviour, IWaypointOrientationModifier
{
    [SerializeField] private OrientationMode _orientationMode = OrientationMode.LookAtDirection;
    
    public void ModifyOrientation(IWaypointModel model, OrientationMode mode)
    {
        model.SetOrientationMode(mode);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var presenter = other.GetComponent<WaypointPresenterReference>();
        if (presenter != null && presenter.Model != null)
        {
            ModifyOrientation(presenter.Model, _orientationMode);
        }
    }
}
WaypointUpVectorTrigger.cs
csharp

Copy
using UnityEngine;

public class WaypointUpVectorTrigger : MonoBehaviour, IWaypointUpVectorModifier
{
    [SerializeField] private Vector3 _newUpVector = Vector3.down;
    
    public void ModifyUpVector(IWaypointModel model, Vector3 upVector)
    {
        model.SetCustomUpVector(upVector);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        var presenter = other.GetComponent<WaypointPresenterReference>();
        if (presenter != null && presenter.Model != null)
        {
            ModifyUpVector(presenter.Model, _newUpVector);
        }
    }
}
WaypointPresenterReference.cs
csharp

Copy
using UnityEngine;

public class WaypointPresenterReference : MonoBehaviour
{
    public IWaypointModel Model { get; private set; }
    
    public void SetModel(IWaypointModel model)
    {
        Model = model;
    }
}
WaypointInstaller.cs
csharp

Copy
using UnityEngine;
using Zenject;

public class WaypointInstaller : MonoInstaller<WaypointInstaller>
{
    [SerializeField] private WaypointData _waypointData;
    [SerializeField] private WaypointView _waypointViewPrefab;
    
    public override void InstallBindings()
    {
        Container.BindInstance(_waypointData).AsSingle();
        
        Container.Bind<MovementCalculationSystem>()
            .AsSingle()
            .WithArguments(_waypointData.MoveSpeed);
            
        Container.Bind<OrientationCalculationSystem>()
            .AsSingle()
            .WithArguments(_waypointData.RotationSpeed, _waypointData.Orientation);
        
        Container.Bind<Transform>()
            .FromComponentOnRoot()
            .AsSingle();
        
        Container.Bind<IWaypointModel>()
            .To<WaypointModel>()
            .AsSingle();
        
        Container.Bind<IWaypointView>()
            .To<WaypointView>()
            .FromComponentInNewPrefab(_waypointViewPrefab)
            .AsSingle()
            .OnInstantiated<WaypointView>((ctx, view) => 
            {
                var model = ctx.Container.Resolve<IWaypointModel>();
                view.Initialize(_waypointData, model);
                
                var reference = view.gameObject.AddComponent<WaypointPresenterReference>();
                reference.SetModel(model);
            });
        
        Container.BindInterfacesAndSelfTo<WaypointPresenter>()
            .AsSingle()
            .NonLazy();
    }
}
WaypointConfig.cs
csharp

Copy
using UnityEngine;

[CreateAssetMenu(fileName = "WaypointConfig", menuName = "Game/Waypoint Config")]
public class WaypointConfig : ScriptableObject
{
    [SerializeField] private WaypointData _defaultData;
    
    public WaypointData DefaultData => _defaultData;
}*/