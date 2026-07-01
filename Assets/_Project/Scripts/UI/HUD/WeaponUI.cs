using UnityEngine;
using TMPro;
using DG.Tweening;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private WeaponInventory _weaponInventory;
    [SerializeField] private TMP_Text _ammoText;

    [Header("Juice Settings")]
    [SerializeField] private float _punchScaleAmount = 0.15f;
    [SerializeField] private float _punchDuration = 0.1f;

    private Weapon _currentWeapon;

    private void OnEnable()
    {
        if (_weaponInventory != null)
        {
            _weaponInventory.WeaponChanged += UpdateActiveWeapon;
            
            UpdateActiveWeapon(_weaponInventory.CurrentWeapon);
        }
    }

    private void OnDisable()
    {
        if (_weaponInventory != null)
        {
            _weaponInventory.WeaponChanged -= UpdateActiveWeapon;
        }

        UnsubscribeFromCurrentWeapon();
        
        _ammoText.transform.DOKill();
        _ammoText.DOKill();
    }

    public void UpdateActiveWeapon(Weapon newWeapon)
    {
        UnsubscribeFromCurrentWeapon();

        _currentWeapon = newWeapon;

        if (_currentWeapon != null)
        {
            _currentWeapon.AmmoChanged += OnAmmoChanged;
            _currentWeapon.ReloadStarted += OnReloadStarted;

            RefreshText();

            _ammoText.DOKill();
            _ammoText.alpha = 1f;
        }
        else
        {
            _ammoText.text = "- / -";
        }
    }

    private void UnsubscribeFromCurrentWeapon()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.AmmoChanged -= OnAmmoChanged;
            _currentWeapon.ReloadStarted -= OnReloadStarted;
        }
    }

    private void RefreshText()
    {
        if (_currentWeapon == null) return;

        _ammoText.text = $"{_currentWeapon.CurrentAmmo} / {_currentWeapon.MagazineSize}";
    }

    private void OnAmmoChanged()
    {
        _ammoText.DOKill();
        _ammoText.alpha = 1f;

        RefreshText();

        _ammoText.transform.DOKill();
        _ammoText.transform.localScale = Vector3.one;
        _ammoText.transform.DOPunchScale(Vector3.one * _punchScaleAmount, _punchDuration, 1, 0.5f);
    }

    private void OnReloadStarted()
    {
        _ammoText.text = "RELOADING...";

        _ammoText.DOKill();
        _ammoText.DOFade(0.3f, 0.3f).SetLoops(-1, LoopType.Yoyo);
    }
}