using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class GM_WinState : IState<GameManager>
// {
//     private static GM_WinState m_Instance;
//     private GM_WinState()
//     {
//         if (m_Instance != null)
//         {
//             return;
//         }

//         m_Instance = this;
//     }
//     public static GM_WinState Instance
//     {
//         get
//         {
//             if (m_Instance == null)
//             {
//                 new GM_WinState();
//             }

//             return m_Instance;
//         }
//     }
//     public void Enter(GameManager _charState)
//     {
//         _charState.OnWinEnter();
//     }

//     public void Execute(GameManager _charState)
//     {
//         _charState.OnWinExecute();
//     }

//     public void Exit(GameManager _charState)
//     {
//         _charState.OnWinExit();
//     }
// }
