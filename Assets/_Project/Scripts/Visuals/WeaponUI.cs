using UnityEngine;
using TMPro;
using DG.Tweening;

public class WeaponUI : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TMP_Text _ammoText;

    [Header("Juice Settings")]
    [SerializeField] private float _punchScaleAmount = 0.15f;
    [SerializeField] private float _punchDuration = 0.1f;

    private Weapon _currentWeapon;

    public void UpdateActiveWeapon(Weapon newWeapon)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.AmmoChanged -= OnAmmoChanged;
            _currentWeapon.ReloadStarted -= OnReloadStarted;
        }

        _currentWeapon = newWeapon;

        if (_currentWeapon != null)
        {
            _currentWeapon.AmmoChanged += OnAmmoChanged;
            _currentWeapon.ReloadStarted += OnReloadStarted;

            RefreshText();
        }
        else
        {
            _ammoText.text = "- / -";
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

    private void OnDisable()
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.AmmoChanged -= OnAmmoChanged;
            _currentWeapon.ReloadStarted -= OnReloadStarted;
        }
        _ammoText.transform.DOKill();
    }
}