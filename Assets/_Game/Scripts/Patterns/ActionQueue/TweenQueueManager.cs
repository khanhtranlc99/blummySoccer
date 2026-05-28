using UnityEngine;
using System;
using DG.Tweening;
using System.Collections.Generic;

public enum TweenActionType
{
    General = 1,
}
public class TweenQueueManager
{
    public Dictionary<TweenActionType, TweenActionQueue> BigData = new Dictionary<TweenActionType, TweenActionQueue>();

    public void KillAllAction(TweenActionType type)
    {
        if (!BigData.ContainsKey(type)) return;

        TweenActionQueue SuitableAction = null;
        if (TryGetTweenActionQueue(type, out SuitableAction))
        {
            SuitableAction.Kill();
        }
    }
    public void InitAction(TweenActionType _Type, Tween newTween)
    {
        TweenActionQueue SuitableAction = null;
        if (!TryGetTweenActionQueue(_Type, out SuitableAction))
            this.BigData.Add(_Type, new TweenActionQueue());

        SuitableAction.PushAction(newTween);
    }
    public void InitAction(TweenActionType _Type, Sequence newSeq)
    {
        TweenActionQueue SuitableAction = null;
        if (!TryGetTweenActionQueue(_Type, out SuitableAction))
            this.BigData.Add(_Type, new TweenActionQueue());

        SuitableAction.PushAction(newSeq);
    }
    public bool TryGetTweenActionQueue(TweenActionType _Type, out TweenActionQueue tweenActionQueue)
    {
        return BigData.TryGetValue(_Type, out tweenActionQueue);
    }
}
public class TweenActionQueue
{
    public Queue<Sequence> actionQueue = new Queue<Sequence>();
    public Action CallbackWhenQueueComplete = null;
    public bool isComplete = true;

    public void Kill()
    {
        this.CallbackWhenQueueComplete = null;
        foreach (var item in actionQueue)
            item.Kill();
        //Facade.Instance.FightActionQueueController.RemoveActionQueue(this.ActionName);
    }
    protected void PushAction(Sequence seq)
    {
        seq.OnComplete(OnComplete);
        actionQueue.Enqueue(seq);
        StartAction(); //Start instantly
    }
    public void PushAction(Tween tween)
    {
        Sequence newSeq = DOTween.Sequence();
        newSeq.Pause();
        newSeq.SetUpdate(UpdateType.Normal, false); //Bỏ ignore timescale
        newSeq.Append(tween);
        PushAction(newSeq);
    }
    public void PushActionCallback(Action action) => this.CallbackWhenQueueComplete = action;
    public void StartAction()
    {
        if (!this.isComplete)
            return;
        if(actionQueue.Count == 0)
        {
            this.isComplete = true;
            return;
        }
        this.isComplete = false;
        PlayNext();
    }

    //Callback
    private void OnComplete()
    {
        //remove animation that was completed
        actionQueue.Dequeue();

        //if there's animations in queue left
        if (actionQueue.Count > 0)
            PlayNext();
        else
        {
            CallbackWhenQueueComplete?.Invoke();
            this.isComplete = true;
            //Facade.Instance.FightActionQueueController.RemoveActionQueue(this.ActionName);
        }
    }

    private void PlayNext()
    {
        //play next
        if (actionQueue.Count > 0)
            actionQueue.Peek().Play();
    }
}
