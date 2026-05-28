using System.Net.Mime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AdOne;
using TMPro;
using UnityEngine.UI;
// using Sirenix.OdinInspector;
using DG.Tweening;

public class Testing : MonoBehaviour
{
    // [Button("Test")]
    public void Test()
    {
        this.transform.DOScaleY(1.05f, .1f);
        this.transform.DOScaleX(1.05f, .1f).SetDelay(.1f).SetLoops(1, LoopType.Yoyo);
    }

}
