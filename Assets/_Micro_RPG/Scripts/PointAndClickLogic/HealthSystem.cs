public class HealthSystem
{
    public void ApplyDamage(IHealthComponent target, float damage)
    {
        target?.TakeDamage(damage);
    }
}