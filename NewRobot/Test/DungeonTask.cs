using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    public class DungeonTask : TestBase
    {
        public enum tStep
        {
            apply_dng,
            dng,
            dng_wait,
            complete,
        }
        public tStep mCurStep = tStep.apply_dng;
        public long mTime;
        public int mDungeonID;
        public int mCount = 0;

        public DungeonTask()
        {
        }

        public override void Start()
        {
            base.Start();
            mCurStep = tStep.apply_dng;
            mCount = 1;
        }

        public override void Loop()
        {
            base.Loop();

            Robot robot = Robot.GetCurRobot();

            if (robot.MyActorManager == null || robot.MyActorManager.mMyPlayerData == null)
                return;


            int batID = robot.MyActorManager.mMyPlayerData.mBattleID;
            if (mCurStep == tStep.apply_dng)
            {
                List<TaskInfoUpdate> lst = robot.MyTaskMgr.GetCurrentTaskList();
                foreach (TaskInfoUpdate task in lst)
                {
                    ProtocolFuns.GetDungeonList(batID);
                }
                mCurStep = tStep.dng;
            }
            else if (mCurStep == tStep.dng)
            {
                ProtocolFuns.BugItem(4, 20, 0);
                UIBattleData uidata = Robot.GetCurRobot().GetUIData("UIBattle") as UIBattleData;
                if (uidata != null )
                {
                    BattleData dngLst = uidata.GetBattleData(batID);
                    if (dngLst != null)
                    {
                        Random r = new Random();
                        List<int> lst = new List<int>();
                        foreach (DungeonShowInfo sInfo in dngLst.GetBatInfo(r.Next(0, 3)))
                        {
                            if ( sInfo.count <= 0 || sInfo.bLock)
                            {
                                continue;
                            }

                            lst.Add(sInfo.dungeonID);
                        }

                        if (lst.Count > 0)
                        {
                            lst.Sort();
                            mDungeonID = lst[0];
                            ProtocolFuns.EnterMap(mDungeonID);
                            mCurStep = tStep.dng_wait;
                            mTime = DateTime.Now.Ticks / 10000000;
                        }
                        else
                        {
                            mCurStep = tStep.apply_dng;
                        }
                    }
                }
            }
            else if (mCurStep == tStep.dng_wait)
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 15)
                {
                    ProtocolFuns.FinishDungeon(mDungeonID, 3, 15);
                    ProtocolFuns.EnterMap(10000);
                    mCurStep = tStep.complete;
                }
            }
            else if (mCurStep == tStep.complete)
            {
                List<TaskInfoUpdate> lst = robot.MyTaskMgr.GetCurrentTaskList();
                foreach (TaskInfoUpdate task in lst)
                {
                    if (task.state == enTaskState.ets_completed)
                    {
                        ProtocolFuns.CompleteTask(task.taskId);
                        //NewRobot.Robot.GetCurRobot().PrintExcept("Complete:" + task.taskId.ToString());
                    }
                }
                mCurStep = tStep.apply_dng;
                mCount--;
                if (mCount <= 0)
                    OnFinish();
            }
            testState = mCurStep.ToString();
        }
    }
}
