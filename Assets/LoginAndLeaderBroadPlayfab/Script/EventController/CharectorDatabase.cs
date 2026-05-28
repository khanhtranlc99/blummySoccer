using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
[CreateAssetMenu(fileName = "CharectorDatabase.asset", menuName = "CharectorDatabase/CharectorDatabaseScripable")]
public class CharectorDatabase : SerializedScriptableObject
{

    public List<CharectorData> lsCharectorDatas;
    public CharectorData CurrentCharector
    {
        get
        {
            return lsCharectorDatas[PlayerPrefs.GetInt("CharCurent")];
          
        }
    }    


    public void Init ()
    {
        SetUp();
    }   
    

    public void SetUp()
    {
        foreach (var item in lsCharectorDatas)
        {
            item.Init();
        }
    }    
}
