using DG.Tweening;
// using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public abstract class UIBehavior : MonoBehaviour, IUpdateUI
{
    public CanvasGroup CanvasComponent;
#if UNITY_EDITOR
    // [Button("ActiveEditor")]
    public virtual void ActiveEditor()
    {
        this.gameObject.SetActive(!CanvasComponent.interactable);
        CanvasComponent.interactable = !CanvasComponent.interactable;
        CanvasComponent.blocksRaycasts = !CanvasComponent.blocksRaycasts;
        CanvasComponent.alpha = CanvasComponent.interactable ? 1 : 0;
    }
#endif
    protected virtual void Awake()
    {
        CanvasComponent = GetComponent<CanvasGroup>();
        SetUpCamera();
        OnInit();
    }
    protected virtual void OnEnable()
    {

    }
    protected virtual void OnReset()
    {

    }
    protected virtual void OnInit()
    {

    }
    protected virtual void SetUpCamera()
    {
        GetComponent<Canvas>().worldCamera = CameraManager.Instance.UICamera;
    }
    public virtual void SetAlpha(float alpha)
    {
        CanvasComponent.alpha = alpha;
    }
    public virtual void ActivePopup(bool active = true)
    {
        UIManager.Instance.DeactivateAllPopup();
        ForceActiveNormalPopup(active);
        CanvasComponent.blocksRaycasts = active;
        CanvasComponent.interactable = active;
    }
    public virtual void ActiveNormalPopup(bool active = true)
    {
        this.CanvasComponent.DOKill();
        if (active)
        {
            gameObject.SetActive(true);
            CanvasComponent.DOFade(1f, .5f);
        }
        else
        {
            CanvasComponent.DOFade(0f, .5f).OnComplete(delegate
            {
                gameObject.SetActive(false);
            });
        }
        CanvasComponent.blocksRaycasts = active;
        CanvasComponent.interactable = active;
    }
    public virtual void ForceActiveNormalPopup(bool active = true)
    {
        if (active)
        {
            SetAlpha(1);
            gameObject.SetActive(true);
        }
        else
        {
            SetAlpha(0);
            gameObject.SetActive(false);
        }
        CanvasComponent.blocksRaycasts = active;
        CanvasComponent.interactable = active;
    }
    public virtual bool IsInteractable() => this.CanvasComponent.interactable;
    public virtual bool IsActive()
    {
        if (CanvasComponent.alpha == 1)
            return true;
        return false;
    }
    public virtual void SetInteractable(bool b)
    {
        CanvasComponent.interactable = b;
    }
    public virtual void SetBlockRaycast(bool b)
    {
        CanvasComponent.blocksRaycasts = b;
    }
    public virtual void IUpdateUI()
    {

    }
    protected virtual void OnDisable()
    {

    }
}