using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Awake() => _animator = GetComponent<Animator>();

    public void SetSpeed(float speed) => _animator.SetFloat(Speed, speed);
    public void PlayAttack() => _animator.SetTrigger(Attack);
}