using System;
using UnityEngine;

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

    public void Play()
    {
        _particleSystem.Play();
    }

    private void OnParticleSystemStopped()
    {
        Died?.Invoke(this);
    }
}