using UnityEngine;
using UnityEngine.UI;

public class WeaponSlotUI : MonoBehaviour
{
    [SerializeField] private Image _iconImage;
    [SerializeField] private Image _highlightBorder;

    [SerializeField] private Color _activeColor = Color.white;
    [SerializeField] private Color _inactiveColor = new Color(1f, 1f, 1f, 0.3f);

    public void Clear()
    {
        _iconImage.gameObject.SetActive(false);
        _highlightBorder.gameObject.SetActive(false);
    }

    public void Fill(Sprite icon)
    {
        _iconImage.sprite = icon;
        _iconImage.gameObject.SetActive(true);
        _iconImage.color = _inactiveColor;
    }

    public void SetSelection(bool isSelected)
    {
        _highlightBorder.gameObject.SetActive(isSelected);

        if (_iconImage.gameObject.activeSelf)
        {
            _iconImage.color = isSelected ? _activeColor : _inactiveColor;
        }
    }
}