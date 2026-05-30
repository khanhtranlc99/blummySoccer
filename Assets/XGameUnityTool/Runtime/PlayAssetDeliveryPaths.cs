using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    [Preserve]
    public static class PlayAssetDeliveryPaths
    {
        public static string RuntimePathRoot = $"{Application.persistentDataPath}/pad_assets";
        private static string _runtimePath = null;

        public static string RuntimePath
        {
            get
            {
                if (_runtimePath == null)
                {
                    _runtimePath = GetAddressablesDefaultRuntimePath();
                    var release = false;
#if !UNITY_EDITOR && UNITY_ANDROID
                    release = true;
#endif
                    var support = release && (AppConfig.CHANNEL == AppChannel.XMYGoogle || AppConfig.CHANNEL == AppChannel.Google_Log_SDK);
                    if (support)
                    {
                        _runtimePath = $"{RuntimePathRoot}/aa";
                    }
                }

                return _runtimePath;
            }
        }

        private static string _buildPath = null;

        public static string BuildPath
        {
            get
            {
                if (_buildPath == null)
                {
                    _buildPath = GetAddressablesDefaultBuildPath();
                }

                return _buildPath;
            }
        }

        private static string GetAddressablesDefaultRuntimePath()
        {
            var type = GetTyp("UnityEngine.AddressableAssets.Addressables");
            var property = type.GetProperty("RuntimePath", BindingFlags.Static | BindingFlags.Public);
            return property.GetValue(null) as string;
        }

        private static string GetAddressablesDefaultBuildPath()
        {
            var type = GetTyp("UnityEngine.AddressableAssets.Addressables");
            var property = type.GetProperty("BuildPath", BindingFlags.Static | BindingFlags.Public);
            return property.GetValue(null) as string;
        }

        private static Type GetTyp(string fullName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(fullName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }
    }
}