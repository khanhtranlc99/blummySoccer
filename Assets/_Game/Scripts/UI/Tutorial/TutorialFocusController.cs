using System;
using UnityEngine;
using UnityEngine.UI;

public class TutorialFocusController : MonoBehaviour
{
    public RectTransform rectangleFocus;
    public Transform panel;
    public Button btnClick;
    public Transform FX_Tutorial;
    private float ScaleFXVal = 1;
    private Vector2 InitSizeDelta;
    protected void Awake()
    {
        ScaleFXVal = FX_Tutorial.localScale.x;
        InitSizeDelta = rectangleFocus.sizeDelta;
    }
    public void SetFocus(Transform trans, float scale = 1.0f, bool withPanel = true, Vector3 offset = default(Vector3))
    {
        this.gameObject.SetActive(true);
        if (withPanel)
            panel.gameObject.SetActive(true);
        else
            panel.gameObject.SetActive(false);

        rectangleFocus.gameObject.SetActive(true);
        rectangleFocus.transform.position = trans.position;
        // RectTransform rect = rectangleFocus as RectTransform;
        rectangleFocus.anchoredPosition3D = new Vector3(rectangleFocus.anchoredPosition3D.x, rectangleFocus.anchoredPosition3D.y, 0) + offset;
        RectTransform rectT = trans as RectTransform;
        rectangleFocus.sizeDelta = rectT.sizeDelta * scale;

        Vector2 RatioScaleFX = new Vector2(rectT.sizeDelta.x / InitSizeDelta.x, rectT.sizeDelta.y / InitSizeDelta.y);
        FX_Tutorial.localScale = ScaleFXVal * RatioScaleFX * scale;
        FX_Tutorial.transform.position = rectangleFocus.transform.position;
    }
    public void SetFocus(Vector2 rectPos, Vector2 rectSize, float scale = 1.0f, bool withPanel = true, Vector3 offset = default(Vector3))
    {
        this.gameObject.SetActive(true);
        if (withPanel)
            panel.gameObject.SetActive(true);
        else
            panel.gameObject.SetActive(false);

        rectangleFocus.gameObject.SetActive(true);
        rectangleFocus.anchoredPosition = rectPos;
        // RectTransform rect = rectangleFocus as RectTransform;
        rectangleFocus.anchoredPosition3D = new Vector3(rectangleFocus.anchoredPosition3D.x, rectangleFocus.anchoredPosition3D.y, 0) + offset;
        rectangleFocus.sizeDelta = rectSize * scale;

        Vector2 RatioScaleFX = new Vector2(rectSize.x / InitSizeDelta.x, rectSize.y / InitSizeDelta.y);
        FX_Tutorial.localScale = ScaleFXVal * RatioScaleFX * scale;
        FX_Tutorial.transform.position = rectangleFocus.transform.position;
    }
    public void AddEventCallback(Action callback)
    {
        btnClick.onClick.RemoveAllListeners();
        btnClick.onClick.AddListener(delegate
        {
            callback?.Invoke();
        });
    }
}
