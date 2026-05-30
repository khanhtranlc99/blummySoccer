// using UnityEditor;
// using UnityEngine;
//
// namespace XGame.BuildApp
// {
//     public class OnAppSettingCreated : AssetPostprocessor
//     {
//         //资源导入时
//         static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
//             string[] movedFromAssetPaths)
//         {
//             for (var index = 0; index < importedAssets.Length; index++)
//             {
//                 var importedAsset = importedAssets[index];
//
//                 var asset = AssetDatabase.LoadAssetAtPath<Object>(importedAsset);
//                 var type = asset.GetType();
//                 var isSubClass = type.IsSubclassOf(typeof(BuildAppSetting));
//                 if (isSubClass)
//                 {
//                     var setting = asset as BuildAppSetting;
//                     setting.OnCreate();
//                 }
//             }
//         }
//     }
// }

