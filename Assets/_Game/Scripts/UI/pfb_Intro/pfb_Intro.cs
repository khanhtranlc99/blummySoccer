using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityExtensions;
using XGame;
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
        XGameSdk.Instance.Track("level", new KVItems()
        {
            {"level_time",  GameController.Instance.GetTotalTime},
        });
        Debug.LogError("level_time_" + GameController.Instance.GetTotalTime);


        //    Winbox.Setup().Show();
    }
    private void Ready()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.READY);
        XGameSdk.Instance.Track("level", new KVItems()
        {    
            {"level_id", Facade.Instance.PlayerPrefManager.CurrentLevel},
            {"level_status", "start"},
            {"game_level_number", GameController.Instance.GetPlayCount(Facade.Instance.PlayerPrefManager.CurrentLevel)}
        });
        GameController.Instance.AddPlay(Facade.Instance.PlayerPrefManager.CurrentLevel);
       


        XGameSdk.Instance.Track("level", new KVItems()
        {
            {"game_level_number",  GameController.Instance.GetPlayCount(Facade.Instance.PlayerPrefManager.CurrentLevel)},
        });



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
        XGameSdk.Instance.Track("level", new KVItems()
        {
            {"level_id", Facade.Instance.PlayerPrefManager.CurrentLevel},
        });
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        gameObject.SetActive(false);


    }


    

}
