using System;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreen : Window
{
    [SerializeField] private Button _restartButton;

    public event Action RestartButtonClicked;

    private void OnEnable()
    {
        _restartButton.onClick.AddListener(OnRestartButtonClick);
    }

    private void OnDisable()
    {
        _restartButton.onClick.RemoveListener(OnRestartButtonClick);
    }

    private void OnRestartButtonClick()
    {
        RestartButtonClicked?.Invoke();
    }
}