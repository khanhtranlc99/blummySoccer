using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class BinaryFormatterController
{
    public static void SaveData<T>(string fileName, T data)
    {
        string path = GetFilePath(fileName);
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();

        LogSystem.LogSuccess($"Saving {path} success!");
    }
    public static T ReadDataExist<T>(string fileName) where T : class
    {
            string path = GetFilePath(fileName);
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            var data = formatter.Deserialize(stream) as T;
            stream.Close();
            return data;
    }
    public static bool IsExist(string fileName) => File.Exists(GetFilePath(fileName)) ? true : false;
    public static string GetFilePath(string fileName) => Application.persistentDataPath + "/" + fileName;
}
