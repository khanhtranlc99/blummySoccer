using System.Collections.Generic;
using EnhancedUI;
using EnhancedUI.EnhancedScroller;
using UnityEngine;

public class RankingItemData
{
    public string contryCode;
    public string linkAvatar;
    public string name;
    public string playfabID;
    public int rank;
    public int score;
}

public class LaybelLeaderBoard : MonoBehaviour, IEnhancedScrollerDelegate
{
    private const int MAX_RESULTS_COUNT = 100;
    [SerializeField] private List<LaybelInfoPlayer> lsLaybelInfoPlayer;
    [SerializeField] private LaybelInfoPlayer prefapsLaybelInfo;
    [SerializeField] private LaybelInfoPlayer playerInfo;
    [SerializeField] private GameObject parent;
    public EnhancedScroller scroller;
    [SerializeField] private GameObject loadingObj;
    private int _totalStartPosition;
    public static bool isRequesting; //Cần public để Label của bản thân đọc được là có request thành công hay không
    public static List<RankingItemData> lsSmartList;

    public EnhancedScrollerCellView GetCellView(EnhancedScroller scroller, int dataIndex, int cellIndex)
    {
        var cellView = scroller.GetCellView(prefapsLaybelInfo) as LaybelInfoPlayer;
        cellView.InitState(lsSmartList[dataIndex]);
        return cellView;
    }

    public float GetCellViewSize(EnhancedScroller scroller, int dataIndex)
    {
        return 150;
    }

    public int GetNumberOfCells(EnhancedScroller scroller)
    {
        return lsSmartList.Count;
    }

    public void Init()
    {
        scroller.Delegate = this;
        lsSmartList = new List<RankingItemData>();
    }

    public void InitState()
    {
        var Container = scroller.transform.Find("Container");
        if (Container != null)
        {
            Container.gameObject.transform.localPosition = Vector3.zero;
        }
        Debug.LogError(Container.gameObject.name);

        loadingObj.gameObject.SetActive(true);
        lsSmartList.Clear();
        scroller.ReloadData();
        GetLeaderBroad();
        _totalStartPosition = 0;
    }

    private void GetLeaderBroad(int tryCount = 0)
    {
        if (tryCount > 10)
        {
            Debug.LogError("Can't Get Leaderboard after 10 times try, abort...");
            isRequesting = false;
            return;
        }
        isRequesting = true;
        GConnection.GetLeaderBoard("LeaderboardName", 0, MAX_RESULTS_COUNT, GetLeaderBroadSuccess,
            e =>
            {
                //Retry
                Debug.LogError($"Retry Get Leaderboard ({tryCount++})");
                GetLeaderBroad(tryCount);
            });
    }

    private void GetLeaderBroadSuccess(List<PlayFabLeaderboardMember> lstResult)
    {
        Debug.Log("Get Leaderboard Success");
        var startIndex = scroller.NumberOfCells > 0
            ? scroller.StartDataIndex
            : 0;
        //var startIndex =  scroller.StartDataIndex;
        for (var i = 0; i < lstResult.Count; i++)
        {
            var dataElement = new RankingItemData();
            dataElement.playfabID = lstResult[i].id;
            dataElement.name = lstResult[i].name;
            dataElement.score = int.Parse(lstResult[i].statistic);
            dataElement.rank = lstResult[i].position;
            dataElement.linkAvatar = lstResult[i].linkAvatar;
            dataElement.contryCode = lstResult[i].countryCode;

            lsSmartList.Add(dataElement);
        }
        isRequesting = false;

        scroller.ReloadData();

        if (lstResult.Count >= MAX_RESULTS_COUNT)
        {
            _totalStartPosition += lstResult.Count;
            scroller.JumpToDataIndex(startIndex);
            scroller.cellViewVisibilityChanged = ScrollerCellViewVisibilityChanged;
        }
        else
        {
            scroller.cellViewVisibilityChanged = null;
            scroller.JumpToDataIndex(lsSmartList.Count - lstResult.Count - 1);
            // scroller.ReloadData();
        }

        loadingObj.gameObject.SetActive(false);
        // scroller.ReloadData();
    }

    private void ScrollerCellViewVisibilityChanged(EnhancedScrollerCellView view)
    {
        if (isRequesting)
            return;
    }

    public void GetData()
    {
    }
}