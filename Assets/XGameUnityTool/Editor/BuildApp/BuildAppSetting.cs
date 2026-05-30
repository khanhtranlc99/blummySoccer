using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace XGame.BuildApp
{
    /// <summary>
    /// 打包配置
    /// </summary>
    public class BuildAppSetting : SerializedScriptableObject
    {
        [Serializable]
        public struct CopySetting
        {
            [FolderPath] public string From;
            [FolderPath] public string To;
        }
        
        [FoldoutGroup("Publish Setting",expanded:true)]
        public AppChannel Channel;
        
        //平台
        [ReadOnly]
        [FoldoutGroup("Publish Setting")]
        public BuildTarget BuildTarget;

        [ValueDropdown("ScriptingImplementationOptions")]
        [FoldoutGroup("Publish Setting")]
        public ScriptingImplementation ScriptingImplementation = ScriptingImplementation.IL2CPP;

        [ShowIf("$StripEngineCodeShowInspector")]
        [FoldoutGroup("Publish Setting")]
        public bool StripEngineCode = true;

        //代码裁剪等级
        [FoldoutGroup("Publish Setting")]
        public ManagedStrippingLevel ManagedStrippingLevel = ManagedStrippingLevel.Low;


        //资源文件移动操作

        [FoldoutGroup("Resource Operation(deprecate)")] [FolderPath] [LabelText("Resource Removal Directory")] [ListDrawerSettings(Expanded = true)]
        public string[] RemoveFolders;

        [FoldoutGroup("Resource Operation(deprecate)")] [LabelText("Resource Replication Directory")] [TableList(AlwaysExpanded = true)]
        public CopySetting[] CopySettings;

        [FoldoutGroup("Version Info(deprecate)"), GUIColor(0.34f, 0.87f, 0.47f)] [LabelText("Iterative Version Number(no practical use)")] [LabelWidth(300)]
        public string VersionString = "1.0.0";

        [FoldoutGroup("Version Info(deprecate)"), GUIColor(0.34f, 0.87f, 0.47f)] [LabelText("Stage Package Type(no practical use)")] [LabelWidth(300)]
        public BuildType BuildType = BuildType.Release;

        #region 自检测

        private bool StripEngineCodeShowInspector
        {
            get { return ScriptingImplementation == ScriptingImplementation.IL2CPP; }
        }

        #endregion


        [ContextMenu("Apply")]
        public virtual void Use()
        {
            if (File.Exists(Paths.UPDATED_MARK))
            {
                File.Delete(Paths.UPDATED_MARK);
                AssetDatabase.Refresh();
            }

            GUI.FocusControl("");
            OnUse();
        }

        protected virtual void OnUse()
        {
            //TODO//
        }


        /// <summary>
        /// 应用后完成平台切换触发
        /// </summary>
        protected virtual void OnAfterSwitchPlateComplete()
        {
            //修改全局宏
            XGameBuildAppUtility.ModifyGlobalMacros(Channel);
            
            switch (BuildTarget)
            {
                case BuildTarget.Android:
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation);
                    break;
                case BuildTarget.iOS:
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.iOS, ScriptingImplementation);
                    break;
                case BuildTarget.WebGL:
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.WebGL, ScriptingImplementation);
                    break;
#if UNITY_OPENHARMONY
                case BuildTarget.OpenHarmony:
                    PlayerSettings.SetScriptingBackend(BuildTargetGroup.OpenHarmony, ScriptingImplementation);
                    break;  
#endif
                default:
                    throw new Exception("未绑定");
            }

            PlayerSettings.stripEngineCode = StripEngineCode;
            switch (BuildTarget)
            {
                case BuildTarget.Android:
                    PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel);
                    break;
                case BuildTarget.iOS:
                    PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.iOS, ManagedStrippingLevel);
                    break;
                case BuildTarget.WebGL:
                    PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, ManagedStrippingLevel);
                    break;
#if UNITY_OPENHARMONY
                case BuildTarget.OpenHarmony:
                    PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.OpenHarmony, ManagedStrippingLevel);
                    break;  
#endif
                default:
                    throw new Exception("未绑定");
            }


            //移动文件夹

            //删除旧资源
            foreach (var folder in RemoveFolders)
            {
                if (!string.IsNullOrWhiteSpace(folder))
                {
                    //删除资源文件夹
                    BuildAppUtil.DeleteFolder(folder);
                }
            }

            foreach (var setting in CopySettings)
            {
                if ((!string.IsNullOrWhiteSpace(setting.From) && !string.IsNullOrWhiteSpace(setting.To)))
                {
                    //复制文件夹到目录
                    BuildAppUtil.CopyToAndReplaceFolder(setting.From, setting.To);
                }
            }
        }

        //TODO//触发应用完成回调

        private ScriptingImplementation[] ScriptingImplementationOptions =>
            GetScriptingImplementationOptions();

        protected virtual ScriptingImplementation[] GetScriptingImplementationOptions()
        {
            return new[]
            {
                ScriptingImplementation.WinRTDotNET,
                ScriptingImplementation.Mono2x,
                ScriptingImplementation.IL2CPP,
            };
        }


        [ContextMenu("Publish")]
        public void Publish()
        {
            if (File.Exists(Paths.UPDATED_MARK))
            {
                XGameEditorUtil.ShowMessageBox("The plugin update has not been executed before. Do you want to execute the 'Apply'?", Use);
                return;
            }

            OnPublish();
        }

        //发布
        protected virtual void OnPublish()
        {
            EditorPreference.Global.currentChannel = Channel;
            EditorPreference.Global.Save();

            Debug.Log($"update AppConfig, Channel={Channel.ToString()}, VersionString={VersionString}, BuildType={BuildType.ToString()}");
            XGameBuildAppUtility.CreateAppConfig(Channel,
                VersionString, BuildType);
        }

        [ContextMenu("Build AA Resources")]
        //构建AA资源
        public virtual void BuildAA()
        {
        }

        public void InvokeSubmitBefore(BuildAppSetting data)
        {
            var matchs = new List<Type>();
            var baseClass = typeof(OnSubmitComplete);
            //反射
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && type != baseClass && type.IsSubclassOf(baseClass))
                    {
                        // Debug.Log(type.FullName);
                        matchs.Add(type);
                    }
                }
            }

            //触发
            foreach (var match in matchs)
            {
                dynamic instance = Activator.CreateInstance(match);
                instance.OnBeforeSubmit(data);
            }
        }

        //触发应用完毕回调
        public void InvokeSubmitComplete(BuildAppSetting data)
        {
            var matchs = new List<Type>();
            var baseClass = typeof(OnSubmitComplete);
            //反射
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && type != baseClass && type.IsSubclassOf(baseClass))
                    {
                        // Debug.Log(type.FullName);
                        matchs.Add(type);
                    }
                }
            }

            //触发
            foreach (var match in matchs)
            {
                dynamic instance = Activator.CreateInstance(match);
                instance.OnSubmit(data);
            }
        }

        //触发发布完毕回调
        public void InvokePublishComplete(PublishResult data)
        {
            //反射
            var matchs = new List<Type>();
            var baseClass = typeof(OnPublishComplete);
            //反射
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (type.IsClass && type != baseClass && type.IsSubclassOf(baseClass))
                    {
                        // Debug.Log(type.FullName);
                        matchs.Add(type);
                    }
                }
            }

            //触发完毕回调
            foreach (var match in matchs)
            {
                dynamic instance = Activator.CreateInstance(match);
                instance.OnPublish(data);
            }
        }
        
        
        
        [OnInspectorGUI]
        protected void OnExtensionGUI()
        {
            switch (Channel)
            {
                case AppChannel.Douyin_XSDK_Android:
                case AppChannel.Douyin_XSDK_IOS:
                    DrawByteDanceMiniGameGUI();
                    break;
                case AppChannel.WeChat_XSDK:
                    DrawWeChatGameGUI();
                    break;
                case AppChannel.Huawei_XSDK:
                    DrawHuaweiKuaiGameGUI();
                    break;
                case AppChannel.Bilibili_XSDK:
                    DrawBilibiliGUI();
                    break;
                case AppChannel.Kuaishou_XSDK_Android:
                case AppChannel.Kuaishou_XSDK:
                    DrawKuaishouMiniGameGUI();
                    break;
            }
        }

        private void DrawWeChatGameGUI()
        {
            if (ToolPreference.Global.OpenWxGe && !ToolPreference.Global.WxGeAutoVersionFlag)
            {
                GUILayout.Space(10);
                GUILayout.Label("引力引擎 快捷设置", ToolPreference.BoldLabel);
                var tem = ToolPreference.Global.WxGeVersion;
                GUILayout.BeginHorizontal();
                ToolPreference.Global.WxGeVersion =
                    EditorGUILayout.IntField("APP版本号：", ToolPreference.Global.WxGeVersion);
                if (tem != ToolPreference.Global.WxGeVersion)
                {
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.WxGeVersion} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                if (GUILayout.Button("版本+1", GUILayout.Width(100)))
                {
                    ToolPreference.Global.WxGeVersion += 1;
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.WxGeVersion} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                GUILayout.EndHorizontal();
            }
        }
        
        
        private void DrawKuaishouMiniGameGUI()
        {
            if (ToolPreference.Global.OpenKuaishouGe && !ToolPreference.Global.KuaishouGeAutoVersionCode)
            {
                GUILayout.Space(10);
                GUILayout.Label("引力引擎 快捷设置", ToolPreference.BoldLabel);
                var tem = ToolPreference.Global.KuaishouGeVersionCode;
                GUILayout.BeginHorizontal();
                ToolPreference.Global.KuaishouGeVersionCode =
                    EditorGUILayout.IntField("APP版本号：", ToolPreference.Global.KuaishouGeVersionCode);
                if (tem != ToolPreference.Global.KuaishouGeVersionCode)
                {
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.KuaishouGeVersionCode} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                if (GUILayout.Button("版本+1", GUILayout.Width(100)))
                {
                    ToolPreference.Global.KuaishouGeVersionCode += 1;
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.KuaishouGeVersionCode} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                GUILayout.EndHorizontal();
            }
        }
        
        private void DrawHuaweiKuaiGameGUI()
        {
            if (ToolPreference.Global.OpenGeHuaweiKuaiGame && !ToolPreference.Global.GeAutoVersionCodeHuaweiKuaiGame)
            {
                GUILayout.Space(10);
                GUILayout.Label("引力引擎 快捷设置", ToolPreference.BoldLabel);
                var tem = ToolPreference.Global.GeVersionCodeHuaweiKuaiGame;
                GUILayout.BeginHorizontal();
                ToolPreference.Global.GeVersionCodeHuaweiKuaiGame =
                    EditorGUILayout.IntField("APP版本号：", ToolPreference.Global.GeVersionCodeHuaweiKuaiGame);
                if (tem != ToolPreference.Global.GeVersionCodeHuaweiKuaiGame)
                {
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.GeVersionCodeHuaweiKuaiGame} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                if (GUILayout.Button("版本+1", GUILayout.Width(100)))
                {
                    ToolPreference.Global.GeVersionCodeHuaweiKuaiGame += 1;
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.GeVersionCodeHuaweiKuaiGame} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                GUILayout.EndHorizontal();
            }
        }

        private void DrawBilibiliGUI()
        {
            if (ToolPreference.Global.OpenBilibiliGe && !ToolPreference.Global.BilibiliGeAutoVersionFlag)
            {
                GUILayout.Space(10);
                GUILayout.Label("引力引擎 快捷设置", ToolPreference.BoldLabel);
                var tem = ToolPreference.Global.BilibiliGeVersion;
                GUILayout.BeginHorizontal();
                ToolPreference.Global.BilibiliGeVersion =
                    EditorGUILayout.IntField("APP版本号：", ToolPreference.Global.BilibiliGeVersion);
                if (tem != ToolPreference.Global.BilibiliGeVersion)
                {
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.BilibiliGeVersion} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                if (GUILayout.Button("版本+1", GUILayout.Width(100)))
                {
                    ToolPreference.Global.BilibiliGeVersion += 1;
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.BilibiliGeVersion} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                GUILayout.EndHorizontal();
            }
        }
        
        private void DrawByteDanceMiniGameGUI()
        {
            if (ToolPreference.Global.OpenByteDanceGe && !ToolPreference.Global.ByteDanceGeAutoVersionCode)
            {
                GUILayout.Space(10);
                GUILayout.Label("引力引擎 快捷设置", ToolPreference.BoldLabel);
                var tem = ToolPreference.Global.ByteDanceGeVersionCode;
                GUILayout.BeginHorizontal();
                ToolPreference.Global.ByteDanceGeVersionCode =
                    EditorGUILayout.IntField("APP版本号：", ToolPreference.Global.ByteDanceGeVersionCode);
                if (tem != ToolPreference.Global.ByteDanceGeVersionCode)
                {
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.ByteDanceGeVersionCode} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                if (GUILayout.Button("版本+1", GUILayout.Width(100)))
                {
                    ToolPreference.Global.ByteDanceGeVersionCode += 1;
                    EditorUtility.SetDirty(ToolPreference.Global);
                    AssetDatabase.SaveAssets();
                    XGameEditorUtil.Log($"更新引力引擎版本号-->{ToolPreference.Global.ByteDanceGeVersionCode} 成功",
                        XGameEditorUtil.LogColor.Success);
                }

                GUILayout.EndHorizontal();
            }
        }
        
        
    }
}