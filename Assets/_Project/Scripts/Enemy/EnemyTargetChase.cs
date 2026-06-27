using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyTargetChase : MonoBehaviour
{
    [SerializeField] private float _pathUpdateInterval = 0.2f;

    private NavMeshAgent _agent;
    private Health _health;
    private Transform _target;
    private float _nextUpdateTime;

    public event Action<float> SpeedChanged;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _health = GetComponent<Health>();
    }

    private void Update()
    {
        if (_target == null || !_agent.isOnNavMesh)
        {
            return;
        }

        if (Time.time >= _nextUpdateTime)
        {
            _nextUpdateTime = Time.time + _pathUpdateInterval;
            _agent.SetDestination(_target.position);
        }

        float currentSpeedNormalized = _agent.velocity.sqrMagnitude > 0.1f ? 1f : 0f;
        SpeedChanged?.Invoke(currentSpeedNormalized);
    }

    private void OnEnable()
    {
        _health.Died += OnDeath;
    }

    private void OnDisable()
    {
        _health.Died -= OnDeath;
    }

    public void Initialize(CharacterData config, Transform target, float attackDistance)
    {
        _target = target;
        _agent.speed = config.MoveSpeed;
        _agent.stoppingDistance = Mathf.Max(0.1f, attackDistance - 0.2f);
        _agent.enabled = true;
    }

    public void StopChasing()
    {
        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
        }

        _agent.enabled = false;
    }

    private void OnDeath()
    {
        if (_agent.isActiveAndEnabled && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
        }
        _agent.enabled = false;

        enabled = false;
    }
}