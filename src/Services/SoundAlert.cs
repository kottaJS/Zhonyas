using System;
using System.IO;
using System.Reflection;
using System.Windows.Media;

namespace LoLFirewallManager.Services
{
    public static class SoundAlert
    {
        private static readonly string BaseDir = AppDomain.CurrentDomain.BaseDirectory;

        private static readonly string[] InCandidates = new string[]
        {
            "zhonyain.mp3",
            "zhonya_in.mp3",
            "in.mp3"
        };

        private static readonly string[] OutCandidates = new string[]
        {
            "zhonyaout.mp3",
            "zhonyaout2.mp3",
            "zhonya_out.mp3",
            "out.mp3"
        };

        private static readonly string[] WarningCandidates = new string[]
        {
            "countdown.mp3",
            "10seconds.mp3",
            "warning.mp3"
        };

        private static MediaPlayer _player;

        public static void PlayZhonyaIn()
        {
            PlayCandidate(InCandidates, "audio");
        }

        public static void PlayTenSecondsWarning()
        {
            PlayCandidate(WarningCandidates, "audio");
        }

        public static void PlayZhonyaOut()
        {
            PlayCandidate(OutCandidates, "audio");
        }

        public static void PlayCancel()
        {
            PlayZhonyaOut();
        }

        private static void PlayCandidate(string[] fileNames, string subFolder)
        {
            try
            {
                for (int i = 0; i < fileNames.Length; i++)
                {
                    string path = ResolvePath(fileNames[i], subFolder);
                    if (!string.IsNullOrEmpty(path) && File.Exists(path))
                    {
                        if (_player == null)
                        {
                            _player = new MediaPlayer();
                        }
                        _player.Open(new Uri(path));
                        _player.Play();
                        return;
                    }
                }
            }
            catch { }
        }

        private static string ResolvePath(string fileName, string subFolder)
        {
            string[] searchPaths = new string[]
            {
                Path.Combine(BaseDir, "assets", subFolder, fileName),
                Path.Combine(BaseDir, "assets", fileName),
                Path.Combine(BaseDir, fileName),
                Path.GetFullPath(Path.Combine("assets", subFolder, fileName)),
                Path.GetFullPath(Path.Combine("assets", fileName)),
                Path.GetFullPath(fileName)
            };

            foreach (string p in searchPaths)
            {
                if (File.Exists(p)) return p;
            }

            try
            {
                var asm = Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream(fileName))
                {
                    if (stream != null)
                    {
                        string tempDir = Path.Combine(Path.GetTempPath(), "Zhonyas");
                        if (!Directory.Exists(tempDir)) Directory.CreateDirectory(tempDir);
                        string tempFile = Path.Combine(tempDir, fileName);

                        if (!File.Exists(tempFile) || new FileInfo(tempFile).Length != stream.Length)
                        {
                            using (FileStream fs = new FileStream(tempFile, FileMode.Create, FileAccess.Write))
                            {
                                stream.CopyTo(fs);
                            }
                        }
                        return tempFile;
                    }
                }
            }
            catch { }

            return null;
        }
    }
}
