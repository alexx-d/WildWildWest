using DG.Tweening;
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

    [Header("Recoil Settings")]
    [SerializeField] private float _recoilPitch = 3f;
    [SerializeField] private float _recoilYawMin = -0.5f;
    [SerializeField] private float _recoilYawMax = 0.5f;

    [Header("Aiming Settings")]
    [Range(10f, 60f)]
    [SerializeField] private float _aimFOV = 40f;
    [Range(0f, 1f)]
    [SerializeField] private float _aimSpreadMultiplier = 0.3f;

    [Header("Magazine & Reload Settings")]
    [SerializeField] private int _magazineSize = 30;
    [SerializeField] private float _reloadDuration = 2f;

    [Header("Projectile Settings")]
    [SerializeField] private bool _isProjectileBased = false;
    [SerializeField] private float _bulletSpeed = 50f;
    [SerializeField] private GameObject _bulletPrefab;

    public int AnimIndex => _weaponAnimIndex;
    public float Damage => _damage;
    public float FireRate => _fireRate;
    public float Spread => _spread;
    public float Range => _range;
    public float RecoilPitch => _recoilPitch;
    public float RecoilYaw => Random.Range(_recoilYawMin, _recoilYawMax);
    public float AimFOV => _aimFOV;
    public float AimSpreadMultiplier => _aimSpreadMultiplier;
    public int MagazineSize => _magazineSize;
    public float ReloadDuration => _reloadDuration;
    public bool IsProjectileBased => _isProjectileBased;
    public float BulletSpeed => _bulletSpeed;
    public GameObject BulletPrefab => _bulletPrefab;
}