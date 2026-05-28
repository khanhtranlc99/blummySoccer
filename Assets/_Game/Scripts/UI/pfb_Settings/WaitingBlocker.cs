using DG.Tweening;
using UnityEngine;

public class WaitingBlocker : MonoBehaviour
{
    [SerializeField] protected Transform IconWait;

    private void OnEnable()
    {
        this.IconWait.DOKill();
        IconWait.DORotate(new Vector3(0, 0, 360), .76f, RotateMode.FastBeyond360)
            .SetLoops(-1, LoopType.Restart)
            .SetEase(Ease.Linear);
    }

    private void OnDisable()
    {
        this.IconWait.DOKill();
    }
}
