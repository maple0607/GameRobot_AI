using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    public class MoneyDngTest : ActivityTest
    {
        
        public tStep mCurStep = tStep.apply;
        public int mCurIndex = 0;
        public long mTime = 0;
        private int mCurDngId = 0;
        public override void Start()
        {
            mCurStep = tStep.apply;
            mCurIndex = 0;
            mCurDngId = 10000;
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
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_MoneyDungeon);
                mCurStep = tStep.data;
            }
            else if(mCurStep == tStep.data)
            {
                UIAcitivyData data = robot.GetUIData("UIActivity") as UIAcitivyData;                
                if (data.mMoneyDngData.mLvList.Count <= 0)
                {
                    return;
                }
                if (data.mMoneyDngData.mCount <= 0)
                {
                    finish = true;
                    return;
                }
                ProtocolFuns.EnterMap(data.mMoneyDngData.mDungeonDict[data.mMoneyDngData.mLvList[mCurIndex]]);
                mCurIndex++;
                mCurStep = tStep.enter;
                mTime = DateTime.Now.Ticks / 10000000;
            }
            else if (mCurStep == tStep.enter)
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 15)
                {
                    ProtocolFuns.FinishDungeon(mCurDngId, 3, 30,robot.MySceneMgr.mCurSceneIdx);
                    ProtocolFuns.EnterMap(10000);
                    mCurStep = tStep.finish;
                }
            }
            else if (mCurStep == tStep.finish)
            {
                UIAcitivyData data = robot.GetUIData("UIActivity") as UIAcitivyData;
                if (data.mMoneyDngData.mLvList.Count <= 0)
                {
                    finish = true;
                    return;
                }
                if (mCurIndex >= data.mMoneyDngData.mLvList.Count)
                {
                    finish = true;
                    return;
                }
                mCurStep = tStep.data;
            }
        }
    }
}
