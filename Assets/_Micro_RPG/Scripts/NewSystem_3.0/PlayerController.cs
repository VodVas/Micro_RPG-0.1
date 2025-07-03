using System;
using UnityEngine;
using Zenject;

public class PlayerController : ITickable, IFixedTickable, IDisposable
{
    private readonly IMovable _movable;
    private readonly IAttacker _attacker;
    private readonly PlayerConfig _config;
    private readonly IInputProvider _inputProvider;
    private readonly IMovementApplier _movementApplier;
    private readonly IAttackAnimationHandler _animationHandler;
    private readonly IGroundedStateProvider _groundedProvider;

    public PlayerController(
        IAttackAnimationHandler animationHandler,
        IMovable movable,
        IAttacker attacker,
        IGroundedStateProvider groundedProvider,
        PlayerConfig config,
        IInputProvider inputProvider,
        IMovementApplier movementApplier)
    {
        _movable = movable;
        _attacker = attacker;
        _groundedProvider = groundedProvider;
        _config = config;
        _inputProvider = inputProvider;
        _animationHandler = animationHandler;
        _movementApplier = movementApplier;
    }

    public void Tick()
    {
        _inputProvider.Tick();
        Move();
        Attack();
    }

    public void FixedTick()
    {
        bool isGrounded = _groundedProvider?.IsGrounded ?? false;
        _movable.ApplyGravity(Time.fixedDeltaTime, isGrounded);
    }

    private void Move()
    {
        Vector2 input = _inputProvider.GetMovement();

        _movable.CalculateMovement(input, Time.deltaTime);
        _movable.CalculateRotation(input);

        _movementApplier.ApplyMovement(_movable.Velocity);
        _animationHandler.ApplyRotation(_movable.TargetRotation, _config.RotationSpeed);
        _animationHandler.SetRunningState(_movable.IsRunning);
    }

    private void Attack()
    {
        if (_inputProvider.GetAttack() && _attacker.TryAttack())
            _animationHandler.TriggerAttackAnimation();
    }

    public void Dispose() =>
        (_inputProvider as IDisposable)?.Dispose();
}