using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class PlayerContainBase : MonoBehaviour
{
    public int idCharector;
    public EnumGame.Gender gender;
    public List<ObjectsInBody> lsObjectsInBody;
    public BodyPlayer bodyPlayer;
  [HideInInspector]  public InformationUI_Base informationUI;
    public int maxHealth;
    public int powerPoint;
    public int criticalRate;


    public abstract void Init(InformationUI_Base informationUI);
    public abstract void SetUpData();
    public abstract void SetUpBody();


}
[System.Serializable]
public class ObjectsInBody
{
    public TypeObjectInBody type;
    public List<ObjectAndId> lsObject;


    public void Init(EnumGame.Gender gender)
    {
      switch(type)
        {
            case TypeObjectInBody.Hair:

                break;
            case TypeObjectInBody.Trousers:
               
                break;
            case TypeObjectInBody.Glove:




                break;
            case TypeObjectInBody.Hat:

                break;
            case TypeObjectInBody.Shoes:

                break;
            case TypeObjectInBody.HeadBand:

                break;
            case TypeObjectInBody.HeadPhone:

                break;
            case TypeObjectInBody.Kneeband:

                break;
            case TypeObjectInBody.Watch:

                break;

        }
    }
}
[System.Serializable]
public class ObjectAndId
{
    public int id;
    public GameObject objectInBody;
}


[System.Serializable]
public class BodyPlayer
{
    public List<GameObject> lsBody_Object;
    public List<Texture> lsSkinBody_Texture;
    public void Init()
    {
        

    }


}



public enum TypeObjectInBody
{ 
    Hair = 0,
    Trousers = 1,
    Glove = 2,
    Hat = 3,
    Shoes = 4,
    HeadBand = 5,
    HeadPhone = 6,
    Kneeband = 7,
    Watch = 8
}
