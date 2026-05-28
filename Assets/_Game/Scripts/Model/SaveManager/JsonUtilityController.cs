using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonUtilityController
{
    public static void SaveData(object data, string fileName)
    {
        string json = JsonUtility.ToJson(data);

        //Kích thước văn bản nhỏ
        File.WriteAllText(GetFilePath(fileName), json);

        //Kích thước văn bản lớn như ảnh, âm thanh...
        //FileStream fileStream = new FileStream(GetFilePath(fileName), FileMode.Create);
        //using (StreamWriter write = new StreamWriter(fileStream))
        //{
        //    write.Write(data);
        //}
    }
    public static T LoadData<T>(string fileName)
    {
        string filePath = GetFilePath(fileName);
        T data = default(T);
        string jsonData = ReadFromFileSmallData(filePath);
        data = JsonUtility.FromJson<T>(jsonData);
        return data;
    }
    public static string ReadFromFileSmallData(string fileName)
    {
        string filePath = GetFilePath(fileName);
        if (File.Exists(filePath))
            return File.ReadAllText(filePath);
        return "";
    }
    public static string ReadFromFileBigData(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
                return reader.ReadToEnd();
        }
        else
            Debug.LogWarning("File not found!");
        return "";
    }
    public static bool IsExist(string fileName) => File.Exists(GetFilePath(fileName)) ? true : false;
    public static string GetFilePath(string fileName) => Application.persistentDataPath + "/" + fileName;
}
