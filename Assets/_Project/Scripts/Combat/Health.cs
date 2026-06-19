using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private CharacterData _config;

    private float _currentHealth;

    public event Action<float> Damaged;
    public event Action<float> Healed;
    public event Action Died;

    public float MaxHealth => _config.MaxHealth;
    public float CurrentHealth => _currentHealth;

    private void Awake()
    {
        _currentHealth = _config.MaxHealth;

        Hitbox[] childHitboxes = GetComponentsInChildren<Hitbox>();

        foreach (Hitbox hitbox in childHitboxes)
        {
            hitbox.Initialize(this);
        }
    }

    public float TakeDamage(float baseDamage, HitboxType hitboxType)
    {
        if (_currentHealth <= 0 || baseDamage < 0)
        {
            return 0;
        }
        float finalDamage = baseDamage * _config.GetMultiplier(hitboxType);

        _currentHealth = Mathf.Clamp(_currentHealth - finalDamage, 0, MaxHealth);

        Damaged?.Invoke(_currentHealth);

        if (_currentHealth <= 0)
        {
            Die();
        }

        return finalDamage;
    }

    public void Heal(float amount)
    {
        if (amount <= 0 || _currentHealth >= MaxHealth)
        {
            return;
        }

        _currentHealth = Mathf.Clamp(_currentHealth + amount, 0, MaxHealth);

        Healed?.Invoke(_currentHealth);
    }

    public void ResetHealth()
    {
        _currentHealth = _config.MaxHealth;
    }

    private void Die()
    {
        Died?.Invoke();
        gameObject.SetActive(false);
    }
}