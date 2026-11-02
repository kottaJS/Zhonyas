using System;
using System.Drawing;
using System.Windows.Forms;
using LoLFirewallManager.Localization;

namespace LoLFirewallManager.UI
{
    public static class HelpDialogs
    {
        public static void ShowGuide(IWin32Window owner)
        {
            MessageBox.Show(owner,
                I18n.Current.GuideContent,
                I18n.Current.MenuGuide,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void ShowVanguardInfo(IWin32Window owner)
        {
            MessageBox.Show(owner,
                I18n.Current.VanguardContent,
                I18n.Current.MenuVanguard,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static void ShowAbout(IWin32Window owner)
        {
            MessageBox.Show(owner,
                I18n.Current.AboutContent,
                I18n.Current.MenuAbout,
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);
        }

        public static bool ConfirmStop(IWin32Window owner, bool autoRearm, bool inStasis, ref bool suppressWarning, Action onSaveSettings)
        {
            if (suppressWarning) return true;
            if (!autoRearm && !inStasis) return true;

            using (Form prompt = new Form())
            {
                prompt.Text = I18n.Current.BtnStop + " Zhonyas";
                prompt.ClientSize = new Size(330, 165);
                prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
                prompt.StartPosition = FormStartPosition.CenterParent;
                prompt.MaximizeBox = false;
                prompt.MinimizeBox = false;
                prompt.ShowInTaskbar = false;
                prompt.Font = new Font("Segoe UI", 9F);
                prompt.BackColor = Color.FromArgb(248, 249, 250);

                string msgText;
                if (autoRearm && inStasis)
                    msgText = I18n.Current.StopMsgBoth;
                else if (autoRearm)
                    msgText = I18n.Current.StopMsgAutoRearm;
                else
                    msgText = I18n.Current.StopMsgStasis;

                Label lblMsg = new Label
                {
                    Text = msgText,
                    Location = new Point(16, 16),
                    Size = new Size(298, 55),
                    ForeColor = Color.FromArgb(15, 23, 42)
                };

                CheckBox chkDontShow = new CheckBox
                {
                    Text = I18n.Current.DontShowAgain,
                    Location = new Point(18, 80),
                    AutoSize = true,
                    ForeColor = Color.FromArgb(100, 116, 139)
                };

                Button btnOk = new Button
                {
                    Text = I18n.Current.BtnStop,
                    DialogResult = DialogResult.OK,
                    Location = new Point(150, 118),
                    Size = new Size(80, 30),
                    UseVisualStyleBackColor = true
                };

                Button btnCancel = new Button
                {
                    Text = I18n.Current.BtnCancel,
                    DialogResult = DialogResult.Cancel,
                    Location = new Point(236, 118),
                    Size = new Size(80, 30),
                    UseVisualStyleBackColor = true
                };

                prompt.Controls.Add(lblMsg);
                prompt.Controls.Add(chkDontShow);
                prompt.Controls.Add(btnOk);
                prompt.Controls.Add(btnCancel);
                prompt.AcceptButton = btnOk;
                prompt.CancelButton = btnCancel;

                if (prompt.ShowDialog(owner) == DialogResult.OK)
                {
                    if (chkDontShow.Checked)
                    {
                        suppressWarning = true;
                        if (onSaveSettings != null) onSaveSettings();
                    }
                    return true;
                }
                return false;
            }
        }
    }
}
