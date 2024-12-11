using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Builds.Preprocessors
{
    public class SetKeystore : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string _keystoreName = "dorfer.keystore";
        private const string _keystoreAlias = "release";
        private const string _keystorePassword = "4afW7SwZjsN&kKtXe7KwS%Ye";
        private const string _keystoreAliasPassword = "4afW7SwZjsN&kKtXe7KwS%Ye";

        public void OnPreprocessBuild(BuildReport report)
        {
            if (report.summary.platform == BuildTarget.Android)
            {
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystoreName = _keystoreName;
                PlayerSettings.Android.keyaliasName = _keystoreAlias;
                PlayerSettings.Android.keystorePass = _keystorePassword;
                PlayerSettings.Android.keyaliasPass = _keystoreAliasPassword;
            }
        }
    }
}
