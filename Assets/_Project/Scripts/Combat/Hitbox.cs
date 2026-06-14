using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField] private HitboxType _type = HitboxType.Default;

    public HitboxType Type => _type;
    public IDamageable ParentDamageable { get; private set; }

    public void Initialize(IDamageable parent)
    {
        ParentDamageable = parent;
    }
}