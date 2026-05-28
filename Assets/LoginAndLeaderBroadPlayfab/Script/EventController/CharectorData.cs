using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public static class EnumGame
{
    public enum Gender
    {
        MaleScrawny, Female, MaleScrawny2, Female2, MaleScrawny3, Female3, MaleFat
    }
}
[System.Serializable]
public class DataItemAcc
{
    public int idAcc;
    public int idByType;
    //public EnumGame.TypeAcc typeAcc;
    public string nameAcc;
    public string typeAccessories;
    public int price;
    public int levelSale;
}
[CreateAssetMenu(fileName = "CharectorData.asset", menuName = "CharectorDatabase/CharectorData")]
public class CharectorData : SerializedScriptableObject
{

    public int idCharector;
    public EnumGame.Gender gender;
    public int maxHealth;
    public int powerPoint;
    public int criticalRate;
    public List<DataItemAcc> dataItemAccs;
    public List<IdAndObjectAssetType> lsObjectType;
    public List<CurrentItem> lsCurrentItems
    {

        get
        {
            var temp = new List<CurrentItem>();
          
            foreach (var item in lsObjectType)
            {
                if (item.CurrentItem(gender) != null)
                {
                    temp.Add(new CurrentItem() { typeObjectInBody = item.typeObjectInBody, idAndObjectAsset = item.CurrentItem(gender) });
                }
            }
            return temp;
        }


    } 
        
        
   


    public void Init ()
    {


    }

  


}
[System.Serializable]
public class CurrentItem
{
    public TypeObjectInBody typeObjectInBody;
    public IdAndObjectAsset idAndObjectAsset;

}






[System.Serializable]
public class IdAndObjectAssetType
{
    public TypeObjectInBody typeObjectInBody;
    public List<IdAndObjectAsset> lsIdAndObjectAssets;
    



    public IdAndObjectAsset CurrentItem(EnumGame.Gender paramGender)
    {
       
        foreach (var item in lsIdAndObjectAssets)
        {
           
                if (PlayerPrefs.GetInt("EquipItemAcc" + paramGender + item.idAsset) == 1)
                {
                    return item;
                }
              
          
          
        }
        return null;
        
    }

}


[System.Serializable]
public class IdAndObjectAsset
{
    public int idAsset;
    public string strAsset;
    public GameObject objectAsset;
    public Vector3 responsiVector;
}
