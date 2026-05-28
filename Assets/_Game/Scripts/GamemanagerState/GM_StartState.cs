using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class GM_StartState : IState<GameManager>
// {
//     private static GM_StartState m_Instance;
//     private GM_StartState()
//     {
//         if (m_Instance != null)
//         {
//             return;
//         }

//         m_Instance = this;
//     }
//     public static GM_StartState Instance
//     {
//         get
//         {
//             if (m_Instance == null)
//             {
//                 new GM_StartState();
//             }

//             return m_Instance;
//         }
//     }
//     public void Enter(GameManager _charState)
//     {
//         _charState.OnStartEnter();
//     }

//     public void Execute(GameManager _charState)
//     {
//         _charState.OnStartExecute();
//     }

//     public void Exit(GameManager _charState)
//     {
//         _charState.OnStartExit();
//     }
// }
