using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private WaveSpawner _waveSpawner;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private Player _player;
    [SerializeField] private PlayerInputReader _playerInput;

    [SerializeField] private ScreenTransitionUI _screenTransition;
    [SerializeField] private UIViewSwitcher _uiViewSwitcher;
    [SerializeField] private UpgradeProvider _upgradeProvider;

    [SerializeField] private BackgroundMusic _backgroundMusic;

    [SerializeField] private CinemachineVirtualCamera _menuVirtualCamera;
    [SerializeField] private MenuCameraRotator _menuCameraRotator;

    private bool _isPaused;

    private void Awake()
    {
        _enemySpawner.SetPlayerTarget(_player.transform, _player.Health);
    }

    private void Start()
    {
        Time.timeScale = 0;

        _uiViewSwitcher.ShowMainMenu();

        _menuVirtualCamera.Priority = 20;
        _menuCameraRotator.StartRotation();

        SetCursorState(isLocked: false);
        _playerInput.EnableMenuInput();
    }

    private void OnEnable()
    {
        _uiViewSwitcher.StartScreen.PlayButtonClicked += OnPlayButtonClick;
        _uiViewSwitcher.GameOverScreen.RestartButtonClicked += OnRestartButtonClick;
        _uiViewSwitcher.VictoryScreen.RestartButtonClicked += OnRestartButtonClick;

        _uiViewSwitcher.PauseScreen.ResumeButtonClicked += ResumeGame;
        _uiViewSwitcher.PauseScreen.RestartButtonClicked += OnRestartButtonClick;

        _playerInput.PausePressed += PauseGame;
        _playerInput.UnpausePressed += ResumeGame;

        _player.Health.Died += OnPlayerDie;
        _waveSpawner.CampaignCompleted += OnCampaignWin;
        _waveSpawner.FirstArenaPrepared += OnFirstArenaPrepared;
        _waveSpawner.WaveCompleted += OnWaveCompleted;

        _uiViewSwitcher.RewardPanelScreen.UpgradeSelected += OnUpgradeRewardSelected;
    }

    private void OnDisable()
    {
        _uiViewSwitcher.StartScreen.PlayButtonClicked -= OnPlayButtonClick;
        _uiViewSwitcher.GameOverScreen.RestartButtonClicked -= OnRestartButtonClick;
        _uiViewSwitcher.VictoryScreen.RestartButtonClicked -= OnRestartButtonClick;

        _uiViewSwitcher.PauseScreen.ResumeButtonClicked -= ResumeGame;
        _uiViewSwitcher.PauseScreen.RestartButtonClicked -= OnRestartButtonClick;

        _playerInput.PausePressed -= PauseGame;
        _playerInput.UnpausePressed -= ResumeGame;

        _player.Health.Died += OnPlayerDie;
        _waveSpawner.CampaignCompleted -= OnCampaignWin;
        _waveSpawner.FirstArenaPrepared -= OnFirstArenaPrepared;
        _waveSpawner.WaveCompleted -= OnWaveCompleted;

        _uiViewSwitcher.RewardPanelScreen.UpgradeSelected -= OnUpgradeRewardSelected;
    }
    private void OnWaveCompleted()
    {
        Time.timeScale = 0;
        _backgroundMusic.ApplyPauseEffects();

        SetCursorState(isLocked: false);
        _playerInput.EnableMenuInput();

        List<RuntimeUpgrade> availableUpgrades = _upgradeProvider.GetRandomUpgrades(_player.UpgradesCount);
        _uiViewSwitcher.ShowRewardSelection(availableUpgrades);
    }

    private void OnUpgradeRewardSelected(RuntimeUpgrade chosenUpgrade)
    {
        _player.ApplyUpgrade(chosenUpgrade.Data.Type, chosenUpgrade.Value, chosenUpgrade.Data.WeaponPrefab);
        _upgradeProvider.RemoveIfOneTime(chosenUpgrade.Data);

        Time.timeScale = 1;
        _backgroundMusic.RemovePauseEffects();
        SetCursorState(isLocked: true);
        _playerInput.EnableGameplayInput();

        _waveSpawner.StartNextWave();
    }

    private void OnPlayButtonClick()
    {
        _waveSpawner.InitializeWaves(_player.transform);
    }

    private void OnRestartButtonClick()
    {
        if (_isPaused)
        {
            ResumeGame();
        }

        _waveSpawner.InitializeWaves(_player.transform);
    }

    private void OnFirstArenaPrepared()
    {
        _menuCameraRotator.StopRotation();
        _menuVirtualCamera.Priority = 0;

        _enemySpawner.ClearAllPools();
        _player.ResetStatus();
        _screenTransition.ResetUI();

        _uiViewSwitcher.ShowGameplayHUD();
        _upgradeProvider.InitializePool();

        Time.timeScale = 1;
        _isPaused = false;
        _backgroundMusic.RemovePauseEffects();

        SetCursorState(isLocked: true);
        StartCoroutine(EnablePlayerInputRoutine());
    }

    private IEnumerator EnablePlayerInputRoutine()
    {
        yield return null;
        _playerInput.EnableGameplayInput();
    }

    private void PauseGame()
    {
        _isPaused = true;
        Time.timeScale = 0;
        _backgroundMusic.ApplyPauseEffects();

        _uiViewSwitcher.ShowPause();
        SetCursorState(isLocked: false);
        _playerInput.EnableMenuInput();
    }

    private void ResumeGame()
    {
        _isPaused = false;
        Time.timeScale = 1;
        _backgroundMusic.RemovePauseEffects();

        _uiViewSwitcher.HidePause();
        SetCursorState(isLocked: true);
        _playerInput.EnableGameplayInput();
    }

    private void OnPlayerDie()
    {
        Time.timeScale = 0;
        _backgroundMusic.ApplyPauseEffects();

        _uiViewSwitcher.ShowGameOver();

        SetCursorState(isLocked: false);
        _playerInput.EnableMenuInput();
    }

    private void OnCampaignWin()
    {
        Time.timeScale = 0;
        _backgroundMusic.ApplyPauseEffects();

        _uiViewSwitcher.ShowVictory();

        SetCursorState(isLocked: false);
        _playerInput.EnableMenuInput();
    }

    private void SetCursorState(bool isLocked)
    {
        Cursor.lockState = isLocked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !isLocked;
    }
}