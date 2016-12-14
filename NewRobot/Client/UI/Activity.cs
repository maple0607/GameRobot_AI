using System;
using System.Collections.Generic;
using System.Text;
using Json;

public class Activity
{
    public enum  eActivityID
    {
        AID_SparShop=3,
        AID_AllianceCOC = 5,
        AID_Kicthen = 6,//吃大餐
        AID_7DayTargetGift = 7, //7天目标
		AID_MulBoss = 8,
		AID_ProtectedAthena = 9,
		AID_GuildHoard = 10, //公会礼佛
        AID_DailyActivity = 11,
		AID_ArenaScore = 12,
		AID_Carnival = 13,//开服嘉年华
        AID_FirstRecharge = 14,//首充大礼
        AID_TotalLogin = 15,//累计登录
        AID_SevenDayHappy = 16,//新玩家七天乐
        AID_Lottery = 17,
		AID_Eighteen = 18,
        AID_Achievements = 19,
        AID_GodTreature = 20,
        AID_MonthlyLogin = 21,
        AID_DoubleReward = 22,
        AID_ContinueLogin = 24,
		AID_DailyRecharge = 28,
		AID_VipReward = 29,//VIP奖励
        AID_OnlineGift= 30,//在线礼包
        AID_LevelGift=31,//升级礼包
        AID_NextDayGift=32,//次日礼包
        AID_EveryDayLogin=33,//每日登录
		AID_EndlessRoad = 34,
		AID_Rebate = 35,
		AID_GrowthFund = 36,//成长基金
		AID_DoubleRecharge = 37,
		AID_Promotion = 38,//充值促销
        AID_RedPacket = 41,//红包
		AID_TodayRecharge = 42,//今日累计充值
        AID_TotalRecharge = 43,//累计充值
		AID_GuildCopy = 44,		//公会副本
		AID_MoneyDungeon = 45,//绫罗秘境
		AID_FriendGift = 46,		//好友赠送体力
        AID_WorShip = 48, //膜拜
    }

    public enum eRewardItemType  
    {
        RT_Money = 0,           // 金币
        RT_GoldMoney,           // 钻石
        RT_Sophisticate,        // 阅历
        RT_Vitality,            // 体力
        RT_Prestige,            // 荣誉
        RT_Energy,              // 命力
        RT_Experience,          // 经验
        RT_SparGray,            // 灰晶
        RT_SparGreen,           // 绿晶
        RT_SparBlue,            // 蓝晶
        RT_SparPurple,          // 紫晶
        RT_SparDragon,          // 龙晶
        RT_FragmentGreen0,      // 绿色命运碎片0
        RT_FragmentGreen1,      // 绿色命运碎片1
        RT_FragmentBlue0,       // 蓝色命运碎片0
        RT_FragmentBlue1,       // 蓝色命运碎片1
        RT_FragmentBlue2,       // 蓝色命运碎片2
        RT_FragmentPurple0,     // 紫色命运碎片0
        RT_FragmentPurple1,     // 紫色命运碎片1
        RT_FragmentPurple2,     // 紫色命运碎片2
        RT_FragmentPurple3,     // 紫色命运碎片3
        RT_FragmentOrange0,     // 橙色命运碎片0
        RT_FragmentOrange1,     // 橙色命运碎片1
        RT_FragmentOrange2,     // 橙色命运碎片2
        RT_FragmentOrange3,     // 橙色命运碎片3
        RT_FragmentOrange4,     // 橙色命运碎片4
        RT_Item,                // 道具
        RT_Role,                // 同伴
        RT_GoldMoneyPurchased,  // 充值钻石
    }
    
    public Activity(byte[] data, ref int offset)
    {
        mID = BitConverter.ToInt32(data, offset); offset += sizeof(int);

		mStartTime = BitConverter.ToInt32(data, offset); offset += sizeof(int);
		mEndTime = BitConverter.ToInt32(data, offset); offset += sizeof(int);

        int nameLength = BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mName = Encoding.UTF8.GetString(data, offset, nameLength); offset += nameLength;
    }
    
    public class RewardItem
    {
        public Activity.eRewardItemType mRewardType;
        public int mItemNum;
        
        public struct sRewardItemInfo
        {
            public ItemMainType itemType;
            public int itemID;
        }
        // if equals  Activity.eRewardType.RT_Item then this field is valid, otherwise is null.
        public sRewardItemInfo mRewardItemInfo;
        
        public RewardItem(JsonProperty r)
        {
            mRewardType = (Activity.eRewardItemType)(int.Parse(r.Items[0].Value));
            
            switch (mRewardType)
            {
            case Activity.eRewardItemType.RT_Money :
            case Activity.eRewardItemType.RT_GoldMoney :
            case Activity.eRewardItemType.RT_Sophisticate :
            case Activity.eRewardItemType.RT_Vitality :
            case Activity.eRewardItemType.RT_Prestige :
            case Activity.eRewardItemType.RT_Energy :
            case Activity.eRewardItemType.RT_Experience :
            case Activity.eRewardItemType.RT_SparGray :
            case Activity.eRewardItemType.RT_SparGreen : 
            case Activity.eRewardItemType.RT_SparBlue :
            case Activity.eRewardItemType.RT_SparPurple :
            case Activity.eRewardItemType.RT_SparDragon :
            case Activity.eRewardItemType.RT_FragmentBlue0:
            case Activity.eRewardItemType.RT_FragmentBlue1:
            case Activity.eRewardItemType.RT_FragmentBlue2:
            case Activity.eRewardItemType.RT_FragmentGreen0:
            case Activity.eRewardItemType.RT_FragmentGreen1:
            case Activity.eRewardItemType.RT_FragmentOrange0:
            case Activity.eRewardItemType.RT_FragmentOrange1:
            case Activity.eRewardItemType.RT_FragmentOrange2:
            case Activity.eRewardItemType.RT_FragmentOrange3:
            case Activity.eRewardItemType.RT_FragmentOrange4:
            case Activity.eRewardItemType.RT_FragmentPurple0:
            case Activity.eRewardItemType.RT_FragmentPurple1:
            case Activity.eRewardItemType.RT_FragmentPurple2:
            case Activity.eRewardItemType.RT_FragmentPurple3:
            case Activity.eRewardItemType.RT_GoldMoneyPurchased:
                mItemNum = int.Parse(r.Items[1].Value);
                break;
                
            case Activity.eRewardItemType.RT_Item :
                mRewardItemInfo = new sRewardItemInfo();
                // They said that only need first reward item, what ever!
                JsonProperty itemInfo = r.Items[1].Items[0];
                mRewardItemInfo.itemType = (ItemMainType)(int.Parse(itemInfo[0].Value));
                mRewardItemInfo.itemID = int.Parse(itemInfo[1].Value);
                   
                mItemNum = int.Parse(itemInfo[2].Value);
                break;
            }
        }
        
        public int getRewardQuality()
        {
            switch (mRewardType)
            {
            case eRewardItemType.RT_Energy:
            case eRewardItemType.RT_Experience:
            case eRewardItemType.RT_GoldMoney:
            case eRewardItemType.RT_Money:
            case eRewardItemType.RT_Prestige:
            case eRewardItemType.RT_Sophisticate:
            case eRewardItemType.RT_SparGray:
            case eRewardItemType.RT_Vitality:
            case eRewardItemType.RT_GoldMoneyPurchased:
                return 0;
            case eRewardItemType.RT_FragmentGreen0:
            case eRewardItemType.RT_FragmentGreen1:
            case eRewardItemType.RT_SparGreen:
                return 1;
            case eRewardItemType.RT_FragmentBlue0:
            case eRewardItemType.RT_FragmentBlue1:
            case eRewardItemType.RT_FragmentBlue2:
            case eRewardItemType.RT_SparBlue:
                return 2;
            case eRewardItemType.RT_FragmentPurple0:
            case eRewardItemType.RT_FragmentPurple1:
            case eRewardItemType.RT_FragmentPurple2:
            case eRewardItemType.RT_FragmentPurple3:
            case eRewardItemType.RT_SparPurple:
                return 3;
            case eRewardItemType.RT_FragmentOrange0:
            case eRewardItemType.RT_FragmentOrange1:
            case eRewardItemType.RT_FragmentOrange2:
            case eRewardItemType.RT_FragmentOrange3:
            case eRewardItemType.RT_FragmentOrange4:
            case eRewardItemType.RT_SparDragon:
                return 4;
            }
            return 0;
        }
	}
        
    //--------------------------------------属性-----------------------------------------//
    public int ID
    {
        get { return mID; }
    }

    public string Name
    {
        get { return mName; }
    }
     
    //--------------------------------------数据-----------------------------------------//
    int mID;         // 活动ID
    string mName;       // 活动名
	int mStartTime;
	int mEndTime;
}
