using DG.Tweening;
using FirstGearGames.SmoothCameraShaker;
// using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoSingleton<CameraManager>
{
    public Camera MainCam, UICamera;
    public FirstGearGames.SmoothCameraShaker.ShakeData MaxForceShake;
    public FirstGearGames.SmoothCameraShaker.ShakeData WinShake_01;
    public FirstGearGames.SmoothCameraShaker.ShakeData WinShake_02;
    public FirstGearGames.SmoothCameraShaker.ShakeData INTRO_SHAKE;
    [SerializeField] protected float targetOS = 9f;

    Tween cameraTween;
    Tween orthorgraphicTween;
    Sequence sequence;
    public void SetCameraPosition(MapController map, Action callBack = null)
    {
        Transform topLeft = map.topLeft;
        Transform bottomRight = map.bottomRight;
        MainCam.transform.position = Vector3.zero;

        this.MainCam.orthographicSize = 9f;
        if (cameraTween != null) cameraTween.Kill();
        if (orthorgraphicTween != null) orthorgraphicTween.Kill();
        if (sequence != null) sequence.Kill();
        if (topLeft != null && bottomRight != null)
        {
            Vector3 middlePoint = (topLeft.transform.position + bottomRight.position) / 2;


            float distanceX = Mathf.Abs(topLeft.transform.position.x - bottomRight.position.x);
            float distanceY = Mathf.Abs(topLeft.transform.position.y - bottomRight.position.y);

            Vector3 desiredPosition = middlePoint - transform.forward;
            desiredPosition.z = -100;

            float requiredSize = Mathf.Max(distanceX / MainCam.aspect, distanceY) / 2 + 5f;
            if (requiredSize < 20f) requiredSize = 20f;
            targetOS = requiredSize;

            MainCam.orthographicSize = requiredSize + 10;

            //  sequence = DOTween.Sequence();
            // sequence.Join(MainCam.transform.DOMove(desiredPosition, 2f));
            // sequence.Join(DOTween.To(() => this.MainCam.orthographicSize, x => this.MainCam.orthographicSize = x, requiredSize, 2f).SetEase(Ease.OutExpo));
            // sequence.OnComplete(delegate
            // {
            //     callBack?.Invoke();
            // });
              BackgroundInfo backGround = GameManager.Instance.CurrentMap._backGround;
            backGround.FitSpriteToCamera();
            sequence = DOTween.Sequence();
            sequence.Join(MainCam.transform.DOMove(desiredPosition, 2f));
            sequence.Join(DOTween.To(() => this.MainCam.orthographicSize, x => this.MainCam.orthographicSize = x, requiredSize, 2f).SetEase(Ease.OutExpo));
            sequence.OnUpdate(() => backGround.FitSpriteToCamera());
            sequence.OnComplete(() => callBack?.Invoke());



        }
    }
    // [Button("Test")]
    public void ShakeMainCam(bool active, CAMERA_SHAKE_TYPE _TYPE = CAMERA_SHAKE_TYPE.WIN_SHAKE_01)
    {
        //this.MainCam.DOShakePosition();
        if (active)
        {
            switch (_TYPE)
            {
                case CAMERA_SHAKE_TYPE.INTRO_SHAKE:
                    CameraShakerHandler.Shake(this.INTRO_SHAKE);
                    break;
                case CAMERA_SHAKE_TYPE.MAXFORCE_LOOP:
                    CameraShakerHandler.Shake(this.MaxForceShake);
                    break;
                case CAMERA_SHAKE_TYPE.WIN_SHAKE_01:
                    CameraShakerHandler.Shake(this.WinShake_01);
                    break;
                case CAMERA_SHAKE_TYPE.WIN_SHAKE_02:
                    CameraShakerHandler.Shake(this.WinShake_02);
                    break;
                default: break;
            }
        }
        else
            CameraShakerHandler.Stop();
    }
    // [Button("Test2")]
    public void DoAnimWin(Transform targetWin)
    {
        //thực hiện anim cho bóng đứng yên và xoay mạnh
        GameManager.Instance.GAME_STATE = GAME_STATE.END;
        GlobalAudioPlayer.PlaySFX(eAudioType.CROWD);
        //di chuyển đến gần giữa bóng,
        //Đồng thời shake cam, duration shake bằng time từ lúc fx nổ đến hết anim
        //OS value move về giá trị target
        ShakeMainCam(true, CAMERA_SHAKE_TYPE.WIN_SHAKE_01);
        Vector3 CamPos = targetWin.position;
        CamPos.z = -100;
        //CamPos.x -= CamPos.x / 3;
        if (orthorgraphicTween != null)
        {
            DOTween.Kill(MainCam.transform);
            orthorgraphicTween.Kill();
        }

        this.MainCam.transform.DOMove(CamPos, .7f).SetEase(Ease.OutExpo);

        DOTween.To(() => this.MainCam.orthographicSize, x => this.MainCam.orthographicSize = x, 3f, .8f).SetEase(Ease.OutExpo);
        //cam show fx flash

        //cho othor chạy dần về ban đầu
        //cho fx nổ và bóng biến mất
        DOVirtual.DelayedCall(.9f, delegate
        {
            ShakeMainCam(true, CAMERA_SHAKE_TYPE.WIN_SHAKE_02);
            GameObject fx_confetti = CreateController.Instance.GetPoolItemVfx(PoolEnum.Fx_Confetti);
            GameObject fx_explosion = CreateController.Instance.GetPoolItemVfx(PoolEnum.Fx_Explosion);

            fx_confetti.transform.position = targetWin.position;
            fx_explosion.transform.position = targetWin.position;

            fx_confetti.SetActive(true);
            fx_explosion.SetActive(true);

            GlobalAudioPlayer.PlaySFX(eAudioType.BALL_WIN);

            CreateController.Instance.Despawn(targetWin.gameObject);
            DOVirtual.DelayedCall(1.5f, delegate
            {
                CreateController.Instance.Despawn(fx_confetti);
                CreateController.Instance.Despawn(fx_explosion);
            });
            this.MainCam.transform.DOMove(CamPos, .2f).SetEase(Ease.Linear);
            DOTween.To(() => this.MainCam.orthographicSize, x => this.MainCam.orthographicSize = x, 9f, 1.0f)
            .SetEase(Ease.OutExpo);


            GameManager.Instance.ActiveOutro();
        });


        // DOVirtual.DelayedCall(2.4f, delegate
        // {

        //     //GameManager.Instance.Nextlevel();
        //     // GameManager.Instance.ActivePopUpWin();
        // });
    }
}
[Serializable]
public enum CAMERA_SHAKE_TYPE
{
    MAXFORCE_LOOP = 1,
    WIN_SHAKE_01 = 2,
    WIN_SHAKE_02 = 3,
    INTRO_SHAKE = 4
}
