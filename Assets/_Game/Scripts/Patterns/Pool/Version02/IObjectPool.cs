public interface IObjectPool<T>
{
    T Allocate();

    void Recycle(T obj);
}
