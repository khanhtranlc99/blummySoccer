using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using UnityEngine.Events;
using Debug = UnityEngine.Debug;

// ReSharper disable ConditionIsAlwaysTrueOrFalse

// ReSharper disable AccessToModifiedClosure

public class GConnection : MonoBehaviour
{
    #region Variable

    public const float MinWaitingTime = 0.8f;
    public static List<PlayFabLeaderboardMember> partLeaderBoard;
    public static List<PlayFabLeaderboardMember> leaderBoardNearPlayer;
    public static List<PlayerData> lsPlayerData = new List<PlayerData>();
    public static PlayerData otherPlayer;
    public static int scoreUser;

    public static PlayFabError PlayFabTimeout => new PlayFabError()
    {
        Error = PlayFabErrorCode.ConnectionError
    };

    #endregion

    #region Update Data

    #region Post

    private static UnityAction<UpdateUserDataResult> actionUpdateDataSuccess;

    [Obsolete("Sử dụng UpdateDatatable", false)]
    public static void UpdateData(string nameData, string jsonValue,
        UnityAction<UpdateUserDataResult> actionUpdateDataSuccess, UnityAction errorAction = null, bool isForce = false)
    {
        if (!GameController.Instance.playerData.isSyncData || isForce)
        {
#if LOG_ERROR
            Debug.LogError("Chưa Sync Lần đầu");
#endif
            errorAction?.Invoke();
            return;
        }

        GConnection.actionUpdateDataSuccess = actionUpdateDataSuccess;

        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { nameData, jsonValue }
            },
            Permission = UserDataPermission.Public //cai nay khong public la khong GetData ve duoc a
        };
        StackTrace tracer = new StackTrace();
        PlayFabClientAPI.UpdateUserData(request, OnUpdateDataSuccess, e =>
        {
            OnUpdateDataError(e);
            Debug.LogError("Update Data Error: " + e);
          
            errorAction?.Invoke();
        });
    }

    [Obsolete("Sử dụng UpdateDatatable", false)]
    public static void UpdateDataQuick(string nameData, string jsonValue,
        UnityAction<UpdateUserDataResult> actionUpdateDataSuccess, UnityAction errorAction = null)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { nameData, jsonValue }
            },
            Permission = UserDataPermission.Public //cai nay khong public la khong GetData ve duoc a
        };

        PlayFabClientAPI.UpdateUserData(request, (s) => { actionUpdateDataSuccess?.Invoke(s); }, e =>
        {
            Debug.LogError("Update Data Error: " + e);
            errorAction?.Invoke();
        });
    }

    public static void OnUpdateDataSuccess(UpdateUserDataResult result)
    {
        actionUpdateDataSuccess?.Invoke(result);
    }

    public static void OnUpdateDataError(PlayFabError result)
    {
    }

  

    public static void UpdateScoreToALeaderBroad(string nameTable,int param, Action onDone = null)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = nameTable,
                    Value = param
                }
            }
        };
        if (!GameController.Instance.loginController.IsLoginPlayfab())
        {
            GameController.Instance.loginController.ReLoginPlayFab((() =>
            {
                PlayFabClientAPI.UpdatePlayerStatistics(request, win =>
                    {
                        onDone?.Invoke();
                        Debug.Log("Sync Data Success");
                        //UseProfile.ScorePvP = param;
                       // GameController.Instance.playerData.SaveData();
                    },
                    error => { Debug.LogError(error + " error"); });
            }));
            return;
        }

        PlayFabClientAPI.UpdatePlayerStatistics(request, win =>
            {
                onDone?.Invoke();
                
                //UseProfile.ScorePvP = param;
              //  GameController.Instance.playerData.SaveData();
            },
            error => { Debug.LogError(error + "error"); });
    }

    /// <summary>
    /// Đồng bộ dữ liệu theo bảng đang chọn
    /// </summary>
    /// <param name="table">Gói dữ liệu muốn đồng bộ</param>
    /// <param name="onSuccess">Action khi thành công</param>
    /// <param name="onFail">Action khi thất bại</param>
    /// <param name="timeout">Thời gian timeout. Mặc định là 5s</param>
    /// <param name="isForce"></param>
    /// <param name="isMinimise"></param>
    public static void UpdateDataTable(GDataTable table, Action<UpdateUserDataResult> onSuccess = null,
        Action<PlayFabError> onFail = null, float timeout = 5f, bool isForce = false,
        bool isMinimise = false, Action onDone = null)
    {
        GetTableData(table, out string sTable, out BaseData bData);

        Debug.Log($"Table: {table}\n" + "Link: " + sTable);

        onFail = Debug.LogError + onFail;

        onFail = (_ => onDone?.Invoke()) + onFail;
        onSuccess = (_ => onDone?.Invoke()) + onSuccess;
        bool isTimeout = false;
        bool isRunErrorCallBack = false;
        var tracer = Environment.StackTrace;
        //var box = WaitingBox.Setup();
        //box.ShowWaiting(timeout, () =>
        //{
        //    isTimeout = true;
        //    ErrorCallback(PlayFabTimeout);
        //}, isMinimise: isMinimise);


        //Nếu chưa từng sync lần đầu
        if (!isForce && !bData.isSyncData)
        {
            GameController.Instance.loginController.ReLoginPlayFab(SendRequest, () => ErrorCallback(PlayFabTimeout));

            Debug.LogError("Not Sync.\n" + tracer);

            return;
        }

        void ResultCallback(UpdateUserDataResult res)
        {
            //Nếu đã timeout -> Vẫn tính là thất bại
            if (isTimeout)
            {
                ErrorCallback(PlayFabTimeout);
                return;
            }

            bData.SetLocalData();
#if LOG_VERBOSE
            Debug.Log($"Success".Color(Color.green));
            Debug.Log(tracer);
#endif
            onSuccess?.Invoke(res);
            //box.HideWaiting();
        }

        void ErrorCallback(PlayFabError e)
        {
            if (isRunErrorCallBack)
                return;
            bData.InitData();
#if LOG_ERROR
            Debug.LogError("Sync Data Fail: \n".Color(ColorDefine.HoiDo) + tracer);
#endif
            onFail?.Invoke(e);
            //box.HideWaiting();
            isRunErrorCallBack = true;
        }

        void SendRequest()
        {
            //Tạo request
            string jsonValue = bData.ToJson();
#if LOG_VERBOSE
            Debug.Log("PUSH DATA: " + jsonValue);
#endif
            var req = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { sTable, jsonValue }
                },
                Permission = UserDataPermission.Public
            };
            //Đã timeout rồi mới đăng nhập thành công -> Vẫn tính là fail
            if (isTimeout)
            {
                ErrorCallback(PlayFabTimeout);
                return;
            }

            PlayFabClientAPI.UpdateUserData(req, ResultCallback, ErrorCallback);
        }

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SendRequest();
        }
        else
        {
            GameController.Instance.loginController.ReLoginPlayFab(SendRequest, () => ErrorCallback(PlayFabTimeout));
        }
    }

    public static void UpdateDataTableBackground(GDataTable table, Action<UpdateUserDataResult> onSuccess = null,
        Action<PlayFabError> onFail = null, float timeout = 5f, bool isForce = false,
        bool isFromDisk = false)
    {
        GetTableData(table, out string sTable, out BaseData bData);

        Debug.Log($"Table: {table}\n" + "Link: " + sTable);

        onFail = Debug.LogError + onFail;

        if (isFromDisk) bData.InitData();
        bool isTimeout = false;
        bool isRunErrorCallBack = false;
        var tracer = Environment.StackTrace;
        IDisposable waitDispose = null;
        waitDispose = Observable.Timer(TimeSpan.FromSeconds(timeout), Scheduler.MainThreadIgnoreTimeScale).Subscribe(
            (data) =>
            {
                isTimeout = true;
                ErrorCallback(PlayFabTimeout);
            });

        //Nếu chưa từng sync lần đầu
        if (!isForce && !bData.isSyncData)
        {
            GameController.Instance.loginController.ReLoginPlayFab(SendRequest, () => ErrorCallback(PlayFabTimeout));

            Debug.LogError("Not Sync First Time");

            return;
        }

        void ResultCallback(UpdateUserDataResult res)
        {
            //Nếu đã timeout -> Vẫn tính là thất bại
            if (isTimeout)
            {
                ErrorCallback(PlayFabTimeout);
                return;
            }

            waitDispose?.Dispose();
            bData.SetLocalData();

            Debug.Log($"Success");
            Debug.Log(tracer);

            onSuccess?.Invoke(res);
        }

        void ErrorCallback(PlayFabError e)
        {
            if (isRunErrorCallBack)
                return;
            waitDispose?.Dispose();
            bData.InitData();

            Debug.LogError("Sync Data Fail: \n" + tracer);

            onFail?.Invoke(e);
            isRunErrorCallBack = true;
        }

        void SendRequest()
        {
            //Tạo request
            string jsonValue = bData.ToJson();
            var req = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { sTable, jsonValue }
                },
                Permission = UserDataPermission.Public,
            };
            //Đã timeout rồi mới đăng nhập thành công -> Vẫn tính là fail
            if (isTimeout)
            {
                ErrorCallback(PlayFabTimeout);
                return;
            }

            PlayFabClientAPI.UpdateUserData(req, ResultCallback, ErrorCallback);
        }

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SendRequest();
        }
        else
        {
            GameController.Instance.loginController.ReLoginPlayFab(SendRequest, () => ErrorCallback(PlayFabTimeout));
        }
    }

    public static void UpdateDataTable(GDataTable table, Action updater, Action<UpdateUserDataResult> onSuccess = null,
        Action<PlayFabError> onFail = null, float timeout = 5f, bool isForce = false,
        bool isMinimise = false, Action onDone = null, float? minTime = null)
    {
        GetTableData(table, out string sTable, out BaseData bData);

        Debug.Log($"Table: {table}\n" + "Link: " + sTable);

        onFail = Debug.LogError + onFail;
     onFail = (_ => onDone?.Invoke()) + onFail;
        onSuccess = (_ => onDone?.Invoke()) + onSuccess;
        bool isTimeout = false;
        bool isRunErrorCallBack = false;
        var tracer = Environment.StackTrace;
        //var box = WaitingBox.Setup();
        bool isMinTimeDone = minTime == null;
        bool isRequestDone = false;
        IDisposable minTimeTimer = default;
        UpdateUserDataResult rt = default;
        //Init BackupData và thực hiện thay đổi data
        string backupJson = bData.ToJson();
        updater?.Invoke();
        bData.InitData();

        DateTime startTime = DateTime.Now;

        if (minTime != null)
        {
            minTimeTimer = Observable.Timer(TimeSpan.FromSeconds(minTime.Value), Scheduler.MainThreadIgnoreTimeScale)
                .Subscribe(_ =>
                {
                    isMinTimeDone = true;

                    Debug.Log("MinTimer Done");
                    CloseWaitingBox();

                });
        }

        //Nếu chưa từng sync lần đầu
        if (!isForce && !bData.isSyncData)
        {
            GameController.Instance.loginController.ReLoginPlayFab(SendRequest, () => ErrorCallback(PlayFabTimeout));

            Debug.LogError("Not Sync.\n" + tracer);

            return;
        }

        //box.ShowWaiting(timeout, () =>
        //{
        //    isTimeout = true;
        //    ErrorCallback(PlayFabTimeout);
        //}, isMinimise: isMinimise);


        void CloseWaitingBox()
        {
            if (isMinTimeDone && isRequestDone) ResultCallback();
        }

        void ResultCallback()
        {
            //Nếu đã timeout -> Vẫn tính là thất bại
            if (isTimeout)
            {
                ErrorCallback(PlayFabTimeout);
                return;
            }

        //    box.HideWaiting();
            bData.SetLocalData();

            Debug.Log($"Success" + "\n" + (DateTime.Now - startTime).TotalSeconds);
            Debug.Log(tracer);


            onSuccess?.Invoke(rt);
        }

        void ErrorCallback(PlayFabError e)
        {
            if (isRunErrorCallBack)
                return;
            bData.SetData(backupJson);

            Debug.LogError("Sync Data Fail: " + "\n" +
                           (DateTime.Now - startTime).TotalSeconds + "\n" + tracer);

            isMinTimeDone = true;
            isRequestDone = true;
            minTimeTimer?.Dispose();
            //box.ForceHideWaiting();
            onFail?.Invoke(e);
            isRunErrorCallBack = true;
        }

        void SendRequest()
        {
            //Tạo request
            string jsonValue = bData.ToJson();

            Debug.Log("PUSH DATA: " + jsonValue);

            var req = new UpdateUserDataRequest
            {
                Data = new Dictionary<string, string>
                {
                    { sTable, jsonValue }
                },
                Permission = UserDataPermission.Public
            };
            //Đã timeout rồi mới đăng nhập thành công -> Vẫn tính là fail
            if (isTimeout)
            {
                ErrorCallback(PlayFabTimeout);
                return;
            }

            PlayFabClientAPI.UpdateUserData(req, res =>
            {
                rt = res;
                isRequestDone = true;
                CloseWaitingBox();
            }, ErrorCallback);
        }

        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            SendRequest();
        }
        else
        {
            GameController.Instance.loginController.ReLoginPlayFab(SendRequest, () => ErrorCallback(PlayFabTimeout));
        }
    }

    #endregion

    #region Get
    public static void GetTime()
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "getServerTime",
        }, result => {
            Debug.Log("Server Time: " + result.FunctionResult.ToString());
        }, error => {
            Debug.LogError(error.GenerateErrorReport());
        });
    }    


    private static UnityAction actionGetDataSuccess;

    public static void GetData(string playFabId, UnityAction actionGetDataSuccess, Action OnFail = null)
    {
        GConnection.actionGetDataSuccess = actionGetDataSuccess;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playFabId }, OnGetDataSuccess,
            (e) =>
            {
                Debug.LogError("Playfab fail: " + e.ErrorMessage);
                OnFail?.Invoke();
            });
    }

    public static void GetData(string playFabId, UnityAction<PlayerData> actionGetDataSuccess,
        Action<PlayFabError> errorAction)
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playFabId }, result =>
        {
            if (result.Data.ContainsKey(DataPlayFab.PLAYER_DATA))
            {
                var jsonDataPlayer = result.Data[DataPlayFab.PLAYER_DATA].Value;
                if (!string.IsNullOrEmpty(jsonDataPlayer))
                {
                    var data = JsonUtility.FromJson<PlayerData>(jsonDataPlayer);
                
                    actionGetDataSuccess(data);
                }
            }
            else
            {
                actionGetDataSuccess(null);
            }
        }, errorAction);
    }



    public static void GetOtherUserData(string playFabId, UnityAction actionGetDataSuccess)
    {
        GConnection.actionGetDataSuccess = actionGetDataSuccess;
        PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playFabId }, OnGetOtherUserDataSuccess,
            noOk => { });
    }

    private static void OnGetDataSuccess(GetUserDataResult result)
    {
        ///result.Data["Data Level"];
        if (result.Data.ContainsKey(DataPlayFab.PLAYER_DATA))
        {
            var jsonDataPlayer = result.Data[DataPlayFab.PLAYER_DATA].Value;
            if (jsonDataPlayer != null && jsonDataPlayer != "")
                GameController.Instance.playerData.SetData(jsonDataPlayer);
        }

        actionGetDataSuccess?.Invoke();
    }

    private static void OnGetOtherUserDataSuccess(GetUserDataResult result)
    {
        var jsonDataNormalPlayer = result.Data["DATA_NORMAL_PLAYER"].Value;
    }



    public static void GetLeaderBoard(string statisticName, int startPosition, int maxResult,
        Action<List<PlayFabLeaderboardMember>> actionSuccess, Action<PlayFabError> actionFailure)
    {
        partLeaderBoard = new List<PlayFabLeaderboardMember>();
        var lstResult = new List<PlayFabLeaderboardMember>();
        var request = new GetLeaderboardRequest
        {
            StatisticName = statisticName,
            StartPosition = startPosition,
            MaxResultsCount = maxResult,
            ProfileConstraints = new PlayerProfileViewConstraints
                { ShowAvatarUrl = true, ShowDisplayName = true, ShowLocations = true },
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGetSuccess, OnLeaderboardGetFailure);

        void OnLeaderboardGetSuccess(GetLeaderboardResult result)
        {
            foreach (var record in result.Leaderboard)
            {
                var player = new PlayFabLeaderboardMember(record);
                lstResult.Add(player);
            }
            partLeaderBoard = lstResult;
            actionSuccess?.Invoke(lstResult);
        }
        void OnLeaderboardGetFailure(PlayFabError error)
        {
            actionFailure?.Invoke(error);
        }
    }
    public static void GetLeaderBoardAroundPlayer(string statisticName, int maxResult, Action callback)
    {
        leaderBoardNearPlayer = new List<PlayFabLeaderboardMember>();
        var lstResult = new List<PlayFabLeaderboardMember>();
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = statisticName,
            MaxResultsCount = maxResult,
            PlayFabId = UseProfile.PlayFabId
        };

        void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
        {
            foreach (var item in result.Leaderboard)
            {
                var itm = new PlayFabLeaderboardMember(item);
                lstResult.Add(itm);
            }

            leaderBoardNearPlayer = lstResult;
            callback?.Invoke();
        }

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnGetDataError);
    }

    public static void GetLeaderBoardAroundPlayer2(string statisticName, int maxResult,
        Action<List<PlayFabLeaderboardMember>> callback)
    {
        var lstResult = new List<PlayFabLeaderboardMember>();
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = statisticName,
            MaxResultsCount = maxResult,
            PlayFabId = UseProfile.PlayFabId
        };

        void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
        {
            foreach (var item in result.Leaderboard)
            {
                var itm = new PlayFabLeaderboardMember(item);
                lstResult.Add(itm);
            }

            callback?.Invoke(lstResult);
        }

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnGetDataError);
    }

    //LA Get rồi so sánh
    public static void GetLeaderBoardAroundPlayer3(string statisticName, Action<PlayerLeaderboardEntry> callback)
    {
        var request = new GetLeaderboardAroundPlayerRequest
        {
            StatisticName = statisticName,
            MaxResultsCount = 5,
            PlayFabId = UseProfile.PlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
                { ShowAvatarUrl = true, ShowDisplayName = true, ShowLocations = true }
        };

        void OnLeaderboardAroundPlayerGet(GetLeaderboardAroundPlayerResult result)
        {
            var rt = result.Leaderboard.Find((entry => entry.PlayFabId == UseProfile.PlayFabId));
            callback?.Invoke(rt);
        }

        PlayFabClientAPI.GetLeaderboardAroundPlayer(request, OnLeaderboardAroundPlayerGet, OnGetDataError);
    }


    public static void OnGetDataError(PlayFabError result)
    {
        Debug.LogError("Leaderboard Get Fail: " + result.Error);
    }

  

    #endregion

    #endregion


    #region DefineEnum

    private static void GetTableData(GDataTable table, out string tableString, out BaseData classBase)
    {
        tableString = default;
        classBase = default;
        switch (table)
        {
            case GDataTable.PlayerData:
                tableString = DataPlayFab.PLAYER_DATA;
                classBase = GameController.Instance.playerData;
                return;
         
        }
    }

    #endregion
}

public enum GDataTable
{
    PlayerData = 1,
    HomeDecorPlayerData = 2,
}

#region PlayFabLeaderboardMember

[Serializable]
public class PlayFabLeaderboardMember
{
    public int position;
    public string name;
    public string statistic;
    public string id;
    public string linkAvatar;
    public string countryCode;

    public PlayFabLeaderboardMember(PlayerLeaderboardEntry entry)
    {
        position = entry.Position;
        name = entry.DisplayName;
        statistic = entry.StatValue.ToString();
        id = entry.PlayFabId;

        if (entry.Profile != null)
        {
            linkAvatar = entry.Profile.AvatarUrl;
            if (entry.Profile.Locations != null && entry.Profile.Locations.Count > 0)
                countryCode = entry.Profile.Locations[0].CountryCode.ToString();
        }
    }

    public PlayFabLeaderboardMember()
    {
        position = 0;
        name = "";
        statistic = "";
        id = "";
    }
}

#endregion