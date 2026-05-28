using DG.Tweening;
// using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public abstract class UIPopupBehavior : MonoBehaviour, IUpdateUI
{
    public CanvasGroup CanvasGroup;
    public Button btnClose, btnPanelClose;
    public Transform Popup;
    protected virtual void Start()
    {
        btnClose?.onClick.AddListener(delegate
        {
            GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
            OnClose();
        });
        btnPanelClose?.onClick.AddListener(OnClose);
    }
#if UNITY_EDITOR
    // [Button("ActiveEditor")]
    public virtual void ActiveEditor()
    {
        //this.gameObject.SetActive(!CanvasGroup.interactable);
        CanvasGroup.interactable = !CanvasGroup.interactable;
        CanvasGroup.blocksRaycasts = !CanvasGroup.blocksRaycasts;
        CanvasGroup.alpha = CanvasGroup.interactable ? 1 : 0;
    }
#endif
    public virtual void ActiveNormalPopup(bool b = true)
    {
        CanvasGroup.interactable = b;
        CanvasGroup.blocksRaycasts = b;
        this.CanvasGroup.DOKill();
        if (b)
        {
            this.gameObject.SetActive(true);
            CanvasGroup.DOFade(1f, 0.6f);
            if (Popup != null)
            {
                Popup.DOKill();
                this.Popup.localScale = Vector3.zero;
                this.Popup.DOScale(1f, .3f).SetEase(Ease.OutBack);
            }
        }
        else
        {
            CanvasGroup.DOFade(0f, 0.6f).OnComplete(delegate
            {
                this.gameObject.SetActive(false);
            });
            if (Popup != null)
            {
                this.Popup.DOScale(0f, .3f);
            }
        }
    }
    public virtual void ForceActiveNormalPopup(bool b)
    {
        CanvasGroup.interactable = b;
        CanvasGroup.blocksRaycasts = b;
        CanvasGroup.alpha = (b) ? 1 : 0;
    }
    public virtual void Init()
    {

    }
    public virtual void IUpdateUI()
    {
    }
    protected virtual void OnClose()
    {
        ActiveNormalPopup(false);
    }
    public virtual bool IsActive()
    {
        if (CanvasGroup.alpha == 1)
            return true;
        return false;
    }
    public virtual void SetInteractable(bool b)
    {
        CanvasGroup.interactable = b;
    }
    public virtual void SetBlockRaycast(bool b)
    {
        CanvasGroup.blocksRaycasts = b;
    }
}
