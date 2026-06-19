using System;
using System.Collections;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData _data;
    [SerializeField] private HitscanShooter _hitscanShooter;

    private float _nextFireTime;
    private bool _isTriggerPulled;
    private bool _isAiming;

    private int _currentAmmo;
    private bool _isReloading;

    public int AnimIndex => _data.AnimIndex;
    public float RecoilPitch => _data.RecoilPitch;
    public float RecoilYaw => _data.RecoilYaw;
    public int CurrentAmmo => _currentAmmo;
    public int MagazineSize => _data.MagazineSize;
    public bool IsReloading => _isReloading;
    public float CurrentSpread
    {
        get
        {
            float spread = _data.Spread;

            if (_isAiming)
            {
                spread *= _data.AimSpreadMultiplier;
            }

            return spread;
        }
    }

    public event Action Fired;
    public event Action ReloadStarted;
    public event Action AmmoChanged;

    private void Awake()
    {
        _currentAmmo = _data.MagazineSize;
    }
    private void Update()
    {
        if (_isTriggerPulled)
        {
            TryExecuteShot();
        }
    }

    public void Initialize(Camera playerCamera)
    {
        if (_hitscanShooter != null)
        {
            _hitscanShooter.Initialize(playerCamera);
        }
    }

    public void PullTrigger()
    {
        _isTriggerPulled = true;
    }

    public void ReleaseTrigger()
    {
        _isTriggerPulled = false;
    }

    public void SetAimState(bool isAiming)
    {
        _isAiming = isAiming;
    }

    public void StartReload()
    {
        if (_isReloading || _currentAmmo == _data.MagazineSize)
        {
            return;
        }

        StartCoroutine(ReloadCoroutine());
    }

    private IEnumerator ReloadCoroutine()
    {
        _isReloading = true;
        ReloadStarted?.Invoke();

        yield return new WaitForSeconds(_data.ReloadDuration);

        _currentAmmo = _data.MagazineSize;
        _isReloading = false;

        AmmoChanged?.Invoke();
    }

    private void TryExecuteShot()
    {
        if (_isReloading || Time.time < _nextFireTime)
        {
            return;
        }

        if (_currentAmmo <= 0)
        {
            StartReload();
            return;
        }

        _nextFireTime = Time.time + _data.FireRate;

        _currentAmmo--;
        AmmoChanged?.Invoke();
        Fired?.Invoke();

        if (_data.IsProjectileBased)
        {
        }
        else
        {
            float currentSpread = _data.Spread;

            if (_isAiming)
            {
                currentSpread *= _data.AimSpreadMultiplier;
            }

            _hitscanShooter.Fire(_data.Damage, _data.Range, currentSpread, Vector3.zero);
        }
    }

    private void OnDisable()
    {
        _isTriggerPulled = false;
        _isAiming = false;

        if (_isReloading)
        {
            StopAllCoroutines();
            _isReloading = false;
        }
    }
}