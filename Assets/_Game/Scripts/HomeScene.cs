using UnityEngine;
using UnityEngine.UI;
using TMPro;
using XGame;
public class HomeScene : MonoBehaviour
{

    public Button btnGameplay;
    public Button btnPvP;
    public TMP_Text tmp;
   
    void Start()
    {

        XGameSdk.Instance.Track("UE", new KVItems()
        {
            {"view_show", "homepage"},
        });

        tmp.text = "Level " +  Facade.Instance.PlayerPrefManager.CurrentLevel.ToString();
        btnGameplay.onClick.AddListener(delegate {
            HandleGamePlay();
        });
        btnPvP.onClick.AddListener(delegate
        {
            HandlePvP();
        });
    }

    public void HandleGamePlay()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        Initiate.Fade("Main", Color.black, 2f);
        XGameSdk.Instance.Track("UE", new KVItems()
        {
            {"button", "start_game"},
        });

    }
    public void HandlePvP()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        Initiate.Fade("HomePvP", Color.black, 2f);
    }


}
