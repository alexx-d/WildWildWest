using UnityEngine;
using TMPro;

public class WaveUI : MonoBehaviour
{
    [SerializeField] private WaveSpawner _waveSpawner;

    [SerializeField] private TMP_Text _waveText;
    [SerializeField] private TMP_Text _enemiesText;

    private void OnEnable()
    {
        _waveSpawner.WaveChanged += UpdateWaveDisplay;
        _waveSpawner.EnemiesCountChanged += UpdateEnemiesDisplay;
    }

    private void OnDisable()
    {
        _waveSpawner.WaveChanged -= UpdateWaveDisplay;
        _waveSpawner.EnemiesCountChanged -= UpdateEnemiesDisplay;
    }

    private void UpdateWaveDisplay(int currentWave)
    {
        _waveText.text = $"Волна: {currentWave}";
    }

    private void UpdateEnemiesDisplay(int enemiesRemaining)
    {
        _enemiesText.text = enemiesRemaining > 0
                ? $"Врагов осталось: {enemiesRemaining}"
                : "Волна зачищена!";
    }
}