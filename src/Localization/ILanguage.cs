namespace LoLFirewallManager.Localization
{
    public interface ILanguage
    {
        string Name { get; }
        string Code { get; }

        // Status
        string StatusTitle { get; }
        string StatusReady { get; }
        string StatusWaitingLoL { get; }
        string StatusWaitingClient { get; }
        string StatusStasisActive { get; }
        string StatusInGame { get; }
        string StatusStopped { get; }

        // Buttons & Controls
        string BtnStart { get; }
        string BtnStop { get; }
        string BtnReconnect { get; }
        string ChkSounds { get; }
        string ChkAutoRearm { get; }

        // Menus
        string MenuOptions { get; }
        string MenuHelp { get; }
        string MenuLanguage { get; }
        string MenuOptGif { get; }
        string MenuOptSounds { get; }
        string MenuOptAutoRearm { get; }
        string MenuExit { get; }
        string MenuGuide { get; }
        string MenuVanguard { get; }
        string MenuGithub { get; }
        string MenuAbout { get; }

        // Tooltips
        string TipToggle { get; }
        string TipReconnect { get; }
        string TipDuration { get; }
        string TipSounds { get; }
        string TipAutoRearm { get; }
        string TipLulu { get; }

        // Durations
        string Dur1Min { get; }
        string Dur2Min { get; }
        string Dur3Min { get; }
        string Dur35Min { get; }
        string Dur4Min { get; }
        string Dur5Min { get; }
        string Dur15Sec { get; }

        // Dialogs & Help
        string GuideContent { get; }
        string VanguardContent { get; }
        string AboutContent { get; }
        string StopMsgBoth { get; }
        string StopMsgAutoRearm { get; }
        string StopMsgStasis { get; }
        string DontShowAgain { get; }
        string BtnCancel { get; }

        // Tray
        string TrayOpen { get; }
        string TrayReconnected { get; }
        string TrayArmed { get; }
    }
}

