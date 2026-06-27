using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerDamageEffects : MonoBehaviour
{
    [SerializeField] private Health _health;
    [SerializeField] private Transform _cameraTransform;
    [SerializeField] private Image _vignetteImage;

    [SerializeField] private float _shakeDuration = 0.2f;
    [SerializeField] private float _shakeIntensityMultiplier = 0.05f;
    [SerializeField] private int _shakeVibrato = 10;

    [SerializeField] private float _flashDuration = 0.4f;
    [SerializeField] private Ease _flashEase = Ease.OutQuad;
    [SerializeField][Range(0f, 1f)] private float _maxLowHealthAlpha = 0.6f;

    [SerializeField] private AudioSource _playerAudioSource;
    [SerializeField] private AudioClip _damageClip;

    private float _lastHealth;

    private void Start()
    {
        _lastHealth = _health.CurrentHealth;

        SetVignetteAlpha(CalculateDangerAlpha(_lastHealth));

        _health.Damaged += OnPlayerDamaged;
        _health.Healed += OnPlayerHealed;
    }

    private void OnDestroy()
    {
        if (_health != null)
        {
            _health.Damaged -= OnPlayerDamaged;
            _health.Healed -= OnPlayerHealed;
        }

        _cameraTransform.DOKill();
        _vignetteImage.DOKill();
    }

    private void OnPlayerDamaged(float currentHealth)
    {
        float damageTaken = _lastHealth - currentHealth;
        _lastHealth = currentHealth;

        if (damageTaken <= 0)
        {
            return;
        }

        _playerAudioSource.PlayOneShot(_damageClip);

        _cameraTransform.DOKill(true);

        float shakeStrength = damageAmountCalculated(damageTaken);
        _cameraTransform.DOShakePosition(_shakeDuration, shakeStrength, _shakeVibrato, randomness: 90, fadeOut: true);

        _vignetteImage.DOKill();

        SetVignetteAlpha(1f);

        float targetDangerAlpha = CalculateDangerAlpha(currentHealth);
        _vignetteImage.DOFade(targetDangerAlpha, _flashDuration).SetEase(_flashEase);
    }

    private void OnPlayerHealed(float currentHealth)
    {
        _lastHealth = currentHealth;

        float targetDangerAlpha = CalculateDangerAlpha(currentHealth);
        _vignetteImage.DOKill();
        _vignetteImage.DOFade(targetDangerAlpha, 0.3f);
    }

    private float damageAmountCalculated(float damage) => damage * _shakeIntensityMultiplier;

    private float CalculateDangerAlpha(float currentHealth)
    {
        float healthPercent = currentHealth / _health.MaxHealth;
        return Mathf.Lerp(_maxLowHealthAlpha, 0f, healthPercent);
    }

    private void SetVignetteAlpha(float alpha)
    {
        if (_vignetteImage == null) return;
        Color color = _vignetteImage.color;
        color.a = alpha;
        _vignetteImage.color = color;
    }
}