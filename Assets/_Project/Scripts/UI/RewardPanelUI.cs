using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanelUI : MonoBehaviour
{
    [SerializeField] private GameObject _panelRoot;
    [SerializeField] private List<UpgradeCardUI> _uiCards;
    [SerializeField] private List<UpgradeData> _allUpgradesPool;

    public event Action<UpgradeData> UpgradeSelected;

    public void ShowRewardSelection()
    {
        _panelRoot.SetActive(true);

        List<UpgradeData> randomCards = GetRandomUniqueUpgrades(3);

        for (int i = 0; i < _uiCards.Count; i++)
        {
            if (i < randomCards.Count)
            {
                _uiCards[i].gameObject.SetActive(true);
                _uiCards[i].Setup(randomCards[i], OnCardChosen);
            }
            else
            {
                _uiCards[i].gameObject.SetActive(false);
            }
        }
    }

    private List<UpgradeData> GetRandomUniqueUpgrades(int count)
    {
        List<UpgradeData> poolCopy = new List<UpgradeData>(_allUpgradesPool);
        List<UpgradeData> result = new List<UpgradeData>();

        for (int i = 0; i < count; i++)
        {
            if (poolCopy.Count == 0) break;

            int randomIndex = UnityEngine.Random.Range(0, poolCopy.Count);
            UpgradeData original = poolCopy[randomIndex];

            UpgradeData runtimeClone = Instantiate(original);

            runtimeClone.InitializeValue();

            result.Add(runtimeClone);
            poolCopy.RemoveAt(randomIndex);
        }
        return result;
    }

    private void OnCardChosen(UpgradeData chosenUpgrade)
    {
        _panelRoot.SetActive(false);
        UpgradeSelected?.Invoke(chosenUpgrade);
    }
}