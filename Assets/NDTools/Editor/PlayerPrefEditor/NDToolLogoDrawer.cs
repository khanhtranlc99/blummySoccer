using DG.DemiEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace ND.Tools
{
    public static class NDToolLogoDrawer
    {
        static string iconPath = "Assets/NDTools/ico.png";
        static Texture ico;
        public static void LoadResource()
        {
            Sprite spr = AssetDatabase.LoadAssetAtPath<Sprite>(iconPath);
            ico = spr.texture;
        }
        public static void DrawLogo()
        {
            GUILayout.BeginHorizontal();
            GUI.DrawTexture(new Rect(0, 5, 30, 30), ico);
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.fontSize = 20;
            style.fontStyle = FontStyle.Bold;
            style.richText = true;
            style.MarginLeft(30);
            style.MarginTop(7);
            GUILayout.Label("ND Utility Panel </>", style);
            GUILayout.EndHorizontal();
        }
    }
}

