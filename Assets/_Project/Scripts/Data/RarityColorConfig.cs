using UnityEngine;

public enum UpgradeRarity
{
    Rare,
    Epic,
    Legendary
}

[CreateAssetMenu(fileName = "RarityColorConfig", menuName = "Gameplay/Rarity Color Config")]
public class RarityColorConfig : ScriptableObject
{
    [SerializeField] private Color _rareColor = new Color(0.2f, 0.5f, 1f);
    [SerializeField] private Color _epicColor = new Color(0.6f, 0.2f, 0.9f);
    [SerializeField] private Color _legendaryColor = new Color(1f, 0.5f, 0f);

    public Color GetColor(UpgradeRarity rarity)
    {
        return rarity switch
        {
            UpgradeRarity.Rare => _rareColor,
            UpgradeRarity.Epic => _epicColor,
            UpgradeRarity.Legendary => _legendaryColor,
            _ => Color.white
        };
    }
}