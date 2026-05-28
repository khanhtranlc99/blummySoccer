using System;
using EnhancedUI.EnhancedScroller;
using Newtonsoft.Json;
using PlayFab;
using PlayFab.ClientModels;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class LaybelInfoPlayer : EnhancedScrollerCellView
{
    public TypeLableInfoPlayer laybelInfo;
    public string idLabel;
    public Text tvRank;
    public RawImage thumnail;
    public RawImage flagImg;
    //public Image frames;
    public Text tvName;
    public Text tvScore;
    public Image bgLable;
    //public Sprite normalBG;
    //public Sprite brownBG;
    [SerializeField] private GameObject loadingObj;
    protected IDisposable _downloadAvatarDispose;
    protected IDisposable _downloadFlagDispose;
    public PlayerData playerData;
    private string playFabId;

    public Image bgLableNew;
    public Image headlabelNew;

    public Sprite yellow;
    public Sprite blue;
    public Sprite normal;

    private void OnDisable()
    {
        if (_downloadAvatarDispose != null)
            _downloadAvatarDispose.Dispose();
        if (_downloadFlagDispose != null)
            _downloadFlagDispose.Dispose();
    }

    public void Init()
    {
    }

    public void InitState(RankingItemData data)
    {
        // GetOtherUserData(data.playfabID);
        SetInfoRank(data.rank, data.score);

        if (_downloadAvatarDispose != null)
            _downloadAvatarDispose.Dispose();
        if (_downloadFlagDispose != null)
            _downloadFlagDispose.Dispose();


        var playerData = new PlayerData();
        playerData.namePlayer = data.name;
        playerData.linkAvatar = data.linkAvatar;
        playerData.flagLink = string.Format(StringHelper.FLAG_API, data.contryCode);
        SetUpInfoPlayer(playerData);
    }

    public void InitStateProfileMe(string leadBroadName)
    {
        _downloadAvatarDispose?.Dispose();
        _downloadFlagDispose?.Dispose();

        loadingObj.gameObject.SetActive(true);
        GConnection.GetLeaderBoardAroundPlayer3(leadBroadName, data =>
        {
            //Chờ bảng XH tổng nếu vị trí nhỏ hơn 100
            Debug.Log(data.ToJson());
            var score = data.StatValue;
            if (data.Position<=100)
            {
                //Đợi đến khi bảng XH tổng get thành công
                StartCoroutine(Helper.StartAction(() =>
                {
                    var realRank = LaybelLeaderBoard.lsSmartList.Find((itemData => itemData.playfabID == UseProfile.PlayFabId));
                    if (realRank != null)
                    {
                        Debug.Log("Tìm thấy vị trí chính xác");
                        SetInfoRank(realRank.rank,score);
                        SetUpInfoPlayer(GameController.Instance.playerData);
                        loadingObj.gameObject.SetActive(false);
                        return;
                    }
                    Debug.Log("Không tìm thấy vị trí chính xác vị trí là 101");
                    SetInfoRank(100, score);
                    SetUpInfoPlayer(GameController.Instance.playerData);
                    loadingObj.gameObject.SetActive(false);
                }, () => !LaybelLeaderBoard.isRequesting));
                return;
            }
            //Khi vị trí lớn hơn hẳn 100 Init luôn.
            SetInfoRank(data.Position, score);
            SetUpInfoPlayer(GameController.Instance.playerData);
            loadingObj.gameObject.SetActive(false);
        });
    }


    //private void GetOtherUserData(string playFabId)
    //{
    //    this.playFabId = playFabId;
    //    _downloadAvatarDispose?.Dispose();
    //    _downloadFlagDispose?.Dispose();

    //    PlayFabClientAPI.GetUserData(new GetUserDataRequest { PlayFabId = playFabId }, OnGetOtherUserDataSuccess,
    //        noOk => { Debug.LogError("noOk"); });
    //}

    //private void OnGetOtherUserDataSuccess(GetUserDataResult result)
    //{
    //    if (playFabId != result.Data[DataPlayFab.PLAYFAB_ID].Value)
    //        return;
    //    var jsonDataNormalPlayer = result.Data["DATA_NORMAL_PLAYER"].Value;
    //    var Data = JsonConvert.DeserializeObject<PlayerData>(jsonDataNormalPlayer);
    //    SetUpInfoPlayer(Data);
    //}

    public void SetUpInfoPlayer(PlayerData playerData)
    {
        tvName.text = playerData.namePlayer;
        if(laybelInfo == TypeLableInfoPlayer.Player)
        {
            //LeaderBoardPvPBox.Instance.tvName.text = playerData.namePlayer;
        }
        //var a = (Texture)Resources.Load("UI/Sprite/Avatars/Avatar0", typeof(Texture));
        //thumnail.texture = a;

        if (!string.IsNullOrEmpty(playerData.linkAvatar))
            _downloadAvatarDispose = Context.DownloadOrCache(playerData.linkAvatar)
                .CatchIgnore()
                .Subscribe(www => thumnail.texture = www);

        if (!string.IsNullOrEmpty(playerData.flagLink))
            _downloadFlagDispose = Context.DownloadOrCache(playerData.flagLink, false)
                .CatchIgnore()
                .Subscribe(www => flagImg.texture = www);
    }

    private void SetInfoRank(int rank, int score)
    {
        tvRank.text = "" + (rank + 1);
        tvScore.text = "" + score;
        if (laybelInfo == TypeLableInfoPlayer.Player)
        {
            LeaderBoardPvPBox.Instance.tvScore.text = "" + score;
            GConnection.scoreUser = score;
        }
        // frames.sprite = GameController.Instance.rankController.GetRankData(score).frames;
        //if (laybelInfo != TypeLableInfoPlayer.Player)
        //{
        //    if (rank % 2 == 0)
        //        bgLable.sprite = brownBG;
        //    else
        //        bgLable.sprite = normalBG;
        //}


        SetUpLaybel(rank);
    }

    private void SetUpLaybel(int param)
    {
        if (param <= 2)
        {
         
            if (param + 1 == 1)
            {
              
                bgLableNew.sprite = yellow;
                //headlabelNew.color = new Color32(255, 223, 17, 255);
            }
            else
            {
            
                bgLableNew.sprite = blue;
               // headlabelNew.color = new Color32(86, 197, 255, 255);
            }

           // tvRank.color = new Color(1, 248f / 255, 0, 1);
        }
        else
        {
            //  tvRank.color = new Color(1, 1, 1, 1);
            bgLableNew.sprite = normal;
            // bgLableNew.color = new Color32(0, 0, 0, 0);
            //  headlabelNew.color = new Color32(228, 110, 40, 255);
            if (laybelInfo == TypeLableInfoPlayer.Player)
            {
         //       headlabelNew.color = new Color32(255, 173, 17, 255);
            }
        }
    }
}

public enum TypeLableInfoPlayer
{
    OtherPlayer,
    Player
}