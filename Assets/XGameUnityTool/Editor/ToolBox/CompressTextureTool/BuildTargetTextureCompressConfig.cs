// using System;
// using System.Collections.Generic;
// using System.Linq;
// using UnityEditor;
// using UnityEngine;
//
// namespace XGame
// {
//     /// <summary>
//     /// 压缩条目数据
//     /// </summary>
//     [Serializable]
//     public class CompressItemData
//     {
//         //guid
//         public string Guid;
//
//         //是否重写配置
//         public bool Override;
//
//         //压缩格式
//         public TextureImporterFormat ImporterFormat;
//
//         //MaxSize
//         public int MaxSize = 8192;
//
//         //序号
//         public int SortOrder = -1;
//
//         public void ChangeFormat(BuildTarget buildTarget, TextureImporterFormat format)
//         {
//             var path = AssetDatabase.GUIDToAssetPath(Guid);
//             var importer = AssetImporter.GetAtPath(path) as TextureImporter;
//             if (importer == null)
//             {
//                 throw new Exception($"找不到TextureImporter guid:{Guid}");
//             }
//
//             if (!Override)
//             {
//                 return;
//             }
//
//             if (format == TextureImporterFormat.Automatic)
//             {
//                 ImporterFormat = format;
//                 return;
//             }
//
//             if (!CompressTextureTool.GetRecommendedFormats(importer, buildTarget).Contains(format))
//             {
//                 Debug.Log($"切换失败，{importer.assetPath} ({buildTarget})不支持格式：{format}");
//             }
//             else
//             {
//                 //支持的格式
//                 ImporterFormat = format;
//             }
//         }
//
//
//         public static CompressItemData CreateInstance(TextureImporter importer, BuildTarget buildTarget)
//         {
//             var data = new CompressItemData();
//             var settings = importer.GetPlatformTextureSettings(buildTarget.ToString());
//             data.Guid = AssetDatabase.AssetPathToGUID(importer.assetPath);
//             data.Override = settings.overridden;
//             data.ImporterFormat = settings.format;
//             data.MaxSize = settings.maxTextureSize;
//             return data;
//         }
//     }
//
//
//     /// <summary>
//     /// 平台纹理压缩配置
//     /// </summary>
//     public class BuildTargetTextureCompressConfig : ScriptableObject
//     {
//         //打包平台
//         public BuildTarget BuildTarget;
//
//         //条目
//         public List<CompressItemData> Items = new List<CompressItemData>();
//
//         //更新数据库
//         public void UpdateDatabase(Dictionary<string, TextureImporter> importers,
//             Dictionary<string, int> sortOrder = null)
//         {
//             if (sortOrder == null)
//             {
//                 var guids = AssetDatabase.FindAssets("t:texture", new[] { "Assets" });
//                 sortOrder = new Dictionary<string, int>();
//                 for (var i = 0; i < guids.Length; i++)
//                 {
//                     var guid = guids[i];
//                     sortOrder[guid] = i;
//                 }
//             }
//
//             //排序
//             var remove = new HashSet<string>();
//             var dic = new Dictionary<string, CompressItemData>();
//             foreach (var item in Items)
//             {
//                 dic[item.Guid] = item;
//                 if (!importers.ContainsKey(item.Guid))
//                 {
//                     remove.Add(item.Guid);
//                 }
//             }
//
//             //补充新的
//             foreach (var kv in importers)
//             {
//                 var guid = kv.Key;
//                 var importer = kv.Value;
//                 if (!dic.ContainsKey(guid))
//                 {
//                     dic[guid] = CompressItemData.CreateInstance(importer, BuildTarget);
//                 }
//             }
//
//             //剔除多余的
//             foreach (var guid in remove)
//             {
//                 dic.Remove(guid);
//             }
//
//             //更新索引
//             Items.Clear();
//             var index = 0;
//
//             foreach (var data in dic.Values)
//             {
//                 if (sortOrder.TryGetValue(data.Guid, out var match))
//                 {
//                     data.SortOrder = match;
//                 }
//                 else
//                 {
//                     data.SortOrder = -1;
//                 }
//
//                 Items.Add(data);
//             }
//
//             Items.Sort((a, b) => { return a.SortOrder.CompareTo(b.SortOrder); });
//         }
//
//         public void Save()
//         {
//             EditorUtility.SetDirty(this);
//             AssetDatabase.SaveAssets();
//         }
//     }
// }