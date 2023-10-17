using System;
using System.Diagnostics;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;

namespace osu_private
{
    internal static class Program
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (Process.GetProcessesByName("osu!private").Length == 2)
            {
                MessageBox.Show("osu!privateは既に起動しています。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            System.Globalization.CultureInfo.CurrentCulture = new System.Globalization.CultureInfo("en-us");
            System.Globalization.CultureInfo.CurrentUICulture = new System.Globalization.CultureInfo("en-us");
            Application.EnableVisualStyles();
            Application.ApplicationExit += (sender, args) =>
            {
                try
                {
                    mainForm.osuPrivate.Kill();
                    if (mainForm.gosumemoryLaunched) mainForm.gosumemory.Kill();
                    Application.Exit();
                }
                catch
                {
                    Application.Exit();
                }
            };
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new registrationForm());
        }
    }
}
