using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    public class ActivityTest
    {
        public enum tStep
        {
            apply,
            data,
            enter,
            finish,
        }
        protected bool finish = false;
        public bool IsFinish
        {
            get { return finish; }
        }

        public virtual void Start()
        {

        }

        public virtual void Loop()
        {
            finish = true;
        }
        public virtual void End()
        {

        }
    }
}
