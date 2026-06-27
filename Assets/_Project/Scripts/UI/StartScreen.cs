using System;
using UnityEngine;
using UnityEngine.UI;

public class StartScreen : Window
{
    [SerializeField] private Button _playButton;
    [SerializeField] private Button _settingsButton;
    [SerializeField] private Button _aboutButton;

    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _aboutPanel;

    public event Action PlayButtonClicked;

    private void OnEnable()
    {
        _playButton.onClick.AddListener(OnPlayClick);
        _settingsButton.onClick.AddListener(OnSettingsClick);
        _aboutButton.onClick.AddListener(OnAboutClick);
    }

    private void OnDisable()
    {
        _playButton.onClick.RemoveListener(OnPlayClick);
        _settingsButton.onClick.RemoveListener(OnSettingsClick);
        _aboutButton.onClick.RemoveListener(OnAboutClick);
    }

    public override void Open()
    {
        base.Open();
        CloseAllSubMenus();
    }

    private void OnPlayClick()
    {
        PlayButtonClicked?.Invoke();
    }

    private void OnSettingsClick()
    {
        bool isAlreadyActive = _settingsPanel.activeSelf;
        CloseAllSubMenus();
        _settingsPanel.SetActive(!isAlreadyActive);
    }

    private void OnAboutClick()
    {
        bool isAlreadyActive = _aboutPanel.activeSelf;
        CloseAllSubMenus();
        _aboutPanel.SetActive(!isAlreadyActive);
    }

    private void CloseAllSubMenus()
    {
        _settingsPanel.SetActive(false);
        _aboutPanel.SetActive(false);
    }
}