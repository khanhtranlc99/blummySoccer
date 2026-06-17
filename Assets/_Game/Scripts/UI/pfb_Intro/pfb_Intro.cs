using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;

public class pfb_Intro : UIBehavior
{
    [SerializeField] Animator animator;
    [SerializeField] CanvasGroup canvasGroup;
    public Button btnNext;
    public RotateObj rotateObj;
    public void OnEnable()
    {
        rotateObj.rotate = 30;
        btnNext.gameObject.SetActive(false);
        btnNext.transform.localScale = Vector3.zero;
        btnNext.onClick.RemoveAllListeners();
        btnNext.onClick.AddListener(delegate { GameManager.Instance.Nextlevel(); btnNext.gameObject.SetActive(false); });



    }

    public void PlayIntro()
    {
        animator.SetTrigger("Intro");
    }
    public void PlayOutro()
    {
        animator.SetTrigger("Outro");
    }
    private void Nextlevel()
    {
        rotateObj.rotate = 5;
        btnNext.gameObject.SetActive(true);
        btnNext.transform.DOScale(new Vector3(1,1,1), 0.5f);


        Debug.LogError("12346Nextlevel");
    //    Winbox.Setup().Show();
    }
    private void Ready()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.READY);
        // SoundManager.Instance.PlayAudioClip(SoundType.READY);
    }

    private void StartGame()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.GO);
        // SoundManager.Instance.PlayAudioClip(SoundType.GO);
        GameManager.Instance.GAME_STATE = GAME_STATE.PLAYING;
        CameraManager.Instance?.ShakeMainCam(true, CAMERA_SHAKE_TYPE.INTRO_SHAKE);
    }
    private void ToggleOff()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        gameObject.SetActive(false);
    }

}
