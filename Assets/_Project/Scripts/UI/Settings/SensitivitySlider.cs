using UnityEngine;
using UnityEngine.UI;

public class SensitivitySlider : MonoBehaviour
{
    [SerializeField] private Slider _slider;
    [SerializeField] private SensitivityData _sensitivityData;
    [SerializeField] private float _defaultValue = 15f;

    private const string SensitivityPrefsKey = "MouseSensitivity";

    private void Awake()
    {
        _slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    private void Start()
    {
        float savedValue = PlayerPrefs.GetFloat(SensitivityPrefsKey, _defaultValue);
        _slider.value = savedValue;
        
        _sensitivityData.Value = savedValue;
    }

    private void OnSliderValueChanged(float value)
    {
        PlayerPrefs.SetFloat(SensitivityPrefsKey, value);
        _sensitivityData.Value = value;
    }
}