using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [SerializeField] private float _attackDistance = 2f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private float _damage = 15f;

    private Health _myHealth;
    private Transform _target;
    private Health _targetHealth;
    private float _nextAttackTime;

    private void Awake()
    {
        _myHealth = GetComponent<Health>();
    }

    private void Update()
    {
        if (_target == null || _targetHealth == null || Time.time < _nextAttackTime)
        {
            return;
        }

        float distanceSqr = (_target.position - transform.position).sqrMagnitude;

        if (distanceSqr <= _attackDistance * _attackDistance)
        {
            Attack();
        }
    }

    private void OnEnable()
    {
        _myHealth.Died += OnDeath;
    }

    private void OnDisable()
    {
        _myHealth.Died -= OnDeath;
    }

    public void Initialize(Transform target, Health targetHealth)
    {
        _target = target;
        _targetHealth = targetHealth;
    }

    private void Attack()
    {
        _nextAttackTime = Time.time + _attackCooldown;
        _targetHealth.TakeDamage(_damage, HitboxType.Torso);
    }

    private void OnDeath()
    {
        enabled = false;
    }
}