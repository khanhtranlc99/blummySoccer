using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using XGame.BuildApp;

namespace XGame
{
    [CreateAssetMenu(menuName = "XGame Build/XGame iOS App Setting")]
    public class XGameIOSAppSetting : IOSAppSetting, IXGameAppSetting
    {
        
        [FoldoutGroup("AA Setting",expanded:true)]
        [LabelText("Enable Addressable Settings")]
        [LabelWidth(180)]
        [ShowIf("HasAddressable")]
        public bool EnableAddressableSetting = false;


        [FoldoutGroup("AA Setting")]
        [ShowIf("GetEnableAddressableSetting")]
        [ValueDropdown("ProfileSettingsOption")]
        //addressable use profile
        public string AddressableUseProfile;


        [FoldoutGroup("AA Setting")]
        [ShowIf("GetEnableAddressableSetting")]
        [SerializeField]
        [ListDrawerSettings(DefaultExpandedState = true)]
        [LabelText("Addressable parameter modification")]
        //aa构建版本
        public List<AddressableVariableModify> AAVariables = new List<AddressableVariableModify>();

        [OnInspectorGUI]
        [PropertyOrder(-1)]
        public void DrawTips()
        {
            switch (Channel)
            {
                case AppChannel.IOS_XGUG_China:
                case AppChannel.IOS_XGUG_Sea:
                {
                    var temColor = GUI.color;
                    GUI.color = new Color(0.11f, 0.84f, 0.42f, 1f);
                    GUILayout.Space(3);
                    EditorGUILayout.TextArea("Tips：");
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.TextArea("有订阅类型或者非消耗类型商品的游戏需要接入恢复购买功能。\nGames with subscription or non consumable products require access to restore purchase functionality. \nAPI:XGameSdk.RestoreCompletedPayInfo");
                    if (GUILayout.Button("Details", GUILayout.Width(60)))
                    {
                        Application.OpenURL(
                            "https://qu2tef36bb.feishu.cn/docx/Vasjd7bhOoNqMHxcAUCcMVrGnNg#Kfs7dbrSZoOvcjxNE4FcXX8Knjg");
                    }

                    GUILayout.EndHorizontal();
                    GUILayout.Space(3);
                    GUI.color = temColor;
                }
                    break;
            }
        }

        public AppChannel GetChannel => Channel;

        public bool GetEnableAddressableSetting => EnableAddressableSetting && HasAddressable;


        public string GetAddressableUseProfile => AddressableUseProfile;


        public List<AddressableVariableModify> GetAAVariablesModify => AAVariables;


        [FoldoutGroup("Publish Setting")] public string PublishName;

        public override void BuildAA()
        {
            BuildAddressableAssetTool.BuildExisting(GetAddressableUseProfile, setting: this);
        }

        private string[] ProfileSettingsOption
        {
            get
            {
                if (AddressableReflection.HasModule() && AddressableReflection.HasDefaultSettings())
                {
                    return AddressableReflection.Settings.profileSettings?.GetAllProfileNames().ToArray();
                }

                return new string[0];
            }
        }
        
        protected override void OnAfterSwitchPlateComplete()
        {
            base.OnAfterSwitchPlateComplete();
            ApplyTool.ApplyChannelSDK(Channel);
            
            //生成config
            XGameBuildAppUtility.udpateGeVersion(GetChannel);
            XGameBuildAppUtility.CreateAppConfig(GetChannel,
                VersionString, BuildType);
            //修改addressable
            XGameBuildAppUtility.ModifyAddressableConfig(this);
            AssetDatabase.Refresh();
            
            Debug.Log("切换成功");
        }

        protected override void OnPublish()
        {
            base.OnPublish();
            PublishIOS.PublishXCodeProject(this, name, PublishName);
        }
        
        private bool HasAddressable => AddressableReflection.HasModule();
    }
}