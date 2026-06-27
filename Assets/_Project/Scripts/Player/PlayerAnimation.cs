using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    private Animator _animator;

    private float _speedMultiplier = 1f;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayMove(Vector2 direction)
    {
        _animator.SetFloat(PlayerAnimatorData.Params.Horizontal, direction.x);
        _animator.SetFloat(PlayerAnimatorData.Params.Vertical, direction.y);

        float finalAnimSpeed = direction.sqrMagnitude > 0.01f ? _speedMultiplier : 1f;
        _animator.SetFloat(PlayerAnimatorData.Params.AnimSpeedMultiplier, finalAnimSpeed);
    }

    public void PlayJump()
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.Jump);
    }

    public void PlayAttack()
    {
        _animator.SetTrigger(PlayerAnimatorData.Params.Attack);
    }

    public void SetWeaponHoldType(float animIndex)
    {
        _animator.SetFloat(PlayerAnimatorData.Params.WeaponType, animIndex);
    }

    public void SetGrounded(bool isGrounded)
    {
        _animator.SetBool(PlayerAnimatorData.Params.IsGrounded, isGrounded);
    }

    public void SetCrouched(bool isCrouching)
    {
        _animator.SetBool(PlayerAnimatorData.Params.IsCrouching, isCrouching);
    }

    public void SetSpeedMultiplier(float value)
    {
        _speedMultiplier = value;
    }
}