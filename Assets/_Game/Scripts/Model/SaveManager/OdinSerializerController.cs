// // using Sirenix.Serialization;
// using System.Collections;
// using System.Collections.Generic;
// using System.IO;
// using UnityEngine;

// public static class OdinSerializerController
// {
// //     public static void Save<T>(T data, string fileName) where T : class
// //     {
// //         DataFormat formatType = DataFormat.Binary;
// // #if UNITY_EDITOR
// //         formatType = DataFormat.JSON;
// // #endif
// //         byte[] bytes = SerializationUtility.SerializeValue(data,formatType);
// //         File.WriteAllBytes(GetFilePath(fileName), bytes);
// //     }
//     public static void Save(byte[] data, string fileName)
//     {
//         File.WriteAllBytes(GetFilePath(fileName), data);
//     }
//     public static T Load<T>(string fileName) where T : class
//     {
//         byte[] bytes = File.ReadAllBytes(GetFilePath(fileName));
//         DataFormat formatType = DataFormat.Binary;
// #if UNITY_EDITOR
//         formatType = DataFormat.JSON;
// #endif
//         return SerializationUtility.DeserializeValue<T>(bytes, formatType);
//     }
//     public static T GetDeserializationObject<T>(byte[] data)
//     {
//         DataFormat formatType = DataFormat.Binary;
// #if UNITY_EDITOR
//         formatType = DataFormat.JSON;
// #endif
//         return SerializationUtility.DeserializeValue<T>(data, formatType);
//     }

//     public static byte[] SerializationObject<T>(T data)
//     {
//         DataFormat formatType = DataFormat.Binary;
// #if UNITY_EDITOR
//         formatType = DataFormat.JSON;
// #endif
//         byte[] bytes = SerializationUtility.SerializeValue(data, formatType);
//         return bytes;
//     }
//     public static bool IsExist(string fileName) => File.Exists(GetFilePath(fileName)) ? true : false;
//     public static string GetFilePath(string fileName) => Application.persistentDataPath + "/" + fileName;
// }
