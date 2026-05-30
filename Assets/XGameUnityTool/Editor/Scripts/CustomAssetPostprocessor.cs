using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    public class CustomAssetPostprocessor : AssetPostprocessor
    {
        
        private static string VivoUserDataFileName = "Assets/VIVO-GAME-SDK/Editor/QGGameConfig.asset";
        private static string XGameVivoUserDataFileName = "Assets/VIVO-GAME-SDK-UserData-XGame/QGGameConfig.asset";
        
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            var isDevelop = File.Exists("Assets/XGameUnityTool/develop.txt");
            // isDevelop = false; //用于测试
            if (!isDevelop)
            {
                foreach (string str in importedAssets)
                {
                    // Debug.Log("Reimported Asset: " + str);
                    HandleVIVOQGGameConfig(str);
                }
            }
            
        }

        private static void HandleVIVOQGGameConfig(string importedAssetPath)
        {
            if (importedAssetPath == VivoUserDataFileName)
            {
                Debug.Log($"导入的资源发现{VivoUserDataFileName}");
                ChangeVIVOQGGameConfig();
            }
        }
        
        private static void ChangeVIVOQGGameConfig()
        {
            
            if (!File.Exists(XGameVivoUserDataFileName))
            {
                Debug.Log($"{XGameVivoUserDataFileName}不存在");
                return;
            }

            if (!File.Exists(VivoUserDataFileName))
            {
                Debug.Log($"{VivoUserDataFileName}不存在");
                return;
            }
            
            Debug.Log($"执行更新 VIVO 的打包配置");

            var type = XGameEditorUtil.GetType("QGMiniGame.QGGameConfig");
            if (null == type)
            {
                Debug.Log("获取QGMiniGame.QGGameConfig类失败！");
                return;
            }
            dynamic xGameQgGameConfig = AssetDatabase.LoadAssetAtPath(VivoUserDataFileName,type);
            var buildSrc = null != xGameQgGameConfig ? xGameQgGameConfig.buildSrc : null;
            Debug.Log($"VIVO 的打包配置更新完成 xGameQgGameConfig对象是否为null={null == xGameQgGameConfig}，buildSrc={buildSrc}");
            if (null != xGameQgGameConfig && !string.IsNullOrWhiteSpace(buildSrc)) return;
            Debug.Log($"覆盖{VivoUserDataFileName}");
            File.Copy(XGameVivoUserDataFileName, VivoUserDataFileName, true);
        }
       
    }
}