using System;
using System.Collections.Generic;
using UnityEngine;

public class RewardPanelUI : Window
{
    [SerializeField] private List<UpgradeCardUI> _uiCards;

    public event Action<RuntimeUpgrade> UpgradeSelected;

    public void Open(List<RuntimeUpgrade> upgradesToShow)
    {
        base.Open();

        for (int i = 0; i < _uiCards.Count; i++)
        {
            if (i < upgradesToShow.Count)
            {
                _uiCards[i].gameObject.SetActive(true);
                _uiCards[i].Setup(upgradesToShow[i], OnCardChosen);
            }
            else
            {
                _uiCards[i].gameObject.SetActive(false);
            }
        }
    }

    private void OnCardChosen(RuntimeUpgrade chosenUpgrade)
    {
        Close();
        UpgradeSelected?.Invoke(chosenUpgrade);
    }
}