using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayEventController : MonoBehaviour
{
    public GameSceneEvent gameSceneEvent;
    //public UserPlayEvent userPlayEvent;




    void Start()
    {
        gameSceneEvent.Init();
        //userPlayEvent.Init(gameSceneEvent.infoUser);
      
    }

  
}
