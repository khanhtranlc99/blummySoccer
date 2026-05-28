using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using Sirenix.OdinInspector;
using Newtonsoft.Json;
using DG.Tweening;
using System.Reflection;
using System;

public class ConfigManager : MonoBehaviour
{
    // [Header("USER DATA")]
    // #region USER DATA
    // public UserData UserData;
    // #endregion

    //Data for popups, game events
    //[Header("GAME DATA")]
    #region GAME DATA


    #endregion

    //Data for common assets game like RewardConfig, SpriteConfig,...
    [Header("CONFIG DATA")]
    #region CONFIG DATA
    public RewardConfig RewardConfig;
    public CurveConfig CurveConfig;

    #endregion
    private void Start()
    {
        // EventHandler.RegisterEvent(EventID.m_save_data, delegate{
        //     UserKnifeConfig.SaveDataConfig();
        // });
    }

    // [Button("Remove all data")]
    public void RemoveAllData()
    {
        // UserData.DeleteData();
        PlayerPrefs.DeleteAll();
    }



    public void ConvertGPGSDataToLocalData(byte[] data)
    {
        // if (data.Length == 0)
        // {
        //     UserData.DeleteData();
        //     Debug.LogError("Have no data in google account");
        // }
        // else
        // {
        //     UserData UserData = DataManager.GetDeserializationObject<UserData>(data);
        //     this.UserData = UserData;
        //     UserData.UserIdentificationData.SaveData();
        // }
    }


}
