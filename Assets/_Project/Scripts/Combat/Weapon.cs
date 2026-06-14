using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private WeaponData _data;
    [SerializeField] private HitscanShooter _hitscanShooter;

    private float _nextFireTime;
    private bool _isTriggerPulled;

    public int AnimIndex => _data.AnimIndex;

    public event Action Fired;

    public void Initialize(Camera playerCamera)
    {
        if (_hitscanShooter != null)
        {
            _hitscanShooter.Initialize(playerCamera);
        }
    }

    public void PullTrigger()
    {
        _isTriggerPulled = true;
    }

    public void ReleaseTrigger()
    {
        _isTriggerPulled = false;
    }

    private void Update()
    {
        if (_isTriggerPulled)
        {
            TryExecuteShot();
        }
    }

    private void TryExecuteShot()
    {
        if (Time.time < _nextFireTime)
        {
            return;
        }

        _nextFireTime = Time.time + _data.FireRate;

        Fired?.Invoke();

        if (_data.IsProjectileBased)
        {
        }
        else
        {
            _hitscanShooter.Fire(_data.Damage, _data.Range, _data.Spread, Vector3.zero);
        }
    }

    private void OnDisable()
    {
        _isTriggerPulled = false;
    }
}