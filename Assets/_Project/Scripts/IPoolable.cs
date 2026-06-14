using System;

public interface IPoolable<T>
{
    public event Action<T> Died;
}