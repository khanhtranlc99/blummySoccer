using System;
using System.Reflection;
using UnityEngine;

namespace XGame
{
    public class XJson
    {
#if UNITY_EDITOR
        private static MethodInfo _methodFromJson = null;
        private static MethodInfo _methodToJson = null;

        private static void LoadMethodInfo()
        {
            if (_methodToJson == null || _methodFromJson == null)
            {
                var type = GetType("Newtonsoft.Json.JsonConvert");
                if (type == null)
                {
                    throw new Exception("找不到Newtonsoft.Json模块，请从XGameUnityTool菜单中安装或者在window/package manager里安装");
                    // Debug.Log("<color=#e74033>找不到Newtonsoft.Json，请从XGameUnityTool菜单中安装</color>");
                }
                else
                {
                    _methodToJson = type.GetMethod("SerializeObject", new[] { typeof(object) });
                    _methodFromJson = type.GetMethod("DeserializeObject", new[] { typeof(string), typeof(Type) });
                }
            }
        }

        private static MethodInfo GetMethodFromJson()
        {
            LoadMethodInfo();
            return _methodFromJson;
        }

        private static MethodInfo GetMethodToJson()
        {
            LoadMethodInfo();
            return _methodToJson;
        }

        private static Type GetType(string typeName)
        {
            Type result = Type.GetType(typeName);
            if (result != null)
            {
                return result;
            }

            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }

        public static T FromJson<T>(string json)
        {
            var method = GetMethodFromJson();
            if (method != null)
            {
                return (T)method.Invoke(null, new object[] { json, typeof(T) });
            }

            return JsonUtility.FromJson<T>(json);
        }

        public static string ToJson(object obj)
        {
            var method = GetMethodToJson();
            if (method != null)
            {
                return (string)method.Invoke(null, new[] { obj });
            }

            return JsonUtility.ToJson(obj);
        }
#endif

#if !UNITY_EDITOR
        public static T FromJson<T>(string json)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
        }

        public static string ToJson(object obj)
        {
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }
#endif
    }
}