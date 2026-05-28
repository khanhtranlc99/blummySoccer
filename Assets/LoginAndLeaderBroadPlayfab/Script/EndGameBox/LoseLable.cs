using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
using UnityEngine.UI;
public class LoseLable : LableEndGame
{
    public Transform postUser;
    public Transform postAi;
    public Transform lableUser;
    public Transform lableAi;
    public Text tvScoreUser;
    public Text tvScoreAi;
    public override void Init(Action callBack)
    {
        tvScoreUser.text = "-" + GConnection.scoreUser;
        callBack?.Invoke();
        //tvScoreAi.text = "+" + GConnection.scoreUser;
        //Sequence sequence = DOTween.Sequence();
        //sequence.Append(lableUser.DOMove(postUser.position, 0.5f));
        //sequence.Append(lableAi.DOMove(postAi.position, 0.5f));
        //sequence.OnComplete(delegate
        //{
        //    callBack?.Invoke();
        //});
    }
}
