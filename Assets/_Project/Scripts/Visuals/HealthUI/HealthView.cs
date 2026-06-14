using UnityEngine;

public abstract class HealthView : MonoBehaviour
{
    [SerializeField] protected Health Health;

    private void OnEnable()
    {
        Health.Damaged += OnHealthChanged;
        Health.Healed += OnHealthChanged;

        OnEnabled();
    }

    private void OnDisable()
    {
        if (Health != null)
        {
            Health.Damaged -= OnHealthChanged;
            Health.Healed -= OnHealthChanged;
        }

        OnDisabled();
    }

    protected virtual void Start()
    {
        OnHealthChanged(Health.CurrentHealth);
    }

    protected abstract void OnHealthChanged(float currentHealth);

    protected virtual void OnEnabled() { }
    protected virtual void OnDisabled() { }
}