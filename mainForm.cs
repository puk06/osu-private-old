using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace osu_private
{
    public partial class mainForm : Form
    {
        public static Process osuPrivate;
        public static Process gosumemory;
        public static bool gosumemoryLaunched;
        public static string username;
        public static Dictionary<string, double> globalPP = new Dictionary<string, double>()
        {
            {"osu", 0},
            {"taiko", 0},
            {"catch", 0},
            {"mania", 0}
        };
        public static Dictionary<string, double> globalACC = new Dictionary<string, double>()
        {
            {"osu", 0},
            {"taiko", 0},
            {"catch", 0},
            {"mania", 0}
        };
        public static Dictionary<string, double> bonusPP = new Dictionary<string, double>()
        {
            {"osu", 0},
            {"taiko", 0},
            {"catch", 0},
            {"mania", 0}
        };
        private DateTime lastWriteTime;
        private static bool firstLaunch = true;
        public mainForm(string username)
        {
            mainForm.username = username;
            launchSoftware(username);
            InitializeComponent();
            modeValue.SelectedIndex = 0;
            try
            {
                StreamReader userdata = new StreamReader($"./src/user/{username}.json");
                string userdataString = userdata.ReadToEnd();
                userdata.Close();
                JObject userdataJson = JObject.Parse(userdataString);
                globalPPValue.Text = Math.Round((double)userdataJson["globalPP"][convertMode(modeValue.Text)], 2) + "pp";
                accValue.Text = Math.Round((double)userdataJson["globalACC"][convertMode(modeValue.Text)], 2) + "%";
                BonusPPValue.Text = Math.Round((double)userdataJson["bonusPP"][convertMode(modeValue.Text)], 2) + "pp";
                globalPP["osu"] = (double)userdataJson["globalPP"]["osu"];
                globalPP["taiko"] = (double)userdataJson["globalPP"]["taiko"];
                globalPP["catch"] = (double)userdataJson["globalPP"]["catch"];
                globalPP["mania"] = (double)userdataJson["globalPP"]["mania"];
                globalACC["osu"] = (double)userdataJson["globalACC"]["osu"];
                globalACC["taiko"] = (double)userdataJson["globalACC"]["taiko"];
                globalACC["catch"] = (double)userdataJson["globalACC"]["catch"];
                globalACC["mania"] = (double)userdataJson["globalACC"]["mania"];
                bonusPP["osu"] = (double)userdataJson["bonusPP"]["osu"];
                bonusPP["taiko"] = (double)userdataJson["bonusPP"]["taiko"];
                bonusPP["catch"] = (double)userdataJson["bonusPP"]["catch"];
                bonusPP["mania"] = (double)userdataJson["bonusPP"]["mania"];
            }
            catch (Exception)
            {
                globalPPValue.Text = "0pp";
                accValue.Text = "0%";
                BonusPPValue.Text = "0pp";
            }

            string filePathToWatch = $"./src/user/{username}.json";
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(filePathToWatch);
            watcher.Filter = Path.GetFileName(filePathToWatch);
            watcher.Changed += (sender, e) =>
            {
                try
                {
                    if (DateTime.Now - lastWriteTime < TimeSpan.FromSeconds(2) && !firstLaunch) return;
                    if (firstLaunch)
                    {
                        firstLaunch = false;
                        return;
                    }

                    lastWriteTime = DateTime.Now;
                    Invoke((MethodInvoker)delegate
                    {
                        StreamReader userdataRecent = new StreamReader($"./src/user/{username}.json");
                        string userdataStringRecent = userdataRecent.ReadToEnd();
                        userdataRecent.Close();
                        JObject userdataJsonRecent = JObject.Parse(userdataStringRecent);
                        modeValue.SelectedIndex = (int)userdataJsonRecent["lastGamemode"];
                        globalPPValue.Text = Math.Round((double)userdataJsonRecent["globalPP"][convertMode(modeValue.Text)], 2) + "pp";
                        accValue.Text = Math.Round((double)userdataJsonRecent["globalACC"][convertMode(modeValue.Text)], 2) + "%";
                        BonusPPValue.Text = Math.Round((double)userdataJsonRecent["bonusPP"][convertMode(modeValue.Text)], 2) + "pp";
                        listBox1.Items.Clear();
                        if (userdataJsonRecent["pp"][convertMode(modeValue.Text)].Count() == 0)
                        {
                            listBox1.Items.Add("No plays.");
                            return;
                        }

                        listBox1.Items.Add("-----------------------------------------------------------------------------------------------------------------");
                        // 以下、UIコントロールへのアクセスを修正
                        for (int i = 0; i < userdataJsonRecent["pp"][convertMode(modeValue.Text)].Count(); i++)
                        {
                            string itemTitle = $"Title: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["title"]}";
                            string versionName = $"Mapper: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["mapper"]}   Difficulty: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["version"]}";
                            string hitsInfo = "";
                            if (convertMode(modeValue.Text) == "osu" || convertMode(modeValue.Text) == "catch")
                            {
                                hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                            }
                            else if (convertMode(modeValue.Text) == "taiko")
                            {
                                hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                            }
                            else if (convertMode(modeValue.Text) == "mania")
                            {
                                hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["geki"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["katu"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                            }
                            else
                            {
                                hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                            }

                            if (itemTitle.Length > 110)
                            {
                                itemTitle = itemTitle.Substring(0, 110) + "...";
                            }

                            if (versionName.Length > 110)
                            {
                                versionName = versionName.Substring(0, 110) + "...";
                            }

                            string resultInfo = $"Score: {string.Format("{0:#,0}", (int)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["score"])} / {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["combo"]}x   {hitsInfo}";
                            listBox1.Items.Add($"#{i + 1}");
                            listBox1.Items.Add(itemTitle);
                            listBox1.Items.Add(versionName);
                            listBox1.Items.Add(resultInfo);
                            listBox1.Items.Add($"Mod: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["mods"]}   Accuracy: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["acc"]}%   PP: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["pp"]}pp");
                            listBox1.Items.Add("-----------------------------------------------------------------------------------------------------------------");
                        }
                        changePPValue.Text = $"{(Math.Round((double)userdataJsonRecent["globalPP"][convertMode(modeValue.Text)] - globalPP[convertMode(modeValue.Text)], 2) >= 0 ? "+" : "-")} {Math.Abs(Math.Round((double)userdataJsonRecent["globalPP"][convertMode(modeValue.Text)] - globalPP[convertMode(modeValue.Text)], 2))}pp";
                        changePPValue.ForeColor = Math.Round((double)userdataJsonRecent["globalPP"][convertMode(modeValue.Text)] - globalPP[convertMode(modeValue.Text)], 2) >= 0 ? System.Drawing.Color.ForestGreen : System.Drawing.Color.Red;
                        changeACCValue.Text = $"{(Math.Round((double)userdataJsonRecent["globalACC"][convertMode(modeValue.Text)] - globalACC[convertMode(modeValue.Text)], 2) >= 0 ? "+" : "-")} {Math.Abs(Math.Round((double)userdataJsonRecent["globalACC"][convertMode(modeValue.Text)] - globalACC[convertMode(modeValue.Text)], 2))}%";
                        changeACCValue.ForeColor = Math.Round((double)userdataJsonRecent["globalACC"][convertMode(modeValue.Text)] - globalACC[convertMode(modeValue.Text)], 2) >= 0 ? System.Drawing.Color.ForestGreen : System.Drawing.Color.Red;
                        changeBonusPPValue.Text = $"{(Math.Round((double)userdataJsonRecent["bonusPP"][convertMode(modeValue.Text)] - bonusPP[convertMode(modeValue.Text)], 2) >= 0 ? "+" : "-")} {Math.Abs(Math.Round((double)userdataJsonRecent["bonusPP"][convertMode(modeValue.Text)] - bonusPP[convertMode(modeValue.Text)], 2))}pp";
                        changeBonusPPValue.ForeColor = Math.Round((double)userdataJsonRecent["bonusPP"][convertMode(modeValue.Text)] - bonusPP[convertMode(modeValue.Text)], 2) >= 0 ? System.Drawing.Color.ForestGreen : System.Drawing.Color.Red;
                        globalPP["osu"] = (double)userdataJsonRecent["globalPP"]["osu"];
                        globalPP["taiko"] = (double)userdataJsonRecent["globalPP"]["taiko"];
                        globalPP["catch"] = (double)userdataJsonRecent["globalPP"]["catch"];
                        globalPP["mania"] = (double)userdataJsonRecent["globalPP"]["mania"];
                        globalACC["osu"] = (double)userdataJsonRecent["globalACC"]["osu"];
                        globalACC["taiko"] = (double)userdataJsonRecent["globalACC"]["taiko"];
                        globalACC["catch"] = (double)userdataJsonRecent["globalACC"]["catch"];
                        globalACC["mania"] = (double)userdataJsonRecent["globalACC"]["mania"];
                        bonusPP["osu"] = (double)userdataJsonRecent["bonusPP"]["osu"];
                        bonusPP["taiko"] = (double)userdataJsonRecent["bonusPP"]["taiko"];
                        bonusPP["catch"] = (double)userdataJsonRecent["bonusPP"]["catch"];
                        bonusPP["mania"] = (double)userdataJsonRecent["bonusPP"]["mania"];

                        Timer timer = new Timer();
                        timer.Interval = 5000;
                        timer.Tick += (sendertick, etick) =>
                        {
                            changePPValue.Text = "";
                            changeACCValue.Text = "";
                            changeBonusPPValue.Text = "";
                            timer.Stop();
                        };
                        timer.Start();
                    });
                }
                catch (Exception)
                {
                    // エラーメッセージを表示する場合もInvokeを使用
                    this.Invoke((MethodInvoker)delegate
                    {
                        globalPPValue.Text = "0pp";
                        accValue.Text = "0%";
                        BonusPPValue.Text = "0pp";
                    });
                }
            };
            watcher.EnableRaisingEvents = true;
        }

        public static void launchSoftware(string username)
        {
            try
            {
                if (Process.GetProcessesByName("gosumemory").Length == 0)
                {
                    try
                    {
                        if (!File.Exists("./src/gosumemory/gosumemory.exe"))
                        {
                            DialogResult result = MessageBox.Show("Gosumemoryがフォルダ内から見つかりませんでした。\nGithubからダウンロードしますか？",
                                "エラー", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                            if (result == DialogResult.Yes)
                            {
                                //webブラウザでダウンロードページを開く
                                MessageBox.Show(
                                    "ダウンロードページをwebブラウザで開きます。\nインストール方法: ダウンロードしたフォルダを開き、osu!trainer/src/gosumemory/gosumemory.exeとなるように配置する。",
                                    "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                Process.Start("https://github.com/l3lackShark/gosumemory/releases/");
                                return;
                            }
                            Application.Exit();
                        }

                        //launch gosumemory
                        gosumemoryLaunched = true;
                        gosumemory = new Process();
                        gosumemory.StartInfo.FileName = "./src/gosumemory/gosumemory.exe";
                        gosumemory.StartInfo.CreateNoWindow = true;
                        gosumemory.StartInfo.UseShellExecute = false;
                        gosumemory.Start();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Gosumemoryの起動に失敗しました。\nエラー内容:{ex}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "\"./src/nodejs/node.exe\"",
                    Arguments = $"\"./src/osu!private.js\" \"{username}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                osuPrivate = new Process();
                osuPrivate.StartInfo = startInfo;
                osuPrivate.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"osu!privateの起動に失敗しました。\nエラー内容:{ex}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void mainForm_FormClosed(object sender, EventArgs e)
        {
            try
            {
                osuPrivate.Kill();
                if (gosumemoryLaunched) gosumemory.Kill();
                Application.Exit();
            }
            catch (Exception)
            {
                Application.Exit();
            }
        }

        public static string convertMode(string value)
        {
            if (value == "osu!standard")
            {
                return "osu";
            }

            if (value == "osu!taiko")
            {
                return "taiko";
            }

            if (value == "osu!catch")
            {
                return "catch";
            }

            if (value == "osu!mania")
            {
                return "mania";
            }

            return "osu";
        }

        private void modeValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                changePPValue.Text = "";
                changeACCValue.Text = "";
                changeBonusPPValue.Text = "";
                StreamReader userdataRecent = new StreamReader($"./src/user/{username}.json");
                string userdataStringRecent = userdataRecent.ReadToEnd();
                userdataRecent.Close();
                JObject userdataJsonRecent = JObject.Parse(userdataStringRecent);
                globalPPValue.Text = Math.Round((double)userdataJsonRecent["globalPP"][convertMode(modeValue.Text)], 2) + "pp";
                accValue.Text = Math.Round((double)userdataJsonRecent["globalACC"][convertMode(modeValue.Text)], 2) + "%";
                BonusPPValue.Text = Math.Round((double)userdataJsonRecent["bonusPP"][convertMode(modeValue.Text)], 2) + "pp";
                listBox1.Items.Clear();
                if (userdataJsonRecent["pp"][convertMode(modeValue.Text)].Count() == 0)
                {
                    listBox1.Items.Add("No plays.");
                    return;
                }

                listBox1.Items.Add("-----------------------------------------------------------------------------------------------------------------");

                for (int i = 0; i < userdataJsonRecent["pp"][convertMode(modeValue.Text)].Count(); i++)
                {
                    string itemTitle = $"Title: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["title"]}";
                    string versionName = $"Mapper: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["mapper"]}   Difficulty: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["version"]}";
                    string hitsInfo = "";
                    if (convertMode(modeValue.Text) == "osu" || convertMode(modeValue.Text) == "catch")
                    {
                        hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                    }
                    else if (convertMode(modeValue.Text) == "taiko")
                    {
                        hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                    }
                    else if (convertMode(modeValue.Text) == "mania")
                    {
                        hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["geki"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["katu"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                    }
                    else
                    {
                        hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["miss"]}}}";
                    }

                    string resultInfo = $"Score: {string.Format("{0:#,0}", (int)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["score"])} / {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["combo"]}x   {hitsInfo}";

                    if (itemTitle.Length > 110)
                    {
                        itemTitle = itemTitle.Substring(0, 110) + "...";
                    }

                    if (versionName.Length > 110)
                    {
                        versionName = versionName.Substring(0, 110) + "...";
                    }
                    listBox1.Items.Add($"#{i + 1}");
                    listBox1.Items.Add(itemTitle);
                    listBox1.Items.Add(versionName);
                    listBox1.Items.Add(resultInfo);
                    listBox1.Items.Add($"Mod: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["mods"]}   Accuracy: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["acc"]}%   PP: {(string)userdataJsonRecent["pp"][convertMode(modeValue.Text)][i]["pp"]}pp");
                    listBox1.Items.Add("-----------------------------------------------------------------------------------------------------------------");
                }
            }
            catch (Exception)
            {
                listBox1.Items.Clear();
                listBox1.Items.Add("No plays.");
                globalPPValue.Text = "0pp";
                accValue.Text = "0%";
                BonusPPValue.Text = "0pp";
            }
        }

        private void deleteScore_Click(object sender, EventArgs e)
        {
            if (!File.Exists($"./src/user/{username}.json"))
            {
                MessageBox.Show("No scores found! \n The delete function will not be enabled until you create a user and create at least one score!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new deleteForm().ShowDialog();

        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                listBox1.ClearSelected();
            }
        }
    }
}
