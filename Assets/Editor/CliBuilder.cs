using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace EVOGAMI.Editor
{
    public static class CliBuilder
    {
        private static string BuildRootPath => Path.GetFullPath("../Build", Application.dataPath);

        private static void Build(BuildTarget buildTarget, string extension)
        {
            if (!EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, buildTarget))
                throw new Exception($"Failed to switch build target to Standalone {buildTarget}");

#if UNITY_EDITOR_OSX || UNITY_EDITOR_LINUX
           if (buildTarget == BuildTarget.StandaloneOSX)
            {
                UnityEditor.OSXStandalone.UserBuildSettings.architecture = OSArchitecture.x64ARM64;
            }
#endif

            var scenesIncluded =
                (from scene in EditorBuildSettings.scenes where scene.enabled select scene.path).ToArray();

            if (scenesIncluded.Length == 0) throw new Exception("No scenes included in build settings");

            Debug.Log($"Scenes included:\n{string.Join("\n", scenesIncluded)}");

            var buildDir = Path.GetFullPath(Application.productName, BuildRootPath);
            if (Directory.Exists(buildDir)) Directory.Delete(buildDir, true);

            var buildPath = Path.GetFullPath($"{Application.productName}{extension}", buildDir);


            Debug.Log($"Build path: {buildPath}");

            var options = new BuildPlayerOptions
            {
                scenes = scenesIncluded,
                locationPathName = buildPath,
                options = BuildOptions.CleanBuildCache,
                targetGroup = BuildTargetGroup.Standalone,
                target = buildTarget
            };

            var report = BuildPipeline.BuildPlayer(options);

            if (report.summary.result != BuildResult.Succeeded) throw new Exception("Build failed");

            var gameDir = Path.GetDirectoryName(report.summary.outputPath);
            Debug.Log($"Build successful - Build {report.summary.totalSize} bytes written to {gameDir}");

            var files = Directory.GetFiles(gameDir, "*", SearchOption.AllDirectories);
            Debug.Log($"Files in the build dir:\n{string.Join("\n", files)}");

            ZipIt(gameDir, buildTarget is not (BuildTarget.StandaloneWindows or BuildTarget.StandaloneWindows64));
            Debug.Log("Done.");
        }

        private static void ZipIt(string gameDir, bool useGzip)
        {
            var zipPath = Path.GetFullPath(GetGameZipName(useGzip ? ".tar.gz" : ".zip"), BuildRootPath);
            Debug.Log($"Zipping {gameDir} to {zipPath} ...");
            if (File.Exists(zipPath))
            {
                Debug.Log("Deleting existing zip file.");
                File.Delete(zipPath);
            }

            if (useGzip)
                Utils.CreateTarGZ(zipPath, gameDir);
            else
                ZipFile.CreateFromDirectory(gameDir, zipPath);
        }

        private static string GetGameZipName(string extension)
        {
            var version = PlayerSettings.bundleVersion;
            if (!string.IsNullOrEmpty(GameVersionBuildProcessor.InformativeVersionOfLastBuild))
                version = GameVersionBuildProcessor.InformativeVersionOfLastBuild;
            var baseName = Application.productName.Replace(" ", "");
            var platform = GetBuildTargetShortString();
            var zipName = $"{baseName}-{version}-{platform}{extension}";
            return zipName;
        }

        private static string GetBuildTargetShortString()
        {
            return EditorUserBuildSettings.activeBuildTarget switch
            {
                BuildTarget.StandaloneWindows64 => "win64",
                BuildTarget.StandaloneLinux64 => "linux64",
                BuildTarget.StandaloneOSX => "osx",
                _ => EditorUserBuildSettings.activeBuildTarget.ToString()
            };
        }


        [MenuItem("Tools/Quick Build/Build StandaloneLinux64")]
        public static void BuildLinux()
        {
            Build(BuildTarget.StandaloneLinux64, ".x86_64");
        }

        [MenuItem("Tools/Quick Build/Build StandaloneWindows64")]
        public static void BuildWindows()
        {
            Build(BuildTarget.StandaloneWindows64, ".exe");
        }

        [MenuItem("Tools/Quick Build/Build StandaloneMacOS (Universal)")]
        public static void BuildMacOS()
        {
            Build(BuildTarget.StandaloneOSX, ".app");
        }
    }
}