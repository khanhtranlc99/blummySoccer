using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
namespace ND.Tools
{
    public class NDPlayerPrefEditor : NDToolOption
    {
        public string intValue, floatValue, stringValue;
        int itemSelected = 0;
        int lastItemSelected = 0;
        string search = "";
        string lastSearch = "";
        string[] listSearchString;
        protected override string GetOptionName()
        {
            return "PlayerPref Editor";
        }
        protected override void OnUse()
        {
            string s = EditorPrefs.GetString("searchList", "");
            listSearchString = JsonConvert.DeserializeObject<string[]>(s);
            if (listSearchString != null)
            {
                search = listSearchString[0];
            }
        }
        public override void OnGUI()
        {
            if (listSearchString != null)
            {
                itemSelected = GUILayout.SelectionGrid(itemSelected, listSearchString, 6);
                if (itemSelected != lastItemSelected)
                {
                    lastItemSelected = itemSelected;
                    search = listSearchString[itemSelected];

                    intValue = GetSavedValue(ValueType.Int, search);
                    floatValue = GetSavedValue(ValueType.Float, search);
                    stringValue = GetSavedValue(ValueType.String, search);
                }
            }
            GUILayout.BeginHorizontal();
            search = GUILayout.TextField(search);
            if (lastSearch != search)
            {
                lastSearch = search;
                intValue = GetSavedValue(ValueType.Int, search);
                floatValue = GetSavedValue(ValueType.Float, search);
                stringValue = GetSavedValue(ValueType.String, search);
            }


            if (GUILayout.Button("Add key"))
            {
                List<string> list = new List<string>();
                if(listSearchString != null && listSearchString.Length > 0)
                list = new List<string>(listSearchString);
                if (search != "" && !list.Contains(search))
                {
                    list.Add(search);
                    string js = JsonConvert.SerializeObject(list);
                    EditorPrefs.SetString("searchList", js);
                }
                string s = EditorPrefs.GetString("searchList", "");
                listSearchString = JsonConvert.DeserializeObject<string[]>(s);

            }
            GUILayout.EndHorizontal();

            GUILayout.Space(10);
            GUILayout.BeginHorizontal();
            GUILayout.Label($"{search} (int)");
            intValue = GUILayout.TextField(intValue);
            if (GUILayout.Button("Save value"))
            {
                PlayerPrefs.SetInt(search, int.Parse(intValue));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"{search} (float)");
            floatValue = GUILayout.TextField(floatValue);
            if (GUILayout.Button("Save value"))
            {
                PlayerPrefs.SetFloat(search, float.Parse(floatValue));
            }
            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            GUILayout.Label($"{search} (string)");
            stringValue = GUILayout.TextField(stringValue);
            if (GUILayout.Button("Save value"))
            {
                PlayerPrefs.SetString(search, stringValue);
            }
            GUILayout.EndHorizontal();
        }
        string GetSavedValue(ValueType type, string input)
        {
            if (type == ValueType.String) return PlayerPrefs.GetString(input, "") == "" ? "null" : PlayerPrefs.GetString(input);
            if (type == ValueType.Float) return PlayerPrefs.GetFloat(input, float.MinValue) == float.MinValue ? "null" : PlayerPrefs.GetFloat(input).ToString();
            if (type == ValueType.Int) return PlayerPrefs.GetInt(input, int.MinValue) == int.MinValue ? "null" : PlayerPrefs.GetInt(input).ToString();
            return "null";
        }
        enum ValueType
        {
            String,
            Float,
            Int
        }
    }
}



