using System;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;

namespace XGame.BuildApp
{
    /// <summary>
    /// Build / publish settings window.
    /// </summary>
    public class BuildAppSettingWindow : EditorWindow
    {
        public static Color ColorGreen = new Color(0.33f, 0.55f, 0.33f, 1f);
        public static Color ColorCyan = new Color(0.33f, 0.55f, 0.55f, 1f);
        public static Color ColorYellow = new Color(0.55f, 0.53f, 0.34f, 1f);
        public static Color ColorRed = new Color(0.7f, 0.19f, 0.15f, 1f);
        public static Color ColorOrange = new Color(0.78f, 0.6f, 0.06f, 1f);
        public static Color ColorBlue = new Color(0.14f, 0.74f, 0.94f, 1f);

        public static Texture GetSettingIcon(AppChannel channel)
        {
            switch (channel)
            {
                case AppChannel.Mar:
                case AppChannel.Android_Light:
                case AppChannel.Test:
                    return EditorTextures.icon_android;
                case AppChannel.Google:
                case AppChannel.XMYGoogle:
                case AppChannel.Google_Log_SDK:
                    return EditorTextures.icon_googleplay;
                case AppChannel.IOSInland:
                case AppChannel.IOSOverseas:
                case AppChannel.IOS_XGUG_China:
                case AppChannel.IOS_XGUG_Sea:
                    return EditorTextures.icon_ios;
                case AppChannel.ByteDanceMiniGame:
                case AppChannel.ByteDanceMiniGameIOS:
                case AppChannel.Douyin_XSDK_Android:
                case AppChannel.Douyin_XSDK_IOS:
                    return EditorTextures.icon_douyin;
                case AppChannel.WeChat:
                case AppChannel.WeChat_ASC:
                case AppChannel.WeChat_XSDK:
                    return EditorTextures.icon_wechat;
                case AppChannel.OppoMini:
                case AppChannel.Oppo_ASC:
                case AppChannel.Oppo_XSDK:
                    return EditorTextures.icon_oppo;
                case AppChannel.VivoMini:
                case AppChannel.Vivo_ASC:
                case AppChannel.Vivo_XSDK:
                    return EditorTextures.icon_vivo;
                case AppChannel.Kuaishou_XSDK:
                case AppChannel.Kuaishou_XSDK_Android:
                    return EditorTextures.icon_kuaishou;
                case AppChannel.Huawei_XSDK:
                case AppChannel.OpenHarmony_Light:
                    return EditorTextures.icon_huawei;
                case AppChannel.Bilibili_XSDK:
                    return EditorTextures.icon_bilibili;
            }

            return null;
        }

        private static int LeftGUIWidth = 226;

        private static string _CacheSelect = null;
        private static string CACHE_SELECT_SAVE_FILE = ".xgameunitytool/cacheselectsetting.bin";

        private static string KEY_SELECT_BUILD_APP_SETTING_INDEX =>
            $"XGAME_KEY_SELECT_BUILD_APP_SETTING_INDEX-{Application.dataPath}";

        //配置
        private List<BuildAppSetting> _settings = new List<BuildAppSetting>();

        //菜单
        private GUIContent[] _options;

        private Vector2 _scroll;

        private Vector2 _scrollSetting;

        //选择索引
        private static int SelectOptionIndex
        {
            get { return EditorPrefs.GetInt(KEY_SELECT_BUILD_APP_SETTING_INDEX, 0); }
            set { EditorPrefs.SetInt(KEY_SELECT_BUILD_APP_SETTING_INDEX, value); }
        }

        private OdinEditor _editor;

        private static BuildAppSetting _currentSelect;

        private bool _clickApply = false;
        private bool _clickBuildAA = false;
        private bool _clickPublish = false;

        [MenuItem("XGameUnityTool/Publish", priority = 1)]
        public static void Open()
        {
            var win = GetWindow<BuildAppSettingWindow>();
            win.titleContent = new GUIContent("Publish");
            win.minSize = new Vector2(860, 600);
            win.Show();
            //检查重大修复
            XGameEditorUtil.AutoCheckMajorFixed(null, XGameEditorUtil.AutoCheckUpdate);
        }

        protected void OnEnable()
        {
            // base.OnEnable();
            TryLoadCacheSelect();
            LoadConfigs();
            if (!string.IsNullOrEmpty(_CacheSelect))
            {
                if (_settings != null)
                {
                    foreach (var item in _settings)
                    {
                        if (item.name == _CacheSelect)
                        {
                            _currentSelect = item;
                            break;
                        }
                    }
                }
            }
        }

        private void TryLoadCacheSelect()
        {
            if (_CacheSelect == null)
            {
                if (File.Exists(CACHE_SELECT_SAVE_FILE))
                {
                    _CacheSelect = File.ReadAllText(CACHE_SELECT_SAVE_FILE);
                }
            }
        }

// #if UNITY_2021_1_OR_NEWER
//         protected override void OnImGUI()
// #else
        protected void OnGUI()
// #endif
        {
            // if (EditorApplication.isCompiling)
            // {
            //     GUILayout.Label("编译中，请稍等...");
            //     return;
            // }
            
            var rect = new Rect(0, 0, position.width, position.height);
            var center = rect.center;
            rect.width -= 8;
            rect.height -= 8;
            rect.center = center;

            var rectLeft = rect;
            rectLeft.width = LeftGUIWidth;
            center = rectLeft.center;
            rectLeft.width -= 4;
            rectLeft.height -= 4;
            rectLeft.center = center;
            if (rectLeft.width > 0 && rectLeft.height > 0)
            {
                GUILayout.BeginArea(rectLeft);
                DrawLeftMenuGUI(rectLeft);
                GUILayout.EndArea();
            }
            var rectLeftLine = rectLeft;
            rectLeftLine.width = 1;
            rectLeftLine.x = rectLeft.xMax;
            EditorGUI.DrawRect(rectLeftLine, Color.black);

            var rectRight = rect;
            rectRight.width = rect.width - LeftGUIWidth;
            rectRight.x = rect.xMax - rectRight.width;
            center = rectRight.center;
            rectRight.width -= 4;
            rectRight.height -= 4;
            rectRight.center = center;
            rectRight.height -= 60;
            if (rectRight.width > 0 && rectRight.height > 0)
            {
                GUILayout.BeginArea(rectRight);
                DrawRightGUI(rectRight);
                GUILayout.EndArea();
            }

            var rectBottom = rectRight;
            rectBottom.height = rect.yMax - rectRight.yMax;
            rectBottom.height -= 8;
            rectBottom.y = rectRight.yMax;
            rectBottom.y += 4;
            center = rectBottom.center;
            EditorGUI.DrawRect(rectBottom, Color.black * 0.2f);
            rectBottom.width -= 8;
            rectBottom.height -= 8;
            rectBottom.center = center;
            if (rectBottom.width > 0 && rectBottom.height > 0)
            {
                GUILayout.BeginArea(rectBottom);
                DrawBottomGUI(rectBottom);
                GUILayout.EndArea();
            }
        }

        protected void OnFocus()
        {
            LoadConfigs();
            Repaint();
        }

        private void LoadConfigs()
        {
            _settings = new List<BuildAppSetting>();
            var guids = AssetDatabase.FindAssets($"t:{typeof(BuildAppSetting).FullName}", new[] { "Assets" });
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                var setting = AssetDatabase.LoadAssetAtPath<BuildAppSetting>(path);
                if (setting != null)
                {
                    _settings.Add(setting);
                }
            }


            SelectOptionIndex = EditorPrefs.GetInt(KEY_SELECT_BUILD_APP_SETTING_INDEX, 0);
            //加载打包配置
            _settings = new List<BuildAppSetting>();
            var settingsPaths =
                AssetDatabase.FindAssets($"t:{typeof(BuildAppSetting).Name}", new string[] { "Assets" });
            foreach (var path in settingsPaths)
            {
                var setting = AssetDatabase.LoadAssetAtPath<BuildAppSetting>(AssetDatabase.GUIDToAssetPath(path));
                _settings.Add(setting);
            }

            _options = new GUIContent[_settings.Count];
            for (int i = 0; i < _settings.Count; i++)
            {
                var setting = _settings[i];
                _options[i] = new GUIContent($"{setting.name}");
            }
        }

        //绘制左侧GUI
        protected void DrawLeftMenuGUI(Rect area)
        {
            GUILayout.BeginHorizontal();
            if (DrawColorButton("Import packaging config", ColorGreen, GUILayout.Height(32)))
            {
                XGameEditorUtil.ImportBuildSetting();
            }

            if (DrawColorButton("Refresh", ColorCyan, GUILayout.Height(32), GUILayout.Width(60)))
            {
                LoadConfigs();
                Repaint();
            }

            GUILayout.EndHorizontal();

            // if (DrawColorButton("Lack of packaging config?", ColorOrange, GUILayout.Height(32)))
            // {
            //     XGameEditorUtil.ImportBuildSetting();
            // }

            // if (DrawColorButton("Corresponding relationship of packaging \n configuration channels", ColorRed, GUILayout.Height(32)))
            // {
            //     Application.OpenURL(
            //         "https://qu2tef36bb.feishu.cn/docx/Vasjd7bhOoNqMHxcAUCcMVrGnNg#FCs9dZEMboi3NixQ1EecN2flnSf");
            // }

            if (DrawColorButton("Documentation", ColorBlue, GUILayout.Height(32)))
            {
                Application.OpenURL(
                    "https://qu2tef36bb.feishu.cn/docx/Vasjd7bhOoNqMHxcAUCcMVrGnNg");
            }

            GUILayout.Space(12f);
            _scrollSetting = GUILayout.BeginScrollView(_scrollSetting);
            if (_settings != null)
            {
                foreach (var setting in _settings)
                {
                    if (setting == null)
                    {
                        continue;
                    }

                    var xGameAppSetting = setting as IXGameAppSetting;
                    var isSelect = setting == _currentSelect;
                    var tex = xGameAppSetting == null ? null : GetSettingIcon(xGameAppSetting.GetChannel);
                    if (DrawMenuButton(isSelect, setting.name, tex, GUILayout.Height(26)))
                    {
                        _currentSelect = setting;
                        WriteCacheSelectSetting();
                    }
                }
            }

            GUILayout.EndScrollView();
        }

        protected void DrawRightGUI(Rect area)
        {
            if (_settings != null)
            {
                BuildAppSetting selectSetting = _currentSelect;

                if (selectSetting == null)
                {
                    var txt = "Import common build presets from the left, then refresh...";
                    if (_settings != null && _settings.Count > 0)
                    {
                        txt = "No preset selected. Choose one from the list on the left.";
                    }

                    GUILayout.Label(txt);
                }

                if (selectSetting != null)
                {
                    DrawBoldTitle(selectSetting.name, GUILayout.Height(32));
                }

                _scroll = GUILayout.BeginScrollView(_scroll);
                //绘制选中
                if (selectSetting != null)
                {
                    var tempGuiOn = GUI.enabled;
                    GUI.enabled = false;
                    SirenixEditorFields.UnityObjectField(selectSetting, selectSetting.GetType(), false);
                    GUI.enabled = tempGuiOn;
                    if (_editor != null && _editor.target != selectSetting)
                    {
                        DestroyImmediate(_editor);
                    }

                    //绘制editor
                    if (_editor == null)
                    {
                        _editor = Editor.CreateEditor(selectSetting, typeof(OdinEditor)) as OdinEditor;
                    }

                    _editor.OnInspectorGUI();
                }

                GUILayout.Space(30);
                GUILayout.EndScrollView();
            }
        }


        protected void DrawBottomGUI(Rect area)
        {
            var temEnable = GUI.enabled;
            GUI.enabled = _currentSelect != null;
            var hasAA = AddressableReflection.HasModule();
            var btnWidth = 160;
            var count = hasAA ? 3 : 2;
            var spaceWidth = Mathf.Max(0f, area.width - btnWidth * count);
            GUILayout.BeginHorizontal();
            GUILayout.Space(spaceWidth);
            _clickApply = DrawColorButton("Apply", ColorGreen, GUILayout.Width(btnWidth), GUILayout.Height(44));
            if (hasAA)
            {
                _clickBuildAA = DrawColorButton("Build AA Resources", ColorYellow, GUILayout.Width(btnWidth), GUILayout.Height(44));
            }

            _clickPublish = DrawColorButton("Publish", ColorCyan, GUILayout.Width(btnWidth), GUILayout.Height(44));
            GUILayout.EndHorizontal();
            GUI.enabled = temEnable;

            var bottom = new Rect(0, 0, area.width, area.height);
            var btnOptionsRect = bottom;
            btnOptionsRect.width = 120;
            btnOptionsRect.height = 26;
            btnOptionsRect.center = bottom.center;
            btnOptionsRect.x = bottom.x;
            btnOptionsRect.x += 8;
            if (GUI.Button(btnOptionsRect, "Channel Config"))
            {
                // 在 BeginArea/OnGUI 内同步打开窗口会导致 GUILayout 重入，EndArea 时栈错乱（Stack empty）
                EditorApplication.delayCall += () => XGameMenuWindow.Open();
            }

            if (_clickApply)
            {
                _clickApply = false;
                var setting = _currentSelect;
                EditorApplication.delayCall += () =>
                {
                    if (setting != null)
                    {
                        setting.Use();
                    }
                };
            }

            if (_clickBuildAA)
            {
                _clickBuildAA = false;
                var setting = _currentSelect;
                EditorApplication.delayCall += () =>
                {
                    if (setting != null)
                    {
                        setting.BuildAA();
                    }
                };
            }

            if (_clickPublish)
            {
                _clickPublish = false;
                var setting = _currentSelect;
                EditorApplication.delayCall += () =>
                {
                    if (setting != null)
                    {
                        setting.Publish();
                    }
                };
            }
        }

        private GUIStyle _colorBtnLabelStyle = null;

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


        private GUIStyle _menuBtnLabelStyle = null;

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

        private GUIStyle _titleLabelStyle = null;

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

        private void WriteCacheSelectSetting()
        {
            if (_currentSelect != null)
            {
                var dir = Path.GetDirectoryName(CACHE_SELECT_SAVE_FILE);
                XGameEditorUtil.CheckCreateFolder(dir);
                //写入忽略项
                var gitIgnore = $"{dir}/.gitignore";
                if (!File.Exists(gitIgnore))
                {
                    File.WriteAllText(gitIgnore, "*");
                }

                File.WriteAllText(CACHE_SELECT_SAVE_FILE, _currentSelect.name);
                _CacheSelect = _currentSelect.name;
            }
        }

        protected bool DrawColorButton(string content, Color color, params GUILayoutOption[] options)
        {
            var temColor = GUI.color;
            if (EditorGUIUtility.isProSkin)
            {
                color *= 2f;
            }

            GUI.color = color;

            var click = GUILayout.Button("", options);
            var last = GUILayoutUtility.GetLastRect();
            // EditorGUI.DrawRect(last, color*0.8f);
            GUI.color = temColor;
            ColorBtnTxtStyle.normal.textColor = Color.white;
            ColorBtnTxtStyle.fontStyle = FontStyle.Bold;
            GUI.Label(last, content, ColorBtnTxtStyle);
            return click;
        }

        protected void DrawBoldTitle(string content, params GUILayoutOption[] options)
        {
            GUILayout.Label("", options);
            var last = GUILayoutUtility.GetLastRect();
            EditorGUI.DrawRect(last, Color.black * 0.4f);
            last.width -= 4;
            last.x += 4;
            GUI.Label(last, content, TitleLabelStyle);
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