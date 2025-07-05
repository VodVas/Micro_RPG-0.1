using UnityEngine;

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
                    Debug.Log(" if (_playerAttacker.TryAttack() && target is EnemyView enemyView)");
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
