using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDragController : MonoBehaviour
{
    [SerializeField] protected CanvasGroup CG_Panel;
    protected RectTransform StartRect, EndRect;
    [SerializeField] protected RectTransform HandRect;
    [SerializeField] protected Action CallbackAfterComplete;

    public Canvas CVStarter, CVEnder;
    public GraphicRaycaster GR_Starter;
    [HideInInspector]
    public TutDrag_SubUIController SubUIStarter;
    protected bool Obj01HasCanvas, Obj02HasCanvas;
    protected bool Obj01HasGR;
    protected int InitOrderCanvas01, InitOrderCanvas02;
    public void Init(RectTransform StartRect, RectTransform EndRect)
    {
        this.StartRect = StartRect;
        this.EndRect = EndRect;
        //Khởi tại canvas và graphic raycaster cho 2 ui element để chúng nổi lên trên cùng
        this.CVStarter = this.StartRect.GetComponent<Canvas>();
        this.Obj01HasCanvas = (this.CVStarter != null);
        if(CVStarter == null)
            this.CVStarter = this.StartRect.gameObject.AddComponent<Canvas>();
        this.InitOrderCanvas01 = this.CVStarter.sortingOrder;
        this.CVStarter.overrideSorting = true;
        this.CVStarter.sortingOrder = 30009;

        this.GR_Starter = this.StartRect.GetComponent<GraphicRaycaster>();
        this.Obj01HasGR = (this.GR_Starter != null);
        if (GR_Starter == null)
            this.GR_Starter = this.StartRect.gameObject.AddComponent<GraphicRaycaster>();

        this.CVEnder = this.EndRect.GetComponent<Canvas>();
        this.Obj02HasCanvas = (this.CVEnder != null);
        if (this.CVEnder == null)
            this.CVEnder = this.EndRect.gameObject.AddComponent<Canvas>();
        this.InitOrderCanvas02 = this.CVEnder.sortingOrder;
        this.CVEnder.overrideSorting = true;
        this.CVEnder.sortingOrder = 30008;

        //Cái này để check event khi kéo thả
        this.SubUIStarter = this.StartRect.gameObject.AddComponent<TutDrag_SubUIController>();

        CG_Panel.alpha = 0;
        HandRect.transform.position = this.StartRect.transform.position;
    }
    //Khi hoàn thành step thì xóa các object đã khởi tạo và reset 
    public void Complete()
    {
        this.CVStarter.sortingOrder = InitOrderCanvas01;
        this.CVEnder.sortingOrder = InitOrderCanvas02;
        //Remove graphic raycaster first
        if (!Obj01HasGR)
            Destroy(this.GR_Starter);
        if (!Obj01HasCanvas)
            Destroy(this.CVStarter);
        if(!Obj02HasCanvas)
            Destroy(this.CVEnder);

        Destroy(this.SubUIStarter);
        ActiveHand(false);
    }
    public void StartTrack()
    {
        this.gameObject.SetActive(true);
        CG_Panel.DOKill();
        CG_Panel.DOFade(1f, .7f).SetEase(Ease.OutBack);
        StartHandTween();
    }
    public void StartHandTween()
    {
        ActiveHand(true);
        HandRect.DOKill();
        HandRect.DOMove(EndRect.transform.position, 1.5f)
            .SetEase(Ease.Linear)
            .From(this.StartRect.transform.position)
            .OnComplete(delegate
            {
                StartHandTween();
            });
    }
    public void ActiveHand(bool active)
    {
        this.HandRect.gameObject.SetActive(active);
        if (!active)
            this.HandRect.DOKill();
    }
}
