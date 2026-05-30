using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;
using XGame.BuildApp;

[CustomEditor(typeof(BuildAppSetting), true)]
public class BuildAppSettingInspector : OdinEditor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        base.OnInspectorGUI();
        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
            AssetDatabase.SaveAssets();
        }
    }
}