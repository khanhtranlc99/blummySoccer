// // using Sirenix.OdinInspector;

// public abstract class BaseUserData
// {
//     // Load data from file => if file not exist => call InitData => SaveData
//     public virtual void LoadData()
//     {
//         if (DataManager.IsExistData(this.GetType().Name))
//         {
//             var loadedData = DataManager.Load<BaseUserData>(this.GetType().Name);
//             if (loadedData != null)
//             {
//                 CopyData(loadedData);
//             }
//             else
//             {
//                 InitData();
//             }
//         }
//         else
//         {
//             InitData();
//         }
//         UpdateDataOnGameStart();
//         SaveData();
//     }
//     protected virtual void CopyData(BaseUserData loadedData)
//     {
//         foreach (var property in this.GetType().GetProperties())
//         {
//             if (property.CanWrite)
//             {
//                 var value = property.GetValue(loadedData);
//                 property.SetValue(this, value);
//             }
//         }

//         foreach (var field in this.GetType().GetFields())
//         {
//             var value = field.GetValue(loadedData);
//             field.SetValue(this, value);
//         }
//     }
//     // override method to setup data when file data not exist, dont forget call SaveData after setup
//     protected abstract void InitData();
//     // ham nay duoc goi bat cu khi nao vao game
//     protected abstract void UpdateDataOnGameStart();

//     private string GetName() => this.GetType().Name;

//     // [Button("Save data")]
//     public virtual void SaveData()
//     {
//         DataManager.Save(this, GetName());
//     }
//     // [Button("Delete data")]
//     public virtual void DeleteData()
//     {
//         DataManager.DeleteData(GetName());
//     }
// }
