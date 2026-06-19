using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(EnemyTargetChase))]
[RequireComponent(typeof(EnemyMeleeAttack))]
[RequireComponent(typeof(EnemyDeathHandler))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private CharacterData _config;
    [SerializeField] private Transform _player;
    [SerializeField] private Health _playerHealth;

    private Health _health;
    private EnemyTargetChase _chase;
    private EnemyMeleeAttack _attack;
    private EnemyDeathHandler _deathHandler;

    private void Awake()
    {
        _health = GetComponent<Health>();
        _chase = GetComponent<EnemyTargetChase>();
        _attack = GetComponent<EnemyMeleeAttack>();
        _deathHandler = GetComponent<EnemyDeathHandler>();

        _deathHandler.Initialize(_health);
        _chase.Initialize(_config, _player);
        _attack.Initialize(_player, _playerHealth);
    }

    private void OnEnable()
    {
        _health.ResetHealth();
        _deathHandler.SetPhysicsActive(true);
        _chase.enabled = true;
        _attack.enabled = true;
    }

    public void SetupTarget(Transform player, Health playerHealth)
    {
        _chase.Initialize(_config, player);
        _attack.Initialize(player, playerHealth);
    }
}