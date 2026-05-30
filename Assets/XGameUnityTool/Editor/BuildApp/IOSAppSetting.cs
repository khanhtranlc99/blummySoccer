using System;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace XGame.BuildApp
{
    // [CreateAssetMenu(menuName = "打包设置/IOS App Setting")]
    public class IOSAppSetting : BuildAppSetting
    {
        private void Reset()
        {
            BuildTarget = BuildTarget.iOS;
        }

        protected override void OnUse()
        {
            base.OnUse();
            BuildAppUtil.SwitchPlatformTo(BuildTarget.iOS, () =>
            {
                InvokeSubmitBefore(this);
                OnAfterSwitchPlateComplete();
                InvokeSubmitComplete(this);
            });
        }

        protected override void OnAfterSwitchPlateComplete()
        {
            base.OnAfterSwitchPlateComplete();
        }

        protected override ScriptingImplementation[] GetScriptingImplementationOptions()
        {
            return new[]
            {
                ScriptingImplementation.IL2CPP,
                // ScriptingImplementation.Mono2x,
            };
        }


        // [Button("导出XCode工程"), HorizontalGroup("buildMenu")]
        protected virtual void OnClickExportXcode()
        {
            BuildPlayerWindow.ShowBuildPlayerWindow();
        }
    }
}