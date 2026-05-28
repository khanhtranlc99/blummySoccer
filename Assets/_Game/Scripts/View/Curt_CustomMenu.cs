using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Diagnostics;
// #if UNITY_EDITOR
// using Sirenix.OdinInspector.Editor;
// using Sirenix.OdinInspector;
// using Sirenix.Utilities.Editor;
// using Sirenix.Utilities;
// #endif

namespace Curt
{
#if UNITY_EDITOR
    public class Curt_CustomMenu
    {
        [MenuItem("Tools/Open Save Folder", priority = 40)]
        private static void OpenPersistentFolder()
        {
            //EditorUtility.RevealInFinder(Application.persistentDataPath);
            Process process = new Process();
            process.StartInfo.FileName = ((Application.platform == RuntimePlatform.WindowsEditor) ? "explorer.exe" : "open");
            process.StartInfo.Arguments = "file://" + Application.persistentDataPath;
            process.Start();
        }
    }
#endif
}

