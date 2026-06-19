using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterData", menuName = "Gamedev/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Base Stats")]
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _moveSpeed = 5f;

    [Header("Hitbox Multipliers")]
    [SerializeField] private float _headMultiplier = 2f;
    [SerializeField] private float _torsoMultiplier = 1f;
    [SerializeField] private float _limbMultiplier = 0.5f;
    [SerializeField] private float _weakPointMultiplier = 3f;

    public float MaxHealth => _maxHealth;
    public float MoveSpeed => _moveSpeed;

    public float GetMultiplier(HitboxType type)
    {
        return type switch
        {
            HitboxType.Head => _headMultiplier,
            HitboxType.Torso => _torsoMultiplier,
            HitboxType.Limb => _limbMultiplier,
            HitboxType.WeakPoint => _weakPointMultiplier,
            _ => 1f
        };
    }
}