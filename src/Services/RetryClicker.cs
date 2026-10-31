using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace LoLFirewallManager.Services
{
    public static class RetryClicker
    {
        [DllImport("user32.dll")]
        private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);
        private delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool EnumChildWindows(IntPtr hWndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        private static extern bool PostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

        private const uint BM_CLICK = 0x00F5;
        private const uint WM_KEYDOWN = 0x0100;
        private const uint WM_KEYUP = 0x0101;
        private const byte VK_RETURN = 0x0D;

        public static bool DismissErrorDialog()
        {
            try
            {
                IntPtr hTarget = IntPtr.Zero;

                EnumWindows((hWnd, lParam) =>
                {
                    StringBuilder sb = new StringBuilder(256);
                    GetWindowText(hWnd, sb, 256);
                    string title = sb.ToString();

                    if (!string.IsNullOrEmpty(title))
                    {
                        if (title.IndexOf("Error de conexi", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            title.IndexOf("Connection Error", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            title.IndexOf("Error de conex", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            hTarget = hWnd;
                            return false;
                        }
                    }
                    return true;
                }, IntPtr.Zero);

                if (hTarget != IntPtr.Zero)
                {
                    EnumChildWindows(hTarget, (hChild, lParam) =>
                    {
                        StringBuilder text = new StringBuilder(256);
                        GetWindowText(hChild, text, 256);
                        string btnStr = text.ToString().Trim();

                        if (btnStr.IndexOf("Reintentar", StringComparison.OrdinalIgnoreCase) >= 0 ||
                            btnStr.IndexOf("Retry", StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            SendMessage(hChild, BM_CLICK, IntPtr.Zero, IntPtr.Zero);
                        }
                        return true;
                    }, IntPtr.Zero);

                    SetForegroundWindow(hTarget);
                    Thread.Sleep(80);

                    for (int i = 0; i < 4; i++)
                    {
                        PostMessage(hTarget, WM_KEYDOWN, (IntPtr)VK_RETURN, IntPtr.Zero);
                        PostMessage(hTarget, WM_KEYUP, (IntPtr)VK_RETURN, IntPtr.Zero);

                        SendKeys.SendWait("{ENTER}");

                        Thread.Sleep(120);
                    }

                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}
