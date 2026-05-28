using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : BaseData
{
    public string linkedId;

    public string namePlayer;
    public string linkAvatar;
    public string flagLink;


    public override void InitData() // gán các biến cần cần đẩy ở client vào đây dùng để đẩy lên sever
    {

        //currentLevel = UseProfile.CurrentLevel;
        //currentLevelPlay = GameController.Instance.useProfile.CurrentLevelPlay;
        namePlayer = UseProfile.NamePlayer;
        linkAvatar = UseProfile.NamePlayer;
        flagLink = UseProfile.FlagLink;
        linkedId = UseProfile.LinkedId;
    }

    public override void SaveToCloud(Action<UpdateUserDataResult> onSuccess, Action<PlayFabError> onFail, float timeout = 5, bool isForce = false)
    {
        GConnection.UpdateDataTable(GDataTable.PlayerData, onSuccess, onFail, timeout, isForce: isForce);
    }

    public override void SetData(string json) 
    {    
        var data = JsonUtility.FromJson<PlayerData>(json);
        SetData(data);

    }
    private void SetData(PlayerData data) // đa hình setdata, dùng để đồng bộ đata ở client với trên sever (dưới với trên)
    {
        //UseProfile.VersionData = data.version;
        //UseProfile.IsLoggedIn = data.isLogin;
        InitData();
    }
    public override void SetLocalData()
    {
        SetData(this);
    }

    public override string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static bool IsLocalDataNew(PlayerData paramData)
    {
        //return UseProfile.CurrentLevel >= paramData.currentLevel;
        return true; // điều kiện để xem là bản client hay server là cũ hay mới
    }

    public void SaveData(Action onSuccess = null, Action onFail = null, bool isFromDisk = true, bool isForce = false) /// Gửi data từ client lên Sever 
    {

        Debug.Log("Save Player Data");

        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            onFail?.Invoke();
            return;
        }
        if (!GameController.Instance.loginController.IsLoginPlayfab())
        {
            onFail?.Invoke();
            return;
        }
        if (isFromDisk) InitData();

        GConnection.UpdateDataTableBackground(GDataTable.PlayerData, _ => onSuccess?.Invoke(), err =>
        {

            Debug.LogError(err);
            onFail?.Invoke();
        }, isForce: isForce);
    }
    public void SetDataSync(PlayerData data)
    {
        SetData(data);

    }
   
}
