using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;
using BestHTTP.Extensions;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;


public class UseProfile : MonoBehaviour
{
    public static string LinkedId
    {
        get { return PlayerPrefs.GetString(StringHelper.LINKED_ID, ""); }
        set
        {

            PlayerPrefs.SetString(StringHelper.LINKED_ID, value);
            PlayerPrefs.Save();
        }
    }
    public static string FlagLink
    {
        get { return PlayerPrefs.GetString(StringHelper.FLAG_LINK, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.FLAG_LINK, value);
            PlayerPrefs.Save();
        }
    }
    public static string PlayFabId
    {
        get { return PlayerPrefs.GetString(StringHelper.PLAYFAB_ID, string.Empty); }
        set
        {
            PlayerPrefs.SetString(StringHelper.PLAYFAB_ID, value);
            PlayerPrefs.Save();
        }
    }


    public static string NamePlayer
    {
        get { return PlayerPrefs.GetString(StringHelper.NAME_PLAYER, ""); }
        set
        {
            
            PlayerPrefs.SetString(StringHelper.NAME_PLAYER, value);
            PlayFabClientAPI.UpdateUserTitleDisplayName(
                new UpdateUserTitleDisplayNameRequest { DisplayName = value }, s => { }, e => { });
            PlayerPrefs.Save();
          //  Debug.LogError(UseProfile.NamePlayer);
        }
    }

    public static string AvatarLink
    {
        get { return PlayerPrefs.GetString(StringHelper.AVATAR_LINK, ""); }
        set
        {
            PlayerPrefs.SetString(StringHelper.AVATAR_LINK, value);
            PlayerPrefs.Save();
        }
    }


    public static DateTime LastTimeSyncName
    {
        get => PlayerPrefs
            .GetString(StringHelper.LastTimeSyncName, DateTime.MinValue.ToString(CultureInfo.InvariantCulture))
            .ToDateTime();
        set
        {
            PlayerPrefs.SetString(StringHelper.LastTimeSyncName, value.ToString(CultureInfo.InvariantCulture));
            PlayerPrefs.Save();
        }
    }

    public static int Score // vi la test nen khong khai bao can than
    {
        get { return PlayerPrefs.GetInt("TestLeaderBroadScore111", 0); }
        set
        {
            PlayerPrefs.SetInt("TestLeaderBroadScore111", value);
            PlayerPrefs.Save();
        }

    }
    public static int Titkit  
    {
        get { return PlayerPrefs.GetInt("Titkit", 5); }
        set
        {
            PlayerPrefs.SetInt("Titkit", value);
            PlayerPrefs.Save();
        }

    }
    public static int WasClickPvP
    {
        get { return PlayerPrefs.GetInt("WasClickPvP", 0); }
        set
        {
            PlayerPrefs.SetInt("WasClickPvP", value);
            PlayerPrefs.Save();
        }

    }
    public static bool WasClickChangeName
    {
        get
        {
            return PlayerPrefs.GetInt("WasClickChangeName", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("WasClickChangeName", value ? 1 : 0);
            PlayerPrefs.Save();
        }

    }
    public static bool IsRemoveAds
    {
        get
        {
            return PlayerPrefs.GetInt("REMOVE_ADS", 0) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("REMOVE_ADS", value ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}
