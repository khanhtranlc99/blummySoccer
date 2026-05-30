using System.IO;
using UnityEditor;

namespace XGame
{
    [InitializeOnLoad]
    public class ToolDownLoaderOnLoaded
    {
        static ToolDownLoaderOnLoaded()
        {
            if (!File.Exists("Assets/XGameUnityTool/package.json"))
            {
                ToolDownLoaderWindow.Open();
            }
        }
    }
}