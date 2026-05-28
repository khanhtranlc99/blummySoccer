using UnityEngine;

public class GameObjectPool : ObjectPool<GameObject>
{
    protected GameObject prefab;

    protected Transform root;

    public GameObjectPool(GameObject prefab, Transform root)
    {
        this.prefab = prefab;
        this.root = root;
        factory = new CustomObjectFactory<GameObject>(FactoryMethod);
    }

    private GameObject FactoryMethod()
    {
        GameObject gameObject = Object.Instantiate(prefab, root);
        gameObject.SetActive(value: false);
        return gameObject;
    }

    public override GameObject Allocate()
    {
        GameObject gameObject = base.Allocate();
        gameObject.SetActive(value: true);
        return gameObject;
    }

    public GameObject AllocateDontActive()
    {
        return base.Allocate();
    }

    public GameObject Allocate(Vector3 pos, Quaternion rotation)
    {
        GameObject gameObject = base.Allocate();
        gameObject.transform.position = pos;
        gameObject.transform.rotation = rotation;
        gameObject.SetActive(value: true);
        return gameObject;
    }

    public override void Recycle(GameObject obj)
    {
        base.Recycle(obj);
        if (obj.transform.parent != root)
        {
            obj.transform.SetParent(root, worldPositionStays: false);
        }
        obj.SetActive(value: false);
    }

    public override void Release()
    {
        while (cacheStack.Count != 0)
        {
            UnityEngine.Object.Destroy(cacheStack.Pop());
        }
    }
}
