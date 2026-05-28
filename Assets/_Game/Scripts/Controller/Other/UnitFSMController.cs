using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;

public class UnitFSMController : MonoBehaviour
{
    public UnitStateBase idleState;
    public UnitStateBase moveState;
    public UnitStateBase dieState;
    public UnitStateBase atkState;

 

    public UnitStateBase currentState;



    public void Init ()
    {
        idleState.Init( this);
        moveState.Init( this);
        atkState.Init( this);
        dieState.Init( this);

    }


    public void ChangeState(StateType stateType)
    {
        if (currentState != null)
        {
            currentState.EndState();
        }

        switch (stateType)
        {
            case StateType.Idle:
                idleState.EndState();
                currentState = idleState;

                break;
            case StateType.Move:
                moveState.EndState();
                currentState = moveState;

                break;
            case StateType.Attack:
                atkState.EndState();
                currentState = atkState;

                break;
            case StateType.Die:
                dieState.EndState();
                currentState = dieState;

                break;
        }
        currentState.StartState();
    }

    public void Stop()
    {
        if (currentState != null)
        {
            currentState.EndState();
        }
    }


    public void Update()
    {
        switch (currentState.stateType)
        {
            case StateType.Idle:
                idleState.UpdateState();
               

                break;
            case StateType.Move:
                moveState.UpdateState();
            

                break;
            case StateType.Attack:
                atkState.UpdateState();
              

                break;
            case StateType.Die:
                dieState.UpdateState();
             
                break;
        }
    }


}
