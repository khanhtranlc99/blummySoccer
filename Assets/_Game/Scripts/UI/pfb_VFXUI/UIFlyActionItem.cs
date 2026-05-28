using UnityEngine;
using UnityEngine.UI;
public class UIFlyActionItem : MonoBehaviour, IDead
{
    public Image imgNode;

    public void IDead()
    {
        CreateController.Instance.Despawn(this.gameObject);
    }

    public void Init(Sprite spr)
    {
        this.imgNode.sprite = spr;
    }
}
