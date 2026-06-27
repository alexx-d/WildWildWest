using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SmoothHealthBarView : HealthView
{
    [SerializeField] private Slider _slider;
    [SerializeField] private float _duration = 0.2f;

    private Coroutine _currentAnimation;

    protected override void OnEnabled()
    {
        StopCurrentAnimation();
        _slider.value = Health.CurrentHealth / Health.MaxHealth;
    }

    protected override void OnDisabled()
    {
        StopCurrentAnimation();
    }

    protected override void OnHealthChanged(float currentHealth)
    {
        StopCurrentAnimation();

        float targetValue = currentHealth / Health.MaxHealth;

        _currentAnimation = StartCoroutine(AnimateBar(targetValue));
    }

    private IEnumerator AnimateBar(float targetValue)
    {
        float elapsed = 0f;
        float startValue = _slider.value;

        while (elapsed < _duration)
        {
            elapsed += Time.unscaledDeltaTime;

            float normalizedTime = elapsed / _duration;

            _slider.value = Mathf.Lerp(startValue, targetValue, normalizedTime);

            yield return null;
        }

        _slider.value = targetValue;
        _currentAnimation = null;
    }

    private void StopCurrentAnimation()
    {
        if (_currentAnimation != null)
        {
            StopCoroutine(_currentAnimation);
            _currentAnimation = null;
        }
    }
}