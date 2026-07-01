using UnityEngine;
using System;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _input;
    [SerializeField] private Health _playerHealth;
    [SerializeField] private PlayerMover _mover;
    [SerializeField] private PlayerCrouch _crouchController;
    [SerializeField] private PlayerRotation _rotation;
    [SerializeField] private GroundDetector _groundDetector;
    [SerializeField] private PlayerStats _stats;

    [SerializeField] private WeaponInventory _weaponInventory;
    [SerializeField] private PlayerAnimation _animation;

    private Weapon _activeWeapon;
    private bool _isAimingRequested;

    public int UpgradesCount => _stats.UpgradesCount;
    public Health Health => _playerHealth;

    private void OnEnable()
    {
        _input.Moved += _mover.SetDirection;
        _input.Moved += _animation.PlayMove;
        _input.Looked += _rotation.SetLookDelta;

        _input.JumpPressed += OnJumpRequested;
        _input.CrouchPressed += _mover.SetCrouchState;
        _input.CrouchPressed += _crouchController.SetCrouchState;
        _input.CrouchPressed += _animation.SetCrouched;
        _groundDetector.GroundedChanged += _animation.SetGrounded;

        _input.ShootPressed += OnShootPressed;
        _input.ShootReleased += OnShootReleased;
        _input.AimPressed += OnAimPressed;
        _input.AimReleased += OnAimReleased;
        _input.ReloadPressed += OnReloadRequested;
        _input.WeaponScrolled += _weaponInventory.ScrollWeapon;
        _input.WeaponChanged += _weaponInventory.SelectWeapon;

        _weaponInventory.WeaponChanged += OnWeaponChanged;
        _stats.MoveSpeedMultiplierChanged += OnMoveSpeedChanged;

        if (_weaponInventory.CurrentWeapon != null)
        {
            OnWeaponChanged(_weaponInventory.CurrentWeapon);
        }
    }

    private void OnDisable()
    {
        _input.Moved -= _mover.SetDirection;
        _input.Moved -= _animation.PlayMove;
        _input.Looked -= _rotation.SetLookDelta;

        _input.JumpPressed -= OnJumpRequested;
        _input.CrouchPressed -= _mover.SetCrouchState;
        _input.CrouchPressed -= _crouchController.SetCrouchState;
        _input.CrouchPressed -= _animation.SetCrouched;
        _groundDetector.GroundedChanged -= _animation.SetGrounded;

        _input.ShootPressed -= OnShootPressed;
        _input.ShootReleased -= OnShootReleased;
        _input.AimPressed -= OnAimPressed;
        _input.AimReleased -= OnAimReleased;
        _input.ReloadPressed -= OnReloadRequested;
        _input.WeaponScrolled -= _weaponInventory.ScrollWeapon;
        _input.WeaponChanged -= _weaponInventory.SelectWeapon;

        _weaponInventory.WeaponChanged -= OnWeaponChanged;
        _stats.MoveSpeedMultiplierChanged -= OnMoveSpeedChanged;

        if (_activeWeapon != null)
        {
            _activeWeapon.Fired -= OnWeaponFired;
        }
    }

    public void ApplyUpgrade(UpgradeType type, float value, Weapon weaponPrefab)
    {
        if (type == UpgradeType.NewWeapon)
        {
            if (weaponPrefab != null) _weaponInventory.PickupWeapon(weaponPrefab);
            return;
        }

        if (type == UpgradeType.Heal)
        {
            _playerHealth.Heal(value);
            return;
        }

        _stats.ApplyUpgrade(type, value);
    }

    public void ResetStatus()
    {
        _playerHealth.ResetHealth();
        _weaponInventory.ResetWeapons();
        _stats.Reset();

        _isAimingRequested = false;
        UpdateAimState();
    }

    private void OnShootPressed() => _activeWeapon?.PullTrigger();
    private void OnShootReleased() => _activeWeapon?.ReleaseTrigger();
    private void OnReloadRequested() => _activeWeapon?.StartReload();

    private void OnAimPressed()
    {
        _isAimingRequested = true;
        UpdateAimState();
    }

    private void OnAimReleased()
    {
        _isAimingRequested = false;
        UpdateAimState();
    }

    private void UpdateAimState()
    {
        bool state = _activeWeapon != null && _isAimingRequested;
        _activeWeapon?.SetAimState(state);

        if (state) _rotation.StartAiming();
        else _rotation.StopAiming();
    }

    private void OnMoveSpeedChanged(float multiplier)
    {
        _mover.SetSpeedMultiplier(multiplier);
        _animation.SetSpeedMultiplier(multiplier);
    }

    private void OnWeaponChanged(Weapon newWeapon)
    {
        if (_activeWeapon != null)
        {
            _activeWeapon.Fired -= OnWeaponFired;
            _activeWeapon.ReleaseTrigger();
        }

        _activeWeapon = newWeapon;

        if (_activeWeapon != null)
        {
            _animation.SetWeaponHoldType(_activeWeapon.AnimIndex);
            _activeWeapon.Fired += OnWeaponFired;
            UpdateAimState();
        }
    }

    private void OnWeaponFired()
    {
        _animation.PlayAttack();

        _rotation.ApplyRecoil(_activeWeapon.RecoilPitch, _activeWeapon.RecoilYaw);
    }

    private void OnJumpRequested()
    {
        if (_groundDetector.IsGrounded)
        {
            _mover.Jump();
            _animation.PlayJump();
        }
    }
}