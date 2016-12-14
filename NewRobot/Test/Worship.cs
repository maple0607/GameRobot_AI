using System;
using System.Collections.Generic;

namespace NewRobot
{
    public class Worship : TestBase
    {
        public override void Start()
        {
            base.Start();
        }

        public override void Loop()
        {
            base.Loop();
            Robot robot = Robot.GetCurRobot();
            if (null == robot.MyActorManager || null == robot.MyActorManager.mMyPlayerData)
            {
                return;
            }
            UIWorshipData data = robot.GetUIData("UIWorship") as UIWorshipData;
            if (null == data)
            {
                OnFinish();
                return;
            }
            if (0 == data.worshipNum)
            {
                OnFinish();
                return;
            }
            ProtocolFuns.GetActivityInfo(48);
        }
    }
}
