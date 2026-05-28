using DG.Tweening;
using MoonlightFramework;

// using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

public sealed class Facade : MonoSingleton<Facade>
{
    public PlayerPrefManager PlayerPrefManager;
    public GameManager GameManager;
    public ConfigManager ConfigManager;
    public TutorialManager TutorialManager;
    public AtlasManager AtlasManager;
    public UIManager UIManager;
    public RewardClaimer RewardClaimer;
    public bool isTaskSetupDone = false;

    public bool isHackMode = false;
    public bool isTestMode = false;
    public bool isStart = false;
    // [Button("Test")]
    public void Test()
    {
        this.RewardClaimer.GetReward(new RewardStruct(REWARD_TYPE.GOLD, 15));
    }
    protected override void Awake()
    {
     
        Initialize();
    }
    void Start()
    {
      if(!isStart)
        {
            isStart = true;
            OpenTaskSetup();
            StartCoroutine(ShowAdsPeriodically());
        }

    }

    private IEnumerator ShowAdsPeriodically()
    {
        WaitForSeconds delay = new WaitForSeconds(4f);
        while (true)
        {
            yield return delay;
            if (Application.internetReachability == NetworkReachability.NotReachable
                && !UseProfile.IsRemoveAds)
            {
                this.UIManager.pfb_BanNoAds.ActiveNormalPopup(true);
            }
        }
    }

    public void OpenTaskSetup()
    {
        StartCoroutine(TaskSetup());
    }
    private IEnumerator TaskSetup()
    {
        yield return StartCoroutine(this.UIManager.TaskSetup());

        yield return new WaitForSeconds(.3f);
        isTaskSetupDone = true;
    }
    private void Initialize()
    {
      
        if( !isStart)
        {
            Application.targetFrameRate = 60;
            base.Awake();
            Instantiate(Utils.ResourcesLoad<GameObject>("CreateController"));
            Instantiate(Utils.ResourcesLoad<GameObject>("ObjectPooling"));
            PlayerPrefManager = new GameObject("PlayerPrefManager").AddComponent<PlayerPrefManager>();
            // ConfigManager.UserData.LoadData();
            DOTween.SetTweensCapacity(400, 50);
            GameManager = Instantiate(Utils.ResourcesLoad<GameManager>("GameManager"));
          //  DontDestroyOnLoad(GameManager);
            this.UIManager.PreInit();
  
        }
    

    
    }
}
