// using System;
// using System.Collections;
// using System.Collections.Generic;
// using System.Reflection;
// using UnityEngine;

// [Serializable]
// public class UserData
// {
//     public UserPropertyData UserPropertyData = new();
//     public UserIdentificationData UserIdentificationData = new();

//     public void LoadData()
//     {
//         Type baseType = typeof(BaseUserData);
//         foreach (var field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
//         {
//             if (baseType.IsAssignableFrom(field.FieldType) ||
//                 (field.FieldType.IsGenericType && baseType.IsAssignableFrom(field.FieldType.GetGenericArguments()[0])))
//             {
//                 var value = field.GetValue(this);
//                 if (value != null)
//                 {
//                     MethodInfo loadDataMethod = value.GetType().GetMethod("LoadData");
//                     if (loadDataMethod != null)
//                     {
//                         loadDataMethod.Invoke(value, null);
//                     }
//                 }
//             }
//         }
//     }
//     void SaveAllData()
//     {
//         Type baseType = typeof(BaseUserData);
//         foreach (var field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
//         {
//             if (baseType.IsAssignableFrom(field.FieldType) ||
//                 (field.FieldType.IsGenericType && baseType.IsAssignableFrom(field.FieldType.GetGenericArguments()[0])))
//             {
//                 var value = field.GetValue(this);
//                 if (value != null)
//                 {
//                     MethodInfo deleteMethod = value.GetType().GetMethod("SaveData");
//                     if (deleteMethod != null)
//                     {
//                         deleteMethod.Invoke(value, null);
//                     }
//                     var newInstance = Activator.CreateInstance(field.FieldType);
//                     field.SetValue(this, newInstance);
//                 }
//             }
//         }
//     }
//     public void DeleteData()
//     {
//         Type baseType = typeof(BaseUserData);
//         foreach (var field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
//         {
//             if (baseType.IsAssignableFrom(field.FieldType) ||
//                 (field.FieldType.IsGenericType && baseType.IsAssignableFrom(field.FieldType.GetGenericArguments()[0])))
//             {
//                 var value = field.GetValue(this);
//                 if (value != null)
//                 {
//                     MethodInfo deleteMethod = value.GetType().GetMethod("DeleteData");
//                     if (deleteMethod != null)
//                     {
//                         deleteMethod.Invoke(value, null);
//                     }
//                     var newInstance = Activator.CreateInstance(field.FieldType);
//                     field.SetValue(this, newInstance);
//                 }
//             }
//         }
//     }
// }
