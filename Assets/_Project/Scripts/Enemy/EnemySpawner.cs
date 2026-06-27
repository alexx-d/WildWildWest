using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private Transform _enemiesContainer;
    [SerializeField] private Transform _projectilesContainer;

    private readonly Dictionary<Enemy, ComponentPool<Enemy>> _poolsDictionary = new();
    private Transform _playerTransform;
    private Health _playerHealth;

    private readonly List<Enemy> _activeEnemies = new List<Enemy>();

    public event Action OnEnemyKilled;

    public void SetPlayerTarget(Transform player, Health playerHealth)
    {
        _playerTransform = player;
        _playerHealth = playerHealth;
    }

    public void SpawnEnemy(Enemy prefab, Vector3 position)
    {
        if (_poolsDictionary.TryGetValue(prefab, out var pool) == false)
        {
            pool = new ComponentPool<Enemy>(prefab, _enemiesContainer);
            _poolsDictionary[prefab] = pool;
        }

        Enemy enemy = pool.Get();
        _activeEnemies.Add(enemy);

        if (enemy.TryGetComponent<NavMeshAgent>(out var agent))
        {
            agent.Warp(position);
        }
        else
        {
            enemy.transform.position = position;
        }

        enemy.SetupTarget(_playerTransform, _playerHealth, _projectilesContainer);

        enemy.Died += HandleEnemyDeath;
    }

    public void ClearAllPools()
    {
        foreach (var enemy in _activeEnemies)
        {
            if (enemy != null)
            {
                enemy.Died -= HandleEnemyDeath;
            }
        }

        _activeEnemies.Clear();

        foreach (var pool in _poolsDictionary.Values)
        {
            pool.Reset();
        }
    }

    private void HandleEnemyDeath(Enemy enemy)
    {
        enemy.Died -= HandleEnemyDeath;
        _activeEnemies.Remove(enemy);
        OnEnemyKilled?.Invoke();
    }
}