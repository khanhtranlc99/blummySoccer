using System;
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XGame.BuildApp
{
    /// <summary>
    /// AndroidApp设置
    /// </summary>
    // [CreateAssetMenu(menuName = "打包设置/Android App Setting")]
    public class AndroidAppSetting : BuildAppSetting
    {
        private void Reset()
        {
            BuildTarget = BuildTarget.Android;
        }

        protected override void OnAfterDeserialize()
        {
        }
        
        [FoldoutGroup("Publish Setting")]
        //发布模式
        public PublishMode PublishMode;
        
        [FoldoutGroup("Publish Setting")]
        public AndroidSdkVersions MinApiVersion = AndroidSdkVersions.AndroidApiLevel23;
        
        [FoldoutGroup("Publish Setting")]
        public AndroidSdkVersions TargetApiVersion = AndroidSdkVersions.AndroidApiLevelAuto;

        [FoldoutGroup("Publish Setting")] public AndroidArchitecture targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        
        // [FoldoutGroup("Publish Setting")] public bool ArmV7 = true;

        // [FoldoutGroup("Publish Setting")] [ShowIf("Arm64CVaild")]
        // public bool ArmV64 = true;
        
        [FoldoutGroup("Publish Setting")] [LabelText("Package Name")]
        public string PackageName;

        [FoldoutGroup("Publish Setting")] [LabelText("Enable Signature")]
        public bool EnableSignature;

        [FoldoutGroup("Publish Setting")]
        [Sirenix.OdinInspector.FilePath(AbsolutePath = true)]
        [LabelText("Keystore")]
        [ShowIf("EnableSignature")]
        public string KeystoreName;

        //Keystore密码
        [FoldoutGroup("Publish Setting")] [LabelText("Keystore Password")] [ShowIf("EnableSignature")]
        public string KeystorePassword = "";

        [FoldoutGroup("Publish Setting")] [LabelText("Alias")] [ShowIf("EnableSignature")]
        public string KeyAliasName;

        //Alias密码
        [FoldoutGroup("Publish Setting")] [LabelText("Alias Password")] [ShowIf("EnableSignature")]
        public string KeyAliasPassword = "";



        protected override void OnUse()
        {
            base.OnUse();

            BuildAppUtil.SwitchPlatformTo(BuildTarget, () =>
            {
                InvokeSubmitBefore(this);
                OnAfterSwitchPlateComplete();
                InvokeSubmitComplete(this);
            });
        }

        //完成平台切换
        protected override void OnAfterSwitchPlateComplete()
        {
            base.OnAfterSwitchPlateComplete();

            /*设置包名*/
            if (!string.IsNullOrWhiteSpace(PackageName))
            {
                PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, PackageName);
            }

            //检查签名
            CheckSignature();

            /*设置签名*/
#if UNITY_2019_1_OR_NEWER
            PlayerSettings.Android.useCustomKeystore = EnableSignature;
            PlayerSettings.Android.keystoreName = KeystoreName;
            PlayerSettings.Android.keystorePass = KeystorePassword;
            PlayerSettings.Android.keyaliasName = KeyAliasName;
            PlayerSettings.Android.keyaliasPass = KeyAliasPassword;
#endif

            /*设置支持cpu架构*/
            PlayerSettings.Android.targetArchitectures = targetArchitectures;
            
            // if (ArmV7 && ArmV64)
            // {
            //     if (Arm64CVaild)
            //     {
            //         PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64 | AndroidArchitecture.ARMv7;
            //     }
            //     else
            //     {
            //         PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            //     }
            // }
            
            // if (ArmV7 && ArmV64 == false)
            // {
            //     PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
            // }

            // if (ArmV64 && ArmV7 == false)
            // {
            //     if (Arm64CVaild)
            //     {
            //         //有效
            //         PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
            //     }
            //     else
            //     {
            //         //无效
            //         PlayerSettings.Android.targetArchitectures = AndroidArchitecture.None;
            //     }
            // }
            //
            // if (ArmV64 == false && ArmV7 == false)
            // {
            //     PlayerSettings.Android.targetArchitectures = AndroidArchitecture.None;
            // }
            
            PlayerSettings.Android.minSdkVersion = MinApiVersion;
            PlayerSettings.Android.targetSdkVersion = TargetApiVersion;
        }

        protected override ScriptingImplementation[] GetScriptingImplementationOptions()
        {
            return new[]
            {
                ScriptingImplementation.IL2CPP,
                // ScriptingImplementation.Mono2x,
            };
        }


        // private bool Arm64CVaild => ScriptingImplementation == ScriptingImplementation.IL2CPP;

        protected virtual void OnClickBuildApk()
        {
            SwitchToApk();
            BuildPlayerWindow.ShowBuildPlayerWindow();
        }


        protected virtual void OnClickBuildAAB()
        {
            SwitchToAAB();
            BuildPlayerWindow.ShowBuildPlayerWindow();
        }


        protected virtual void OnClickBuildProject()
        {
            SwitchToProject();
            BuildPlayerWindow.ShowBuildPlayerWindow();
        }

        protected void SwitchToApk()
        {
            PublishAndroid.SwitchToApk();
        }


        protected void SwitchToAAB()
        {
            PublishAndroid.SwitchToAAB();
        }


        protected void SwitchToProject()
        {
            PublishAndroid.SwitchToProject();
        }


        private void CheckSignature()
        {
            if (EnableSignature)
            {
                if (!File.Exists(KeystoreName))
                {
                    throw new Exception($"Signature file not found {KeyAliasName}");
                }
            }
        }
    }
}