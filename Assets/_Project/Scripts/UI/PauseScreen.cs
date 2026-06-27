using System;
using UnityEngine;
using UnityEngine.UI;

public class PauseScreen : Window
{
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _restartButton;

    public event Action ResumeButtonClicked;
    public event Action RestartButtonClicked;

    protected override void Awake()
    {
        base.Awake();

        _resumeButton.onClick.AddListener(() => ResumeButtonClicked?.Invoke());
        _restartButton.onClick.AddListener(() => RestartButtonClicked?.Invoke());
    }
}