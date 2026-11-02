using System;
using System.Drawing;
using System.Windows.Forms;
using LoLFirewallManager.Localization;

namespace LoLFirewallManager.UI
{
    public class TrayManager : IDisposable
    {
        private readonly NotifyIcon _trayIcon;
        private readonly ContextMenuStrip _trayMenu;
        private readonly ToolStripMenuItem _trayOpen;
        private readonly ToolStripMenuItem _trayToggle;
        private readonly ToolStripMenuItem _trayReconnect;
        private readonly ToolStripMenuItem _trayExit;

        public TrayManager(Icon appIcon, Action onRestore, Action onToggle, Action onReconnect, Action onExit)
        {
            _trayMenu = new ContextMenuStrip();

            _trayOpen = new ToolStripMenuItem(I18n.Current.TrayOpen, null, (s, e) => onRestore())
            {
                Font = new Font(_trayMenu.Font, FontStyle.Bold)
            };
            _trayToggle = new ToolStripMenuItem(I18n.Current.BtnStart, null, (s, e) => onToggle());
            _trayReconnect = new ToolStripMenuItem(I18n.Current.BtnReconnect, null, (s, e) => onReconnect());
            _trayExit = new ToolStripMenuItem(I18n.Current.MenuExit, null, (s, e) => onExit());

            _trayMenu.Items.Add(_trayOpen);
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add(_trayToggle);
            _trayMenu.Items.Add(_trayReconnect);
            _trayMenu.Items.Add(new ToolStripSeparator());
            _trayMenu.Items.Add(_trayExit);

            _trayIcon = new NotifyIcon
            {
                Icon = appIcon,
                Text = "Zhonyas - " + I18n.Current.StatusReady,
                ContextMenuStrip = _trayMenu,
                Visible = true
            };

            _trayIcon.MouseClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) onRestore();
            };
            _trayIcon.MouseDoubleClick += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) onRestore();
            };
        }

        public void UpdateText(string text)
        {
            try
            {
                if (text.Length > 63) text = text.Substring(0, 63);
                _trayIcon.Text = text;
            }
            catch { }
        }

        public void UpdateToggleText(string text)
        {
            _trayToggle.Text = text;
        }

        public void ShowBalloon(int timeout, string title, string text, ToolTipIcon icon)
        {
            _trayIcon.ShowBalloonTip(timeout, title, text, icon);
        }

        public void UpdateLocalizedMenu()
        {
            _trayOpen.Text = I18n.Current.TrayOpen;
            _trayReconnect.Text = I18n.Current.BtnReconnect;
            _trayExit.Text = I18n.Current.MenuExit;
        }

        public void Dispose()
        {
            if (_trayIcon != null)
            {
                _trayIcon.Visible = false;
                _trayIcon.Dispose();
            }
            if (_trayMenu != null)
            {
                _trayMenu.Dispose();
            }
        }
    }
}

