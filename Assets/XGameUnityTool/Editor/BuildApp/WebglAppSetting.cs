using System;
using UnityEditor;
using UnityEngine;

namespace XGame.BuildApp
{
    /// <summary>
    /// Webgl App Setting
    /// </summary>
    // [CreateAssetMenu(menuName = "打包设置/Webgl App Setting")]
    public class WebglAppSetting : BuildAppSetting
    {
        private void Reset()
        {
            BuildTarget = BuildTarget.WebGL;
        }

        protected override void OnUse()
        {
            base.OnUse();
            BuildAppUtil.SwitchPlatformTo(BuildTarget.WebGL, () =>
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
                ScriptingImplementation.Mono2x,
            };
        }
    }
}