using System;
using System.Collections.Generic;
using System.Text;
using Json;
namespace NewRobot
{
    public class ProtectHoard : ActivityTest
    {
        public tStep mCurStep = tStep.apply;
        public enum EnterMapStep
        {
            DoAction,
            Done,
        }
        private EnterMapStep mEnterStep = EnterMapStep.DoAction;
        Random random = new Random();
        private int mCurDngId = 0;
        private long mTime = 0;
        private int mCurIdx = 0;
        public override void Start()
        {
            mCurStep = tStep.apply;
            ProtocolEvent proEvent = Robot.GetCurRobot().MyNetWorkMgr.GetMyEvent();
            proEvent.OnGetFriendData += OnGetPlayerInfo;
            proEvent.OnDoActivityAction += OnDoAction;
        }
        public override void End()
        {
            ProtocolEvent proEvent = Robot.GetCurRobot().MyNetWorkMgr.GetMyEvent();
            proEvent.OnGetFriendData -= OnGetPlayerInfo;
            proEvent.OnDoActivityAction -= OnDoAction;
        }
        private void OnDoAction(byte[] data)
        {
            int offset = ProtocolEvent.ProtocolHeaderSize;
            int actID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            int actDataLength = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            string actData = System.Text.Encoding.UTF8.GetString(data, offset, actDataLength); offset += actDataLength;
            if(actID == (int)Activity.eActivityID.AID_ProtectedAthena)
            {
                if (string.IsNullOrEmpty(actData))
                {
                    finish = true;
                    return;
                }
                JsonObject info = new JsonObject(actData);
                if (info["Result"].Value.Equals("1"))
                {
                    UIAcitivyData uiActData = Robot.GetCurRobot().GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
                    if (uiActData.mProtectData.mFriendData.Count > 0)
                    {
                        int idx = random.Next(uiActData.mProtectData.mFriendData.Count);
                        ProtocolFuns.GetFriendData((byte)Activity.eActivityID.AID_ProtectedAthena, uiActData.mProtectData.mFriendData[idx]);
                    }
                    else
                        this.EnterMap();
                }
            }           
        }
        private void OnGetPlayerInfo(byte[] data)
        {
            int offset = ProtocolEvent.ProtocolHeaderSize;
            Activity.eActivityID id = (Activity.eActivityID)data[offset]; offset++;
            if (id == Activity.eActivityID.AID_ProtectedAthena)
            {
                this.EnterMap();
            }
            else
                finish = true;
        }
        private void EnterMap()
        {
            Robot robot = Robot.GetCurRobot();
            UIAcitivyData baseData = robot.GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
            if (robot.MyActorManager.mMyPlayerData.mLevel < baseData.mProtectData.lvs[mCurIdx])
            {
                finish = true;
                return;
            }
            mCurDngId = baseData.mProtectData.mDungeonList[baseData.mProtectData.lvs[mCurIdx]];
            ProtocolFuns.EnterMap(mCurDngId);
            mCurStep = tStep.enter;
            mTime = DateTime.Now.Ticks / 10000000;
            mCurIdx++;
        }
        public override void Loop()
        {
            Robot robot = Robot.GetCurRobot();
            if (robot.MyActorManager == null || robot.MyActorManager.mMyPlayerData == null)
            {
                finish = true;
                return;
            }
            if (mCurStep == tStep.apply)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_ProtectedAthena);
                mCurStep = tStep.data;
            }
            else if (mCurStep == tStep.data)
            {
                UIAcitivyData baseData = robot.GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
                if (baseData.mProtectData.lvs.Count <= 0)
                {
                    finish = true;
                    return;
                }
                if (baseData.mProtectData.mLostNum <= 0)
                {
                    finish = true;
                    return;
                }
                if (mEnterStep == EnterMapStep.DoAction)
                {
                    JsonObject info = new JsonObject();
                    info["ProtectAthena"] = new JsonProperty("1");
                    ProtocolFuns.DoActivityAction((int)Activity.eActivityID.AID_ProtectedAthena, info.ToString());
                    mEnterStep = EnterMapStep.Done;
                }
            }
            else if (mCurStep == tStep.enter)
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 15)
                {
                    ProtocolFuns.FinishDungeon(mCurDngId, 3, 30, robot.MySceneMgr.mCurSceneIdx);
                    ProtocolFuns.EnterMap(10000);
                    mCurStep = tStep.finish;
                }
            }
            else if (mCurStep == tStep.finish)
            {
                UIAcitivyData baseData = robot.GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
                baseData.mProtectData.lvs.Clear();
                mEnterStep = EnterMapStep.DoAction;               
                mCurStep = tStep.apply;
            }
        }
    }
}
