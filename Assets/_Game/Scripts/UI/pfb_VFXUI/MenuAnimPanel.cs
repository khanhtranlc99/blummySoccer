using DG.Tweening;
using DG.Tweening.Plugins.Core.PathCore;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MenuAnimPanel : MonoBehaviour
{
    #region ANIM ACTION 01
    public AnimationCurve AnimCurve;
    public AnimationCurve AnimCurveMove;
    private static Vector3[] disperseOffset = new Vector3[]
    {
        new Vector3(53f, 53f),
        new Vector3(-50f, 40f),
        new Vector3(45f, -45f),
        new Vector3(-30f, -40f)
    };
    public static Vector3[] ObjectSize = new Vector3[]
    {
        new Vector3(-1.3f, 1.3f, 1f),
        new Vector3(-1.1f, 1.1f, 1f),
        new Vector3(-0.9f, 0.9f, 1f),
        new Vector3(-1f, 1f, 1f)
    };
    public static float[] flyMove2PlayerItemDelay = new float[]
    {
        0.1f,
        0.3f,
        0.25f,
        0.4f
    };
    [SerializeField] protected FlyActionNode FlyActionNode;
    public float flyDiamondDisperseTime = .2f;
    public float flyDiamondMoveSpeed = 1000;
    public GameObjectPool FlyActionNodePool;
    private void Awake()
    {
        this.FlyActionNodePool = new GameObjectPool(FlyActionNode.gameObject, base.transform);
    }
    public ActionQueue.Node GetFlyActionNode(Vector3 targetPos, Vector3 startPos, RewardStruct RewardStruct, Transform targetTransf)
    {
        ActionQueue.Node node = new ActionQueue.Node();
        //active priority layer
        ActionQueue.Node nodeActive = new ActionQueue.Node();
        nodeActive.PushAction(new Func<float, bool>(delegate (float value)
        {
            UIManager.Instance.pfb_VFXUI.SetAlpha(1);
            targetTransf.GetComponent<Canvas>().sortingOrder = 30;
            GraphicRaycaster Raycaster = targetTransf.GetComponent<GraphicRaycaster>();
            if (Raycaster != null)
                Raycaster.enabled = false;
            return true;
        }));
        //Iupdate
        IUpdateUIProperty IUpdateUIProperty = targetTransf.GetComponent<IUpdateUIProperty>();

        node.Attacth2SelfNextNode(nodeActive);
        int num = RewardStruct.numberReward % 4;
        int[] array = new int[]
        {
            RewardStruct.numberReward / 4,
            RewardStruct.numberReward / 4,
            RewardStruct.numberReward / 4,
            RewardStruct.numberReward / 4 + num
        };
        Sprite TempSprite = Facade.Instance.ConfigManager.RewardConfig.GetRewardSpriteByType(RewardStruct.REWARD_TYPE);
        for (int num2 = 0; num2 != 4; num2++)
        {
            GameObject fd1 = this.FlyActionNodePool.AllocateDontActive();
            fd1.GetComponent<FlyActionNode>().Init(TempSprite);
            fd1.transform.localScale = ObjectSize[num2];
            MoveAction moveAction = default(MoveAction);
            Vector3 vector = startPos + disperseOffset[num2];
            moveAction.Init(fd1.transform, this.flyDiamondDisperseTime, startPos, vector, false, null, false, 0f);
            ActionQueue.Node node2 = new ActionQueue.Node();
            node2.PushAction(new Func<float, bool>(moveAction.Update));
            nodeActive.Attacth2SelfNextNode(node2);
            MoveAction moveAction2 = default(MoveAction);
            float LastSpeed = this.flyDiamondMoveSpeed - UnityEngine.Random.Range(0, 500);
            float time = Vector3.Distance(vector, targetPos) / LastSpeed;
            int gotRewardCount = array[num2];
            moveAction2.Init(fd1.transform, time, vector, targetPos, false, delegate
            {
                //Facade.Instance.GetReward(RewardData.REWARD_TYPE, gotRewardCount);
                if (IUpdateUIProperty != null)
                    IUpdateUIProperty.IUpdateUIPropertyFake(gotRewardCount, RewardStruct.numberReward);

                this.FlyActionNodePool.Recycle(fd1);
                if (num2 == 4) //Last callback
                {
                    UIManager.Instance.pfb_VFXUI.ResetAfterDoAnim(targetTransf);
                    if (IUpdateUIProperty != null)
                        IUpdateUIProperty.IUpdateUIProperty();
                }
            }, false, flyMove2PlayerItemDelay[num2] * 0.2f);
            ActionQueue.Node node3 = new ActionQueue.Node();
            node3.PushAction(new Func<float, bool>(moveAction2.Update));
            node2.Attacth2SelfNextNode(node3);
            ScaleAction scaleAction = default(ScaleAction);
            scaleAction.Init(0.15f, this.AnimCurve, this.AnimCurve, null, targetTransf, 0f);
            node3.Attacth2SelfNextNode(new ActionQueue.Node().PushAction(new Func<float, bool>(scaleAction.Update)));
        }
        return node;
    }
    #endregion


    #region ANIM ACTION 02
    public void DoAnimGetReward(Vector3 startPos, Transform targetTransf, RewardStruct RewardStruct)
    {
        int NumberDividedValue = 20;
        int NumberNormal = RewardStruct.numberReward / NumberDividedValue;
        float TimeDelay = 0f;
        Canvas CanvasTarget = targetTransf.GetComponent<Canvas>();
        GraphicRaycaster Raycaster = targetTransf.GetComponent<GraphicRaycaster>();
        if (Raycaster != null)
            Raycaster.enabled = false;
        //Iupdate
        IUpdateUIProperty IUpdateUIProperty = targetTransf.GetComponent<IUpdateUIProperty>();

        int StartSortingOrder = CanvasTarget.sortingOrder;
        CanvasTarget.sortingOrder = 30;

        Sprite SprReward = Facade.Instance.ConfigManager.RewardConfig.GetRewardSpriteByType(RewardStruct.REWARD_TYPE);
        for (int i = 0; i < NumberDividedValue; i++)
        {
            UIFlyActionItem UIItem = CreateController.Instance.GetPoolObject(PoolEnum.UIFlyActionItem).GetComponent<UIFlyActionItem>();
            UIItem.Init(SprReward);
            UIItem.transform.SetParent(this.transform);
            UIItem.transform.position = startPos;
            UIItem.transform.localPosition = new Vector3(UIItem.transform.localPosition.x, UIItem.transform.localPosition.y, 0);
            UIItem.imgNode.SetAlpha(.5f);
            Vector3[] newArrayPoints = new Vector3[3]
            {
                startPos,
                UnityEngine.Random.insideUnitCircle * 18.5f + (Vector2)startPos,
                targetTransf.position
            };
            Path newPath = new Path(PathType.CatmullRom, newArrayPoints, 15);
            UIItem.transform.localScale = UnityEngine.Random.Range(0.55f, 1.2f) * Vector2.one;
            UIItem.gameObject.SetActive(true);
            int TempIndex = i;
            int FakeRewardNumber = NumberNormal * (i + 1);

            UIItem.transform.DOPath(newPath, .6f + TimeDelay)
                .SetEase(Ease.InQuad)
                .OnKill(delegate
                {
                    UIItem.IDead();
                    if (IUpdateUIProperty != null && TempIndex == NumberDividedValue - 1)
                    {
                        UIManager.Instance.pfb_VFXUI.ResetAfterDoAnim(targetTransf);
                        IUpdateUIProperty.IUpdateUIProperty();
                        CanvasTarget.sortingOrder = StartSortingOrder;
                        targetTransf.localScale = Vector3.one;
                    }
                })
                .OnComplete(delegate
                {
                    if (IUpdateUIProperty != null)
                    {
                        IUpdateUIProperty.IUpdateUIPropertyFake(FakeRewardNumber, RewardStruct.numberReward);
                        targetTransf.localScale = Vector3.one;
                        targetTransf.DOKill();
                        targetTransf.DOShakeScale(0.2f, 0.6f).SetUpdate(false);
                    }
                });
            UIItem.imgNode.DOFade(1f, .1f + TimeDelay).SetEase(Ease.InQuad);
            TimeDelay += .04f;
        }
    }
    #endregion

}
