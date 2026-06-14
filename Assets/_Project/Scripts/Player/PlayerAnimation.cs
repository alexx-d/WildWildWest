using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayMove(Vector2 direction)
    {
        _animator.SetFloat(PlayerAnimatorData.Params.Horizontal, direction.x);
        _animator.SetFloat(PlayerAnimatorData.Params.Vertical, direction.y);
    }

    public void PlayJump()
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.Jump);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.Attack);
    }

    public void SetWeaponHoldType(int animIndex)
    {
        _animator.SetInteger(PlayerAnimatorData.Params.WeaponType, animIndex);
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(PlayerAnimatorData.Params.IsGrounded, isGrounded);
    }

    public void SetCrouched(bool isCrouching)
    {
        _animator.SetBool(PlayerAnimatorData.Params.IsCrouching, isCrouching);
    }
}