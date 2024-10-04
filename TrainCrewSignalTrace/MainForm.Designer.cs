namespace TrainCrewSignalTrace
{
    partial class MainForm
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
            this.ATSResetButton = new System.Windows.Forms.Button();
            this.SignalComboBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ATSResetButton
            // 
            this.ATSResetButton.Location = new System.Drawing.Point(365, 663);
            this.ATSResetButton.Name = "ATSResetButton";
            this.ATSResetButton.Size = new System.Drawing.Size(141, 34);
            this.ATSResetButton.TabIndex = 2;
            this.ATSResetButton.Text = "ATS復帰";
            this.ATSResetButton.UseVisualStyleBackColor = true;
            this.ATSResetButton.Click += new System.EventHandler(this.ATSResetButton_Click);
            // 
            // SignalComboBox
            // 
            this.SignalComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SignalComboBox.Font = new System.Drawing.Font("MS UI Gothic", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.SignalComboBox.FormattingEnabled = true;
            this.SignalComboBox.Items.AddRange(new object[] {
            "R",
            "YY",
            "Y",
            "YG",
            "G",
            "N"});
            this.SignalComboBox.Location = new System.Drawing.Point(436, 618);
            this.SignalComboBox.Name = "SignalComboBox";
            this.SignalComboBox.Size = new System.Drawing.Size(70, 27);
            this.SignalComboBox.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(365, 618);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 27);
            this.label3.TabIndex = 4;
            this.label3.Text = "信号現示";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            this.textBox1.Font = new System.Drawing.Font("BIZ UDゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.textBox1.Location = new System.Drawing.Point(12, 8);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(322, 688);
            this.textBox1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.LightGray;
            this.label2.Font = new System.Drawing.Font("BIZ UDゴシック", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(340, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 239);
            this.label2.TabIndex = 1;
            this.label2.Text = "label2";
            // 
            // MainForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(536, 709);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.SignalComboBox);
            this.Controls.Add(this.ATSResetButton);
            this.Controls.Add(this.label2);
            this.Name = "MainForm";
            this.Text = "TrainCrewSignalTrace";
            this.TopMost = true;
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button ATSResetButton;
        private System.Windows.Forms.ComboBox SignalComboBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
    }
}

