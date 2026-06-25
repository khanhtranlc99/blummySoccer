using UnityEngine;
using XGame;

public class PvPController : Singleton<PvPController>
{
    public PvPScene pvpScene;
    public PvP_PlayerContain playerContain;
    private float startTime;

    public float GetTotalTime
    {
        get
        {
            float completeTime = Time.time - startTime;
            return completeTime;
        }
    }

    public void Start()
    {
        playerContain.Init();
        pvpScene.Init();
        
          XGameSdk.Instance.Track("pvp", new KVItems()
        {       
            {"pvp_id", PvPController.Instance.pvpScene.round}, 
            {"pvp_status", "start"},
              

        });



        startTime = Time.time;

    }

}