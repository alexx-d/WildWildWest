using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private CampaignData _campaignData;
    [SerializeField] private ArenaZoneSelector _arenaZoneSelector;
    [SerializeField] private ScreenTransitionUI _transitionUI;

    [SerializeField] private int _delayBetweenWaves = 5;
    [SerializeField] private float _delayBeforeReward = 0.5f;
    [SerializeField] private float _fadeDuration = 0.5f;

    private int _currentWaveIndex = 0;
    private int _enemiesRemainingAlive = 0;
    private bool _isSpawningWave = false;
    private bool _isWaitingForReward = false;
    private Transform[] _currentSpawnPoints = new Transform[0];

    public event Action<int> WaveChanged;
    public event Action<int> EnemiesCountChanged;
    public event Action CampaignCompleted;
    public event Action FirstArenaPrepared;
    public event Action WaveCompleted;

    private void OnEnable()
    {
        _enemySpawner.OnEnemyKilled += TrackEnemyDeath;
    }

    private void OnDisable()
    {
        _enemySpawner.OnEnemyKilled -= TrackEnemyDeath;
    }

    public void InitializeWaves(Transform playerTransform)
    {
        StopAllCoroutines();

        _currentWaveIndex = 0;
        _enemiesRemainingAlive = 0;
        _isSpawningWave = false;

        StartCoroutine(WavesRoutine(playerTransform));
    }

    public void StartNextWave()
    {
        _isWaitingForReward = false;
    }

    private IEnumerator WavesRoutine(Transform playerTransform)
    {
        yield return _transitionUI.FadeToBlack(_fadeDuration).WaitForCompletion();
        _currentSpawnPoints = _arenaZoneSelector.SetupRandomArena(playerTransform);
        FirstArenaPrepared?.Invoke();
        yield return null;

        while (_currentWaveIndex < _campaignData.Waves.Count)
        {
            if (_currentWaveIndex > 0)
            {
                yield return new WaitForSeconds(_delayBeforeReward);
                _isWaitingForReward = true;
                WaveCompleted?.Invoke();
                yield return new WaitUntil(() => !_isWaitingForReward);

                int timeRemaining = _delayBetweenWaves;

                while (timeRemaining > 0)
                {
                    _transitionUI.UpdateTimerText(timeRemaining);
                    yield return new WaitForSeconds(1f);
                    timeRemaining--;
                }

                _transitionUI.HideTimer();

                yield return _transitionUI.FadeToBlack(_fadeDuration).WaitForCompletion();
                _currentSpawnPoints = _arenaZoneSelector.SetupRandomArena(playerTransform);
                yield return null;
            }

            WaveData currentWave = _campaignData.Waves[_currentWaveIndex];
            _enemiesRemainingAlive = currentWave.GetTotalEnemyCount();
            _isSpawningWave = true;

            WaveChanged?.Invoke(_currentWaveIndex + 1);
            EnemiesCountChanged?.Invoke(_enemiesRemainingAlive);

            yield return _transitionUI.FadeFromBlack(_fadeDuration).WaitForCompletion();

            yield return StartCoroutine(SpawnEnemies(currentWave));

            yield return new WaitUntil(() => _enemiesRemainingAlive <= 0 && !_isSpawningWave);

            _currentWaveIndex++;
        }

        CampaignCompleted?.Invoke();
    }

    private IEnumerator SpawnEnemies(WaveData wave)
    {
        foreach (var group in wave.EnemyGroups)
        {
            for (int i = 0; i < group.Count; i++)
            {
                if (_currentSpawnPoints.Length == 0)
                {
                    yield break;
                }

                Transform spawnPoint = _currentSpawnPoints[UnityEngine.Random.Range(0, _currentSpawnPoints.Length)];
                _enemySpawner.SpawnEnemy(group.Prefab, spawnPoint.position);

                yield return new WaitForSeconds(wave.SpawnInterval);
            }
        }
        _isSpawningWave = false;
    }

    private void TrackEnemyDeath()
    {
        _enemiesRemainingAlive--;
        EnemiesCountChanged?.Invoke(_enemiesRemainingAlive);
    }
}