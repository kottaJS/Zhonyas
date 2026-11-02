using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using LoLFirewallManager.Localization;
using LoLFirewallManager.Services;

namespace LoLFirewallManager.UI
{
    public partial class MainForm : Form
    {
        private AppSettings _settings;
        private MatchTracker _tracker;
        private TrayManager _trayManager;
        private LuluAnimator _animator;

        public MainForm()
        {
            _settings = SettingsManager.Load();
            I18n.SetLanguageByCode(_settings.Language);

            this.Icon = AssetLoader.LoadAppIcon();
            _tracker = new MatchTracker
            {
                SoundsEnabled = _settings.Sounds,
                AutoRearmEnabled = _settings.AutoRearm,
                ConfiguredSeconds = _settings.DurationSeconds
            };

            InitializeComponent();

            _animator = new LuluAnimator(_picLulu, AssetLoader.LoadLuluFrames(), _settings.EnableGif);
            _trayManager = new TrayManager(this.Icon, RestoreWindow, ToggleMonitoring, () => _tracker.ForceReconnect(), () => this.Close());

            _tracker.StateChanged += OnTrackerStateChanged;
            _tracker.Tick += OnTrackerTick;
            _tracker.Reconnected += () => InvokeOnUI(() => _trayManager.ShowBalloon(2500, "Zhonyas", I18n.Current.TrayReconnected, ToolTipIcon.Info));
            _tracker.ArmedForNextMatch += () => InvokeOnUI(() => _trayManager.ShowBalloon(2500, "Zhonyas", I18n.Current.TrayArmed, ToolTipIcon.Info));

            I18n.LanguageChanged += ApplyLocalizedTexts;

            this.Shown += (s, e) => RestoreWindow();
            this.FormClosing += (s, e) =>
            {
                _animator.Dispose();
                _tracker.Stop();
                _trayManager.Dispose();
            };

            if (_settings.AutoRearm && Process.GetProcessesByName("LeagueClient").Length > 0)
                _tracker.Start();
            else
                FirewallService.DeleteRules();
        }

        private void ToggleMonitoring()
        {
            if (_tracker.State == MatchState.Idle)
            {
                _tracker.Start();
            }
            else
            {
                bool suppress = _settings.SuppressStopWarning;
                if (!HelpDialogs.ConfirmStop(this, _settings.AutoRearm, _tracker.State == MatchState.Counting, ref suppress, SaveSettings))
                    return;
                _settings.SuppressStopWarning = suppress;
                _tracker.Stop();
            }
        }

        private void BtnToggle_Click(object sender, EventArgs e) { ToggleMonitoring(); }

        private void SetGifEnabled(bool enable)
        {
            _settings.EnableGif = enable;
            if (_menuOptEnableGif != null) _menuOptEnableGif.Checked = enable;
            _animator.Enabled = enable;
            SaveSettings();
        }

        private void SaveSettings() { SettingsManager.Save(_settings); }

        private void UpdateStatusDisplay()
        {
            string text = I18n.Current.StatusReady;
            Color color = Color.FromArgb(22, 163, 74);

            switch (_tracker.State)
            {
                case MatchState.WaitingMatch:
                    text = I18n.Current.StatusWaitingLoL;
                    color = Color.FromArgb(217, 119, 6);
                    break;
                case MatchState.WaitingClient:
                    text = I18n.Current.StatusWaitingClient;
                    color = Color.FromArgb(100, 116, 139);
                    break;
                case MatchState.Counting:
                    text = I18n.Current.StatusStasisActive;
                    color = Color.FromArgb(220, 38, 38);
                    break;
                case MatchState.InGame:
                    text = I18n.Current.StatusInGame;
                    color = Color.FromArgb(22, 163, 74);
                    break;
            }

            _lblStatusVal.Text = text;
            _lblStatusVal.ForeColor = color;
            _cmbDuration.Enabled = (_tracker.State != MatchState.Counting && _tracker.State != MatchState.InGame);
            if (_trayManager != null && _tracker.State != MatchState.Counting)
                _trayManager.UpdateText("Zhonyas - " + text);
        }

        private void OnTrackerStateChanged(MatchState state)
        {
            InvokeOnUI(() =>
            {
                string toggleText = (state == MatchState.Idle) ? I18n.Current.BtnStart : I18n.Current.BtnStop;
                _btnToggle.Text = toggleText;
                if (_trayManager != null) _trayManager.UpdateToggleText(toggleText);

                _cmbDuration.Enabled = (state != MatchState.Counting && state != MatchState.InGame);

                if (state == MatchState.Idle)
                {
                    _progress.Value = 0;
                    _lblTime.Text = FormatTime(_tracker.ConfiguredSeconds);
                }
                else if (state == MatchState.InGame)
                {
                    _progress.Value = 100;
                    _lblTime.Text = "0s";
                }

                _animator.SetState(state);
                UpdateStatusDisplay();
            });
        }

        private void OnTrackerTick(int remaining, int pct)
        {
            InvokeOnUI(() =>
            {
                _lblTime.Text = FormatTime(remaining);
                _progress.Value = pct;
                if (_trayManager != null) _trayManager.UpdateText("Zhonyas - " + FormatTime(remaining));
            });
        }

        private void RestoreWindow()
        {
            this.Show();
            if (this.WindowState == FormWindowState.Minimized) this.WindowState = FormWindowState.Normal;
            this.TopMost = true;
            this.BringToFront();
            this.Activate();
            this.Focus();
            this.TopMost = false;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized) this.Hide();
        }

        private void InvokeOnUI(Action action)
        {
            if (this.InvokeRequired) this.BeginInvoke(action);
            else action();
        }

        private static string FormatTime(int s)
        {
            return s <= 0 ? "0s" : (s / 60 > 0 ? string.Format("{0}m {1:D2}s", s / 60, s % 60) : string.Format("{0}s", s));
        }
    }
}
