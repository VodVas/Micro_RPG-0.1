using UnityEngine;

public class PlayerModel : IMovable, IAttacker
{
    private readonly MovementSystem _movement;
    private readonly AttackSystem _attack;

    public Vector3 Velocity => _movement.Velocity;
    public Quaternion TargetRotation => _movement.TargetRotation;
    public bool IsRunning => _movement.IsRunning;

    public PlayerModel(MovementSystem movement, AttackSystem attack)
    {
        _movement = movement;
        _attack = attack;
    }

    public void CalculateMovement(Vector2 input, float deltaTime)
        => _movement.CalculateMovement(input, deltaTime);

    public void CalculateRotation(Vector2 input)
        => _movement.CalculateRotation(input);

    public void ApplyGravity(float deltaTime, bool isGrounded)
        => _movement.ApplyGravity(deltaTime, isGrounded);

    public bool TryAttack()
        => _attack.TryAttack();
}