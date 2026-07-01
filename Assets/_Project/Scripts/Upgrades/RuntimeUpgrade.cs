using UnityEngine;

public readonly struct RuntimeUpgrade
{
    public UpgradeData Data { get; }
    public float Value { get; }
    public UpgradeRarity Rarity { get; }

    public RuntimeUpgrade(UpgradeData data)
    {
        Data = data;
        Value = data.RollValue();

        if (data.IsRandomValue == false)
        {
            Rarity = data.StaticRarity;
        }
        else
        {
            float t = Mathf.InverseLerp(data.MinValue, data.MaxValue, Value);

            if (t >= 0.85f) Rarity = UpgradeRarity.Legendary;
            else if (t >= 0.50f) Rarity = UpgradeRarity.Epic;
            else Rarity = UpgradeRarity.Rare;
        }
    }

    public string GetDescription()
    {
        float displayValue = Data.ShowAsPercentage ? Value * 100f : Value;

        return string.Format(Data.Description, displayValue);
    }
}