using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ComponentPool<T> where T : Component
{
    private readonly T _prefab;
    private readonly Transform _container;
    private readonly ObjectPool<T> _pool;
    private readonly List<T> _activeObjects = new();

    public ComponentPool(T prefab, Transform container, int capacity = 20, int maxSize = 50)
    {
        _prefab = prefab;
        _container = container;

        _pool = new ObjectPool<T>(
            createFunc: () => Object.Instantiate(_prefab, _container),
            actionOnGet: (obj) => obj.gameObject.SetActive(true),
            actionOnRelease: (obj) =>
            {
                if (obj.transform.parent != _container)
                {
                    obj.transform.SetParent(_container);
                }
                obj.gameObject.SetActive(false);
            },
            actionOnDestroy: (obj) => Object.Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: capacity,
            maxSize: maxSize
        );
    }

    public T Get()
    {
        T obj = _pool.Get();
        _activeObjects.Add(obj);

        if (obj is IPoolable<T> poolable)
        {
            poolable.Died += Release;
        }

        return obj;
    }

    public void Release(T obj)
    {
        if (obj is IPoolable<T> poolable)
        {
            poolable.Died -= Release;
        }

        _activeObjects.Remove(obj);
        _pool.Release(obj);
    }

    public void Reset()
    {
        for (int i = _activeObjects.Count - 1; i >= 0; i--)
        {
            T obj = _activeObjects[i];
            if (obj is not null)
            {
                Release(obj);
            }
        }
    }
}