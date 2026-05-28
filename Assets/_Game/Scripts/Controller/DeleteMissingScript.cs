// using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;


#if UNITY_EDITOR
public class DeleteMissingScript : MonoBehaviour
{
    // [Button]
    void RunRemove()
    {
        RemoveInvalidScriptsInChildren(transform);
    }

    void RemoveInvalidScriptsInChildren(Transform parent)
    {
        Component[] componentsParent = parent.GetComponents<Component>();
        foreach (Component component in componentsParent)
        {
            if (component == null)
            {
                GameObjectUtility.RemoveMonoBehavioursWithMissingScript(parent.gameObject);
            }
        }


        // Duyệt qua tất cả các child của parent
        foreach (Transform child in parent)
        {
            // Kiểm tra và loại bỏ script không hợp lệ trong child
            Component[] components = child.GetComponents<Component>();
            foreach (Component component in components)
            {
                if (component == null)
                {
                    GameObjectUtility.RemoveMonoBehavioursWithMissingScript(child.gameObject);
                }
            }

            // Nếu child có thêm child, tiếp tục đệ quy để duyệt qua các child đó
            if (child.childCount > 0)
            {
                RemoveInvalidScriptsInChildren(child);
            }
        }

    }
}

#endif