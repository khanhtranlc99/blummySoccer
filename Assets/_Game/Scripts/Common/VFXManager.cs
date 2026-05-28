using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoSingleton<VFXManager>
{
    public ActionQueue actionQueue = new ActionQueue();
    private void Update()
    {
        actionQueue.Update();
    }
}
