// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GM_RunningState : IState<GameManager>
// {
//     private static GM_RunningState m_Instance;
//     private GM_RunningState()
//     {
//         if (m_Instance != null)
//         {
//             return;
//         }

//         m_Instance = this;
//     }
//     public static GM_RunningState Instance
//     {
//         get
//         {
//             if (m_Instance == null)
//             {
//                 new GM_RunningState();
//             }

//             return m_Instance;
//         }
//     }
//     public void Enter(GameManager _charState)
//     {
//         _charState.OnRunningEnter();
//     }

//     public void Execute(GameManager _charState)
//     {
//         _charState.OnRunningExecute();
//     }

//     public void Exit(GameManager _charState)
//     {
//         _charState.OnRunningExit();
//     }
// }
