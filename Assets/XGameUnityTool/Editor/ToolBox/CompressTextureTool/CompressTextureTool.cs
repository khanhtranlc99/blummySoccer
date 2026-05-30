// using System;
// using System.Collections.Generic;
// using System.IO;
// using System.Reflection;
// using System.Threading.Tasks;
// using UnityEditor;
// using UnityEditor.Experimental.AssetImporters;
// using UnityEngine;
//
// namespace XGame
// {
//     public static class CompressTextureTool
//     {
//         private static string CONFIG_SAVE_PATH = "Assets/XGameUnityTool_Gen/CompressTextureTool/Editor";
//         private static Type _type_UnityEditor_TextureImporter = null;
//
//         private static Type Type_UnityEditor_TextureImporter
//         {
//             get
//             {
//                 if (_type_UnityEditor_TextureImporter == null)
//                 {
//                     _type_UnityEditor_TextureImporter = GetType("UnityEditor.TextureImporter");
//                 }
//
//                 return _type_UnityEditor_TextureImporter;
//             }
//         }
//
//         private static MethodInfo _method_RecommendedFormatsFromTextureTypeAndPlatform = null;
//
//         private static MethodInfo Method_RecommendedFormatsFromTextureTypeAndPlatform
//         {
//             get
//             {
//                 if (_method_RecommendedFormatsFromTextureTypeAndPlatform == null)
//                 {
//                     _method_RecommendedFormatsFromTextureTypeAndPlatform = Type_UnityEditor_TextureImporter.GetMethod(
//                         "RecommendedFormatsFromTextureTypeAndPlatform",
//                         BindingFlags.Static | BindingFlags.NonPublic);
//                 }
//
//                 return _method_RecommendedFormatsFromTextureTypeAndPlatform;
//             }
//         }
//
//         private static MethodInfo _method_GetSourceTextureInformation = null;
//
//         private static MethodInfo Method_GetSourceTextureInformation
//         {
//             get
//             {
//                 if (_method_GetSourceTextureInformation == null)
//                 {
//                     _method_GetSourceTextureInformation = Type_UnityEditor_TextureImporter.GetMethod(
//                         "GetSourceTextureInformation",
//                         BindingFlags.Instance | BindingFlags.NonPublic);
//                 }
//
//                 return _method_GetSourceTextureInformation;
//             }
//         }
//
//
//         private static Dictionary<string, TextureImporter> _importers = new Dictionary<string, TextureImporter>();
//         private static Dictionary<string, TextureImporter> _validImporters = new Dictionary<string, TextureImporter>();
//
//         private static Dictionary<TextureImporter, SourceTextureInformation> _sourceTextureInfos =
//             new Dictionary<TextureImporter, SourceTextureInformation>();
//
//
//         public static TextureImporter GetImporter(string guid)
//         {
//             if (_importers.TryGetValue(guid, out var match))
//             {
//                 return match;
//             }
//
//             return null;
//         }
//
//         //更新纹理数据库
//         public static async Task UpdateDatabase()
//         {
//             var guids = AssetDatabase.FindAssets("t:texture", new[] { "Assets" });
//             await UpdateDatabase(guids);
//         }
//
//         //增量更新数据库
//         public static async Task UpdateDatabase(string[] guids)
//         {
//             //生成GUI序号
//             var sortOrders = new Dictionary<string, int>();
//             for (int i = 0; i < guids.Length; i++)
//             {
//                 var guid = guids[i];
//                 sortOrders[guid] = i;
//             }
//
//             var hash = new HashSet<string>();
//             foreach (var element in guids)
//             {
//                 hash.Add(element);
//             }
//
//             var remove = new HashSet<string>();
//             foreach (var kv in _importers)
//             {
//                 if (hash.Contains(kv.Key))
//                 {
//                     continue;
//                 }
//
//                 remove.Add(kv.Key);
//             }
//
//             //剔除多余的部分
//             foreach (var element in remove)
//             {
//                 _importers.Remove(element);
//             }
//
//             //计算缺失的部分
//             var miss = new List<string>();
//             foreach (var element in guids)
//             {
//                 if (_importers.TryGetValue(element, out var match))
//                 {
//                     if (match != null)
//                     {
//                         continue;
//                     }
//                 }
//
//                 miss.Add(element);
//             }
//
//             //读取TextureImporter
//             var total = miss.Count;
//             var current = 0;
//             var counter = 0;
//             foreach (var element in miss)
//             {
//                 current += 1;
//                 counter += 1;
//                 var process = (float)current / total;
//                 var path = AssetDatabase.GUIDToAssetPath(element);
//                 _importers[element] = AssetImporter.GetAtPath(path) as TextureImporter;
//                 if (counter > 200)
//                 {
//                     counter = 0;
//                     EditorUtility.DisplayProgressBar("请稍等", $"读取纹理中...{process * 100}%", process);
//                     await Task.Yield();
//                 }
//             }
//
//             if (total > 0)
//             {
//                 EditorUtility.ClearProgressBar();
//             }
//
//             //更新有效的importer
//             _validImporters = new Dictionary<string, TextureImporter>();
//             foreach (var kv in _importers)
//             {
//                 var key = kv.Key;
//                 var importer = kv.Value;
//                 if (importer != null)
//                 {
//                     _validImporters[key] = importer;
//                 }
//             }
//
//             //更新配置
//             var configs = AssetDatabase.FindAssets("t:BuildTargetTextureCompressConfig", new[] { "Assets" });
//             foreach (var guid in configs)
//             {
//                 var path = AssetDatabase.GUIDToAssetPath(guid);
//                 var config = AssetDatabase.LoadAssetAtPath<BuildTargetTextureCompressConfig>(path);
//                 config.UpdateDatabase(_validImporters, sortOrders);
//             }
//         }
//
//         //创建配置
//         public static async void CreateBuildTargetTextureCompressConfig(BuildTarget target)
//         {
//             var config = ScriptableObject.CreateInstance<BuildTargetTextureCompressConfig>();
//             config.BuildTarget = target;
//             await UpdateDatabase();
//             config.UpdateDatabase(_validImporters);
//             //生成名称
//             var path =
//                 $"{CONFIG_SAVE_PATH}/{GUID.Generate().ToString()}.asset";
//             if (!Directory.Exists(CONFIG_SAVE_PATH))
//             {
//                 Directory.CreateDirectory(CONFIG_SAVE_PATH);
//                 AssetDatabase.Refresh();
//             }
//
//             AssetDatabase.CreateAsset(config, path);
//             AssetDatabase.Refresh();
//             EditorGUIUtility.PingObject(config);
//         }
//
//
//         //推荐格式类型
//         public static TextureImporterFormat[] GetRecommendedFormats(TextureImporter importer, BuildTarget target)
//         {
//             return Method_RecommendedFormatsFromTextureTypeAndPlatform.Invoke(null,
//                     new object[] { importer.textureType, target }) as
//                 TextureImporterFormat[];
//         }
//
//
//         //获取图片原始信息
//         public static SourceTextureInformation GetSourceTextureInformation(TextureImporter importer)
//         {
//             if (_sourceTextureInfos.TryGetValue(importer, out var match))
//             {
//                 if (match == null)
//                 {
//                     _sourceTextureInfos.Remove(importer);
//                 }
//                 else
//                 {
//                     return match;
//                 }
//             }
//
//             var info = Method_GetSourceTextureInformation?.Invoke(importer, null) as SourceTextureInformation;
//             _sourceTextureInfos.Add(importer, info);
//             return info;
//         }
//
//         private static Type GetType(string typeName)
//         {
//             var assemblies = AppDomain.CurrentDomain.GetAssemblies();
//             foreach (var assembly in assemblies)
//             {
//                 var type = assembly.GetType(typeName);
//                 if (type != null)
//                 {
//                     return type;
//                 }
//             }
//
//             return null;
//         }
//
//         public static void SaveAllConfig()
//         {
//             //更新配置
//             var configs = AssetDatabase.FindAssets("t:BuildTargetTextureCompressConfig", new[] { "Assets" });
//             var dirty = false;
//             foreach (var guid in configs)
//             {
//                 var path = AssetDatabase.GUIDToAssetPath(guid);
//                 var config = AssetDatabase.LoadAssetAtPath<BuildTargetTextureCompressConfig>(path);
//                 EditorUtility.SetDirty(config);
//                 dirty = true;
//             }
//
//             if (dirty)
//             {
//                 AssetDatabase.SaveAssets();
//             }
//         }
//
//         //转换
//         public static async void Apply(BuildTargetTextureCompressConfig config, HashSet<string> guids,
//             bool showDialog = true)
//         {
//             var items = new Dictionary<string, CompressItemData>();
//             foreach (var item in config.Items)
//             {
//                 items[item.Guid] = item;
//             }
//
//             var match = new HashSet<CompressItemData>();
//             foreach (var guid in guids)
//             {
//                 if (items.TryGetValue(guid, out var matchItem))
//                 {
//                     match.Add(matchItem);
//                 }
//             }
//
//             if (match.Count <= 0)
//             {
//                 Debug.Log("处理完毕，没有匹配项");
//                 return;
//             }
//
//             var buildTarget = config.BuildTarget;
//             var lastTime = 0d;
//             if (showDialog)
//             {
//                 if (!EditorUtility.DisplayDialog("提示", $"使用配置：{config}处理图片吗?\n有效条目：{match.Count}", "是"))
//                 {
//                     return;
//                 }
//
//                 var total = match.Count;
//                 var current = 0;
//                 foreach (var data in match)
//                 {
//                     current += 1;
//                     var process = (float)current / total;
//                     if ((EditorApplication.timeSinceStartup - lastTime) > 2f)
//                     {
//                         lastTime = EditorApplication.timeSinceStartup;
//                         Debug.Log($"请稍等...{(process * 100).ToString("F1")}%,剩余：{total - current}项");
//                         await Task.Yield();
//                     }
//
//                     var path = AssetDatabase.GUIDToAssetPath(data.Guid);
//                     var importer = AssetImporter.GetAtPath(path) as TextureImporter;
//                     if (importer == null)
//                     {
//                         continue;
//                     }
//
//                     var settings = importer.GetPlatformTextureSettings(buildTarget.ToString());
//                     var modify = false;
//                     //更新重载状态
//                     if (settings.overridden != data.Override)
//                     {
//                         settings.overridden = data.Override;
//                         modify = true;
//                     }
//
//                     //更新重载格式
//                     if (data.Override)
//                     {
//                         if (settings.format != data.ImporterFormat)
//                         {
//                             settings.format = data.ImporterFormat;
//                             modify = true;
//                         }
//
//                         if (settings.maxTextureSize != data.MaxSize)
//                         {
//                             settings.maxTextureSize = data.MaxSize;
//                             modify = true;
//                         }
//                     }
//
//                     //修改
//                     if (modify)
//                     {
//                         importer.SetPlatformTextureSettings(settings);
//                         importer.SaveAndReimport();
//                     }
//
//                     EditorUtility.ClearProgressBar();
//                 }
//
//                 Debug.Log("处理完毕");
//             }
//         }
//     }
// }