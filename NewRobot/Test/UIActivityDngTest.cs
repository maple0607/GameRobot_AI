using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    public class UIActivityDngTest : TestBase
    {
        public ActivityTest mCurTest;
        public List<ActivityTest> mTestLst = new List<ActivityTest>();
        private int mTestIdx = 0;

        private void AddInitTest()
        {
            if (mTestLst.Count == 0)
            {
                mTestLst.Add(new PlunderTest());
                mTestLst.Add(new MoneyDngTest());
                mTestLst.Add(new ProtectHoard());
            }
        }

        public override void Start()
        {
            base.Start();
            AddInitTest();
            ChangeTest();
        }
        public override void Loop()
        {
            base.Loop();
            if (mCurTest == null)
            {
                OnFinish();
                return;
            }
            if (mCurTest.IsFinish)
            {
                ChangeTest();
            }
            else
                mCurTest.Loop();

        }
        private void ChangeTest()
        {
            if(mCurTest != null)
                mCurTest.End();
            if (mTestIdx >= mTestLst.Count)
            {
                mCurTest = null;
                return;
            }
            mCurTest = mTestLst[mTestIdx];
            if (mCurTest != null)
            {
                mCurTest.Start();
            }
            mTestIdx++;
        }
    }
}
