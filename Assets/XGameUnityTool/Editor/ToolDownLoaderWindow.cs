using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    /// <summary>
    /// 下载器
    /// </summary>
    public class ToolDownLoaderWindow : EditorWindow
    {
        [Serializable]
        protected class JsonLastest
        {
            public string version;
            public string url;
        }

        private const string LastUrl = "http://remote.xplaymobile.com:5004/xgame-unity-tool/lastest.json";

        [MenuItem("XGameUnityTool/Downloader")]
        public static void Open()
        {
            var win = GetWindow<ToolDownLoaderWindow>();
            win.titleContent = new GUIContent("xgame-unity-tool-downloader");
            win.minSize = new Vector2(300, 100);
            win.maxSize = win.minSize;
            win.Show();
        }

        // [MenuItem("XGameUnityTool/Downloader", true)]
        // private static bool OpenToggle()
        // {
        //     if (File.Exists("Assets/XGameUnityTool/develop.txt"))
        //     {
        //         return true;
        //     }
        //
        //     return !File.Exists("Assets/XGameUnityTool/package.json");
        // }

        private int _process = 0;
        private string _url = string.Empty;
        private float _loadUrlProcess = 0;
        private DateTime _starTime;
        private string _version;

        private void OnEnable()
        {
            _process = 0;
            _version = "";
            _starTime = DateTime.Now;
            HttpGet(LastUrl, (res) =>
            {
                try
                {
                    // Debug.Log(res);
                    var data = JsonUtility.FromJson<JsonLastest>(res);
                    _url = data.url;
                    _version = data.version;
                    Debug.Log($"XgameUnityTool last version: {data.version}");
                }
                catch (Exception e)
                {
                    // ignored
                }
            }, (err) => { Debug.LogError($"Failed to retrieve version information for XgameUnityTool. err={err}"); }, 10);
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                Close();
            }

            if (string.IsNullOrWhiteSpace(_url))
            {
                _loadUrlProcess = (float)(DateTime.Now - _starTime).TotalSeconds;
                GUILayout.Box("Load version info...", GUILayout.Width(300), GUILayout.Height(100));
                var lastRect = GUILayoutUtility.GetLastRect();
                var processRect = new Rect(lastRect);
                var percent = _loadUrlProcess % 1;
                processRect.width = lastRect.width * percent;
                EditorGUI.DrawRect(processRect, Color.green * 0.5f);
                Repaint();
            }
            else
            {   
                GUILayout.Box($"xgame-unity-tool\nversion: {_version}", GUILayout.Height(40), GUILayout.Width(300));
                var isDownload = _process != 0;
                var btnDownloadTxt = isDownload ? $"Downloading...{_process}%" : "Download";
                var temEnable = GUI.enabled;
                GUI.enabled = !isDownload;
                GUI.backgroundColor = new Color(0.36f, 0.86f, 0.36f, 1f);
                if (GUILayout.Button(btnDownloadTxt, GUILayout.Height(50)))
                {
                    var last = _url.LastIndexOf('/') + 1;
                    var toolName = _url.Substring(last, _url.Length - last);
                    // var savePath = $"{Path.GetDirectoryName(Application.dataPath)}/xgame-unity-tool-download/{toolName}";
                    var savePath = Path.Combine(Path.GetDirectoryName(Application.dataPath),
                        "xgame-unity-tool-download", toolName);
                    DownloadSync(_url, savePath);
                }

                GUI.enabled = temEnable;
                if (_process != 0)
                {
                    var lastRect = GUILayoutUtility.GetLastRect();
                    var processRect = new Rect(lastRect);
                    processRect.width = lastRect.width * (_process / 100f);
                    EditorGUI.DrawRect(processRect, Color.green * 0.5f);
                    Repaint();
                }
            }
        }


        private void DownloadSync(string url, string savePath)
        {
            var window = this;
            CheckCreateFolder(Path.GetDirectoryName(savePath));

            Action import = () =>
            {
                try
                {
                    window.Close();
                }
                catch (Exception e)
                {
                    // ignored
                }

                ShowMessageBox("Download complete, proceed to import?",
                    () => { AssetDatabase.ImportPackage(savePath, true); });
            };
            if (File.Exists(savePath))
            {
                Debug.Log($"Local existence：{savePath},Directly import");
                import.Invoke();
                return;
            }

            using var webClient = new WebClient();
            webClient.DownloadProgressChanged += delegate(object _, DownloadProgressChangedEventArgs e)
            {
                try
                {
                    window._process = e.ProgressPercentage;
                    Debug.Log($"{e.ProgressPercentage.ToString()}% progress, download to {savePath}");
                }
                catch (Exception exception)
                {
                    // ignored
                }
            };

            webClient.DownloadFileCompleted += delegate(object _, System.ComponentModel.AsyncCompletedEventArgs e)
            {
                if (e.Error != null || e.Cancelled) return;
                try
                {
                    Debug.Log("Download successful");
                    window._process = 0;
                    if (File.Exists(savePath))
                    {
                        import?.Invoke();
                    }
                }
                catch (Exception exception)
                {
                    // ignored
                }
            };

            webClient.DownloadFileAsync(new Uri(url), savePath);
        }


        /// <summary>
        /// 显示提示框
        /// </summary>
        private static void ShowMessageBox(string msg, Action success = null, Action fail = null)
        {
#if UNITY_EDITOR
            if (EditorUtility.DisplayDialog("prompt", msg, "ok"))
                success?.Invoke();
            else
                fail?.Invoke();
#endif
        }


        private static void CheckCreateFolder(string path)
        {
            if (Directory.Exists(path)) return;
            Directory.CreateDirectory(path);
            AssetDatabase.Refresh();
        }


        private static async void HttpGet(string url, Action<string> success, Action<string> error = null,
            int timeOut = 60)
        {
            var req =
                (HttpWebRequest)WebRequest.Create(url);
            req.Method = "GET";
            req.Timeout = timeOut * 1000;
            req.ContentType = "application/json";
            try
            {
                var resp = await req.GetResponseAsync();
                var response = (HttpWebResponse)resp;
                var resStream = response.GetResponseStream();
                if (response.StatusCode == HttpStatusCode.OK && resStream != null)
                {
                    using var reader = new StreamReader(resStream, Encoding.UTF8);
                    var json = await reader.ReadToEndAsync();
                    success?.Invoke(json);
                }
                else
                {
                    error?.Invoke(response.StatusCode.ToString());
                }
            }
            catch (Exception e)
            {
                error?.Invoke(e.Message);
            }
        }
    }
}