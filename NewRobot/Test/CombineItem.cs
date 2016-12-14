using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    public class CombineItem : TestBase
    {
        public CombineItem()
        {
        }

        public override void Start()
        {
            base.Start();
            ProtocolFuns.CombineItem(0, true);
        }

        public override void Loop()
        {
            
            OnFinish();
        }
    }
}
