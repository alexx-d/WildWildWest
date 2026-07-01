using System;
using UnityEngine;

public interface IWeaponModifiers
{
    float DamageMultiplier { get; }
    float ReloadSpeedMultiplier { get; }
    float FireRateMultiplier { get; }
}

public class PlayerStats : MonoBehaviour, IWeaponModifiers
{
    public int UpgradesCount { get; private set; } = 3;
    public float DamageMultiplier { get; private set; } = 1f;
    public float ReloadSpeedMultiplier { get; private set; } = 1f;
    public float FireRateMultiplier { get; private set; } = 1f;
    public float MoveSpeedMultiplier { get; private set; } = 1f;

    public event Action<float> MoveSpeedMultiplierChanged;

    public void ApplyUpgrade(UpgradeType type, float value)
    {
        switch (type)
        {
            case UpgradeType.DamageBuff:
                DamageMultiplier += value;
                break;

            case UpgradeType.ReloadSpeedBuff:
                ReloadSpeedMultiplier += value;
                break;

            case UpgradeType.MoveSpeedBuff:
                MoveSpeedMultiplier += value;
                MoveSpeedMultiplierChanged?.Invoke(MoveSpeedMultiplier);
                break;
        }
    }

    public void Reset()
    {
        UpgradesCount = 3;
        DamageMultiplier = 1f;
        ReloadSpeedMultiplier = 1f;
        FireRateMultiplier = 1f;
        MoveSpeedMultiplier = 1f;

        MoveSpeedMultiplierChanged?.Invoke(1f);
    }
}