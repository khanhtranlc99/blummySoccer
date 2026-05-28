using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class LableTitkit : MonoBehaviour
{
    public Button btnBuy;
    public Text tvContent;
    public ShopTickitBox shopTickitBox;
    public abstract void Init(ShopTickitBox param);
}
