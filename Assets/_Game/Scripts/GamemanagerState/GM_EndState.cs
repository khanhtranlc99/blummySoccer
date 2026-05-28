// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class GM_EndState : IState<GameManager>
// {
//     private static GM_EndState m_Instance;
//     private GM_EndState()
//     {
//         if (m_Instance != null)
//         {
//             return;
//         }

//         m_Instance = this;
//     }
//     public static GM_EndState Instance
//     {
//         get
//         {
//             if (m_Instance == null)
//             {
//                 new GM_EndState();
//             }

//             return m_Instance;
//         }
//     }
//     public void Enter(GameManager _charState)
//     {
//         _charState.OnEndEnter();
//     }

//     public void Execute(GameManager _charState)
//     {
//         _charState.OnEndExecute();
//     }

//     public void Exit(GameManager _charState)
//     {
//         _charState.OnEndExit();
//     }
// }
