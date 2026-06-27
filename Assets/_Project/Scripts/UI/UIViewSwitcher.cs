using UnityEngine;

public class UIViewSwitcher : MonoBehaviour
{
    [SerializeField] private GameObject _hudLayer;

    [SerializeField] private StartScreen _startScreen;
    [SerializeField] private EndGameScreen _gameOverScreen;
    [SerializeField] private VictoryScreen _victoryScreen;
    [SerializeField] private PauseScreen _pauseScreen;

    public StartScreen StartScreen => _startScreen;
    public EndGameScreen GameOverScreen => _gameOverScreen;
    public VictoryScreen VictoryScreen => _victoryScreen;
    public PauseScreen PauseScreen => _pauseScreen;

    public void ShowMainMenu()
    {
        _gameOverScreen.Close();
        _victoryScreen.Close();
        _pauseScreen.Close();
        _hudLayer.SetActive(false);

        _startScreen.Open();
    }

    public void ShowGameplayHUD()
    {
        _startScreen.Close();
        _gameOverScreen.Close();
        _victoryScreen.Close();
        _pauseScreen.Close();

        _hudLayer.SetActive(true);
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
}