using System;
using System.IO;
using System.Linq;
using System.Text;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 发布工具包
    /// </summary>
    public class PublishToolPackageEditorWindow : EditorWindow
    {
        private static string CHANGE_LOG_PATH = "Assets/XGameUnityTool/CHANGELOG.md";

        private static string PublishSettingPath => $"Assets/XGameUnityTool/Editor/Scripts/PublishSetting.asset";

        private static string PACKAGE_JSON_PATH = Paths.PACKAGE_JSON;

        private string version = "";

        private int versionCode = 0;

        private Vector2 _scroll;
        private OdinEditor _changeInfoEditor = null;

        private OdinEditor ChangeInfoEditor
        {
            get
            {
                if (_changeInfoEditor != null && _changeInfoEditor.target != ChangeLogInfo.Default)
                {
                    DestroyImmediate(_changeInfoEditor);
                }

                if (_changeInfoEditor == null)
                {
                    _changeInfoEditor = (OdinEditor)Editor.CreateEditor(ChangeLogInfo.Default, typeof(OdinEditor));
                }

                return _changeInfoEditor;
            }
        }

        private PublishToolPackageSetting _setting = null;

        private PublishToolPackageSetting PublishSetting
        {
            get
            {
                if (_setting == null)
                {
                    _setting = XGameEditorUtil.LoadOrCreate<PublishToolPackageSetting>(PublishSettingPath);
                }

                return _setting;
            }
        }

        [MenuItem("XGameUnityTool/Developer/Release toolkit")]
        private static void Open()
        {
            var win = GetWindow<PublishToolPackageEditorWindow>();
            win.minSize = new Vector2(800, 600);
            win.titleContent = new GUIContent("Release toolkit");
            win.Show();
        }

        private void OnEnable()
        {
            var today = DateTime.Today;
            var nY = today.Year;
            var nM = today.Month;
            var nD = today.Day;
            var nC = 1;

            if (GetCurrentVersionInfo(out var y, out var m, out var d, out var c))
            {
                if (today.Year == y && today.Month == m && today.Day == d)
                {
                    //同一天
                    nC = c + 1;
                }
            }

            var yCode = nY.ToString().PadLeft(4, '0');
            var mCode = nM.ToString().PadLeft(2, '0');
            var dCode = nD.ToString().PadLeft(2, '0');
            var cCode = nC.ToString().PadLeft(2, '0');
            //生成版本号
            versionCode = int.Parse($"{yCode}{mCode}{dCode}{cCode}");
            version = $"{nY}.{nM}.{nD}.{nC}";
            Debug.Log($"{versionCode}   {version}");
            Debug.Log($"工具版本号：{XGameEditorUtil.GetToolVersionCode()}");
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling || EditorApplication.isPlaying)
            {
                Close();
            }

            var temColor = GUI.color;
            GUI.color = Color.green;
            if (PublishSetting != null)
            {
                GUILayout.Label("发布参数设置");
                GUILayout.BeginHorizontal();
                GUILayout.Label("上传路径(只支持本地盘符)", GUILayout.Width(160));
                PublishSetting.UploadPath = GUILayout.TextField(PublishSetting.UploadPath);
                if (GUILayout.Button("保存", GUILayout.Width(40)))
                {
                    EditorUtility.SetDirty(PublishSetting);
                    AssetDatabase.SaveAssets();
                    GUI.FocusControl(null);
                }

                GUI.color = temColor;
                GUILayout.Space(15);
                GUILayout.EndHorizontal();
            }

            GUILayout.BeginHorizontal(GUILayout.Height(22));
            GUILayout.Label($"版本：{version}");
            GUILayout.EndHorizontal();
            SirenixEditorGUI.BeginHorizontalToolbar();
            if (GUILayout.Button("清空所有", GUILayout.Width(200)))
            {
                XGameEditorUtil.ShowMessageBox("清空所有？无法撤销！", () => { ChangeLogInfo.Default.ClearAll(); });
            }

            var last = GUILayoutUtility.GetLastRect();
            EditorGUI.DrawRect(last, Color.red * 0.3f);

            var txt = $"保存";
            // if (EditorUtility.IsDirty(ChangeLogInfo.Default))
            // {
            //     txt = $"*{txt}";
            // }

            if (GUILayout.Button(txt, GUILayout.Width(200)))
            {
                EditorUtility.SetDirty(ChangeLogInfo.Default);
                AssetDatabase.SaveAssets();
            }

            last = GUILayoutUtility.GetLastRect();
            EditorGUI.DrawRect(last, Color.green * 0.3f);

            if (GUILayout.Button("发布工具包"))
            {
                XGameEditorUtil.ShowMessageBox("是否发布？", PublishPackage);
            }

            last = GUILayoutUtility.GetLastRect();
            EditorGUI.DrawRect(last, Color.yellow * 0.3f);

            SirenixEditorGUI.EndHorizontalToolbar();
            _scroll = GUILayout.BeginScrollView(_scroll);
            ChangeInfoEditor.OnInspectorGUI();
            GUILayout.Space(150);
            GUILayout.EndScrollView();
            GUILayout.Space(20);
        }


        //获取当前版本信息
        private bool GetCurrentVersionInfo(out int year, out int month, out int day, out int code)
        {
            year = 0;
            month = 0;
            day = 0;
            code = 0;
            var lines = File.ReadAllLines(CHANGE_LOG_PATH);
            foreach (var line in lines)
            {
                if (line.StartsWith("## [") && line.Contains("]") && line.Contains("-"))
                {
                    var from = line.IndexOf("[");
                    var to = line.IndexOf("]");
                    if (from >= 0 && to >= 0)
                    {
                        var version = line.Substring(from + 1, to - from - 1);
                        var nums = version.Split('.');
                        if (nums.Length == 4)
                        {
                            year = int.Parse(nums[0]);
                            month = int.Parse(nums[1]);
                            day = int.Parse(nums[2]);
                            code = int.Parse(nums[3]);
                        }

                        return true;
                    }

                    break;
                }
            }

            return false;
        }

        private void PublishPackage()
        {
            //导出文件
            var folders = new[]
            {
                // @"Assets\Plugins\LitJson",
                @"Assets\Plugins\Sirenix",
                @"Assets\XGameUnityTool\Editor\BuildApp",
                @"Assets\XGameUnityTool\Editor\CodeTemplate",
                @"Assets\XGameUnityTool\Editor\Scripts",
                @"Assets\XGameUnityTool\Editor\ToolBox",
                // @"Assets\XGameUnityTool\Editor\Bat",
                @"Assets\XGameUnityTool\Editor\ZipTool",
                @"Assets\XGameUnityTool\Editor\LocalTest",
                @"Assets\XGameUnityTool\Res",
                @"Assets\XGameUnityTool\Runtime",
                // @"Assets\XGameUnityTool\Editor\MarkDownPreview",
                @"Assets\XGameUnityTool\CHANGELOG.md",
                @"Assets\XGameUnityTool\package.json",
                @"Assets/XGameUnityTool/Editor/Texture",
            };

            //检查文件目录
            var exportPath =
                EditorUtility.SaveFilePanel("导出为：", Application.dataPath, $"xgame-unity-tool-v{version}",
                    "unitypackage");
            if (string.IsNullOrWhiteSpace(exportPath))
            {
                return;
            }

            foreach (var path in folders)
            {
                if (!Directory.Exists(path) && !File.Exists(path))
                {
                    throw new Exception($"导出失败，找不到 {path}");
                }
            }

            #region 写入更新日志

            {
                var lines = File.ReadAllLines(CHANGE_LOG_PATH).ToList();
                var index = lines.FindIndex((a) =>
                {
                    if (a.StartsWith("# 更新日志"))
                    {
                        return true;
                    }

                    return false;
                });


                var sb = new StringBuilder();
                var today = DateTime.Today;
                var info = ChangeLogInfo.Default;
                sb.AppendLine("");
                sb.AppendLine(
                    $"## [{version}] - {today.Year}-{today.Month.ToString().PadLeft(2, '0')}-{today.Day.ToString().PadLeft(2, '0')}");
                if (info.Features != null && info.Features.Count > 0)
                {
                    sb.AppendLine("");
                    sb.AppendLine($"### 新增");
                    foreach (var element in info.Features)
                    {
                        sb.AppendLine($"* {element}");
                    }
                }

                if (info.Fixed != null && info.Fixed.Count > 0)
                {
                    sb.AppendLine("");
                    sb.AppendLine($"### 修复");
                    foreach (var element in info.Fixed)
                    {
                        sb.AppendLine($"* {element}");
                    }
                }

                if (info.Changed != null && info.Changed.Count > 0)
                {
                    sb.AppendLine("");
                    sb.AppendLine($"### 变更");
                    foreach (var element in info.Changed)
                    {
                        sb.AppendLine($"* {element}");
                    }
                }

                if (info.Refactored != null && info.Refactored.Count > 0)
                {
                    sb.AppendLine("");
                    sb.AppendLine($"### 优化");
                    foreach (var element in info.Refactored)
                    {
                        sb.AppendLine($"* {element}");
                    }
                }

                if (info.Removed != null && info.Removed.Count > 0)
                {
                    sb.AppendLine("");
                    sb.AppendLine($"### 移除");
                    foreach (var element in info.Removed)
                    {
                        sb.AppendLine($"* {element}");
                    }
                }

                sb.AppendLine("");


                if (index >= 0)
                {
                    lines.Insert(index + 1, sb.ToString());
                }

                File.WriteAllLines(CHANGE_LOG_PATH, lines);
                AssetDatabase.Refresh();
                Debug.Log("写入更新日志,成功");
                XGameEditorUtil.PingObject(CHANGE_LOG_PATH);
            }

            #endregion

            #region 写入版本号

            {
                var lines = File.ReadAllLines(PACKAGE_JSON_PATH);

                for (int i = 0; i < lines.Length; i++)
                {
                    var element = lines[i];
                    if (element.Contains("\"versionCode\":"))
                    {
                        lines[i] = $"  \"versionCode\":{versionCode},";
                    }

                    if (element.Contains("\"version\":"))
                    {
                        lines[i] = $"  \"version\":\"{version}\",";
                    }
                }

                File.WriteAllLines(PACKAGE_JSON_PATH, lines);
                AssetDatabase.Refresh();
                Debug.Log("写入package.json");
            }

            #endregion


            var packageName = Path.GetFileNameWithoutExtension(exportPath);
            var folder = $"{Path.GetDirectoryName(exportPath)}/{packageName}_update";
            var versionConfPath = $"{folder}/lastest.json";
            var changeLogPath = $"{folder}/CHANGELOG.md";
            var packageMoveTo = $"{folder}/{packageName}.unitypackage";
            XGameEditorUtil.CheckCreateFolder(folder);

            //生成versionConf
            File.Copy(CHANGE_LOG_PATH, changeLogPath, true);
            var updateVersion = new XGameEditorUtil.JsonUpdateVersion();
            updateVersion.url = $"{Paths.REMOTE_XGAME_UNTIY_TOOL_URL}/{packageName}.unitypackage";
            updateVersion.version = version;
            updateVersion.versionCode = versionCode;
            var contents = XJson.ToJson(updateVersion);
            File.WriteAllText(versionConfPath, contents, Encoding.UTF8);

            {
                var sb = new StringBuilder();
                sb.AppendLine($"\\\\192.168.31.169\\ubuntu\\nginx\\html");
                sb.AppendLine($"remote.xplaymobile.com:5004");
                sb.AppendLine($"账号：ubuntu");
                sb.AppendLine($"密码：xplay2018");
                // File.WriteAllText($"{folder}/发布说明.txt", sb.ToString(), Encoding.UTF8);
            }


            var uploadPath = $"{folder}/upload.bat";
            {
                var sb = new StringBuilder();
                sb.AppendLine(@$"xcopy /E /Y . ""{PublishSetting.UploadPath}""");
                sb.AppendLine($"pause");
                File.WriteAllText(uploadPath, sb.ToString(), Encoding.GetEncoding("GB2312"));
            }


            #region 写入重大修复

            {
                var info = ChangeLogInfo.Default;
                var filePath = $"{folder}/major_fixed.json";
                if (info.IsMajorFixed)
                {
                    var xj = new XJsonObject();
                    xj["versionCode"] = versionCode;
                    xj["content"] = info.MajorFixedContent;
                    File.WriteAllText(filePath, xj.ToJson(), Encoding.UTF8);
                }
            }

            #endregion


            //写入新版本标记
            File.WriteAllText(Paths.UPDATED_MARK, "");
            AssetDatabase.Refresh();

            //更新package版本号
            AssetDatabase.ExportPackage(folders, exportPath, ExportPackageOptions.Recurse);
            AssetDatabase.Refresh();
            if (File.Exists(exportPath))
            {
                if (File.Exists(packageMoveTo))
                {
                    File.Delete(packageMoveTo);
                }

                File.Move(exportPath, packageMoveTo);
            }

            //定位到文件
            EditorUtility.RevealInFinder(versionConfPath);
            XGameEditorUtil.ShowMessageBox($"导出成功,上传到云端？", () =>
            {
                if (File.Exists(uploadPath))
                {
                    XGameEditorUtil.RunBat(uploadPath);
                }
            });
            Close();

            if (XGameEditorUtil.IsDevelop())
            {
                Debug.Log("开发模式，删除updated.md");
                if (File.Exists(Paths.UPDATED_MARK))
                {
                    File.Delete(Paths.UPDATED_MARK);
                    AssetDatabase.Refresh();
                }
            }
        }
    }
}