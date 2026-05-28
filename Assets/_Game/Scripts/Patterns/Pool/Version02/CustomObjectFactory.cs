using System;

public class CustomObjectFactory<T> : IObjectFactory<T>
{
    protected Func<T> factoryMethod;

    public CustomObjectFactory(Func<T> factoryMethod)
    {
        this.factoryMethod = factoryMethod;
    }

    public T Create()
    {
        return factoryMethod();
    }
}
