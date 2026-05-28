using Microsoft.CSharp;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnumEditor
{
# if UNITY_EDITOR
    public static void GenerateEnumScript(List<string> ListStrings, string EnumType)
    {
        string filePath = Path.Combine("Assets/_Game/Scripts/Base/ENUM", EnumType + ".cs");

        // Ensure the directory exists
        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

        // Generate the enum script content
        string enumContent = GenerateEnumContent(EnumType, ListStrings);

        // Write the enum content to a script file
        File.WriteAllText(filePath, enumContent);

        // Refresh the Unity Asset Database
#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif
    }

    static string GenerateEnumContent(string enumName, List<string> enumValues)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.AppendLine("public enum " + enumName);
        sb.AppendLine("{");

        for (int i = 0; i < enumValues.Count; i++)
        {
            sb.AppendLine("    " + enumValues[i] + " = " + i + (i < enumValues.Count - 1 ? "," : ""));
            if (i < enumValues.Count - 1)
            {
                sb.AppendLine();
            }
        }

        sb.AppendLine("}");
        return sb.ToString();
    }
#endif
}
