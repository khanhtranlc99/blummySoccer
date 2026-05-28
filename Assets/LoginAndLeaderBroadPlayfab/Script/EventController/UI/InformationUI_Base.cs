using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class InformationUI_Base : MonoBehaviour
{
    public Text tvHealth;
    public Text tvPowerPoint;
    public Image thumnailAvatar;
    public Image fillAmoutHealth;
    


    public virtual void Init(int paramHeath, int paramPowerPoint , Sprite paramSprite )
    {
        tvHealth.text = "" + paramHeath;
        tvPowerPoint.text = "" + paramPowerPoint;
        thumnailAvatar.sprite = paramSprite;
        fillAmoutHealth.fillAmount = 1;
    }



    public virtual void ShowText(int paramHeath, float paramAmoutHealth  )
    {
        tvHealth.text = "" + paramHeath;
        fillAmoutHealth.fillAmount = paramAmoutHealth;
    }

}
