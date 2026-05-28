using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TutorialManager : MonoBehaviour
{
    public void Play(int Idx)
    {
        switch (Idx)
        {
            case 1:
                Tutorial01 tut01 = new Tutorial01();
                tut01.Start();
                break;
            default:
                break;
        }
    }

}
