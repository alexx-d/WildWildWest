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

    private Weapon _activeWeapon;

    private void OnEnable()
    {
        _input.Moved += _mover.SetDirection;
        _input.Moved += _animation.PlayMove;
        _input.Looked += _rotation.SetLookDelta;

        _input.Jumped += OnJumpRequested;
        _input.Crouched += _mover.SetCrouchState;
        _input.Crouched += _crouchController.SetCrouchState;
        _input.Crouched += _animation.SetCrouched;
        _groundDetector.GroundedChanged += _animation.SetGrounded;

        _input.ShootPressed += OnShootPressed;
        _input.ShootReleased += OnShootReleased;
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

        _input.Jumped -= OnJumpRequested;
        _input.Crouched -= _mover.SetCrouchState;
        _input.Crouched -= _crouchController.SetCrouchState;
        _input.Crouched -= _animation.SetCrouched;
        _groundDetector.GroundedChanged -= _animation.SetGrounded;

        _input.ShootPressed -= OnShootPressed;
        _input.ShootReleased -= OnShootReleased;
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

    private void OnWeaponChanged(Weapon newWeapon)
    {
        if (_activeWeapon != null)
        {
            _activeWeapon.Fired -= OnWeaponFired;
        }

        _activeWeapon = newWeapon;

        if (_activeWeapon != null)
        {
            _animation.SetWeaponHoldType(_activeWeapon.AnimIndex);
            _activeWeapon.Fired += OnWeaponFired;
        }
    }

    private void OnWeaponFired()
    {
        _animation.PlayAttack();
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