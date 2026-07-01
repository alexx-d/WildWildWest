using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Gameplay/Upgrade Card")]
public class UpgradeData : ScriptableObject
{
    [SerializeField] private string _title;
    [SerializeField][TextArea] private string _description;
    [SerializeField] private Sprite _icon;

    [SerializeField] private UpgradeType _type;
    [SerializeField] private Weapon _weaponPrefab;

    [Header("Рандомизация значения")]
    [SerializeField] private bool _isOneTime = false;
    [SerializeField] private bool _isRandomValue = false;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;
    [SerializeField] private float _skewExponent = 2f;

    [Header("Настройки отображения в UI")]
    [SerializeField] private bool _showAsPercentage = true;
    [SerializeField] private UpgradeRarity _staticRarity = UpgradeRarity.Rare;

    public string Title => _title;
    public string Description => _description;
    public Sprite Icon => _icon;
    public UpgradeType Type => _type;
    public bool IsOneTime => _isOneTime;
    public Weapon WeaponPrefab => _weaponPrefab;
    public bool ShowAsPercentage => _showAsPercentage;

    public bool IsRandomValue => _isRandomValue;
    public float MinValue => _minValue;
    public float MaxValue => _maxValue;
    public UpgradeRarity StaticRarity => _staticRarity;

    public float RollValue()
    {
        if (_isRandomValue == false)
        {
            return _minValue;
        }

        float t = Mathf.Pow(Random.value, _skewExponent);
        float rawValue = Mathf.Lerp(_minValue, _maxValue, t);
        return Mathf.Round(rawValue * 100f) / 100f;
    }
}