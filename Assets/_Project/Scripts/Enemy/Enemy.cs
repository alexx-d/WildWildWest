using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyTargetChase))]
public class Enemy : MonoBehaviour, IPoolable<Enemy>
{
    [SerializeField] private CharacterData _config;
    [SerializeField] private EnemyAnimator _animator;
    [SerializeField] private EnemyAttack _attack;

    private Health _health;
    private EnemyTargetChase _chase;
    private Collider[] _allColliders;

    public event Action<Enemy> Died;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _chase = GetComponent<EnemyTargetChase>();
        _allColliders = GetComponentsInChildren<Collider>();
    }

    private void OnEnable()
    {
        _health.ResetHealth();
        SetPhysicsActive(true);

        _chase.enabled = true;
        _attack.enabled = true;

        _chase.SpeedChanged += OnSpeedChanged;
        _attack.Attacked += _animator.PlayAttack;

        _health.Died += OnEnemyDead;
    }
    private void OnDisable()
    {
        _chase.SpeedChanged -= OnSpeedChanged;
        _attack.Attacked -= _animator.PlayAttack;

        _health.Died -= OnEnemyDead;
    }
    
    public void SetupTarget(Transform player, Health playerHealth, Transform projectileContainer)
    {
        _chase.Initialize(_config, player, _attack.AttackDistance);
        _attack.Initialize(player, playerHealth, projectileContainer);
    }

    private void OnSpeedChanged(float speed)
    {
        _animator.SetSpeed(speed);
    }

    private void OnEnemyDead()
    {
        SetPhysicsActive(false);
        _chase.enabled = false;
        _attack.enabled = false;

        Died?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void SetPhysicsActive(bool isActive)
    {
        if (_allColliders == null)
        {
            return;
        }

        foreach (var col in _allColliders)
        {
            if (col != null) col.enabled = isActive;
        }
    }
}