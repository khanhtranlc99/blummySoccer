#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class ParticleToPngs : EditorWindow
{

    protected class ParticleToPngsStyle
    {
        public readonly GUIStyle dragdot = "U2D.dragDot";
        public readonly GUIStyle dragdotDimmed = "U2D.dragDotDimmed";
        public readonly GUIStyle dragdotactive = "U2D.dragDotActive";
        public readonly GUIStyle createRect = "U2D.createRect";
        public readonly GUIStyle preToolbar = "preToolbar";
        public readonly GUIStyle preButton = "preButton";
        public readonly GUIStyle preLabel = "preLabel";
        public readonly GUIStyle preSlider = "preSlider";
        public readonly GUIStyle preSliderThumb = "preSliderThumb";
        public readonly GUIStyle preBackground = "preBackground";
        public readonly GUIStyle pivotdotactive = "U2D.pivotDotActive";
        public readonly GUIStyle pivotdot = "U2D.pivotDot";

        public readonly GUIStyle dragBorderdot = new GUIStyle();
        public readonly GUIStyle dragBorderDotActive = new GUIStyle();

        public readonly GUIStyle toolbar;
        public readonly GUIContent alphaIcon;
        public readonly GUIContent RGBIcon;
        public readonly GUIStyle notice;

        public readonly GUIContent smallMip;
        public readonly GUIContent largeMip;

        public ParticleToPngsStyle()
        {
            toolbar = new GUIStyle(EditorStyles.inspectorDefaultMargins);
            toolbar.margin.top = 0;
            toolbar.margin.bottom = 0;
            alphaIcon = EditorGUIUtility.IconContent("PreTextureAlpha");
            RGBIcon = EditorGUIUtility.IconContent("PreTextureRGB");
            preToolbar.border.top = 0;
            createRect.border = new RectOffset(3, 3, 3, 3);

            notice = new GUIStyle(GUI.skin.label);
            notice.alignment = TextAnchor.MiddleCenter;
            notice.normal.textColor = Color.yellow;

            dragBorderdot.fixedHeight = 5f;
            dragBorderdot.fixedWidth = 5f;
            dragBorderdot.normal.background = EditorGUIUtility.whiteTexture;

            dragBorderDotActive.fixedHeight = dragBorderdot.fixedHeight;
            dragBorderDotActive.fixedWidth = dragBorderdot.fixedWidth;
            dragBorderDotActive.normal.background = EditorGUIUtility.whiteTexture;

            smallMip = EditorGUIUtility.IconContent("PreTextureMipMapLow");
            largeMip = EditorGUIUtility.IconContent("PreTextureMipMapHigh");
        }
    }

    protected ParticleToPngsStyle style;

    public string folder = "Captures";
    public float frameRate = 10;
    public int sizeMultiplier = 1;
    private string realFolder = "";
    private int captureCount;
    private bool isCapturing;
    private string path;
    private bool isStarted;

    private bool isStartCaptured;
    private float countTime = 0;
    private int screenWidth = 720;
    private int screenHeight = 1280;

    private RenderTexture renderTexture;
    private Texture2D outputTexture;

    private Camera cam;

    private bool autoOpen
    {
        get => EditorPrefs.GetBool("isOpenCaptureFolderOnStop", false);
        set => EditorPrefs.SetBool("isOpenCaptureFolderOnStop", value);
    }

    [MenuItem("Tools/CaptureTexture")]
    static void Init()
    {
        var w = GetWindow<ParticleToPngs>();
        w.Refresh();
        w.Show();
    }

    protected void InitStyles()
    {
        if (style == null)
            style = new ParticleToPngsStyle();
    }

    private void OnEnable()
    {
        EditorApplication.update += Update;
    }

    private void Update()
    {
        if (isCapturing)
        {
            if (Application.isPlaying && EditorApplication.isPlaying)
            {
                countTime += Time.deltaTime;
                if (countTime >= 1f / frameRate)
                {
                    if (!isStarted)
                    {
                        isStarted = true;
                        cam = new GameObject().AddComponent<Camera>();
                        cam.orthographic = Camera.main.orthographic;
                        cam.transform.position = Camera.main.transform.position;
                        cam.orthographicSize = Camera.main.orthographicSize;
                        cam.fieldOfView = Camera.main.fieldOfView;
                        // cam = Camera.main;
                        cam.transform.position = new Vector3(0, 0, -10);
                        cam.orthographic = true;
                        if (cam.targetTexture == null)
                        {
                            cam.targetTexture = new RenderTexture(screenWidth, screenHeight, 24,
                                GraphicsFormat.R8G8B8A8_UNorm);
                        }

                        renderTexture = cam.targetTexture;
                        screenWidth = renderTexture.width;
                        screenHeight = renderTexture.height;

                        outputTexture = new Texture2D(screenWidth, screenHeight);
                    }

                    captureCount++;
                    path = $"{realFolder}/shot {Time.frameCount:D04}.png";

                    RenderTextureToPNG();
                    Focus();
                    countTime -= 1f / frameRate;
                }
            }
            else if (isStarted)
            {
                isCapturing = false;
            }
        }
    }

    void SavePng()
    {
        var pngShot = outputTexture.EncodeToPNG();
        File.WriteAllBytes(path, pngShot);
    }

    public void RenderTextureToPNG()
    {
        cam.targetTexture = renderTexture;
        cam.Render();
        RenderTexture oldRT = RenderTexture.active;
        RenderTexture.active = cam.activeTexture;
        outputTexture.ReadPixels(new Rect(0, 0, cam.activeTexture.width, cam.activeTexture.height), 0, 0);
        outputTexture.Apply();
        SavePng();
        RenderTexture.active = oldRT;
    }

    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    private void OnGUI()
    {
        InitStyles();
        GUILayout.BeginHorizontal(style.preToolbar);

        if (GUILayout.Button("Open Folder", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            OpenCaptureFolder();
        if (GUILayout.Button("Open Root Folder", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false)))
            OpenRootFolder();

        GUILayout.Label("Size", GUILayout.ExpandWidth(false));
        screenWidth = EditorGUILayout.IntField(screenWidth, GUILayout.Width(60));
        screenHeight = EditorGUILayout.IntField(screenHeight, GUILayout.Width(60));
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();

        GUILayout.EndHorizontal();
        autoOpen = GUILayout.Toggle(autoOpen, "Auto open folder on stop", GUILayout.ExpandWidth(false));
        GUILayout.BeginHorizontal();
        GUILayout.Label("Frame rate", GUILayout.ExpandWidth(false));
        frameRate = EditorGUILayout.Slider(frameRate, 1, 60);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (GUILayout.Button(isCapturing ? "Stop" : "Capture Gift"))
        {
            Camera.main.targetTexture = null;
            isStarted = false;
            captureCount = 0;
            EditorApplication.isPlaying = !isCapturing;
            if (!isCapturing)
                Refresh();

            isCapturing = !isCapturing;
            if (autoOpen && !isCapturing)
                OpenCaptureFolder();

            countTime = frameRate;
        }

        if (GUILayout.Button("Take screenshot"))
        {
            TakeScreenShoot();
        }

        GUILayout.EndHorizontal();

        if (EditorApplication.isPlaying)
        {
            GUILayout.Label("Capture count: " + captureCount);
            int preViewSize = 200;
            if (outputTexture != null)
            {
                var _tex = outputTexture;
                GUIStyle s = new GUIStyle();
                s.normal.background = s.active.background;

                GUILayout.BeginHorizontal(s);
                GUILayout.Label(_tex, GUILayout.Width(preViewSize),
                    GUILayout.Height(preViewSize * _tex.height / (float)_tex.width));
                GUILayout.EndHorizontal();
            }

            GUILayout.Label("Capture count: " + captureCount);
        }

        var listCamera = FindObjectsOfType<Camera>();
        GUILayout.BeginVertical(GUILayout.Width(300));
        foreach (var camera in listCamera)
        {
            GUILayout.BeginHorizontal();
            if (GUILayout.Button($"Capture by {camera.name}"))
                TakeScreenShoot(camera);
            if (GUILayout.Button($"Focus"))
                Selection.activeObject = camera;
            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

    private void OpenCaptureFolder()
    {
        EditorUtility.RevealInFinder(realFolder);
    }

    private void OpenRootFolder()
    {
        EditorUtility.RevealInFinder("Captures");
    }

    void Refresh()
    {
        frameRate = 10;
        Time.captureFramerate = (int)frameRate;
        folder = "Captures\\Capture";
        realFolder = folder;
        int count = 1;
        if (!Directory.Exists("Captures"))
            Directory.CreateDirectory("Captures");

        while (Directory.Exists(realFolder))
        {
            realFolder = folder + count;
            count++;
        }

        var info = Directory.CreateDirectory(realFolder);
        Debug.Log(info.FullName);
    }

    public void TakeScreenShoot(Camera came = null)
    {
        if (came == null)
            came = SceneView.lastActiveSceneView.camera;
        cam = came;

        cam.targetTexture = new RenderTexture(screenWidth, screenHeight, 24,
            GraphicsFormat.R8G8B8A8_UNorm);
        renderTexture = cam.targetTexture;
        screenWidth = renderTexture.width;
        screenHeight = renderTexture.height;
        RenderTexture.active = renderTexture;

        outputTexture = new Texture2D(screenWidth, screenHeight);
        
        if (!Directory.Exists("Captures/Screenshot"))
            Directory.CreateDirectory("Captures/Screenshot");
        
        path = $"Captures/Screenshot/shot {Time.frameCount:D04}.png";
        RenderTextureToPNG();
        RenderTexture.active = null;
        cam.targetTexture = null;

        if (autoOpen)
            EditorUtility.RevealInFinder("Captures/Screenshot");
    }
}
#endif