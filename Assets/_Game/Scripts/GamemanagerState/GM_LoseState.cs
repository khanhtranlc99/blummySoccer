using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// public class GM_LoseState : IState<GameManager>
// {
//     private static GM_LoseState m_Instance;
//     private GM_LoseState()
//     {
//         if (m_Instance != null)
//         {
//             return;
//         }

//         m_Instance = this;
//     }
//     public static GM_LoseState Instance
//     {
//         get
//         {
//             if (m_Instance == null)
//             {
//                 new GM_LoseState();
//             }

//             return m_Instance;
//         }
//     }
//     public void Enter(GameManager _charState)
//     {
//         _charState.OnLoseEnter();
//     }

//     public void Execute(GameManager _charState)
//     {
//         _charState.OnLoseExecute();
//     }

//     public void Exit(GameManager _charState)
//     {
//         _charState.OnLoseExit();
//     }
// }
