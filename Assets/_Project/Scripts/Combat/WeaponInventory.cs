using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponInventory : MonoBehaviour
{
    [SerializeField] private Transform _weaponHolder;
    [SerializeField] private Camera _playerCamera;
    [SerializeField] private List<Weapon> _startingWeaponPrefabs;

    private List<Weapon> _equippedWeapons = new List<Weapon>();
    private int _currentIndex = -1;

    public event Action<Weapon> WeaponChanged;
    public Weapon CurrentWeapon => (_currentIndex >= 0 && _currentIndex < _equippedWeapons.Count) ? _equippedWeapons[_currentIndex] : null;

    private void Start()
    {
        foreach (var prefab in _startingWeaponPrefabs)
        {
            if (prefab != null)
            {
                PickupWeapon(prefab);
            }
        }
    }

    public void ScrollWeapon(float scrollDelta)
    {
        if (_equippedWeapons.Count <= 1)
        {
            return;
        }

        int newIndex = _currentIndex;

        if (scrollDelta > 0f)
        {
            newIndex = (_currentIndex + 1) % _equippedWeapons.Count;
        }
        else if (scrollDelta < 0f)
        {
            newIndex = (_currentIndex - 1 + _equippedWeapons.Count) % _equippedWeapons.Count;
        }

        SelectWeapon(newIndex);
    }

    public void SelectWeapon(int index)
    {
        if (index < 0 || index >= _equippedWeapons.Count || index == _currentIndex)
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
        WeaponChanged?.Invoke(CurrentWeapon);
    }

    public void PickupWeapon(Weapon weaponPrefab)
    {
        foreach (var equipped in _equippedWeapons)
        {
            if (equipped.AnimIndex == weaponPrefab.AnimIndex)
            {
                return;
            }
        }

        Weapon newWeapon = Instantiate(weaponPrefab, gameObject.transform);

        ParentConstraint constraint = newWeapon.gameObject.AddComponent<ParentConstraint>();

        ConstraintSource source = new ConstraintSource();
        source.sourceTransform = _weaponHolder;
        source.weight = 1;
        constraint.AddSource(source);

        constraint.SetTranslationOffset(0, Vector3.zero);
        constraint.SetRotationOffset(0, Vector3.zero);

        constraint.constraintActive = true;
        constraint.locked = true;

        newWeapon.gameObject.SetActive(false);

        newWeapon.Initialize(_playerCamera);

        _equippedWeapons.Add(newWeapon);

        if (_equippedWeapons.Count == 1)
        {
            SelectWeapon(0);
        }
    }
}