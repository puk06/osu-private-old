using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace osu_private
{
    public partial class deleteForm : Form
    {
        public deleteForm()
        {
            InitializeComponent();
            try
            {
                scoreList.Items.Clear();
                modeValue.SelectedIndex = 0;
                StreamReader userdataRecent = new StreamReader($"./src/user/{mainForm.username}.json");
                string userdataStringRecent = userdataRecent.ReadToEnd();
                userdataRecent.Close();
                JObject userdataJsonRecent = JObject.Parse(userdataStringRecent);
                string mode = mainForm.convertMode(modeValue.Text);
                foreach (var score in userdataJsonRecent["pp"][mode])
                {
                    scoreList.Items.Add($"{score["title"]} [{score["version"]}] | {score["pp"]}pp");
                }

                if (scoreList.Items.Count != 0)
                {
                    deleteButton.Enabled = true;
                    scoreList.SelectedIndex = 0;
                    int maxWidth = 0;
                    foreach (var item in scoreList.Items)
                    {
                        if (maxWidth < TextRenderer.MeasureText(item.ToString(), scoreList.Font).Width)
                        {
                            maxWidth = TextRenderer.MeasureText(item.ToString(), scoreList.Font).Width;
                        }
                    }
                    scoreList.DropDownWidth = maxWidth + 10;
                }
                else
                {
                    deleteButton.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Failed to load scores", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void deleteButton_Click(object sender, EventArgs e)
        {
            if (scoreList.Text == "")
            {
                MessageBox.Show("Select a score!", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            DialogResult result = MessageBox.Show($"Are you sure you want to delete this score?\n\n Score: {scoreList.Items[scoreList.SelectedIndex]}", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
            if (result == DialogResult.Yes)
            {
                try
                {
                    StreamReader sr = new StreamReader(@"./src/user/" + mainForm.username + ".json", Encoding.GetEncoding("UTF-8"));
                    string json = sr.ReadToEnd();
                    sr.Close();
                    JObject jo = JObject.Parse(json);
                    string mode = mainForm.convertMode(modeValue.Text);
                    int index = scoreList.SelectedIndex;
                    jo["pp"][mode][index].Remove();
                    StreamWriter sw = new StreamWriter(@"./src/user/" + mainForm.username + ".json", false, new UTF8Encoding(false));
                    sw.Write(jo.ToString(Formatting.Indented));
                    sw.Close();
                    MessageBox.Show("Successfully deleted", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Hide();
                }
                catch
                {
                    MessageBox.Show("Failed to delete", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Canceled!", "Cancel", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void modeValue_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                scoreList.Items.Clear();
                StreamReader userdataRecent = new StreamReader($"./src/user/{mainForm.username}.json");
                string userdataStringRecent = userdataRecent.ReadToEnd();
                userdataRecent.Close();
                JObject userdataJsonRecent = JObject.Parse(userdataStringRecent);
                string mode = mainForm.convertMode(modeValue.Text);
                foreach (var score in userdataJsonRecent["pp"][mode])
                {
                    scoreList.Items.Add($"{score["title"]} [{score["version"]}] | {score["pp"]}pp");
                }

                if (scoreList.Items.Count != 0)
                {
                    deleteButton.Enabled = true;
                    scoreList.SelectedIndex = 0;
                    int maxWidth = 0;
                    foreach (var item in scoreList.Items)
                    {
                        if (maxWidth < TextRenderer.MeasureText(item.ToString(), scoreList.Font).Width)
                        {
                            maxWidth = TextRenderer.MeasureText(item.ToString(), scoreList.Font).Width;
                        }
                    }
                    scoreList.DropDownWidth = maxWidth + 10;
                }
                else
                {
                    deleteButton.Enabled = false;
                }
            }
            catch
            {
                MessageBox.Show("Failed to load scores", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
