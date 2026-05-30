using UnityEngine;
using UnityEngine.Scripting;

namespace XGame
{
    /// <summary>
    /// 使用GUI模拟Toast
    /// </summary>
    [Preserve]
    public class ToastGUI : MonoBehaviour
    {
        //字体大小等级
        public static int[] FontSizeOptions = new int[]
        {
            118, 102, 96, 80, 64, 48, 32, 24
        };


        public int PopUpID { get; private set; } = 0;

        private static ToastGUI _instance = null;
        private static object _lock = new object();

        /// <summary>
        /// 单例对象实例
        /// </summary>
        public static ToastGUI Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                        {
                            GameObject clone = new GameObject("XGAME_TOAST_GUI");
                            _instance = clone.AddComponent<ToastGUI>();
                            _instance.Init();
                        }
                    }
                }

                return _instance;
            }
        }

        private GUIStyle _labelStyle = null;

        private GUIStyle LabelStyle
        {
            get
            {
                if (_labelStyle == null)
                {
                    _labelStyle = new GUIStyle();
                    _labelStyle.richText = true;
                    _labelStyle.border = new RectOffset(2, 2, 2, 2);
                    _labelStyle.padding = new RectOffset(2, 2, 2, 2);
                    _labelStyle.margin = new RectOffset(2, 2, 2, 2);
                    _labelStyle.alignment = TextAnchor.MiddleCenter;
                    _labelStyle.normal.textColor = Color.white;
                }

                return _labelStyle;
            }
        }

        public Rect RectBg;
        public Rect RectLabel;

        public Texture TextureBg;

        private void Init()
        {
            DontDestroyOnLoad(gameObject);
            TextureBg = Resources.Load<Texture>("XGameUnityTool/XGAME_UNITY_TOAST_BACKGROUND");
        }

        public bool IsPlaying = false;
        public float PlayingTime = 0;
        public float Duration;
        public GUIContent TxtContent;


        //构建
        public void Build()
        {
        }

        public int ShowToast(string content, float duration = 1.6f)
        {
            TxtContent = new GUIContent(content);
            InitDrawRange(TxtContent);
            IsPlaying = true;
            PlayingTime = 0;
            Duration = duration;
            return PopUpID += 1;
        }

        //初始化绘制区域
        private void InitDrawRange(GUIContent txt)
        {
            //计算字体大小
            var rect = new Rect(0, 0, Screen.width, Screen.height);
            RectBg = rect;
            RectBg.width = Mathf.Clamp(rect.width * 0.8f, 600, 1920);
            var labelMaxWidth = RectBg.width * 0.8f;
            var labelRectSize = new Vector2();
            foreach (var fontSize in FontSizeOptions)
            {
                LabelStyle.fontSize = fontSize;
                labelRectSize = LabelStyle.CalcSize(txt);
                if (labelRectSize.x < labelMaxWidth)
                {
                    break;
                }
            }

            RectBg.height = labelRectSize.y + 44;
            RectBg.center = rect.center;
            RectLabel.size = labelRectSize;
            RectLabel.center = RectBg.center;
        }


        private void Update()
        {
            if (IsPlaying)
            {
                PlayingTime += Time.unscaledDeltaTime;
                PlayingTime += Time.unscaledDeltaTime;
                if (PlayingTime > Duration)
                {
                    IsPlaying = false;
                }
            }
        }

        private void OnGUI()
        {
            if (IsPlaying)
            {
                var temDepth = GUI.depth;
                GUI.depth = 30000;
                //绘制背景
                GUI.DrawTexture(RectBg, TextureBg);
                GUI.Label(RectLabel, TxtContent, LabelStyle);
                GUI.depth = temDepth;
            }
        }
    }
}