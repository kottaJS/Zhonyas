using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LoLFirewallManager.Services
{
    public static class FirewallService
    {
        private static readonly string[] RuleNames = new string[]
        {
            "Zhonyas_UDP_5000_Out",
            "Zhonyas_UDP_7000_Out",
            "Zhonyas_UDP_5000_In",
            "Zhonyas_UDP_7000_In",
            "Zhonyas_Game_Out",
            "Zhonyas_Client_Out",
            "Zhonyas_Block_Game_Out",
            "Zhonyas_Block_Game_In",
            "Zhonyas_Block_Client_Out",
            "Zhonyas_Block_Client_In",
            "LoL_AutoReconnect_Block"
        };

        public static bool AddBlockRules(string gameExePath, out string message)
        {
            DeleteRules();

            try { gameExePath = Path.GetFullPath(gameExePath).Replace('/', '\\').Trim(); } catch { }

            bool allOk = true;
            StringBuilder sbErrors = new StringBuilder();

            string cmdUdp5kOut = "advfirewall firewall add rule name=\"Zhonyas_UDP_5000_Out\" dir=out action=block protocol=UDP remoteport=5000-5500 enable=yes profile=any";
            string cmdUdp7kOut = "advfirewall firewall add rule name=\"Zhonyas_UDP_7000_Out\" dir=out action=block protocol=UDP remoteport=7000-8000 enable=yes profile=any";
            string cmdUdp5kIn = "advfirewall firewall add rule name=\"Zhonyas_UDP_5000_In\" dir=in action=block protocol=UDP localport=5000-5500 enable=yes profile=any";
            string cmdUdp7kIn = "advfirewall firewall add rule name=\"Zhonyas_UDP_7000_In\" dir=in action=block protocol=UDP localport=7000-8000 enable=yes profile=any";

            string o, e;
            if (RunNetsh(cmdUdp5kOut, out o, out e) != 0) { allOk = false; sbErrors.Append(e).Append(" "); }
            if (RunNetsh(cmdUdp7kOut, out o, out e) != 0) { allOk = false; sbErrors.Append(e).Append(" "); }
            if (RunNetsh(cmdUdp5kIn, out o, out e) != 0) { allOk = false; sbErrors.Append(e).Append(" "); }
            if (RunNetsh(cmdUdp7kIn, out o, out e) != 0) { allOk = false; sbErrors.Append(e).Append(" "); }

            if (File.Exists(gameExePath))
            {
                string cmdGame = string.Format("advfirewall firewall add rule name=\"Zhonyas_Game_Out\" dir=out action=block program=\"{0}\" enable=yes profile=any", gameExePath);
                RunNetsh(cmdGame, out o, out e);
            }

            if (allOk)
            {
                message = "Block applied.";
                return true;
            }
            else
            {
                message = sbErrors.ToString().Trim();
                return false;
            }
        }

        public static void DeleteRules()
        {
            for (int i = 0; i < RuleNames.Length; i++)
            {
                string o, e;
                RunNetsh(string.Format("advfirewall firewall delete rule name=\"{0}\"", RuleNames[i]), out o, out e);
            }
        }

        public static bool RuleExists()
        {
            string stdout, stderr;
            int code = RunNetsh("advfirewall firewall show rule name=\"Zhonyas_UDP_5000_Out\"", out stdout, out stderr);
            return (code == 0 && !string.IsNullOrEmpty(stdout) && stdout.Contains("Zhonyas_UDP_5000_Out"));
        }

        private static int RunNetsh(string arguments, out string standardOutput, out string standardError)
        {
            try
            {
                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "netsh.exe",
                    Arguments = arguments,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                };

                using (Process p = Process.Start(psi))
                {
                    standardOutput = p.StandardOutput.ReadToEnd();
                    standardError = p.StandardError.ReadToEnd();
                    p.WaitForExit(5000);
                    return p.ExitCode;
                }
            }
            catch (Exception ex)
            {
                standardOutput = string.Empty;
                standardError = ex.Message;
                return -1;
            }
        }
    }
}
