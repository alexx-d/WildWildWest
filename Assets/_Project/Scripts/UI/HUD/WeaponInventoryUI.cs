using UnityEngine;

public class WeaponInventoryUI : MonoBehaviour
{
    [SerializeField] private WeaponInventory _inventory;
    [SerializeField] private WeaponSlotUI[] _slots = new WeaponSlotUI[5];

    private void Start()
    {
        InitSlots();
        SubscribeEvents();
        RefreshFullUI();
    }

    private void OnDestroy()
    {
        UnsubscribeEvents();
    }

    private void InitSlots()
    {
        foreach (var slot in _slots)
        {
            slot.Clear();
        }
    }

    private void SubscribeEvents()
    {
        _inventory.SlotUpdated += OnSlotUpdated;
        _inventory.ActiveSlotChanged += OnActiveSlotChanged;
    }

    private void UnsubscribeEvents()
    {
        _inventory.SlotUpdated -= OnSlotUpdated;
        _inventory.ActiveSlotChanged -= OnActiveSlotChanged;
    }

    private void RefreshFullUI()
    {
        if (_inventory == null)
        {
            return;
        }

        for (int i = 0; i < _slots.Length; i++)
        {
            Weapon weapon = _inventory.Slots[i];

            if (weapon != null)
            {
                _slots[i].Fill(weapon.Icon);
            }
        }

        OnActiveSlotChanged(_inventory.CurrentIndex);
    }

    private void OnSlotUpdated(int slotIndex, Weapon newWeapon)
    {
        if (newWeapon == null)
        {
            _slots[slotIndex].Clear();
            return;
        }

        if (slotIndex >= 0 && slotIndex < _slots.Length)
        {
            _slots[slotIndex].Fill(newWeapon.Icon);
        }
    }

    private void OnActiveSlotChanged(int activeSlotIndex)
    {
        for (int i = 0; i < _slots.Length; i++)
        {
            _slots[i].SetSelection(i == activeSlotIndex);
        }
    }
}