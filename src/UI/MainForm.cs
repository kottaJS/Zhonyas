using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;
using LoLFirewallManager.Services;

namespace LoLFirewallManager.UI
{
    public class MainForm : Form
    {
        private enum State
        {
            Idle,
            WaitingClient,
            WaitingMatch,
            Counting,
            InGame
        }

        private string _gameExePath;
        private string _clientExePath;

        private State _state = State.Idle;
        private Process _proc = null;
        private Stopwatch _sw = new Stopwatch();

        private bool _suppressStopWarning = false;
        private static readonly string SettingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Zhonyas", "settings.ini");

        private int _configuredSeconds = 210;
        private int _totalSec = 210;
        private bool _playedTenSec = false;

        private Timer _monitorTimer;
        private Timer _animTimer;

        private Bitmap[] _luluFrames = null;
        private int _currentFrame = 0;

        private NotifyIcon _trayIcon;
        private ContextMenuStrip _trayMenu;
        private ToolStripMenuItem _trayOpen;
        private ToolStripMenuItem _trayToggle;
        private ToolStripMenuItem _trayReconnect;
        private ToolStripMenuItem _trayExit;

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

        public MainForm()
        {
            LoadSettings();
            InitIcon();
            LoadLuluGif();
            InitUI();
            InitTray();

            _gameExePath = LeagueFinder.FindGameExePath();
            _clientExePath = LeagueFinder.FindClientExePath();

            CheckInitialFirewall();

            if (_chkAutoLoop.Checked && Process.GetProcessesByName("LeagueClient").Length > 0)
            {
                Start();
            }
        }

        private void InitIcon()
        {
            try
            {
                string iconPath = ResolveAssetPath("zhonyas.ico", "icons");
                if (string.IsNullOrEmpty(iconPath))
                    iconPath = ResolveAssetPath("3157-_1_.ico", "icons");

                if (!string.IsNullOrEmpty(iconPath) && File.Exists(iconPath))
                {
                    this.Icon = new Icon(iconPath);
                }
                else
                {
                    this.Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
                }
            }
            catch { }
        }

        private void LoadLuluGif()
        {
            try
            {
                string gifPath = ResolveAssetPath("luluzhonya.gif", "images");
                if (!string.IsNullOrEmpty(gifPath) && File.Exists(gifPath))
                {
                    using (Image img = Image.FromFile(gifPath))
                    {
                        ParseGifFrames(img);
                        return;
                    }
                }

                var asm = System.Reflection.Assembly.GetExecutingAssembly();
                using (Stream stream = asm.GetManifestResourceStream("luluzhonya.gif"))
                {
                    if (stream != null)
                    {
                        using (Image img = Image.FromStream(stream))
                        {
                            ParseGifFrames(img);
                            return;
                        }
                    }
                }
            }
            catch { }
        }

        private void ParseGifFrames(Image img)
        {
            FrameDimension dim = new FrameDimension(img.FrameDimensionsList[0]);
            int count = img.GetFrameCount(dim);
            _luluFrames = new Bitmap[count];
            for (int i = 0; i < count; i++)
            {
                img.SelectActiveFrame(dim, i);
                _luluFrames[i] = new Bitmap(img);
            }
        }

        private static string ResolveAssetPath(string fileName, string subFolder)
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string[] candidates = new string[]
            {
                Path.Combine(baseDir, "assets", subFolder, fileName),
                Path.Combine(baseDir, "assets", fileName),
                Path.Combine(baseDir, fileName),
                Path.GetFullPath(Path.Combine("assets", subFolder, fileName)),
                Path.GetFullPath(Path.Combine("assets", fileName)),
                Path.GetFullPath(fileName)
            };

            foreach (string p in candidates)
            {
                if (File.Exists(p)) return p;
            }
            return null;
        }

        private void InitTray()
        {
            _trayMenu = new ContextMenuStrip();
            _trayOpen = new ToolStripMenuItem("Open Zhonyas", null, (s, e) => RestoreWindow());
            _trayOpen.Font = new Font(_trayMenu.Font, FontStyle.Bold);

            _trayToggle = new ToolStripMenuItem("Start", null, (s, e) => BtnToggle_Click(s, e));
            _trayReconnect = new ToolStripMenuItem("Reconnect Now", null, (s, e) => ForceReconnect());
            _trayExit = new ToolStripMenuItem("Exit", null, (s, e) => this.Close());

            _trayMenu.Items.Add(_trayOpen);
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add(_trayToggle);
            _trayMenu.Items.Add(_trayReconnect);
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add(_trayExit);

            _trayIcon = new NotifyIcon();
            _trayIcon.Icon = this.Icon;
            _trayIcon.Text = "Zhonyas - Ready";
            _trayIcon.ContextMenuStrip = _trayMenu;
            _trayIcon.Visible = true;

            _trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    RestoreWindow();
                }
            };
        }

        private void RestoreWindow()
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
            this.BringToFront();
            this.Activate();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void InitUI()
        {
            this.Text = "Zhonyas";
            this.ClientSize = new Size(286, 420);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point);
            this.BackColor = Color.FromArgb(248, 249, 250);

            _lblStatusTitle = new Label
            {
                Text = "Status:",
                Location = new Point(18, 14),
                AutoSize = true,
                ForeColor = Color.FromArgb(100, 116, 139)
            };

            _lblStatusVal = new Label
            {
                Text = "Ready",
                Location = new Point(68, 13),
                AutoSize = true,
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                ForeColor = Color.FromArgb(22, 163, 74)
            };

            this.Controls.Add(_lblStatusTitle);
            this.Controls.Add(_lblStatusVal);

            _picLulu = new PictureBox
            {
                Location = new Point(18, 38),
                Size = new Size(250, 195),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(15, 23, 42)
            };
            if (_luluFrames != null && _luluFrames.Length > 0)
            {
                _picLulu.Image = _luluFrames[0];
            }
            this.Controls.Add(_picLulu);

            _lblTime = new Label
            {
                Text = "3m 30s",
                Location = new Point(18, 238),
                Size = new Size(250, 32),
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.FromArgb(15, 23, 42)
            };
            this.Controls.Add(_lblTime);

            _progress = new ProgressBar
            {
                Location = new Point(18, 274),
                Size = new Size(250, 12),
                Value = 0
            };
            this.Controls.Add(_progress);

            _cmbDuration = new ComboBox
            {
                Location = new Point(18, 294),
                Size = new Size(250, 26),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 9F, FontStyle.Regular),
                BackColor = Color.White
            };
            _cmbDuration.Items.Add(new DurationItem("1 min (Very Safe)", 60));
            _cmbDuration.Items.Add(new DurationItem("2 min (Safe)", 120));
            _cmbDuration.Items.Add(new DurationItem("3 min (Safe)", 180));
            _cmbDuration.Items.Add(new DurationItem("3.5 min (Recommended)", 210));
            _cmbDuration.Items.Add(new DurationItem("4 min (Risky - Match may start)", 240));
            _cmbDuration.Items.Add(new DurationItem("5 min (Danger - Remake risk)", 300));
            _cmbDuration.Items.Add(new DurationItem("15 sec (Quick test)", 15));
            _cmbDuration.SelectedIndex = 3;

            _cmbDuration.SelectedIndexChanged += (s, e) =>
            {
                DurationItem item = _cmbDuration.SelectedItem as DurationItem;
                if (item != null)
                {
                    _configuredSeconds = item.Seconds;
                    if (_state == State.Idle || _state == State.WaitingMatch || _state == State.WaitingClient)
                    {
                        _lblTime.Text = FormatTime(_configuredSeconds);
                    }
                }
            };
            this.Controls.Add(_cmbDuration);

            _chkSounds = new CheckBox
            {
                Text = "Sound alerts",
                Location = new Point(18, 326),
                AutoSize = true,
                Checked = true
            };

            _chkAutoLoop = new CheckBox
            {
                Text = "Auto-loop matches",
                Location = new Point(132, 326),
                AutoSize = true,
                Checked = true,
                ForeColor = Color.FromArgb(51, 65, 85)
            };
            this.Controls.Add(_chkSounds);
            this.Controls.Add(_chkAutoLoop);

            _btnToggle = new Button
            {
                Text = "Start",
                Location = new Point(18, 355),
                Size = new Size(120, 38),
                Font = new Font("Segoe UI", 10F, FontStyle.Bold),
                UseVisualStyleBackColor = true,
                Cursor = Cursors.Hand
            };
            _btnToggle.Click += BtnToggle_Click;

            _btnReconnect = new Button
            {
                Text = "Reconnect",
                Location = new Point(148, 355),
                Size = new Size(120, 38),
                Font = new Font("Segoe UI", 9.5F, FontStyle.Regular),
                ForeColor = Color.FromArgb(185, 28, 28),
                UseVisualStyleBackColor = true,
                Cursor = Cursors.Hand
            };
            _btnReconnect.Click += (s, e) => ForceReconnect();

            this.Controls.Add(_btnToggle);
            this.Controls.Add(_btnReconnect);

            _animTimer = new Timer();
            _animTimer.Interval = 75;
            _animTimer.Tick += AnimTimer_Tick;
            _animTimer.Start();

            _monitorTimer = new Timer();
            _monitorTimer.Interval = 500;
            _monitorTimer.Tick += MonitorTimer_Tick;

            this.FormClosing += MainForm_FormClosing;
        }

        private void CheckInitialFirewall()
        {
            if (FirewallService.RuleExists())
            {
                SetStatus("Active", Color.FromArgb(220, 38, 38));
                DialogResult dr = MessageBox.Show(
                    "A previous block rule was detected in Windows Firewall.\n\nReconnect now?",
                    "Zhonyas", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (dr == DialogResult.Yes)
                {
                    ForceReconnect();
                }
            }
        }

        private void SetStatus(string text, Color color)
        {
            _lblStatusVal.Text = text;
            _lblStatusVal.ForeColor = color;
        }

        private void BtnToggle_Click(object sender, EventArgs e)
        {
            if (_state == State.Idle)
            {
                Start();
            }
            else
            {
                if (!ConfirmStop()) return;
                Stop();
            }
        }

        private bool ConfirmStop()
        {
            if (_suppressStopWarning) return true;

            using (Form prompt = new Form())
            {
                prompt.Text = "Stop Zhonyas";
                prompt.ClientSize = new Size(320, 165);
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;
                prompt.ShowInTaskbar = false;
                prompt.Font = new Font("Segoe UI", 9F);
                prompt.BackColor = Color.FromArgb(248, 249, 250);

                Label lblMsg = new Label
                {
                    Text = "Stopping will turn off Zhonyas and cancel auto-loop.\n\nUse Reconnect instead if you only want to unblock early.",
                    Location = new Point(16, 16),
                    Size = new Size(288, 55),
                    ForeColor = Color.FromArgb(15, 23, 42)
                };

                CheckBox chkDontShow = new CheckBox
                {
                    Text = "Don't show this again",
                    Location = new Point(18, 80),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(100, 116, 139)
                };

                Button btnOk = new Button
                {
                    Text = "Stop",
                    DialogResult = DialogResult.OK,
                    Location = new Point(140, 118),
                    Size = new Size(80, 30),
                    UseVisualStyleBackColor = true
                };

                Button btnCancel = new Button
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(226, 118),
                    Size = new Size(80, 30),
                    UseVisualStyleBackColor = true
                };

                prompt.Controls.Add(lblMsg);
                prompt.Controls.Add(chkDontShow);
                prompt.Controls.Add(btnOk);
                prompt.Controls.Add(btnCancel);
                prompt.AcceptButton = btnOk;
                prompt.CancelButton = btnCancel;

                if (prompt.ShowDialog(this) == DialogResult.OK)
                {
                    if (chkDontShow.Checked)
                    {
                        _suppressStopWarning = true;
                        SaveSettings();
                    }
                    return true;
                }
                return false;
            }
        }

        private void LoadSettings()
        {
            try
            {
                if (File.Exists(SettingsFile))
                {
                    string content = File.ReadAllText(SettingsFile);
                    if (content.Contains("SuppressStopWarning=1"))
                    {
                        _suppressStopWarning = true;
                    }
                }
            }
            catch { }
        }

        private void SaveSettings()
        {
            try
            {
                string dir = Path.GetDirectoryName(SettingsFile);
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                File.WriteAllText(SettingsFile, "SuppressStopWarning=" + (_suppressStopWarning ? "1" : "0"));
            }
            catch { }
        }

        private void Start()
        {
            if (!File.Exists(_gameExePath))
                _gameExePath = LeagueFinder.FindGameExePath();
            if (!File.Exists(_clientExePath))
                _clientExePath = LeagueFinder.FindClientExePath();

            _playedTenSec = false;
            _proc = null;

            _btnToggle.Text = "Stop";
            _trayToggle.Text = "Stop";

            Process[] gameList = Process.GetProcessesByName("League of Legends");
            if (gameList.Length > 0)
            {
                _proc = gameList[0];
                int alreadyElapsed = 0;
                try
                {
                    TimeSpan elapsed = DateTime.Now - _proc.StartTime;
                    alreadyElapsed = (int)Math.Max(0, elapsed.TotalSeconds);
                }
                catch { }

                string msg;
                FirewallService.AddBlockRules(_gameExePath, out msg);

                if (_chkSounds.Checked) SoundAlert.PlayZhonyaIn();

                _totalSec = Math.Max(1, _configuredSeconds - alreadyElapsed);
                _state = State.Counting;
                SetStatus("Stasis Active", Color.FromArgb(220, 38, 38));
                _lblTime.Text = FormatTime(_totalSec);
                _progress.Value = (int)(((double)alreadyElapsed / _configuredSeconds) * 100);
                UpdateTrayText(string.Format("Zhonyas - {0}", FormatTime(_totalSec)));

                _currentFrame = 108;
                _sw.Restart();
            }
            else
            {
                Process[] clientList = Process.GetProcessesByName("LeagueClient");
                if (clientList.Length > 0)
                {
                    string msg;
                    FirewallService.AddBlockRules(_gameExePath, out msg);

                    _totalSec = _configuredSeconds;
                    _state = State.WaitingMatch;
                    SetStatus("Waiting for LoL...", Color.FromArgb(217, 119, 6));
                    _lblTime.Text = FormatTime(_configuredSeconds);
                    _progress.Value = 0;
                    UpdateTrayText("Zhonyas - Waiting for LoL...");
                }
                else
                {
                    _totalSec = _configuredSeconds;
                    _state = State.WaitingClient;
                    SetStatus("Waiting for Client...", Color.FromArgb(100, 116, 139));
                    _lblTime.Text = FormatTime(_configuredSeconds);
                    _progress.Value = 0;
                    UpdateTrayText("Zhonyas - Waiting for Client...");
                }
            }

            _monitorTimer.Start();
        }

        private void Stop()
        {
            _monitorTimer.Stop();
            _sw.Reset();

            FirewallService.DeleteRules();

            bool wasInStasis = (_state == State.Counting);

            _state = State.Idle;

            if (wasInStasis)
            {
                _currentFrame = 138;
                if (_chkSounds.Checked) SoundAlert.PlayZhonyaOut();
            }
            else
            {
                _currentFrame = 0;
            }

            SetStatus("Stopped", Color.FromArgb(100, 116, 139));
            _lblTime.Text = FormatTime(_configuredSeconds);
            _progress.Value = 0;
            _btnToggle.Text = "Start";
            _trayToggle.Text = "Start";
            UpdateTrayText("Zhonyas - Stopped");
        }

        private void ForceReconnect()
        {
            FirewallService.DeleteRules();

            bool wasInStasis = (_state == State.Counting);

            if (wasInStasis)
            {
                _state = State.InGame;
                _currentFrame = 138;
                if (_chkSounds.Checked) SoundAlert.PlayZhonyaOut();

                SetStatus("In Game", Color.FromArgb(22, 163, 74));
                _lblTime.Text = "0s";
                _progress.Value = 100;
                _btnToggle.Text = "Stop";
                _trayToggle.Text = "Stop";
                UpdateTrayText("Zhonyas - In Game");

                TriggerAutoRetry();
            }
            else
            {
                _monitorTimer.Stop();
                _sw.Reset();
                _state = State.Idle;
                _currentFrame = 0;

                SetStatus("Ready", Color.FromArgb(22, 163, 74));
                _lblTime.Text = FormatTime(_configuredSeconds);
                _progress.Value = 0;
                _btnToggle.Text = "Start";
                _trayToggle.Text = "Start";
                UpdateTrayText("Zhonyas - Ready");
            }
        }

        private void UpdateTrayText(string text)
        {
            try
            {
                if (text.Length > 63) text = text.Substring(0, 63);
                _trayIcon.Text = text;
            }
            catch { }
        }

        private static string FormatTime(int seconds)
        {
            if (seconds <= 0) return "0s";
            int m = seconds / 60;
            int s = seconds % 60;
            if (m > 0)
                return string.Format("{0}m {1:D2}s", m, s);
            return string.Format("{0}s", seconds);
        }

        private void AnimTimer_Tick(object sender, EventArgs e)
        {
            if (_luluFrames == null || _luluFrames.Length == 0) return;

            if (_state == State.Counting)
            {
                if (_currentFrame < 108 || _currentFrame > 119)
                {
                    _currentFrame = 108;
                }
                else if (_currentFrame < 115)
                {
                    _currentFrame++;
                }
                else
                {
                    _currentFrame++;
                    if (_currentFrame > 119)
                    {
                        _currentFrame = 115;
                    }
                }
            }
            else if (_currentFrame >= 138 && _currentFrame < _luluFrames.Length)
            {
                _currentFrame++;
                if (_currentFrame >= _luluFrames.Length)
                {
                    _currentFrame = 0;
                }
            }
            else
            {
                _currentFrame++;
                if (_currentFrame > 107)
                {
                    _currentFrame = 0;
                }
            }

            if (_currentFrame >= 0 && _currentFrame < _luluFrames.Length)
            {
                _picLulu.Image = _luluFrames[_currentFrame];
            }
        }

        private void MonitorTimer_Tick(object sender, EventArgs e)
        {
            if (_state == State.WaitingClient)
            {
                if (Process.GetProcessesByName("LeagueClient").Length > 0 || Process.GetProcessesByName("League of Legends").Length > 0)
                {
                    string msg;
                    FirewallService.AddBlockRules(_gameExePath, out msg);

                    _state = State.WaitingMatch;
                    SetStatus("Waiting for LoL...", Color.FromArgb(217, 119, 6));
                    UpdateTrayText("Zhonyas - Waiting for LoL...");
                }
                return;
            }

            if (_state == State.WaitingMatch)
            {
                if (Process.GetProcessesByName("LeagueClient").Length == 0 && Process.GetProcessesByName("League of Legends").Length == 0)
                {
                    FirewallService.DeleteRules();
                    _state = State.WaitingClient;
                    SetStatus("Waiting for Client...", Color.FromArgb(100, 116, 139));
                    UpdateTrayText("Zhonyas - Waiting for Client...");
                    return;
                }

                Process[] list = Process.GetProcessesByName("League of Legends");
                if (list.Length > 0)
                {
                    _proc = list[0];
                    _state = State.Counting;
                    _totalSec = _configuredSeconds;
                    SetStatus("Stasis Active", Color.FromArgb(220, 38, 38));
                    _lblTime.Text = FormatTime(_totalSec);
                    _progress.Value = 0;

                    if (_chkSounds.Checked) SoundAlert.PlayZhonyaIn();

                    _currentFrame = 108;
                    _sw.Restart();
                }
                return;
            }

            if (_state == State.Counting)
            {
                bool alive = false;
                try
                {
                    if (_proc != null && !_proc.HasExited) alive = true;
                }
                catch { }

                if (!alive && Process.GetProcessesByName("League of Legends").Length > 0)
                    alive = true;

                if (!alive)
                {
                    FirewallService.DeleteRules();
                    _currentFrame = 138;
                    if (_chkSounds.Checked) SoundAlert.PlayZhonyaOut();

                    if (_chkAutoLoop.Checked)
                    {
                        _state = State.WaitingMatch;
                        SetStatus("Waiting for LoL...", Color.FromArgb(217, 119, 6));
                    }
                    else
                    {
                        Stop();
                    }
                    return;
                }

                double elapsed = _sw.Elapsed.TotalSeconds;
                int remaining = _totalSec - (int)Math.Floor(elapsed);

                if (remaining > 0)
                {
                    _lblTime.Text = FormatTime(remaining);
                    UpdateTrayText(string.Format("Zhonyas - {0}", FormatTime(remaining)));

                    int p = (int)((elapsed / _totalSec) * 100);
                    _progress.Value = Math.Min(100, Math.Max(0, p));

                    if (_chkSounds.Checked)
                    {
                        if (remaining <= 10 && !_playedTenSec)
                        {
                            _playedTenSec = true;
                            SoundAlert.PlayTenSecondsWarning();
                        }
                    }
                }
                else
                {
                    _sw.Reset();
                    _lblTime.Text = "0s";
                    _progress.Value = 100;

                    FirewallService.DeleteRules();

                    _state = State.InGame;
                    _currentFrame = 138;

                    SetStatus("In Game", Color.FromArgb(22, 163, 74));
                    UpdateTrayText("Zhonyas - In Game");

                    if (_chkSounds.Checked)
                    {
                        SoundAlert.PlayZhonyaOut();
                    }

                    _trayIcon.ShowBalloonTip(2500, "Zhonyas", "Reconnected! Game unfreezed.", ToolTipIcon.Info);

                    TriggerAutoRetry();
                }
                return;
            }

            if (_state == State.InGame)
            {
                bool gameAlive = false;
                try
                {
                    if (_proc != null && !_proc.HasExited) gameAlive = true;
                }
                catch { }

                if (!gameAlive && Process.GetProcessesByName("League of Legends").Length > 0)
                    gameAlive = true;

                if (!gameAlive)
                {
                    if (_chkAutoLoop.Checked)
                    {
                        if (Process.GetProcessesByName("LeagueClient").Length > 0)
                        {
                            string msg;
                            FirewallService.AddBlockRules(_gameExePath, out msg);

                            _totalSec = _configuredSeconds;
                            _playedTenSec = false;
                            _currentFrame = 0;
                            _state = State.WaitingMatch;
                            SetStatus("Waiting for LoL...", Color.FromArgb(217, 119, 6));
                            _lblTime.Text = FormatTime(_configuredSeconds);
                            _progress.Value = 0;
                            UpdateTrayText("Zhonyas - Waiting for LoL...");

                            _trayIcon.ShowBalloonTip(2500, "Zhonyas", "Match ended. Armed for next match!", ToolTipIcon.Info);
                        }
                        else
                        {
                            _state = State.WaitingClient;
                            SetStatus("Waiting for Client...", Color.FromArgb(100, 116, 139));
                            UpdateTrayText("Zhonyas - Waiting for Client...");
                        }
                    }
                    else
                    {
                        _state = State.Idle;
                        SetStatus("Ready", Color.FromArgb(22, 163, 74));
                        _btnToggle.Text = "Start";
                        _trayToggle.Text = "Start";
                    }
                }
            }
        }

        private void TriggerAutoRetry()
        {
            System.Threading.ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < 8; i++)
                {
                    System.Threading.Thread.Sleep(500);
                    if (RetryClicker.DismissErrorDialog())
                    {
                        break;
                    }
                }
            });
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _monitorTimer.Stop();
                _animTimer.Stop();
                FirewallService.DeleteRules();

                if (_trayIcon != null)
                {
                    _trayIcon.Visible = false;
                    _trayIcon.Dispose();
                }
            }
            catch { }
        }

        private class DurationItem
        {
            public string Text { get; private set; }
            public int Seconds { get; private set; }

            public DurationItem(string text, int seconds)
            {
                Text = text;
                Seconds = seconds;
            }

            public override string ToString()
            {
                return Text;
            }
        }
    }
}
