using System;
using System.Drawing;
using System.Windows.Forms;
using LoLFirewallManager.Localization;

namespace LoLFirewallManager.UI
{
    public partial class MainForm
    {
        private MenuStrip _mainMenu;
        private ToolStripMenuItem _menuOptions;
        private ToolStripMenuItem _menuOptEnableGif;
        private ToolStripMenuItem _menuOptSounds;
        private ToolStripMenuItem _menuOptAutoRearm;
        private ToolStripMenuItem _menuOptExit;
        private ToolStripMenuItem _menuHelp;
        private ToolStripMenuItem _menuHelpGuide;
        private ToolStripMenuItem _menuHelpVanguard;
        private ToolStripMenuItem _menuHelpGithub;
        private ToolStripMenuItem _menuHelpAbout;
        private ToolStripMenuItem _menuLanguage;
        private ToolTip _toolTip;

        private Label _lblStatusTitle;
        private Label _lblStatusVal;
        private PictureBox _picLulu;
        private Label _lblTime;
        private ProgressBar _progress;
        private ComboBox _cmbDuration;
        private CheckBox _chkSounds;
        private CheckBox _chkAutoLoop;
        private Button _btnToggle;
        private Button _btnReconnect;

        private void InitializeComponent()
        {
            this.Text = "Zhonyas";
            this.ClientSize = new Size(286, 442);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.FromArgb(248, 249, 250);

            // --- MenuStrip ---
            _mainMenu = new MenuStrip
            {
                BackColor = Color.FromArgb(241, 245, 249),
                Font = new Font("Segoe UI", 9F),
                RenderMode = ToolStripRenderMode.System
            };

            _menuOptions = new ToolStripMenuItem();
            _menuOptEnableGif = new ToolStripMenuItem("", null, (s, e) => SetGifEnabled(_menuOptEnableGif.Checked))
            {
                CheckOnClick = true,
                Checked = _settings.EnableGif
            };
            _menuOptSounds = new ToolStripMenuItem("", null, (s, e) => { _chkSounds.Checked = _menuOptSounds.Checked; })
            {
                CheckOnClick = true,
                Checked = _settings.Sounds
            };
            _menuOptAutoRearm = new ToolStripMenuItem("", null, (s, e) => { _chkAutoLoop.Checked = _menuOptAutoRearm.Checked; })
            {
                CheckOnClick = true,
                Checked = _settings.AutoRearm
            };
            _menuOptExit = new ToolStripMenuItem("", null, (s, e) => this.Close());

            _menuOptions.DropDownItems.Add(_menuOptEnableGif);
            _menuOptions.DropDownItems.Add(_menuOptSounds);
            _menuOptions.DropDownItems.Add(_menuOptAutoRearm);
            _menuOptions.DropDownItems.Add(new ToolStripSeparator());
            _menuOptions.DropDownItems.Add(_menuOptExit);

            _menuHelp = new ToolStripMenuItem();
            _menuHelpGuide = new ToolStripMenuItem("", null, (s, e) => HelpDialogs.ShowGuide(this));
            _menuHelpVanguard = new ToolStripMenuItem("", null, (s, e) => HelpDialogs.ShowVanguardInfo(this));
            _menuHelpGithub = new ToolStripMenuItem("", null, (s, e) =>
            {
                try { System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo("https://github.com/kottaJS/Zhonyas") { UseShellExecute = true }); } catch { }
            });
            _menuHelpAbout = new ToolStripMenuItem("", null, (s, e) => HelpDialogs.ShowAbout(this));

            _menuHelp.DropDownItems.Add(_menuHelpGuide);
            _menuHelp.DropDownItems.Add(_menuHelpVanguard);
            _menuHelp.DropDownItems.Add(new ToolStripSeparator());
            _menuHelp.DropDownItems.Add(_menuHelpGithub);
            _menuHelp.DropDownItems.Add(_menuHelpAbout);

            _menuLanguage = new ToolStripMenuItem();
            RebuildLanguageMenu();

            _mainMenu.Items.Add(_menuOptions);
            _mainMenu.Items.Add(_menuHelp);
            _mainMenu.Items.Add(_menuLanguage);
            this.MainMenuStrip = _mainMenu;
            this.Controls.Add(_mainMenu);

            int y = 26;

            // --- Status Row ---
            _lblStatusTitle = new Label
            {
                Location = new Point(18, y + 12),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139)
            };
            _lblStatusVal = new Label
            {
                Location = new Point(68, y + 11),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74)
            };
            this.Controls.Add(_lblStatusTitle);
            this.Controls.Add(_lblStatusVal);

            // --- Lulu PictureBox ---
            _picLulu = new PictureBox
            {
                Location = new Point(18, y + 36),
                Size = new Size(250, 195),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(15, 23, 42),
                Cursor = Cursors.Hand
            };
            _picLulu.Click += (s, e) => SetGifEnabled(!_settings.EnableGif);
            this.Controls.Add(_picLulu);

            // --- Time Label & Progress Bar ---
            _lblTime = new Label
            {
                Text = FormatTime(_tracker.ConfiguredSeconds),
                Location = new Point(18, y + 236),
                Size = new Size(250, 32),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };
            this.Controls.Add(_lblTime);

            _progress = new ProgressBar
            {
                Location = new Point(18, y + 272),
                Size = new Size(250, 12),
                Value = 0
            };
            this.Controls.Add(_progress);

            // --- Duration Dropdown ---
            _cmbDuration = new ComboBox
            {
                Location = new Point(18, y + 292),
                Size = new Size(250, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F),
                BackColor = Color.White
            };
            _cmbDuration.SelectedIndexChanged += (s, e) =>
            {
                DurationItem item = _cmbDuration.SelectedItem as DurationItem;
                if (item != null)
                {
                    _tracker.ConfiguredSeconds = item.Seconds;
                    _settings.DurationSeconds = item.Seconds;
                    if (_tracker.State == Services.MatchState.Idle ||
                        _tracker.State == Services.MatchState.WaitingMatch ||
                        _tracker.State == Services.MatchState.WaitingClient)
                    {
                        _lblTime.Text = FormatTime(item.Seconds);
                    }
                    SaveSettings();
                }
            };
            this.Controls.Add(_cmbDuration);

            // --- CheckBoxes ---
            _chkSounds = new CheckBox
            {
                Location = new Point(18, y + 324),
                AutoSize = true,
                Checked = _settings.Sounds
            };
            _chkSounds.CheckedChanged += (s, e) =>
            {
                _settings.Sounds = _chkSounds.Checked;
                _tracker.SoundsEnabled = _chkSounds.Checked;
                if (_menuOptSounds != null) _menuOptSounds.Checked = _chkSounds.Checked;
                SaveSettings();
            };

            _chkAutoLoop = new CheckBox
            {
                Location = new Point(148, y + 324),
                AutoSize = true,
                Checked = _settings.AutoRearm,
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            _chkAutoLoop.CheckedChanged += (s, e) =>
            {
                _settings.AutoRearm = _chkAutoLoop.Checked;
                _tracker.AutoRearmEnabled = _chkAutoLoop.Checked;
                if (_menuOptAutoRearm != null) _menuOptAutoRearm.Checked = _chkAutoLoop.Checked;
                SaveSettings();
            };
            this.Controls.Add(_chkSounds);
            this.Controls.Add(_chkAutoLoop);

            // --- Action Buttons ---
            _btnToggle = new Button
            {
                Location = new Point(18, y + 353),
                Size = new Size(120, 38),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                UseVisualStyleBackColor = true,
                Cursor = Cursors.Hand
            };
            _btnToggle.Click += BtnToggle_Click;

            _btnReconnect = new Button
            {
                Location = new Point(148, y + 353),
                Size = new Size(120, 38),
                Font = new Font("Segoe UI", 9.5F),
                ForeColor = Color.FromArgb(185, 28, 28),
                UseVisualStyleBackColor = true,
                Cursor = Cursors.Hand
            };
            _btnReconnect.Click += (s, e) => _tracker.ForceReconnect();

            this.Controls.Add(_btnToggle);
            this.Controls.Add(_btnReconnect);

            // --- Tooltip ---
            _toolTip = new ToolTip
            {
                AutoPopDelay = 7000,
                InitialDelay = 400,
                ReshowDelay = 150,
                ShowAlways = true
            };

            ApplyLocalizedTexts();
        }

        private void RebuildLanguageMenu()
        {
            _menuLanguage.DropDownItems.Clear();
            foreach (ILanguage lang in I18n.SupportedLanguages)
            {
                ILanguage targetLang = lang;
                ToolStripMenuItem item = new ToolStripMenuItem(targetLang.Name, null, (s, e) =>
                {
                    I18n.Current = targetLang;
                    _settings.Language = targetLang.Code;
                    SaveSettings();
                })
                {
                    Checked = (I18n.Current.Code == targetLang.Code)
                };
                _menuLanguage.DropDownItems.Add(item);
            }
        }

        private void ApplyLocalizedTexts()
        {
            _menuOptions.Text = I18n.Current.MenuOptions;
            _menuOptEnableGif.Text = I18n.Current.MenuOptGif;
            _menuOptSounds.Text = I18n.Current.MenuOptSounds;
            _menuOptAutoRearm.Text = I18n.Current.MenuOptAutoRearm;
            _menuOptExit.Text = I18n.Current.MenuExit;

            _menuHelp.Text = I18n.Current.MenuHelp;
            _menuHelpGuide.Text = I18n.Current.MenuGuide;
            _menuHelpVanguard.Text = I18n.Current.MenuVanguard;
            _menuHelpGithub.Text = I18n.Current.MenuGithub;
            _menuHelpAbout.Text = I18n.Current.MenuAbout;

            _menuLanguage.Text = I18n.Current.MenuLanguage;
            RebuildLanguageMenu();

            _lblStatusTitle.Text = I18n.Current.StatusTitle;
            _chkSounds.Text = I18n.Current.ChkSounds;
            _chkAutoLoop.Text = I18n.Current.ChkAutoRearm;
            _btnReconnect.Text = I18n.Current.BtnReconnect;
            _btnToggle.Text = (_tracker.State == Services.MatchState.Idle) ? I18n.Current.BtnStart : I18n.Current.BtnStop;

            if (_trayManager != null)
            {
                _trayManager.UpdateToggleText(_btnToggle.Text);
                _trayManager.UpdateLocalizedMenu();
            }

            _toolTip.SetToolTip(_btnToggle, I18n.Current.TipToggle);
            _toolTip.SetToolTip(_btnReconnect, I18n.Current.TipReconnect);
            _toolTip.SetToolTip(_cmbDuration, I18n.Current.TipDuration);
            _toolTip.SetToolTip(_chkSounds, I18n.Current.TipSounds);
            _toolTip.SetToolTip(_chkAutoLoop, I18n.Current.TipAutoRearm);
            _toolTip.SetToolTip(_picLulu, I18n.Current.TipLulu);

            PopulateDurations();
            UpdateStatusDisplay();
        }

        private void PopulateDurations()
        {
            int prevSeconds = _tracker.ConfiguredSeconds;
            _cmbDuration.Items.Clear();
            _cmbDuration.Items.Add(new DurationItem(I18n.Current.Dur1Min, 60));
            _cmbDuration.Items.Add(new DurationItem(I18n.Current.Dur2Min, 120));
            _cmbDuration.Items.Add(new DurationItem(I18n.Current.Dur3Min, 180));
            _cmbDuration.Items.Add(new DurationItem(I18n.Current.Dur35Min, 210));
            _cmbDuration.Items.Add(new DurationItem(I18n.Current.Dur4Min, 240));
            _cmbDuration.Items.Add(new DurationItem(I18n.Current.Dur5Min, 300));
            _cmbDuration.Items.Add(new DurationItem(I18n.Current.Dur15Sec, 15));

            int sel = 3;
            for (int i = 0; i < _cmbDuration.Items.Count; i++)
            {
                DurationItem it = _cmbDuration.Items[i] as DurationItem;
                if (it != null && it.Seconds == prevSeconds) { sel = i; break; }
            }
            _cmbDuration.SelectedIndex = sel;
        }

        private class DurationItem
        {
            public string Text { get; private set; }
            public int Seconds { get; private set; }
            public DurationItem(string text, int seconds) { Text = text; Seconds = seconds; }
            public override string ToString() { return Text; }
        }
    }
}
