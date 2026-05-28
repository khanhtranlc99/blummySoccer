using UnityEngine;

public class PvPController : Singleton<PvPController>
{
    public PvPScene pvpScene;
    public PvP_PlayerContain playerContain;

    public void Start()
    {
        playerContain.Init();
        pvpScene.Init();
    }
}