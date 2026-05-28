using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BtnAnim : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 targetScaleValue = Vector3.one; // Default scale value to (1,1,1)
    public Transform StartPos;
    public bool isSetPos = false;
    public Vector2 EndPos;
    public float time = 0.5f;
    public bool setUpdate = false;
    public float delay = 0f;

    private Vector3 originScale;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        originScale = rect.localScale;
    }

    private void OnEnable()
    {
        // Ensure rect is set
        if (rect == null)
        {
            rect = GetComponent<RectTransform>();
        }

        // Initialize the scale to zero
        rect.localScale = Vector3.zero;

        if (isSetPos && StartPos != null)
        {
            // Set the starting position if isSetPos is true
            rect.position = StartPos.position;
            rect.DOAnchorPos(EndPos, time).SetUpdate(setUpdate);
        }

        // Animate the scale to the target value
        rect.DOScale(targetScaleValue, time).SetUpdate(setUpdate).SetDelay(delay);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        DOTween.Kill(rect);
        rect.DOScale(0.8f * targetScaleValue, 0.1f).SetEase(Ease.OutBack);
    }

    public void OnPointerUp(PointerEventData eventData)
    {

        DOTween.Kill(rect);
        rect.DOScale(targetScaleValue, 0.1f);
    }

    private void OnDisable()
    {
        DOTween.Kill(rect);
        rect.localScale = originScale;
    }
}
