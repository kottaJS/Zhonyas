using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace LoLFirewallManager.Services
{
    public enum MatchState
    {
        Idle,
        WaitingClient,
        WaitingMatch,
        Counting,
        InGame
    }

    public class MatchTracker
    {
        public event Action<MatchState> StateChanged;
        public event Action<int, int> Tick;
        public event Action StasisStarted;
        public event Action Reconnected;
        public event Action ArmedForNextMatch;

        private MatchState _state = MatchState.Idle;
        private Process _proc = null;
        private readonly Stopwatch _sw = new Stopwatch();

        private string _gameExePath;
        private string _clientExePath;

        private int _configuredSeconds = 210;
        private int _totalSec = 210;
        private bool _playedTenSec = false;

        private readonly System.Windows.Forms.Timer _monitorTimer;

        public MatchState State { get { return _state; } }
        public int ConfiguredSeconds
        {
            get { return _configuredSeconds; }
            set { _configuredSeconds = Math.Max(1, value); }
        }
        public bool SoundsEnabled { get; set; }
        public bool AutoRearmEnabled { get; set; }

        public MatchTracker()
        {
            SoundsEnabled = true;
            AutoRearmEnabled = true;

            _gameExePath = LeagueFinder.FindGameExePath();
            _clientExePath = LeagueFinder.FindClientExePath();

            _monitorTimer = new System.Windows.Forms.Timer { Interval = 500 };
            _monitorTimer.Tick += OnMonitorTick;
        }

        public void Start()
        {
            if (!System.IO.File.Exists(_gameExePath))
                _gameExePath = LeagueFinder.FindGameExePath();
            if (!System.IO.File.Exists(_clientExePath))
                _clientExePath = LeagueFinder.FindClientExePath();

            _playedTenSec = false;
            _proc = null;

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
                if (SoundsEnabled) SoundAlert.PlayZhonyaIn();

                _totalSec = Math.Max(1, _configuredSeconds - alreadyElapsed);
                _state = MatchState.Counting;
                _sw.Restart();
                if (StateChanged != null) StateChanged(_state);
                if (StasisStarted != null) StasisStarted();
            }
            else
            {
                Process[] clientList = Process.GetProcessesByName("LeagueClient");
                if (clientList.Length > 0)
                {
                    string msg;
                    FirewallService.AddBlockRules(_gameExePath, out msg);
                    _totalSec = _configuredSeconds;
                    _state = MatchState.WaitingMatch;
                    if (StateChanged != null) StateChanged(_state);
                }
                else
                {
                    _totalSec = _configuredSeconds;
                    _state = MatchState.WaitingClient;
                    if (StateChanged != null) StateChanged(_state);
                }
            }

            _monitorTimer.Start();
        }

        public void Stop()
        {
            _monitorTimer.Stop();
            _sw.Reset();
            FirewallService.DeleteRules();

            bool wasInStasis = (_state == MatchState.Counting);
            _state = MatchState.Idle;

            if (wasInStasis && SoundsEnabled)
            {
                SoundAlert.PlayZhonyaOut();
            }

            if (StateChanged != null) StateChanged(_state);
        }

        public void ForceReconnect()
        {
            FirewallService.DeleteRules();
            bool wasInStasis = (_state == MatchState.Counting);

            if (wasInStasis)
            {
                _state = MatchState.InGame;
                if (SoundsEnabled) SoundAlert.PlayZhonyaOut();
                if (StateChanged != null) StateChanged(_state);
                if (Reconnected != null) Reconnected();
                TriggerAutoRetry();
            }
            else
            {
                _monitorTimer.Stop();
                _sw.Reset();
                _state = MatchState.Idle;
                if (StateChanged != null) StateChanged(_state);
            }
        }

        private void OnMonitorTick(object sender, EventArgs e)
        {
            if (_state == MatchState.WaitingClient)
            {
                if (Process.GetProcessesByName("LeagueClient").Length > 0 || Process.GetProcessesByName("League of Legends").Length > 0)
                {
                    string msg;
                    FirewallService.AddBlockRules(_gameExePath, out msg);
                    _state = MatchState.WaitingMatch;
                    if (StateChanged != null) StateChanged(_state);
                }
                return;
            }

            if (_state == MatchState.WaitingMatch)
            {
                if (Process.GetProcessesByName("LeagueClient").Length == 0 && Process.GetProcessesByName("League of Legends").Length == 0)
                {
                    FirewallService.DeleteRules();
                    _state = MatchState.WaitingClient;
                    if (StateChanged != null) StateChanged(_state);
                    return;
                }

                Process[] list = Process.GetProcessesByName("League of Legends");
                if (list.Length > 0)
                {
                    _proc = list[0];
                    _state = MatchState.Counting;
                    _totalSec = _configuredSeconds;
                    if (SoundsEnabled) SoundAlert.PlayZhonyaIn();
                    _sw.Restart();
                    if (StateChanged != null) StateChanged(_state);
                    if (StasisStarted != null) StasisStarted();
                }
                return;
            }

            if (_state == MatchState.Counting)
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
                    Stop();
                    return;
                }

                int elapsed = (int)_sw.Elapsed.TotalSeconds;
                int remaining = Math.Max(0, _totalSec - elapsed);
                int pct = Math.Min(100, (int)(((double)elapsed / _configuredSeconds) * 100));

                if (Tick != null) Tick(remaining, pct);

                if (remaining > 0)
                {
                    if (remaining <= 10 && !_playedTenSec)
                    {
                        _playedTenSec = true;
                        if (SoundsEnabled) SoundAlert.PlayTenSecondsWarning();
                    }
                }
                else
                {
                    _sw.Reset();
                    FirewallService.DeleteRules();
                    _state = MatchState.InGame;
                    if (SoundsEnabled) SoundAlert.PlayZhonyaOut();
                    if (StateChanged != null) StateChanged(_state);
                    if (Reconnected != null) Reconnected();
                    TriggerAutoRetry();
                }
                return;
            }

            if (_state == MatchState.InGame)
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
                    if (AutoRearmEnabled)
                    {
                        if (Process.GetProcessesByName("LeagueClient").Length > 0)
                        {
                            string msg;
                            FirewallService.AddBlockRules(_gameExePath, out msg);
                            _totalSec = _configuredSeconds;
                            _playedTenSec = false;
                            _state = MatchState.WaitingMatch;
                            if (StateChanged != null) StateChanged(_state);
                            if (ArmedForNextMatch != null) ArmedForNextMatch();
                        }
                        else
                        {
                            _state = MatchState.WaitingClient;
                            if (StateChanged != null) StateChanged(_state);
                        }
                    }
                    else
                    {
                        _state = MatchState.Idle;
                        if (StateChanged != null) StateChanged(_state);
                    }
                }
            }
        }

        private void TriggerAutoRetry()
        {
            ThreadPool.QueueUserWorkItem(_ =>
            {
                for (int i = 0; i < 8; i++)
                {
                    Thread.Sleep(500);
                    if (RetryClicker.DismissErrorDialog())
                        break;
                }
            });
        }
    }
}

