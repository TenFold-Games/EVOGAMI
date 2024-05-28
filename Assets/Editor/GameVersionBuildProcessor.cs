using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Debug = UnityEngine.Debug;

namespace EVOGAMI.Editor
{
    public class GameVersionBuildProcessor : IPreprocessBuildWithReport, IPostprocessBuildWithReport
    {
        public static string InformativeVersionOfLastBuild { get; private set; } = "";

        public void OnPostprocessBuild(BuildReport report)
        {
            InformativeVersionOfLastBuild = PlayerSettings.bundleVersion;
            var currentVersion = SemanticVersion.Parse(PlayerSettings.bundleVersion);
            var sanitized = new SemanticVersion(currentVersion.major, currentVersion.minor, currentVersion.patch, "", 0)
                .ToString();
            Debug.Log("Setting version to " + sanitized);
            PlayerSettings.bundleVersion =
                new SemanticVersion(currentVersion.major, currentVersion.minor, currentVersion.patch, "", 0).ToString();
            AssetDatabase.SaveAssets();
        }

        public int callbackOrder => 99;

        public void OnPreprocessBuild(BuildReport report)
        {
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            InformativeVersionOfLastBuild = "";

            try
            {
                var commitHash = Utils.GetCurrentCommitHash();

                var currentVersion = SemanticVersion.Parse(PlayerSettings.bundleVersion);
                // sanitize the current version
                var sanitized =
                    new SemanticVersion(currentVersion.major, currentVersion.minor, currentVersion.patch, "", 0)
                        .ToString();
                if (!string.IsNullOrEmpty(commitHash)) sanitized += "-" + commitHash;

                if (report.summary.options.HasFlag(BuildOptions.AllowDebugging)) sanitized += "-debug";

                if (report.summary.options.HasFlag(BuildOptions.Development)) sanitized += "-dev";

                Debug.Log("Setting version to " + sanitized);
                PlayerSettings.bundleVersion = sanitized;
            }
            catch (ArgumentException)
            {
                Debug.LogError("Can't parse version string into Unity's SemanticVersion");
                throw;
            }
        }
    }
}