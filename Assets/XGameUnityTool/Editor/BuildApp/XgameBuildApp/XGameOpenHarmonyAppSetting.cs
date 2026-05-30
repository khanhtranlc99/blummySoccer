using System;
using XGame.BuildApp;
using System.Collections.Generic;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XGame
{
    [CreateAssetMenu(menuName = "XGame Build/XGame OpenHarmony App Setting")]
    public class XGameOpenHarmonyAppSetting : OpenHarmonyAppSetting, IXGameAppSetting
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
        
        [FoldoutGroup("Publish Setting")] public string PublishName;

        
        public AppChannel GetChannel => Channel;

        public bool GetEnableAddressableSetting => EnableAddressableSetting && HasAddressable;


        public string GetAddressableUseProfile => AddressableUseProfile;


        public List<AddressableVariableModify> GetAAVariablesModify => AAVariables;
        
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

            switch (Channel)
            {
                case AppChannel.OpenHarmony_Light:
                {
                    var type = XGameEditorUtil.GetType("XGame.PublishXGameOpenHarmony");
                    dynamic publish = Activator.CreateInstance(type);
                    publish.Publish(this);
                }
                    break;
                default:
                    throw new Exception($"未实现发布逻辑：{Channel}");
            }
        }
        
        private bool HasAddressable => AddressableReflection.HasModule();
    }
}