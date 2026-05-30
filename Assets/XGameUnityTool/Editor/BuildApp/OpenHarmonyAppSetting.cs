using System;
using UnityEditor;
using UnityEngine;

namespace XGame.BuildApp
{
    public class OpenHarmonyAppSetting : BuildAppSetting
    {
        private void Reset()
        {
#if UNITY_OPENHARMONY
            BuildTarget = BuildTarget.OpenHarmony;
#endif
        }

        protected override void OnUse()
        {
#if UNITY_OPENHARMONY
            base.OnUse();
            BuildAppUtil.SwitchPlatformTo(BuildTarget.OpenHarmony, () =>
            {
                InvokeSubmitBefore(this);
                OnAfterSwitchPlateComplete();
                InvokeSubmitComplete(this);
            });
#else
            Debug.Log("当前平台不是鸿蒙！！");

#endif
        }

        protected override void OnAfterSwitchPlateComplete()
        {
#if UNITY_OPENHARMONY
            BuildTarget = BuildTarget.OpenHarmony;
#endif
            base.OnAfterSwitchPlateComplete();
        }

        protected override void OnPublish()
        {
#if UNITY_OPENHARMONY
            BuildTarget = BuildTarget.OpenHarmony;
#endif
            base.OnPublish();
        }

        protected override ScriptingImplementation[] GetScriptingImplementationOptions()
        {
            return new[]
            {
                ScriptingImplementation.IL2CPP,
            };
        }
    }
}