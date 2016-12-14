using System;
using System.Collections.Generic;
using System.Text;
using NewRobot;

namespace NewRobot
{
    public class ShopTest : TestBase
    {
        public enum tStep
        {
            buy,
            wait,

        }
        public ShopTest()
        {
        }

        private tStep mCurStep;
        public long mTime;
        private int mCount = 0;
        private int[] itemIDs = { 77};

        public override void Start()
        {
            base.Start();
            ProtocolFuns.BugItem(2, 10000, 0);
            ProtocolFuns.getMallInfo();
            mCurStep = tStep.buy;
            mCount = 1;
        }

        public override void Loop()
        {
            if (mCurStep == tStep.buy)
            {
                UIShopData data = Robot.GetCurRobot().GetUIData("UIShop") as UIShopData;
                if (data != null)
                {
                    Random rd = new Random();
                    if (data.mItemDict.Count > 0)
                    {
                        ShopItemInfo item = data.mItemDict[0][rd.Next(0, data.mItemDict[0].Count)];
                        ProtocolFuns.buyStoreItem(0, item.mUUID, 1);
                    }
                }
                mCurStep = tStep.wait;
                mTime = DateTime.Now.Ticks / 10000000;
            }
            else
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 1)
                {
                    mCurStep = tStep.buy;
                    mCount--;
                    if (mCount <= 0)
                        OnFinish();
                }
            }
            testState = mCurStep.ToString();
        }
    }
}
