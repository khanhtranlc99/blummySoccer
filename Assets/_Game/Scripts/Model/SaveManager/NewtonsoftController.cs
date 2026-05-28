using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Text;
using System;
using UnityEditor;

public static class NewtonsoftController
{
    public static void Save(object data, string fileName)
    {
        string json = SerializedObject(data);
        WriteToFile(fileName, json);
    }

    public static T Load<T>(string fileName)
    {
        string json = ReadFromFile(fileName);
        T Generic = DeserializeObject<T>(json);
        return Generic;
    }
    public static void WriteToFile(string fileName, string data)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter write = new StreamWriter(fileStream))
        {
            write.Write(data);
        }
    }
    public static string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found!");
        }
        return "";
    }
    public static bool IsExist(string fileName) => File.Exists(GetFilePath(fileName)) ? true : false;
    public static string GetFilePath(string fileName) => Application.persistentDataPath + "/" + fileName;
    public static string SerializedObject<T>(T Object)
    {
#if UNITY_EDITOR
        Newtonsoft.Json.Formatting format = Newtonsoft.Json.Formatting.Indented;
#else
        Newtonsoft.Json.Formatting format = Newtonsoft.Json.Formatting.None;
#endif
        return JsonConvert.SerializeObject(Object, format);
    }
    public static T DeserializeObject<T>(string _data){
        var data = JsonConvert.DeserializeObject<T>(_data);
        return data;
    }

    #region String Builder
    public static string jsonEncode(object json)
    {
        StringBuilder stringBuilder = new StringBuilder(2000);
        return (!serializeValue(json, stringBuilder)) ? null : stringBuilder.ToString();
    }
    private static bool serializeValue(object value, StringBuilder builder)
    {
        if (value == null)
        {
            builder.Append("null");
        }
        else if (value.GetType().IsArray)
        {
            serializeArray(new ArrayList((ICollection)value), builder);
        }
        else if (value is string)
        {
            serializeString((string)value, builder);
        }
        else if (value is char)
        {
            serializeString(Convert.ToString((char)value), builder);
        }
        else if (value is Hashtable)
        {
            serializeObject((Hashtable)value, builder);
        }
        else if (value is Dictionary<string, string>)
        {
            serializeDictionary((Dictionary<string, string>)value, builder);
        }
        else if (value is ArrayList)
        {
            serializeArray((ArrayList)value, builder);
        }
        else if (value is bool && (bool)value)
        {
            builder.Append("true");
        }
        else if (value is bool && !(bool)value)
        {
            builder.Append("false");
        }
        else
        {
            if (!value.GetType().IsPrimitive)
            {
                return false;
            }
            serializeNumber(Convert.ToDouble(value), builder);
        }
        return true;
    }
    private static bool serializeObjectOrArray(object objectOrArray, StringBuilder builder)
    {
        if (objectOrArray is Hashtable)
        {
            return serializeObject((Hashtable)objectOrArray, builder);
        }
        if (objectOrArray is ArrayList)
        {
            return serializeArray((ArrayList)objectOrArray, builder);
        }
        return false;
    }
    private static bool serializeObject(Hashtable anObject, StringBuilder builder)
    {
        builder.Append("{");
        IDictionaryEnumerator enumerator = anObject.GetEnumerator();
        bool flag = true;
        while (enumerator.MoveNext())
        {
            string aString = enumerator.Key.ToString();
            object value = enumerator.Value;
            if (!flag)
            {
                builder.Append(", ");
            }
            serializeString(aString, builder);
            builder.Append(":");
            if (!serializeValue(value, builder))
            {
                return false;
            }
            flag = false;
        }
        builder.Append("}");
        return true;
    }

    private static bool serializeDictionary(Dictionary<string, string> dict, StringBuilder builder)
    {
        builder.Append("{");
        bool flag = true;
        foreach (KeyValuePair<string, string> item in dict)
        {
            if (!flag)
            {
                builder.Append(", ");
            }
            serializeString(item.Key, builder);
            builder.Append(":");
            serializeString(item.Value, builder);
            flag = false;
        }
        builder.Append("}");
        return true;
    }

    private static bool serializeArray(ArrayList anArray, StringBuilder builder)
    {
        builder.Append("[");
        bool flag = true;
        for (int i = 0; i < anArray.Count; i++)
        {
            object value = anArray[i];
            if (!flag)
            {
                builder.Append(", ");
            }
            if (!serializeValue(value, builder))
            {
                return false;
            }
            flag = false;
        }
        builder.Append("]");
        return true;
    }
    private static void serializeString(string aString, StringBuilder builder)
    {
        builder.Append("\"");
        char[] array = aString.ToCharArray();
        foreach (char c in array)
        {
            switch (c)
            {
                case '"':
                    builder.Append("\\\"");
                    continue;
                case '\\':
                    builder.Append("\\\\");
                    continue;
                case '\b':
                    builder.Append("\\b");
                    continue;
                case '\f':
                    builder.Append("\\f");
                    continue;
                case '\n':
                    builder.Append("\\n");
                    continue;
                case '\r':
                    builder.Append("\\r");
                    continue;
                case '\t':
                    builder.Append("\\t");
                    continue;
            }
            int num = Convert.ToInt32(c);
            if (num >= 32 && num <= 126)
            {
                builder.Append(c);
            }
            else
            {
                builder.Append("\\u" + Convert.ToString(num, 16).PadLeft(4, '0'));
            }
        }
        builder.Append("\"");
    }

    private static void serializeNumber(double number, StringBuilder builder)
    {
        builder.Append(Convert.ToString(number));
    }
    #endregion
}
