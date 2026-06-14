using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "Gamedev/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Animation Settings")]
    [SerializeField] private int _weaponAnimIndex;

    [Header("Common Shooting Settings")]
    [SerializeField] private float _damage = 20f;
    [SerializeField] private float _fireRate = 0.1f;
    [SerializeField] private float _spread = 0.02f;
    [SerializeField] private float _range = 100f;

    [Header("Projectile Settings")]
    [SerializeField] private bool _isProjectileBased = false;
    [SerializeField] private float _bulletSpeed = 50f;
    [SerializeField] private GameObject _bulletPrefab;

    public int AnimIndex => _weaponAnimIndex;
    public float Damage => _damage;
    public float FireRate => _fireRate;
    public float Spread => _spread;
    public float Range => _range;

    public bool IsProjectileBased => _isProjectileBased;
    public float BulletSpeed => _bulletSpeed;
    public GameObject BulletPrefab => _bulletPrefab;
}