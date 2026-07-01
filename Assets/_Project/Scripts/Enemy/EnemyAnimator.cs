using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EnemyAnimator : MonoBehaviour
{
    private Animator _animator;
    private static readonly int VelocityX = Animator.StringToHash("VelocityX");
    private static readonly int VelocityY = Animator.StringToHash("VelocityY");
    private static readonly int SpeedMultiplier = Animator.StringToHash("SpeedMultiplier");
    private static readonly int Attack = Animator.StringToHash("Attack");

    private void Awake() => _animator = GetComponent<Animator>();

    public void SetMovementVelocity(float x, float y, float speedMultiplier)
    {
        _animator.SetFloat(VelocityX, x);
        _animator.SetFloat(VelocityY, y);
        _animator.SetFloat(SpeedMultiplier, Mathf.Max(0.1f, speedMultiplier));
    }

    public void PlayAttack()
    {
        _animator.SetTrigger(Attack);
    }
}