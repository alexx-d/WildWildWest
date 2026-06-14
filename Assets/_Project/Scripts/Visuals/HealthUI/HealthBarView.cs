using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : HealthView
{
    [SerializeField] private Slider _slider;

    protected override void OnHealthChanged(float currentHealth)
    {
        _slider.value = (float)currentHealth / Health.MaxHealth;
    }
}