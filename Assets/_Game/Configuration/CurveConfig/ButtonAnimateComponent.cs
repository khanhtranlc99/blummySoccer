using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAnimateComponent : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IPointerExitHandler
{
    [SerializeField]
    protected Transform _target;
    public CurveType CurveType = CurveType.MEDIUM_BOUNCE;

    private Tween TempTween;
    protected bool isDoTween = false;
    private Vector3 InitScale = Vector3.one;
    private void Awake()
    {
        this.InitScale = _target.transform.localScale;
    }
    private void Reset()
    {
        this._target = this.transform;
    }
    private void OnEnable()
    {
        this._target.localScale = this.InitScale;
        this.isDoTween = false;
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        this.TempTween.Kill();
        TempTween = this._target.DOScale(this.InitScale * .75f, .25f);
        this.isDoTween = false;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDoTween) return;
        isDoTween = true;
        this.TempTween.Kill();
        // this.TempTween = this._target.DOScale(this.InitScale, .45f).SetEase(Facade.Instance.ConfigManager.CurveConfig.GetCurveByType(this.CurveType));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDoTween) return;
        isDoTween = true;
        this.TempTween.Kill();
        this.TempTween = this._target.DOScale(this.InitScale, .45f);
    }
}
