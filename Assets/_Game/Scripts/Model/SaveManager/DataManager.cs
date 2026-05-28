// using System.IO;

// public static class DataManager
// {
//     public static void Save<T>(T data, string fileName) where T : class
//     {
//         //NewtonsoftController.Save(data, fileName);
//         OdinSerializerController.Save(data, fileName);
//     }

//     public static T Load<T>(string fileName) where T : class
//     {
//         //return NewtonsoftController.Load<T>(fileName);
//         return OdinSerializerController.Load<T>(fileName);
//     }
//     public static bool IsExistData(string fileName)
//     {
//         return OdinSerializerController.IsExist(fileName);
//     }
//     public static void DeleteData(string fileName)
//     {
//         if (IsExistData(fileName))
//         {
//             File.Delete(OdinSerializerController.GetFilePath(fileName));
//         }
//     }
//     public static byte[] GetSerializationData<T>(T obj)
//     {
//         //string json = NewtonsoftController.SerializedObject(obj);
//         //return Encoding.UTF8.GetBytes(json);
//         return OdinSerializerController.SerializationObject(obj);
//     }
//     public static T GetDeserializationObject<T>(byte[] data)
//     {
//         //string jsonString = System.Text.Encoding.UTF8.GetString(data);
//         //return NewtonsoftController.DeserializeObject<T>(jsonString);
//         return OdinSerializerController.GetDeserializationObject<T>(data);
//     }
// }
