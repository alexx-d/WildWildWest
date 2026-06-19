using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, Controls.IPlayerActions
{
    public event Action<Vector2> Moved;
    public event Action<Vector2> Looked;

    public event Action JumpPressed;
    public event Action ReloadPressed;
    public event Action<bool> CrouchPressed;

    public event Action ShootPressed;
    public event Action ShootReleased;
    public event Action AimPressed;
    public event Action AimReleased;
    public event Action<float> WeaponScrolled;

    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
        }
        _controls.Player.Enable();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Vector2 lookTarget = context.ReadValue<Vector2>();
        Looked?.Invoke(lookTarget);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 moveTarget = context.ReadValue<Vector2>();
        Moved?.Invoke(moveTarget);
    }

    public void OnShoot(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            ShootPressed?.Invoke();
        }
        else if (context.canceled)
        {
            ShootReleased?.Invoke();
        }
    }

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            AimPressed?.Invoke();
        }
        else if (context.canceled)
        {
            AimReleased?.Invoke();
        }
    }

    public void OnScrollWeapon(InputAction.CallbackContext context)
    {
        if (context.performed == false)
        {
            return;
        }

        float scrollValue = context.ReadValue<Vector2>().y;

        WeaponScrolled?.Invoke(scrollValue);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            JumpPressed?.Invoke();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            CrouchPressed?.Invoke(true);
        }
        else if (context.canceled)
        {
            CrouchPressed?.Invoke(false);
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            ReloadPressed?.Invoke();
        }
    }
}