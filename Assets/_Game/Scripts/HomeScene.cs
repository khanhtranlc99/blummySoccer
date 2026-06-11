using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HomeScene : MonoBehaviour
{

    public Button btnGameplay;
    public Button btnPvP;
    public TMP_Text tmp;
   
    void Start()
    {
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
    }
    public void HandlePvP()
    {
        GlobalAudioPlayer.PlaySFX(eAudioType.CLICK);
        Initiate.Fade("HomePvP", Color.black, 2f);
    }


}
