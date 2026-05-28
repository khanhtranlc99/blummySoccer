using DG.Tweening;
using TMPro;
using UnityEngine;


public class PopupText : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI txtMessage;
    [SerializeField] protected CanvasGroup CanvasGroup;

    public void Play(MESSAGE_TYPE _type)
    {
        txtMessage.text = _type.ToString();
        CanvasGroup.alpha = 1;
        float PosY = this.transform.position.y + .5f;
        transform.DOScale(1.3f, .4f).From(0f);
        transform.DOMoveY(PosY, .4f).OnComplete(delegate
        {
            CanvasGroup.DOFade(0f, .8f).OnComplete(
                delegate
                {
                    CreateController.Instance.Despawn(this.gameObject);
                });
        });
    }
}
