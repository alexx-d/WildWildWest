using System.Collections.Generic;
using UnityEngine;

public class UIViewSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _hudLayer;

    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private EndGameScreen _gameOverScreen;
    [SerializeField] private VictoryScreen _victoryScreen;
    [SerializeField] private PauseScreen _pauseScreen;
    [SerializeField] private RewardPanelUI _rewardPanelScreen;

    public StartScreen StartScreen => _startScreen;
    public EndGameScreen GameOverScreen => _gameOverScreen;
    public VictoryScreen VictoryScreen => _victoryScreen;
    public PauseScreen PauseScreen => _pauseScreen;
    public RewardPanelUI RewardPanelScreen => _rewardPanelScreen;

    private Window[] _allScreens;

    private void Awake()
    {
        _allScreens = new Window[]
        {   _startScreen,
            _gameOverScreen,
            _victoryScreen,
            _pauseScreen,
            _rewardPanelScreen
        };
    }

    public void ShowMainMenu()
    {
        CloseAllScreens();
        _hudLayer.SetActive(false);

        _startScreen.Open();
    }

    public void ShowGameplayHUD()
    {
        CloseAllScreens();
        _hudLayer.SetActive(true);
    }

    public void ShowRewardSelection(List<RuntimeUpgrade> upgrades)
    {
        _rewardPanelScreen.Open(upgrades);
    }

    public void ShowGameOver()
    {
        _gameOverScreen.Open();
    }

    public void ShowVictory()
    {
        _victoryScreen.Open();
    }

    public void ShowPause()
    {
        _pauseScreen.Open();
    }

    public void HidePause()
    {
        _pauseScreen.Close();
    }

    private void CloseAllScreens()
    {
        foreach (Window screen in _allScreens)
        {
            if (screen != null)
            {
                screen.Close();
            }
        }
    }
}