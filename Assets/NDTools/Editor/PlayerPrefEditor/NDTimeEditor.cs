using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ND.Tools
{
    public class NDTimeEditor : NDToolOption
    {
        float timeScale = 1f;
        float lastTimeScale = 1f;
        protected override string GetOptionName()
        {
            return "Time Controller";
        }
        public override void OnGUI()
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("DeltaTime (Play mode)");
            GUILayout.Label(Time.deltaTime.ToString());
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            timeScale = EditorGUI.FloatField(new Rect(), timeScale);
            timeScale = EditorGUILayout.Slider(timeScale, 0, 2);
            if(lastTimeScale != timeScale)
            {
                Time.timeScale = timeScale;
                lastTimeScale = timeScale;
            }
            GUILayout.EndHorizontal();
        }
    }
}

