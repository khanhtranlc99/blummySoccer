using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;
#if UNITY_2020_1_OR_NEWER
using SourceTextureInformation = UnityEditor.AssetImporters.SourceTextureInformation;
#else
using SourceTextureInformation = UnityEditor.Experimental.AssetImporters.SourceTextureInformation;
#endif

namespace XGame
{
    /// <summary>
    /// 压缩工具
    /// </summary>
    public class XCompressTool : EditorWindow
    {
        public struct PathDepth
        {
            public string Path;
            public string Name;
            public int Depth;
            public List<string> Paths;

            public PathDepth(string path, string name, int depth, List<string> paths)
            {
                Path = path;
                Name = name;
                Depth = depth;
                Paths = paths;
            }
        }

        public class FolderTree : TreeView
        {
            public class ItemData
            {
                public PathDepth PathDepth;
                public int? Count;

                public ItemData(PathDepth pathDepth)
                {
                    PathDepth = pathDepth;
                }
            }

            //文件夹-图片数量字典1（原始路径）
            private Dictionary<string, int> _originalTexMap = new Dictionary<string, int>();

            //文件夹-图片数量字典2（unity路径使用/）
            private Dictionary<string, int> _folderTexMap = new Dictionary<string, int>();
            private Dictionary<int, ItemData> _idMap = new Dictionary<int, ItemData>();
            private HashSet<string> _filterFolders = new HashSet<string>();

            public event Action OnFolderFilterChanged = null;

            public event Action OnClickTreeItem = null;

            private int _pingId;

            public FolderTree(TreeViewState state, Dictionary<string, int> folderMap) : base(state)
            {
                _originalTexMap = folderMap;
                foreach (var pair in folderMap)
                {
                    var path = pair.Key.Replace("\\", "/");
                    _folderTexMap[path] = pair.Value;
                }

                Reload();
            }

            protected override TreeViewItem BuildRoot()
            {
                var root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
                var depthMap = new Dictionary<string, PathDepth>();
                var folders = _folderTexMap.Keys.ToList();
                //拆分文件夹目录
                foreach (var folder in folders)
                {
                    var depths = ParseToPathDepths(folder);
                    foreach (var depth in depths)
                    {
                        depthMap[depth.Path] = depth;
                    }
                }

                var list = depthMap.Values.ToList();
                //深度排序
                list.Sort((a, b) =>
                {
                    var pathA = a.Paths;
                    var pathB = b.Paths;
                    var result = 0;
                    for (int i = 0; i < pathA.Count && i < pathB.Count; i++)
                    {
                        var nameA = pathA[i];
                        var nameB = pathB[i];
                        result = String.Compare(nameA, nameB, StringComparison.Ordinal);
                        if (result != 0)
                        {
                            return result;
                        }
                    }

                    return pathA.Count.CompareTo(pathB.Count);
                });
                var tree = new List<TreeViewItem>();
                var id = 0;
                //生成层级树
                foreach (var element in list)
                {
                    id += 1;
                    tree.Add(new TreeViewItem(id, element.Depth, element.Name));
                    _idMap[id] = new ItemData(element);
                }

                //根据层级生成
                SetupParentsAndChildrenFromDepths(root, tree);
                return root;
            }

            private List<PathDepth> ParseToPathDepths(string path)
            {
                //创建
                var depths = new List<PathDepth>();
                var stack = new Stack<string>();
                var check = path;
                while (!string.IsNullOrEmpty(check))
                {
                    var name = Path.GetFileName(check);
                    check = Path.GetDirectoryName(check);
                    stack.Push(name);
                }

                var sb = new StringBuilder();
                var depth = 0;
                var paths = new List<string>();
                while (stack.Count > 0)
                {
                    var name = stack.Pop();
                    if (depth != 0)
                    {
                        sb.Append("/");
                    }

                    paths.Add(name);
                    sb.Append(name);
                    depth += 1;
                    depths.Add(new PathDepth(sb.ToString(), name, depth, paths.ToList()));
                }

                return depths;
            }

            public bool IsChanged(Dictionary<string, int> folderMap)
            {
                if (_originalTexMap.Count != folderMap.Count)
                {
                    return true;
                }

                foreach (var pair in _originalTexMap)
                {
                    var key = pair.Key;
                    var value = pair.Value;
                    if (folderMap.TryGetValue(key, out var match))
                    {
                        if (match == value)
                        {
                            continue;
                        }
                    }

                    return true;
                }

                return false;
            }

            private int GetTextureCount(int id)
            {
                var data = _idMap[id];
                if (data.Count == null)
                {
                    var total = 0;
                    foreach (var pair in _folderTexMap)
                    {
                        var key = pair.Key;
                        var count = pair.Value;
                        if (key.StartsWith(data.PathDepth.Path))
                        {
                            total += count;
                        }
                    }

                    data.Count = total;
                }

                return data.Count.Value;
            }

            protected override void RowGUI(RowGUIArgs args)
            {
                args.label = $"{args.label}（{GetTextureCount(args.item.id)}）";
                base.RowGUI(args);
                if (_pingId == args.item.id)
                {
                    var rect = args.rowRect;
                    EditorGUI.DrawRect(rect, Color.cyan * 0.3f);
                }
            }


            protected override void SingleClickedItem(int id)
            {
                base.SingleClickedItem(id);
                OnClickTreeItem?.Invoke();
            }

            protected override void SelectionChanged(IList<int> selectedIds)
            {
                base.SelectionChanged(selectedIds);
                _filterFolders.Clear();
                foreach (var id in selectedIds)
                {
                    _filterFolders.Add(_idMap[id].PathDepth.Path);
                }

                OnFolderFilterChanged?.Invoke();
            }

            public void Release()
            {
                OnFolderFilterChanged = null;
                OnClickTreeItem = null;
            }

            public HashSet<string> GetFilterFolder() => _filterFolders;

            public bool TryGetItemDataFormTexturePath(string path, out ItemData data, out int id)
            {
                id = -1;
                data = null;
                if (string.IsNullOrEmpty(path))
                {
                    return false;
                }

                var folder = Path.GetDirectoryName(path);
                folder = folder.Replace("\\", "/");
                foreach (var pair in _idMap)
                {
                    var key = pair.Key;
                    var value = pair.Value;
                    if (value.PathDepth.Path == folder)
                    {
                        id = key;
                        data = value;
                        return true;
                    }
                }

                return false;
            }


            public List<int> FindTreeItemIdsByFolder(string folder)
            {
                var result = new List<int>();
                foreach (var pair in _idMap)
                {
                    if (folder.StartsWith(pair.Value.PathDepth.Path))
                    {
                        result.Add(pair.Key);
                    }
                }

                return result;
            }

            public void PingFolder(string texturePath, bool expand)
            {
                _pingId = -1;
                if (TryGetItemDataFormTexturePath(texturePath, out var match, out var id))
                {
                    _pingId = id;
                    if (expand)
                    {
                        var ids = FindTreeItemIdsByFolder(_idMap[id].PathDepth.Path); //GetExpanded().Union().ToList();
                        SetExpanded(ids);
                    }
                }
            }
        }

        private enum SelectMode
        {
            Single,
            Shift,
            Ctrl,
        }

        //焦点场景
        private enum FocusScene
        {
            FolderTree,
            ItemView,
        }

        //GUI布局场景
        private enum LayoutScene
        {
            Left,
            Center,
        }

        //图片信息
        public class TextureInfo
        {
            public string Guid;
            private Texture _texture;

            private Dictionary<BuildTarget, TextureImporterFormat> _defaultFormatMap =
                new Dictionary<BuildTarget, TextureImporterFormat>();

            public Texture Texture
            {
                get
                {
                    if (_texture == null)
                    {
                        _texture = AssetDatabase.LoadAssetAtPath<Texture>(GetImporter(Guid).assetPath);
                    }

                    return _texture;
                }
            }

            public TextureImporterFormat DefaultFormat
            {
                get
                {
                    if (_defaultFormatMap.TryGetValue(_filterBuildTarget, out var match))
                    {
                        return match;
                    }

                    var platform = _filterBuildTarget.ToString();
#if UNITY_2019_3_OR_NEWER
                    platform = BuildPipeline.GetBuildTargetName(_filterBuildTarget);
#endif
                    var format = GetImporter(Guid)
                        .GetAutomaticFormat(platform);
                    _defaultFormatMap[_filterBuildTarget] = format;
                    return format;
                }
            }

            public TextureImporterPlatformSettings Settings
            {
                get
                {
                    var platform = _filterBuildTarget.ToString();
#if UNITY_2019_3_OR_NEWER
                    platform = BuildPipeline.GetBuildTargetName(_filterBuildTarget);
#endif
                    return GetImporter(Guid)
                        .GetPlatformTextureSettings(platform);
                }
            }

            public string Name => Texture.name;
            public SourceTextureInformation SourceInfo;
        }

        //属性栏配置
        public class PropertyTabBinding
        {
            public PropertyTab EnumType;
            public float LabelWidth;
            public bool CanSort => SortDelegate != null;
            public Action<List<string>, bool> SortDelegate;


            public PropertyTabBinding(PropertyTab enumType, float labelWidth,
                Action<List<string>, bool> sortDelegate)
            {
                EnumType = enumType;
                LabelWidth = labelWidth;
                SortDelegate = sortDelegate;
            }
        }

        //元素高度
        private const float ITEM_HEIGHT = 22;
        private static int PROPERTY_TITLE_BAR_HEIGHT = 24;
        private const float ITEM_EDGE_RANGE = 60;
        private const float INSPECTOR_VIEW_HEIGHT = 1000;
        private const float PREVIEW_HEIGHT = 260;
        private static RectOffset _item_padding = null;

        private static Dictionary<string, int> _lastSortMap = new Dictionary<string, int>();

        private static RectOffset ITEM_PADDING
        {
            get
            {
                if (_item_padding == null)
                {
                    _item_padding = new RectOffset(10, 10, PROPERTY_TITLE_BAR_HEIGHT + 2, 30);
                }

                return _item_padding;
            }
        }


        private static Dictionary<string, string> FormatNames = new Dictionary<string, string>()
        {
            { "Alpha8", "Alpha 8" },
            { "ARGB16", "ARGB 16 bits" },
            { "RGB24", "RGB 24 bits" },
            { "RGBA32", "RGBA 32 bits" },
            { "ARGB32", "ARGB 32 bits" },
            { "RGB16", "RGB 16 bits" },
            { "R16", "R 16 bits" },
            { "DXT1", "RGB DXT 1" },
            { "DXT5", "RGBA DXT 5" },
            { "RGBA16", "RGBA 16 bits" },
            { "RHalf", "R Half" },
            { "RGHalf", "RG Half" },
            { "RGBAHalf", "RGBA Half" },
            { "RFloat", "R Float" },
            { "RGFloat", "RG Float" },
            { "RGBAFloat", "RGBA Float" },
            { "RGB9E5", "RGB9e5 32 bits" },
            { "BC6H", "RGB HDR BC6H" },
            { "BC7", "RGBA BC7" },
            { "BC4", "R BC4" },
            { "BC5", "RG BC5" },
            { "DXT1Crunched", "RGB Crunched DXT 1" },
            { "DXT5Crunched", "RGBA Crunched DXT 5" },
            { "PVRTC_RGB2", "RGB PVRTC 2 bits" },
            { "PVRTC_RGBA2", "RGBA PVRTC 2 bits" },
            { "PVRTC_RGB4", "RGB PVRTC 4 bits" },
            { "PVRTC_RGBA4", "RGBA PVRTC 4 bits" },
            { "ETC_RGB4", "RGB ETC 4 bits" },
            { "EAC_R", "R EAC 4 bits" },
            { "EAC_R_SIGNED", "R EAC Signed 4 bits" },
            { "EAC_RG", "RG EAC 8 bits" },
            { "EAC_RG_SIGNED", "RG EAC Signed 8 bits" },
            { "ETC2_RGB4", "RGB ETC2 4 bits" },
            { "ETC2_RGB4_PUNCHTHROUGH_ALPHA", "RGB +1-bit Alpha ETC2 4 bits" },
            { "ETC2_RGBA8", "RGBA ETC2 8 bits" },
            { "ASTC_4x4", "RGB(A) ASTC 4x4 block" },
            { "ASTC_RGB_4x4", "RGB ASTC 4x4 block" },
            { "ASTC_5x5", "RGB(A) ASTC 5x5 block" },
            { "ASTC_RGB_5x5", "RGB ASTC 5x5 block" },
            { "ASTC_6x6", "RGB(A) ASTC 6x6 block" },
            { "ASTC_RGB_6x6", "RGB ASTC 6x6 block" },
            { "ASTC_8x8", "RGB(A) ASTC 8x8 block" },
            { "ASTC_RGB_8x8", "RGB ASTC 8x8 block" },
            { "ASTC_10x10", "RGB(A) ASTC 10x10 block" },
            { "ASTC_RGB_10x10", "RGB ASTC 10x10 block" },
            { "ASTC_12x12", "RGB(A) ASTC 12x12 block" },
            { "ASTC_RGB_12x12", "RGB ASTC 12x12 block" },
            { "ASTC_RGBA_4x4", "RGBA ASTC 4x4 block" },
            { "ASTC_RGBA_5x5", "RGBA ASTC 5x5 block" },
            { "ASTC_RGBA_6x6", "RGBA ASTC 6x6 block" },
            { "ASTC_RGBA_8x8", "RGBA ASTC 8x8 block" },
            { "ASTC_RGBA_10x10", "RGBA ASTC 10x10 block" },
            { "ASTC_RGBA_12x12", "RGBA ASTC 12x12 block" },
            { "RG16", "RG 16" },
            { "R8", "R 8" },
            { "ETC_RGB4Crunched", "RGB Crunched ETC" },
            { "ETC2_RGBA8Crunched", "RGBA Crunched ETC" },
            { "ASTC_HDR_4x4", "RGB(A) ASTC HDR 4x4 block" },
            { "ASTC_HDR_5x5", "RGB(A) ASTC HDR 5x5 block" },
            { "ASTC_HDR_6x6", "RGB(A) ASTC HDR 6x6 block" },
            { "ASTC_HDR_8x8", "RGB(A) ASTC HDR 8x8 block" },
            { "ASTC_HDR_10x10", "RGB(A) ASTC HDR 10x10 block" },
            { "ASTC_HDR_12x12", "RGB(A) ASTC HDR 12x12 block" },
            { "RG32", "RG 32 bit" },
            { "RGB48", "RGB 48 bit" },
            { "RGBA64", "RGBA 64 bit" },
        };


        public enum PropertyTab
        {
            NO,
            Name,
            Override,
            Format,
            MaxSize,
            SourceSize,
            MipMap,
            NpotScale,
            Path,
            Type,
        }

        private const string CONTROL_NAME_SEARCH_BAR = "CONTROL_NAME_SEARCH_BAR";

        private const string CONTROL_FOLDER_SEARCH_BAR = "CONTROL_FOLDER_SEARCH_BAR";

        //平台选择
        private static List<BuildTarget> FILTER_BUILD_TARGET
        {
            get
            {
                if (_FILTER_BUILD_TARGET == null)
                {
                    _FILTER_BUILD_TARGET = new List<BuildTarget>();
                    var check = new Dictionary<BuildTarget, BuildTargetGroup>()
                    {
                        { BuildTarget.Android, BuildTargetGroup.Android },
                        { BuildTarget.iOS, BuildTargetGroup.iOS },
                        { BuildTarget.WebGL, BuildTargetGroup.WebGL },
                        { BuildTarget.StandaloneWindows, BuildTargetGroup.Standalone },
                        { BuildTarget.StandaloneWindows64, BuildTargetGroup.Standalone },
                        { BuildTarget.StandaloneLinux64, BuildTargetGroup.Standalone },
                    };

                    foreach (var pair in check)
                    {
                        if (BuildPipeline.IsBuildTargetSupported(pair.Value, pair.Key))
                        {
                            _FILTER_BUILD_TARGET.Add(pair.Key);
                        }
                    }
                }

                return _FILTER_BUILD_TARGET;
            }
        }


        private static List<BuildTarget> _FILTER_BUILD_TARGET = null;


        //筛选发生变化
        private static event Action OnFilterChanged = null;

        //选择项变化
        private static event Action OnSelectChanged = null;

        private static float LEFT_MENU_WIDTH = 268;

        //过滤TextureImporterType
        private static TextureImporterType[] FILTER_TEXTURE_TYPE = GetValidEnumValues<TextureImporterType>();

        //过滤TextureImporterShape
        private static TextureImporterShape[] FILTER_SHAPE_TYPE = GetValidEnumValues<TextureImporterShape>();

        private static Dictionary<TextureImporterFormat, string> _formatNamesRemap = null;

        private static string GetFormatName(TextureImporterFormat format)
        {
            if (_formatNamesRemap == null)
            {
                _formatNamesRemap = new Dictionary<TextureImporterFormat, string>();
                foreach (var kv in FormatNames)
                {
                    var key = kv.Key;
                    var name = kv.Value;
                    try
                    {
                        var enumType = (TextureImporterFormat)Enum.Parse(typeof(TextureImporterFormat), key);
                        _formatNamesRemap[enumType] = name;
                    }
                    catch (Exception e)
                    {
                    }
                }
            }

            if (_formatNamesRemap.TryGetValue(format, out var match))
            {
                return match;
            }

            return format.ToString();
        }

        //选中的实例
        private static HashSet<string> _selectGuids = new HashSet<string>();
        private static TextureImporter[] _selectImporters;
        private static HashSet<string> _selectImportersGuids = new HashSet<string>();
        private static bool _previewGUIFlag = true;
        private static Vector2 _scrollLeftWindow;
        private static bool _filterFlag = true;
        private static FolderTree _folderTree;
        private static TreeViewState _treeViewState;
        private static string _searchTreeTxt;
        private static bool _multipleInspector = false;
        private static HashSet<string> _cacheSelectTextures = new HashSet<string>();
        private static HashSet<string> _cacheSelectCubeMap = new HashSet<string>();
        private static HashSet<string> _hoverSelect = new HashSet<string>();

        //缓存的排序类型
        private static PropertyTab? _cacheSortBy = null;

        //排序状态
        private static Dictionary<PropertyTab, bool> _sortReverseMap = new Dictionary<PropertyTab, bool>();


        // [MenuItem("XGameUnityTool/工具箱/纹理压缩")]
        private static void Open()
        {
            var window = GetWindow<XCompressTool>();
            window.minSize = new Vector2(1280, 800);
            window.titleContent = new GUIContent("纹理压缩");
            window.Show();
        }


        /// <summary>
        /// TextureImporter
        /// </summary>
        private static Dictionary<string, TextureImporter> _importers = new Dictionary<string, TextureImporter>();

        private static Dictionary<string, TextureInfo> _infos = new Dictionary<string, TextureInfo>();

        //默认排序
        private static Dictionary<string, int> _defaultOrders = new Dictionary<string, int>();

        private Dictionary<string, Rect> _itemsRects = new Dictionary<string, Rect>();


        private static Vector2 _scroll;

        //过滤图片类型
        private static HashSet<TextureImporterType> _filterTextureType =
            new HashSet<TextureImporterType>(FILTER_TEXTURE_TYPE);

        //过滤shape类型
        private static HashSet<TextureImporterShape> _filterShapeType =
            new HashSet<TextureImporterShape>(FILTER_SHAPE_TYPE);

        //过滤平台
        private static BuildTarget _filterBuildTarget = BuildTarget.Android;

        private static Editor _assetEditor = null;
        private static Editor _inspectorEditor = null;
        private static Editor InspectorEditor => _inspectorEditor;

        private static PropertyTabBinding[] _propertyTabBindings = new[]
        {
            new PropertyTabBinding(PropertyTab.NO, 40, null),
            new PropertyTabBinding(PropertyTab.Name, 260, SortByName),
            new PropertyTabBinding(PropertyTab.Override, 90, SortByOverride),
            new PropertyTabBinding(PropertyTab.Format, 260, SortByFormat),
            new PropertyTabBinding(PropertyTab.MaxSize, 90, SortByMaxSize),
            new PropertyTabBinding(PropertyTab.SourceSize, 120, SortBySourceSize),
            new PropertyTabBinding(PropertyTab.MipMap, 90, SortByMipMap),
            new PropertyTabBinding(PropertyTab.NpotScale, 120, SortByNpotScale),
            new PropertyTabBinding(PropertyTab.Type, 140, SortByTextureType),
            new PropertyTabBinding(PropertyTab.Path, 800, SortByPath),
        };

        private static Dictionary<PropertyTab, PropertyTabBinding> _propertyTabMap = null;

        //属性宽度
        private static Dictionary<PropertyTab, PropertyTabBinding> PropertyTabMap
        {
            get
            {
                if (_propertyTabMap == null)
                {
                    _propertyTabMap = new Dictionary<PropertyTab, PropertyTabBinding>();
                    foreach (var binding in _propertyTabBindings)
                    {
                        _propertyTabMap[binding.EnumType] = binding;
                    }
                }

                return _propertyTabMap;
            }
        }


        //区域范围
        private static Rect _leftArea;
        private static Rect _centerArea;
        private static Rect _inspectorArea;
        private static Rect _searchBarArea;
        private static Rect _itemsArea;
        private static Rect _searchFolderArea;


        //搜索栏
        private static string _searchTxt = "";
        private static Rect _searchBarRange;
        private static Vector2 _itemWindowScroll;
        private List<string> _matchItems = new List<string>();
        private static int _inspectorWidth = 360;
        private static Vector2 _scrollInspector;
        private string _lastSelectGuid;
        private FocusScene _focusScene = FocusScene.ItemView;
        private LayoutScene _lastLayoutScene = LayoutScene.Center;

        private async void OnEnable()
        {
            //读取当前配置
            _filterBuildTarget = EditorUserBuildSettings.activeBuildTarget;
            _filterTextureType = new HashSet<TextureImporterType>(FILTER_TEXTURE_TYPE);
            _filterShapeType = new HashSet<TextureImporterShape>(FILTER_SHAPE_TYPE);
            _folderTree?.Release();
            _folderTree = null;
            await UpdateDatabaseAsync();
            UpdateMatchItems();
            OnSelectChanged -= WhenSelectChanged;
            OnSelectChanged += WhenSelectChanged;
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling && (_assetEditor != null || _inspectorEditor != null))
            {
                DestroyImmediate(_assetEditor, true);
                DestroyImmediate(_inspectorEditor, true);
            }

            HandleEvent();
            var rect = new Rect(0, 0, position.width, position.height);
            var rectLeftMenu = new Rect(0, 0, LEFT_MENU_WIDTH, rect.height);
            _leftArea = rectLeftMenu;
            DrawLeftGUI(rectLeftMenu);
            var rectLine = rectLeftMenu;
            rectLine.width = 1;
            rectLine.x = rectLeftMenu.xMax;
            EditorGUI.DrawRect(rectLine, Color.black);
            var rectCenter = rectLeftMenu;
            rectCenter.x = rectLeftMenu.xMax;
            rectCenter.width = rect.xMax - rectLeftMenu.xMax;
            rectCenter.width -= _inspectorWidth;
            rectCenter.width -= 2;
            _centerArea = rectCenter;

            DrawCenterGUI(rectCenter);

            rectLine = rectCenter;
            rectLine.width = 1;
            rectLine.x = rectCenter.xMax;
            EditorGUI.DrawRect(rectLine, Color.black);

            var rectInspector = rectCenter;
            rectInspector.x = rectCenter.xMax;
            rectInspector.x += 2;
            rectInspector.width = rect.xMax - rectInspector.x;
            _inspectorArea = rectInspector;
            DrawRightGUI(rectInspector);
        }

        private async void OnFocus()
        {
            OnSelectChanged -= WhenSelectChanged;
            OnSelectChanged += WhenSelectChanged;
            OnFilterChanged -= WhenFilterChanged;
            OnFilterChanged += WhenFilterChanged;
            await UpdateDatabaseAsync();
            UpdateMatchItems(false);
        }

        //异步加载纹理数据库
        private async Task UpdateDatabaseAsync()
        {
            //缓存旧数据
            var cache = _importers;
            _importers = new Dictionary<string, TextureImporter>();
            var guids = AssetDatabase.FindAssets("t:texture", new[] { "Assets" });
            _defaultOrders.Clear();
            var order = 0;
            foreach (var guid in guids)
            {
                order += 1;
                _defaultOrders[guid] = order;
            }

            var append = new HashSet<string>();
            foreach (var guid in guids)
            {
                if (cache.TryGetValue(guid, out var match))
                {
                    //存在缓存条目
                    if (match != null)
                    {
                        _importers[guid] = match;
                    }
                    else
                    {
                        append.Add(guid);
                    }
                }
                else
                {
                    //如果没有条目
                    append.Add(guid);
                }
            }

            var total = append.Count;
            var current = 0;
            var nextCheck = EditorApplication.timeSinceStartup + 1f;
            foreach (var element in append)
            {
                current += 1;
                var process = (float)current / total;
                var path = AssetDatabase.GUIDToAssetPath(element);
                var importer = AssetImporter.GetAtPath(path) as TextureImporter;
                if (importer != null)
                {
                    _importers[element] = importer;
                }

                if (EditorApplication.timeSinceStartup > nextCheck)
                {
                    EditorUtility.DisplayProgressBar("Please wait...", $"Reading textures... {(process * 100).ToString("F1")}%", process);
                    nextCheck = EditorApplication.timeSinceStartup + 2;
                    await Task.Yield();
                }
            }

            //更新文件夹
            var folderMap = new Dictionary<string, int>();
            foreach (var pair in _importers)
            {
                var folder = Path.GetDirectoryName(pair.Value.assetPath);
                if (folder != null)
                {
                    if (!folderMap.ContainsKey(folder))
                    {
                        folderMap[folder] = 0;
                    }

                    folderMap[folder] += 1;
                }
            }

            //查看是否一致
            if (_folderTree == null || _folderTree.IsChanged(folderMap))
            {
                _folderTree?.Release();
                _treeViewState = new TreeViewState();
                _folderTree = new FolderTree(_treeViewState, folderMap);
                _folderTree.OnFolderFilterChanged += WhenFolderFilterChanged;
                _folderTree.OnClickTreeItem += WhenClickFolderTree;
            }

            EditorUtility.ClearProgressBar();
        }


        private void WhenFolderFilterChanged()
        {
            UpdateMatchItems();
        }

        //更新匹配项
        private void UpdateMatchItems(bool resort = true)
        {
            _matchItems = new List<string>();
            var filterFolder = new HashSet<string>();
            if (_folderTree != null)
            {
                filterFolder = new HashSet<string>(_folderTree.GetFilterFolder());
            }

            //从文件夹中过滤
            var searchTxtLow = string.IsNullOrEmpty(_searchTxt) ? "" : _searchTxt.ToLower();
            foreach (var pair in _importers)
            {
                var guid = pair.Key;
                var import = pair.Value;
                var inside = filterFolder.Count <= 0;
                foreach (var filter in filterFolder)
                {
                    if (import.assetPath.StartsWith(filter))
                    {
                        inside = true;
                        break;
                    }
                }

                if (!inside)
                {
                    continue;
                }

                if (!_filterTextureType.Contains(import.textureType))
                {
                    continue;
                }

                if (!_filterShapeType.Contains(import.textureShape))
                {
                    continue;
                }

                if (!string.IsNullOrEmpty(searchTxtLow))
                {
                    if (!import.assetPath.ToLower().Contains(searchTxtLow))
                    {
                        continue;
                    }
                }

                _matchItems.Add(guid);
            }

            if (resort)
            {
                if (_cacheSortBy == null)
                {
                    //按默认排序
                    SortByDefault();
                }
                else
                {
                    var tab = _cacheSortBy.Value;
                    var binding = GetPropertyTabBinding(tab);
                    binding.SortDelegate?.Invoke(_matchItems, GetSortState(tab));
                }

                _lastSortMap.Clear();
                var index = 0;
                foreach (var guid in _matchItems)
                {
                    index += 1;
                    _lastSortMap[guid] = index;
                }
            }
            else
            {
                //同步上一次的排序
                _matchItems.Sort((a, b) =>
                {
                    if (!_lastSortMap.TryGetValue(a, out var indexA))
                    {
                        indexA = -1;
                    }

                    if (!_lastSortMap.TryGetValue(b, out var indexB))
                    {
                        indexB = -1;
                    }

                    return indexA.CompareTo(indexB);
                });
            }
        }

        #region 左侧GUI

        //左侧GUI
        private void DrawLeftGUI(Rect view)
        {
            EditorGUI.DrawRect(view, Color.black * 0.2f);
            var menuView = view;
            var center = menuView.center;
            menuView.width -= 8;
            menuView.height -= 8;
            menuView.center = center;
            GUILayout.BeginArea(menuView);
            _scrollLeftWindow = GUILayout.BeginScrollView(_scrollLeftWindow);
            DrawFilterBuildTargetGUI();
            GUILayout.Space(3);
            var last = DrawHeader("目录", GUILayout.Height(24));
            var rectSearchTree = last;
            rectSearchTree.height = 20;
            rectSearchTree.y = last.yMax;
            var searchTxtArea = rectSearchTree;
            searchTxtArea.width -= 20;
            var clearBtnArea = searchTxtArea;
            clearBtnArea.width = rectSearchTree.xMax - searchTxtArea.xMax;
            clearBtnArea.x = searchTxtArea.xMax;
            clearBtnArea.width -= 2;
            clearBtnArea.x += 1;
            if (_folderTree != null)
            {
                GUI.SetNextControlName(CONTROL_FOLDER_SEARCH_BAR);
                _folderTree.searchString = GUI.TextArea(searchTxtArea, _folderTree.searchString);
                if (GUI.GetNameOfFocusedControl() == CONTROL_FOLDER_SEARCH_BAR)
                {
                    EditorGUI.DrawRect(searchTxtArea, Color.blue * 0.4f);
                }

                if (GUI.Button(clearBtnArea, "×"))
                {
                    _folderTree.searchString = string.Empty;
                    GUI.FocusControl("");
                }

                if (string.IsNullOrEmpty(_folderTree.searchString) && GUI.GetNameOfFocusedControl() !=
                    CONTROL_FOLDER_SEARCH_BAR)
                {
                    GUI.Label(searchTxtArea, "搜索...", Styles.SearchTips);
                }
            }

            var rectFolderTree = menuView;
            rectFolderTree.y = rectSearchTree.yMax;
            rectFolderTree.y += 2;
            rectFolderTree.height = menuView.yMax - rectFolderTree.y;
            rectFolderTree.width += 4;
            rectFolderTree.x -= 4;
            if (_folderTree != null)
            {
                _folderTree.OnGUI(rectFolderTree);
            }

            _searchFolderArea = rectSearchTree;
            _searchFolderArea.position += menuView.position;
            //目录
            // DrawFolderTree();
            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }

        private void DrawFilterBuildTargetGUI()
        {
            var rect = DrawHeader("所选平台:", GUILayout.Height(26));
            var btnPopUp = CreateRightTopRect(rect, rect.width - 80, 4);
            if (!FILTER_BUILD_TARGET.Contains(_filterBuildTarget))
            {
                SelectBuildTarget(FILTER_BUILD_TARGET.First());
            }

            var btnTxt = _filterBuildTarget.ToString();
            if (GUI.Button(btnPopUp, btnTxt, "PopUp"))
            {
                var menu = new GenericMenu();
                foreach (var buildTarget in FILTER_BUILD_TARGET)
                {
                    menu.AddItem(new GUIContent(buildTarget.ToString()), false, () =>
                    {
                        _filterBuildTarget = buildTarget;
                        //重置选择
                        EditorUserBuildSettings.selectedBuildTargetGroup =
                            BuildPipeline.GetBuildTargetGroup(buildTarget);
                    });
                }

                menu.ShowAsContext();
            }
        }

        private void SelectBuildTarget(BuildTarget target)
        {
            _filterBuildTarget = target;
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        //快速过滤工具栏
        private void DrawFastFilterToolBar()
        {
            var rect = DrawHeader("过滤器", GUILayout.Height(24));
            var rectBtnFlag = CreateRightTopRect(rect, 60, 2);
            var txt = _filterFlag ? "收起▲" : "展开▼";
            if (GUI.Button(rectBtnFlag, txt))
            {
                _filterFlag = !_filterFlag;
            }

            if (_filterFlag)
            {
                GUILayout.Space(3);
                GUILayout.BeginHorizontal();
                GUILayout.Label("快速过滤：", GUILayout.Width(90));
                if (GUILayout.Button("所有", GUILayout.Width(100)))
                {
                    _filterTextureType = new HashSet<TextureImporterType>(FILTER_TEXTURE_TYPE);
                    _filterShapeType = new HashSet<TextureImporterShape>(FILTER_SHAPE_TYPE);
                    ClearSortCache();
                    OnFilterChanged?.Invoke();
                }

                if (GUILayout.Button("Default(2D)", GUILayout.Width(100)))
                {
                    _filterTextureType = new HashSet<TextureImporterType>(new[] { TextureImporterType.Default });
                    _filterShapeType = new HashSet<TextureImporterShape>(new[] { TextureImporterShape.Texture2D });
                    ClearSortCache();
                    OnFilterChanged?.Invoke();
                }

                if (GUILayout.Button("Sprite(2D)", GUILayout.Width(100)))
                {
                    _filterTextureType = new HashSet<TextureImporterType>(new[] { TextureImporterType.Sprite });
                    _filterShapeType = new HashSet<TextureImporterShape>(new[] { TextureImporterShape.Texture2D });
                    ClearSortCache();
                    OnFilterChanged?.Invoke();
                }

                GUILayout.EndHorizontal();
            }
        }

        //过滤Texture Type
        private void DrawFilterTextureImporterTypeGUI()
        {
            if (_filterFlag)
            {
                GUILayout.Space(3);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Texture Type：", GUILayout.Width(90));
                if (GUILayout.Button("全选", GUILayout.Width(50)))
                {
                    SelectAllFilterTextureType();
                }

                if (GUILayout.Button("全不选", GUILayout.Width(50)))
                {
                    ClearAllFilterTextureType();
                }

                foreach (var element in FILTER_TEXTURE_TYPE)
                {
                    var isOn = _filterTextureType.Contains(element);
                    var tog = isOn;
                    var txt = element.ToString();
                    tog = GUILayout.Toggle(tog, txt, Styles.ToggleFlex);
                    if (isOn != tog)
                    {
                        if (tog)
                        {
                            AddFilterTextureType(element);
                        }
                        else
                        {
                            RemoveFilterTextureType(element);
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }
        }

        private void AddFilterTextureType(TextureImporterType type)
        {
            _filterTextureType.Add(type);
            ClearSortCache();
            //更新
            OnFilterChanged?.Invoke();
        }

        private void RemoveFilterTextureType(TextureImporterType type)
        {
            _filterTextureType.Remove(type);
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        private void SelectAllFilterTextureType()
        {
            _filterTextureType = new HashSet<TextureImporterType>(FILTER_TEXTURE_TYPE);
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        private void ClearAllFilterTextureType()
        {
            _filterTextureType = new HashSet<TextureImporterType>();
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        //过滤Texture Shape
        private void DrawFilterTextureImporterShapeGUI()
        {
            if (_filterFlag)
            {
                GUILayout.Space(3);
                GUILayout.BeginHorizontal();
                GUILayout.Label("Texture Shape:", GUILayout.Width(90));
                if (GUILayout.Button("全选", GUILayout.Width(50)))
                {
                    SelectAllFilterTextureShape();
                }

                if (GUILayout.Button("全不选", GUILayout.Width(50)))
                {
                    ClearAllFilterTextureShape();
                }

                foreach (var element in FILTER_SHAPE_TYPE)
                {
                    var isOn = _filterShapeType.Contains(element);
                    var tog = isOn;
                    tog = GUILayout.Toggle(tog, element.ToString(), Styles.ToggleFlex);
                    if (isOn != tog)
                    {
                        if (tog)
                        {
                            AddFilterTextureShape(element);
                        }
                        else
                        {
                            RemoveFilterTextureShape(element);
                        }
                    }
                }

                GUILayout.EndHorizontal();
            }
        }

        private void SelectAllFilterTextureShape()
        {
            _filterShapeType = new HashSet<TextureImporterShape>(FILTER_SHAPE_TYPE);
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        private void ClearAllFilterTextureShape()
        {
            _filterShapeType = new HashSet<TextureImporterShape>();
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        private void AddFilterTextureShape(TextureImporterShape shape)
        {
            _filterShapeType.Add(shape);
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        private void RemoveFilterTextureShape(TextureImporterShape shape)
        {
            _filterShapeType.Remove(shape);
            ClearSortCache();
            OnFilterChanged?.Invoke();
        }

        #endregion

        #region 中心GUI

        //过滤器GUI
        private void DrawFilterGUI(Rect view)
        {
            GUILayout.BeginArea(view);
            DrawFastFilterToolBar();
            DrawFilterTextureImporterTypeGUI();
            DrawFilterTextureImporterShapeGUI();
            GUILayout.EndArea();
        }

        private void DrawCenterGUI(Rect view)
        {
            var rect = new Rect(0, 0, view.width, view.height);
            var rectFilter = view;
            rectFilter.height = 100;
            if (!_filterFlag)
            {
                rectFilter.height = 24;
            }

            DrawFilterGUI(rectFilter);


            //搜索框
            var rectSearchBar = rectFilter;
            rectSearchBar.height = 22;
            rectSearchBar.y = rectFilter.yMax;
            var center = rectSearchBar.center;
            rectSearchBar.height -= 4;
            rectSearchBar.width -= 4;
            rectSearchBar.center = center;
            DrawSearchBarGUI(rectSearchBar);
            var rectItemWindowView = rectSearchBar;
            rectItemWindowView.y = rectSearchBar.yMax;
            rectItemWindowView.y += 2;
            rectItemWindowView.height = view.yMax - rectItemWindowView.y;
            rectItemWindowView.height -= 24;
            DrawItemWindow(rectItemWindowView);

            var rectCenterBottom = rectItemWindowView;
            rectCenterBottom.y = rectItemWindowView.yMax;
            rectCenterBottom.height = 24;
            DrawInfoGUI(rectCenterBottom);
            _searchBarArea = rectSearchBar;
        }

        private void DrawSearchBarGUI(Rect view)
        {
            GUI.SetNextControlName(CONTROL_NAME_SEARCH_BAR);
            var temSearch = _searchTxt;

            _searchTxt = GUI.TextArea(view, _searchTxt);
            if (GUI.GetNameOfFocusedControl() == CONTROL_NAME_SEARCH_BAR)
            {
                EditorGUI.DrawRect(view, Color.blue * 0.4f);
            }

            if (string.IsNullOrEmpty(_searchTxt) && GUI.GetNameOfFocusedControl() != CONTROL_NAME_SEARCH_BAR)
            {
                GUI.Label(view, "搜索...", Styles.SearchTips);
            }

            if (temSearch != _searchTxt)
            {
                OnFilterChanged?.Invoke();
            }
        }

        private void DrawItemWindow(Rect view)
        {
            var windowWidth = CalculateItemWindowWidth();
            var windowHeight = CalculateItemWindowHeight();
            GUILayout.BeginArea(view);
            var rect = new Rect(0, 0, view.width, view.height);
            var rectViewPort = rect;
            var rectScrollView = new Rect(0, 0, windowWidth, windowHeight);
            //绘制
            _itemWindowScroll = GUI.BeginScrollView(rectViewPort, _itemWindowScroll, rectScrollView);
            DrawItems(view, _itemWindowScroll.y, rectViewPort, rectScrollView);
            DrawPropertyTabBar(_itemWindowScroll.y, rectViewPort, rectScrollView);
            //绘制属性标题
            GUI.EndScrollView();
            GUILayout.EndArea();
            _itemsArea = view;
            _itemsArea.y += PROPERTY_TITLE_BAR_HEIGHT;
            var h = windowHeight - PROPERTY_TITLE_BAR_HEIGHT - 16;
            var h2 = rectViewPort.height - PROPERTY_TITLE_BAR_HEIGHT - 16;
            _itemsArea.height = Mathf.Min(h, h2);
            _itemsArea.width -= 16;
        }


        private void DrawInfoGUI(Rect view)
        {
            EditorGUI.DrawRect(view, Color.black);
            if (!string.IsNullOrEmpty(_lastSelectGuid))
            {
                var importer = GetImporter(_lastSelectGuid);
                GUI.TextArea(view, importer.assetPath, Styles.WhiteTitleLabel);
            }
        }

        //修正items视窗
        private void FitItemWindowScrollPosition(string guid, int offsetIndex)
        {
            var index = _matchItems.IndexOf(guid);
            var finalIndex = index + offsetIndex;
            if (index >= 0 && index < _matchItems.Count && finalIndex >= 0 && finalIndex <= _matchItems.Count)
            {
                if (_itemsRects.TryGetValue(guid, out var rect))
                {
                    var nextRect = rect;
                    nextRect.y += ITEM_HEIGHT * offsetIndex;
                    if (nextRect.yMin < _itemsArea.yMin)
                    {
                        _itemWindowScroll =
                            new Vector2(_itemWindowScroll.x,
                                _itemWindowScroll.y - ITEM_HEIGHT * Mathf.Abs(offsetIndex));
                    }

                    if (nextRect.yMax > _itemsArea.yMax)
                    {
                        _itemWindowScroll =
                            new Vector2(_itemWindowScroll.x,
                                _itemWindowScroll.y + ITEM_HEIGHT * Mathf.Abs(offsetIndex));
                    }
                }
                else
                {
                    //如果不在范围内
                    _itemWindowScroll =
                        new Vector2(_itemWindowScroll.x,
                            finalIndex * ITEM_HEIGHT - ITEM_HEIGHT);
                }
            }
        }

        private void DrawPropertyTabBar(float offsetY, Rect viewPort, Rect scrollView)
        {
            var rect = new Rect(0, offsetY, scrollView.width, PROPERTY_TITLE_BAR_HEIGHT);
            var bottom = rect;
            bottom.width = Mathf.Max(viewPort.width, scrollView.width);
            EditorGUI.DrawRect(bottom, new Color(0.22f, 0.22f, 0.22f, 1f));
            var offsetX = (float)ITEM_PADDING.left;
            foreach (var pair in PropertyTabMap)
            {
                var key = pair.Key;
                var value = pair.Value;
                var itemRect = rect;
                itemRect.width = value.LabelWidth;
                itemRect.width -= 1;
                itemRect.x = offsetX;
                EditorGUI.DrawRect(itemRect, Color.black);
                GUI.Label(itemRect, key.ToString(), Styles.WhiteTitleLabel);
                if (value.CanSort)
                {
                    var rectSortBtn = itemRect;
                    rectSortBtn.height -= 4;
                    rectSortBtn.center = itemRect.center;
                    rectSortBtn.width = rectSortBtn.height;
                    rectSortBtn.x = itemRect.xMax - rectSortBtn.width;
                    rectSortBtn.x -= 2;
                    if (GUI.Button(rectSortBtn, "▼"))
                    {
                        SortBy(key);
                    }

                    if (value.EnumType == PropertyTab.Name)
                    {
                        var btnDefaultSort = rectSortBtn;
                        btnDefaultSort.width = 60;
                        btnDefaultSort.x = rectSortBtn.xMin - btnDefaultSort.width;
                        btnDefaultSort.x -= 2;
                        if (GUI.Button(btnDefaultSort, "默认排序"))
                        {
                            SortByDefault();
                        }
                    }
                }


                offsetX += value.LabelWidth;
            }
        }

        private void DrawItems(Rect area, float offsetY, Rect viewPort, Rect scrollView)
        {
            _itemsRects.Clear();
            //计算绘制的元素索引范围
            var startY = offsetY - ITEM_PADDING.top;
            var endY = startY + viewPort.height;
            startY -= ITEM_EDGE_RANGE;
            endY += ITEM_EDGE_RANGE;
            //元素数量
            var count = Mathf.CeilToInt((endY - startY) / ITEM_HEIGHT);
            var startIndex = (int)(startY / ITEM_HEIGHT);
            var endIndex = startIndex + count;
            //截取有效范围
            for (int i = startIndex; i <= endIndex; i++)
            {
                var rect = CalculateItemRect(i, scrollView);
                var dark = i % 2 == 0;
                EditorGUI.DrawRect(rect, dark ? Color.black * 0.1f : Color.white * 0.1f);
                if (!(i >= 0 && i < _matchItems.Count))
                {
                    continue;
                }

                var no = i + 1;
                var guid = _matchItems[i];
                var cacheRect = rect;
                cacheRect.position += area.position;
                cacheRect.position -= Vector2.up * offsetY;
                _itemsRects[guid] = cacheRect;

                var importer = GetImporter(guid);
                var info = GetInfo(guid);
                var settings = info.Settings;
                var source = info.SourceInfo;

                if (_selectGuids.Contains(guid))
                {
                    EditorGUI.DrawRect(rect, Styles.IsProSKin ? Color.cyan * 0.5f : Color.blue * 0.3f);
                }

                if (_hoverSelect.Contains(guid))
                {
                    EditorGUI.DrawRect(rect, Color.green * 0.3f);
                }

                //信息-序号
                var rectNo = rect;
                rectNo.width = GetPropertyWidth(PropertyTab.NO);
                GUI.Label(rectNo, no.ToString(), Styles.SmallLabel);

                //绘制信息-Name
                var rectName = rectNo;
                rectName.x += rectName.width;
                rectName.width = GetPropertyWidth(PropertyTab.Name);
                var rectPreviewTex = rectName;
                rectPreviewTex.width = rectPreviewTex.height;
                GUI.DrawTexture(rectPreviewTex, info.Texture, ScaleMode.ScaleToFit);
                rectName.x = rectPreviewTex.xMax;
                rectName.width -= rectPreviewTex.width;
                GUI.Label(rectName, info.Name);

                //信息-Override
                var rectOverride = rectName;
                rectOverride.x += rectOverride.width;
                rectOverride.width = GetPropertyWidth(PropertyTab.Override);
                GUI.Label(rectOverride, settings.overridden ? "开启" : "-", Styles.MiddleLabel);

                //信息-Format
                var temEnable = GUI.enabled;
                GUI.enabled = settings.overridden;
                var rectFormat = rectOverride;
                rectFormat.x += rectFormat.width;
                rectFormat.width = GetPropertyWidth(PropertyTab.Format);
                var formatTxt = settings.overridden
                    ? GetFormatName(settings.format)
                    : GetFormatName(info.DefaultFormat);
                GUI.Label(rectFormat, formatTxt);
                GUI.enabled = temEnable;


                //信息-MaxSize
                var rectMaxSize = rectFormat;
                rectMaxSize.x += rectMaxSize.width;
                rectMaxSize.width = GetPropertyWidth(PropertyTab.MaxSize);
                var maxSizeTxt = settings.overridden
                    ? settings.maxTextureSize.ToString()
                    : importer.GetDefaultPlatformTextureSettings().maxTextureSize.ToString();
                GUI.Label(rectMaxSize, maxSizeTxt, Styles.MiddleLabel);

                //信息-原始大小
                var rectSourceSize = rectMaxSize;
                rectSourceSize.x += rectSourceSize.width;
                rectSourceSize.width = GetPropertyWidth(PropertyTab.SourceSize);

                GUI.Label(rectSourceSize, source != null ? $"{source.width} × {source.height}" : "---",
                    Styles.MiddleLabel);

                //信息-MipMap
                var rectMipMap = rectSourceSize;
                rectMipMap.x += rectMipMap.width;
                rectMipMap.width = GetPropertyWidth(PropertyTab.MipMap);
                GUI.Label(rectMipMap, importer.mipmapEnabled ? "开启" : "-", Styles.MiddleLabel);

                //信息-npotscale
                var rectNpot = rectMipMap;
                rectNpot.x += rectNpot.width;
                rectNpot.width = GetPropertyWidth(PropertyTab.NpotScale);
                GUI.Label(rectNpot, importer.npotScale.ToString());

                //信息-type
                var rectType = rectNpot;
                rectType.x += rectType.width;
                rectType.width = GetPropertyWidth(PropertyTab.Type);
                GUI.Label(rectType, $"{importer.textureType}({importer.textureShape})");

                //信息-path
                var rectPath = rectType;
                rectPath.x += rectPath.width;
                rectPath.width = GetPropertyWidth(PropertyTab.Path);
                GUI.Label(rectPath, importer.assetPath);
            }
        }

        private Rect CalculateItemRect(int index, Rect scrollView)
        {
            var x = ITEM_PADDING.left;
            var y = index * ITEM_HEIGHT + ITEM_PADDING.top;
            return new Rect(x, y, scrollView.width, ITEM_HEIGHT);
        }

        private float GetPropertyWidth(PropertyTab propertyTab)
        {
            return PropertyTabMap[propertyTab].LabelWidth;
        }

        //item window width
        private float CalculateItemWindowWidth()
        {
            var result = 0f;
            result += ITEM_PADDING.horizontal;
            foreach (var value in PropertyTabMap.Values)
            {
                result += value.LabelWidth;
            }

            return result;
        }

        //item window height
        private float CalculateItemWindowHeight()
        {
            return ITEM_PADDING.vertical + _matchItems.Count * ITEM_HEIGHT;
        }

        //图片信息
        private static TextureInfo GetInfo(string guid)
        {
            if (!_infos.ContainsKey(guid))
            {
                var importer = GetImporter(guid);
                var source = GetSourceTextureInformation(importer);
                _infos[guid] = new TextureInfo()
                {
                    Guid = guid,
                    SourceInfo = source,
                };
            }

            return _infos[guid];
        }

        private static TextureImporter GetImporter(string guid)
        {
            return _importers[guid];
        }

        private void Select(string[] guids)
        {
            _selectGuids = new HashSet<string>(guids);
            _lastSelectGuid = guids.Last();
            OnSelectChanged?.Invoke();
        }

        private void Select(string guid, SelectMode mode)
        {
            switch (mode)
            {
                case SelectMode.Single:
                {
                    _selectGuids.Clear();
                    _selectGuids.Add(guid);
                    _lastSelectGuid = guid;
                    OnSelectChanged?.Invoke();
                }

                    break;
                case SelectMode.Shift:
                {
                    var startIndex = _matchItems.IndexOf(_lastSelectGuid);
                    var endIndex = _matchItems.IndexOf(guid);
                    if (startIndex >= 0 && endIndex >= 0)
                    {
                        var min = Mathf.Min(startIndex, endIndex);
                        var max = Mathf.Max(startIndex, endIndex);
                        _selectGuids.Clear();
                        for (int i = min; i <= max && i < _matchItems.Count; i++)
                        {
                            _selectGuids.Add(_matchItems[i]);
                        }
                    }

                    _lastSelectGuid = guid;
                    OnSelectChanged?.Invoke();
                }
                    break;
                case SelectMode.Ctrl:
                {
                    if (_selectGuids.Contains(guid))
                    {
                        _selectGuids.Remove(guid);
                    }
                    else
                    {
                        _selectGuids.Add(guid);
                    }

                    _lastSelectGuid = guid;
                    OnSelectChanged?.Invoke();
                }
                    break;
            }
        }


        private bool SelectOffset(int offset)
        {
            var index = GetIndexOf(_lastSelectGuid) + offset;
            if (index >= 0 && index < _matchItems.Count)
            {
                var guid = _matchItems[index];
                FitItemWindowScrollPosition(_lastSelectGuid, offset);
                Select(guid, SelectMode.Single);
                return true;
            }
            else
            {
                if (_selectGuids.Contains(_lastSelectGuid))
                {
                    FitItemWindowScrollPosition(_lastSelectGuid, 0);
                }
            }

            return false;
        }

        private bool ShiftSelectOffset(int offset)
        {
            var cacheSelect = _lastSelectGuid;
            var index = GetIndexOf(cacheSelect) + offset;
            if (index >= 0 && index < _matchItems.Count)
            {
                var guid = _matchItems[index];
                if (_selectGuids.Contains(guid))
                {
                    //存在，判断是否剔除
                    _selectGuids.Remove(cacheSelect);
                }
                else
                {
                    //不存在
                    _selectGuids.Add(guid);
                }

                _lastSelectGuid = guid;
                OnSelectChanged?.Invoke();
                FitItemWindowScrollPosition(cacheSelect, offset);
                return true;
            }
            else
            {
                if (_selectGuids.Contains(_lastSelectGuid))
                {
                    FitItemWindowScrollPosition(_lastSelectGuid, 0);
                }
            }

            return false;
        }


        private int GetIndexOf(string guid)
        {
            return _matchItems.IndexOf(guid);
        }

        #endregion

        #region 右侧GUI

        private void DrawRightGUI(Rect view)
        {
            var rectTitle = view;
            rectTitle.height = 22;
            GUI.Label(rectTitle, "Inspector", Styles.TitleLabel);
            var rect = new Rect(0, 0, view.width, view.height);
            var rectInspector = rectTitle;
            rectInspector.y = rectTitle.yMax;
            rectInspector.height = rect.yMax - rectInspector.y;
            var center = rectInspector.center;
            rectInspector.width -= 8;
            rectInspector.height -= 8;
            rectInspector.center = center;
            rectInspector.height -= PREVIEW_HEIGHT;
            var hover = false;
            if (_multipleInspector)
            {
                //显示按钮，重复选实例
                GUILayout.BeginArea(rectInspector);
                if (_cacheSelectTextures.Count > 0)
                {
                    var click = GUILayout.Button(new GUIContent($"{_cacheSelectTextures.Count} Texture2D",
                        EditorGUIUtility.IconContent("Texture Icon").image), GUILayout.Height(24));
                    var last = GUILayoutUtility.GetLastRect();
                    if (last.Contains(Event.current.mousePosition))
                    {
                        _hoverSelect = new HashSet<string>(_cacheSelectTextures);
                        Repaint();
                    }

                    if (click)
                    {
                        Select(_cacheSelectTextures.ToArray());
                    }
                }

                if (_cacheSelectCubeMap.Count > 0)
                {
                    var click = GUILayout.Button(new GUIContent($"{_cacheSelectCubeMap.Count} CubeMap",
                        EditorGUIUtility.IconContent("Cubemap Icon").image), GUILayout.Height(24));
                    var last = GUILayoutUtility.GetLastRect();
                    if (last.Contains(Event.current.mousePosition))
                    {
                        _hoverSelect = new HashSet<string>(_cacheSelectCubeMap);
                        Repaint();
                    }

                    if (click)
                    {
                        Select(_cacheSelectCubeMap.ToArray());
                    }
                }

                GUILayout.EndArea();
            }


            if (!_multipleInspector)
            {
                var rectPreview = rectInspector;
                rectPreview.y = rectInspector.yMax;
                rectPreview.height = PREVIEW_HEIGHT;
                if (!_previewGUIFlag)
                {
                    rectPreview.height = 0;
                    rectPreview.y = view.yMax;
                }

                var rectGUIView = rectInspector;
                rectGUIView.height = Mathf.Max(INSPECTOR_VIEW_HEIGHT, rectInspector.height);
                _scrollInspector = GUI.BeginScrollView(rectInspector, _scrollInspector, rectGUIView, false, false,
                    GUIStyle.none, GUIStyle.none);
                GUILayout.BeginArea(rectGUIView);
                var editorError = false;

                if (InspectorEditor != null)
                {
                    try
                    {
                        InspectorEditor.DrawHeader();
                        InspectorEditor.OnInspectorGUI();
                    }
                    catch (Exception e)
                    {
                        editorError = true;
                    }
                }

                GUILayout.EndArea();

                GUI.EndScrollView();

                var rectFoldoutPreviewGUI = rectPreview;
                rectFoldoutPreviewGUI.y -= 22;
                rectFoldoutPreviewGUI.height += 22;
                center = rectFoldoutPreviewGUI.center;
                rectFoldoutPreviewGUI.width += 8;
                rectFoldoutPreviewGUI.center = center;
                var rectPreviewGUITitle = rectFoldoutPreviewGUI;
                rectPreviewGUITitle.height = 22;


                var rectBtnClosePreview = rectFoldoutPreviewGUI;
                rectBtnClosePreview.height = 22;
                rectBtnClosePreview.height -= 4;
                rectBtnClosePreview.width = 60;
                rectBtnClosePreview.x = rectFoldoutPreviewGUI.xMax - rectBtnClosePreview.width;
                rectBtnClosePreview.y = rectFoldoutPreviewGUI.yMin;
                rectBtnClosePreview.x -= 2;
                rectBtnClosePreview.y += 2;


                //绘制预览视窗
                if (InspectorEditor != null)
                {
                    EditorGUI.DrawRect(rectFoldoutPreviewGUI, Color.black);
                    try
                    {
                        if (_previewGUIFlag)
                        {
                            if (InspectorEditor.HasPreviewGUI())
                            {
                                InspectorEditor.DrawPreview(rectPreview);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        editorError = true;
                    }

                    var txt = _previewGUIFlag ? "收起▼" : "展开▲";

                    GUI.Label(rectPreviewGUITitle, "预览视窗", Styles.WhiteTitleLabel);
                    if (GUI.Button(rectBtnClosePreview, txt))
                    {
                        _previewGUIFlag = !_previewGUIFlag;
                    }
                }


                if (editorError)
                {
                    DestroyImmediate(_inspectorEditor, true);
                    RebuildInspectorEditor();
                }
            }
        }

        #endregion

        #region 排序

        //排序
        private static void SortBy(PropertyTab propertyTab)
        {
            _cacheSortBy = propertyTab;
            var cache = GetSortState(propertyTab);
            var state = !cache;
            SetSortState(propertyTab, state);
            OnFilterChanged?.Invoke();
        }

        //按名称排序
        private static void SortByName(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (!state)
                {
                    (ta, tb) = (b, a);
                }

                var result = String.Compare(GetInfo(ta).Name, GetInfo(tb).Name, StringComparison.Ordinal);
                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按override排序
        private static void SortByOverride(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (state)
                {
                    (ta, tb) = (b, a);
                }

                var infoA = GetInfo(ta);
                var infoB = GetInfo(tb);
                var result = infoA.Settings.overridden.CompareTo(infoB.Settings.overridden);
                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按Format排序
        private static void SortByFormat(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (!state)
                {
                    (ta, tb) = (b, a);
                }

                var infoA = GetInfo(ta);
                var infoB = GetInfo(tb);
                var formatA = infoA.Settings.overridden ? infoA.Settings.format : infoA.DefaultFormat;
                var formatB = infoB.Settings.overridden ? infoB.Settings.format : infoB.DefaultFormat;
                var result = formatA.CompareTo(formatB);
                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按MaxSize排序
        private static void SortByMaxSize(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (!state)
                {
                    (ta, tb) = (b, a);
                }

                var infoA = GetInfo(ta);
                var infoB = GetInfo(tb);
                var importerA = GetImporter(ta);
                var importerB = GetImporter(tb);
                var sizeA = infoA.Settings.overridden
                    ? infoA.Settings.maxTextureSize
                    : importerA.GetDefaultPlatformTextureSettings().maxTextureSize;
                var sizeB = infoB.Settings.overridden
                    ? infoB.Settings.maxTextureSize
                    : importerB.GetDefaultPlatformTextureSettings().maxTextureSize;
                var result = sizeA.CompareTo(sizeB);
                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按Source排序
        private static void SortBySourceSize(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (!state)
                {
                    (ta, tb) = (b, a);
                }

                var infoA = GetInfo(ta);
                var infoB = GetInfo(tb);
                var sizeA = infoA.SourceInfo.width * infoA.SourceInfo.height;
                var sizeB = infoB.SourceInfo.width * infoB.SourceInfo.height;
                var result = sizeA.CompareTo(sizeB);
                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按mipmap排序
        private static void SortByMipMap(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (state)
                {
                    (ta, tb) = (b, a);
                }

                var importerA = GetImporter(ta);
                var importerB = GetImporter(tb);
                var result = importerA.mipmapEnabled.CompareTo(importerB.mipmapEnabled);
                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按NpotScale排序
        private static void SortByNpotScale(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (!state)
                {
                    (ta, tb) = (b, a);
                }

                var importerA = GetImporter(ta);
                var importerB = GetImporter(tb);
                var result = importerA.npotScale.CompareTo(importerB.npotScale);
                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按TextureType排序
        private static void SortByTextureType(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (!state)
                {
                    (ta, tb) = (b, a);
                }

                var importerA = GetImporter(ta);
                var importerB = GetImporter(tb);
                var result = importerA.textureType.CompareTo(importerB.textureType);
                if (result == 0)
                {
                    result = importerA.textureShape.CompareTo(importerB.textureShape);
                }

                if (result == 0)
                {
                    result = GetTextureOrder(a).CompareTo(GetTextureOrder(b));
                }

                return result;
            });
        }

        //按Path排序
        private static void SortByPath(List<string> list, bool state)
        {
            list.Sort((a, b) =>
            {
                var (ta, tb) = (a, b);
                if (!state)
                {
                    (ta, tb) = (b, a);
                }

                return GetTextureOrder(ta).CompareTo(GetTextureOrder(tb));
            });
        }

        //默认排序
        private void SortByDefault()
        {
            _matchItems.Sort((a, b) => { return GetTextureOrder(a).CompareTo(GetTextureOrder(b)); });
        }

        #endregion

        #region 事件处理

        private void HandleEvent()
        {
            //键盘点击
            if (Event.current.type == EventType.KeyDown)
            {
                switch (Event.current.keyCode)
                {
                    case KeyCode.Escape: //点击ESC
                        if (GUI.GetNameOfFocusedControl() == CONTROL_NAME_SEARCH_BAR)
                        {
                            GUI.FocusControl("");
                            var temSearch = _searchTxt;
                            _searchTxt = "";
                            if (temSearch != _searchTxt)
                            {
                                OnFilterChanged?.Invoke();
                            }

                            Event.current.Use();
                        }

                        if (GUI.GetNameOfFocusedControl() == CONTROL_FOLDER_SEARCH_BAR)
                        {
                            GUI.FocusControl("");
                            if (_folderTree != null)
                            {
                                _folderTree.searchString = "";
                            }

                            Event.current.Use();
                        }


                        break;
                    case KeyCode.Return:
                    case KeyCode.KeypadEnter:
                    {
                        //点击回车
                        if (GUI.GetNameOfFocusedControl() == CONTROL_NAME_SEARCH_BAR)
                        {
                            GUI.FocusControl("");
                            Event.current.Use();
                        }


                        if (GUI.GetNameOfFocusedControl() == CONTROL_FOLDER_SEARCH_BAR)
                        {
                            GUI.FocusControl("");
                            Event.current.Use();
                        }
                    }
                        break;
                    case KeyCode.F:
                    {
                        if (Event.current.modifiers == EventModifiers.Control)
                        {
                            switch (_lastLayoutScene)
                            {
                                case LayoutScene.Left:
                                {
                                    GUI.FocusControl(CONTROL_FOLDER_SEARCH_BAR);

                                    if (_folderTree != null && !string.IsNullOrEmpty(_folderTree.searchString) &&
                                        _folderTree.searchString.Length > 0)
                                    {
                                        TextEditor te =
                                            (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor),
                                                GUIUtility.keyboardControl);
                                        te.OnFocus();
                                        if (te != null)
                                        {
                                            te.cursorIndex = 0; //CursorStartPosition;
                                            te.selectIndex =
                                                _folderTree.searchString.Length; //cursor selected end position…
                                        }
                                    }
                                }
                                    break;
                                case LayoutScene.Center:
                                {
                                    GUI.FocusControl(CONTROL_NAME_SEARCH_BAR);
                                    if (!string.IsNullOrEmpty(_searchTxt) && _searchTxt.Length > 0)
                                    {
                                        TextEditor te =
                                            (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor),
                                                GUIUtility.keyboardControl);
                                        te.OnFocus();
                                        if (te != null)
                                        {
                                            te.cursorIndex = 0; //CursorStartPosition;
                                            te.selectIndex = _searchTxt.Length; //cursor selected end position…
                                        }
                                    }
                                }
                                    break;
                            }


                            Event.current.Use();
                        }
                    }
                        break;
                    case KeyCode.UpArrow:
                    {
                        if (_focusScene == FocusScene.ItemView && _selectGuids.Contains(_lastSelectGuid))
                        {
                            if (Event.current.modifiers == (EventModifiers.Shift | EventModifiers.FunctionKey))
                            {
                                //多选
                                ShiftSelectOffset(-1);
                            }
                            else
                            {
                                SelectOffset(-1);
                            }

                            Event.current.Use();
                        }
                    }
                        break;
                    case KeyCode.DownArrow:
                    {
                        if (_focusScene == FocusScene.ItemView && _selectGuids.Contains(_lastSelectGuid))
                        {
                            if (Event.current.modifiers == (EventModifiers.Shift | EventModifiers.FunctionKey))
                            {
                                //多选
                                ShiftSelectOffset(1);
                            }
                            else
                            {
                                SelectOffset(1);
                            }

                            Event.current.Use();
                        }
                    }
                        break;

                    case KeyCode.A:
                    {
                        if (Event.current.modifiers == EventModifiers.Control)
                        {
                            if (_lastLayoutScene == LayoutScene.Center)
                            {
                                //全选
                                Select(_matchItems.ToArray());
                                Event.current.Use();
                            }
                        }
                    }
                        break;
                }
            }

            //鼠标点击
            if (Event.current.type == EventType.MouseDown)
            {
                if (Event.current.button == 0)
                {
                    var mousePosition = Event.current.mousePosition;

                    if (GUI.GetNameOfFocusedControl() == CONTROL_NAME_SEARCH_BAR &&
                        !_searchBarArea.Contains(mousePosition))
                    {
                        GUI.FocusControl("");
                    }

                    if (GUI.GetNameOfFocusedControl() == CONTROL_FOLDER_SEARCH_BAR &&
                        !_searchFolderArea.Contains(mousePosition))
                    {
                        GUI.FocusControl("");
                    }

                    if (_leftArea.Contains(mousePosition))
                    {
                        _lastLayoutScene = LayoutScene.Left;
                    }

                    if (_centerArea.Contains(mousePosition))
                    {
                        _lastLayoutScene = LayoutScene.Center;
                    }

                    if (_inspectorArea.Contains(mousePosition))
                    {
                        //TODO//点击右侧
                    }

                    if (_itemsArea.Contains(mousePosition) && IsInsideItemRect(mousePosition, out var match))
                    {
                        switch (Event.current.modifiers)
                        {
                            case EventModifiers.None:
                            {
                                Select(match, SelectMode.Single);
                                if (Event.current.clickCount >= 2)
                                {
                                    Ping(GetInfo(match).Texture);
                                    if (_folderTree != null)
                                    {
                                        _folderTree.PingFolder(GetImporter(match).assetPath, true);
                                    }
                                }

                                //选择实例
                                Event.current.Use();
                            }
                                return;
                            case EventModifiers.Control:
                            {
                                Select(match, SelectMode.Ctrl);
                                //选择实例
                                Event.current.Use();
                            }
                                return;
                            case EventModifiers.Shift:
                            {
                                Select(match, SelectMode.Shift);
                                //选择实例
                                Event.current.Use();
                            }
                                return;
                        }
                    }
                }
            }
        }


        private bool IsInsideItemRect(Vector2 mousePos, out string match)
        {
            match = "";
            foreach (var pair in _itemsRects)
            {
                var guid = pair.Key;
                var rect = pair.Value;
                if (mousePos.y > rect.yMin && mousePos.y < rect.yMax)
                {
                    match = guid;
                    return true;
                }
            }

            return false;
        }

        //选择项改变时
        private void WhenSelectChanged()
        {
            if (!_selectGuids.Contains(_lastSelectGuid))
            {
                _lastSelectGuid = "";
            }

            //更新选中实例
            SelectImporters(_selectGuids);

            //更新inspector
            if (!string.IsNullOrEmpty(_lastSelectGuid))
            {
                var assetTargets = new List<Object>();
                foreach (var guid in _selectGuids)
                {
                    assetTargets.Add(AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(guid)));
                }

                // Selection.objects = assetTargets.ToArray();
            }

            _focusScene = FocusScene.ItemView;
            GUIUtility.keyboardControl = -1;


            if (_folderTree != null && !string.IsNullOrEmpty(_lastSelectGuid))
            {
                _folderTree.PingFolder(GetImporter(_lastSelectGuid).assetPath, false);
            }
            else
            {
                _folderTree.PingFolder("", false);
            }
        }


        //过滤项改变时
        private void WhenFilterChanged()
        {
            //更新匹配项
            UpdateMatchItems();
        }

        private void SelectImporters(HashSet<string> guids, bool force = false)
        {
            if (!SelectImportersIsChanged(guids) && force == false)
            {
                return;
            }

            _cacheSelectTextures.Clear();
            _cacheSelectCubeMap.Clear();
            _hoverSelect.Clear();
            var tex2d = new List<Texture2D>();
            var cubeMaps = new List<Cubemap>();
            _selectImportersGuids = new HashSet<string>(guids);
            var importers = new HashSet<TextureImporter>();
            foreach (var guid in _selectImportersGuids)
            {
                var importer = GetImporter(guid);
                importers.Add(importer);
                var tex = GetInfo(guid).Texture as Texture2D;
                var cubemap = GetInfo(guid).Texture as Cubemap;
                if (tex != null)
                {
                    tex2d.Add(tex);
                    _cacheSelectTextures.Add(guid);
                }

                if (cubemap != null)
                {
                    cubeMaps.Add(cubemap);
                    _cacheSelectCubeMap.Add(guid);
                }
            }

            _selectImporters = importers.ToArray();
            //加入二选一机制
            _multipleInspector = (cubeMaps.Count > 0 && tex2d.Count > 0);
            Object[] targets = tex2d.ToArray();
            if (tex2d.Count <= 0)
            {
                targets = cubeMaps.ToArray();
            }

            DestroyImmediate(_inspectorEditor, true);
            DestroyImmediate(_assetEditor, true);
            _inspectorEditor = Editor.CreateEditor(_selectImporters);
            _assetEditor = Editor.CreateEditor(targets);
            var typeName = "UnityEditor.Experimental.AssetImporters.AssetImporterEditor";
#if UNITY_2020_1_OR_NEWER
            typeName = $"UnityEditor.AssetImporters.AssetImporterEditor";
#endif
            var type = GetType(typeName);
            var method = type.GetMethod("InternalSetAssetImporterTargetEditor",
                BindingFlags.NonPublic | BindingFlags.Instance);
            method.Invoke(_inspectorEditor, new object[] { _assetEditor });
            EditorUserBuildSettings.selectedBuildTargetGroup = BuildPipeline.GetBuildTargetGroup(_filterBuildTarget);
        }


        private void RebuildInspectorEditor()
        {
            SelectImporters(_selectGuids, true);
        }


        private bool SelectImportersIsChanged(HashSet<string> guids)
        {
            if (guids.Count != _selectImportersGuids.Count)
            {
                return true;
            }

            foreach (var guid in guids)
            {
                if (!_selectImportersGuids.Contains(guid))
                {
                    return true;
                }
            }

            return false;
        }

        private void WhenClickFolderTree()
        {
            _focusScene = FocusScene.FolderTree;
        }

        #endregion

        private static PropertyTabBinding GetPropertyTabBinding(PropertyTab tab)
        {
            return PropertyTabMap[tab];
        }

        private static bool GetSortState(PropertyTab tab)
        {
            if (_sortReverseMap.TryGetValue(tab, out var match))
            {
                return match;
            }
            else
            {
                _sortReverseMap[tab] = false;
                return _sortReverseMap[tab];
            }
        }


        private static void SetSortState(PropertyTab tab, bool reverse)
        {
            _sortReverseMap[tab] = reverse;
        }

        //清理排序缓存
        private static void ClearSortCache()
        {
            _cacheSortBy = null;
            _sortReverseMap.Clear();
        }

        //获取sort order
        private static int GetTextureOrder(string guid)
        {
            if (_defaultOrders.TryGetValue(guid, out var order))
            {
                return order;
            }

            return -1;
        }

        private static void Ping(Object target)
        {
            EditorApplication.ExecuteMenuItem("Window/General/Project");
            EditorGUIUtility.PingObject(target);
        }

        private static HashSet<T> CollectionToHashSet<T>(IEnumerable<T> target)
        {
            var result = new HashSet<T>();
            foreach (var element in target)
            {
                result.Add(element);
            }

            return result;
        }

        private static bool DrawMenuButton(string txt, bool selected, params GUILayoutOption[] options)
        {
            var click = GUILayout.Button("", options);
            var rect = GUILayoutUtility.GetLastRect();
            if (selected)
            {
                EditorGUI.DrawRect(rect, Color.black * 0.7f);
            }

            GUI.Label(rect, txt, Styles.GetMenuButtonLabel(selected));
            return click;
        }

        //反射获取有效的枚举
        private static T[] GetValidEnumValues<T>() where T : Enum
        {
            var type = typeof(T);
            var match = new HashSet<string>();
            var names = Enum.GetNames(typeof(T));
            foreach (var element in names)
            {
                var fieldInfo = type.GetField(element);
                // 检查特性是否废弃
                if (fieldInfo.GetCustomAttributes(typeof(ObsoleteAttribute), false).Length > 0)
                {
                    continue;
                }
                else
                {
                    match.Add(fieldInfo.Name);
                }
            }

            List<T> result = new List<T>();
            foreach (var value in match)
            {
                result.Add((T)Enum.Parse(typeof(T), value));
            }

            return result.ToArray();
        }

        private static Type _type_UnityEditor_TextureImporter = null;

        private static Type Type_UnityEditor_TextureImporter
        {
            get
            {
                if (_type_UnityEditor_TextureImporter == null)
                {
                    _type_UnityEditor_TextureImporter = GetType("UnityEditor.TextureImporter");
                }

                return _type_UnityEditor_TextureImporter;
            }
        }

        private static MethodInfo _method_RecommendedFormatsFromTextureTypeAndPlatform = null;

        private static MethodInfo Method_RecommendedFormatsFromTextureTypeAndPlatform
        {
            get
            {
                if (_method_RecommendedFormatsFromTextureTypeAndPlatform == null)
                {
                    _method_RecommendedFormatsFromTextureTypeAndPlatform = Type_UnityEditor_TextureImporter.GetMethod(
                        "RecommendedFormatsFromTextureTypeAndPlatform",
                        BindingFlags.Static | BindingFlags.NonPublic);
                }

                return _method_RecommendedFormatsFromTextureTypeAndPlatform;
            }
        }

        private static MethodInfo _method_GetSourceTextureInformation = null;

        private static MethodInfo Method_GetSourceTextureInformation
        {
            get
            {
                if (_method_GetSourceTextureInformation == null)
                {
                    _method_GetSourceTextureInformation = Type_UnityEditor_TextureImporter.GetMethod(
                        "GetSourceTextureInformation",
                        BindingFlags.Instance | BindingFlags.NonPublic);
                }

                return _method_GetSourceTextureInformation;
            }
        }


        private static Type GetType(string typeName)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(typeName);
                if (type != null)
                {
                    return type;
                }
            }

            return null;
        }


        public static SourceTextureInformation GetSourceTextureInformation(
            TextureImporter importer)
        {
            var info =
                Method_GetSourceTextureInformation?.Invoke(importer, null) as
                    SourceTextureInformation;
            return info;
        }


        //推荐格式类型
        public static TextureImporterFormat[] GetRecommendedFormats(TextureImporter importer, BuildTarget target)
        {
            return Method_RecommendedFormatsFromTextureTypeAndPlatform.Invoke(null,
                    new object[] { importer.textureType, target }) as
                TextureImporterFormat[];
        }


        //绘制标题
        private static Rect DrawHeader(string title, params GUILayoutOption[] options)
        {
            GUILayout.Label("", Styles.WhiteTitleLabel, options);
            var rect = GUILayoutUtility.GetLastRect();
            EditorGUI.DrawRect(rect, Color.black);
            GUI.Label(rect, title, Styles.WhiteTitleLabel);
            return rect;
        }


        //创建右上角rect
        public static Rect CreateRightTopRect(Rect rect, float width, float margin)
        {
            var rectResult = rect;
            rectResult.width = width;
            rectResult.x = rect.xMax - rectResult.width;
            var center = rectResult.center;
            rectResult.height -= margin * 2;
            rectResult.width -= margin * 2;
            rectResult.center = center;
            return rectResult;
        }

        private void OnDisable()
        {
            _folderTree?.Release();
            _folderTree = null;
            DestroyImmediate(_assetEditor, true);
            DestroyImmediate(_inspectorEditor, true);
        }

        #region GUI样式

        public class Styles
        {
            public static bool IsProSKin => EditorGUIUtility.isProSkin;

            private static GUIStyle _toggleFlex = null;

            public static GUIStyle ToggleFlex => GetStyle(ref _toggleFlex, "toggle", (style) =>
            {
#if UNITY_2019_3_OR_NEWER
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(2, 24, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.alignment = TextAnchor.MiddleLeft;
                style.contentOffset = Vector2.right * 16;
#endif
            
                style.fixedWidth = 0;
                style.stretchWidth = false;
            });


            private static GUIStyle _buttonLeft = null;

            public static GUIStyle ButtonLeft => GetStyle(ref _buttonLeft, "button", (style) =>
            {
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(2, 2, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.alignment = TextAnchor.MiddleLeft;
            });


            private static GUIStyle _smallLabel = null;

            public static GUIStyle SmallLabel => GetStyle(ref _smallLabel, "label", (style) =>
            {
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(2, 2, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.alignment = TextAnchor.MiddleLeft;
                style.fontSize = 10;
            });

            private static GUIStyle _titleLabel = null;

            public static GUIStyle TitleLabel => GetStyle(ref _titleLabel, "label", (style) =>
            {
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(8, 8, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.alignment = TextAnchor.MiddleLeft;
                style.fontStyle = FontStyle.Bold;
                style.fontSize = 14;
            });

            private static GUIStyle _boldLabel = null;

            public static GUIStyle BoldLabel => GetStyle(ref _boldLabel, "label", (style) =>
            {
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(8, 8, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.fontStyle = FontStyle.Bold;
            });


            private static GUIStyle _whiteTitleLabel = null;

            public static GUIStyle WhiteTitleLabel => GetStyle(ref _whiteTitleLabel, "label", (style) =>
            {
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(8, 8, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.fontStyle = FontStyle.Bold;
                style.normal.textColor = Color.white;
                style.alignment = TextAnchor.MiddleLeft;
            });


            private static GUIStyle _middleLabel = null;

            public static GUIStyle MiddleLabel => GetStyle(ref _middleLabel, "label", (style) =>
            {
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(8, 8, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.alignment = TextAnchor.MiddleCenter;
            });


            private static GUIStyle _menuButtonLabel = null;

            public static GUIStyle MenuButtonLabel => GetStyle(ref _menuButtonLabel, "label", (style) =>
            {
                style.margin = new RectOffset(0, 0, 0, 0);
                style.padding = new RectOffset(8, 8, 2, 2);
                style.border = new RectOffset(0, 0, 0, 0);
                style.alignment = TextAnchor.MiddleLeft;
            });

            public static GUIStyle GetMenuButtonLabel(bool isSelect)
            {
                var textColor = IsProSKin ? Color.white : Color.black;
                if (isSelect)
                {
                    textColor = IsProSKin ? new Color(0.3f, 0.53f, 1f, 1f) : Color.white;
                }

                MenuButtonLabel.normal.textColor = textColor;
                MenuButtonLabel.fontStyle = isSelect ? FontStyle.Bold : FontStyle.Normal;
                return MenuButtonLabel;
            }


            private static GUIStyle _searchTips = null;

            public static GUIStyle SearchTips => GetStyle(ref _searchTips, "label",
                (style) =>
                {
                    var col = style.normal.textColor;
                    style.normal.textColor = new Color(col.r, col.g, col.b, col.a * 0.5f);
                    style.fontStyle = FontStyle.Italic;
                    style.margin = new RectOffset(0, 0, 0, 0);
                    style.padding = new RectOffset(4, 8, 2, 2);
                    style.border = new RectOffset(0, 0, 0, 0);
                    style.alignment = TextAnchor.MiddleLeft;
                });


            private static GUIStyle GetStyle(ref GUIStyle style, string styleName, Action<GUIStyle> onCreate = null)
            {
                if (style == null)
                {
                    style = new GUIStyle(styleName);
                    onCreate?.Invoke(style);
                }

                return style;
            }
        }

        #endregion
    }
}