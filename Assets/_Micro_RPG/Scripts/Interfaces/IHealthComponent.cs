using System;

public interface IHealthComponent
{
    float CurrentHealth { get; }
    float MaxHealth { get; }
    bool IsAlive { get; }
    void TakeDamage(float damage);

    event Action<float> HealthChanged;
    event Action Died;
}