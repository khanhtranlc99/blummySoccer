using System;
using System.IO;
using System.Net;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    //工具更新窗口
    public class UpdateToolWindow : EditorWindow
    {
        //更新日志云端路径
        private static string changelog_remote_url = $"{Paths.REMOTE_XGAME_UNTIY_TOOL_URL}/CHANGELOG.md";

        //缓存路径
        private static string changelog_cache_path = $"Assets/XGameUnityTool_Gen/Editor/remote_changelog.md";

        private static string _PackageUrl;

        private static string[] loadingTips = new[]
        {
            "Downloading.",
            "Downloading..",
            "Downloading..."
        };

        private ActionHandler<string> _loadHandler;

        private TextAsset _cacheChangelog = null;

        private static dynamic _inspectorWindow;

        private static string _Version;

        private static string _CurrentVersion;
        private float _process;

        public static void Open(string packageUrl, string version)
        {
            _PackageUrl = packageUrl;
            _Version = version;
            _CurrentVersion = XGameEditorUtil.GetToolVersion();
            var win = GetWindow<UpdateToolWindow>();
            win.minSize = new Vector2(220, 130);
            win.maxSize = win.minSize;
            win.titleContent = new GUIContent($"Update xgame-unity-tool");
            win.Show();
        }

        private void OnEnable()
        {
            _process = 0;
        }

        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                Close();
            }

            if (string.IsNullOrWhiteSpace(_PackageUrl))
            {
                Close();
            }

            GUILayout.Label($"current version：{_CurrentVersion}", GUILayout.Height(22));
            GUILayout.Label($"latest version：{_Version}", GUILayout.Height(22));
            GUILayout.Button("", GUILayout.Height(2));
            GUILayout.Space(5);
            var btnLoadTxt = $"release notes";
            if (_loadHandler != null)
            {
                var index = DateTime.Now.Ticks % loadingTips.Length;
                btnLoadTxt = loadingTips[index];
            }

            var temBgCol = GUI.backgroundColor;
            GUI.backgroundColor = new Color(1f, 0.85f, 0f, 1f);
            if (GUILayout.Button(btnLoadTxt, GUILayout.Height(32)))
            {
                if (_cacheChangelog == null)
                {
                    if (_loadHandler == null)
                    {
                        LoadRemoteChangeLog();
                    }
                }
                else
                {
                    var changelog = _cacheChangelog;
                    EditorApplication.delayCall += () =>
                    {
                        if (changelog != null)
                        {
                            Selection.activeObject = changelog;
                        }

                        NewInspectorEditorWindow();
                    };
                }
            }

            GUI.backgroundColor = temBgCol;
            var isDownload = _process != 0;
            var temEnable = GUI.enabled;
            GUI.enabled = !isDownload;
            GUI.backgroundColor = new Color(0.36f, 0.86f, 0.36f, 1f);
            var btnDownloadTxt = isDownload ? $"Downloading..." : "Download";

            if (GUILayout.Button(btnDownloadTxt, GUILayout.Height(32)))
            {
                var last = _PackageUrl.LastIndexOf('/') + 1;
                var name = _PackageUrl.Substring(last, _PackageUrl.Length - last);
                var savePath = $"{Path.GetDirectoryName(Application.dataPath)}/xgame-unity-tool-download/{name}";
                DownloadSync(_PackageUrl, savePath);
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


            GUI.backgroundColor = temBgCol;
        }

        private void OnDisable()
        {
            ClearHandler();
            if (_inspectorWindow != null)
            {
                _inspectorWindow.Close();
            }
        }


        public static void NewInspectorEditorWindow()
        {
            if (_inspectorWindow != null)
            {
                _inspectorWindow.Close();
            }

            string inspectorWindowTypeName = "UnityEditor.InspectorWindow";
            var windowType = typeof(Editor).Assembly.GetType(inspectorWindowTypeName);
            dynamic window = EditorWindow.CreateInstance(windowType);
            PropertyInfo info = windowType.GetProperty("isLocked");
            info.SetValue(window, true);
            _inspectorWindow = window;
            window.Show();
        }

        private void ClearHandler()
        {
            if (_loadHandler != null)
            {
                _loadHandler.Clear();
                _loadHandler = null;
            }
        }

        private void LoadRemoteChangeLog()
        {
            ClearHandler();
            //删除本地
            if (File.Exists(changelog_cache_path))
            {
                File.Delete(changelog_cache_path);
                AssetDatabase.Refresh();
            }

            var handler = new ActionHandler<string>((res) =>
            {
                XGameEditorUtil.CheckCreateFolder(Path.GetDirectoryName(changelog_cache_path));
                File.WriteAllText(changelog_cache_path, res);
                AssetDatabase.Refresh();
                _cacheChangelog = AssetDatabase.LoadAssetAtPath<TextAsset>(changelog_cache_path);
                var asset = _cacheChangelog;
                EditorApplication.delayCall += () =>
                {
                    if (asset != null)
                    {
                        Selection.activeObject = asset;
                    }

                    NewInspectorEditorWindow();
                };
            });

            _loadHandler = handler;

            //云端加载更新日志
            XGameEditorUtil.HttpGet(changelog_remote_url, (res) =>
            {
                // Debug.Log($"Download succeeded: {res}");
                handler?.Invoke(res);
                if (_loadHandler == handler)
                {
                    _loadHandler = null;
                }
            }, (err) => { Debug.Log("Download failed."); });
        }


        private void DownloadSync(string url, string savePath)
        {
            var window = this;
            XGameEditorUtil.CheckCreateFolder(Path.GetDirectoryName(savePath));

            Action import = () =>
            {
                try
                {
                    window?.Close();
                }
                catch (Exception e)
                {
                }

                var path = savePath;
                EditorApplication.delayCall += () =>
                {
                    XGameEditorUtil.ShowMessageBox("Download complete. Import the package?",
                        () => { XGameEditorUtil.SafeImportUnityPackage(path, true); });
                };
            };
            if (File.Exists(savePath))
            {
                Debug.Log($"File already exists locally: {savePath}. Importing directly.");
                import?.Invoke();
                return;
            }

            using (WebClient webClient = new WebClient())
            {
                webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(
                    delegate(object sender, DownloadProgressChangedEventArgs e)
                    {
                        try
                        {
                            window._process = e.ProgressPercentage;
                            Debug.Log($"Downloading [{e.ProgressPercentage}%]: {url} -> {savePath}");
                        }
                        catch (Exception exception)
                        {
                        }
                    });

                webClient.DownloadFileCompleted += new System.ComponentModel.AsyncCompletedEventHandler
                (delegate(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
                {
                    if (e.Error == null && !e.Cancelled)
                    {
                        try
                        {
                            Debug.Log("Download succeeded.");
                            window._process = 0;
                            if (File.Exists(savePath))
                            {
                                import?.Invoke();
                            }
                        }
                        catch (Exception exception)
                        {
                        }
                    }
                });

                webClient.DownloadFileAsync(new Uri(url), savePath);
            }
        }
    }
}