using System;
using System.Linq;
using System.Windows.Forms;

namespace osu_private
{
    public partial class registrationForm : Form
    {
        public registrationForm()
        {
            InitializeComponent();
            string[] userNames = System.IO.Directory.GetFileSystemEntries(@"./src/user")
                .Select(x => System.IO.Path.GetFileNameWithoutExtension(x)).ToArray();
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
                MessageBox.Show("新規ユーザーのようです！このゲームについて軽く説明します。\n\n1. ユーザー名は、選べばそのユーザーのデータが読み込めて、新しい名前を入力したらユーザーが作成されます\n\n2. 新しいユーザーで、記録をつけなかった場合はそのユーザー名は保存されません！記録をつけるとデータが作成されます！\n\n3. ソフトの仕様上、他人のリプレイなどでも記録が付いてしまいます。注意してください！\n\n4. pp計算式は現在(2023/10/14)のものを使っています。\n\n5. 完全にオフラインで実行可能です。PCにネット環境が無くても動作します！osuでのログインも必要ありません！\n\n6. プライベートサーバーのように、GlobalPPやBonusPPなどの計算を行います！タイムアタックなどに使ってください！\n ※プレイが終わった後、すぐ選曲画面に戻るのでは無く、リザルト画面が表示され、PPが反映されたら戻るようにしてください！ \n\nユーザーの削除については、srcフォルダ内のuserというフォルダに(ユーザー名).jsonというファイルがあるので、消したいユーザー名を選択し、消すだけです。", "ゲームの説明", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (usernameForm.Text == "")
            {
                MessageBox.Show("登録したいユーザー名を入力してください。\nPlease enter a username you wanna set!", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                mainForm mainForm = new mainForm(usernameForm.Text);
                mainForm.Show();
                Hide();
            }
        }
    }
}
