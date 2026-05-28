using ND.Tools;
using UnityEditor;
using UnityEngine;

public class NDTextureCompress : NDToolOption
{
    string folder = "Assets/_GAME/_Textures/UI/ChallengerMode";
    TextureImporterFormat type = TextureImporterFormat.ETC2_RGBA8Crunched;
    protected override string GetOptionName()
    {
        return "Change Texture Compress Type";
    }

    public override void OnGUI()
    {
        folder = GUILayout.TextField(folder);
        type = (TextureImporterFormat)EditorGUILayout.EnumPopup(type);
        if (GUILayout.Button("Change compress type all"))
        {
            ChangeCompressTexturesInFolder();
        }
    }
    void ChangeCompressTexturesInFolder()
    {
        string[] guids = AssetDatabase.FindAssets("t:Sprite t:Texture2D t:Texture", new[] { folder });
        int count = 0;
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;

            if (importer != null)
            {
                importer.textureCompression = TextureImporterCompression.Compressed;

                TextureImporterPlatformSettings androidSettings = new TextureImporterPlatformSettings();
                androidSettings.name = "Android";
                androidSettings.overridden = true;
                androidSettings.format = type;

                importer.SetPlatformTextureSettings(androidSettings);

                AssetDatabase.ImportAsset(path);
                Debug.Log("Converted: " + path);
                count++;
            }
        }

        AssetDatabase.Refresh();
        Debug.Log($"Converted {count} items");
        Debug.Log("Conversion complete!");
    }
}
