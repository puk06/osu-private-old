using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Octokit;

namespace osu_private
{
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {
            GithubUpdateChecker();
            InitializeComponent();
            string[] userNames = Directory.GetFileSystemEntries(@"./src/user")
                .Select(Path.GetFileNameWithoutExtension).ToArray();
            foreach (var user in userNames)
            {
                usernameForm.Items.Add(user);
            }

            if (usernameForm.Items.Count != 0)
            {
                usernameForm.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("新規ユーザーのようです！このゲームについて軽く説明します。\n\n1. ユーザー名は、選べばそのユーザーのデータが読み込めて、新しい名前を入力したらユーザーが作成されます\n\n2. 新しいユーザーで、記録をつけなかった場合はそのユーザー名は保存されません！記録をつけるとデータが作成されます！\n\n3. ソフトの仕様上、他人のリプレイなどでも記録が付いてしまいます。注意してください！\n\n4. pp計算式は2023/10/14のものを使っています。\n\n5. 完全にオフラインで実行可能です。PCにネット環境が無くても動作します！osuでのログインも必要ありません！\n\n6. プライベートサーバーのように、GlobalPPやBonusPPなどの計算を行います！タイムアタックなどに使ってください！\n ※プレイが終わった後、すぐ選曲画面に戻るのでは無く、リザルト画面が表示され、PPが反映されたら戻るようにしてください！ \n\nユーザーの削除については、srcフォルダ内のuserというフォルダに(ユーザー名).jsonというファイルがあるので、消したいユーザー名を選択し、消すだけです。", "ゲームの説明", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (usernameForm.Text == "")
            {
                MessageBox.Show("登録したいユーザー名を入力してください。\nPlease enter a username you wanna set!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (IsInvalidFileName(usernameForm.Text))
            {
                MessageBox.Show("ユーザー名に使用できない文字が含まれています。\nInvalid characters are included in the username!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (usernameForm.Text.Length > 20)
            {
                MessageBox.Show("ユーザー名はエラーを防ぐために20文字以内で入力してください。\nPlease enter a username within 20 characters!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MainForm mainForm = new MainForm(usernameForm.Text);
                mainForm.Show();
                Hide();
            }
        }

        private static async void GithubUpdateChecker()
        {
            const string softwareReleasesLatest = "https://github.com/puk06/osu-private/releases/latest";
            StreamReader currentVersion = new StreamReader("version");
            string currentVersionString = await currentVersion.ReadToEndAsync();
            currentVersion.Close();
            const string owner = "puk06";
            const string repo = "osu-private";
            var githubClient = new GitHubClient(new ProductHeaderValue("osu-private"));

            try
            {
                var latestRelease = await githubClient.Repository.Release.GetLatest(owner, repo);
                if (latestRelease.Name == currentVersionString) return;
                DialogResult result = MessageBox.Show($"最新バージョンがあります！\n\n現在: {currentVersionString} \n更新後: {latestRelease.TagName}\n\nダウンロードページを開きますか？", "アップデートのお知らせ", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                if (result == DialogResult.Yes) Process.Start(softwareReleasesLatest);
            }
            catch
            {
                MessageBox.Show("アップデートチェック中にエラーが発生しました。", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private static bool IsInvalidFileName(string fileName)
        {
            char[] invalidChars = Path.GetInvalidFileNameChars();
            return invalidChars.Any(fileName.Contains);
        }
    }
}
