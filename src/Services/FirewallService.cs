using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace LoLFirewallManager.Services
{
    public static class FirewallService
    {
        private const int NET_FW_IP_PROTOCOL_UDP = 17;
        private const int NET_FW_RULE_DIR_IN = 1;
        private const int NET_FW_RULE_DIR_OUT = 2;
        private const int NET_FW_ACTION_BLOCK = 0;
        private const int NET_FW_PROFILE2_ALL = 0x7FFFFFFF;

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

            try
            {
                Type policyType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                Type ruleType = Type.GetTypeFromProgID("HNetCfg.FWRule");

                if (policyType != null && ruleType != null)
                {
                    dynamic fwPolicy = Activator.CreateInstance(policyType);

                    // 1. UDP 5000-5500 Out
                    dynamic r1 = Activator.CreateInstance(ruleType);
                    r1.Name = "Zhonyas_UDP_5000_Out";
                    r1.Protocol = NET_FW_IP_PROTOCOL_UDP;
                    r1.RemotePorts = "5000-5500";
                    r1.Direction = NET_FW_RULE_DIR_OUT;
                    r1.Action = NET_FW_ACTION_BLOCK;
                    r1.Enabled = true;
                    r1.Profiles = NET_FW_PROFILE2_ALL;
                    fwPolicy.Rules.Add(r1);

                    // 2. UDP 7000-8000 Out
                    dynamic r2 = Activator.CreateInstance(ruleType);
                    r2.Name = "Zhonyas_UDP_7000_Out";
                    r2.Protocol = NET_FW_IP_PROTOCOL_UDP;
                    r2.RemotePorts = "7000-8000";
                    r2.Direction = NET_FW_RULE_DIR_OUT;
                    r2.Action = NET_FW_ACTION_BLOCK;
                    r2.Enabled = true;
                    r2.Profiles = NET_FW_PROFILE2_ALL;
                    fwPolicy.Rules.Add(r2);

                    // 3. UDP 5000-5500 In
                    dynamic r3 = Activator.CreateInstance(ruleType);
                    r3.Name = "Zhonyas_UDP_5000_In";
                    r3.Protocol = NET_FW_IP_PROTOCOL_UDP;
                    r3.LocalPorts = "5000-5500";
                    r3.Direction = NET_FW_RULE_DIR_IN;
                    r3.Action = NET_FW_ACTION_BLOCK;
                    r3.Enabled = true;
                    r3.Profiles = NET_FW_PROFILE2_ALL;
                    fwPolicy.Rules.Add(r3);

                    // 4. UDP 7000-8000 In
                    dynamic r4 = Activator.CreateInstance(ruleType);
                    r4.Name = "Zhonyas_UDP_7000_In";
                    r4.Protocol = NET_FW_IP_PROTOCOL_UDP;
                    r4.LocalPorts = "7000-8000";
                    r4.Direction = NET_FW_RULE_DIR_IN;
                    r4.Action = NET_FW_ACTION_BLOCK;
                    r4.Enabled = true;
                    r4.Profiles = NET_FW_PROFILE2_ALL;
                    fwPolicy.Rules.Add(r4);

                    // 5. Game exe block if file exists
                    if (!string.IsNullOrEmpty(gameExePath) && File.Exists(gameExePath))
                    {
                        dynamic rGame = Activator.CreateInstance(ruleType);
                        rGame.Name = "Zhonyas_Game_Out";
                        rGame.ApplicationName = gameExePath;
                        rGame.Direction = NET_FW_RULE_DIR_OUT;
                        rGame.Action = NET_FW_ACTION_BLOCK;
                        rGame.Enabled = true;
                        rGame.Profiles = NET_FW_PROFILE2_ALL;
                        fwPolicy.Rules.Add(rGame);
                    }

                    message = "Block applied via Windows native API.";
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Native COM firewall error, falling back to netsh: " + ex.Message);
            }

            // Fallback a netsh si la API COM fallara
            return AddBlockRulesNetsh(gameExePath, out message);
        }

        public static void DeleteRules()
        {
            try
            {
                Type policyType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                if (policyType != null)
                {
                    dynamic fwPolicy = Activator.CreateInstance(policyType);
                    foreach (string name in RuleNames)
                    {
                        try
                        {
                            fwPolicy.Rules.Remove(name);
                        }
                        catch { }
                    }
                    return;
                }
            }
            catch { }

            // Fallback a netsh si COM no esta disponible
            try
            {
                for (int i = 0; i < RuleNames.Length; i++)
                {
                    string o, e;
                    RunNetsh(string.Format("advfirewall firewall delete rule name=\"{0}\"", RuleNames[i]), out o, out e);
                }
            }
            catch { }
        }

        public static bool RuleExists()
        {
            try
            {
                Type policyType = Type.GetTypeFromProgID("HNetCfg.FwPolicy2");
                if (policyType != null)
                {
                    dynamic fwPolicy = Activator.CreateInstance(policyType);
                    try
                    {
                        dynamic rule = fwPolicy.Rules.Item("Zhonyas_UDP_5000_Out");
                        return (rule != null && rule.Enabled);
                    }
                    catch
                    {
                        return false;
                    }
                }
            }
            catch { }

            string stdout, stderr;
            int code = RunNetsh("advfirewall firewall show rule name=\"Zhonyas_UDP_5000_Out\"", out stdout, out stderr);
            return (code == 0 && !string.IsNullOrEmpty(stdout) && stdout.Contains("Zhonyas_UDP_5000_Out"));
        }

        private static bool AddBlockRulesNetsh(string gameExePath, out string message)
        {
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
