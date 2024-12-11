using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditorInternal;

namespace Builds.Preprocessors
{
    // https://answers.unity.com/questions/199334/editor-check-if-unity-pro-license-installed.html
    public class RemoveSplashScreen : IPreprocessBuildWithReport
    {
        public int callbackOrder => 1;

        public void OnPreprocessBuild(BuildReport report)
        {
            if (InternalEditorUtility.HasPro())
            {
                PlayerSettings.SplashScreen.showUnityLogo = false;
            }
        }
    }
}
