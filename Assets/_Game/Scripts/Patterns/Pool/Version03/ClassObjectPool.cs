using System.Collections.Generic;

public class ClassObjectPool<T> where T : new()
{
    private readonly Stack<T> _objects;
    private readonly int _maxSize;

    public ClassObjectPool(int initialCapacity, int maxSize)
    {
        _objects = new Stack<T>(initialCapacity);
        _maxSize = maxSize;

        for (int i = 0; i < initialCapacity; i++)
        {
            _objects.Push(new T());
        }
    }

    public T GetObject()
    {
        if (_objects.Count > 0)
        {
            return _objects.Pop();
        }
        else
        {
            return new T();
        }
    }

    public void ReturnObject(T obj)
    {
        if (_objects.Count < _maxSize)
        {
            _objects.Push(obj);
        }
    }
}