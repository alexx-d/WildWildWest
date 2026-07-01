using UnityEngine;

public class EnemyMeleeAttack : EnemyAttack
{
    [SerializeField] private float _damage = 15f;

    protected override void ExecuteAttack()
    {
    }

    public void DealDamage()
    {
        if (Target == null)
        {
            return;
        }

        float distanceSqr = (Target.position - transform.position).sqrMagnitude;

        if (distanceSqr <= AttackDistance * AttackDistance)
        {
            TargetHealth.TakeDamage(_damage, HitboxType.Torso);
        }
    }
}