using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitStateBase : MonoBehaviour
{
    public StateType stateType;

  
    public UnitFSMController unitFSMController;
    public virtual void Init(UnitFSMController paramFsm)
    {
    
        unitFSMController = paramFsm;
    }
    public abstract void StartState();
    public abstract void UpdateState();
    public abstract void EndState();



}
public enum StateType
{
    None = 0,
    Idle = 1,
    Move = 2,
  
    Die = 3,
    Attack = 4,
}



