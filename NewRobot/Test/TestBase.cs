using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    public delegate void PFinish();
    public class TestBase
    {
        public string testName = "Unknown";
        public string testState;
        public string extraInfo;
        public static Dictionary<TestType, string> TestNameDic = new Dictionary<TestType, string>();

        public enum TestType
        {
            CombineItem,
            DungeonTask,
            Faction,
            Friend,
            Move,
            ShopTest,
            RechargeActivity,
            DngActivity,
            Worship,
            Max
        }

        public PFinish OnFinish;

        public static TestBase CreateTest( TestType type )
        {
            TestBase test = null;
            switch (type)
            {
                case TestType.CombineItem:
                    test = new CombineItem();
                    break;
                case TestType.DungeonTask:
                    test = new DungeonTask();
                    break;
                case TestType.Faction:
                    test = new Faction();
                    break;
                case TestType.Friend:
                    test = new Friend();
                    break;
                case TestType.Move:
                    test = new Move();
                    break;
                case TestType.ShopTest:
                    test = new ShopTest();
                    break;
                case TestType.RechargeActivity:
                    test = new RechargeActivity();
                    break;
                case TestType.DngActivity:
                    test = new UIActivityDngTest();
                    break;
                case TestType.Worship:
                    test = new Worship();
                    break;
                default:
                    test = new TestBase();
                    break;

            }
            if (TestNameDic.ContainsKey(type))
            {
                test.testName = TestNameDic[type];
            }
            return test;
        }

        public TestBase()
        {
        }

        public virtual void Start()
        {
        }

        public virtual void Loop()
        {
        }
    }
}
