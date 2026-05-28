using DG.Tweening;
// using Sirenix.OdinInspector;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public enum PUSH_NOTIFICATION_TYPE
{
    ADS_PASS = 1,
    AUTO_REWARD = 2,
    BAG = 3,
    EMAIL = 4,
    QUEST = 5,
    SHOP = 6,
    RANK = 7,
    CAMPAIGN = 8,
    MERGE = 9,
    EGGHATCH = 10,
    LIBRARY = 11,
    UPGRADE_MONSTER = 12
}
public class pfb_VFXUI : UIBehavior
{
    [SerializeField] protected Button btnBag;
    public MenuAnimPanel AnimPanel;
    [SerializeField] protected Transform CoinContainerPos;
    protected Tween TempTW;
    #region DO ANIM
    public void OnGetReward(Vector3 start, RewardStruct RewardStruct)
    {
        Camera main = Camera.main;
        Vector2 localPoint = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(AnimPanel.transform as RectTransform, RectTransformUtility.WorldToScreenPoint(main, GetTargetTransByRewardType(RewardStruct.REWARD_TYPE).position), main, out localPoint);
        Vector2 localPoint2 = Vector2.zero;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(AnimPanel.transform as RectTransform, RectTransformUtility.WorldToScreenPoint(main, start), main, out localPoint2);
        if (RewardStruct.numberReward <= 10)
            VFXManager.Instance.actionQueue.PushNode(AnimPanel.GetFlyActionNode(localPoint, localPoint2, RewardStruct, GetTargetTransByRewardType(RewardStruct.REWARD_TYPE)));
        else
            AnimPanel.DoAnimGetReward(start, GetTargetTransByRewardType(RewardStruct.REWARD_TYPE), RewardStruct);
    }
    protected Vector3 GetExtraPositionByIndex(int index)
    {
        float multiValue = 1.5f;
        if (index == 1) return Vector2.zero;
        if (index == 2) return new Vector2(UnityEngine.Random.Range(-1, 1) * multiValue, UnityEngine.Random.Range(0, 1) * multiValue);
        else if (index == 3) return new Vector2(UnityEngine.Random.Range(-1, 1) * multiValue, UnityEngine.Random.Range(0, -1) * multiValue);

        return Vector2.zero;
    }
    protected Transform GetTargetTransByRewardType(REWARD_TYPE REWARD_TYPE)
    {
        switch (REWARD_TYPE)
        {
            case REWARD_TYPE.GOLD: return this.CoinContainerPos;
            default: return this.btnBag.transform;
        }
    }
    List<Tween> ListTweenDelayReset = new List<Tween>();
    protected List<Transform> ListTargetAnim = new List<Transform>();
    public void ResetAfterDoAnim(Transform targetTransf)
    {
        int indexTw = 0;
        if (this.ListTargetAnim.Contains(targetTransf)) //Trường hợp có 1 anim khác cũng bắn vào trùng với target cũ thì kill Tween cũ và lấy index của target cũ
        {
            indexTw = this.ListTargetAnim.IndexOf(targetTransf);
            this.ListTweenDelayReset[indexTw].Kill();
        }
        else
        {
            this.ListTargetAnim.Add(targetTransf);
            indexTw = this.ListTargetAnim.Count - 1;
        }
        targetTransf.GetComponent<Canvas>().sortingOrder = 30;
        //Get A tween empty
        Tween suitableTween = null;
        if (this.ListTweenDelayReset.Count < indexTw + 1 || this.ListTweenDelayReset.Count == 0)
        {
            int dif = Mathf.Abs(this.ListTweenDelayReset.Count - indexTw - 1);
            //add new tween
            for (global::System.Int32 i = 0; i < dif; i++)
            {
                Tween newTween = Utils.DelayCallForUpdate(0f, delegate { });
                this.ListTweenDelayReset.Add(newTween);
            }
        }
        suitableTween = this.ListTweenDelayReset[indexTw];
        suitableTween.Kill();
        suitableTween = Utils.DelayCallForUpdate(.5f, delegate
        {
            if (!CanvasComponent.blocksRaycasts)
                SetAlpha(0f);
            Canvas PriorityCanvas = targetTransf.GetComponent<Canvas>();
            GraphicRaycaster Raycaster = targetTransf.GetComponent<GraphicRaycaster>();
            if (PriorityCanvas != null)
                PriorityCanvas.sortingOrder = 0;
            if (Raycaster != null)
                Raycaster.enabled = true;
            this.ListTargetAnim.Remove(targetTransf);
        });
    }
    #endregion
}
