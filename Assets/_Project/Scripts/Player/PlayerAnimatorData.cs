using UnityEngine;

public static class PlayerAnimatorData
{
    public static class Params
    {
        public static readonly int Horizontal = Animator.StringToHash(nameof(Horizontal));
        public static readonly int Vertical = Animator.StringToHash(nameof(Vertical));
        public static readonly int AnimSpeedMultiplier = Animator.StringToHash(nameof(AnimSpeedMultiplier));

        public static readonly int IsGrounded = Animator.StringToHash(nameof(IsGrounded));
        public static readonly int IsCrouching = Animator.StringToHash(nameof(IsCrouching));

        public static readonly int Jump = Animator.StringToHash(nameof(Jump));
        public static readonly int Attack = Animator.StringToHash(nameof(Attack));
        public static readonly int WeaponType = Animator.StringToHash(nameof(WeaponType));
    }
}