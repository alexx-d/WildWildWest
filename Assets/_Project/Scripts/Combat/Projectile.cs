using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Projectile : MonoBehaviour, IPoolable<Projectile>
{
    [SerializeField] private float _speed = 25f;
    [SerializeField] private float _maxLifeTime = 4f;
    [SerializeField] private float _projectileRadius = 0.2f;

    [SerializeField] private LayerMask _hitLayers;

    [SerializeField] private TrailRenderer _trail;

    private float _damage;
    private float _currentLifeTime;

    public event Action<Projectile> Died;

    private void Update()
    {
        float moveDistance = _speed * Time.deltaTime;
        Vector3 moveDirection = transform.forward;

        if (Physics.SphereCast(transform.position, _projectileRadius, moveDirection, out RaycastHit hit, moveDistance, _hitLayers, QueryTriggerInteraction.Collide))
        {
            transform.position = hit.point;
            EvaluateHit(hit.collider);
            return;
        }

        transform.position += moveDirection * moveDistance;
        _currentLifeTime += Time.deltaTime;

        if (_currentLifeTime >= _maxLifeTime)
        {
            Die();
        }
    }

    public void Launch(Vector3 direction, float damage)
    {
        _damage = damage;
        _currentLifeTime = 0f;
        transform.forward = direction;

        if (_trail != null)
        {
            _trail.Clear();
        }
    }

    private void EvaluateHit(Collider hitCollider)
    {
        if (hitCollider.TryGetComponent<Hitbox>(out Hitbox hitbox))
        {
            if (hitbox.ParentDamageable != null)
            {
                hitbox.ParentDamageable.TakeDamage(_damage, hitbox.Type);
            }

            Die();
        }
        else
        {
            Die();
        }
    }

    private void Die()
    {
        Died?.Invoke(this);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _projectileRadius);
    }
}