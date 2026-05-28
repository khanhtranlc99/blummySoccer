using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public abstract class ObjectPool<T> : IObjectPool<T>
{
    protected IObjectFactory<T> factory;

    protected Stack<T> cacheStack = new Stack<T>();

    protected List<T> activeList = new List<T>();

    public virtual int RecycleCount => cacheStack.Count;

    public virtual int Count => cacheStack.Count;

    public virtual T Allocate()
    {
        T val = (cacheStack.Count == 0) ? factory.Create() : cacheStack.Pop();
        activeList.Add(val);
        return val;
    }

    public virtual void Recycle(T obj)
    {
        cacheStack.Push(obj);
        activeList.Remove(obj);
    }

    public virtual void Spawn2Cache(int count)
    {
        if (count > 0)
        {
            for (int i = 0; i != count; i++)
            {
                T item = factory.Create();
                cacheStack.Push(item);
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("[ERROR]Spawn2Cache失败");
        }
    }

    public virtual void Release()
    {
        cacheStack.Clear();
    }

    public T[] GetActiveObjects()
    {
        return activeList.ToArray();
    }
}
