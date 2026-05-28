using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Settings_CustomToggle : MonoBehaviour
{
    [SerializeField] protected Button btnClick;
    [SerializeField] protected Action<bool> callbackAfterClick;
    [SerializeField] protected Image imgHandle;
    public bool isOn = false;
    private float PrimitiveValueX;
    private void Start()
    {
        btnClick.onClick.AddListener(OnClick);
    }
    public void Init(bool isOn, Action<bool> CallbackAfterClick)
    {
        this.PrimitiveValueX = this.imgHandle.rectTransform.anchoredPosition.x;
        this.isOn = isOn;
        callbackAfterClick = CallbackAfterClick;

        int Direction = isOn ? 1 : -1;
        this.imgHandle.rectTransform.anchoredPosition = new Vector2(Direction * this.PrimitiveValueX, this.imgHandle.rectTransform.anchoredPosition.y);
        this.imgHandle.sprite = isOn ? Facade.Instance.AtlasManager.GetSpriteUISettings("SwitchButton_On") : Facade.Instance.AtlasManager.GetSpriteUISettings("SwitchButton_Off");
    }

    protected void OnClick()
    {
        isOn = !isOn;
        int Direction = isOn ? 1 : -1;
        this.imgHandle.rectTransform.DOKill();
        this.imgHandle.rectTransform.DOAnchorPos3DX(Direction * this.PrimitiveValueX, .2f);
        this.imgHandle.sprite = isOn ? Facade.Instance.AtlasManager.GetSpriteUISettings("SwitchButton_On") : Facade.Instance.AtlasManager.GetSpriteUISettings("SwitchButton_Off");

        this.callbackAfterClick?.Invoke(isOn);
        // SoundManager.Instance.PlayAudioClip(SoundType.CLICK);
    }

}
