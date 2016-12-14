using System;
using System.Collections;
using System.Collections.Generic;
using Json;
using System.Security.Cryptography;
using System.Text;

public enum ModifiedAttributeType
{
	MAT_Unknown = 0,
	MAT_Money,                      // 金币
	MAT_GoldMoney,                  // 钻石
	MAT_GoldMoneyPurchased,         // 购买的钻石
	MAT_GoldMoneyNeedPurchased,     // 升级到下一个阶段需要的钻石
	
	MAT_Vitality,                   // 体力
	MAT_Experience,                 // 经验
	MAT_Level,                      // 等级
	MAT_VipLevel,                   // Vip等级
	
	MAT_Energy,					    // 真气
    MAT_Spar,                       // 晶石
		
	MAT_Life,						// 生命
	MAT_Attack,						// 攻击
	MAT_Defence,					// 防御
	MAT_Critical,					// 暴击
	MAT_AntiCritical,               // 韧性
	MAT_Hit,						// 命中
	MAT_Dodge,						// 闪避
	
	MAT_BagIsMax,                   // 背包是否满（1 = 满， 0 = 不满
	
	MAT_BattlePoint,                // 战力
	MAT_BattleID,                   //战役
	
	MAT_ArenaRank,                  // 竞技场排名
	
	MAT_Purchased,                  // 是否充值了
	
	MAT_ShowVipExperience,          // 是否显示充值经验
	
	MAT_InvokeRole,				    // 解锁角色
    MAT_InvokePet,				    // 解锁宠物
    MAT_ArenaPoint,                 // 竞技场点数
    MAT_BossPoint,                  // Boss点数
    MAT_GuildDismiss,				// 公会解散
    MAT_AddFriend,					// 好友申请
    MAT_CurTime,					// 当前时间
    MAT_Vigor,						// 精力
    MAT_Count,
}

public class PlayerDataBase
{
    public int mIndex;          // 索引
    public string mName;
    public int mIcon;			 // 头像
    public int mLevel;          // 等级
    public int mSelectedRoleIdx = -1;

    public Dictionary<int, RoleData> mRoleData = null;
    public Dictionary<int, PetInfo> mPetData = new Dictionary<int, PetInfo>();

    public PlayerDataBase()
    {
    }

    public PlayerDataBase(byte[] data, ref int offset, bool isMydata = true, bool isRobot = false)
    {
        mIndex = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
        mIcon = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mLevel = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mSelectedRoleIdx = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        int roleNum = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        if (mRoleData != null)
            mRoleData.Clear();
        else
            mRoleData = new Dictionary<int, RoleData>();
        if (mPetData != null)
            mPetData.Clear();
        else
            mPetData = new Dictionary<int, PetInfo>();
        for (int i = 0; i < roleNum; i++)
        {
            RoleData role = new RoleData(data, ref offset, isRobot);
            mRoleData[role.mIndex] = role;
        }
    }
}

public class PetInfo
{
    public int index;
    public int id;
    public string name;
    public int job;
    public int lvl;
    public int stage;
    public int order;
    public int life;
    public int attack;
    public int defence;
    public int critical;
    public int anticritical;
    public int hit;
    public int dodge;
    public int strike;
    public int antistrike;
    public int shieldRecover;
    public int shieldCD;

    public int trainLife;
    public int trainAttack;
    public int trainDefence;
    public int trainCritical;
    public int trainAntiCritical;
    public int trainHit;
    public int trainDodge;

    public int battlePoint;
    public List<SkillInfo> skillInfo = new List<SkillInfo>();
}

public class PlayerData : PlayerDataBase
{
    public string mUUID;
    public string mAccount;
    public string mGuildName;

    public int mVitality;                    // 体力
    public int mExperience;                  // 经验
    public int mMaxExperience;               // 当前等级最大经验

    public int mVipLevel;                    // Vip等级
    public int mMoney;                       // 银币
    public int mGoldMoney;                   // 金币
    public int mGoldMoneyPurchased;          // 充值的金币
    public int mMapID;                       // 地图ID
    public int mBattleID;					 // 战役ID 当前最大
    public int mBattlePoint;                 // 战斗力
    public int mTitle;				         //称号
    public int mVogir;				         //掠夺精力

    public byte mBuyNum;                     // 购买次数信息数量
    public int mEnergy;                      // 真气
    public int mSpar;                        // 晶石
    public int mBagSize;
    public List<ItemInfo> mBagData;
    public Dictionary<int, int> mVIPNum = new Dictionary<int, int>();

    public PlayerData()
    {
        mRoleData = new Dictionary<int, RoleData>();
    }

    public void AnalizePlayerData(byte[] data, ref int offset)
    {
        mUUID = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
        mAccount = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
        mName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
        mGuildName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;

        mIndex = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mIcon = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mVitality = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mExperience = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mMaxExperience = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mLevel = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mVipLevel = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mMoney = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mGoldMoney = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mGoldMoneyPurchased = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mEnergy = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mSpar = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mMapID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mBattleID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mBattlePoint = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mTitle = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mVogir = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
    }

    public void UpatePlayerPetInfo(int job, PetInfo newInfo)
    {
        if (mPetData.ContainsKey(job))
        {
            mPetData[job] = newInfo;
        }
        else
        {
            mPetData.Add(job, newInfo);
        }
    }

    public PlayerData(byte[] data, ref int offset)
    {
        AnalizePlayerData(data, ref offset);

        int roleNum = (int)data[offset]; offset += sizeof(byte);
        int petNum = (int)data[offset]; offset += sizeof(byte);
        mBuyNum = data[offset]; offset += sizeof(byte);
        if (mRoleData != null)
            mRoleData.Clear();
        else
            mRoleData = new Dictionary<int, RoleData>();

        for (int i = 0; i < roleNum; i++)
        {
            RoleData role = new RoleData(data, ref offset);
            mRoleData[role.mIndex] = role;
            if (mSelectedRoleIdx == -1)
            {
                mSelectedRoleIdx = role.mIndex;
            }
        }
        for (int i = 0; i < petNum; i++)
        {
            PetInfo mPetInfo = new PetInfo();
            mPetInfo.index = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.id = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.name = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
            mPetInfo.job = (int)data[offset]; offset++;
            mPetInfo.lvl = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.stage = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.order = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.life = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.attack = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.defence = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.critical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.anticritical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.hit = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.dodge = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.strike = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.antistrike = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.shieldRecover = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.shieldCD = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.trainLife = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.trainAttack = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.trainDefence = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.trainCritical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.trainAntiCritical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.trainHit = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.trainDodge = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mPetInfo.battlePoint = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

            int skillNum = (int)data[offset]; offset++;
            int signLength = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            for (int j = 0; j < skillNum; j++)
            {
                int skillID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                int skillLvl = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                SkillInfo skill = new SkillInfo(skillID, skillLvl);
                mPetInfo.skillInfo.Add(skill);
            }
            string sign = Encoding.UTF8.GetString(data, offset, signLength); offset += signLength;

            MD5 md5 = new MD5CryptoServiceProvider();
            string source = String.Format("{0:D4}{1:D4}{2:D4}{3:D4}{4:D4}{5:D4}{6:D4}{7:D4}", mPetInfo.life, mPetInfo.attack, mPetInfo.defence, mPetInfo.critical, mPetInfo.anticritical, mPetInfo.hit, mPetInfo.dodge, mPetInfo.battlePoint);
            byte[] bytes_md5_in = Encoding.UTF8.GetBytes(source);
            byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);
            string mysign = BitConverter.ToString(bytes_md5_out).Replace("-", "").ToLower();
            if (mysign != sign)
            {
            }

            if (mPetData.ContainsKey(mPetInfo.job))
            {
                mPetData[mPetInfo.job] = mPetInfo;
            }
            else
            {
                mPetData.Add(mPetInfo.job, mPetInfo);
            }
        }
        for (byte i = 0; i < mBuyNum; i++)
        {
            int type = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            int num = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mVIPNum[type] = num;
        }

#if UNITY_ANDROID && !UNITY_EDITOR && !USE_ACCOUNTTYPE
		string svrID = ServerLstMgr.Instance.mCurServerInfo.nSvrId.ToString();
		UnityEngine.Debug.LogWarning(string.Format("User Account : {0} {1}", mAccount, svrID));
		DCAccount.login(mAccount, svrID);
		//if ( GameData.Instance.accountType == AccountType.AT_UC || GameData.Instance.accountType == AccountType. )
		{
			if ( GameData.Instance.ntModule != null )
			{
				JsonObject extraData = new JsonObject();
				extraData["roleId"] = new JsonProperty(mAccount);
				extraData["roleName"] = new JsonProperty(mName);
				extraData["roleLevel"] = new JsonProperty(mLevel.ToString());
				extraData["zoneId"] = new JsonProperty(ServerLstMgr.Instance.mCurServerInfo.nSvrId);
				extraData["zoneName"] = new JsonProperty(ServerLstMgr.Instance.mCurServerInfo.sName);

				GameData.Instance.ntModule.handleExtraData(extraData.ToString());
			}
		}
#endif
    }

    public PlayerData(byte[] data, ref int offset, bool other)
    {
        AnalizePlayerData(data, ref offset);

        int roleNum = (int)data[offset]; offset += sizeof(byte);
        mBuyNum = data[offset]; offset += sizeof(byte);

        if (mRoleData != null)
            mRoleData.Clear();
        else
            mRoleData = new Dictionary<int, RoleData>();

        for (int i = 0; i < roleNum; i++)
        {
            RoleData role = new RoleData(data, ref offset);
            mRoleData[role.mIndex] = role;
        }
    }

    public PetInfo GetPetInfoByPetClass(int petClass)
    {
        PetInfo r = null;
        foreach (PetInfo info in mPetData.Values)
        {
            if (info.job == petClass)
            {
                r = info;
                break;
            }
        }
        return r;
    }

    public PetInfo GetPetInfoByServerIndex(int index)
    {
        PetInfo r = null;
        foreach (PetInfo info in mPetData.Values)
        {
            if (info.index == index)
            {
                r = info;
                break;
            }
        }
        return r;
    }

    public PetInfo GetPetInfoByServerOrder(int order)
    {
        PetInfo r = null;
        foreach (PetInfo info in mPetData.Values)
        {
            if (info.order == order)
            {
                r = info;
                break;
            }
        }
        return r;
    }

    public bool PlayerHasPet()
    {
        return mPetData.Count > 0;
    }

    public void InitBagData(byte[] data, ref int offset)
    {
        if (mBagData == null)
            mBagData = new List<ItemInfo>();

        mBagData.Clear();
        mBagSize = System.BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        int num = System.BitConverter.ToUInt16(data, offset); offset += sizeof(UInt16);
        for (int i = 0; i < num; i++)
        {
            ItemInfo info = new ItemInfo();
            info.InitItemInfo(data, ref offset);
            mBagData.Add(info);
        }
    }

    public ItemInfo GetItemFromBag(int itemID)
    {
        foreach (ItemInfo item in mBagData)
        {
            if (item.mID == itemID)
                return item;
        }
        return null;
    }

    public int GetItemNumFromBag(int itemID)
    {
        int curNum = 0;
        foreach (ItemInfo item in mBagData)
        {
            if (item.mID == itemID)
                curNum = curNum + item.mItemNum;
        }
        return curNum;
    }

    public ItemInfo GetItemFromBagIndex(int itemIndex)
    {
        foreach (ItemInfo item in mBagData)
        {
            if (item.mItemIndex == itemIndex)
                return item;
        }
        return null;
    }

    public ItemInfo getItemFromIndex(int itemID)
    {
        foreach (ItemInfo item in mBagData)
        {
            if (item.mItemIndex == itemID)
                return item;
        }
        return null;
    }



    public RoleData GetCurRoleData()
    {
        return mRoleData[mSelectedRoleIdx];
    }


    public RoleData GetRoleDataByJob(Job job)
    {
        foreach (RoleData mRoleData in this.mRoleData.Values)
        {
            if (mRoleData.mJob == job)
                return mRoleData;
        }
        return null;
    }

    public int GetCompleteSuitId(int roleIndex)
    {
        Dictionary<int, RoleData>.Enumerator et = mRoleData.GetEnumerator();
        RoleData roleData = null;
        while (et.MoveNext())
        {
            if (et.Current.Value.mIndex == roleIndex)
            {
                roleData = et.Current.Value;
                break;
            }
        }
        return GetCompleteSuitId(roleData);
    }

    public int GetCompleteSuitId(Job job)
    {
        Dictionary<int, RoleData>.Enumerator et = mRoleData.GetEnumerator();
        RoleData roleData = null;
        while (et.MoveNext())
        {
            if (et.Current.Value.mJob == job)
            {
                roleData = et.Current.Value;
                break;
            }
        }
        return GetCompleteSuitId(roleData);
    }

    static public int GetCompleteSuitId(RoleData roleData)
    {
        if (roleData == null)
            return 0;
        Dictionary<int, int> suits = new Dictionary<int, int>();
        for (int i = 0; i < roleData.mEquipments.Length; ++i)
        {
            if (roleData.mEquipments[i].mSuitID > 0)
            {
                if (suits.ContainsKey(roleData.mEquipments[i].mSuitID))
                {
                    suits[roleData.mEquipments[i].mSuitID] += 1;
                }
                else
                {
                    suits[roleData.mEquipments[i].mSuitID] = 1;
                }
            }
        }

        Dictionary<int, int>.Enumerator eti = suits.GetEnumerator();
        while (eti.MoveNext())
        {
            List<SuitData> sdata = ItemManager.Instance.GetSuitDatasBySuitId(eti.Current.Key);
            if (sdata != null && sdata.Count > 0)
            {
                if (eti.Current.Value >= sdata[sdata.Count - 1].mSuitNum)
                    return eti.Current.Key;
            }
        }
        return 0;
    }

    public int GetCompleteSuitIdOfCurRole()
    {
        return GetCompleteSuitId(GetCurRoleData());
    }
}