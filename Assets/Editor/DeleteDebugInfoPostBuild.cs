using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace EVOGAMI.Editor
{
    public class DeleteDebugInfoPostBuild : IPostprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPostprocessBuild(BuildReport report)
        {
            Debug.Log("[DeleteDebugInfoPostBuild.OnPostprocessBuild]");

            if (report.summary.options.HasFlag(BuildOptions.AllowDebugging))
            {
                Debug.Log("This is a debug build, skip checking debug info.");
                return;
            }

            var path = Path.GetFullPath(
                $"{Application.productName}_BurstDebugInformation_DoNotShip",
                Path.GetDirectoryName(report.summary.outputPath)
            );
            
            if (!Directory.Exists(path)) return;
            Debug.Log($"BurstDebugInfo exists! Deleting {path}");
            Directory.Delete(path, true);
        }
    }
}