using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace XGame
{
    //PlayAssetDelivery分包模式
    public enum PlayAssetDeliveryMode
    {
        InstallTime, //install-time
        FastFollow,  //fast-follow
        OnDemand,    //on-demand
    }

    /// <summary>
    /// pad分包设置
    /// </summary>
    [Serializable]
    public class PlayAssetDeliveryOption
    {
        [ValueDropdown("$AssetGroupOptions")]
        public Object AssetGroup = null;
        [ValueDropdown("$ModeOptions")]
        public PlayAssetDeliveryMode Mode = PlayAssetDeliveryMode.OnDemand;

        private static Object[] AssetGroupOptions()
        {
            if (AddressableReflection.HasModule() && AddressableReflection.Settings != null)
            {
                var result = new List<Object>();
                var guids = AssetDatabase.FindAssets("t:AddressableAssetGroup");
                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    result.Add(AssetDatabase.LoadAssetAtPath<Object>(path));
                }

                return result.ToArray();
            }

            Debug.Log("搜索不到aa资源组！！！！！");
            //TODO//反射获取
            return new Object[0];
        }

        private static IEnumerable ModeOptions = new ValueDropdownList<PlayAssetDeliveryMode>()
        {
            { "install-time", PlayAssetDeliveryMode.InstallTime },
            { "fast-follow", PlayAssetDeliveryMode.FastFollow },
            { "on-demand", PlayAssetDeliveryMode.OnDemand },
        };
    }
}