using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private List<Weapon> _startingWeaponPrefabs;
    [SerializeField] private Transform _worldFXContainer;

    private readonly Weapon[] _slots = new Weapon[5];
    private int _currentIndex = -1;

    public event Action<int, Weapon> SlotUpdated;
    public event Action<int> ActiveSlotChanged;
    public event Action<Weapon> WeaponChanged;

    public Weapon CurrentWeapon => (IsValidIndex(_currentIndex)) ? _slots[_currentIndex] : null;
    public int CurrentIndex => _currentIndex;
    public Weapon[] Slots => _slots;

    private void Start()
    {
        SpawnStartingWeapons();
    }

    public void ScrollWeapon(float scrollDelta)
    {
        if (GetEquippedWeaponsCount() <= 1)
        {
            return;
        }

        int step = scrollDelta > 0f ? 1 : -1;
        int nextIndex = _currentIndex;

        for (int i = 0; i < _slots.Length; i++)
        {
            nextIndex = (nextIndex + step + _slots.Length) % _slots.Length;

            if (_slots[nextIndex] != null)
            {
                SelectWeapon(nextIndex);
                return;
            }
        }
    }

    public void SelectWeapon(int index)
    {
        if (IsValidIndex(index) == false || _slots[index] == null || index == _currentIndex)
        {
            return;
        }

        if (CurrentWeapon != null)
        {
            CurrentWeapon.ReleaseTrigger();
            CurrentWeapon.gameObject.SetActive(false);
        }

        _currentIndex = index;

        CurrentWeapon.gameObject.SetActive(true);

        ActiveSlotChanged?.Invoke(_currentIndex);
        WeaponChanged?.Invoke(CurrentWeapon);
    }

    public void PickupWeapon(Weapon weaponPrefab)
    {
        int targetSlot = weaponPrefab.AssignedSlot;

        if (IsValidIndex(targetSlot) == false || _slots[targetSlot] != null)
        {
            return;
        }

        Weapon newWeapon = Instantiate(weaponPrefab, transform);
        SetupParentConstraint(newWeapon.gameObject);

        newWeapon.gameObject.SetActive(false);
        newWeapon.Initialize(_playerCamera, _worldFXContainer);

        _slots[targetSlot] = newWeapon;

        SlotUpdated?.Invoke(targetSlot, newWeapon);

        SelectWeapon(targetSlot);
    }

    public void ResetWeapons()
    {
        if (CurrentWeapon != null)
        {
            CurrentWeapon.ReleaseTrigger();
        }

        _currentIndex = -1;
        WeaponChanged?.Invoke(null);
        ActiveSlotChanged?.Invoke(-1);

        for (int i = 0; i < _slots.Length; i++)
        {
            if (_slots[i] != null)
            {
                Destroy(_slots[i].gameObject);
                _slots[i] = null;
            }

            SlotUpdated?.Invoke(i, null);
        }

        SpawnStartingWeapons();
    }

    private void SpawnStartingWeapons()
    {
        foreach (var prefab in _startingWeaponPrefabs)
        {
            if (prefab != null)
            {
                PickupWeapon(prefab);
            }
        }
    }

    private void SetupParentConstraint(GameObject target)
    {
        ParentConstraint constraint = target.AddComponent<ParentConstraint>();
        ConstraintSource source = new ConstraintSource { sourceTransform = _weaponHolder, weight = 1f };
        constraint.AddSource(source);
        constraint.SetTranslationOffset(0, Vector3.zero);
        constraint.SetRotationOffset(0, Vector3.zero);
        constraint.constraintActive = true;
        constraint.locked = true;
    }

    private int GetEquippedWeaponsCount()
    {
        int count = 0;

        foreach (var weapon in _slots)
        {
            if (weapon != null)
            {
                count++;
            }
        }

        return count;
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < _slots.Length;
    }
}