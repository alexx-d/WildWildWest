using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Gamedev/Wave Data")]
public class WaveData : ScriptableObject
{
    [Serializable]
    public class EnemySpawnGroup
    {
        public Enemy Prefab;
        public int Count;
    }

    [Header("Wave Enemies")]
    [SerializeField] private List<EnemySpawnGroup> _enemyGroups;

    [Header("Wave Settings")]
    [SerializeField] private float _spawnInterval = 1.0f;

    public List<EnemySpawnGroup> EnemyGroups => _enemyGroups;
    public float SpawnInterval => _spawnInterval;

    public int GetTotalEnemyCount()
    {
        int total = 0;
        foreach (var group in _enemyGroups)
        {
            total += group.Count;
        }
        return total;
    }
}