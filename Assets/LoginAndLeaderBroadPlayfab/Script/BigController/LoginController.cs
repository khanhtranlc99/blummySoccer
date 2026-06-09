using System;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;
public enum TypeLogin
{
    FB = 0,
    GG = 1,
    GameCenter = 2
}

public class LoginController : MonoBehaviour
{
   
    public static bool IsConnected;
    private bool _isTryLogin;
    private Action _onLoginSuccess;
    private Action _onLoginFail;
    public static bool isSyncingData => GameController.Instance.playerData.isSyncData;


    public void Init ()
    {
        _isTryLogin = false;
        LoginPlayFab();
    }

    public void LoginPlayFab(Action onLoginSuccess = null, Action onError = null)
    {
        if (_isTryLogin)
        {
            _onLoginSuccess += onLoginSuccess;
            _onLoginFail += onError;
            return;
        }

        _onLoginSuccess += () => _isTryLogin = false;
        _onLoginFail += () => _isTryLogin = false;
        _isTryLogin = true;
        var loginRequest = new LoginWithCustomIDRequest
        {
            CustomId = GUtils.GetDeviceId(),
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(loginRequest, res => LoginSuccess(res, null, _onLoginSuccess, _onLoginFail),
            err =>
            {
                RegisterError(err);
                _onLoginFail?.Invoke();
            });
    }

    public void LoginPlayFab(string customID, Action onLoginSuccess = null, Action onError = null)
    {
        if (_isTryLogin)
        {
            _onLoginSuccess += onLoginSuccess;
            _onLoginFail += onError;
            return;
        }

        _onLoginSuccess += () => _isTryLogin = false;
        _onLoginFail += () => _isTryLogin = false;
        _isTryLogin = true;
        var loginRequest = new LoginWithCustomIDRequest
        {
            CustomId = customID,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(loginRequest, res => LoginSuccess(res, null, _onLoginSuccess, _onLoginFail),
            e => { _onLoginFail?.Invoke(); });
    }

    public void ReLoginPlayFab(Action onLoginSuccess = null, Action errorAction = null, Action onLoadConfigDone = null)
    {
        if (_isTryLogin)
        {
            _onLoginSuccess += onLoginSuccess;
            _onLoginFail += errorAction;
            return;
        }

        _onLoginSuccess += () => _isTryLogin = false;
        _onLoginFail += () => _isTryLogin = false;
        _isTryLogin = true;
        var loginRequest = new LoginWithCustomIDRequest
        {
            CustomId = GUtils.GetDeviceId(),
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(loginRequest,
            res => LoginSuccess(res, onLoadConfigDone, _onLoginSuccess, _onLoginFail),
            e => { _onLoginFail?.Invoke(); });
    }


    protected virtual void LoginSuccess(LoginResult result, Action onLoadConfigDone, Action onLoginSuccess,
       Action onError)
    {
        UseProfile.PlayFabId = result.PlayFabId;
        var linkedID = GUtils.GetDeviceId();
        if(!UseProfile.WasClickChangeName)
        {
            UseProfile.NamePlayer = "Player#" + GetLastFiveCharOfId(result.PlayFabId);
        }
     
        //if (string.IsNullOrEmpty(UseProfile.NamePlayer))
        //{
        //    UseProfile.NamePlayer = "Muscle#" + GetLastFiveCharOfId(result.PlayFabId);
        //}

        IsConnected = true;
       // UserSegmentConfig.LoadPlayFabConfig(() => { onLoadConfigDone?.Invoke(); }, onError);  có thể load config từ playfab ở đây

        if (UseProfile.FlagLink == "")
            Helper.GetCountryByIP();

        //Nếu lần này là đăng kí tài khoản
        if (result.NewlyCreated)
        {
            bool isRun = false;
            SaveName(UseProfile.NamePlayer.Insert(UseProfile.NamePlayer.Length - 5, Random.Range(0, 9).ToString()));
            // GameController.Instance.playerData.SaveData(OnDone, OnDone, true, true);
           // Debug.LogError("LoginOK- Lần đầu");
            OnDone();
            void OnDone()
            {
                if (isRun)
                    return;
                isRun = true;
                GameController.Instance.playerData.isSyncData = true;
                GameController.Instance.playerData.InitData();



                LoginSuccessHandle(onLoginSuccess);
            }
        }
        else
        {
        //    Debug.LogError("LoginOK- Đã đăng ký r");
            GameController.Instance.playerData.InitData();
       //     Debug.LogError(UseProfile.NamePlayer);
            //Đồng bộ data
            //if (!GameController.Instance.playerData.isSyncData)
            //{
            //    GConnection.GetData(result.PlayFabId, data =>
            //    {
            //        //if (UseProfile.LinkedId != "")
            //        //    UseProfile.IsLoggedIn = true;

            //        if (data != null && linkedID != data.linkedId)
            //        {
            //            IsConnected = false;
            //            UseProfile.LinkedId = data.linkedId;
            //            LoginPlayFab();
            //        }
            //        else
            //        {
            //            //Chưa từng đẩy data lên hoặc data mới
            //            if (data == null || PlayerData.IsLocalDataNew(data))
            //            {
            //                //Đẩy data mới lên
            //                GameController.Instance.playerData.SaveData(isFromDisk: true, isForce: true);
            //                GameController.Instance.playerData.isSyncData = true;
            //            }
            //            else
            //            {
            //                //Lấy data theo Server
            //                GameController.Instance.playerData.SetDataSync(data);
            //               // GameController.Instance.LoadScene(SceneName.GAME_PLAY); lấy được thì load lại scene
            //                GameController.Instance.playerData.isSyncData = true;
            //            }
            //        }
            //    }, e =>
            //    {

            //        Debug.LogError("Chưa đồng bộ");

            //    });
            //}


        }

        StartCoroutine(Helper.StartAction(() => LoginSuccessHandle(onLoginSuccess), () => isSyncingData));
    }

    private void LoginSuccessHandle(Action onLoginSuccess)
    {
        onLoginSuccess?.Invoke();
        if ((DateTime.Now - UseProfile.LastTimeSyncName).TotalDays >= 7)
        {
            UseProfile.LastTimeSyncName = DateTime.Now;
            SaveName();
            PlayFabClientAPI.UpdateAvatarUrl(new UpdateAvatarUrlRequest { ImageUrl = UseProfile.AvatarLink }, s => { },
                e => { });
        }
    }
    private void SaveName(string name = null, Action<PlayFabError> onFail = null)
    {
        if(UseProfile.WasClickChangeName)
        {
            return;
        }
        if (!string.IsNullOrEmpty(name))
        {
            UseProfile.NamePlayer = name;
        }

        PlayFabClientAPI.UpdateUserTitleDisplayName(
            new UpdateUserTitleDisplayNameRequest { DisplayName = UseProfile.NamePlayer }, s => { }, onFail);
    }
    private string GetLastFiveCharOfId(string param)
    {
        if (param.Length <= 0)
        {
            return "00000";
        }

        var a = "";

        for (var i = 1; i < 6; i++) a += param[param.Length - i];


        return a;
    }
    protected virtual void RegisterError(PlayFabError error)
    {
        var textError = error.GenerateErrorReport();
        Debug.LogWarning(textError);
    }
    public bool IsLoginPlayfab()
    {
        return IsConnected;
    }
}
