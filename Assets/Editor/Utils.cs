using System;
using System.Diagnostics;
using System.IO;
using Unity.SharpZipLib.GZip;
using Unity.SharpZipLib.Tar;
using Debug = UnityEngine.Debug;

namespace EVOGAMI.Editor
{
    public static class Utils
    {
        public static string GetCurrentCommitHash()
        {
            try
            {
                Debug.Log("[GetCurrentCommitHash] Getting commit hash...");
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "git",
                        Arguments = "rev-parse --short HEAD",
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        RedirectStandardOutput = true
                    }
                };
                process.Start();
                var commitHash = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();
                Debug.Log("[GetCurrentCommitHash] Current commit hash: " + commitHash);
                return commitHash;
            }
            catch (Exception e)
            {
                Debug.LogWarning("[GetCurrentCommitHash] Can't get current commit hash: " + e.Message);
                Debug.LogException(e);
                return "";
            }
        }

        public static void CreateTarGZ(string tgzFilename, string fileName)
        {
            Debug.Log($"[CreateTarGZ] Creating {tgzFilename} from {fileName}");
            using var outStream = File.Create(tgzFilename);
            using var gzoStream = new GZipOutputStream(outStream);
            using var tarArchive = TarArchive.CreateOutputTarArchive(gzoStream);

            tarArchive.RootPath = Directory.Exists(fileName) ? fileName : Path.GetDirectoryName(fileName);

            var tarEntry = TarEntry.CreateEntryFromFile(fileName);
            // tarEntry.Name = Path.GetFileName(fileName);

            tarArchive.WriteEntry(tarEntry, true);
        }
    }
}