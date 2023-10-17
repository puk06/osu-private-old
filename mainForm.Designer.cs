using System.Windows.Forms;

namespace osu_private
{
    partial class mainForm
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.accText = new System.Windows.Forms.Label();
            this.ppText = new System.Windows.Forms.Label();
            this.bonusPPText = new System.Windows.Forms.Label();
            this.accValue = new System.Windows.Forms.Label();
            this.globalPPValue = new System.Windows.Forms.Label();
            this.BonusPPValue = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.modeValue = new System.Windows.Forms.ComboBox();
            this.changePPValue = new System.Windows.Forms.Label();
            this.changeACCValue = new System.Windows.Forms.Label();
            this.changeBonusPPValue = new System.Windows.Forms.Label();
            this.deleteScore = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.playtimeValue = new System.Windows.Forms.Label();
            this.playcountValue = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // accText
            // 
            this.accText.AutoSize = true;
            this.accText.Font = new System.Drawing.Font("メイリオ", 16.25F);
            this.accText.Location = new System.Drawing.Point(320, 54);
            this.accText.Name = "accText";
            this.accText.Size = new System.Drawing.Size(76, 33);
            this.accText.TabIndex = 0;
            this.accText.Text = "ACC :";
            // 
            // ppText
            // 
            this.ppText.AutoSize = true;
            this.ppText.Font = new System.Drawing.Font("メイリオ", 16.25F);
            this.ppText.Location = new System.Drawing.Point(14, 54);
            this.ppText.Name = "ppText";
            this.ppText.Size = new System.Drawing.Size(124, 33);
            this.ppText.TabIndex = 1;
            this.ppText.Text = "GlobalPP :";
            // 
            // bonusPPText
            // 
            this.bonusPPText.AutoSize = true;
            this.bonusPPText.Font = new System.Drawing.Font("メイリオ", 16.25F);
            this.bonusPPText.Location = new System.Drawing.Point(548, 54);
            this.bonusPPText.Name = "bonusPPText";
            this.bonusPPText.Size = new System.Drawing.Size(124, 33);
            this.bonusPPText.TabIndex = 2;
            this.bonusPPText.Text = "BonusPP :";
            // 
            // accValue
            // 
            this.accValue.AutoSize = true;
            this.accValue.Font = new System.Drawing.Font("メイリオ", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accValue.Location = new System.Drawing.Point(390, 48);
            this.accValue.Name = "accValue";
            this.accValue.Size = new System.Drawing.Size(63, 41);
            this.accValue.TabIndex = 3;
            this.accValue.Text = "0%";
            // 
            // globalPPValue
            // 
            this.globalPPValue.AutoSize = true;
            this.globalPPValue.Font = new System.Drawing.Font("メイリオ", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.globalPPValue.Location = new System.Drawing.Point(131, 48);
            this.globalPPValue.Name = "globalPPValue";
            this.globalPPValue.Size = new System.Drawing.Size(67, 41);
            this.globalPPValue.TabIndex = 4;
            this.globalPPValue.Text = "0pp";
            // 
            // BonusPPValue
            // 
            this.BonusPPValue.AutoSize = true;
            this.BonusPPValue.Font = new System.Drawing.Font("メイリオ", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BonusPPValue.Location = new System.Drawing.Point(667, 48);
            this.BonusPPValue.Name = "BonusPPValue";
            this.BonusPPValue.Size = new System.Drawing.Size(67, 41);
            this.BonusPPValue.TabIndex = 5;
            this.BonusPPValue.Text = "0pp";
            // 
            // listBox1
            // 
            this.listBox1.Font = new System.Drawing.Font("メイリオ", 9.7F);
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(16, 121);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(848, 544);
            this.listBox1.TabIndex = 6;
            this.listBox1.KeyDown += new System.Windows.Forms.KeyEventHandler(this.listBox1_KeyDown);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("メイリオ", 16.25F);
            this.label1.Location = new System.Drawing.Point(20, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 33);
            this.label1.TabIndex = 7;
            this.label1.Text = "MODE :";
            // 
            // modeValue
            // 
            this.modeValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.modeValue.Font = new System.Drawing.Font("メイリオ", 12F);
            this.modeValue.FormattingEnabled = true;
            this.modeValue.Items.AddRange(new object[] {
            "osu!standard",
            "osu!taiko",
            "osu!catch",
            "osu!mania"});
            this.modeValue.Location = new System.Drawing.Point(122, 10);
            this.modeValue.Name = "modeValue";
            this.modeValue.Size = new System.Drawing.Size(197, 32);
            this.modeValue.TabIndex = 8;
            this.modeValue.SelectedIndexChanged += new System.EventHandler(this.modeValue_SelectedIndexChanged);
            // 
            // changePPValue
            // 
            this.changePPValue.AutoSize = true;
            this.changePPValue.Font = new System.Drawing.Font("メイリオ", 12F);
            this.changePPValue.Location = new System.Drawing.Point(134, 87);
            this.changePPValue.Name = "changePPValue";
            this.changePPValue.Size = new System.Drawing.Size(0, 24);
            this.changePPValue.TabIndex = 9;
            // 
            // changeACCValue
            // 
            this.changeACCValue.AutoSize = true;
            this.changeACCValue.Font = new System.Drawing.Font("メイリオ", 12F);
            this.changeACCValue.Location = new System.Drawing.Point(393, 87);
            this.changeACCValue.Name = "changeACCValue";
            this.changeACCValue.Size = new System.Drawing.Size(0, 24);
            this.changeACCValue.TabIndex = 10;
            // 
            // changeBonusPPValue
            // 
            this.changeBonusPPValue.AutoSize = true;
            this.changeBonusPPValue.Font = new System.Drawing.Font("メイリオ", 12F);
            this.changeBonusPPValue.Location = new System.Drawing.Point(670, 87);
            this.changeBonusPPValue.Name = "changeBonusPPValue";
            this.changeBonusPPValue.Size = new System.Drawing.Size(0, 24);
            this.changeBonusPPValue.TabIndex = 11;
            // 
            // deleteScore
            // 
            this.deleteScore.Location = new System.Drawing.Point(341, 10);
            this.deleteScore.Name = "deleteScore";
            this.deleteScore.Size = new System.Drawing.Size(112, 32);
            this.deleteScore.TabIndex = 12;
            this.deleteScore.Text = "Delete score";
            this.deleteScore.UseVisualStyleBackColor = true;
            this.deleteScore.Click += new System.EventHandler(this.deleteScore_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("メイリオ", 10F);
            this.label2.Location = new System.Drawing.Point(493, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 21);
            this.label2.TabIndex = 13;
            this.label2.Text = "Playtime : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("メイリオ", 10F);
            this.label3.Location = new System.Drawing.Point(485, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 21);
            this.label3.TabIndex = 14;
            this.label3.Text = "Playcount : ";
            // 
            // playtimeValue
            // 
            this.playtimeValue.AutoSize = true;
            this.playtimeValue.Font = new System.Drawing.Font("メイリオ", 11F);
            this.playtimeValue.Location = new System.Drawing.Point(572, 9);
            this.playtimeValue.Name = "playtimeValue";
            this.playtimeValue.Size = new System.Drawing.Size(56, 23);
            this.playtimeValue.TabIndex = 15;
            this.playtimeValue.Text = "0h 0m";
            // 
            // playcountValue
            // 
            this.playcountValue.AutoSize = true;
            this.playcountValue.Font = new System.Drawing.Font("メイリオ", 11F);
            this.playcountValue.Location = new System.Drawing.Point(572, 30);
            this.playcountValue.Name = "playcountValue";
            this.playcountValue.Size = new System.Drawing.Size(19, 23);
            this.playcountValue.TabIndex = 16;
            this.playcountValue.Text = "0";
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 694);
            this.Controls.Add(this.playcountValue);
            this.Controls.Add(this.playtimeValue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.deleteScore);
            this.Controls.Add(this.changeBonusPPValue);
            this.Controls.Add(this.changeACCValue);
            this.Controls.Add(this.changePPValue);
            this.Controls.Add(this.modeValue);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.BonusPPValue);
            this.Controls.Add(this.globalPPValue);
            this.Controls.Add(this.accValue);
            this.Controls.Add(this.bonusPPText);
            this.Controls.Add(this.ppText);
            this.Controls.Add(this.accText);
            this.Font = new System.Drawing.Font("メイリオ", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.Name = "mainForm";
            this.Text = "osu!private";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.mainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label accText;
        public System.Windows.Forms.Label ppText;
        public System.Windows.Forms.Label bonusPPText;
        public System.Windows.Forms.Label accValue;
        public System.Windows.Forms.Label globalPPValue;
        public System.Windows.Forms.Label BonusPPValue;
        public System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox modeValue;
        private System.Windows.Forms.Label changePPValue;
        private System.Windows.Forms.Label changeACCValue;
        private System.Windows.Forms.Label changeBonusPPValue;
        private System.Windows.Forms.Button deleteScore;
        private Label label2;
        private Label label3;
        private Label playtimeValue;
        private Label playcountValue;
    }
}

