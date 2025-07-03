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