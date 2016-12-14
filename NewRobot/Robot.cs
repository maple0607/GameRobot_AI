using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.IO;
using System.Threading;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace NewRobot
{
    public enum eRobotState
    {
        s_none,
        s_al,
        s_cl,
        s_gl,
        s_game,
    }

    public class UIData
    {
        public UIData()
        {

        }

        public virtual void Release()
        {
        }

        public virtual void AnalyzeToData(string custom, byte[] data)
        {
        }

        public virtual void AnalyzeTextProtocol(List<TextProtocolData> dataList)
        {

        }
    }


    public class Robot
    {
        private static Dictionary<string, Robot> m_selfDict = new Dictionary<string, Robot>();
        public static Robot GetRobotByName( string name )
        {
            lock (m_selfDict)
            {
                if (m_selfDict.ContainsKey(name))
                {
                    return m_selfDict[name];
                }
                else
                {
                    Robot r = new Robot(name);
                    m_selfDict[name] = r;
                    return r;
                }
            }
        }

        public static Robot GetCurRobot()
        {
            lock (m_selfDict)
            {
                string name = Thread.CurrentThread.Name;
                if (m_selfDict.ContainsKey(name))
                {
                    return m_selfDict[name];
                }
                else
                {
                    Robot r = new Robot(name);
                    m_selfDict[name] = r;
                    return r;
                }
            }
        }

        private string mThreadName;
        private int mNum;
        private string mIp;
        private string mPort;
        private string mHttp;
        private string mAccount;
        private string mPassword;
        private string mIdentifyCode;
        private string mServerID;
        private eRobotState mRobotState = eRobotState.s_none;

        private int mCurIdx = 0;

        public event PChangeBtnName OnChangeBtnName;
        public event PLogInfo OnLogInfo;

        public static List<TestBase.TestType> TestTypeLst = new List<TestBase.TestType>();
        private List<TestBase> mTestLst = new List<TestBase>();
        public TestBase mCurTestBase;

        private Dictionary<string, UIData> mUIDataDict = new Dictionary<string, UIData>();
        private SceneMgr mSceneMgr;
        public SceneMgr MySceneMgr
        {
            get
            {
                return mSceneMgr;
            }
        }

        private ActorManager mActorManager;
        public ActorManager MyActorManager
        {
            get
            {
                return mActorManager;
            }
        }

        private NetWorkMgr mNetWorkMgr = null;
        public NetWorkMgr MyNetWorkMgr
        {
            get
            {
                return mNetWorkMgr;
            }
        }

        private TaskMgr mTaskMgr = null;
        public TaskMgr MyTaskMgr
        {
            get
            {
                return mTaskMgr;
            }
        }

        public Robot(string name)
        {
            mThreadName = name;
        }

        public void PrintExcept(string ex)
        {
            OnLogInfo(mAccount, ex);
        }

        public void Start(string ip, string port, string http, string account, string password, int num, string serverid)
        {
            mIp = ip;
            mPassword = password;
            mPort = port;
            mHttp = http;
            mAccount = account;
            mNum = num;
            mServerID = serverid;
            
            mSceneMgr = new SceneMgr();
            mActorManager = new ActorManager();
            mTaskMgr = new TaskMgr();

            this.InitProtocol();
            this.ChangeState(eRobotState.s_al);
        }

        private void InitProtocol()
        {
            mNetWorkMgr = new NetWorkMgr(this.NetDroppedHandler);
            mNetWorkMgr.IPAddress = mIp;
            mNetWorkMgr.Port = int.Parse(mPort);
            ProtocolEvent myEvent = mNetWorkMgr.GetMyEvent();

            myEvent.OnPing += new ProtocolEvent.PPing(mNetWorkMgr.OnPing);
            //myEvent.OnWaiting += new ProtocolEvent.PWaiting(MyGameLogin.OnWaiting);
            myEvent.OnLogin += new ProtocolEvent.PLogin(OnGameLogin);
            myEvent.OnEventToUI += new ProtocolEvent.PEventToUI(OnEventSendToUI);     
            myEvent.OnEnterScene += new ProtocolEvent.PEnterScene(MySceneMgr.OnEnterMap);
            myEvent.OnGetPlayerInfo += new ProtocolEvent.PGetPlayerInfo(mActorManager.OnPlayerDetail);
            myEvent.OnGetBagData += new ProtocolEvent.PGetBagData(mActorManager.onBagData);
            myEvent.OnGetRoleInfo += new ProtocolEvent.PGetRoleInfo(mActorManager.onGetRoleInfo);
              
            myEvent.OnGetTaskList += new ProtocolEvent.PGetTaskList(mTaskMgr.OnTaskList);
            myEvent.OnCompleteTask += new ProtocolEvent.PCompleteTask(mTaskMgr.OnTaskComplete);
            myEvent.OnNotifyTask += new ProtocolEvent.PNotifyTask(mTaskMgr.OnNotifyTask);

            AddUIData(ConstUIName.UIMainData, new MainData());
            AddUIData("UIBattle", new UIBattleData());
            AddUIData("UIFriend", new UIFriendListData());
            AddUIData("UIFaction", new UIFactionData());
            AddUIData("UIShop", new UIShopData());
            AddUIData(ConstUIName.UIActivityPage, new UIAcitivyData());
            AddUIData("UIWorship", new UIWorshipData());
        }

        public UIData GetUIData(string szName)
        {
            try
            {
                return mUIDataDict[szName];
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public void AddUIData(string szName, UIData data)
        {
            mUIDataDict[szName] = data;
        }

        private void OnEventSendToUI(string name, string custom, byte[] type)
        {
            if (mUIDataDict.ContainsKey(name))
            {
                mUIDataDict[name].AnalyzeToData(custom, type);
            }
        }

        public void OnGameLogin(bool login, bool created, bool forbidden, string extraData, string sessionID)
        {
            if (login)
            {
                ChangeState(eRobotState.s_game);
                if (!created)
                {
                    CreateRole(mAccount, 1, 1);
                }
                for (int i = 0; i < TestTypeLst.Count; i++)
                {
                    mTestLst.Add( TestBase.CreateTest( TestTypeLst[i] ) );
                }
            }
            else
            {
                ChangeState(eRobotState.s_none);
            }
        }

           

        public void NetDroppedHandler()
        {
            this.ChangeState(eRobotState.s_none);
        }

        private string httpLogin(string uri, string username, string password)
        {
            string url = uri + "?username=" + username + "&password=" + password;

            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";  

           

            //byte[] byteArray = Encoding.UTF8.GetBytes(postData);
           // request.ContentType = "application/x-www-form-urlencoded";
           // request.ContentLength = byteArray.Length;
            //Stream dataStream = request.GetRequestStream();
            //dataStream.Write(byteArray, 0, byteArray.Length);
            //dataStream.Close();

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            StreamReader reader = new StreamReader(response.GetResponseStream());
            return reader.ReadToEnd();
        }

        private void StartAccountLogin()
        {
            mIdentifyCode = httpLogin(mHttp + "loginex", mAccount, "1");
            if (mIdentifyCode == "")
            {
                this.ChangeState(eRobotState.s_none);
            }
            else
            {
                this.ChangeState(eRobotState.s_cl);
            }
        }

        private void GameLogin()
        {

            byte[] identifyCodeData = Encoding.UTF8.GetBytes(mIdentifyCode);
            byte[] deviceDescription = Encoding.UTF8.GetBytes("123");
            int identifyCodeDataLength = identifyCodeData.Length;
            int deviceDescriptionLength = deviceDescription.Length;
            byte[] protocol = new byte[1 + 1 + 1 + 4 + 4 + 4 + identifyCodeDataLength + deviceDescriptionLength];
            int offset = 0;
            protocol[offset] = (byte)C2SProtocol.C2S_Login; ++offset;
            protocol[offset] = (byte)1; ++offset;
            protocol[offset] = (byte)1; ++offset;

            Array.Copy(BitConverter.GetBytes(int.Parse(mServerID)), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
            Array.Copy(BitConverter.GetBytes(identifyCodeDataLength), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
            Array.Copy(BitConverter.GetBytes(deviceDescriptionLength), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
            Array.Copy(identifyCodeData, 0, protocol, offset, identifyCodeDataLength); offset += identifyCodeDataLength;
            Array.Copy(deviceDescription, 0, protocol, offset, deviceDescriptionLength); offset += deviceDescriptionLength;
            mNetWorkMgr.SendData(protocol);
        }

        private void CreateRole(string rolename, byte nJob, byte icon)
        {
            if (rolename != "")
            {
                byte[] protocol = new byte[64 + 1 + 1 + 1];
                byte[] roleNameData = Encoding.UTF8.GetBytes(rolename);
                protocol[0] = (byte)C2SProtocol.C2S_Create;

                Array.Copy(roleNameData, 0, protocol, 1, roleNameData.Length);
                protocol[65] = (byte)nJob;
                protocol[66] = icon;
                mNetWorkMgr.SendData(protocol);
            }
            else
            {

            }
        }

        private void ChangeState(eRobotState state)
        {
            mRobotState = state;
            if ( OnChangeBtnName != null)
            {
                OnChangeBtnName(mNum, state);
            }

            switch (state)
            {
                case eRobotState.s_al:
                    {
                        this.StartAccountLogin();
                    }
                    break;
                case eRobotState.s_cl:
                    {
                        mNetWorkMgr.StartNetWork();
                    }
                    break;
                case eRobotState.s_gl:
                    {
                        this.GameLogin();
                    }
                    break;
                case eRobotState.s_none:
                    {
                        Thread.Sleep(4000);
                        ChangeState(eRobotState.s_al);
                    }
                    break;
            }
        }

        public void OnFinishTest()
        {
            mCurTestBase.OnFinish -= new PFinish(OnFinishTest);
            mCurTestBase.OnFinish = null;
            mCurTestBase = null;
        }

        public void Update()
        {
            switch (mRobotState)
            {
                case eRobotState.s_cl:
                    {
                        if ( mNetWorkMgr.GetCurNetState() == eNetState.net_working)
			            {
                            this.ChangeState(eRobotState.s_gl);
			            }
                        else if (mNetWorkMgr.GetCurNetState() == eNetState.net_dropped)
			            {
                            this.ChangeState(eRobotState.s_none);
			            }
                    }
                    break;
                case eRobotState.s_game:
                    {
                        if (mCurTestBase == null)
                        {
                            if ( mTestLst.Count > 0)
                            {
                                if (mCurIdx >= mTestLst.Count)
                                    mCurIdx = 0;

                                mCurTestBase = mTestLst[mCurIdx];
                                mCurTestBase.OnFinish += new PFinish(OnFinishTest);
                                mCurTestBase.Start();
                                mCurIdx++;
                            }
                        }
                        else
                        {
                            mCurTestBase.Loop();
                        }
                    }
                    break;
            }
            mNetWorkMgr.FixedUpdate();
        }

        public string name;
        public int level;
        public int fightValue;
    }
}
