using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pfb_Intro : UIBehavior
{
    [SerializeField] Animator animator;
    [SerializeField] CanvasGroup canvasGroup;

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
        //GameManager.Instance.Nextlevel();
        //Debug.LogError("12346Nextlevel");
        Winbox.Setup().Show();
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
