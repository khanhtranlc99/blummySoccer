using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Sirenix.OdinInspector;
using Sirenix.Utilities.Editor;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;
using XGame.BuildApp;
using Debug = UnityEngine.Debug;


namespace XGame
{
    [CreateAssetMenu(menuName = "XGame Build/XGame Android App Setting")]
    public class XGameAndroidAppSetting : AndroidAppSetting, IXGameAppSetting
    {
        //参数修改
        [FoldoutGroup("Publish Setting")] public GraphicsDeviceType[] GraphicsAPIs = Array.Empty<GraphicsDeviceType>();
#if UNITY_2021_3_OR_NEWER
        [FoldoutGroup("Publish Setting")] [LabelText("Texture Compression")]
        public MobileTextureSubtarget TextureSubtarget = MobileTextureSubtarget.ASTC;
#else
        [FoldoutGroup("Publish Setting")] [LabelText("Texture Compression")]
        public MobileTextureSubtarget TextureSubtarget = MobileTextureSubtarget.Generic;
#endif

        [FoldoutGroup("AA Setting", expanded: true)]
        [LabelText("Enable Addressable Settings")]
        [LabelWidth(180)]
        [SerializeField]
        [ShowIf("HasAddressable")]
        public bool EnableAddressableSetting;


        [FoldoutGroup("AA Setting")] [ValueDropdown("ProfileSettingsOption")] [ShowIf("GetEnableAddressableSetting")]
        public string AddressableUseProfile;

        [FoldoutGroup("AA Setting")]
        [ShowIf("GetEnableAddressableSetting")] 
        [SerializeField]
        [LabelText("Addressable parameter modification")]
        [TableList(AlwaysExpanded = true)]
        public List<AddressableVariableModify> AAVariablesModify = new List<AddressableVariableModify>();


        [FoldoutGroup("AA Setting")] [ShowIf("HasAddressable")] [ShowIf("IsPublishXMYGoogle")] [LabelText("Split AA resources by country")] [LabelWidth(180)]
        public bool OpenSplitAAResByCountry = false;

        [FoldoutGroup("AA Setting")] [ShowIf("OpenSplitAAResByCountry")] [LabelText("Export directory")] [FolderPath] [LabelWidth(180)]
        public string SplitAAExportPath = "countryAA";

        [OnInspectorGUI]
        [FoldoutGroup("AA Setting")]
        [ShowIf("OpenSplitAAResByCountry")]
        private void CheckSplitAAByCountry()
        {
            var sameGroup = GetSameGroupAAByCountry();
            if (sameGroup != string.Empty)
            {
                GUILayout.Label(new GUIContent($"国家分包存在重复Asset Group: {sameGroup}", EditorIcons.UnityWarningIcon));
            }
        }

        private string GetSameGroupAAByCountry()
        {
            var dic = new Dictionary<int, object>();
            var arr = new List<string>();

            var option = SplitAaDataOption;
            foreach (var item in option)
            {
                if (item.AssetGroup != null)
                {
                    var instanceId = item.AssetGroup.GetInstanceID();
                    if (dic.ContainsKey(instanceId))
                    {
                        arr.Add(item.AssetGroup.name);
                        continue;
                    }

                    dic[instanceId] = null;
                }
            }

            var str = new StringBuilder();
            if (arr.Count > 0)
            {
                str.Append("\n");
                for (var i = 1; i <= arr.Count; i++)
                {
                    str.Append(arr[i - 1]);
                    if (i % 5 == 0)
                    {
                        str.Append("\n");
                    }
                    else
                    {
                        if (i != arr.Count)
                        {
                            str.Append(", ");
                        }
                    }
                }
            }

            return str.ToString();
        }

        [FoldoutGroup("AA Setting")]
        [ShowIf("OpenSplitAAResByCountry")]
        [LabelText("Split settings")]
        [TableList(ShowIndexLabels = false)]
        public List<SplitAAOption> SplitAaDataOption = new List<SplitAAOption>();


        [FoldoutGroup("AA Setting")]
        [ShowIf("HasAddressable")]
        [LabelText("Split AA resources by PlayAssetDelivery")]
        [LabelWidth(240)]
        [ShowIf("IsPublishXMYGoogle")]
        public bool PlayAssetDeliveryIsEnable = false;

        [FoldoutGroup("AA Setting")] [ShowIf("PlayAssetDeliveryIsEnable")] [LabelText("Export directory")] [FolderPath] [LabelWidth(240)]
        public string PlayAssetDeliveryExportPath = "pad_export";

        [FoldoutGroup("AA Setting")]
        [ShowIf("PlayAssetDeliveryIsEnable")]
        [LabelText("Split settings")]
        [TableList(ShowIndexLabels = false)]
        public List<PlayAssetDeliveryOption> PlayAssetDeliveryOptions = new List<PlayAssetDeliveryOption>();


        [OnInspectorGUI]
        [FoldoutGroup("AA Setting")]
        [ShowIf("PlayAssetDeliveryIsEnable")]
        private void CheckSplitPad()
        {
            var sameGroup = GetSameGroupPad();
            if (sameGroup != string.Empty)
            {
                GUILayout.Label(new GUIContent($"PAD分包存在重复Asset Group: {sameGroup}", EditorIcons.UnityWarningIcon));
            }
        }

        private string GetSameGroupPad()
        {
            var dic = new Dictionary<int, object>();
            var arr = new List<string>();

            var option = PlayAssetDeliveryOptions;
            foreach (var item in option)
            {
                if (item.AssetGroup != null)
                {
                    var instanceId = item.AssetGroup.GetInstanceID();
                    if (dic.ContainsKey(instanceId))
                    {
                        arr.Add(item.AssetGroup.name);
                        continue;
                    }

                    dic[instanceId] = null;
                }
            }

            var str = new StringBuilder();
            if (arr.Count > 0)
            {
                str.Append("\n");
                for (var i = 1; i <= arr.Count; i++)
                {
                    str.Append(arr[i - 1]);
                    if (i % 5 == 0)
                    {
                        str.Append("\n");
                    }
                    else
                    {
                        if (i != arr.Count)
                        {
                            str.Append(", ");
                        }
                    }
                }
            }

            return str.ToString();
        }

        public bool OpenSplitAAByCountryAndPad => OpenSplitAAResByCountry && PlayAssetDeliveryIsEnable;
        // public bool OpenSplitAAByCountryOrPad => (OpenSplitAAResByCountry && SplitAaDataOption.Count > 0) || (PlayAssetDeliveryIsEnable && PlayAssetDeliveryOptions.Count > 0);

        [OnInspectorGUI]
        [FoldoutGroup("AA Setting")]
        [ShowIf("OpenSplitAAByCountryAndPad")]
        private void CheckCountryAndPadSameGroup()
        {
            var sameGroup = GetSameGroupCountryAndPad();
            if (sameGroup != string.Empty)
            {
                GUILayout.Label(new GUIContent($"国家分包和PAD分包的配置同时存在相同的Asset Group: {sameGroup}",
                    EditorIcons.UnityWarningIcon));
            }
        }

        private string GetSameGroupCountryAndPad()
        {
            var dic = new Dictionary<int, object>();
            var arr = new List<string>();

            var option = PlayAssetDeliveryOptions;
            var option2 = SplitAaDataOption;
            foreach (var item in option)
            {
                if (item.AssetGroup != null)
                {
                    var instanceId = item.AssetGroup.GetInstanceID();
                    dic[instanceId] = null;
                }
            }

            foreach (var item in option2)
            {
                if (item.AssetGroup != null)
                {
                    var instanceId = item.AssetGroup.GetInstanceID();
                    if (dic.ContainsKey(instanceId))
                    {
                        arr.Add(item.AssetGroup.name);
                    }
                }
            }

            var str = new StringBuilder();
            if (arr.Count > 0)
            {
                str.Append("\n");
                for (var i = 1; i <= arr.Count; i++)
                {
                    str.Append(arr[i - 1]);
                    if (i % 5 == 0)
                    {
                        str.Append("\n");
                    }
                    else
                    {
                        if (i != arr.Count)
                        {
                            str.Append(", ");
                        }
                    }
                }
            }

            return str.ToString();
        }


        [FoldoutGroup("Publish Setting")] public string PublishName;

        [FoldoutGroup("Publish Setting")] [LabelText("Append Android Project")] [ShowIf("IsPublishXMYGoogleNoApk")]
        public bool IsOverBuild = false;

        [FoldoutGroup("Publish Setting")] [LabelText("Generate Symbol Files")] [ShowIf("IsSupportCreateSymbols")]
#if UNITY_2021_3_OR_NEWER
        public AndroidCreateSymbols IsCreateSymbols = AndroidCreateSymbols.Public;
#else
        public bool IsCreateSymbols = true;
#endif

        [FoldoutGroup("Publish Setting")] [ShowIf("IsPublishGoogle")]
        public BuildOptions BuildOptions = BuildOptions.None;

        [FoldoutGroup("Publish Setting")]
        [ShowIf("IsPublishGoogle")]
        [LabelText("Open Test Ad(?)")]
        [Tooltip("向构建的APK添加测试广告依赖，方便本地测试。Add test ad dependencies to the built APK for easy local testing")]
        public bool isAddTestAd;

        [FoldoutGroup("Publish Setting")] [ShowIf("IsPublishAar")] [LabelText("Generate AAR")]
        public bool isOutputAar = true;

        public AppChannel GetChannel => Channel;
        public bool GetEnableAddressableSetting => EnableAddressableSetting && HasAddressable;
        public string GetAddressableUseProfile => AddressableUseProfile;
        public List<AddressableVariableModify> GetAAVariablesModify => AAVariablesModify;

        public bool IsPublishXMYGoogle => Channel == AppChannel.XMYGoogle || Channel == AppChannel.Google_Log_SDK;
        public bool IsPublishXMYGoogleNoApk => PublishMode == PublishMode.XMYGoogle;

        public bool IsSupportCreateSymbols =>
            Channel == AppChannel.XMYGoogle || Channel == AppChannel.Mar || Channel == AppChannel.Android_Light ||
            Channel == AppChannel.Google_Log_SDK;

        public bool IsPublishGoogle => PublishMode == PublishMode.APK &&
                                       (Channel == AppChannel.XMYGoogle || Channel == AppChannel.Google_Log_SDK);

        public bool IsPublishAar => IsPublishGoogle || Channel == AppChannel.Android_Light || Channel == AppChannel.Mar;

        private string[] ProfileSettingsOption
        {
            get
            {
                if (AddressableReflection.HasModule() && AddressableReflection.HasDefaultSettings())
                {
                    return AddressableReflection.Settings.profileSettings?.GetAllProfileNames().ToArray();
                }

                return new string[] { };
            }
        }

        public override void BuildAA()
        {
            var openSplit = false;
            var enablePlayAssetDeliver = false;
            if (HasAddressable)
            {
                openSplit = OpenSplitAAResByCountry;
                enablePlayAssetDeliver = PlayAssetDeliveryIsEnable;
            }

            BuildAddressableAssetTool.BuildExisting(AddressableUseProfile, openSplit, SplitAAExportPath,
                SplitAaDataOption, enablePlayAssetDeliver, PlayAssetDeliveryExportPath, PlayAssetDeliveryOptions, this);
            Debug.Log("AA构建完毕");
        }


        protected override void OnAfterSwitchPlateComplete()
        {
            base.OnAfterSwitchPlateComplete();
            //设置GraphicsAPIs
            if (null != GraphicsAPIs && GraphicsAPIs.Length > 0)
            {
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, false);
                PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, GraphicsAPIs);
            }
            else
            {
                PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.Android, true);
            }

            EditorUserBuildSettings.androidBuildSubtarget = TextureSubtarget;

            ApplyTool.ApplyChannelSDK(Channel);

            //生成config
            XGameBuildAppUtility.udpateGeVersion(GetChannel);
            XGameBuildAppUtility.CreateAppConfig(Channel,
                VersionString, BuildType);
            //修改addressable
            XGameBuildAppUtility.ModifyAddressableConfig(this);
            AssetDatabase.Refresh();

            Debug.Log("切换成功");
        }


        protected override void OnPublish()
        {
            base.OnPublish();

            EditorPreference.Global.currentPublishMode = PublishMode;
            EditorPreference.Global.isAddTestAdToXmyApk = isAddTestAd;
            EditorPreference.Global.isAddTestAdToGoogleLogSDK = isAddTestAd;
            EditorPreference.Global.Save();

            if (IsSupportCreateSymbols)
            {
#if UNITY_2021_3_OR_NEWER
                EditorUserBuildSettings.androidCreateSymbols = IsCreateSymbols;
#else
                EditorUserBuildSettings.androidCreateSymbolsZip = IsCreateSymbols;
#endif
            }

            //执行发布逻辑
            switch (PublishMode)
            {
                case PublishMode.APK: //发布apk
                {
                    if (Channel == AppChannel.XMYGoogle)
                    {
                        var type = XGameEditorUtil.GetType("XGame.PublishXMYGoogleApk");
                        dynamic publish = Activator.CreateInstance(type);
                        publish.Publish(this);
                    }
                    else if (Channel == AppChannel.Google_Log_SDK)
                    {
                        var type = XGameEditorUtil.GetType("XGame.PublishGoogleLogSDKApk");
                        dynamic publish = Activator.CreateInstance(type);
                        publish.Publish(this);
                    }
                    else if (Channel == AppChannel.Android_Light)
                    {
                        var type = XGameEditorUtil.GetType("XGame.PublishLightSdkApk");
                        dynamic publish = Activator.CreateInstance(type);
                        publish.Publish(this);
                    } 
                    else if (Channel == AppChannel.Mar)
                    {
                        var type = XGameEditorUtil.GetType("XGame.PublishMarSdkApk");
                        dynamic publish = Activator.CreateInstance(type);
                        publish.Publish(this);
                    }
                    else
                    {
                        PublishAndroid.PublishApk(this, name, PublishName);
                    }
                }
                    break;
                case PublishMode.Project: //导出工程
                {
                    PublishAndroid.PublishProject(this, name, PublishName);
                }
                    break;
                case PublishMode.AAB: //导出aab
                {
                    PublishAndroid.PublishAAB(this, name, PublishName);
                }
                    break;
                case PublishMode.XMYGoogle:
                {
                    var type = XGameEditorUtil.GetType("XGame.PublishXMYGoogle");
                    dynamic publish = Activator.CreateInstance(type);
                    publish.Publish(this, name, PublishName);
                }
                    break;
                case PublishMode.DouyinXSDKAndroid:
                {
                    var type = XGameEditorUtil.GetType("XGame.PublishDouyinXSDK");
                    dynamic publish = Activator.CreateInstance(type);
                    publish.Publish(this);
                }
                    break;
                case PublishMode.KuaishouXSDKAndroid:
                {
                    var type = XGameEditorUtil.GetType("XGame.PublishKuaishouXSDKAndroid");
                    dynamic publish = Activator.CreateInstance(type);
                    publish.Publish(this);
                }
                    break;
                default:
                    throw new Exception($"未实现发布逻辑：{PublishMode}");
            }
        }


        private bool HasAddressable => AddressableReflection.HasModule();
    }
}