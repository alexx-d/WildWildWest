using UnityEngine;

public class EnemyRangedAttack : EnemyAttack
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private Transform _firePoint;

    private ComponentPool<Projectile> _projectilePool;

    public override void Initialize(Transform target, Health targetHealth, Transform projectileContainer = null)
    {
        base.Initialize(target, targetHealth, projectileContainer);

        if (_projectilePool == null && projectileContainer != null)
        {
            _projectilePool = new ComponentPool<Projectile>(_projectilePrefab, projectileContainer);
        }
    }

    protected override void ExecuteAttack()
    {
        if (Target == null || _firePoint == null)
        {
            return;
        }

        Vector3 targetCenter = Target.position + Vector3.up * 1f;
        Vector3 direction = (targetCenter - _firePoint.position).normalized;

        Projectile projectile = _projectilePool.Get();
        projectile.transform.position = _firePoint.position;
        projectile.Launch(direction, _damage);
    }
}