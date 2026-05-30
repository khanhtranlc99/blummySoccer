// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using UnityEditor;
// using UnityEngine;
//
// namespace XGame
// {
//     /// <summary>
//     /// 纹理压缩工具
//     /// </summary>
//     public class CompressTextureToolWindow : EditorWindow
//     {
//         private static Dictionary<string, string> FormatNames = new Dictionary<string, string>()
//         {
//             { "Alpha8", "Alpha 8" },
//             { "ARGB16", "ARGB 16 bits" },
//             { "RGB24", "RGB 24 bits" },
//             { "RGBA32", "RGBA 32 bits" },
//             { "ARGB32", "ARGB 32 bits" },
//             { "RGB16", "RGB 16 bits" },
//             { "R16", "R 16 bits" },
//             { "DXT1", "RGB DXT 1" },
//             { "DXT5", "RGBA DXT 5" },
//             { "RGBA16", "RGBA 16 bits" },
//             { "RHalf", "R Half" },
//             { "RGHalf", "RG Half" },
//             { "RGBAHalf", "RGBA Half" },
//             { "RFloat", "R Float" },
//             { "RGFloat", "RG Float" },
//             { "RGBAFloat", "RGBA Float" },
//             { "RGB9E5", "RGB9e5 32 bits" },
//             { "BC6H", "RGB HDR BC6H" },
//             { "BC7", "RGBA BC7" },
//             { "BC4", "R BC4" },
//             { "BC5", "RG BC5" },
//             { "DXT1Crunched", "RGB Crunched DXT 1" },
//             { "DXT5Crunched", "RGBA Crunched DXT 5" },
//             { "PVRTC_RGB2", "RGB PVRTC 2 bits" },
//             { "PVRTC_RGBA2", "RGBA PVRTC 2 bits" },
//             { "PVRTC_RGB4", "RGB PVRTC 4 bits" },
//             { "PVRTC_RGBA4", "RGBA PVRTC 4 bits" },
//             { "ETC_RGB4", "RGB ETC 4 bits" },
//             { "EAC_R", "R EAC 4 bits" },
//             { "EAC_R_SIGNED", "R EAC Signed 4 bits" },
//             { "EAC_RG", "RG EAC 8 bits" },
//             { "EAC_RG_SIGNED", "RG EAC Signed 8 bits" },
//             { "ETC2_RGB4", "RGB ETC2 4 bits" },
//             { "ETC2_RGB4_PUNCHTHROUGH_ALPHA", "RGB +1-bit Alpha ETC2 4 bits" },
//             { "ETC2_RGBA8", "RGBA ETC2 8 bits" },
//             { "ASTC_4x4", "RGB(A) ASTC 4x4 block" },
//             { "ASTC_RGB_4x4", "RGB(A) ASTC 4x4 block" },
//             { "ASTC_5x5", "RGB(A) ASTC 5x5 block" },
//             { "ASTC_RGB_5x5", "RGB(A) ASTC 5x5 block" },
//             { "ASTC_6x6", "RGB(A) ASTC 6x6 block" },
//             { "ASTC_RGB_6x6", "RGB(A) ASTC 6x6 block" },
//             { "ASTC_8x8", "RGB(A) ASTC 8x8 block" },
//             { "ASTC_RGB_8x8", "RGB(A) ASTC 8x8 block" },
//             { "ASTC_10x10", "RGB(A) ASTC 10x10 block" },
//             { "ASTC_RGB_10x10", "RGB(A) ASTC 10x10 block" },
//             { "ASTC_12x12", "RGB(A) ASTC 12x12 block" },
//             { "ASTC_RGB_12x12", "RGB(A) ASTC 12x12 block" },
//             { "ASTC_RGBA_4x4", "RGBA ASTC 4x4 block" },
//             { "ASTC_RGBA_5x5", "RGBA ASTC 5x5 block" },
//             { "ASTC_RGBA_6x6", "RGBA ASTC 6x6 block" },
//             { "ASTC_RGBA_8x8", "RGBA ASTC 8x8 block" },
//             { "ASTC_RGBA_10x10", "RGBA ASTC 10x10 block" },
//             { "ASTC_RGBA_12x12", "RGBA ASTC 12x12 block" },
//             { "RG16", "RG 16" },
//             { "R8", "R 8" },
//             { "ETC_RGB4Crunched", "RGB Crunched ETC" },
//             { "ETC2_RGBA8Crunched", "RGBA Crunched ETC" },
//             { "ASTC_HDR_4x4", "RGB(A) ASTC HDR 4x4 block" },
//             { "ASTC_HDR_5x5", "RGB(A) ASTC HDR 5x5 block" },
//             { "ASTC_HDR_6x6", "RGB(A) ASTC HDR 6x6 block" },
//             { "ASTC_HDR_8x8", "RGB(A) ASTC HDR 8x8 block" },
//             { "ASTC_HDR_10x10", "RGB(A) ASTC HDR 10x10 block" },
//             { "ASTC_HDR_12x12", "RGB(A) ASTC HDR 12x12 block" },
//             { "RG32", "RG 32 bit" },
//             { "RGB48", "RGB 48 bit" },
//             { "RGBA64", "RGBA 64 bit" },
//         };
//
//         private static TextureImporterType[] FILTER_TEXTURE_TYPE = GetValidEnumValues<TextureImporterType>();
//
//         private static TextureImporterShape[] FILTER_SHAPE_TYPE = GetValidEnumValues<TextureImporterShape>();
//
//         private static Dictionary<TextureImporterFormat, string> _formatNamesRemap = null;
//
//         private static string GetFormatName(TextureImporterFormat format)
//         {
//             if (_formatNamesRemap == null)
//             {
//                 _formatNamesRemap = new Dictionary<TextureImporterFormat, string>();
//                 foreach (var kv in FormatNames)
//                 {
//                     var key = kv.Key;
//                     var name = kv.Value;
//                     try
//                     {
//                         var enumType = (TextureImporterFormat)Enum.Parse(typeof(TextureImporterFormat), key);
//                         _formatNamesRemap[enumType] = name;
//                     }
//                     catch (Exception e)
//                     {
//                     }
//                 }
//             }
//
//             if (_formatNamesRemap.TryGetValue(format, out var match))
//             {
//                 return match;
//             }
//
//             return format.ToString();
//         }
//
//
//         private enum SelectMode
//         {
//             Single,
//             Shift,
//             Ctrl,
//         }
//
//         private enum SortType
//         {
//             Name,
//             Override,
//             Format,
//             MaxSize,
//             SourceSize,
//             MipMap,
//             NPot,
//             TextureType,
//         }
//
//         //点击apply
//         private enum ClickApplyAction
//         {
//             None,
//             MatchItems,
//             SelectItems,
//         }
//
//         //左侧菜单宽度
//         private const float LEFT_MENU_WIDTH = 246;
//         private const float NAME_WIDTH = 226;
//         private const float MENU_BUTTON_HEIGHT = 48;
//         private const float ITEM_HEIGHT = 20;
//         private static TextureImporter _demoImporter;
//         private static string[] _guids = null;
//         private static List<BuildTargetTextureCompressConfig> _configs = null;
//         private static BuildTargetTextureCompressConfig _currentSelect = null;
//
//         private BuildTargetTextureCompressConfig CurrentSelect
//         {
//             get => _currentSelect;
//             set
//             {
//                 if (_currentSelect == value)
//                 {
//                     return;
//                 }
//
//                 _currentSelect = value;
//                 SetMatchItemsDirty();
//             }
//         }
//
//         private static Vector2 _scroll;
//
//         private static Dictionary<string, Texture> _previewTextures = new Dictionary<string, Texture>();
//         private static List<CompressItemData> _matchItems = new List<CompressItemData>();
//
//         private static HashSet<string> _matchItemsGuid = new HashSet<string>();
//
//         //上一次选择的实例
//         private static string _lastSelectGuid;
//         private static bool _foldoutMenuFlag = true;
//         private static Texture _previewTexture;
//         private static string _previewGuid;
//
//         //过滤图片类型
//         private static HashSet<TextureImporterType> _filterTextureType =
//             new HashSet<TextureImporterType>(FILTER_TEXTURE_TYPE);
//
//         //过滤shape类型
//         private static HashSet<TextureImporterShape> _filterShapeType =
//             new HashSet<TextureImporterShape>(FILTER_SHAPE_TYPE);
//
//
//         public static Texture GetPreviewTexture(string guid)
//         {
//             if (_previewTextures.TryGetValue(guid, out var match))
//             {
//                 return match;
//             }
//
//             return null;
//         }
//
//         public static HashSet<string> _selectGuids = new HashSet<string>();
//
//         private List<Rect> _bottomRaycastRects = new List<Rect>();
//         private Rect _bottomGUIViewRect;
//         private Rect _scrollViewRange;
//         private Rect _scrollViewContainerRange;
//         private Rect _scrollViewEmptyRange;
//         private Vector2 _scrollMenuBtn;
//
//
//         #region GUI样式
//
//         private static bool IsProSKin => EditorGUIUtility.isProSkin;
//         private static GUIStyle _styleConfigTitleLabel = null;
//
//         private static GUIStyle StyleConfigTitleLabel => GetStyle(ref _styleConfigTitleLabel, "label", (style) =>
//         {
//             style.margin = new RectOffset(0, 0, 0, 0);
//             style.padding = new RectOffset(8, 8, 2, 2);
//             style.border = new RectOffset(0, 0, 0, 0);
//             style.alignment = TextAnchor.MiddleLeft;
//         });
//
//         private static GUIStyle GetStyleConfigTitleLabel(bool isSelect)
//         {
//             var textColor = IsProSKin ? Color.white : Color.black;
//             if (isSelect)
//             {
//                 textColor = IsProSKin ? new Color(0.3f, 0.53f, 1f, 1f) : Color.white;
//             }
//
//             StyleConfigTitleLabel.normal.textColor = textColor;
//             StyleConfigTitleLabel.fontStyle = isSelect ? FontStyle.Bold : FontStyle.Normal;
//             return StyleConfigTitleLabel;
//         }
//
//
//         private static GUIStyle _styleConfigSubLabel = null;
//
//
//         private static GUIStyle StyleConfigSubLabel => GetStyle(ref _styleConfigSubLabel, "label", (style) =>
//         {
//             style.margin = new RectOffset(0, 0, 0, 0);
//             style.padding = new RectOffset(8, 8, 2, 2);
//             style.border = new RectOffset(0, 0, 0, 0);
//             style.alignment = TextAnchor.MiddleLeft;
//         });
//
//         private static GUIStyle GetStyleConfigSubLabel(bool isSelect)
//         {
//             var textColor = IsProSKin ? Color.white : Color.black;
//             if (isSelect)
//             {
//                 textColor = IsProSKin ? new Color(0.3f, 0.53f, 1f, 1f) : Color.white;
//             }
//
//             StyleConfigSubLabel.normal.textColor = textColor;
//             StyleConfigSubLabel.fontStyle = isSelect ? FontStyle.Bold : FontStyle.Normal;
//             return StyleConfigSubLabel;
//         }
//
//
//         private static GUIStyle _styleSearchTips = null;
//
//         private static GUIStyle StyleSearchTips => GetStyle(ref _styleSearchTips, "label",
//             (style) =>
//             {
//                 var col = style.normal.textColor;
//                 style.normal.textColor = new Color(col.r, col.g, col.b, col.a * 0.5f);
//                 style.fontStyle = FontStyle.Italic;
//                 style.margin = new RectOffset(0, 0, 0, 0);
//                 style.padding = new RectOffset(4, 8, 2, 2);
//                 style.border = new RectOffset(0, 0, 0, 0);
//                 style.alignment = TextAnchor.MiddleLeft;
//             });
//
//
//         private static GUIStyle _styleProcessBarTxt = null;
//
//         private static GUIStyle StyleProcessBarTxt => GetStyle(ref _styleProcessBarTxt, "label",
//             (style) =>
//             {
//                 style.normal.textColor =
//                     IsProSKin ? new Color(0.33f, 0.54f, 0.97f, 1f) : new Color(0.22f, 0.37f, 0.67f, 1f);
//                 style.fontStyle = FontStyle.Bold;
//                 style.fontSize = 10;
//                 style.margin = new RectOffset(0, 0, 0, 0);
//                 style.padding = new RectOffset(4, 8, 2, 2);
//                 style.border = new RectOffset(0, 0, 0, 0);
//                 style.alignment = TextAnchor.LowerLeft;
//             });
//
//
//         private static GUIStyle _styleBlockLabel = null;
//
//         private static GUIStyle StyleBlockLabel => GetStyle(ref _styleBlockLabel, "label",
//             (style) =>
//             {
//                 style.normal.textColor = Color.white;
//                 style.fontStyle = FontStyle.Bold;
//                 style.margin = new RectOffset(0, 0, 0, 0);
//                 style.padding = new RectOffset(4, 8, 2, 2);
//                 style.border = new RectOffset(0, 0, 0, 0);
//                 style.fontSize = 12;
//                 style.alignment = TextAnchor.MiddleLeft;
//             });
//
//         private static GUIStyle _styleLabelMid = null;
//
//         private static GUIStyle StyleLabelMid => GetStyle(ref _styleLabelMid, "label",
//             (style) =>
//             {
//                 style.margin = new RectOffset(0, 0, 0, 0);
//                 style.padding = new RectOffset(4, 8, 2, 2);
//                 style.border = new RectOffset(0, 0, 0, 0);
//                 style.fontSize = 12;
//                 style.alignment = TextAnchor.MiddleCenter;
//             });
//
//
//         private static GUIStyle GetStyle(ref GUIStyle style, string styleName, Action<GUIStyle> onCreate = null)
//         {
//             if (style == null)
//             {
//                 style = new GUIStyle(styleName);
//                 onCreate?.Invoke(style);
//             }
//
//             return style;
//         }
//
//         #endregion
//
//         private string _deleteConfigPath = "";
//         private bool _matchItemsDirty = true;
//         private Rect _searchInputRange;
//         private string _searchTxt = "";
//         private Dictionary<SortType, bool> _sortStatus = new Dictionary<SortType, bool>();
//         private bool _displayInsideProcessBar = false;
//         private float _processBarFillAmount = 0;
//         private string _processBarTxt = "";
//
//         private ClickApplyAction _applyAction = ClickApplyAction.None;
//
//
//         [MenuItem("XGameUnityTool/工具箱/纹理压缩")]
//         private static void Open()
//         {
//             var win = GetWindow<CompressTextureToolWindow>();
//             win.minSize = new Vector2(1400, 720);
//             win.titleContent = new GUIContent("纹理压缩");
//             win.Reload();
//             win.Show();
//         }
//
//
//         private void OnEnable()
//         {
//             //更新数据
//             Reload();
//         }
//
//         private void OnFocus()
//         {
//             //更新数据
//             Reload();
//         }
//
//         private async void OnGUI()
//         {
//             RefreshMatchItems();
//             HandleInputEvent();
//             //加载所有图片
//             var rect = new Rect(0, 0, position.width, position.height);
//             var rectLeftMenu = new Rect(0, 0, LEFT_MENU_WIDTH, position.height);
//             if (_foldoutMenuFlag)
//             {
//                 DrawLeftMenuGUI(rectLeftMenu);
//             }
//             else
//             {
//                 rectLeftMenu.width = 16;
//             }
//
//             //绘制分割线
//             var line = rectLeftMenu;
//             line.x = rectLeftMenu.xMax;
//             line.width = 1;
//             EditorGUI.DrawRect(line, Color.black);
//
//             var rectContent = rectLeftMenu;
//             rectContent.x = line.xMax;
//             rectContent.width = rect.xMax - rectContent.x;
//             var center = rectContent.center;
//             rectContent.width -= 8;
//             rectContent.height -= 8;
//             rectContent.center = center;
//             rectContent.height -= 48;
//             DrawContentGUI(rectContent);
//
//
//             var rectBottom = rectContent;
//             rectBottom.y = rectContent.yMax;
//             rectBottom.y -= 20;
//             rectBottom.height = rect.yMax - rectBottom.y;
//             rectBottom.height -= 2;
//             var rectBottomLine = rectBottom;
//             rectBottomLine.height = 1;
//             rectBottomLine.y -= 1;
//             EditorGUI.DrawRect(rectBottomLine, Color.black * 0.7f);
//             DrawBottomGUI(rectBottom);
//             _bottomGUIViewRect = rectBottom;
//
//             var rectFoldoutMenu = line;
//             rectFoldoutMenu.width = 22;
//             rectFoldoutMenu.height = 22;
//             rectFoldoutMenu.center = line.center;
//             if (GUI.Button(rectFoldoutMenu, _foldoutMenuFlag ? "◀" : "▶"))
//             {
//                 _foldoutMenuFlag = !_foldoutMenuFlag;
//             }
//
//             if (!string.IsNullOrWhiteSpace(_deleteConfigPath))
//             {
//                 AssetDatabase.DeleteAsset(_deleteConfigPath);
//                 AssetDatabase.Refresh();
//                 _deleteConfigPath = null;
//                 await Reload();
//             }
//
//             HandleApply();
//         }
//
//         //左侧菜单栏
//         private void DrawLeftMenuGUI(Rect view)
//         {
//             EditorGUI.DrawRect(view, Color.black * 0.2f);
//             var menuView = view;
//             menuView.height -= menuView.width;
//             var center = menuView.center;
//             menuView.width -= 8;
//             menuView.height -= 8;
//             menuView.center = center;
//             var previewView = view;
//             previewView.height = previewView.width;
//             previewView.y = view.yMax - previewView.height;
//             EditorGUI.DrawRect(previewView, new Color(0.19f, 0.19f, 0.19f, 1f));
//             if (Selection.activeObject != null && Selection.activeObject is Texture)
//             {
//                 GUI.DrawTexture(previewView, Selection.activeObject as Texture, ScaleMode.ScaleToFit);
//             }
//
//             GUILayout.BeginArea(menuView);
//
//             GUILayout.BeginHorizontal();
//             if (GUILayout.Button("新建配置"))
//             {
//                 //弹出右键菜单
//                 var menu = new GenericMenu();
//                 menu.AddItem(new GUIContent("Android"), false,
//                     () => { NewConfig(BuildTarget.Android); });
//                 menu.AddItem(new GUIContent("IOS"), false,
//                     () => NewConfig(BuildTarget.iOS));
//                 menu.AddItem(new GUIContent("Webgl"), false,
//                     () => NewConfig(BuildTarget.WebGL));
//                 menu.AddItem(new GUIContent("PC"), false,
//                     () => NewConfig(BuildTarget.StandaloneWindows));
//                 menu.ShowAsContext();
//             }
//
//             if (GUILayout.Button("刷新"))
//             {
//                 Reload();
//             }
//
//             GUILayout.EndHorizontal();
//
//             DrawConfigsGUI();
//             //绘制配置项
//             GUILayout.EndArea();
//         }
//
//         private void DrawConfigsGUI()
//         {
//             _scrollMenuBtn = GUILayout.BeginScrollView(_scrollMenuBtn);
//             if (_configs != null)
//             {
//                 foreach (var element in _configs)
//                 {
//                     DrawConfigMenuButton(element);
//                 }
//             }
//
//             GUILayout.Space(30);
//             GUILayout.EndScrollView();
//         }
//
//         //绘制配置菜单按钮
//         private void DrawConfigMenuButton(BuildTargetTextureCompressConfig config)
//         {
//             GUILayout.Label("", GUILayout.Height(MENU_BUTTON_HEIGHT));
//             var last = GUILayoutUtility.GetLastRect();
//             var isSelect = config == CurrentSelect;
//             if (Event.current.type == EventType.MouseDown && Event.current.button == 1 &&
//                 last.Contains(Event.current.mousePosition))
//             {
//                 //弹出右键菜单
//                 var menu = new GenericMenu();
//                 menu.AddItem(new GUIContent("删除"), false, () =>
//                 {
//                     if (EditorUtility.DisplayDialog("提示", $"删除{config.name}?不可撤销！", "是"))
//                     {
//                         _deleteConfigPath = AssetDatabase.GetAssetPath(config);
//                     }
//                 });
//                 menu.ShowAsContext();
//                 Event.current.Use();
//             }
//
//             if (GUI.Button(last, ""))
//             {
//                 if (isSelect)
//                 {
//                     ClearSelect();
//                     Selection.activeObject = config;
//                     EditorApplication.ExecuteMenuItem("Window/General/Project");
//                     EditorGUIUtility.PingObject(config);
//                 }
//
//                 CurrentSelect = config;
//             }
//
//
//             if (isSelect)
//             {
//                 EditorGUI.DrawRect(last, Color.black * 0.7f);
//             }
//
//             var rectLabel = last;
//             rectLabel.height = 24;
//
//             var style = GetStyleConfigTitleLabel(isSelect);
//             GUI.Label(rectLabel, config.name, style);
//
//
//             var rectBottom = rectLabel;
//             rectBottom.width -= 24;
//             rectBottom.y = rectLabel.yMax;
//             rectBottom.height = last.yMax - rectBottom.y;
//             style = GetStyleConfigSubLabel(isSelect);
//             GUI.Label(rectBottom, $"[{config.BuildTarget}]", style);
//
//             var rectGMenu = rectBottom;
//             rectGMenu.width = last.xMax - rectBottom.xMax;
//             rectGMenu.x = rectBottom.xMax;
//             var center = rectGMenu.center;
//             rectGMenu.width -= 8;
//             rectGMenu.height -= 8;
//             rectGMenu.center = center;
//
//             GUI.Label(rectGMenu, new GUIContent("≡", "右键菜单"));
//             if (isSelect && Event.current.type == EventType.KeyDown)
//             {
//                 switch (Event.current.keyCode)
//                 {
//                     case KeyCode.Delete:
//                     {
//                         DeleteConfig(config);
//                         Event.current.Use();
//                     }
//                         break;
//                 }
//             }
//         }
//
//         //右侧内容区
//         private void DrawContentGUI(Rect view)
//         {
//             GUILayout.BeginArea(view);
//
//             var rect = new Rect(0, 0, view.width, view.height);
//             var padding = new RectOffset(10, 10, 0, 30);
//             if (CurrentSelect != null)
//             {
//                 var buildTargetStr = CurrentSelect.BuildTarget.ToString();
//                 var titleRect = rect;
//                 titleRect.height = 26;
//                 EditorGUI.DrawRect(titleRect, Color.black * 0.6f);
//                 GUI.Label(titleRect, CurrentSelect.name, GetStyleConfigTitleLabel(true));
//
//                 var filterRect = titleRect;
//                 filterRect.height = 44;
//                 filterRect.y = titleRect.yMax;
//                 filterRect.y += 2;
//                 //过滤类型
//                 GUILayout.BeginArea(filterRect);
//                 GUILayout.BeginHorizontal();
//                 if (GUILayout.Button("全选", GUILayout.Width(60)))
//                 {
//                     _filterTextureType = new HashSet<TextureImporterType>(FILTER_TEXTURE_TYPE);
//                     SetMatchItemsDirty();
//                 }
//
//                 if (GUILayout.Button("全不选", GUILayout.Width(60)))
//                 {
//                     _filterTextureType.Clear();
//                     SetMatchItemsDirty();
//                 }
//
//                 GUILayout.Label("类型:", GUILayout.Width(60));
//                 {
//                     var full = _filterTextureType.Count == FILTER_TEXTURE_TYPE.Length;
//                     foreach (var element in FILTER_TEXTURE_TYPE)
//                     {
//                         var tog = _filterTextureType.Contains(element) | full;
//                         var temTog = tog;
//                         temTog = GUILayout.Toggle(temTog, element.ToString());
//                         if (temTog != tog)
//                         {
//                             if (temTog)
//                             {
//                                 _filterTextureType.Add(element);
//                                 SetMatchItemsDirty();
//                             }
//                             else
//                             {
//                                 _filterTextureType.Remove(element);
//                                 SetMatchItemsDirty();
//                             }
//                         }
//                     }
//                 }
//
//                 GUILayout.EndHorizontal();
//                 GUILayout.BeginHorizontal();
//                 //过滤shape
//                 if (GUILayout.Button("全选", GUILayout.Width(60)))
//                 {
//                     _filterShapeType = new HashSet<TextureImporterShape>(FILTER_SHAPE_TYPE);
//                     SetMatchItemsDirty();
//                 }
//
//                 if (GUILayout.Button("全不选", GUILayout.Width(60)))
//                 {
//                     _filterShapeType.Clear();
//                     SetMatchItemsDirty();
//                 }
//
//                 GUILayout.Label("Shape:", GUILayout.Width(60));
//                 {
//                     var full = _filterShapeType.Count == FILTER_TEXTURE_TYPE.Length;
//                     foreach (var element in FILTER_SHAPE_TYPE)
//                     {
//                         var tog = _filterShapeType.Contains(element) | full;
//                         var temTog = tog;
//                         temTog = GUILayout.Toggle(temTog, element.ToString(), GUILayout.Width(91));
//                         if (temTog != tog)
//                         {
//                             if (temTog)
//                             {
//                                 _filterShapeType.Add(element);
//                                 SetMatchItemsDirty();
//                             }
//                             else
//                             {
//                                 _filterShapeType.Remove(element);
//                                 SetMatchItemsDirty();
//                             }
//                         }
//                     }
//                 }
//
//                 var resetAllFilter = new Rect();
//                 resetAllFilter.width = 100;
//                 resetAllFilter.height = 22;
//                 resetAllFilter.x = filterRect.width - resetAllFilter.width;
//                 resetAllFilter.x -= 4;
//                 resetAllFilter.y = filterRect.height - resetAllFilter.height;
//                 resetAllFilter.y -= 2;
//                 if (GUI.Button(resetAllFilter, "复位所有"))
//                 {
//                     _filterTextureType = new HashSet<TextureImporterType>(FILTER_TEXTURE_TYPE);
//                     _filterShapeType = new HashSet<TextureImporterShape>(FILTER_SHAPE_TYPE);
//                     SetMatchItemsDirty();
//                 }
//
//                 EditorGUI.DrawRect(resetAllFilter, Color.green * 0.5f);
//
//                 GUILayout.EndHorizontal();
//                 GUILayout.EndArea();
//
//
//                 var searchRect = filterRect;
//                 searchRect.height = 22;
//                 searchRect.y = filterRect.yMax;
//                 searchRect.y += 2;
//                 var temSearchTxt = _searchTxt;
//                 temSearchTxt = GUI.TextField(searchRect, temSearchTxt);
//                 if (temSearchTxt != _searchTxt)
//                 {
//                     _searchTxt = temSearchTxt;
//                     SetMatchItemsDirty();
//                 }
//
//                 if (string.IsNullOrEmpty(_searchTxt))
//                 {
//                     GUI.Label(searchRect, " 搜索...", StyleSearchTips);
//                 }
//
//                 _searchInputRange = searchRect;
//                 _searchInputRange.position = view.position + searchRect.position;
//
//                 var tabRectBar = searchRect;
//                 tabRectBar.height = 24;
//                 tabRectBar.y = searchRect.yMax;
//                 tabRectBar.y += 2;
//                 tabRectBar.x += padding.left;
//                 tabRectBar.width -= padding.horizontal;
//                 var blockRect = tabRectBar;
//                 var overrideWidth = 70;
//                 var formatWidth = 200;
//                 var npotWidth = 100;
//                 var mipmapWidth = 80;
//                 var maxSizeWidth = 120;
//                 var sourceSizeWidth = 120;
//                 var typeWidth = 160;
//                 var whiteWidth = 20f;
//                 var nameWidth = NAME_WIDTH;
//                 whiteWidth = blockRect.width - overrideWidth - formatWidth - npotWidth - mipmapWidth - maxSizeWidth -
//                              whiteWidth - typeWidth - nameWidth;
//                 blockRect.width = nameWidth;
//                 DrawTabBlock(blockRect, "名称");
//                 var rectSortByName = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = overrideWidth;
//                 DrawTabBlock(blockRect, "覆写");
//                 var rectSortOverride = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = formatWidth;
//                 DrawTabBlock(blockRect, "格式");
//                 var rectSortByFormat = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = maxSizeWidth;
//                 DrawTabBlock(blockRect, "maxSize");
//                 var rectSortByMaxSize = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = sourceSizeWidth;
//                 DrawTabBlock(blockRect, "原始大小");
//                 var rectSortBySourceSize = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = mipmapWidth;
//                 DrawTabBlock(blockRect, "mipmap");
//                 var rectSortByMipMap = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = npotWidth;
//                 DrawTabBlock(blockRect, "npot");
//                 var rectSortByNpot = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = typeWidth;
//                 DrawTabBlock(blockRect, "类型");
//                 var rectSortByTexType = CreateSortBtnRect(blockRect);
//                 blockRect.x += blockRect.width;
//                 blockRect.width = whiteWidth;
//                 DrawTabBlock(blockRect, "");
//                 var items = _matchItems ?? new List<CompressItemData>();
//
//                 var rectSortDefault = rectSortByName;
//                 rectSortDefault.width = 60;
//                 rectSortDefault.x = rectSortByName.xMin - rectSortDefault.width;
//                 rectSortDefault.x -= 4;
//                 if (GUI.Button(rectSortDefault, "默认排序"))
//                 {
//                     SortByDefault();
//                 }
//
//                 //绘制排序按钮
//                 if (GUI.Button(rectSortByName, "▼"))
//                 {
//                     SortByName();
//                 }
//
//                 if (GUI.Button(rectSortOverride, "▼"))
//                 {
//                     SortByOverride();
//                 }
//
//                 if (GUI.Button(rectSortByFormat, "▼"))
//                 {
//                     SortByFormat();
//                 }
//
//                 if (GUI.Button(rectSortByMaxSize, "▼"))
//                 {
//                     SortByMaxSize();
//                 }
//
//                 if (GUI.Button(rectSortBySourceSize, "▼"))
//                 {
//                     SortBySourceSize();
//                 }
//
//                 if (GUI.Button(rectSortByMipMap, "▼"))
//                 {
//                     SortByMipMap();
//                 }
//
//                 if (GUI.Button(rectSortByNpot, "▼"))
//                 {
//                     SortByNpot();
//                 }
//
//                 if (GUI.Button(rectSortByTexType, "▼"))
//                 {
//                     SortByTexType();
//                 }
//
//                 var scrollRect = tabRectBar;
//                 scrollRect.y = tabRectBar.yMax;
//                 scrollRect.height = rect.height - scrollRect.yMax;
//                 var viewRect = scrollRect;
//                 viewRect.height = padding.vertical + items.Count * ITEM_HEIGHT;
//                 _scroll = GUI.BeginScrollView(scrollRect, _scroll, viewRect, false, true, GUIStyle.none,
//                     GUI.skin.verticalScrollbar);
//
//                 var rectItem = viewRect;
//                 rectItem.y += padding.top;
//                 rectItem.width -= padding.horizontal;
//                 rectItem.height = ITEM_HEIGHT;
//                 var index = 0;
//                 var viewRangeMaxY = _scroll.y + scrollRect.height + 200;
//                 var viewRangeMinY = _scroll.y - ITEM_HEIGHT;
//
//                 var widthArr = new[]
//                 {
//                     nameWidth, overrideWidth, formatWidth, maxSizeWidth, sourceSizeWidth, mipmapWidth,
//                     npotWidth, typeWidth
//                 };
//                 var offsets = new float[widthArr.Length];
//                 for (int i = 0; i < offsets.Length; i++)
//                 {
//                     if (i == 0)
//                     {
//                         offsets[i] = rectItem.x;
//                     }
//                     else
//                     {
//                         offsets[i] += (offsets[i - 1] + widthArr[i - 1]);
//                     }
//                 }
//
//                 foreach (var element in items)
//                 {
//                     index += 1;
//                     if (index % 2 == 0)
//                     {
//                         EditorGUI.DrawRect(rectItem, Color.black * 0.1f);
//                     }
//
//                     var isRenderer = rectItem.y > viewRangeMinY && rectItem.y < viewRangeMaxY;
//                     if (isRenderer)
//                     {
//                         if (_selectGuids.Contains(element.Guid))
//                         {
//                             EditorGUI.DrawRect(rectItem, IsProSKin ? Color.cyan * 0.5f : Color.blue * 0.3f);
//                         }
//
//                         var importer = CompressTextureTool.GetImporter(element.Guid);
//                         var itemY = rectItem.y;
//                         var height = rectItem.height;
//                         var rectName = new Rect(offsets[0], itemY, widthArr[0], height);
//                         var rectOverride = new Rect(offsets[1], itemY, widthArr[1], height);
//                         var rectFormat = new Rect(offsets[2], itemY, widthArr[2], height);
//                         var rectMaxSize = new Rect(offsets[3], itemY, widthArr[3], height);
//                         var rectSourceSize = new Rect(offsets[4], itemY, widthArr[4], height);
//                         var rectMipMap = new Rect(offsets[5], itemY, widthArr[5], height);
//                         var rectNpot = new Rect(offsets[6], itemY, widthArr[6], height);
//                         var rectType = new Rect(offsets[7], itemY, widthArr[7], height);
//
//                         //名称信息
//                         var rectIcon = rectName;
//                         rectIcon.width = rectName.height;
//                         var previewTex = GetPreviewTexture(element.Guid);
//                         if (previewTex != null)
//                         {
//                             GUI.DrawTexture(rectIcon, previewTex, ScaleMode.ScaleToFit);
//                         }
//                         else
//                         {
//                             GUI.Label(rectIcon, "?");
//                         }
//
//                         var rectLabel = rectName;
//                         rectLabel.width -= rectIcon.width;
//                         rectLabel.x += rectIcon.width;
//                         GUI.Label(rectLabel, previewTex == null ? "---" : previewTex.name);
//
//                         //覆写状态
//                         var togOverride = rectOverride;
//                         togOverride.width = togOverride.height;
//                         togOverride.center = rectOverride.center;
//                         var temOverride = element.Override;
//                         temOverride = GUI.Toggle(togOverride, temOverride, "");
//                         //处理覆写
//                         if (temOverride != element.Override)
//                         {
//                             ModifyOverride(element, temOverride);
//                         }
//
//
//                         var temEnable = GUI.enabled;
//                         GUI.enabled = element.Override;
//                         //格式
//                         var formatTxt = GetFormatName(element.ImporterFormat);
//                         if (!element.Override || element.ImporterFormat == TextureImporterFormat.Automatic)
//                         {
//                             var format = importer.GetAutomaticFormat(buildTargetStr);
//                             formatTxt = $"{GetFormatName(format)}";
//                         }
//
//                         if (GUI.Button(rectFormat, formatTxt, "PopUp"))
//                         {
//                             var item = element;
//                             var buildTarget = _currentSelect.BuildTarget;
//                             var formats =
//                                 CompressTextureTool.GetRecommendedFormats(importer, _currentSelect.BuildTarget)
//                                     .ToList();
//                             var menu = new GenericMenu();
//                             menu.AddItem(new GUIContent("默认推荐"), false,
//                                 () =>
//                                 {
//                                     // item.ChangeFormat(buildTarget, TextureImporterFormat.Automatic);
//                                     ModifyFormat(buildTarget, item, TextureImporterFormat.Automatic);
//                                 });
//                             foreach (var format in formats)
//                             {
//                                 var f = format;
//                                 var formatName = GetFormatName(format);
//                                 menu.AddItem(new GUIContent(formatName), false,
//                                     () => { ModifyFormat(buildTarget, item, f); });
//                             }
//
//                             menu.ShowAsContext();
//                         }
//
//                         var maxSizeTxt = element.MaxSize.ToString();
//                         if (!element.Override)
//                         {
//                             maxSizeTxt = importer.GetDefaultPlatformTextureSettings().maxTextureSize.ToString();
//                         }
//
//                         //maxsize
//                         if (GUI.Button(rectMaxSize, maxSizeTxt, "PopUp"))
//                         {
//                             var item = element;
//                             var sizeArr = new[] { 32, 64, 128, 256, 512, 1024, 2048, 4096, 8192 };
//                             var menu = new GenericMenu();
//                             foreach (var size in sizeArr)
//                             {
//                                 menu.AddItem(new GUIContent(size.ToString()), false,
//                                     () => { ModifyMaxSize(item, size); });
//                             }
//
//                             menu.ShowAsContext();
//                         }
//
//                         GUI.enabled = temEnable;
//
//                         //原始大小
//                         var source = CompressTextureTool.GetSourceTextureInformation(importer);
//                         GUI.Label(rectSourceSize, source != null ? $"{source.width} × {source.height}" : "---",
//                             StyleLabelMid);
//
//                         //mipmap
//                         var togMipMap = rectMipMap;
//                         togMipMap.width = togMipMap.height;
//                         togMipMap.center = rectMipMap.center;
//                         var temMipMapEnable = importer.mipmapEnabled;
//                         temMipMapEnable = GUI.Toggle(rectMipMap, temMipMapEnable, "");
//                         if (temMipMapEnable != importer.mipmapEnabled)
//                         {
//                             ModifyMipMap(element, temMipMapEnable);
//                         }
//
//                         //NPOT
//                         if (GUI.Button(rectNpot, importer.npotScale.ToString(), "PopUp"))
//                         {
//                             var npotArr = GetValidEnumValues<TextureImporterNPOTScale>();
//                             var menu = new GenericMenu();
//                             foreach (var enumType in npotArr)
//                             {
//                                 menu.AddItem(new GUIContent(enumType.ToString()), false,
//                                     () => ModifyNpot(element, enumType));
//                             }
//
//                             menu.ShowAsContext();
//                         }
//
//                         GUI.Label(rectType, $"{importer.textureType}({importer.textureShape})");
//                     }
//
//                     if (Event.current.type == EventType.MouseDown && Event.current.button == 0 &&
//                         rectItem.Contains(Event.current.mousePosition))
//                     {
//                         switch (Event.current.modifiers)
//                         {
//                             case EventModifiers.None:
//                                 Select(element.Guid, SelectMode.Single);
//                                 break;
//                             case EventModifiers.Shift:
//                                 Select(element.Guid, SelectMode.Shift);
//                                 break;
//                             case EventModifiers.Control:
//                                 Select(element.Guid, SelectMode.Ctrl);
//                                 break;
//                         }
//
//                         Event.current.Use();
//                     }
//
//
//                     rectItem.y += ITEM_HEIGHT;
//                 }
//
//                 _scrollViewRange = scrollRect;
//                 _scrollViewRange.position += view.position;
//                 _scrollViewContainerRange = viewRect;
//                 _scrollViewContainerRange.position += view.position;
//                 GUI.EndScrollView();
//             }
//             else
//             {
//                 var rectTips = rect;
//                 rectTips.height = 24;
//                 GUI.Label(rectTips, "请选择配置...");
//             }
//
//             GUILayout.EndArea();
//             _scrollViewEmptyRange = _scrollViewRange;
//             _scrollViewEmptyRange.y += _scrollViewContainerRange.height;
//             if (_scrollViewContainerRange.height < _scrollViewRange.height)
//             {
//                 _scrollViewEmptyRange.height = Mathf.Clamp(_scrollViewRange.height - _scrollViewContainerRange.height,
//                     0,
//                     float.MaxValue);
//             }
//         }
//
//         //底部区
//         private void DrawBottomGUI(Rect view)
//         {
//             _bottomRaycastRects.Clear();
//             if (CurrentSelect != null)
//             {
//                 var rect = new Rect(0, 0, view.width, view.height);
//                 GUILayout.BeginArea(view);
//                 var rectStatusBar = rect;
//                 rectStatusBar.height = 24;
//                 var total = _matchItems.Count;
//                 GUI.Label(rectStatusBar, $"共：{total}，当前选中：{_selectGuids.Count}");
//                 var rectApply = rect;
//                 rectApply.width = 120;
//                 rectApply.height -= 24;
//                 rectApply.center = rect.center;
//                 rectApply.x = rect.xMax - rectApply.width - 8;
//                 if (GUI.Button(rectApply, "转换"))
//                 {
//                     _applyAction = ClickApplyAction.MatchItems;
//                 }
//
//                 var rectApplySelected = rectApply;
//                 var temEnable = GUI.enabled;
//                 GUI.enabled = _selectGuids.Count > 0;
//                 rectApplySelected.x = rectApply.xMin - rectApplySelected.width;
//                 rectApplySelected.x -= 10;
//                 if (GUI.Button(rectApplySelected, temEnable ? $"转换选中项*({_selectGuids.Count})" : "转换选中项"))
//                 {
//                     _applyAction = ClickApplyAction.SelectItems;
//                 }
//
//                 GUI.enabled = temEnable;
//
//                 var rectSaveBtn = rectApply;
//                 rectSaveBtn.x = rectApplySelected.xMin - rectSaveBtn.width;
//                 rectSaveBtn.x -= 10;
//                 if (GUI.Button(rectSaveBtn, $"保存配置"))
//                 {
//                     CompressTextureTool.SaveAllConfig();
//                     Debug.Log("保存成功");
//                 }
//
//                 rectApply.position += view.position;
//                 rectSaveBtn.position += view.position;
//                 rectApplySelected.position += view.position;
//                 _bottomRaycastRects.Clear();
//                 _bottomRaycastRects.Add(rectApply);
//                 _bottomRaycastRects.Add(rectSaveBtn);
//                 _bottomRaycastRects.Add(rectApplySelected);
//                 GUILayout.EndArea();
//             }
//
//             if (_displayInsideProcessBar)
//             {
//                 var processBarRect = view;
//                 processBarRect.height = 6;
//                 processBarRect.y = view.yMax - processBarRect.height;
//                 EditorGUI.DrawRect(processBarRect, Color.black * 0.8f);
//                 var rectBar = processBarRect;
//                 rectBar.width -= 2;
//                 rectBar.height -= 2;
//                 rectBar.center = processBarRect.center;
//                 rectBar.width = Mathf.Lerp(0, rectBar.width, _processBarFillAmount);
//                 EditorGUI.DrawRect(rectBar, new Color(0.33f, 0.54f, 0.97f, 1f));
//                 var rectLabel = rectBar;
//                 rectLabel.height = 16;
//                 rectLabel.y -= rectLabel.height;
//                 rectLabel.y -= 2;
//                 GUI.Label(rectLabel, _processBarTxt, StyleProcessBarTxt);
//             }
//         }
//
//         //绘制block
//         private void DrawTabBlock(Rect rect, string label)
//         {
//             rect.width -= 1;
//             EditorGUI.DrawRect(rect, Color.black * 0.8f);
//             GUI.Label(rect, label, StyleBlockLabel);
//         }
//
//
//         private Rect CreateSortBtnRect(Rect block)
//         {
//             var result = block;
//             result.width = result.height;
//             result.x = block.xMax - result.width;
//             var center = result.center;
//             result.width -= 4;
//             result.height -= 4;
//             result.center = center;
//             return result;
//         }
//
//
//         private void DisplayInSideProcessBar(string txt, float process)
//         {
//             _displayInsideProcessBar = true;
//             _processBarTxt = txt;
//             _processBarFillAmount = process;
//         }
//
//         private void ClearInSideProcessBar()
//         {
//             _displayInsideProcessBar = false;
//         }
//
//         //加载纹理
//         private async Task Reload()
//         {
//             _guids = AssetDatabase.FindAssets("t:texture", new[] { "Assets" });
//             //更新
//             await CompressTextureTool.UpdateDatabase(_guids);
//             //更新配置项
//             LoadConfigs();
//             //异步更新预览图
//             ReloadPreviewTextureAsync();
//             SetMatchItemsDirty();
//         }
//
//         private static void LoadConfigs()
//         {
//             _configs = new List<BuildTargetTextureCompressConfig>();
//             var guids = AssetDatabase.FindAssets("t:BuildTargetTextureCompressConfig", new[] { "Assets" });
//             foreach (var element in guids)
//             {
//                 var path = AssetDatabase.GUIDToAssetPath(element);
//                 var asset = AssetDatabase.LoadAssetAtPath<BuildTargetTextureCompressConfig>(path);
//                 if (asset != null)
//                 {
//                     _configs.Add(asset);
//                 }
//             }
//         }
//
//         //新配置
//         private void NewConfig(BuildTarget target)
//         {
//             CompressTextureTool.CreateBuildTargetTextureCompressConfig(target);
//             Reload();
//         }
//
//         //删除配置
//         private void DeleteConfig(BuildTargetTextureCompressConfig config)
//         {
//             if (EditorUtility.DisplayDialog("提示", $"删除{config.name}?不可撤销！", "是"))
//             {
//                 _deleteConfigPath = AssetDatabase.GetAssetPath(config);
//             }
//         }
//
//         private static async void ReloadPreviewTextureAsync()
//         {
//             if (_guids == null)
//             {
//                 return;
//             }
//
//             var guids = _guids.ToArray();
//             var counter = 0;
//             foreach (var guid in guids)
//             {
//                 try
//                 {
//                     if (_previewTextures.TryGetValue(guid, out var match))
//                     {
//                         if (match != null)
//                         {
//                             continue;
//                         }
//                     }
//
//                     var tex = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(guid));
//                     _previewTextures[guid] = tex;
//                 }
//                 catch (Exception e)
//                 {
//                 }
//
//                 counter += 1;
//                 if (counter > 50)
//                 {
//                     counter = 0;
//                     await Task.Yield();
//                 }
//             }
//         }
//
//         private void SetMatchItemsDirty()
//         {
//             _matchItemsDirty = true;
//         }
//
//         private void RefreshMatchItems()
//         {
//             if (_matchItemsDirty)
//             {
//                 _matchItemsDirty = false;
//                 _matchItems = new List<CompressItemData>();
//                 _sortStatus = new Dictionary<SortType, bool>();
//                 _matchItemsGuid.Clear();
//                 if (_currentSelect == null)
//                 {
//                     UpdateSelectGuids();
//                     return;
//                 }
//
//                 var items = _currentSelect.Items;
//                 var low = _searchTxt.ToLower();
//                 var filterName = _searchTxt.ToLower().Length > 0;
//                 foreach (var item in items)
//                 {
//                     var importer = CompressTextureTool.GetImporter(item.Guid);
//                     if (filterName && !importer.assetPath.ToLower().Contains(low))
//                     {
//                         continue;
//                     }
//
//                     if (_filterTextureType.Contains(importer.textureType) &&
//                         _filterShapeType.Contains(importer.textureShape))
//                     {
//                         _matchItems.Add(item);
//                         _matchItemsGuid.Add(item.Guid);
//                     }
//                 }
//
//                 UpdateSelectGuids();
//             }
//         }
//
//         private void Select(string guid, SelectMode mode)
//         {
//             var texture = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(guid));
//             EditorGUIUtility.PingObject(texture);
//             Selection.activeObject = texture;
//             switch (mode)
//             {
//                 case SelectMode.Single:
//                 {
//                     _selectGuids.Clear();
//                     _selectGuids.Add(guid);
//                 }
//
//                     break;
//                 case SelectMode.Shift:
//                 {
//                     var startIndex = _matchItems.FindIndex((a) =>
//                     {
//                         if (a.Guid == _lastSelectGuid)
//                         {
//                             return true;
//                         }
//
//                         return false;
//                     });
//                     var endIndex = _matchItems.FindIndex((a) =>
//                     {
//                         if (a.Guid == guid)
//                         {
//                             return true;
//                         }
//
//                         return false;
//                     });
//                     if (startIndex >= 0 && endIndex >= 0)
//                     {
//                         var min = Mathf.Min(startIndex, endIndex);
//                         var max = Mathf.Max(startIndex, endIndex);
//                         _selectGuids.Clear();
//                         for (int i = min; i <= max && i < _matchItems.Count; i++)
//                         {
//                             _selectGuids.Add(_matchItems[i].Guid);
//                         }
//                     }
//                 }
//                     break;
//                 case SelectMode.Ctrl:
//                 {
//                     if (_selectGuids.Contains(guid))
//                     {
//                         _selectGuids.Remove(guid);
//                     }
//                     else
//                     {
//                         _selectGuids.Add(guid);
//                     }
//                 }
//                     break;
//             }
//
//             _lastSelectGuid = guid;
//             UpdateSelectGuids();
//         }
//
//         private void SelectAll()
//         {
//             if (_matchItemsGuid.Count > 0)
//             {
//                 _lastSelectGuid = "";
//                 if (_selectGuids.Count > 0)
//                 {
//                     var last = _selectGuids.Last();
//                     Selection.activeObject =
//                         AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(last));
//                     _lastSelectGuid = last;
//                 }
//
//                 _selectGuids = new HashSet<string>(_matchItemsGuid);
//                 UpdateSelectGuids();
//             }
//         }
//
//         //更新当前选择
//         private void UpdateSelectGuids()
//         {
//             var tem = new HashSet<string>();
//             foreach (var element in _selectGuids)
//             {
//                 if (_matchItemsGuid.Contains(element))
//                 {
//                     tem.Add(element);
//                 }
//             }
//
//             _selectGuids = tem;
//             if (!_selectGuids.Contains(_lastSelectGuid))
//             {
//                 _lastSelectGuid = "";
//             }
//
//             // //更新选择
//             var selection = new HashSet<Texture>();
//             foreach (var element in _selectGuids)
//             {
//                 var tex = AssetDatabase.LoadAssetAtPath<Texture>(CompressTextureTool.GetImporter(element).assetPath);
//                 if (tex != null)
//                 {
//                     selection.Add(tex);
//                 }
//             }
//
//             Selection.objects = selection.ToArray();
//         }
//
//         private void ClearSelect()
//         {
//             _selectGuids.Clear();
//             Selection.objects = null;
//             _lastSelectGuid = "";
//         }
//
//         private void HandleInputEvent()
//         {
//             if (CurrentSelect != null)
//             {
//                 if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
//                 {
//                     if (!_searchInputRange.Contains(Event.current.mousePosition))
//                     {
//                         GUI.FocusControl("");
//                     }
//
//                     if (_selectGuids.Count > 0 && _bottomGUIViewRect.Contains(Event.current.mousePosition))
//                     {
//                         var clearSelect = true;
//                         foreach (var rect in _bottomRaycastRects)
//                         {
//                             if (rect.Contains(Event.current.mousePosition))
//                             {
//                                 clearSelect = false;
//                             }
//                         }
//
//                         if (clearSelect)
//                         {
//                             ClearSelect();
//                             Event.current.Use();
//                         }
//                     }
//
//                     if (_selectGuids.Count > 0 && _scrollViewEmptyRange.Contains(Event.current.mousePosition))
//                     {
//                         ClearSelect();
//                         Event.current.Use();
//                     }
//                 }
//             }
//
//             if (Event.current.type == EventType.KeyDown && Event.current.modifiers == EventModifiers.Control &&
//                 Event.current.keyCode == KeyCode.A)
//             {
//                 //全选
//                 SelectAll();
//                 Event.current.Use();
//             }
//         }
//
//         private void ModifyOverride(CompressItemData data, bool isOn)
//         {
//             //设置所有
//             if (_selectGuids.Contains(data.Guid))
//             {
//                 foreach (var item in _matchItems)
//                 {
//                     if (_selectGuids.Contains(item.Guid))
//                     {
//                         item.Override = isOn;
//                     }
//                 }
//             }
//             else
//             {
//                 data.Override = isOn;
//             }
//         }
//
//         private void ModifyFormat(BuildTarget target, CompressItemData data, TextureImporterFormat format)
//         {
//             if (_selectGuids.Contains(data.Guid))
//             {
//                 foreach (var item in _matchItems)
//                 {
//                     if (_selectGuids.Contains(item.Guid))
//                     {
//                         item.ChangeFormat(target, format);
//                     }
//                 }
//             }
//             else
//             {
//                 data.ChangeFormat(target, format);
//             }
//         }
//
//         private void ModifyMaxSize(CompressItemData data, int size)
//         {
//             if (_selectGuids.Contains(data.Guid))
//             {
//                 foreach (var item in _matchItems)
//                 {
//                     if (_selectGuids.Contains(item.Guid))
//                     {
//                         item.MaxSize = size;
//                     }
//                 }
//             }
//             else
//             {
//                 data.MaxSize = size;
//             }
//         }
//
//         private async void ModifyMipMap(CompressItemData data, bool enable)
//         {
//             if (_selectGuids.Contains(data.Guid))
//             {
//                 var total = _selectGuids.Count;
//                 if (total > 50)
//                 {
//                     if (!EditorUtility.DisplayDialog("提示", $"修改选中项mipmap -> {enable}? 共：{total}项", "是"))
//                     {
//                         return;
//                     }
//                 }
//
//                 var current = 0;
//                 var counter = 0;
//                 foreach (var guid in _selectGuids)
//                 {
//                     current += 1;
//                     var process = (float)current / total;
//                     counter += 1;
//                     if (counter > 5 || current == 1)
//                     {
//                         counter = 0;
//                         EditorUtility.DisplayProgressBar("处理中",
//                             $"{guid}...{(process * 100).ToString("F1")}% \n 剩余：{total - current}", process);
//                         await Task.Yield();
//                     }
//
//                     var path = AssetDatabase.GUIDToAssetPath(guid);
//                     var importer = AssetImporter.GetAtPath(path) as TextureImporter;
//                     if (importer != null && importer.mipmapEnabled != enable)
//                     {
//                         importer.mipmapEnabled = enable;
//                         importer.SaveAndReimport();
//                     }
//                 }
//
//                 Debug.Log($"处理完毕，共：{total}项");
//                 EditorUtility.ClearProgressBar();
//             }
//             else
//             {
//                 var path = AssetDatabase.GUIDToAssetPath(data.Guid);
//                 var importer = AssetImporter.GetAtPath(path) as TextureImporter;
//                 if (importer != null && importer.mipmapEnabled != enable)
//                 {
//                     importer.mipmapEnabled = enable;
//                     importer.SaveAndReimport();
//                 }
//             }
//         }
//
//         private async void ModifyNpot(CompressItemData data, TextureImporterNPOTScale npotScale)
//         {
//             if (_selectGuids.Contains(data.Guid))
//             {
//                 var total = _selectGuids.Count;
//                 if (total > 50)
//                 {
//                     if (!EditorUtility.DisplayDialog("提示", $"修改选中项mipmap -> {npotScale}? 共：{total}项", "是"))
//                     {
//                         return;
//                     }
//                 }
//
//                 var current = 0;
//                 var counter = 0;
//                 foreach (var guid in _selectGuids)
//                 {
//                     current += 1;
//                     var process = (float)current / total;
//                     counter += 1;
//                     if (counter > 5 || current == 1)
//                     {
//                         counter = 0;
//                         EditorUtility.DisplayProgressBar("处理中",
//                             $"{guid}...{(process * 100).ToString("F1")}% \n 剩余：{total - current}", process);
//                         await Task.Yield();
//                     }
//
//                     var path = AssetDatabase.GUIDToAssetPath(guid);
//                     var importer = AssetImporter.GetAtPath(path) as TextureImporter;
//                     if (importer != null && importer.npotScale != npotScale)
//                     {
//                         importer.npotScale = npotScale;
//                         importer.SaveAndReimport();
//                     }
//                 }
//
//                 Debug.Log($"处理完毕，共：{total}项");
//                 EditorUtility.ClearProgressBar();
//             }
//             else
//             {
//                 var path = AssetDatabase.GUIDToAssetPath(data.Guid);
//                 var importer = AssetImporter.GetAtPath(path) as TextureImporter;
//                 if (importer != null && importer.npotScale != npotScale)
//                 {
//                     importer.npotScale = npotScale;
//                     importer.SaveAndReimport();
//                 }
//             }
//         }
//
//         //是否逆序
//         private bool IsReverseSort(SortType sortType)
//         {
//             if (!_sortStatus.ContainsKey(sortType))
//             {
//                 _sortStatus[sortType] = false;
//             }
//
//             return _sortStatus[sortType];
//         }
//
//         //按名称排序
//         private void SortByName()
//         {
//             var reverse = IsReverseSort(SortType.Name);
//             _sortStatus[SortType.Name] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             var dic = new Dictionary<string, string>();
//             //读取名称
//             foreach (var item in items)
//             {
//                 var tex = GetPreviewTexture(item.Guid);
//                 if (tex == null)
//                 {
//                     tex = AssetDatabase.LoadAssetAtPath<Texture>(AssetDatabase.GUIDToAssetPath(item.Guid));
//                 }
//
//                 dic[item.Guid] = tex == null ? "" : tex.name;
//             }
//
//             items.Sort((a, b) =>
//             {
//                 var (ga, gb) = !reverse ? (a, b) : (b, a);
//                 var result = String.Compare(dic[ga.Guid], dic[gb.Guid], StringComparison.Ordinal);
//                 if (result == 0)
//                 {
//                     result = a.SortOrder.CompareTo(b.SortOrder);
//                 }
//
//                 return result;
//             });
//         }
//
//         //按Override排序
//         private void SortByOverride()
//         {
//             var reverse = IsReverseSort(SortType.Override);
//             _sortStatus[SortType.Override] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             //读取名称
//             items.Sort((a, b) =>
//             {
//                 var (ga, gb) = !reverse ? (a, b) : (b, a);
//                 var result = ga.Override.CompareTo(gb.Override);
//                 if (result == 0)
//                 {
//                     result = a.SortOrder.CompareTo(b.SortOrder);
//                 }
//
//                 return result;
//             });
//         }
//
//         //按Format排序
//         private async void SortByFormat()
//         {
//             var reverse = IsReverseSort(SortType.Format);
//             _sortStatus[SortType.Format] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             var formats = new Dictionary<string, TextureImporterFormat>();
//             var buildTargetStr = CurrentSelect.BuildTarget.ToString();
//             var total = items.Count;
//             var current = 0;
//             var counter = 0;
//             //读取Importer
//             foreach (var item in items)
//             {
//                 current += 1;
//                 counter += 1;
//                 var process = (float)current / total;
//
//                 var guid = item.Guid;
//                 var importer = CompressTextureTool.GetImporter(guid);
//                 if (importer == null)
//                 {
//                     importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter;
//                 }
//
//                 var format = item.ImporterFormat;
//                 if (!item.Override || item.ImporterFormat == TextureImporterFormat.Automatic)
//                 {
//                     format = importer.GetAutomaticFormat(buildTargetStr);
//                 }
//
//                 formats[guid] = format;
//                 if (counter > 200)
//                 {
//                     counter = 0;
//                     DisplayInSideProcessBar("排序中...请稍等...", process);
//                     await Task.Yield();
//                     Repaint();
//                 }
//             }
//
//             ClearInSideProcessBar();
//             items.Sort((a, b) =>
//             {
//                 var (ga, gb) = !reverse ? (a, b) : (b, a);
//                 var result = formats[ga.Guid].CompareTo(formats[gb.Guid]);
//                 if (result == 0)
//                 {
//                     result = a.SortOrder.CompareTo(b.SortOrder);
//                 }
//
//                 return result;
//             });
//         }
//
//         //按maxsize排序
//         private void SortByMaxSize()
//         {
//             var reverse = IsReverseSort(SortType.MaxSize);
//             _sortStatus[SortType.MaxSize] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             var sizes = new Dictionary<string, int>();
//             //读取Importer
//             foreach (var item in items)
//             {
//                 var guid = item.Guid;
//                 var importer = CompressTextureTool.GetImporter(guid);
//                 if (importer == null)
//                 {
//                     importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter;
//                 }
//
//                 var size = item.MaxSize;
//                 if (item.Override)
//                 {
//                     size = importer.GetDefaultPlatformTextureSettings().maxTextureSize;
//                 }
//
//                 sizes[guid] = size;
//             }
//
//             items.Sort((a, b) =>
//             {
//                 var (sizeA, sizeB) = !reverse ? (a, b) : (b, a);
//                 var result = sizes[sizeA.Guid].CompareTo(sizes[sizeB.Guid]);
//                 if (result == 0)
//                 {
//                     result = a.SortOrder.CompareTo(b.SortOrder);
//                 }
//
//                 return result;
//             });
//         }
//
//         //按资源分辨率大小排序
//         private void SortBySourceSize()
//         {
//             var reverse = IsReverseSort(SortType.SourceSize);
//             _sortStatus[SortType.SourceSize] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             var sizes = new Dictionary<string, int>();
//             //读取Importer
//             foreach (var item in items)
//             {
//                 var guid = item.Guid;
//                 var importer = CompressTextureTool.GetImporter(guid);
//                 if (importer == null)
//                 {
//                     importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter;
//                 }
//
//                 var source = CompressTextureTool.GetSourceTextureInformation(importer);
//                 var size = source.width * source.height;
//                 sizes[guid] = size;
//             }
//
//             items.Sort((a, b) =>
//             {
//                 var (sizeA, sizeB) = !reverse ? (a, b) : (b, a);
//                 var result = sizes[sizeA.Guid].CompareTo(sizes[sizeB.Guid]);
//                 if (result == 0)
//                 {
//                     result = a.SortOrder.CompareTo(b.SortOrder);
//                 }
//
//                 return result;
//             });
//         }
//
//         //按MipMap开关排序
//         private void SortByMipMap()
//         {
//             var reverse = IsReverseSort(SortType.MipMap);
//             _sortStatus[SortType.MipMap] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             var status = new Dictionary<string, bool>();
//             //读取Importer
//             foreach (var item in items)
//             {
//                 var guid = item.Guid;
//                 var importer = CompressTextureTool.GetImporter(guid);
//                 if (importer == null)
//                 {
//                     importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter;
//                 }
//
//                 status[guid] = importer.mipmapEnabled;
//             }
//
//             items.Sort((a, b) =>
//             {
//                 var (ga, gb) = !reverse ? (a, b) : (b, a);
//                 var result = status[ga.Guid].CompareTo(status[gb.Guid]);
//                 if (result == 0)
//                 {
//                     result = a.SortOrder.CompareTo(b.SortOrder);
//                 }
//
//                 return result;
//             });
//         }
//
//         //按Npot排序
//         private void SortByNpot()
//         {
//             var reverse = IsReverseSort(SortType.NPot);
//             _sortStatus[SortType.NPot] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             var types = new Dictionary<string, TextureImporterNPOTScale>();
//             //读取Importer
//             foreach (var item in items)
//             {
//                 var guid = item.Guid;
//                 var importer = CompressTextureTool.GetImporter(guid);
//                 if (importer == null)
//                 {
//                     importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter;
//                 }
//
//                 types[guid] = importer.npotScale;
//             }
//
//             items.Sort((a, b) =>
//             {
//                 var (ga, gb) = !reverse ? (a, b) : (b, a);
//                 var result = types[ga.Guid].CompareTo(types[gb.Guid]);
//                 if (result == 0)
//                 {
//                     result = a.SortOrder.CompareTo(b.SortOrder);
//                 }
//
//                 return result;
//             });
//         }
//
//         //按图片类型排序
//         private void SortByTexType()
//         {
//             var reverse = IsReverseSort(SortType.TextureType);
//             _sortStatus[SortType.TextureType] = !reverse;
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             var types = new Dictionary<string, TextureImporterType>();
//             var shapes = new Dictionary<string, TextureImporterShape>();
//             //读取Importer
//             foreach (var item in items)
//             {
//                 var guid = item.Guid;
//                 var importer = CompressTextureTool.GetImporter(guid);
//                 if (importer == null)
//                 {
//                     importer = AssetImporter.GetAtPath(AssetDatabase.GUIDToAssetPath(guid)) as TextureImporter;
//                 }
//
//                 types[guid] = importer.textureType;
//                 shapes[guid] = importer.textureShape;
//             }
//
//             items.Sort((a, b) =>
//             {
//                 var (ga, gb) = !reverse ? (a, b) : (b, a);
//                 var result = types[ga.Guid].CompareTo(types[gb.Guid]);
//                 if (result == 0)
//                 {
//                     result = shapes[ga.Guid].CompareTo(shapes[gb.Guid]);
//                     if (result == 0)
//                     {
//                         result = a.SortOrder.CompareTo(b.SortOrder);
//                     }
//                 }
//
//                 return result;
//             });
//         }
//
//         //默认排序
//         private void SortByDefault()
//         {
//             var items = _matchItems;
//             if (items == null || items.Count <= 0)
//             {
//                 return;
//             }
//
//             items.Sort((a, b) =>
//             {
//                 return a.SortOrder.CompareTo(b.SortOrder);
//                 ;
//             });
//         }
//
//         private void OnDisable()
//         {
//             CompressTextureTool.SaveAllConfig();
//         }
//
//         private void HandleApply()
//         {
//             if (_applyAction != ClickApplyAction.None)
//             {
//                 var action = _applyAction;
//                 _applyAction = ClickApplyAction.None;
//                 switch (action)
//                 {
//                     case ClickApplyAction.MatchItems:
//                     {
//                         if (CurrentSelect != null && _matchItems != null && _matchItems.Count > 0)
//                         {
//                             var guids = new HashSet<string>();
//                             foreach (var item in _matchItems)
//                             {
//                                 guids.Add(item.Guid);
//                             }
//
//                             CurrentSelect.Save();
//                             CompressTextureTool.Apply(CurrentSelect, guids);
//                         }
//                     }
//                         break;
//                     case ClickApplyAction.SelectItems:
//                     {
//                         if (CurrentSelect != null && _selectGuids != null && _selectGuids.Count > 0)
//                         {
//                             var guids = new HashSet<string>(_selectGuids);
//                             CurrentSelect.Save();
//                             CompressTextureTool.Apply(CurrentSelect, guids);
//                         }
//                     }
//                         break;
//                 }
//             }
//         }
//
//         //反射获取类型
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
//         //反射获取有效的枚举
//         private static T[] GetValidEnumValues<T>() where T : Enum
//         {
//             var type = typeof(T);
//             var match = new HashSet<string>();
//             var names = Enum.GetNames(typeof(T));
//             foreach (var element in names)
//             {
//                 var fieldInfo = type.GetField(element);
//                 // 检查特性是否废弃
//                 if (fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0)
//                 {
//                     continue;
//                 }
//                 else
//                 {
//                     match.Add(fieldInfo.Name);
//                 }
//             }
//
//             List<T> result = new List<T>();
//             foreach (var value in match)
//             {
//                 result.Add((T)Enum.Parse(typeof(T), value));
//             }
//
//             return result.ToArray();
//         }
//     }
// }