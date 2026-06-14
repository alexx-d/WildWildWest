using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ComponentPool<T> : MonoBehaviour where T : Component
{
    [SerializeField] private T _prefab;
    [SerializeField] private int _poolCapacity = 20;
    [SerializeField] private int _poolMaxSize = 50;

    [SerializeField] private Transform _container;

    private ObjectPool<T> _pool;
    private readonly List<T> _activeObjects = new List<T>();

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: () => Instantiate(_prefab, _container),
            actionOnGet: null,
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj.gameObject),
            collectionCheck: false,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize
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