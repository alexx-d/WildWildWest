using System;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private float _rotationSpeed = 30f;

    protected Transform Target;
    protected Health TargetHealth;
    private float _nextAttackTime;

    public float AttackDistance => _attackDistance;
    public event Action Attacked;

    public virtual void Initialize(Transform target, Health targetHealth, Transform projectileContainer = null)
    {
        Target = target;
        TargetHealth = targetHealth;
    }

    private void Update()
    {
        if (Target == null || TargetHealth == null)
        {
            return;
        }

        float distanceSqr = (Target.position - transform.position).sqrMagnitude;
        float attackDistanceSqr = _attackDistance * _attackDistance;

        if (distanceSqr <= attackDistanceSqr * 1.5f)
        {
            RotateTowardsTarget();
        }

        if (Time.time < _nextAttackTime)
        {
            return;
        }

        if (distanceSqr <= attackDistanceSqr)
        {
            _nextAttackTime = Time.time + _attackCooldown;
            ExecuteAttack();
            Attacked?.Invoke();
        }
    }

    protected abstract void ExecuteAttack();

    private void RotateTowardsTarget()
    {
        Vector3 direction = (Target.position - transform.position).normalized;
        direction.y = 0;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}