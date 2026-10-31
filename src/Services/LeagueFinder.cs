using System;
using System.Diagnostics;
using System.IO;

namespace LoLFirewallManager.Services
{
    public static class LeagueFinder
    {
        public static string FindGameExePath()
        {
            string baseDir = FindLeagueBaseDir();
            string exe = Path.Combine(baseDir, @"Game\League of Legends.exe");
            return Path.GetFullPath(exe).Replace('/', '\\');
        }

        public static string FindClientExePath()
        {
            string baseDir = FindLeagueBaseDir();
            string exe = Path.Combine(baseDir, @"LeagueClient.exe");
            return Path.GetFullPath(exe).Replace('/', '\\');
        }

        public static string FindLeagueBaseDir()
        {
            try
            {
                string metaPath = @"C:\ProgramData\Riot Games\Metadata\league_of_legends.live\league_of_legends.live.product_settings.yaml";
                if (File.Exists(metaPath))
                {
                    string[] lines = File.ReadAllLines(metaPath);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        string line = lines[i];
                        if (line.Contains("product_install_full_path:"))
                        {
                            int firstQuote = line.IndexOf('"');
                            int lastQuote = line.LastIndexOf('"');
                            if (firstQuote != -1 && lastQuote > firstQuote)
                            {
                                string dir = line.Substring(firstQuote + 1, lastQuote - firstQuote - 1).Trim().Replace('/', '\\');
                                if (Directory.Exists(dir))
                                    return dir;
                            }
                        }
                    }
                }
            }
            catch { }

            try
            {
                Process[] clientProcs = Process.GetProcessesByName("LeagueClient");
                if (clientProcs.Length > 0)
                {
                    string clientExe = clientProcs[0].MainModule.FileName;
                    string dir = Path.GetDirectoryName(clientExe).Replace('/', '\\');
                    if (Directory.Exists(dir)) return dir;
                }
            }
            catch { }

            try
            {
                DriveInfo[] drives = DriveInfo.GetDrives();
                for (int i = 0; i < drives.Length; i++)
                {
                    DriveInfo d = drives[i];
                    if (d.IsReady && d.DriveType == DriveType.Fixed)
                    {
                        string candidate = Path.Combine(d.RootDirectory.FullName, @"Riot Games\League of Legends");
                        if (Directory.Exists(candidate))
                            return candidate.Replace('/', '\\');
                    }
                }
            }
            catch { }

            return @"C:\Riot Games\League of Legends";
        }
    }
}
