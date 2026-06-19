using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerInputReader _input;

    [SerializeField] private PlayerMover _mover;
    [SerializeField] private PlayerCrouch _crouchController;
    [SerializeField] private PlayerRotation _rotation;
    [SerializeField] private GroundDetector _groundDetector;

    [SerializeField] private WeaponInventory _weaponInventory;

    [SerializeField] private PlayerAnimation _animation;
    [SerializeField] private WeaponUI _weaponUI;
    [SerializeField] private CrosshairUI _crosshairUI;

    private Weapon _activeWeapon;
    private bool _isAimingRequested;

    private void Update()
    {
        if (_activeWeapon != null)
        {
            _crosshairUI.UpdateSpread(_activeWeapon.CurrentSpread);
        }
    }

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

        _weaponInventory.WeaponChanged += OnWeaponChanged;

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

        _weaponInventory.WeaponChanged -= OnWeaponChanged;

        if (_activeWeapon != null)
        {
            _activeWeapon.Fired -= OnWeaponFired;
        }
    }

    private void OnShootPressed()
    {
        if (_weaponInventory.CurrentWeapon != null)
        {
            _weaponInventory.CurrentWeapon.PullTrigger();
        }
            
    }

    private void OnShootReleased()
    {
        if (_weaponInventory.CurrentWeapon != null)
        {
            _weaponInventory.CurrentWeapon.ReleaseTrigger();
        }
    }

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
        if (_activeWeapon != null && _isAimingRequested)
        {
            _activeWeapon.SetAimState(true);
            _rotation.StartAiming();
        }
        else
        {
            if (_activeWeapon != null)
            {
                _activeWeapon.SetAimState(false);
            }
            _rotation.StopAiming();
        }
    }

    private void OnReloadRequested()
    {
        if (_activeWeapon != null)
        {
            _activeWeapon.StartReload();
        }
    }

    private void OnWeaponChanged(Weapon newWeapon)
    {
        if (_activeWeapon != null)
        {
            _activeWeapon.Fired -= OnWeaponFired;
            _activeWeapon.ReloadStarted -= OnWeaponReloadStarted;
            _activeWeapon.ReleaseTrigger();
        }

        _activeWeapon = newWeapon;
        _weaponUI.UpdateActiveWeapon(_activeWeapon);

        if (_activeWeapon != null)
        {
            _animation.SetWeaponHoldType(_activeWeapon.AnimIndex);
            _activeWeapon.Fired += OnWeaponFired;
            _activeWeapon.ReloadStarted += OnWeaponReloadStarted;
            UpdateAimState();
        }
    }

    private void OnWeaponFired()
    {
        _animation.PlayAttack();

        if (_activeWeapon != null && _rotation != null)
        {
            _rotation.ApplyRecoil(_activeWeapon.RecoilPitch, _activeWeapon.RecoilYaw);
        }
    }

    private void OnWeaponReloadStarted()
    {
        //_animation.PlayReload();
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