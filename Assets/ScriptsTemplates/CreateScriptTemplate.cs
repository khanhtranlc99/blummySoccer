#if UNITY_EDITOR
using UnityEditor;
public static class CreateScriptTemplate
{
    [MenuItem("Assets/Create/Script/NormalScript", priority = 40)]
    public static void CreateNormalScript()
    {
        string templatePath = "Assets/ScriptsTemplates/NormalScript.cs.txt";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "NormalScript.cs");
    }

    [MenuItem("Assets/Create/Script/UIScript", priority = 40)]
    public static void CreateUIScript()
    {
        string templatePath = "Assets/ScriptsTemplates/UIScript.cs.txt";
        ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, "UIScript.cs");
    }
}
#endif