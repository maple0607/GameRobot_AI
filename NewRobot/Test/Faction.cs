using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    class Faction : TestBase
    {
        public enum tStep
        {
            getFactionList,
            waitFactionInfo,
            
            makeMoney,
            waitMakeMoney,

            getApplyList,
            waitApplyList,

            getExp,
            waitExp,

            agree_jion,
            apply_jion,
            create_faction,
            build_faction,
            get_treasureInfo,

        }
        public tStep mCurStep = tStep.getFactionList;
        public long mTime;
        public int mDungeonID;
        public int mCount = 0;

        public Faction()
        {
        }

        public override void Start()
        {
            base.Start();
            Robot robot = Robot.GetCurRobot();

            if (robot.MyActorManager == null || robot.MyActorManager.mMyPlayerData == null)
            {
                OnFinish();
                return;
            }
            if (robot.MyActorManager.mMyPlayerData.mLevel < 20)
            {
                mCurStep = tStep.getExp;
            }
            else
            {
                if (robot.MyActorManager.mMyPlayerData.mGuildName == "")
                {
                    mCurStep = tStep.getFactionList;
                }
                else
                {
                    mCurStep = tStep.agree_jion;
                }
            }
            mCount = 1;
        }

        public override void Loop()
        {
            base.Loop();

            Robot robot = Robot.GetCurRobot();

            if (robot.MyActorManager == null || robot.MyActorManager.mMyPlayerData == null)
                return;
            UIFactionData factionData = robot.GetUIData("UIFaction") as UIFactionData;
            if (factionData == null) return;
            if (mCurStep == tStep.getExp)
            {
                ProtocolFuns.BugItem(6, 1000000, 0);
                mCurStep = tStep.waitExp;
                mTime = DateTime.Now.Ticks / 10000000;
            }
            else if( mCurStep == tStep.waitExp )
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 5)
                {
                    mCurStep = tStep.getFactionList;
                }
            }else if (mCurStep == tStep.getFactionList)
            {
                ProtocolFuns.GetGuildList(0, 100);
                mCurStep = tStep.waitFactionInfo;
                mTime = DateTime.Now.Ticks / 10000000;
            }else if( mCurStep == tStep.waitFactionInfo )
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 5)
                {
                    mCurStep = tStep.apply_jion;
                }
            }else if( mCurStep == tStep.apply_jion )
            {
                if (factionData.maxGuildNum < 1)
                {
                    mCurStep = tStep.create_faction;
                }
                else
                {
                    for (int i = 0; i < factionData.mFactionInfoList.Count; i++)
                    {
                        if (factionData.mFactionInfoList[i].memberCount < factionData.mFactionInfoList[i].maxCount && !factionData.mFactionInfoList[i].isApplyed)
                        {
                            ProtocolFuns.JionGuild(factionData.mFactionInfoList[i].factionName);
                            extraInfo = factionData.mFactionInfoList[i].factionName;
                            mCount--;
                            if (mCount <= 0)
                            {
                                OnFinish();
                                return;
                            }
                        }
                    }
                    mCurStep = tStep.create_faction;
                }
            }else if (mCurStep == tStep.create_faction)
            {
                if (robot.MyActorManager.mMyPlayerData.mMoney < 1000000)
                {
                    ProtocolFuns.BugItem(1, 1000000, 0);
                    mCurStep = tStep.waitMakeMoney;
                    mTime = DateTime.Now.Ticks / 10000000;
                }
                else
                {
                    ProtocolFuns.CreateGuild(robot.MyActorManager.mMyPlayerData.mName, false);
                    mCount--;
                    if (mCount <= 0)
                    {
                        OnFinish();
                    }
                }
            }
            else if (mCurStep == tStep.waitMakeMoney)
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 5)
                {
                    ProtocolFuns.CreateGuild(robot.MyActorManager.mMyPlayerData.mName, false);
                    mCurStep = tStep.create_faction;
                    mCount--;
                    if (mCount <= 0)
                    {
                        OnFinish();
                    }
                }
            }
            else if (mCurStep == tStep.agree_jion)
            {
                ProtocolFuns.ApprovalGuildApplication("", true);
               // ProtocolFuns.QuitGuild();
                mCount--;
                if (mCount <= 0)
                {
                    OnFinish();
                }
            }
            testState = mCurStep.ToString();
        }
    }


}
