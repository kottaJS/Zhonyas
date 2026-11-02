namespace LoLFirewallManager.Localization
{
    public class EnglishLanguage : ILanguage
    {
        public string Name { get { return "English"; } }
        public string Code { get { return "en"; } }

        public string StatusTitle { get { return "Status:"; } }
        public string StatusReady { get { return "Ready"; } }
        public string StatusWaitingLoL { get { return "Waiting for LoL..."; } }
        public string StatusWaitingClient { get { return "Waiting for Client..."; } }
        public string StatusStasisActive { get { return "Stasis Active"; } }
        public string StatusInGame { get { return "In Game"; } }
        public string StatusStopped { get { return "Stopped"; } }

        public string BtnStart { get { return "Start"; } }
        public string BtnStop { get { return "Stop"; } }
        public string BtnReconnect { get { return "Reconnect"; } }
        public string ChkSounds { get { return "Sounds"; } }
        public string ChkAutoRearm { get { return "Auto-rearm"; } }

        public string MenuOptions { get { return "&Options"; } }
        public string MenuHelp { get { return "&Help"; } }
        public string MenuLanguage { get { return "&Language"; } }
        public string MenuOptGif { get { return "Lulu Animation (Disable to save CPU)"; } }
        public string MenuOptSounds { get { return "Sound Alerts"; } }
        public string MenuOptAutoRearm { get { return "Auto-rearm Next Match"; } }
        public string MenuExit { get { return "Exit"; } }
        public string MenuGuide { get { return "How to Use (FAQ)"; } }
        public string MenuVanguard { get { return "Anti-Cheat & Vanguard Safety"; } }
        public string MenuGithub { get { return "Visit GitHub Project..."; } }
        public string MenuAbout { get { return "About Zhonyas"; } }

        public string TipToggle { get { return "Start or stop monitoring League of Legends."; } }
        public string TipReconnect { get { return "Emergency unfreeze. Instantly unblocks the game without stopping Zhonyas."; } }
        public string TipDuration { get { return "Select how long to freeze your loading screen before reconnecting (3.5 min recommended)."; } }
        public string TipSounds { get { return "Play audio cues when stasis starts, at 10s countdown, and upon reconnect."; } }
        public string TipAutoRearm { get { return "Automatically prepares Zhonyas for the next game when your current match finishes."; } }
        public string TipLulu { get { return "Lulu animation preview. Click or check Options to disable animation and save CPU."; } }

        public string Dur1Min { get { return "1 min (Very Safe)"; } }
        public string Dur2Min { get { return "2 min (Safe)"; } }
        public string Dur3Min { get { return "3 min (Safe)"; } }
        public string Dur35Min { get { return "3.5 min (Recommended)"; } }
        public string Dur4Min { get { return "4 min (Risky - Match may start)"; } }
        public string Dur5Min { get { return "5 min (Danger - Remake risk)"; } }
        public string Dur15Sec { get { return "15 sec (Quick test)"; } }

        public string GuideContent
        {
            get
            {
                return "HOW TO USE ZHONYAS:\n\n" +
                       "1. Choose your preferred stall duration (3.5 min is recommended).\n" +
                       "2. Click 'Start' (or leave 'Auto-rearm' enabled).\n" +
                       "3. In the game loading screen, Zhonyas temporarily holds your connection.\n" +
                       "4. When countdown ends, you automatically reconnect before minions spawn.\n" +
                       "5. Click 'Reconnect' at any time to unfreeze immediately.\n\n" +
                       "TIPS:\n" +
                       "- In 'Options', disable the Lulu animation to drop CPU usage to 0%.\n" +
                       "- Hover over any button or checkbox to see what it does.";
            }
        }

        public string VanguardContent
        {
            get
            {
                return "ANTI-CHEAT & VANGUARD SAFETY:\n\n" +
                       "Zhonyas operates strictly at the Windows Firewall level.\n\n" +
                       "- It DOES NOT inject code into League of Legends.\n" +
                       "- It DOES NOT modify game files.\n" +
                       "- It DOES NOT read game memory.\n" +
                       "- It uses official Windows networking APIs.\n\n" +
                       "Because of this, it does not trigger Vanguard anti-cheat bans.";
            }
        }

        public string AboutContent
        {
            get
            {
                return "Zhonyas v1.0.0\n" +
                       "League of Legends Loading Screen Utility\n\n" +
                       "Created by kottaJS\n" +
                       "Licensed under GPL-3.0";
            }
        }

        public string StopMsgBoth
        {
            get { return "Stopping will cancel both active stasis and auto-rearm for future matches.\n\nUse Reconnect instead if you only want to unblock early."; }
        }

        public string StopMsgAutoRearm
        {
            get { return "Stopping will turn off Zhonyas and cancel auto-rearm for future matches.\n\nUse Reconnect instead if you only want to unblock early."; }
        }

        public string StopMsgStasis
        {
            get { return "Stopping will cancel your active stasis.\n\nUse Reconnect instead if you only want to unblock early."; }
        }

        public string DontShowAgain { get { return "Don't show this again"; } }
        public string BtnCancel { get { return "Cancel"; } }

        public string TrayOpen { get { return "Open Zhonyas"; } }
        public string TrayReconnected { get { return "Reconnected! Game unfreezed."; } }
        public string TrayArmed { get { return "Match ended. Armed for next match!"; } }
    }
}

