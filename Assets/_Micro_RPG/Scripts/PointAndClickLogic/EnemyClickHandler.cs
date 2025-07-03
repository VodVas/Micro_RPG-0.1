using UnityEngine;

public class EnemyClickHandler : MonoBehaviour, IAttackable
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private int _priority = 10;
    [SerializeField] private float _timeBeforeDeath = 0.5f;

    private float _currentHealth;

    public bool IsAlive => _currentHealth > 0;
    public int Priority => _priority;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void OnClick(Vector3 clickPosition)
    {
        if (!IsAlive) return;

        Debug.Log($"Enemy clicked at {clickPosition}");
    }

    public void TakeDamage(float damage)
    {
        if (!IsAlive) return;

        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject, _timeBeforeDeath);
    }
}