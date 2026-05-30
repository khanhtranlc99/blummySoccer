using System.IO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// XGame 菜单窗口
    /// </summary>
    public class XGameMenuWindow : EditorWindow
    {
        private enum MenuItemType
        {
            General,
            TestParam,
        }

        private static MenuItemType _currMenuItemType = MenuItemType.General;
        private static Vector2 _scrollGeneral;
        private static Vector2 _scrollTestParam;
        private ToolPreferenceInspector _toolPreferenceEditor;

        private ToolPreferenceInspector ToolPreferenceEditor
        {
            get
            {
                if (_toolPreferenceEditor != null && _toolPreferenceEditor.target != ToolPreference.Global)
                {
                    DestroyImmediate(_toolPreferenceEditor);
                }

                if (_toolPreferenceEditor == null)
                {
                    _toolPreferenceEditor =
                        Editor.CreateEditor(ToolPreference.Global, typeof(ToolPreferenceInspector)) as
                            ToolPreferenceInspector;
                }

                return _toolPreferenceEditor;
            }
        }

        private OdinEditor _sdkPreferenceEditor;

        private OdinEditor SdkPreferenceEditor
        {
            get
            {
                if (_sdkPreferenceEditor != null && _sdkPreferenceEditor.target != SdkPreference.Global)
                {
                    DestroyImmediate(_sdkPreferenceEditor);
                }

                if (_sdkPreferenceEditor == null)
                {
                    _sdkPreferenceEditor =
                        Editor.CreateEditor(SdkPreference.Global, typeof(OdinEditor)) as
                            OdinEditor;
                }

                return _sdkPreferenceEditor;
            }
        }

        [MenuItem("XGameUnityTool/Channel Config", priority = 2)]
        public static void Open()
        {
            var window = GetWindow<XGameMenuWindow>();
            window.minSize = new Vector2(800, 500);
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(900, 650);
            window.titleContent = new GUIContent("Channel Config");
            window.Show();
            //检查重大修复
            XGameEditorUtil.AutoCheckMajorFixed(null, XGameEditorUtil.AutoCheckUpdate);
        }


        private void OnGUI()
        {
            var rect = new Rect(0, 0, position.width, position.height);
            var leftArea = rect;
            leftArea.width = 192;
            var center = leftArea.center;
            leftArea.height -= 4;
            leftArea.width -= 4;
            leftArea.center = center;
            DrawLeftMenuGUI(leftArea);
            var rectLeftLine = leftArea;
            rectLeftLine.width = 1;
            rectLeftLine.x = leftArea.xMax + 1;
            EditorGUI.DrawRect(rectLeftLine, Color.black);
            var rectContent = rect;
            rectContent.x = rectLeftLine.xMax;
            rectContent.width = rect.xMax - rectContent.x;
            center = rectContent.center;
            rectContent.width -= 4;
            rectContent.height -= 4;
            rectContent.center = center;
            DrawContentGUI(rectContent);
        }

        private void DrawLeftMenuGUI(Rect area)
        {
            GUILayout.BeginArea(area);
            if (DrawMenuButton(_currMenuItemType == MenuItemType.General, "Channel Config", null, GUILayout.Height(32)))
            {
                _currMenuItemType = MenuItemType.General;
            }

            if (DrawMenuButton(_currMenuItemType == MenuItemType.TestParam, "Test params (Unity env only)", null,
                    GUILayout.Height(32)))
            {
                _currMenuItemType = MenuItemType.TestParam;
            }

            // if (DrawMenuButton(_currMenuItemType == MenuItemType.BuildSetting, "打包配置", null, GUILayout.Height(32)))
            // {
            //     _currMenuItemType = MenuItemType.BuildSetting;
            // }
            GUILayout.EndArea();
        }

        private void DrawContentGUI(Rect rect)
        {
            GUILayout.BeginArea(rect);
            switch (_currMenuItemType)
            {
                case MenuItemType.General:
                {
                    _scrollGeneral = GUILayout.BeginScrollView(_scrollGeneral);
                    ToolPreferenceEditor.OnInspectorGUI();
                    GUILayout.Space(22);
                    GUILayout.EndScrollView();
                }
                    break;
                case MenuItemType.TestParam:
                {
                    _scrollTestParam = GUILayout.BeginScrollView(_scrollTestParam);
                    SdkPreferenceEditor.OnInspectorGUI();
                    GUILayout.Space(32);
                    GUILayout.EndScrollView();
                }
                    break;
            }

            GUILayout.EndArea();
        }


        [MenuItem("XGameUnityTool/Cleanup XGameOutPut Directory", priority = 4)]
        private static void ClearCache()
        {
            var folder = @"./XGameOutPut";
            if (Directory.Exists(folder))
            {
                DirectoryInfo info = new DirectoryInfo(folder);
                
                var size = DirSize(info);
                var mb = size / 1024f / 1024;
                XGameEditorUtil.ShowMessageBox($"Clean up {info.FullName} cache (size): {mb}MB", () =>
                {
                    Directory.Delete(info.FullName, true);
                    XGameEditorUtil.ShowMessageBox("Cleanup successful");
                });
            }
            else
            {
                XGameEditorUtil.ShowMessageBox("Cleanup successful");
            }
        }

        /// <summary>
        /// 大小
        /// </summary>
        public static long DirSize(DirectoryInfo d)
        {
            long size = 0;
            // Add file sizes.
            FileInfo[] fis = d.GetFiles();
            foreach (FileInfo fi in fis)
            {
                size += fi.Length;
            }

            // Add subdirectory sizes.
            DirectoryInfo[] dis = d.GetDirectories();
            foreach (DirectoryInfo di in dis)
            {
                size += DirSize(di);
            }

            return size;
        }


        [MenuItem("XGameUnityTool/Open XGameOutPut Directory",priority = 4)]
        private static void OpenCache()
        {
            var folder = @"./XGameOutPut";
            if (Directory.Exists(folder))
            {
                DirectoryInfo info = new DirectoryInfo(folder);
                EditorUtility.RevealInFinder(info.FullName);
            }
            else
            {
                Debug.Log("暂无XGameOutPut目录");
            }
        }

        // [MenuItem("XGameUnityTool/打开H5Audio缓存目录")]
        // private static void OpenH5AudioCacheFolder()
        // {
        //     var cachePath = $"{Application.persistentDataPath}/{XGameH5AudioUnity.CACHE_FOLDER_NAME}";
        //     if (Directory.Exists(cachePath))
        //     {
        //         EditorUtility.RevealInFinder(cachePath);
        //     }
        //     else
        //     {
        //         EditorUtility.RevealInFinder(Application.persistentDataPath);
        //     }
        // }

        

        private void OnDisable()
        {
            if (ToolPreference.Global != null && EditorUtility.IsDirty(ToolPreference.Global))
            {
                XGameEditorUtil.ShowMessageBox("There are unsaved changes, do you want to save them?",
                    () => { ToolPreferenceInspector.SaveToolPreference(); });
            }
        }


        private GUIStyle _colorBtnLabelStyle;

        private GUIStyle ColorBtnTxtStyle
        {
            get
            {
                if (_colorBtnLabelStyle == null)
                {
                    _colorBtnLabelStyle = new GUIStyle("label");
                    _colorBtnLabelStyle.margin = new RectOffset(2, 2, 2, 2);
                    _colorBtnLabelStyle.padding = new RectOffset(2, 2, 2, 2);
                    _colorBtnLabelStyle.border = new RectOffset(2, 2, 2, 2);
                    _colorBtnLabelStyle.alignment = TextAnchor.MiddleCenter;
                    _colorBtnLabelStyle.fixedHeight = 0;
                    _colorBtnLabelStyle.fixedWidth = 0;
                    _colorBtnLabelStyle.stretchHeight = false;
                    _colorBtnLabelStyle.stretchWidth = false;
                }

                return _colorBtnLabelStyle;
            }
        }


        private GUIStyle _menuBtnLabelStyle;

        private GUIStyle MenuBtnLabelStyle
        {
            get
            {
                if (_menuBtnLabelStyle == null)
                {
                    _menuBtnLabelStyle = new GUIStyle("label");
                    _menuBtnLabelStyle.margin = new RectOffset(2, 2, 2, 2);
                    _menuBtnLabelStyle.padding = new RectOffset(2, 2, 2, 2);
                    _menuBtnLabelStyle.border = new RectOffset(2, 2, 2, 2);
                    _menuBtnLabelStyle.padding = new RectOffset(2, 2, 2, 2);
                    _menuBtnLabelStyle.alignment = TextAnchor.MiddleLeft;
                    _menuBtnLabelStyle.fixedHeight = 0;
                    _menuBtnLabelStyle.fixedWidth = 0;
                    _menuBtnLabelStyle.stretchHeight = false;
                    _menuBtnLabelStyle.stretchWidth = false;
                }

                return _menuBtnLabelStyle;
            }
        }

        private GUIStyle _titleLabelStyle;

        private GUIStyle TitleLabelStyle
        {
            get
            {
                if (_titleLabelStyle == null)
                {
                    _titleLabelStyle = new GUIStyle("label");
                    _titleLabelStyle.margin = new RectOffset(2, 2, 2, 2);
                    _titleLabelStyle.padding = new RectOffset(2, 2, 2, 2);
                    _titleLabelStyle.border = new RectOffset(2, 2, 2, 2);
                    _titleLabelStyle.alignment = TextAnchor.MiddleLeft;
                    _titleLabelStyle.fixedHeight = 0;
                    _titleLabelStyle.fixedWidth = 0;
                    _titleLabelStyle.stretchHeight = false;
                    _titleLabelStyle.stretchWidth = false;
                    _titleLabelStyle.fontStyle = FontStyle.Bold;
                    _titleLabelStyle.fontSize = 16;
                    _titleLabelStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
                }

                return _titleLabelStyle;
            }
        }

        private bool DrawMenuButton(bool select, string content, Texture tex, params GUILayoutOption[] options)
        {
            var click = GUILayout.Button("", options);
            var last = GUILayoutUtility.GetLastRect();
            var fontStyle = MenuBtnLabelStyle.fontStyle;
            MenuBtnLabelStyle.normal.textColor = EditorGUIUtility.isProSkin ? Color.white : Color.black;
            if (select)
            {
                EditorGUI.DrawRect(last, Color.black * 0.6f);
                MenuBtnLabelStyle.fontStyle = FontStyle.Bold;
                MenuBtnLabelStyle.normal.textColor = Color.white;
            }

            last.x += 2;
            last.width -= 2;
            GUI.Label(last, new GUIContent(content, tex), MenuBtnLabelStyle);
            MenuBtnLabelStyle.fontStyle = fontStyle;
            return click;
        }
    }
}