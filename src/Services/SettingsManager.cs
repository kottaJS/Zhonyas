using System;
using System.IO;
using System.Text;

namespace LoLFirewallManager.Services
{
    public class AppSettings
    {
        public bool SuppressStopWarning { get; set; }
        public bool EnableGif { get; set; }
        public bool Sounds { get; set; }
        public bool AutoRearm { get; set; }
        public int DurationSeconds { get; set; }
        public string Language { get; set; }

        public AppSettings()
        {
            SuppressStopWarning = false;
            EnableGif = true;
            Sounds = true;
            AutoRearm = true;
            DurationSeconds = 210;
            Language = "en";
        }
    }

    public static class SettingsManager
    {
        private static readonly string SettingsFile = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Zhonyas", "settings.ini");

        public static AppSettings Load()
        {
            AppSettings s = new AppSettings();
            try
            {
                if (File.Exists(SettingsFile))
                {
                    string[] lines = File.ReadAllLines(SettingsFile);
                    foreach (string line in lines)
                    {
                        if (line.StartsWith("SuppressStopWarning="))
                            s.SuppressStopWarning = (line.Substring("SuppressStopWarning=".Length).Trim() == "1");
                        else if (line.StartsWith("EnableGif="))
                            s.EnableGif = (line.Substring("EnableGif=".Length).Trim() == "1");
                        else if (line.StartsWith("Sounds="))
                            s.Sounds = (line.Substring("Sounds=".Length).Trim() == "1");
                        else if (line.StartsWith("AutoRearm="))
                            s.AutoRearm = (line.Substring("AutoRearm=".Length).Trim() == "1");
                        else if (line.StartsWith("DurationSeconds="))
                        {
                            int sec;
                            if (int.TryParse(line.Substring("DurationSeconds=".Length).Trim(), out sec) && sec > 0)
                                s.DurationSeconds = sec;
                        }
                        else if (line.StartsWith("Language="))
                        {
                            s.Language = line.Substring("Language=".Length).Trim();
                        }
                    }
                }
            }
            catch { }
            return s;
        }

        public static void Save(AppSettings s)
        {
            try
            {
                string dir = Path.GetDirectoryName(SettingsFile);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SuppressStopWarning=" + (s.SuppressStopWarning ? "1" : "0"));
                sb.AppendLine("EnableGif=" + (s.EnableGif ? "1" : "0"));
                sb.AppendLine("Sounds=" + (s.Sounds ? "1" : "0"));
                sb.AppendLine("AutoRearm=" + (s.AutoRearm ? "1" : "0"));
                sb.AppendLine("DurationSeconds=" + s.DurationSeconds);
                sb.AppendLine("Language=" + (s.Language ?? "en"));
                File.WriteAllText(SettingsFile, sb.ToString());
            }
            catch { }
        }
    }
}

