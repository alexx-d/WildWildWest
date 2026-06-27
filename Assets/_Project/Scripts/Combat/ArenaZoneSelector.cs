using System.Collections.Generic;
using UnityEngine;

public class ArenaZoneSelector : MonoBehaviour
{
    [SerializeField] private List<Arena> _arenas;

    private Arena _currentArena;
    private int _lastArenaIndex = -1;

    private void Awake()
    {
        foreach (var arena in _arenas)
        {
            if (arena != null)
            {
                arena.SetArenaActive(false);
            }
        }
    }

    public Transform[] SetupRandomArena(Transform playerTransform)
    {
        if (_currentArena != null)
        {
            _currentArena.SetArenaActive(false);
        }

        int randomIndex = _lastArenaIndex;

        if (_arenas.Count > 1)
        {
            while (randomIndex == _lastArenaIndex)
            {
                randomIndex = Random.Range(0, _arenas.Count);
            }
        }
        else
        {
            randomIndex = 0;
        }

        _lastArenaIndex = randomIndex;
        _currentArena = _arenas[randomIndex];
        _currentArena.SetArenaActive(true);

        TeleportPlayer(playerTransform, _currentArena.PlayerSpawnPoint);

        return _currentArena.EnemySpawnPoints;
    }

    private void TeleportPlayer(Transform player, Transform targetPoint)
    {
        if (player == null || targetPoint == null) return;

        var characterController = player.GetComponent<CharacterController>();

        if (characterController != null)
        {
            characterController.enabled = false;
        }

        player.position = targetPoint.position;
        player.rotation = targetPoint.rotation;

        if (characterController != null)
        {
            characterController.enabled = true;
        }
    }
}