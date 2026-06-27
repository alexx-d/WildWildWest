using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private float _damage = 15f;
    [SerializeField] private float _rotationSpeed = 10f;

    private Transform _target;
    private Health _targetHealth;
    private float _nextAttackTime;

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
    }

    public void DealDamage()
    {
        float distanceSqr = (_target.position - transform.position).sqrMagnitude;

        if (distanceSqr <= _attackDistance * _attackDistance)
        {
            _targetHealth.TakeDamage(_damage, HitboxType.Torso);
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
        Attacked?.Invoke();
    }
}