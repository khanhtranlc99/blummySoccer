using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using XGame;
using XGame.BuildApp;

[CustomEditor(typeof(ToolPreference))]
public class ToolPreferenceInspector : OdinEditor
{
    private static Color COLOR_INSTALL = new Color(0f, 0.84f, 0.73f, 1f);
    private static Color COLOR_UNINSTALL = new Color(0.86f, 0.31f, 0.25f, 1f);
    private static Color COLOR_DOC = new Color(1, 1, 2, 1f);


    private const string LogoBase64 =
        "iVBORw0KGgoAAAANSUhEUgAAAIAAAAAoCAYAAAA2cfJIAAAACXBIWXMAAAsTAAALEwEAmpwYAAAF+mlUWHRYTUw6Y29tLmFkb2JlLnhtcAAAAAAAPD94cGFja2V0IGJlZ2luPSLvu78iIGlkPSJXNU0wTXBDZWhpSHpyZVN6TlRjemtjOWQiPz4gPHg6eG1wbWV0YSB4bWxuczp4PSJhZG9iZTpuczptZXRhLyIgeDp4bXB0az0iQWRvYmUgWE1QIENvcmUgNS42LWMxNDIgNzkuMTYwOTI0LCAyMDE3LzA3LzEzLTAxOjA2OjM5ICAgICAgICAiPiA8cmRmOlJERiB4bWxuczpyZGY9Imh0dHA6Ly93d3cudzMub3JnLzE5OTkvMDIvMjItcmRmLXN5bnRheC1ucyMiPiA8cmRmOkRlc2NyaXB0aW9uIHJkZjphYm91dD0iIiB4bWxuczp4bXA9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC8iIHhtbG5zOmRjPSJodHRwOi8vcHVybC5vcmcvZGMvZWxlbWVudHMvMS4xLyIgeG1sbnM6cGhvdG9zaG9wPSJodHRwOi8vbnMuYWRvYmUuY29tL3Bob3Rvc2hvcC8xLjAvIiB4bWxuczp4bXBNTT0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wL21tLyIgeG1sbnM6c3RFdnQ9Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9zVHlwZS9SZXNvdXJjZUV2ZW50IyIgeG1wOkNyZWF0b3JUb29sPSJBZG9iZSBQaG90b3Nob3AgQ0MgMjAxOCAoV2luZG93cykiIHhtcDpDcmVhdGVEYXRlPSIyMDIyLTAxLTA3VDEwOjAwOjM0KzA4OjAwIiB4bXA6TW9kaWZ5RGF0ZT0iMjAyMi0wMS0xOVQxNTozMjozOSswODowMCIgeG1wOk1ldGFkYXRhRGF0ZT0iMjAyMi0wMS0xOVQxNTozMjozOSswODowMCIgZGM6Zm9ybWF0PSJpbWFnZS9wbmciIHBob3Rvc2hvcDpDb2xvck1vZGU9IjMiIHBob3Rvc2hvcDpJQ0NQcm9maWxlPSJzUkdCIElFQzYxOTY2LTIuMSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDo2NWM0YmU5Yy0yY2U3LWM3NDktYWJmZS03OWFlNDE4YzIzN2EiIHhtcE1NOkRvY3VtZW50SUQ9ImFkb2JlOmRvY2lkOnBob3Rvc2hvcDpiNjA3NDk3YS0xMzBlLTAzNDUtYTU2MC0yOTNjNjMyZGRlMDEiIHhtcE1NOk9yaWdpbmFsRG9jdW1lbnRJRD0ieG1wLmRpZDplNjkyZDI3MC03OGU5LTIyNDgtOThhNC1jM2FmOGUwY2U0NjIiPiA8eG1wTU06SGlzdG9yeT4gPHJkZjpTZXE+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJjcmVhdGVkIiBzdEV2dDppbnN0YW5jZUlEPSJ4bXAuaWlkOmU2OTJkMjcwLTc4ZTktMjI0OC05OGE0LWMzYWY4ZTBjZTQ2MiIgc3RFdnQ6d2hlbj0iMjAyMi0wMS0wN1QxMDowMDozNCswODowMCIgc3RFdnQ6c29mdHdhcmVBZ2VudD0iQWRvYmUgUGhvdG9zaG9wIENDIDIwMTggKFdpbmRvd3MpIi8+IDxyZGY6bGkgc3RFdnQ6YWN0aW9uPSJzYXZlZCIgc3RFdnQ6aW5zdGFuY2VJRD0ieG1wLmlpZDo2NWM0YmU5Yy0yY2U3LWM3NDktYWJmZS03OWFlNDE4YzIzN2EiIHN0RXZ0OndoZW49IjIwMjItMDEtMTlUMTU6MzI6MzkrMDg6MDAiIHN0RXZ0OnNvZnR3YXJlQWdlbnQ9IkFkb2JlIFBob3Rvc2hvcCBDQyAyMDE4IChXaW5kb3dzKSIgc3RFdnQ6Y2hhbmdlZD0iLyIvPiA8L3JkZjpTZXE+IDwveG1wTU06SGlzdG9yeT4gPC9yZGY6RGVzY3JpcHRpb24+IDwvcmRmOlJERj4gPC94OnhtcG1ldGE+IDw/eHBhY2tldCBlbmQ9InIiPz6h9jZIAAAlJUlEQVR4nO2cd7idVZX/P/stp957ym3n9pvGvemNFIOh96BgwDYUxwaIjIKoMA4KCgEREASjIj8VEccCiIPSZCCQRBJID6kkN7fn9nZ6ecv+/bFPEiIE0XGceeZxPc95cnLPeffae6+1V/mutY9wLQshBIVVLy4uvPbaJ+w33nivVlE+rk+Y+Ctz4cLviWgZ7sEeMD0AoOsACF1H2jbJ1lYsy8KUEl8ggG/+fOyBAUQgALaFzGbRSkNo5eUYxx0HhYIaR3NB0yDd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";

    private Texture _logo = null;

    private Texture Logo
    {
        get
        {
            if (_logo == null)
            {
                var bytes = Convert.FromBase64String(LogoBase64);
                var tex = new Texture2D(256, 256);
                tex.LoadImage(bytes);
                _logo = tex;
            }

            return _logo;
        }
    }

    private ToolPreference Target => target as ToolPreference;

    private string version = string.Empty;

    protected override void OnEnable()
    {
        base.OnEnable();
        version = XGameEditorUtil.GetToolVersion();
    }

    public override void OnInspectorGUI()
    {
        //绘制标题
        GUILayout.Label(Logo, SirenixGUIStyles.LabelCentered, GUILayout.Height(60));

        var rect = GUILayoutUtility.GetLastRect();
        var versionRect = new Rect(rect);
        versionRect.width = 140;
        versionRect.height = 22;
        versionRect.x = rect.xMax - versionRect.width - 4;
        versionRect.y += 4;
        GUI.Button(versionRect, new GUIContent($"{version}", "Current version"));

        var btnCheckUpdateRect = new Rect(rect);
        btnCheckUpdateRect.width = 140;
        btnCheckUpdateRect.height = 22;
        btnCheckUpdateRect.x = rect.xMax - btnCheckUpdateRect.width - 4;
        btnCheckUpdateRect.y += 4;
        btnCheckUpdateRect.y += 22;
        var temCol = GUI.backgroundColor;
        GUI.backgroundColor = new Color(0.03f, 0.99f, 0.21f, 1f) * 1.2f;
        if (GUI.Button(btnCheckUpdateRect, "Check for updates"))
        {
            XGameEditorUtil.CheckUpdate();
        }

        GUI.backgroundColor = temCol;

        var btnDocRect = new Rect(btnCheckUpdateRect);
        btnDocRect.y += btnDocRect.height + 2;

        GUI.backgroundColor = COLOR_DOC;
        if (GUI.Button(btnDocRect, "Documentation"))
        {
            Application.OpenURL(@"https://qu2tef36bb.feishu.cn/docx/Vasjd7bhOoNqMHxcAUCcMVrGnNg");
            Repaint();
        }


        // if (EditorUtility.IsDirty(Target))
        {
            var rectTips = new Rect(rect);
            rectTips.x = 8;
            rectTips.y = 8;
            rectTips.height = 16;
            rectTips.width = 220;
            var tipBottomRect = new Rect(rectTips);
            tipBottomRect.width += 4;
            tipBottomRect.height += 4;
            tipBottomRect.center = rectTips.center;
            EditorGUI.DrawRect(tipBottomRect, Color.green * 0.4f);
            GUI.Label(rectTips, new GUIContent("Save modifications to take effect", EditorIcons.Info.Raw));
        }


        GUI.backgroundColor = temCol;

        GUILayout.Space(30);
        GUILayout.Button("", GUILayout.Height(2));
        base.OnInspectorGUI();
        GUI.backgroundColor = COLOR_INSTALL;
        if (GUILayout.Button("Save", GUILayout.Height(30)))
        {
            SaveToolPreference();
            GUI.FocusControl("");
        }

        GUI.backgroundColor = temCol;
    }


    public static string BrowseGradle()
    {
        var folderPath = "";
        var userName = System.Environment.GetEnvironmentVariable("UserName").ToString();
        var gradleFolder = $@"C:\Users\{userName}\.gradle\wrapper\dists";
        if (Directory.Exists(gradleFolder))
        {
            folderPath = gradleFolder;
        }

        return EditorUtility.OpenFolderPanel("Locate Gradle", folderPath, "");
    }

    // private static void WriteXGPChannels()
    // {
    //     XGameEditorUtil.CheckCreateFolder(Path.GetDirectoryName(XGPChannels.ASSET_DATABASE_PATH));
    //     var asset = XGameEditorUtil.LoadOrCreate<XGPChannels>(XGPChannels.ASSET_DATABASE_PATH);
    //     //抖音
    //     // asset.ByteDanceChannelID = ToolPreference.Global.ByteDanceChannelId;
    //     asset.DouyinXSDKChannelID = ToolPreference.Global.DouyinXSDKChannel;
    //     //vivo
    //     // asset.VivoChannelID = ToolPreference.Global.VivoMiniSdkChannelId;
    //     // asset.VivoASCChannelID = ToolPreference.Global.VivoAscSdkChannelId;
    //     asset.VivoXSDKChannelID = ToolPreference.Global.VivoXSDKSdkChannelId;
    //     //oppo
    //     // asset.OppoChannelID = ToolPreference.Global.OppoMiniSdkChannelId;
    //     // asset.OppoASCChannelID = ToolPreference.Global.OppoAscSdkChannelId;
    //     asset.OppoXSDKChannelID = ToolPreference.Global.OppoXSDKSdkChannelId;
    //     //微信
    //     // asset.WechatChannelID = ToolPreference.Global.WeChatSdkChannelId;
    //     // asset.WechatASCChannelID = ToolPreference.Global.WeChatASCSdkChannelId;
    //     asset.WechatXSDKChannelID = ToolPreference.Global.WeChatXSDKSdkChannelId;
    //     EditorUtility.SetDirty(asset);
    //     AssetDatabase.SaveAssets();
    // }

    
    public static void SaveToolPreference()
    {
        //写入xgpchannels
        // WriteXGPChannels();
        
        EditorUtility.SetDirty(ToolPreference.Global);
        AssetDatabase.SaveAssets();
        
        XGameBuildAppUtility.UpdateAppConfig();

        XGameEditorUtil.Log("Saved successfully.", XGameEditorUtil.LogColor.Success);
    }
}