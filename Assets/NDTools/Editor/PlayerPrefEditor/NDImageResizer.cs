// using Sirenix.Utilities.Editor;
using System;
using System.IO;
using UnityEditor;
using UnityEngine;
namespace ND.Tools
{
    public class NDImageResizer : NDToolOption
    {
        Texture2D selectedTexture = null;
        int w, h;
        protected override string GetOptionName()
        {
            return "Image Resizer";
        }
        public override void OnGUI()
        {
            selectedTexture = EditorGUILayout.ObjectField("Drop image here ", selectedTexture, typeof(Texture2D), false) as Texture2D;
            if (selectedTexture != null)
            {
                GUILayout.Label($"{selectedTexture.width} x {selectedTexture.height}");
                EditorGUILayout.BeginHorizontal();
                GUILayout.Label("Width");
                w = EditorGUILayout.IntField(w);
                GUILayout.Label("Height");
                h = EditorGUILayout.IntField(h);
                EditorGUILayout.EndHorizontal();

                if (GUILayout.Button("Resize"))
                {
                    Texture2D newTex = Resize(selectedTexture, w, h);
                    SaveTexture(newTex);
                }

                if (GUILayout.Button($"Resize to {selectedTexture.width - (selectedTexture.width % 4)} x {selectedTexture.height - (selectedTexture.height % 4)}"))
                {
                    Texture2D newTex = Resize(selectedTexture, selectedTexture.width - (selectedTexture.width % 4), selectedTexture.height - (selectedTexture.height % 4));
                    SaveTexture(newTex);
                }

                int potW = NextPowerOfTwo(selectedTexture.width);
                int potH = NextPowerOfTwo(selectedTexture.height);
                if (GUILayout.Button($"Resize to POT texture {potW} x {potH}"))
                {
                    Texture2D newTex = Resize(selectedTexture, potW, potH);
                    SaveTexture(newTex);
                }
            }

            folderPath = GUILayout.TextField(folderPath);
            if (GUILayout.Button("Batch Resize in _Textures Folder"))
            {
                BatchResizeTextures();
            }
        }

        Texture2D Resize(Texture2D texture2D, int targetX, int targetY)
        {
            RenderTexture rt = new RenderTexture(targetX, targetY, 24);
            RenderTexture.active = rt;
            Graphics.Blit(texture2D, rt);
            Texture2D result = new Texture2D(targetX, targetY);
            result.alphaIsTransparency = texture2D.alphaIsTransparency;
            result.ReadPixels(new Rect(0, 0, targetX, targetY), 0, 0);
            result.Apply();
            return result;
        }

        void SaveTexture(Texture2D texture, string savePath = null)
        {
            string texPath = savePath ?? AssetDatabase.GetAssetPath(selectedTexture);
            string appPath = Directory.GetCurrentDirectory();
            savePath = Path.Combine(appPath, texPath);
            savePath = savePath.Replace("/", "\\");
            byte[] bytes = texture.EncodeToPNG();
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
            File.WriteAllBytes(savePath, bytes);
            AssetDatabase.Refresh();
        }

        int NextPowerOfTwo(int value)
        {
            if (value <= 0) return 1;
            int pot = 1;
            while (pot < value)
            {
                pot <<= 1;
            }
            return pot;
        }
        string folderPath = "Assets/_GAME";

        void BatchResizeTextures()
        {
            string[] texturePaths = Directory.GetFiles(folderPath, "*.png", SearchOption.AllDirectories);

            foreach (string texturePath in texturePaths)
            {
                Texture2D texture = AssetDatabase.LoadAssetAtPath<Texture2D>(texturePath);
                if (texture != null)
                {
                    Texture2D resizedTexture = Resize(texture, texture.width - (texture.width % 4), texture.height - (texture.height % 4)); ;
                    SaveTexture(resizedTexture, texturePath);
                    Debug.Log($"Resized: {texturePath}");
                }
            }

            AssetDatabase.Refresh();
        }
    }
}
