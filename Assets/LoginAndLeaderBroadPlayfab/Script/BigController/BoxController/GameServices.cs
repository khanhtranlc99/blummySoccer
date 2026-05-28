
using System;

using UniRx;
using UnityEngine;

using System.Threading.Tasks;



#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class GameServices : Singleton<GameServices>
{


   



    private static Texture2D _defaultAvatar;
    public static Texture2D defaultAvatar
    {
        get
        {
            if (_defaultAvatar == null)
                _defaultAvatar = Resources.Load("avatar") as Texture2D;
            return _defaultAvatar;
        }
    }

    private static Texture2D defaultFlag
    {
        get
        {
            return Resources.Load("flag") as Texture2D;
        }
    }



 
   
}
