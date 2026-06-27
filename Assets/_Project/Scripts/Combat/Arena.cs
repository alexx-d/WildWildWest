using UnityEngine;

public class Arena : MonoBehaviour
{
    [SerializeField] private Transform _playerSpawnPoint;
    [SerializeField] private Transform[] _enemySpawnPoints;

    [SerializeField] private GameObject[] _arenaBarriers;

    public Transform PlayerSpawnPoint => _playerSpawnPoint;
    public Transform[] EnemySpawnPoints => _enemySpawnPoints;

    public void SetArenaActive(bool isActive)
    {
        foreach (var barrier in _arenaBarriers)
        {
            if (barrier != null) barrier.SetActive(isActive);
        }
    }
}