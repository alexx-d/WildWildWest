using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyTargetChase : MonoBehaviour
{
    private enum MoveState { Chasing, Circling, Wandering }

    [Header("Настройки навигации")]
    [SerializeField] private float _pathUpdateInterval = 0.2f;
    [SerializeField] private float _wanderRadius = 6f;

    [Header("Настройки кружения (Strafing)")]
    [SerializeField] private float _circleDistanceFactor = 0.85f;
    [SerializeField] private float _directionChangeInterval = 2.5f;
    [SerializeField] private float _strafeSpeedMultiplier = 0.5f;

    private float _baseSpeed;
    private NavMeshAgent _agent;
    private Transform _target;
    private float _attackDistance;

    private MoveState _currentState = MoveState.Chasing;
    private float _nextUpdateTime;
    private float _nextDirectionChangeTime;
    private int _strafeDirection = 1;

    public event Action<float, float, float> MovementChanged;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (_target == null || !_agent.enabled || !_agent.isOnNavMesh)
        {
            return;
        }

        UpdateMoveState();

        if (Time.time >= _nextUpdateTime)
        {
            _nextUpdateTime = Time.time + _pathUpdateInterval;
            ExecuteCurrentStateBehavior();
        }

        Vector3 localVelocity = transform.InverseTransformDirection(_agent.velocity);

        float velocityX = _baseSpeed > 0 ? localVelocity.x / _baseSpeed : 0f;
        float velocityY = _baseSpeed > 0 ? localVelocity.z / _baseSpeed : 0f;

        float speedMultiplier = _baseSpeed > 0 ? _agent.velocity.magnitude / _baseSpeed : 0f;

        MovementChanged?.Invoke(velocityX, velocityY, speedMultiplier);
    }

    public void Initialize(CharacterData config, Transform target, float attackDistance)
    {
        _target = target;
        _attackDistance = attackDistance;

        _agent.enabled = true;
        _baseSpeed = config.MoveSpeed;
        _agent.speed = _baseSpeed;

        _agent.stoppingDistance = 0.2f;

        _currentState = MoveState.Chasing;
        _strafeDirection = UnityEngine.Random.value > 0.5f ? 1 : -1;
    }

    private void UpdateMoveState()
    {
        if (_agent.pathStatus == NavMeshPathStatus.PathPartial)
        {
            if (_currentState != MoveState.Wandering)
            {
                _currentState = MoveState.Wandering;
                _agent.speed = _baseSpeed;
                _nextUpdateTime = 0f;
            }

            return;
        }

        float distance = Vector3.Distance(transform.position, _target.position);

        if (distance <= _attackDistance)
        {
            _currentState = MoveState.Circling;
            _agent.speed = _baseSpeed * _strafeSpeedMultiplier;
        }
        else
        {
            if (_currentState == MoveState.Wandering || distance > _attackDistance * 1.2f)
            {
                _currentState = MoveState.Chasing;
                _agent.speed = _baseSpeed;
            }
        }
    }

    private void ExecuteCurrentStateBehavior()
    {
        switch (_currentState)
        {
            case MoveState.Chasing:
                _agent.SetDestination(_target.position);
                break;

            case MoveState.Circling:
                HandleCirclingMovement();
                break;

            case MoveState.Wandering:
                HandleWanderMovement();
                break;
        }
    }

    private void HandleCirclingMovement()
    {
        if (Time.time >= _nextDirectionChangeTime)
        {
            _nextDirectionChangeTime = Time.time + UnityEngine.Random.Range(_directionChangeInterval * 0.7f, _directionChangeInterval * 1.3f);
            _strafeDirection = UnityEngine.Random.value > 0.5f ? 1 : -1;
        }

        Vector3 dirFromTarget = (transform.position - _target.position).normalized;

        Vector3 tangent = Vector3.Cross(dirFromTarget, Vector3.up) * _strafeDirection;

        float strafeModifier = Mathf.Clamp(_attackDistance * 0.4f, 0.5f, 2.5f);

        Vector3 desiredPosition = _target.position +
                                 (dirFromTarget * _attackDistance * _circleDistanceFactor) +
                                 (tangent * strafeModifier);

        _agent.SetDestination(desiredPosition);
    }

    private void HandleWanderMovement()
    {
        if (_agent.remainingDistance <= _agent.stoppingDistance || _agent.velocity.sqrMagnitude < 0.1f)
        {
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * _wanderRadius;
            randomDirection += transform.position;

            if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, _wanderRadius, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }
    }

    public void StopChasing()
    {
        if (_agent.enabled && _agent.isOnNavMesh)
        {
            _agent.isStopped = true;
        }
        _agent.enabled = false;
    }
}