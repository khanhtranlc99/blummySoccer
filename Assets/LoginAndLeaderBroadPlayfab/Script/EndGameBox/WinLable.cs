using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using DG.Tweening;
public class WinLable : LableEndGame
{
    public Transform postUser;
    public Transform postAi;
    public Transform lableUser;
    public Transform lableAi;
    public Text tvScoreUser;
    public Text tvScoreAi;
    public override void Init(Action callBack)
    {
        tvScoreUser.text = "+" + (PvPController.Instance.pvpScene.scoreUser *10);
        tvScoreAi.text = "-" + (PvPController.Instance.pvpScene.scoreUser * 10);
        Sequence sequence = DOTween.Sequence();
        sequence.Append(lableUser.DOMoveX(postUser.position.x, 0.5f));
        sequence.Append(lableAi.DOMoveX(postAi.position.x, 0.5f));
        sequence.OnComplete(delegate
        {
            callBack?.Invoke();
        });
    }
}
