using System.Collections.Generic;
using UnityEngine;

public class UpgradeProvider : MonoBehaviour
{
    [SerializeField] private List<UpgradeData> _allUpgradesPool;

    private readonly List<UpgradeData> _runPool = new List<UpgradeData>();

    public void InitializePool()
    {
        _runPool.Clear();
        _runPool.AddRange(_allUpgradesPool);
    }

    public List<RuntimeUpgrade> GetRandomUpgrades(int count)
    {
        List<UpgradeData> tempPickPool = new List<UpgradeData>(_runPool);
        List<RuntimeUpgrade> result = new List<RuntimeUpgrade>();

        int countToGenerate = Mathf.Min(count, tempPickPool.Count);

        for (int i = 0; i < countToGenerate; i++)
        {
            int randomIndex = Random.Range(0, tempPickPool.Count);
            UpgradeData selectedData = tempPickPool[randomIndex];

            result.Add(new RuntimeUpgrade(selectedData));

            tempPickPool.RemoveAt(randomIndex);
        }

        return result;
    }

    public void RemoveIfOneTime(UpgradeData data)
    {
        if (data.IsOneTime)
        {
            _runPool.Remove(data);
        }
    }
}