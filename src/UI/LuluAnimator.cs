using System;
using System.Drawing;
using System.Windows.Forms;
using LoLFirewallManager.Services;

namespace LoLFirewallManager.UI
{
    public class LuluAnimator : IDisposable
    {
        private readonly PictureBox _pictureBox;
        private readonly Bitmap[] _frames;
        private readonly Timer _timer;
        private int _currentFrame;
        private MatchState _state;
        private bool _enabled;

        public LuluAnimator(PictureBox pictureBox, Bitmap[] frames, bool enabled)
        {
            _pictureBox = pictureBox;
            _frames = frames;
            _enabled = enabled;
            _timer = new Timer { Interval = 75 };
            _timer.Tick += OnTick;

            if (_enabled)
                _timer.Start();
            else
                UpdateStaticImage();
        }

        public bool Enabled
        {
            get { return _enabled; }
            set
            {
                _enabled = value;
                if (_enabled)
                {
                    _timer.Start();
                }
                else
                {
                    _timer.Stop();
                    UpdateStaticImage();
                }
            }
        }

        public void SetState(MatchState state)
        {
            _state = state;
            if (state == MatchState.Idle)
                _currentFrame = 0;
            else if (state == MatchState.Counting)
                _currentFrame = 108;
            else if (state == MatchState.InGame)
                _currentFrame = 138;

            if (!_enabled)
                UpdateStaticImage();
        }

        private void UpdateStaticImage()
        {
            if (_frames == null || _frames.Length == 0) return;
            if (_state == MatchState.Counting && _frames.Length > 108)
                _pictureBox.Image = _frames[108];
            else if (_state == MatchState.InGame && _frames.Length > 138)
                _pictureBox.Image = _frames[138];
            else
                _pictureBox.Image = _frames[0];
        }

        private void OnTick(object sender, EventArgs e)
        {
            if (!_enabled || _frames == null || _frames.Length == 0) return;

            if (_state == MatchState.Counting)
            {
                if (_currentFrame < 108 || _currentFrame > 119) _currentFrame = 108;
                else if (_currentFrame < 115) _currentFrame++;
                else { _currentFrame++; if (_currentFrame > 119) _currentFrame = 115; }
            }
            else if (_currentFrame >= 138 && _currentFrame < _frames.Length)
            {
                _currentFrame++;
                if (_currentFrame >= _frames.Length) _currentFrame = 0;
            }
            else
            {
                _currentFrame++;
                if (_currentFrame > 107) _currentFrame = 0;
            }

            if (_currentFrame >= 0 && _currentFrame < _frames.Length)
                _pictureBox.Image = _frames[_currentFrame];
        }

        public void Dispose()
        {
            _timer.Stop();
            _timer.Dispose();
        }
    }
}

