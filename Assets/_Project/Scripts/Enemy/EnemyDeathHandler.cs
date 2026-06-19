using UnityEngine;

public class EnemyDeathHandler : MonoBehaviour
{
    private Health _health;
    private Collider[] _allColliders;

    public void Initialize(Health health)
    {
        _health = health;
        _allColliders = GetComponentsInChildren<Collider>();

        _health.Died += OnDeath;
    }

    private void OnDisable()
    {
        _health.Died -= OnDeath;
    }

    private void OnDeath()
    {
        SetPhysicsActive(false);

        gameObject.SetActive(false);
    }

    public void SetPhysicsActive(bool isActive)
    {
        if (_allColliders == null)
        {
            return;
        }

        foreach (var col in _allColliders)
        {
            col.enabled = isActive;
        }
    }
}