using UnityEditor;
using UnityEngine;
using XGame.BuildApp;

namespace XGame
{
    public class EditorPreference : ScriptableObject
    {
        private static EditorPreference _global = null;
        //全局配置
        public static EditorPreference Global
        {
            get
            {
                if (_global == null)
                {
                    _global = XGameEditorUtil.LoadOrCreate<EditorPreference>(
                        "Assets/XGameUnityTool_Gen/Editor/EditorPreference.asset");
                }

                return _global;
            }
        }
        
        public void Save()
        {
            EditorUtility.SetDirty(this);
            AssetDatabase.SaveAssets();
        }

        public AppChannel currentChannel;
        public PublishMode currentPublishMode;
        public bool isAddTestAdToXmyApk;
        public bool isAddTestAdToGoogleLogSDK;




    }
}