using System;
using UnityEngine;

public class EnemyRangedAttack : EnemyAttack
{
    [SerializeField] private float _attackDistance = 15f;
    [SerializeField] private float _attackCooldown = 2f;
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _rotationSpeed = 12f;

    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    private Transform _target;
    private Health _targetHealth;
    private float _nextAttackTime;

    private ComponentPool<Projectile> _projectilePool;

    public override float AttackDistance => _attackDistance;
    public override event Action Attacked;

    private void Update()
    {
        if (_target == null || _targetHealth == null)
        {
            return;
        }

        float distanceSqr = (_target.position - transform.position).sqrMagnitude;
        float attackDistanceSqr = _attackDistance * _attackDistance;

        if (distanceSqr <= attackDistanceSqr)
        {
            RotateTowardsTarget();
        }

        if (Time.time < _nextAttackTime)
        {
            return;
        }

        if (distanceSqr <= attackDistanceSqr)
        {
            Attack();
        }
    }

    public override void Initialize(Transform target, Health targetHealth, Transform projectileContainer = null)
    {
        _target = target;
        _targetHealth = targetHealth;

        if (_projectilePool == null && projectileContainer != null)
        {
            _projectilePool = new ComponentPool<Projectile>(_projectilePrefab, projectileContainer);
        }
    }

    private void RotateTowardsTarget()
    {
        Vector3 direction = (_target.position - transform.position).normalized;
        direction.y = 0;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }

    private void Attack()
    {
        _nextAttackTime = Time.time + _attackCooldown;

        SpawnProjectile();

        Attacked?.Invoke();
    }

    private void SpawnProjectile()
    {
        if (_target == null || _firePoint == null)
        {
            return;
        }

        Vector3 targetCenter = _target.position + Vector3.up * 1f;
        Vector3 direction = (targetCenter - _firePoint.position).normalized;

        Projectile projectile = _projectilePool.Get();

        projectile.transform.position = _firePoint.position;
        projectile.Launch(direction, _damage);
    }
}