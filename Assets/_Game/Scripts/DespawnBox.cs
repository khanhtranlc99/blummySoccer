using Core.Pool;
using UnityEngine;

public class DespawnBox : MonoBehaviour
{
    // PoolIdentify poolIdentify;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.GetComponent<PoolIdentify>() != null)
        {
            CreateController.Instance.Despawn(other.gameObject);
        }
    }
}
