using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using Newtonsoft.Json;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using System;

public class LoginClone : MonoBehaviour
{


    
  
  
 
    int coutn;

    public void OnButton()
    {
        coutn = 200;
        StartCoroutine(SendData());
    }
    public IEnumerator SendData()
    {
        yield return new WaitForSeconds(5);
        coutn += 1;
        HandelRegister100(coutn);
        if (coutn == 300)
        {
            StopCoroutine(SendData());
            Debug.LogError("==============100OK");
        }
        else
        {
            StartCoroutine(SendData());
        }
    }
    public void OnbtnLogginAndPvPScore()
    {
        coutn = 1;
        StartCoroutine(Login());
    }
    public IEnumerator Login()
    {
        yield return new WaitForSeconds(5);
        coutn += 1;
        LoginWithEmail100(coutn);
        if (coutn == 300)
        {
            StopCoroutine(Login());
            Debug.LogError("==============1000OK");
        }
        else
        {
            StartCoroutine(Login());
        }
    }
    public void HandelRegister100(int id)
    {

        var register = new RegisterPlayFabUserRequest { Email = "Khanhdeptrai" + id.ToString() + "@gmail.com", Password = "123456" + id.ToString(), Username = "Khanhdeptrai" + id };
        PlayFabClientAPI.RegisterPlayFabUser(register, delegate { LoginWithEmail100(id); Debug.LogError("+ 1 cháu"); }, (noOK) => { Debug.LogError("noOK" + noOK); });

    }
    public void LoginWithEmail100(int id)
    {
        var requestLogin = new LoginWithEmailAddressRequest { Email = "Khanhdeptrai" + id.ToString() + "@gmail.com", Password = "123456" + id.ToString() };
        PlayFabClientAPI.LoginWithEmailAddress(requestLogin, HandleLoginSugget, (noOK) => { Debug.LogError("noOK" + noOK); });
       
    }
    public void HandleLoginSugget(LoginResult result)
    {
        //  Random.RandomRange(10, 1000)
        PlayFabClientAPI.UpdateUserTitleDisplayName(
     new UpdateUserTitleDisplayNameRequest { DisplayName = "#MuscleClone" + 10 + UnityEngine.Random.Range(0,1000) }, s => { UpdateScoreToALeaderBroad("LeaderboardName", UnityEngine.Random.RandomRange(10, 1000)); }, error => { });





    }

    public static void UpdateScoreToALeaderBroad(string nameTable, int param, Action onDone = null)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = nameTable,
                    Value = param
                }
            }
        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, win =>
        {
            onDone?.Invoke();
            Debug.Log("Sync Data Success");

        },
         error => { Debug.LogError(error + " error"); });



    }



}
