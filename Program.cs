using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace osu_private
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        private static void Main()
        {
            if (Process.GetProcessesByName("osu!private").Length == 2)
            {
                MessageBox.Show("osu!privateは既に起動しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            CultureInfo.CurrentCulture = new CultureInfo("en-us");
            CultureInfo.CurrentUICulture = new CultureInfo("en-us");
            Application.EnableVisualStyles();
            Application.ApplicationExit += (sender, args) =>
            {
                try
                {
                    if (MainForm.Gosumemory != null && !MainForm.Gosumemory.HasExited) MainForm.Gosumemory.Kill();
                }
                catch (Exception error)
                {
                    MessageBox.Show($"gosumemoryの終了に失敗しました。\nエラー内容: {error}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                try
                {
                    if (MainForm.OsuPrivate != null && !MainForm.OsuPrivate.HasExited) MainForm.OsuPrivate.Kill();
                }
                catch (Exception error)
                {
                    MessageBox.Show($"osu!private.jsの終了に失敗しました。\nエラー内容: {error}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new RegistrationForm());
        }
    }
}
