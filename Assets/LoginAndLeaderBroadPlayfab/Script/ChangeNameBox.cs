using System.Collections;
using System.Collections.Generic;
using PlayFab.ClientModels;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using PlayFab;
public class ChangeNameBox : BaseBox
{
    public static ChangeNameBox _instance;
    public static ChangeNameBox Setup()
    {
        if (_instance == null)
        {
            _instance = Instantiate(Resources.Load<ChangeNameBox>(PathPrefabs.CHANGE_NAME_BOX));
            _instance.Init();
        }
        _instance.InitState();
        return _instance;
    }
    public TMP_InputField inputField;
    public Button btnOK;
    public Button btnClose;

    private void Init()
    {
        btnOK.onClick.AddListener(delegate { HandleOK();  });
        btnClose.onClick.AddListener(HandleClose);
    }
    private void InitState()
    {
        
    }
    private void HandleClose()
    {
        Close();
    }
    private void HandleOK()
    {
        if(inputField.text != null && inputField.text != "")
        {
            UseProfile.NamePlayer = inputField.text;
            LeaderBoardPvPBox.Instance.tvName.text = UseProfile.NamePlayer;
            LeaderBoardPvPBox.Instance.laybelMe.tvName.text = UseProfile.NamePlayer;
            UseProfile.WasClickChangeName = true;
            Close();
            //PlayFabClientAPI.UpdateUserTitleDisplayName(
            //new UpdateUserTitleDisplayNameRequest { DisplayName = UseProfile.NamePlayer }, s => { Debug.LogError("ok"); Close(); }, f => { Debug.LogError("No-ok"); Close(); });

        }
        else
        {
            GameController.Instance.effectController.SpawnEffectText_FlyUp
                        (
                        btnOK.transform.position,
                        "Input pls!",
                        Color.white,
                        isSpawnItemPlayer: true
                        );
        }

    }
}

