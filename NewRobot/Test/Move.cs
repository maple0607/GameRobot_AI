using System;
using System.Collections.Generic;
using System.Text;
namespace NewRobot
{
    public class Move : TestBase
    {
        public enum tStep
        {
            walk,
            wait,
        }
        public tStep mCurStep = tStep.walk;
        public long mTime;

        private int mCount = 0;
        public Move()
        {
        }

        public override void Start()
        {
            base.Start();
            mCount = 10;
            mCurStep = tStep.walk;
        }

        public override void Loop()
        {
            Robot robot = Robot.GetCurRobot();
            if (robot.MySceneMgr.CurSceneInfo.mMapID != 10000)
            {
                ProtocolFuns.EnterMap(10000);
                return;
            }

            if (mCurStep == tStep.walk)
            {
                Random r = new Random();
                ProtocolFuns.moveTo(r.Next(1, 20000), r.Next(1, 20000), r.Next(1, 20000), r.Next(1, 20000));
                mCurStep = tStep.wait;
                mTime = DateTime.Now.Ticks / 10000000;
            }
            else
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 3)
                {
                    mCurStep = tStep.walk;
                    /*mCount--;
                    if (mCount <= 0)
                        OnFinish();*/
                }
            }
            testState = mCurStep.ToString();
        }
    }
}
