// using System;
// using System.Collections.Generic;
// using Sirenix.OdinInspector;
// using Sirenix.Utilities.Editor;
// using UnityEditor;
// using UnityEngine;
//
// namespace XGame.BuildApp
// {
//     /// <summary>
//     /// 发布设置总览
//     /// </summary>
//     [Serializable]
//     public class BuildAppSettingOverview
//     {
//         private static Color COLOR_GREEN = new Color(1.35f, 1.89f, 1.52f);
//
//         [ReadOnly] [ListDrawerSettings(Expanded = true)]
//         //查询所有打包设置
//         public BuildAppSetting[] Settings;
//
//         public BuildAppSettingOverview()
//         {
//             Settings = LoadAll().ToArray();
//         }
//
//
//         public static List<BuildAppSetting> LoadAll()
//         {
//             var assetGuids = AssetDatabase.FindAssets($"t:{typeof(BuildAppSetting).Name}", new[] { "Assets" });
//             List<BuildAppSetting> settings = new List<BuildAppSetting>();
//
//             foreach (var guid in assetGuids)
//             {
//                 var setting = AssetDatabase.LoadAssetAtPath<BuildAppSetting>(AssetDatabase.GUIDToAssetPath(guid));
//                 if (setting != null)
//                 {
//                     settings.Add(setting);
//                 }
//             }
//
//             return settings;
//         }
//
//
//         [OnInspectorGUI, PropertyOrder(-1)]
//         private void OnInspectorGUI()
//         {
//             GUILayout.TextArea("说明:\n*创建配置*\n·右键->打包设置->XX\n", SirenixGUIStyles.DetailedMessageBox);
//
//             var temBgCol = GUI.backgroundColor;
//             GUI.backgroundColor = COLOR_GREEN;
//             if (GUILayout.Button("导入常用配置"))
//             {
//                 XGameEditorUtil.ImportBuildSetting();
//             }
//
//             GUI.backgroundColor = temBgCol;
//
//             SirenixEditorGUI.BeginHorizontalToolbar();
//             if (SirenixEditorGUI.ToolbarButton("新建: Android 发布设置"))
//             {
//                 var guid = GUID.Generate().ToString();
//                 var path = $"Assets/XGameUnityTool/Editor/Custom/安卓打包配置_{guid}.asset";
//                 var setting = XGameEditorUtil.CreateScriptableObject<XGameAndroidAppSetting>(path);
//                 EditorGUIUtility.PingObject(setting);
//             }
//
//             if (SirenixEditorGUI.ToolbarButton("新建: IOS 发布设置"))
//             {
//                 var guid = GUID.Generate().ToString();
//                 var path = $"Assets/XGameUnityTool/Editor/Custom/IOS打包配置_{guid}.asset";
//                 var setting = XGameEditorUtil.CreateScriptableObject<XGameIOSAppSetting>(path);
//                 EditorGUIUtility.PingObject(setting);
//             }
//
//             SirenixEditorGUI.EndHorizontalToolbar();
//         }
//     }
// }