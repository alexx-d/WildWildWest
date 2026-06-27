using System;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class PoolableParticle : MonoBehaviour, IPoolable<PoolableParticle>
{
    public event Action<PoolableParticle> Died;
    private ParticleSystem _particleSystem;

    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        var main = _particleSystem.main;
        main.stopAction = ParticleSystemStopAction.Callback;
    }

    private void OnDisable()
    {
        if (_particleSystem != null)
        {
            _particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    public void Play()
    {
        _particleSystem.Clear();
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        Died?.Invoke(this);
    }
}