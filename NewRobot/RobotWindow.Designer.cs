namespace NewRobot
{
    partial class RobotWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mTBNum = new System.Windows.Forms.TextBox();
            this.mBStart = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.mTBLoginServerID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.mTBLoginUrl = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.mTBHostport = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.mTBHostname = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.mLGongHao = new System.Windows.Forms.Label();
            this.robotState = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // mTBNum
            // 
            this.mTBNum.Location = new System.Drawing.Point(249, 48);
            this.mTBNum.Name = "mTBNum";
            this.mTBNum.Size = new System.Drawing.Size(139, 21);
            this.mTBNum.TabIndex = 10;
            this.mTBNum.Text = "10";
            // 
            // mBStart
            // 
            this.mBStart.Location = new System.Drawing.Point(313, 83);
            this.mBStart.Name = "mBStart";
            this.mBStart.Size = new System.Drawing.Size(75, 23);
            this.mBStart.TabIndex = 0;
            this.mBStart.Text = "开始";
            this.mBStart.UseVisualStyleBackColor = true;
            this.mBStart.Click += new System.EventHandler(this.mBStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.mTBLoginServerID);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.mTBLoginUrl);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.mTBHostport);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.mTBHostname);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 116);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "服务器";
            // 
            // mTBLoginServerID
            // 
            this.mTBLoginServerID.Location = new System.Drawing.Point(71, 89);
            this.mTBLoginServerID.Name = "mTBLoginServerID";
            this.mTBLoginServerID.Size = new System.Drawing.Size(152, 21);
            this.mTBLoginServerID.TabIndex = 7;
            this.mTBLoginServerID.Text = "2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 93);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "ServerID:";
            // 
            // mTBLoginUrl
            // 
            this.mTBLoginUrl.Location = new System.Drawing.Point(71, 62);
            this.mTBLoginUrl.Name = "mTBLoginUrl";
            this.mTBLoginUrl.ReadOnly = true;
            this.mTBLoginUrl.Size = new System.Drawing.Size(152, 21);
            this.mTBLoginUrl.TabIndex = 4;
            this.mTBLoginUrl.Text = "http://192.168.0.247/";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 65);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 5;
            this.label5.Text = "登录地址:";
            // 
            // mTBHostport
            // 
            this.mTBHostport.Location = new System.Drawing.Point(71, 37);
            this.mTBHostport.Name = "mTBHostport";
            this.mTBHostport.Size = new System.Drawing.Size(152, 21);
            this.mTBHostport.TabIndex = 2;
            this.mTBHostport.Text = "9988";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "端口:";
            // 
            // mTBHostname
            // 
            this.mTBHostname.Location = new System.Drawing.Point(71, 13);
            this.mTBHostname.Name = "mTBHostname";
            this.mTBHostname.ReadOnly = true;
            this.mTBHostname.Size = new System.Drawing.Size(152, 21);
            this.mTBHostname.TabIndex = 1;
            this.mTBHostname.Text = "192.168.0.247";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP地址:";
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel1
            // 
            this.panel1.Location = new System.Drawing.Point(407, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(352, 110);
            this.panel1.TabIndex = 13;
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(766, 12);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLog.Size = new System.Drawing.Size(420, 110);
            this.txtLog.TabIndex = 14;
            // 
            // mLGongHao
            // 
            this.mLGongHao.AutoSize = true;
            this.mLGongHao.Location = new System.Drawing.Point(247, 29);
            this.mLGongHao.Name = "mLGongHao";
            this.mLGongHao.Size = new System.Drawing.Size(59, 12);
            this.mLGongHao.TabIndex = 11;
            this.mLGongHao.Text = "使用工号:";
            // 
            // robotState
            // 
            this.robotState.Location = new System.Drawing.Point(160, 160);
            this.robotState.Multiline = true;
            this.robotState.Name = "robotState";
            this.robotState.Size = new System.Drawing.Size(0, 0);
            this.robotState.TabIndex = 15;
            // 
            // RobotWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1198, 762);
            this.Controls.Add(this.txtLog);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.mTBNum);
            this.Controls.Add(this.mLGongHao);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.mBStart);
            this.Controls.Add(this.robotState);
            this.MaximumSize = new System.Drawing.Size(1214, 800);
            this.MinimumSize = new System.Drawing.Size(1214, 800);
            this.Name = "RobotWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "机器人";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RobotWindow_FormClosing);
            this.Load += new System.EventHandler(this.RobotWindow_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox mTBNum;
        private System.Windows.Forms.Button mBStart;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox mTBLoginUrl;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox mTBHostport;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox mTBHostname;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox[] checkBoxes;
        private System.Windows.Forms.TextBox txtLog;
        private System.Windows.Forms.Label mLGongHao;
        private System.Windows.Forms.TextBox robotState;
        private System.Windows.Forms.TextBox mTBLoginServerID;
        private System.Windows.Forms.Label label3;
    }
}

