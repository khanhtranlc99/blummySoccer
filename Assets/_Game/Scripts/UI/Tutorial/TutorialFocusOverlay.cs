using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFocusOverlay : MonoBehaviour
{
    [SerializeField] protected CanvasGroup CG_Panel;
    public RectTransform HandRect, TargetRect;
    public GameObject HandLeft, HandRight;
    [SerializeField] protected Action CallbackAfterComplete;
    [HideInInspector]public Canvas CV_Target;
    [HideInInspector] public GraphicRaycaster GR_Target;
    [HideInInspector] public TutDrag_SubUIController SubUITarget;
    protected bool TargetHasCanvas;
    protected bool TargetHasGR;
    protected int InitOrderCanvas;
    public void Init(RectTransform StartRect)
    {
        this.TargetRect = StartRect;
        //Khởi tại canvas và graphic raycaster cho 2 ui element để chúng nổi lên trên cùng
        this.CV_Target = this.TargetRect.GetComponent<Canvas>();
        this.TargetHasCanvas = (this.CV_Target != null);
        if (CV_Target == null)
            this.CV_Target = this.TargetRect.gameObject.AddComponent<Canvas>();
        this.InitOrderCanvas = this.CV_Target.sortingOrder;
        this.CV_Target.overrideSorting = true;
        this.CV_Target.sortingOrder = 30009;

        this.GR_Target = this.TargetRect.GetComponent<GraphicRaycaster>();
        this.TargetHasGR = (this.GR_Target != null);
        if (GR_Target == null)
            this.GR_Target = this.TargetRect.gameObject.AddComponent<GraphicRaycaster>();
        //Cái này để check event khi kéo thả
        this.SubUITarget = this.TargetRect.gameObject.AddComponent<TutDrag_SubUIController>();

        CG_Panel.alpha = 0;
        HandRect.transform.position = this.TargetRect.transform.position;
        SetHandRight(true);
    }
    //Khi complete thì remove hết các element ui đã khởi tạo
    public void Complete()
    {
        this.CV_Target.sortingOrder = InitOrderCanvas;
        //Remove graphic raycaster first
        if (!TargetHasGR)
            Destroy(this.GR_Target);
        if (!TargetHasCanvas)
            Destroy(this.CV_Target);

        Destroy(this.SubUITarget);
        ActiveHand(false);
    }
    public void StartTrack()
    {
        CG_Panel.DOKill();
        CG_Panel.DOFade(1f, .7f).SetEase(Ease.OutBack).From(0f);
        this.gameObject.SetActive(true);
        ActiveHand(true);
    }
    public void ActiveHand(bool active)
    {
        this.HandRect.gameObject.SetActive(active);
    }

    public void ActivePanel(bool b)
    {
        CG_Panel.DOKill();
        if (b)
        {
            CG_Panel.DOFade(1f, .7f).SetEase(Ease.OutBack).From(0f);
        }
        else
        {
            CG_Panel.alpha = 0;
        }
    }
    public void SetHandRight(bool b)
    {
        this.HandRight.SetActive(b);
        this.HandLeft.SetActive(!b);
    }
}
