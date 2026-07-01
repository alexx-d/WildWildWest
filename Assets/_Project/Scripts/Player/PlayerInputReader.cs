using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputReader : MonoBehaviour, Controls.IPlayerActions, Controls.IUIActions
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
    public event Action<int> WeaponChanged;

    public event Action PausePressed;
    public event Action UnpausePressed;

    private Controls _controls;

    private void OnEnable()
    {
        if (_controls == null)
        {
            _controls = new Controls();
            _controls.Player.SetCallbacks(this);
            _controls.UI.SetCallbacks(this);
        }

        EnableGameplayInput();
    }

    private void OnDisable()
    {
        _controls.Player.Disable();
        _controls.UI.Disable();
    }

    public void EnableGameplayInput()
    {
        _controls.UI.Disable();
        _controls.Player.Enable();
    }

    public void EnableMenuInput()
    {
        _controls.Player.Disable();
        _controls.UI.Enable();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        Looked?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Moved?.Invoke(context.ReadValue<Vector2>());
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

    public void OnChangeWeapon(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            string controlName = context.control.name;
            char lastChar = controlName[controlName.Length - 1];

            if (char.IsDigit(lastChar))
            {
                int keyNumber = (int)char.GetNumericValue(lastChar);
                int slotIndex = keyNumber - 1;

                WeaponChanged?.Invoke(slotIndex);
            }
        }
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

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            PausePressed?.Invoke();
        }
    }

    public void OnUnpause(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            UnpausePressed?.Invoke();
        }
    }
}