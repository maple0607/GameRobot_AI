using System;
using System.Collections.Generic;
using System.Text;

namespace NewRobot
{
    public class GoldMoneyUsage
    {
        public int target;
        public int param1;
        public int param2;
        public int mCost;
        public int mValue;
        public int mMaxNum;
        public int mLeftNum;

        public GoldMoneyUsage(byte[] data, ref int offset)
        {
            target = BitConverter.ToInt32(data, offset); offset += sizeof(int);
            param1 = BitConverter.ToInt32(data, offset); offset += sizeof(int);
            param2 = BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mCost = BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mValue = BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mMaxNum = BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mLeftNum = BitConverter.ToInt32(data, offset); offset += sizeof(int);
        }
    }
    public class MainData : UIData
    {
        private Dictionary<int, GoldMoneyUsage> mGoldMoneyUsage = new Dictionary<int, GoldMoneyUsage>();
        public override void AnalyzeToData(string custom, byte[] data)
        {
            int offset = 0;
            S2CProtocol protocol = (S2CProtocol)data[offset]; offset++;
            switch (protocol)
            {
                case S2CProtocol.S2C_GetGoldMoneyUsage:
                    {
                        GoldMoneyUsage _usage = new GoldMoneyUsage(data, ref offset);
                        mGoldMoneyUsage[_usage.target] = _usage;
                    }
                    break;
            }
        }
        public int GetLeftNum(int target)
        {
            if (mGoldMoneyUsage.ContainsKey(target))
                return mGoldMoneyUsage[target].mLeftNum;
            return 0;
        }
    }
}
