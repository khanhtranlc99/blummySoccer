using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Core.Pool;
// using Sirenix.OdinInspector;

public class CreateController : MonoSingleton<CreateController>
{
    // [ListDrawerSettings(ShowIndexLabels = true, ShowFoldout = true, NumberOfItemsPerPage = 30)]
    public List<ObjectPoolItem> ListItemPool = new();
    public List<ObjectPoolItem> ListItemVFX = new();
#if UNITY_EDITOR
    // [Button("Create enum pool")]
    public void Create()
    {
        List<string> ListStrings = new List<string>();
        foreach (var item in ListItemPool)
        {
            ListStrings.Add(item.poolObject.name);
        }
        foreach (var item in ListItemVFX)
        {
            ListStrings.Add(item.poolObject.name);
        }
        EnumEditor.GenerateEnumScript(ListStrings, "PoolEnum");
    }
    // [Button("Update all Pool item")]
    public void UpdateAllPoolItem()
    {
        ListItemPool.ForEach(x => x._type = x.poolObject.name.ToEnum<PoolEnum>());
        ListItemVFX.ForEach(x => x._type = x.poolObject.name.ToEnum<PoolEnum>());
    }
#endif
    protected override void Awake()
    {
        base.Awake();
        Preload();
    }
    public void Preload()
    {
        foreach (var item in ListItemPool)
            if (item.poolAmount > 0)
                SmartPool.Instance.Preload(item.poolObject, item.poolAmount);
    }
    public GameObject GetPoolObject(PoolEnum _type, bool active = false)
    {
        ObjectPoolItem OPI = ListItemPool.Find(x => x._type == _type);
        if (OPI != null)
            return SmartPool.Instance.GetPoolObject(OPI.poolObject, active);
        return null;
    }
    public GameObject GetPoolItemVfx(PoolEnum _type, bool active = false)
    {
        ObjectPoolItem OPI = ListItemVFX.Find(x => x._type == _type);
        if (OPI != null)
            return SmartPool.Instance.GetPoolObject(OPI.poolObject, active);
        return null;
    }
    public void Despawn(GameObject gameObject)
    {
        SmartPool.Instance.Despawn(gameObject);
    }
    public void DespawnAll()
    {
        SmartPool.Instance.ReturnPoolAll();
    }
    public void SetParentToPool(GameObject gameObject)
    {
        SmartPool.Instance.SetParentToPool(gameObject);
    }

    [System.Serializable]
    public class ObjectPoolItem
    {
        public PoolEnum _type;
        public int poolAmount;
        public GameObject poolObject;
    }
}
