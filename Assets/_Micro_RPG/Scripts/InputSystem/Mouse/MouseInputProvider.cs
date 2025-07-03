using System;
using UnityEngine.InputSystem;
using UnityEngine;

public sealed class MouseInputProvider : InputProviderBase
{
    private readonly Transform _player;
    private readonly Camera _camera;
    private readonly Mouse _mouse;
    private readonly IMouseTargetingSystem _targetingSystem;
    private readonly IMouseRaycastService _raycastService;
    private readonly PlayerInputActions _inputActions;

    public MouseInputProvider(
        Transform player,
        Camera camera,
        IMouseTargetingSystem targetingSystem,
        IMouseRaycastService raycastService,
        PlayerInputActions inputActions)
    {
        _player = player ?? throw new ArgumentNullException(nameof(player));
        _camera = camera ?? throw new ArgumentNullException(nameof(camera));
        _targetingSystem = targetingSystem ?? throw new ArgumentNullException(nameof(targetingSystem));
        _raycastService = raycastService ?? throw new ArgumentNullException(nameof(raycastService));
        _inputActions = inputActions ?? throw new ArgumentNullException(nameof(inputActions));
        _mouse = Mouse.current;// ?? throw new InvalidOperationException("Mouse input device not found!");

        _inputActions.Enable();
    }

    protected override void UpdateInput()
    {
        HandleMouseClick();
        UpdateMovement();
    }

    private void HandleMouseClick()
    {
        if (_mouse == null || !_mouse.leftButton.wasPressedThisFrame) return;

        var mousePosition = _mouse.position.ReadValue();
        var raycastResult = _raycastService.PerformRaycast(mousePosition, _camera);

        if (!raycastResult.HasHit) return;

        if (raycastResult.Clickable != null)
        {
            _targetingSystem.SetTarget(raycastResult.HitPoint, raycastResult.Clickable);
            raycastResult.Clickable.OnClick(raycastResult.HitPoint);

            if (raycastResult.Clickable is IAttackable attackable && attackable.IsAlive)
            {
                SetAttack(true);
            }
        }
        else if (raycastResult.IsGround)
        {
            _targetingSystem.SetTarget(raycastResult.HitPoint);
            SetAttack(false);
        }
    }

    private void UpdateMovement()
    {
        var movement = _targetingSystem.GetMovementDirection(_player.position, _camera);
        SetMovement(movement);
    }

    public override void Dispose()
    {
        _inputActions?.Dispose();
    }
}