using System;
using UnityEngine;

public abstract class EnemyAttack : MonoBehaviour
{
    public abstract float AttackDistance { get; }
    public abstract event Action Attacked;

    public abstract void Initialize(Transform target, Health targetHealth, Transform projectileContainer = null);
}