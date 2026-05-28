
using UnityEngine;

public class SingletonScriptableObject<T> : ScriptableObject where T : SingletonScriptableObject<T>
{
    private static T instance;
    public static T i
    {
        get
        {
            if (instance == null)
            {
                instance = Resources.Load<T>("ScriptableObject/" + typeof(T).Name);
                if (instance == null)
                {
                    Debug.LogError($"SingletonScriptableObject: No instance of {typeof(T).Name} found in Resources.");
                }
            }
            return instance;
        }
    }
}
