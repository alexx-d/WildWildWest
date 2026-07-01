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

    [SerializeField] private Image _cardBackground;
    [SerializeField] private RarityColorConfig _colorConfig;

    private RuntimeUpgrade _data;
    private Action<RuntimeUpgrade> _onSelectedCallback;

    private void OnEnable()
    {
        _selectButton.onClick.AddListener(OnCardClicked);
    }

    private void OnDisable()
    {
        _selectButton.onClick.RemoveListener(OnCardClicked);
    }

    public void Setup(RuntimeUpgrade data, Action<RuntimeUpgrade> onSelected)
    {
        _data = data;
        _onSelectedCallback = onSelected;

        _titleText.text = data.Data.Title;
        float displayValue = data.Value;

        if (data.Data.ShowAsPercentage)
        {
            displayValue = data.Value * 100f;
        }

        _descriptionText.text = data.GetDescription();

        if (_iconImage != null && data.Data.Icon != null)
        {
            _iconImage.sprite = data.Data.Icon;
        }

        if (_cardBackground != null && _colorConfig != null)
        {
            _cardBackground.color = _colorConfig.GetColor(data.Rarity);
        }
    }

    private void OnCardClicked()
    {
        _onSelectedCallback?.Invoke(_data);
    }
}