using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;


namespace NewRobot
{
    public delegate void PChangeBtnName(int num, eRobotState state);
    public delegate void PLogInfo(string account, string log);

    public partial class RobotWindow : Form
    {
        public List<eRobotState> mButtonState = new List<eRobotState>();
        public List<Button> mButtonLst = new List<Button>();

        public List<Thread> mThreadLst = new List<Thread>();

        public int startNum;
        public string mLogInfo;

        public Robot mCurSelRobot = null;

        public RobotWindow()
        {
            InitializeComponent();
            InitCheckList();
        }

        private void InitCheckList()
        {
            TestBase.TestNameDic.Add(TestBase.TestType.CombineItem, "合成道具");
            TestBase.TestNameDic.Add(TestBase.TestType.DungeonTask, "任务副本");
            TestBase.TestNameDic.Add(TestBase.TestType.Faction, "公会");
            TestBase.TestNameDic.Add(TestBase.TestType.Friend, "好友");
            TestBase.TestNameDic.Add(TestBase.TestType.Move, "移动");
            TestBase.TestNameDic.Add(TestBase.TestType.ShopTest, "商店");
            TestBase.TestNameDic.Add(TestBase.TestType.RechargeActivity, "充值活动");
            TestBase.TestNameDic.Add(TestBase.TestType.DngActivity, "活动副本");
            TestBase.TestNameDic.Add(TestBase.TestType.Worship, "膜拜");

            this.checkBoxes = new System.Windows.Forms.CheckBox[TestBase.TestNameDic.Count];
            int offsetX = 80;
            int offsetY = 22;
            int startPosX = 13;
            int startPosY = 12;
            int countX = 0;
            int countY = 0;
            int rowNum = 4;
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                countY = i % rowNum;
                countX = i / rowNum;
                checkBoxes[i] = new System.Windows.Forms.CheckBox();
                checkBoxes[i].Location = new System.Drawing.Point(startPosX + countX * offsetX, startPosY + countY * offsetY);
                checkBoxes[i].Name = "ckbDngTask";
                checkBoxes[i].Size = new System.Drawing.Size(72, 16);
                checkBoxes[i].TabIndex = 0;
                checkBoxes[i].Text = TestBase.TestNameDic[(TestBase.TestType)i];
                checkBoxes[i].UseVisualStyleBackColor = true;
                checkBoxes[i].Checked = true;
                this.panel1.Controls.Add(checkBoxes[i]);
            }
                
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            /*
            
            //newButton.Click += new EventHandler(newButton_Click);
            //重载运算符“+=”，把事件处理程序注册为CLICK事件的监听程序，
            //同时用非默认的构造函数创一个新的EventHandler对象，其名称是新事件处理函数的名称。
            //本句也可写作  newButton.Click += newButton_Click;（参见“委托”“事件”
            Controls.Add(newButton);*/
            
        }

        private void RobotWindow_Load(object sender, EventArgs e)
        {
            int total = 200;
            for (int x = 0; x < 20; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    if (total <= 0)
                    {
                        break;
                    }

                    Button newButton = new Button();//创建一个名为newButton的新按钮
                    newButton.Font = new Font(newButton.Font.FontFamily, 8);
                    newButton.BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    newButton.Location = new Point(x * 60, y * 60 + 160);
                    newButton.Width = 58;
                    newButton.Height = 58;
                    newButton.Text = string.Format("R_{0}", 200 - total);
                    newButton.Click += new System.EventHandler( mBRobot_Click );
                    Controls.Add(newButton);

                    mButtonState.Add(eRobotState.s_none);
                    mButtonLst.Add(newButton);
                    total--;
                }
            }
        }

        private void mBRobot_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            int num = int.Parse(btn.Text.Split('_')[1]);
            Robot robot = Robot.GetRobotByName( num.ToString() );
            mCurSelRobot = robot;
            robotState.Location = new Point(btn.Location.X + 28, btn.Location.Y + 28);
            robotState.Size = new System.Drawing.Size(420, 150);
        }

        private void mBStart_Click(object sender, EventArgs e)
        {
            for( int i = 0; i < mButtonLst.Count; i++)
            {
                Thread t = new Thread(StartThread);
                t.Name = i.ToString();
                t.Start(mButtonLst[i]);
                mThreadLst.Add(t);
            }
            
            for (int i = 0; i < checkBoxes.Length; i++)
            {
                if (checkBoxes[i].Checked)
                {
                    Robot.TestTypeLst.Add( (TestBase.TestType)i );
                }
            }
        }

        private void StartThread(object obj)
        {
            Button btn = (Button)obj;
            int num = int.Parse(btn.Text.Split('_')[1]);
            int startNum = int.Parse(mTBNum.Text) * 200;
            int total = startNum + num;

            string aName = "";
            if ( total >= 10000)
            {
                aName = string.Format("sl{0}", total);
            }
            else
            {
                if ( total >= 1000)
                {
                    aName = string.Format("sl0{0}", total);
                }
                else
                {
                    if ( total >= 100)
                    {
                        aName = string.Format("sl00{0}", total);
                    }
                    else
                    {
                        if ( total >= 10)
                        {
                            aName = string.Format("sl000{0}", total);
                        }
                        else
                        {
                            aName = string.Format("sl0000{0}", total);
                        }
                    }
                }
            }

            Robot r = Robot.GetCurRobot();
            r.Start(this.mTBHostname.Text,
                    this.mTBHostport.Text,
                    this.mTBLoginUrl.Text,
                    aName, "123456", num,
                    this.mTBLoginServerID.Text);

            r.OnChangeBtnName += new PChangeBtnName(onChangeBtnName);
            r.OnLogInfo += new PLogInfo(onLogInfo);
            if (num == 0)
            {
                mCurSelRobot = r;
            }
            while (true)
            {
                r.Update();
                Thread.Sleep(100);
            }
        }

        private void onLogInfo(string account, string log)
        {
            lock (mLogInfo)
            {
                mLogInfo += string.Format("{0}_{1}\r\n", account, log);
            }
        }

        private void onChangeBtnName(int num, eRobotState state)
        {
            mButtonState[num] = state;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (mLogInfo != "")
            {
                txtLog.Text += mLogInfo;
                mLogInfo = "";
                this.txtLog.Focus();//获取焦点
                this.txtLog.Select(this.txtLog.TextLength, 0);//光标定位到文本最后
                this.txtLog.ScrollToCaret();//滚动到光标处
            }

            for (int i = 0; i < mButtonState.Count; i++)
            {
                eRobotState state = mButtonState[i];
                mButtonLst[i].Text = string.Format("R_{0}_[{1}]", i, state);
                switch(state)
                {
                case eRobotState.s_al:
                    {
                        mButtonLst[i].BackColor = System.Drawing.Color.FromArgb(128, 128, 128);
                    }
                    break;
                case eRobotState.s_cl:
                    {
                        mButtonLst[i].BackColor = System.Drawing.Color.FromArgb(255, 255, 0);
                    }
                    break;
                case eRobotState.s_gl:
                    {
                        mButtonLst[i].BackColor = System.Drawing.Color.FromArgb(255, 0, 255);
                    }
                    break;
                case eRobotState.s_none:
                    {
                        mButtonLst[i].BackColor = System.Drawing.Color.FromArgb(255, 0, 0);
                    }
                    break;
                case eRobotState.s_game:
                    {
                        mButtonLst[i].BackColor = System.Drawing.Color.FromArgb(0, 255, 0);
                    }
                    break;
                }
            }
            if (mCurSelRobot != null && mCurSelRobot.mCurTestBase != null )
            {
                mCurSelRobot.PrintExcept(":  正在测试<" +  mCurSelRobot.mCurTestBase.testName + "> " + mCurSelRobot.mCurTestBase.testState + "  " + mCurSelRobot.mCurTestBase.extraInfo);
                //robotState.Text = "name" + mCurSelRobot.nameInGame + "\r\n";
               // robotState.Text += "level" + mCurSelRobot.levelInGame + "\r\n";
                robotState.Text += ":  正在测试<" + mCurSelRobot.mCurTestBase.testName + "> " + mCurSelRobot.mCurTestBase.testState + "  " + mCurSelRobot.mCurTestBase.extraInfo + "\r\n";
            }
        }

        private void RobotWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
            foreach (Thread t in mThreadLst)
            {
                t.Abort();
            }
        }
    }
}
