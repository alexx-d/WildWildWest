using UnityEngine;

[CreateAssetMenu(fileName = "NewUpgrade", menuName = "Gameplay/Upgrade Card")]
public class UpgradeData : ScriptableObject
{
    public string Title;
    [TextArea]
    [Tooltip("Используй {0} в тексте, чтобы подставить значение. Например: 'Урон +{0}%'")]
    public string Description;
    public Sprite Icon;

    public UpgradeType Type;
    public Weapon WeaponPrefab;

    [Header("Рандомизация значения")]
    [SerializeField] private bool _isRandomValue = false;
    [SerializeField] private float _minValue;
    [SerializeField] private float _maxValue;

    [Tooltip("Чем выше степень, тем меньше шанс получить максимальное значение. 2 — хорошо, 3 — очень редкие хай-роллы.")]
    [SerializeField] private float _skewExponent = 2f;

    [Header("Настройки отображения в UI")]
    [SerializeField] private bool _showAsPercentage = true;

    public float Value { get; private set; }
    public bool ShowAsPercentage => _showAsPercentage;

    public void InitializeValue()
    {
        if (_isRandomValue == false)
        {
            Value = _minValue;
            return;
        }

        float t = Mathf.Pow(Random.value, _skewExponent);

        float rawValue = Mathf.Lerp(_minValue, _maxValue, t);

        Value = Mathf.Round(rawValue * 100f) / 100f;
    }
}