using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;

public class MoneyEffect : MonoBehaviour
{
    public enum TypeMoveEffect
    {
        MoveToCome = 0,
        FlyUp = 1,
    }

    public TypeMoveEffect typeMoveEffect;
    public CanvasGroup canvasGroup;
    public Image iconItem;
    public Text addValueTxt;
    private bool isFollowObject;
    private GameObject objectFollow;
    private Vector3 offsetFollow;
    public Text textContent;
    [SerializeField] private Transform childObj;
    //[SerializeField] private GiftDatabase giftDatabase;


    




    public void SetMoveTextFlyUp(string value, Color colorText, bool isFollowObject = false, GameObject objectFollow = null)
    {
        this.isFollowObject = isFollowObject;
        this.objectFollow = objectFollow;
        if (isFollowObject && objectFollow != null)
            offsetFollow = transform.position - objectFollow.transform.position;

        this.typeMoveEffect = TypeMoveEffect.FlyUp;

        addValueTxt.gameObject.SetActive(false);
        iconItem.gameObject.SetActive(false);

        textContent.gameObject.SetActive(true);
        textContent.text = value;
        textContent.color = colorText;

        childObj.DOKill();
        childObj.localScale = Vector3.zero;
        childObj.transform.localPosition = Vector3.zero;
        canvasGroup.DOKill();
        canvasGroup.alpha = 1;
        childObj.DOScale(1, 0.2f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Vector2 posCome = GetPointDistanceFromObject(180, Vector2.up, this.transform.position);
            childObj.DOMove(posCome, 1.5f).SetUpdate(true).OnComplete(() => { this.gameObject.SetActive(false); });
            canvasGroup.DOFade(0f, 1.2f).SetUpdate(true);
        });

        //
    }

    public void SetMoveTextFlyUpTypePlayer(string value, Color colorText,  bool isFollowObject = false, GameObject objectFollow = null)
    {
        this.isFollowObject = isFollowObject;
        this.objectFollow = objectFollow;
        if (isFollowObject && objectFollow != null)
            offsetFollow = transform.position - objectFollow.transform.position;

        this.typeMoveEffect = TypeMoveEffect.FlyUp;
        addValueTxt.color = colorText;
        addValueTxt.gameObject.SetActive(false);
        iconItem.gameObject.SetActive(false);

        textContent.gameObject.SetActive(true);
        textContent.text = value;
        textContent.color = colorText;

        childObj.DOKill();
        childObj.localScale = Vector3.one;
        childObj.transform.localPosition = Vector3.zero;
        canvasGroup.DOKill();
        canvasGroup.alpha = 1;
        childObj.DOScale(1, 0.1f).SetUpdate(true).SetEase(Ease.OutBack).OnComplete(() =>
        {
            Vector2 posCome = GetPointDistanceFromObject(65, Vector2.up, this.transform.position);
            childObj.DOMove(posCome, 0.45f).SetUpdate(true).OnComplete(() => { canvasGroup.DOFade(0f, 0.1f).SetUpdate(true).OnComplete(() => { this.gameObject.SetActive(false); }); });
            // 
            //
        });

        //
    }

    private void Update()
    {
        if (isFollowObject)
        {
            if (this.objectFollow != null)
            {
                Vector3 targetCamPos = objectFollow.transform.position + offsetFollow;
                // Smoothly interpolate between the camera's current position and it's target position.
                transform.position = targetCamPos;
            }
        }
    }

    public static Vector3 GetPointDistanceFromObject(float distance, Vector3 direction, Vector3 fromPoint)
    {
        distance -= 1;
        //if (distance < 0)
        //    distance = 0;

        Vector3 finalDirection = direction + direction.normalized * distance;
        Vector3 targetPosition = fromPoint + finalDirection;

        return targetPosition;
    }
}
