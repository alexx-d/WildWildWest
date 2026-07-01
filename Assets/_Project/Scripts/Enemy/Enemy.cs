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

        _chase.MovementChanged += OnMovementChanged;
        _attack.Attacked += _animator.PlayAttack;
        _health.Died += OnEnemyDead;
    }

    private void OnDisable()
    {
        _chase.MovementChanged -= OnMovementChanged;
        _attack.Attacked -= _animator.PlayAttack;
        _health.Died -= OnEnemyDead;

        _chase.StopChasing();
    }

    public void SetupTarget(Transform player, Health playerHealth, Transform projectileContainer)
    {
        _attack.Initialize(player, playerHealth, projectileContainer);
        _chase.Initialize(_config, player, _attack.AttackDistance);
    }

    private void OnMovementChanged(float velocityX, float velocityY, float speedMultiplier)
    {
        _animator.SetMovementVelocity(velocityX, velocityY, speedMultiplier);
    }

    private void OnEnemyDead()
    {
        SetPhysicsActive(false);
        _chase.StopChasing();
        _attack.enabled = false;

        Died?.Invoke(this);
        gameObject.SetActive(false);
    }

    private void SetPhysicsActive(bool isActive)
    {
        if (_allColliders == null) return;

        foreach (var col in _allColliders)
        {
            if (col != null) col.enabled = isActive;
        }
    }
}