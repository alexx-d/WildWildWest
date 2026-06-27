using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UpgradeCardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Button _selectButton;

    private UpgradeData _data;
    private Action<UpgradeData> _onSelectedCallback;

    private void OnEnable() => _selectButton.onClick.AddListener(OnCardClicked);
    private void OnDisable() => _selectButton.onClick.RemoveListener(OnCardClicked);

    public void Setup(UpgradeData data, Action<UpgradeData> onSelected)
    {
        _data = data;
        _onSelectedCallback = onSelected;
        _titleText.text = data.Title;

        float displayValue = data.Value;

        if (data.ShowAsPercentage)
        {
            displayValue = data.Value * 100f;
        }

        if (data.Description.Contains("{0}"))
        {
            _descriptionText.text = string.Format(data.Description, displayValue);
        }
        else
        {
            _descriptionText.text = data.Description;
        }

        if (_iconImage != null && data.Icon != null)
        {
            _iconImage.sprite = data.Icon;
        }
    }

    private void OnCardClicked()
    {
        _onSelectedCallback?.Invoke(_data);
    }
}