using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class OnBeforeBuildAndroid : IPreprocessBuildWithReport
{
    public void OnPreprocessBuild(BuildReport report)
    {
    }


    public int callbackOrder { get; }
}