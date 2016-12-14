using System;
using System.Collections.Generic;
using System.Text;
using Json;
namespace NewRobot
{
    public class PlunderData
    {
        public delegate void DRecivieData();
        public DRecivieData OnReciviData;
        public struct PlunderTarget
        {
            public string mUUID;
            public string mName;
            public bool isRobt;
            public PlunderTarget(string uuid, string sName, bool robot)
            {
                this.mUUID = uuid;
                this.mName = sName;
                this.isRobt = robot;
            }
        }
        public int mCostNum = int.MaxValue;
        public List<PlunderTarget> mTargetName = new List<PlunderTarget>();
        public void AnalyzeData(JsonObject obj)
        {            
            if (obj["Result"].Value.Equals("1"))
            {
                JsonProperty property = obj["Info"];
                string actionType = obj["Action"].Value;
                if (actionType.Equals("info"))
                {
                    mCostNum = int.Parse(property["Cost"].Value);
                }
                else if (actionType.Equals("refresh"))
                {
                    mTargetName.Clear();
                    foreach (JsonProperty player in property.Items)
                    {
                        string UUID = player[0].Value;
                        string sName = player[1].Value;
                        bool isRobot = player[2].IsTrue;
                        PlunderTarget target = new PlunderTarget(UUID, sName, isRobot);
                        mTargetName.Add(target);
                    }
                }
            }
            if (OnReciviData != null)
            {
                OnReciviData();
            }
        }
    }
    public class MoneyDng
    {
        public Dictionary<int, int> mDungeonDict = new Dictionary<int, int>();
        public List<int> mLvList = new List<int>();
        public int mCount = 0;

        public void InitPage(string acStr)
        {
            JsonObject obj = new JsonObject(acStr);
            JsonProperty property = obj["config"];
            mCount = int.Parse(obj["limittime"].Value);
            foreach (JsonProperty item in property.Items)
            {
                int lv = int.Parse(item[0].Value);
                int dngId = int.Parse(item[1].Value);
                mDungeonDict[lv] = dngId;
                if (!mLvList.Contains(lv))
                    mLvList.Add(lv);
            }
        }
    }
    public class ProtectHoardData
    {
        public Dictionary<int, int> mDungeonList = new Dictionary<int, int>();
        public List<int> lvs = new List<int>();
        public int mLostNum = 0;
        public List<string> mFriendData = new List<string>();

        public void OnGetActInfo(string actData)
        {
            mDungeonList.Clear();
            lvs.Clear();
            JsonObject info = new JsonObject(actData);
            mLostNum = int.Parse(info["ResetCount"].Value);

            JsonProperty dungeon = info["DungeonID"];
            for (int i = 0, max = dungeon.Items.Count; i < max; i++)
            {
                JsonProperty temp = dungeon.Items[i];
                int lv = int.Parse(temp.Items[0].Value);
                int id = int.Parse(temp.Items[1].Value);
                mDungeonList[lv] = id;
                lvs.Add(lv);
            }
            mFriendData.Clear();
            JsonProperty friends = info["FiveFriends"];
            if (friends.Items.Count > 0)
            {
                foreach (JsonProperty f in friends.Items)
                {
                    mFriendData.Add(f.Items[0].Value);
                }
            }
            lvs.Sort();
        }
    }


    public class UIAcitivyData : UIData
    {
        public PlunderData mPlunderData;
        public MoneyDng mMoneyDngData;
        public ProtectHoardData mProtectData;

        public UIAcitivyData()
        {
           mMoneyDngData = new MoneyDng();
           mProtectData = new ProtectHoardData();
           mPlunderData = new PlunderData();
        }

        public override void AnalyzeToData(string custom, byte[] data)
        {
            S2CProtocol protocol = (S2CProtocol)data[0];
            switch (protocol)
            {
                case S2CProtocol.S2C_GetActivityInfo:
                    {
                        int offset = 1;
                        int actID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                        int actDataLength = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                        string actData = System.Text.Encoding.UTF8.GetString(data, offset, actDataLength); offset += actDataLength;
                        SwitchToActivity(actID, actData);
                    }
                    break;
            }
        }

        private void SwitchToActivity(int id, string actData)
        {
            Activity.eActivityID actID = (Activity.eActivityID)id;
            switch (actID)
            {
                case Activity.eActivityID.AID_Eighteen:
                    {
                       
                    }
                    break;
                case Activity.eActivityID.AID_MulBoss:
                    {
                       
                    }
                    break;
                case Activity.eActivityID.AID_ProtectedAthena:
                    {

                    }
                    break;
                case Activity.eActivityID.AID_EndlessRoad:
                    {
                        mProtectData.OnGetActInfo(actData);
                    }
                    break;
                case Activity.eActivityID.AID_MoneyDungeon:
                    {
                        mMoneyDngData.InitPage(actData);
                    }
                    break;
            }
        }

    }
}
