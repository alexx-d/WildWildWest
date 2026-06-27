using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private Slider _slider;
    [SerializeField] private string _parameterName;
    [SerializeField] private float _defaultValue = 0.75f;

    private bool IsSetupValid => _mixer is not null && _parameterName is { Length: > 0 };

    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Start()
    {
        LoadSettings();
    }

    public void ApplyVolume(float value)
    {
        if (IsSetupValid)
        {
            _mixer.SetFloat(_parameterName, AudioUtils.LinearToDecibels(value));
        }
    }

    private void OnSliderValueChanged(float value)
    {
        ApplyVolume(value);
        PlayerPrefs.SetFloat(_parameterName, value);
    }

    private void LoadSettings()
    {
        _slider.value = PlayerPrefs.GetFloat(_parameterName, _defaultValue);
        ApplyVolume(_slider.value);
    }
}