using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using NewRobot;

public class BattleReward
{
	public int mBattleID;
	public int mDegree;
	public int mStage;
}
public class DungeonShowInfo
{
    public int dungeonID;
    public byte star;
    public bool bLock;
    public byte count;
    public List<int> items;
    public DungeonShowInfo(int dngID)
    {
        dungeonID = dngID;
        star = 0;
        bLock = false;
        count = 0;
		items = new List<int> ();
    }
}

public class BattleData
{
	public List<List<DungeonShowInfo>> mDngsData;
	public int mMaxDiffcult;
	public bool mInit = false;
	
	public BattleData(int batID)
	{
		mMaxDiffcult = 0;
		mDngsData = new List<List<DungeonShowInfo>>();
		for ( int i = 0; i < 3; i++)
		{
			List<DungeonShowInfo> lst = new List<DungeonShowInfo>();
			for ( int j = 0; j < 10; j++)
				lst.Add(new DungeonShowInfo(20000 + batID * 100 + i * 10 + j));
			mDngsData.Add(lst);
		}
	}
	
	public List<DungeonShowInfo> GetBatInfo (int diffcult)
	{
		if ( diffcult < 0 )
			return mDngsData[mMaxDiffcult];
		else
			return mDngsData[diffcult];
	}
}

public class GoldMoneyUsage
{
	public int target ;
	public int param1 ;
	public int param2 ;
	public int mCost ;
	public int mValue;
	public int mMaxNum ;
	public int mLeftNum ;

	public GoldMoneyUsage(byte[] data,ref int offset)
	{
		target = BitConverter.ToInt32 (data,offset);                                        	 offset += sizeof(int);
		param1 = BitConverter.ToInt32 (data,offset);                                         	 offset += sizeof(int);
		param2 = BitConverter.ToInt32 (data,offset);                                         	 offset += sizeof(int);
		mCost = BitConverter.ToInt32 (data,offset);                                        	 	 offset += sizeof(int);
		mValue = BitConverter.ToInt32 (data, offset);											 offset += sizeof(int);
		mMaxNum = BitConverter.ToInt32 (data,offset);                                         	 offset += sizeof(int);
		mLeftNum = BitConverter.ToInt32 (data,offset);                                         	 offset += sizeof(int);
	}
}


public class UIBattleData : UIData
{
	private Dictionary<int, BattleData> mBattleInfo;
	public List<int> mBattleId;
	public int mTaskDungeon = -1;
	public int hasGoAim = 0;
	public bool openUI = false;
	public Dictionary<int,List<BattleReward>> mBattleReward;
	public int[] mDiffMaxDungeon = new int[]{1,1,1};

	public int mRaidsCost = 1;

	public UIBattleData ()
	{
		mBattleInfo = new Dictionary<int, BattleData>();
		mBattleReward = new Dictionary<int, List<BattleReward>>();
		mBattleId = new List<int> ();
		for (int i=0; i<7; i++) {
			mBattleInfo[i] = new BattleData(i);
			mBattleReward[i] = new List<BattleReward>();
			mBattleId.Add(i);
		}
	}
	public BattleData GetBattleData(int dngID)
	{
		if (mBattleInfo.ContainsKey (dngID))
			return mBattleInfo [dngID];
		return null;
	}

	public int[] GetMaxDungeon()
	{
		int[] diff = new int[]{1,1,1};
		for(int i = 0;i<7;i++)
		{
			if(HasBattleInfo(i))
			{
				for(int dif = 0;dif<3;dif++)
				{
					for(int dungeonId = 0;dungeonId < 10;dungeonId ++)
					{
						DungeonShowInfo mDungeonShowInfo = GetBattleInfo (i, dif)[dungeonId];
						if(!mDungeonShowInfo.bLock)
						{
							diff[dif] = diff[dif] > mDungeonShowInfo.dungeonID ? diff[dif] : mDungeonShowInfo.dungeonID;
						}
					}

				}
			}
		}

		return diff;
	}

    public override void AnalyzeToData(string custom, byte[] data)
    {
        int offset = 0;
        S2CProtocol protocol = (S2CProtocol)data[offset]; offset++;
        if (protocol == S2CProtocol.S2C_NotifyScene)
        {
            byte type = data[offset];
            offset++;
            int dungeonID = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);

            int battleID = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            ProtocolFuns.GetDungeonList(dungeonID / 100 % 100);
        }
        else if (protocol == S2CProtocol.S2C_GetBattleReward)
        {
            byte mResult = data[offset];
            offset++;
            if (mResult == 1)
            {
                BattleReward mBattle = new BattleReward();
                mBattle.mBattleID = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                mBattle.mDegree = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                mBattle.mStage = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                if (!mBattleReward.ContainsKey(mBattle.mBattleID))
                {
                    mBattleReward[mBattle.mBattleID] = new List<BattleReward>();
                }
            }
        }
        else if (protocol == S2CProtocol.S2C_GetDungeonList)
        {
            int battleID = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);

            mRaidsCost = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

            if (mBattleReward.ContainsKey(battleID))
                mBattleReward[battleID].Clear();
            else
                mBattleReward[battleID] = new List<BattleReward>();
            short dungeonNum = System.BitConverter.ToInt16(data, offset);
            offset += sizeof(short);
            short battleRewardNum = System.BitConverter.ToInt16(data, offset);
            offset += sizeof(short);
            for (int idx = 0; idx < dungeonNum; idx++)
            {
                int dngID = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);

                int dif = dngID / 10 % 10;
                int dngIdx = dngID % 10;
                //Debug.Log(dngID + " " + dif + " " + dngIdx);

                if (!mBattleInfo.ContainsKey(battleID))
                {
                    mBattleInfo[battleID] = new BattleData(battleID);
                    mBattleId.Add(battleID);
                }

                if (dif > mBattleInfo[battleID].mMaxDiffcult)
                    mBattleInfo[battleID].mMaxDiffcult = dif;

                DungeonShowInfo info = mBattleInfo[battleID].mDngsData[dif][dngIdx];


                info.bLock = data[offset] == 1;
                offset++;
                info.star = data[offset];
                offset++;
                info.count = data[offset];
                offset++;
                int lootNum = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                info.items.Clear();
                for (int j = 0; j < lootNum; j++)
                {
                    int itemID = System.BitConverter.ToInt32(data, offset);
                    offset += sizeof(int);
                    info.items.Add(itemID);
                }
            }
            for (int idx = 0; idx < battleRewardNum; idx++)
            {
                BattleReward curBattleReward = new BattleReward();
                int battID = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                int degree = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                int stage = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                curBattleReward.mBattleID = battID;
                curBattleReward.mDegree = degree;
                curBattleReward.mStage = stage;
                mBattleReward[battID].Add(curBattleReward);
                //to do
            }
            mBattleInfo[battleID].mInit = true;
        }
        else if (protocol == S2CProtocol.S2C_RaidsScene)
        {
        }
    }

    public override void Release()
    {
        mBattleInfo.Clear();
    }

    public bool HasBattleInfo(int batID)
    {
		if(mBattleInfo.ContainsKey(batID))
			return mBattleInfo[batID].mInit;
        return false;
    }

    public List<DungeonShowInfo> GetBattleInfo(int batID, int diffcult)
    {
		if(mBattleInfo.ContainsKey(batID))
			return mBattleInfo[batID].GetBatInfo(diffcult);
		return null;
    }
    public DungeonShowInfo GetBattleInfo(int batID, int diffcult, int index)
    {
		if(mBattleInfo.ContainsKey(batID))
			return mBattleInfo[batID].GetBatInfo(diffcult)[index];
		return null;
    }

    public void getState(ref int battleId, ref int stage)
    {
        foreach (KeyValuePair<int, BattleData> data in mBattleInfo)
        {
            if (data.Value.mInit)
            {
                int star = 0;
                for (int i = 0; i < 10; ++i)
                {
                    if (data.Value.mDngsData[0][i].star == 3)
                    {
                        star++;
                    }
                }
                int s = 0;
                if (star == 10)
                    s = 3;
                else if (star >= 8)
                    s = 2;
                else if (star >= 6)
                    s = 1;
                if (s == 0)
                    continue;
                List<int> mCount = new List<int>();
                foreach (BattleReward reward in mBattleReward[data.Key])
                {
                    if (reward.mDegree == 0)
                    {
                        mCount.Add(reward.mStage);
                    }
                }
                for (int i = 1; i <= s; ++i)
                {
                    if (!mCount.Contains(i))
                    {
                        battleId = data.Key;
                        stage = i;
                        return;
                    }
                }
            }
        }
        battleId = -1;
        stage = -1;
    }
}