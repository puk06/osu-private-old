using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace osu_private
{
    public partial class MainForm : Form
    {
        public static Process OsuPrivate;
        public static Process Gosumemory;
        public static string Username;

        private static readonly Dictionary<string, double> GlobalPp = new Dictionary<string, double>
        {
            {"osu", 0},
            {"taiko", 0},
            {"catch", 0},
            {"mania", 0}
        };

        private static readonly Dictionary<string, double> GlobalAcc = new Dictionary<string, double>
        {
            {"osu", 0},
            {"taiko", 0},
            {"catch", 0},
            {"mania", 0}
        };

        private static readonly Dictionary<string, double> BonusPp = new Dictionary<string, double>
        {
            {"osu", 0},
            {"taiko", 0},
            {"catch", 0},
            {"mania", 0}
        };

        private long _lastTick;
        private bool _firstTime = true;
        
        public MainForm(string username)
        {
            Username = username;
            LaunchSoftware(username);
            InitializeComponent();
            modeValue.SelectedIndex = 0;
            try
            {
                StreamReader userdata = new StreamReader($"./src/user/{username}.json");
                string userdataString = userdata.ReadToEnd();
                userdata.Close();
                JObject userdataJson = JObject.Parse(userdataString);
                globalPPValue.Text = Math.Round((double)userdataJson["globalPP"][ConvertMode(modeValue.Text)], 2) + "pp";
                accValue.Text = Math.Round((double)userdataJson["globalACC"][ConvertMode(modeValue.Text)], 2) + "%";
                BonusPPValue.Text = Math.Round((double)userdataJson["bonusPP"][ConvertMode(modeValue.Text)], 2) + "pp";
                playtimeValue.Text = (string)userdataJson["playtime"][ConvertMode(modeValue.Text)];
                playcountValue.Text = (string)userdataJson["playcount"][ConvertMode(modeValue.Text)];
                GlobalPp["osu"] = (double)userdataJson["globalPP"]["osu"];
                GlobalPp["taiko"] = (double)userdataJson["globalPP"]["taiko"];
                GlobalPp["catch"] = (double)userdataJson["globalPP"]["catch"];
                GlobalPp["mania"] = (double)userdataJson["globalPP"]["mania"];
                GlobalAcc["osu"] = (double)userdataJson["globalACC"]["osu"];
                GlobalAcc["taiko"] = (double)userdataJson["globalACC"]["taiko"];
                GlobalAcc["catch"] = (double)userdataJson["globalACC"]["catch"];
                GlobalAcc["mania"] = (double)userdataJson["globalACC"]["mania"];
                BonusPp["osu"] = (double)userdataJson["bonusPP"]["osu"];
                BonusPp["taiko"] = (double)userdataJson["bonusPP"]["taiko"];
                BonusPp["catch"] = (double)userdataJson["bonusPP"]["catch"];
                BonusPp["mania"] = (double)userdataJson["bonusPP"]["mania"];
            }
            catch
            {
                globalPPValue.Text = "0pp";
                accValue.Text = "0%";
                BonusPPValue.Text = "0pp";
                playtimeValue.Text = "0h 0m";
                playcountValue.Text = "0";
            }
            
            Timer timer = new Timer();
            timer.Interval = 5000;
            timer.Tick += (sendertick, etick) =>
            {
                changePPValue.Text = "";
                changeACCValue.Text = "";
                changeBonusPPValue.Text = "";
                timer.Stop();
            };

            string filePathToWatch = $"./src/user/{username}.json";
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = Path.GetDirectoryName(filePathToWatch);
            watcher.Filter = Path.GetFileName(filePathToWatch);
            watcher.Changed += async (sender, e) =>
            {
                try
                {
                    await Task.Delay(100);
                    if (DateTime.Now.Ticks - _lastTick < 10000000 && !_firstTime) return;
                    if (_firstTime) _firstTime = false;
                    _lastTick = DateTime.Now.Ticks;
                    Invoke((MethodInvoker)delegate
                    {
                        StreamReader userdataRecent = new StreamReader($"./src/user/{username}.json");
                        string userdataStringRecent = userdataRecent.ReadToEnd();
                        userdataRecent.Close();
                        errorText.Text = "";
                        JObject userdataJsonRecent = JObject.Parse(userdataStringRecent);
                        modeValue.SelectedIndex = (int)userdataJsonRecent["lastGamemode"];
                        globalPPValue.Text = Math.Round((double)userdataJsonRecent["globalPP"][ConvertMode(modeValue.Text)], 2) + "pp";
                        accValue.Text = Math.Round((double)userdataJsonRecent["globalACC"][ConvertMode(modeValue.Text)], 2) + "%";
                        BonusPPValue.Text = Math.Round((double)userdataJsonRecent["bonusPP"][ConvertMode(modeValue.Text)], 2) + "pp";
                        playtimeValue.Text = (string)userdataJsonRecent["playtime"][ConvertMode(modeValue.Text)];
                        playcountValue.Text = (string)userdataJsonRecent["playcount"][ConvertMode(modeValue.Text)];
                        BestPerformance.Items.Clear();
                        if (!(userdataJsonRecent["pp"][ConvertMode(modeValue.Text)] ?? throw new InvalidOperationException()).Any())
                        {
                            BestPerformance.Items.Add("No plays.");
                            return;
                        }

                        BestPerformance.Items.Add("-----------------------------------------------------------------------------------------------------------------");
                        for (int i = 0; i < (userdataJsonRecent["pp"][ConvertMode(modeValue.Text)] ?? throw new InvalidOperationException()).Count(); i++)
                        {
                            string itemTitle = $"Title: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["title"]}";
                            string versionName = $"Mapper: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["mapper"]}   Difficulty: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["version"]}";
                            string hitsInfo = "";
                            switch (ConvertMode(modeValue.Text))
                            {
                                case "osu":
                                    hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                                    break;
                                
                                case "taiko":
                                    hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                                    break;
                                
                                case "catch":
                                    hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                                    break;
                                
                                case "mania":
                                    hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["geki"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["katu"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                                    break;
                            }

                            if (itemTitle.Length > 110) itemTitle = itemTitle.Substring(0, 110) + "...";
                            if (versionName.Length > 110) versionName = versionName.Substring(0, 110) + "...";
                            
                            string resultInfo = $"Score: {(int)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["score"]:#,0} / {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["combo"]}x   {hitsInfo}";
                            BestPerformance.Items.Add($"#{i + 1}");
                            BestPerformance.Items.Add(itemTitle);
                            BestPerformance.Items.Add(versionName);
                            BestPerformance.Items.Add(resultInfo);
                            BestPerformance.Items.Add($"Mod: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["mods"]}   Accuracy: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["acc"]}%   PP: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["pp"]}pp");
                            BestPerformance.Items.Add("-----------------------------------------------------------------------------------------------------------------");
                        }
                        changePPValue.Text = Math.Abs(Math.Round((double)userdataJsonRecent["globalPP"][ConvertMode(modeValue.Text)] - GlobalPp[ConvertMode(modeValue.Text)], 2)) == 0 ? "" : $"{(Math.Round((double)userdataJsonRecent["globalPP"][ConvertMode(modeValue.Text)] - GlobalPp[ConvertMode(modeValue.Text)], 2) >= 0 ? "+" : "-")} {Math.Abs(Math.Round((double)userdataJsonRecent["globalPP"][ConvertMode(modeValue.Text)] - GlobalPp[ConvertMode(modeValue.Text)], 2))}pp";
                        changePPValue.ForeColor = Math.Round((double)userdataJsonRecent["globalPP"][ConvertMode(modeValue.Text)] - GlobalPp[ConvertMode(modeValue.Text)], 2) >= 0 ? Color.ForestGreen : Color.Red;
                        changeACCValue.Text = Math.Abs(Math.Round((double)userdataJsonRecent["globalACC"][ConvertMode(modeValue.Text)] - GlobalAcc[ConvertMode(modeValue.Text)], 2)) == 0 ? "" : $"{(Math.Round((double)userdataJsonRecent["globalACC"][ConvertMode(modeValue.Text)] - GlobalAcc[ConvertMode(modeValue.Text)], 2) >= 0 ? "+" : "-")} {Math.Abs(Math.Round((double)userdataJsonRecent["globalACC"][ConvertMode(modeValue.Text)] - GlobalAcc[ConvertMode(modeValue.Text)], 2))}%";
                        changeACCValue.ForeColor = Math.Round((double)userdataJsonRecent["globalACC"][ConvertMode(modeValue.Text)] - GlobalAcc[ConvertMode(modeValue.Text)], 2) >= 0 ? Color.ForestGreen : Color.Red;
                        changeBonusPPValue.Text = Math.Abs(Math.Round((double)userdataJsonRecent["bonusPP"][ConvertMode(modeValue.Text)] - BonusPp[ConvertMode(modeValue.Text)], 2)) == 0 ? "" : $"{(Math.Round((double)userdataJsonRecent["bonusPP"][ConvertMode(modeValue.Text)] - BonusPp[ConvertMode(modeValue.Text)], 2) >= 0 ? "+" : "-")} {Math.Abs(Math.Round((double)userdataJsonRecent["bonusPP"][ConvertMode(modeValue.Text)] - BonusPp[ConvertMode(modeValue.Text)], 2))}pp";
                        changeBonusPPValue.ForeColor = Math.Round((double)userdataJsonRecent["bonusPP"][ConvertMode(modeValue.Text)] - BonusPp[ConvertMode(modeValue.Text)], 2) >= 0 ? Color.ForestGreen : Color.Red;

                        GlobalPp["osu"] = (double)userdataJsonRecent["globalPP"]["osu"];
                        GlobalPp["taiko"] = (double)userdataJsonRecent["globalPP"]["taiko"];
                        GlobalPp["catch"] = (double)userdataJsonRecent["globalPP"]["catch"];
                        GlobalPp["mania"] = (double)userdataJsonRecent["globalPP"]["mania"];
                        GlobalAcc["osu"] = (double)userdataJsonRecent["globalACC"]["osu"];
                        GlobalAcc["taiko"] = (double)userdataJsonRecent["globalACC"]["taiko"];
                        GlobalAcc["catch"] = (double)userdataJsonRecent["globalACC"]["catch"];
                        GlobalAcc["mania"] = (double)userdataJsonRecent["globalACC"]["mania"];
                        BonusPp["osu"] = (double)userdataJsonRecent["bonusPP"]["osu"];
                        BonusPp["taiko"] = (double)userdataJsonRecent["bonusPP"]["taiko"];
                        BonusPp["catch"] = (double)userdataJsonRecent["bonusPP"]["catch"];
                        BonusPp["mania"] = (double)userdataJsonRecent["bonusPP"]["mania"];
                        if (timer.Enabled) timer.Stop();
                        timer.Start();
                    });
                }
                catch
                {
                    errorText.Text = "※ファイルの読み込み中にエラーが発生しました。次回記録を付けた際に反映されなかった記録なども表示されます。";
                }
            };
            watcher.EnableRaisingEvents = true;
        }

        private static void LaunchSoftware(string username)
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
                            MessageBox.Show(
                                "ダウンロードページをwebブラウザで開きます。\nインストール方法: ダウンロードしたフォルダを開き、osu!trainer/src/gosumemory/gosumemory.exeとなるように配置する。",
                                "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Process.Start("https://github.com/l3lackShark/gosumemory/releases/");
                            return;
                        }
                        Application.Exit();
                    }
                    
                    ProcessStartInfo startInfo = new ProcessStartInfo
                    {
                        FileName = "./src/gosumemory/gosumemory.exe",
                        CreateNoWindow = true,
                        UseShellExecute = false
                    };
                    Gosumemory = new Process();
                    Gosumemory.StartInfo = startInfo;
                    Gosumemory.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Gosumemoryの起動に失敗しました。\nエラー内容:{ex}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }

            try
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = "\"./src/nodejs/node.exe\"",
                    Arguments = $"\"./src/osu!private.js\" \"{username}\"",
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                OsuPrivate = new Process();
                OsuPrivate.StartInfo = startInfo;
                OsuPrivate.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"osu!privateの起動に失敗しました。\nエラー内容:{ex}", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }

        private void mainForm_FormClosed(object sender, EventArgs e) => Application.Exit();

        public static string ConvertMode(string value)
        {
            switch (value)
            {
                case "osu!standard":
                    return "osu";
                case "osu!taiko":
                    return "taiko";
                case "osu!catch":
                    return "catch";
                case "osu!mania":
                    return "mania";
                default:
                    return "osu";
            }
        }

        private void modeValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                changePPValue.Text = "";
                changeACCValue.Text = "";
                changeBonusPPValue.Text = "";
                StreamReader userdataRecent = new StreamReader($"./src/user/{Username}.json");
                string userdataStringRecent = userdataRecent.ReadToEnd();
                userdataRecent.Close();
                JObject userdataJsonRecent = JObject.Parse(userdataStringRecent);
                globalPPValue.Text = Math.Round((double)userdataJsonRecent["globalPP"][ConvertMode(modeValue.Text)], 2) + "pp";
                accValue.Text = Math.Round((double)userdataJsonRecent["globalACC"][ConvertMode(modeValue.Text)], 2) + "%";
                BonusPPValue.Text = Math.Round((double)userdataJsonRecent["bonusPP"][ConvertMode(modeValue.Text)], 2) + "pp";
                playtimeValue.Text = (string)userdataJsonRecent["playtime"][ConvertMode(modeValue.Text)];
                playcountValue.Text = (string)userdataJsonRecent["playcount"][ConvertMode(modeValue.Text)];
                BestPerformance.Items.Clear();
                if (!(userdataJsonRecent["pp"][ConvertMode(modeValue.Text)] ?? throw new InvalidOperationException()).Any())
                {
                    BestPerformance.Items.Add("No plays.");
                    return;
                }

                BestPerformance.Items.Add("-----------------------------------------------------------------------------------------------------------------");

                for (int i = 0; i < (userdataJsonRecent["pp"][ConvertMode(modeValue.Text)] ?? throw new InvalidOperationException()).Count(); i++)
                {
                    string itemTitle = $"Title: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["title"]}";
                    string versionName = $"Mapper: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["mapper"]}   Difficulty: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["version"]}";
                    string hitsInfo = "";
                    switch (ConvertMode(modeValue.Text))
                    {
                        case "osu":
                            hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                            break;
                        
                        case "taiko":
                            hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                            break;
                        
                        case "catch":
                            hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                            break;
                        
                        case "mania":
                            hitsInfo = $"Hits: {{{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["geki"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["300"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["katu"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["100"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["50"]}/{(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["miss"]}}}";
                            break;
                    }
                    
                    if (itemTitle.Length > 110) itemTitle = itemTitle.Substring(0, 110) + "...";
                    if (versionName.Length > 110) versionName = versionName.Substring(0, 110) + "...";
                    
                    string resultInfo = $"Score: {(int)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["score"]:#,0} / {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["combo"]}x   {hitsInfo}";
                    BestPerformance.Items.Add($"#{i + 1}");
                    BestPerformance.Items.Add(itemTitle);
                    BestPerformance.Items.Add(versionName);
                    BestPerformance.Items.Add(resultInfo);
                    BestPerformance.Items.Add($"Mod: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["mods"]}   Accuracy: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["acc"]}%   PP: {(string)userdataJsonRecent["pp"][ConvertMode(modeValue.Text)][i]["pp"]}pp");
                    BestPerformance.Items.Add("-----------------------------------------------------------------------------------------------------------------");
                }
            }
            catch
            {
                BestPerformance.Items.Clear();
                BestPerformance.Items.Add("No plays.");
                globalPPValue.Text = "0pp";
                accValue.Text = "0%";
                BonusPPValue.Text = "0pp";
                playtimeValue.Text = "0h 0m";
                playcountValue.Text = "0";
            }
        }

        private void deleteScore_Click(object sender, EventArgs e)
        {
            if (!File.Exists($"./src/user/{Username}.json"))
            {
                MessageBox.Show("No scores found! \n The delete function will not be enabled until you create a user and create at least one score!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            new DeleteForm().ShowDialog();
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape) BestPerformance.ClearSelected();
        }
    }
}
