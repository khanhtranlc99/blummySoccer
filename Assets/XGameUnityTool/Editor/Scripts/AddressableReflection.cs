using System;
using System.Collections.Generic;
using System.Reflection;

namespace XGame
{
    /// <summary>
    /// 反射AA
    /// </summary>
    public class AddressableReflection
    {
        public const string TYPE_NAME_AddressableAssetSettings =
            "UnityEditor.AddressableAssets.Settings.AddressableAssetSettings";

        public const string TYPE_NAME_BundledAssetGroupSchema =
            "UnityEditor.AddressableAssets.Settings.GroupSchemas.BundledAssetGroupSchema";

        private const string TYPE_NAME_AddressableAssetGroup =
            "UnityEditor.AddressableAssets.Settings.AddressableAssetGroup";

        private const string TYPE_NAME_ProfileValueReference =
            "UnityEditor.AddressableAssets.Settings.ProfileValueReference";


        private static bool? _hasModule = null;
        private static dynamic _settings = null;

        private static Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public static Type TypeOfAddressableAssetGroup => GetType(TYPE_NAME_AddressableAssetGroup);

        public static Type TypeOfBundledAssetGroupSchema => GetType(TYPE_NAME_BundledAssetGroupSchema);

        public static Type TypeOfProfileValueReference => GetType(TYPE_NAME_ProfileValueReference);

        //是否开启addressable模块
        public static bool HasModule()
        {
            if (_hasModule == null)
            {
                _hasModule = GetType("UnityEngine.AddressableAssets.Addressables") != null;
            }

            return _hasModule.Value;
        }

        //是否有默认设置
        public static bool HasDefaultSettings()
        {
            return Settings != null;
        }

        /// <summary>
        /// UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject.Settings实例
        /// </summary>
        public static dynamic Settings
        {
            get
            {
                if (_settings == null)
                {
                    var type = GetType("UnityEditor.AddressableAssets.AddressableAssetSettingsDefaultObject");
                    if (type != null)
                    {
                        var property = type.GetProperty("Settings", BindingFlags.Public | BindingFlags.Static);
                        _settings = property.GetValue(null);
                    }
                }

                return _settings;
            }
        }

        //获取类型
        public static Type GetType(string type)
        {
            if (!_types.ContainsKey(type))
            {
                _types[type] = XGameEditorUtil.GetType(type);
            }

            return _types[type];
        }

        //执行静态方法
        public static void CallStaticMethod(string typeName, string methodName, BindingFlags flags, object[] args,
            int parameters = -1)
        {
            var type = GetType(typeName);
            if (type != null)
            {
                MethodInfo method = null;
                var methods = type.GetMethods();
                if (parameters >= 0)
                {
                    foreach (var e in methods)
                    {
                        if (e.Name == methodName && parameters == e.GetParameters().Length)
                        {
                            method = e;
                            break;
                        }
                    }
                }
                else
                {
                    method = type.GetMethod(methodName, flags);
                }

                if (method != null)
                {
                    method.Invoke(null, args);
                }
            }
        }


        //反射获取方法
        public static MethodInfo GetMethod(string typeName, string methodName, BindingFlags flags,
            int parameters = -1)
        {
            MethodInfo method = null;
            var type = GetType(typeName);
            if (type != null)
            {
                var methods = type.GetMethods();
                if (parameters >= 0)
                {
                    foreach (var e in methods)
                    {
                        if (e.Name == methodName && parameters == e.GetParameters().Length)
                        {
                            method = e;
                            return method;
                        }
                    }
                }
                else
                {
                    method = type.GetMethod(methodName, flags);
                }
            }

            return method;
        }
    }
}