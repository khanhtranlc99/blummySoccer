
// public class GM_DefaultState : IState<GameManager>
// {
//     private static GM_DefaultState m_Instance;
//     private GM_DefaultState()
//     {
//         if (m_Instance != null)
//         {
//             return;
//         }

//         m_Instance = this;
//     }
//     public static GM_DefaultState Instance
//     {
//         get
//         {
//             if (m_Instance == null)
//             {
//                 new GM_DefaultState();
//             }

//             return m_Instance;
//         }
//     }
//     public void Enter(GameManager _charState)
//     {
//         _charState.OnDefaultEnter();
//     }

//     public void Execute(GameManager _charState)
//     {
//         _charState.OnDefaultExecute();
//     }

//     public void Exit(GameManager _charState)
//     {
//         _charState.OnDefaultExit();
//     }
// }
