using System;
using System.Collections;
using System.Collections.Generic;

public class SkillInfo
{
	public int mID;                         // id
	public int mLevel;						// 等级

	public SkillInfo(int id, int lvl)
	{
		mID = id;
		mLevel = lvl;
	}
}

public class RolePartInfo
{
	public Int32 mPos;
    public Int32 mLevel;
    public Int32 mQuality;
	public RoleEquipmentInfo[] mSlotInfo;

	public void InitRolePartInfo(byte []data, ref int offset){
		mPos = System.BitConverter.ToInt32(data, offset);                                       offset += sizeof(int);
		mLevel = System.BitConverter.ToInt32(data, offset);                                     offset += sizeof(int);
		mQuality = System.BitConverter.ToInt32(data, offset);                                   offset += sizeof(int);
        int slotNum = (int)data[offset];                                                        offset += sizeof(byte);
		mSlotInfo = new RoleEquipmentInfo[slotNum];
		for (int j = 0; j < slotNum; j++)
		{
			int isValid = (int)data[offset];                                                        offset += sizeof(byte);
			int order = (int)data[offset];                                                          offset += sizeof(byte);
			int hasItem = (int)data[offset];                                                        offset += sizeof(byte);
			mSlotInfo[order] = new RoleEquipmentInfo();
			if (hasItem == 1)
				mSlotInfo[order].InitItemInfo(data, ref offset);
		}
	}
};

public class RoleData : RoleDataBase
{
    public int mLife;							// 生命
    public int mAttack;							// 攻击
    public int mDefence;						// 防御
    public int mCritical;                   	// 暴击
    public int mAntiCritical;					// 韧性
    public int mHit;							// 命中
    public int mDodge;							// 闪避
    public int mStrike;							// 打击力
    public int mAntiStrike;						// 抗打击力
    public int mShieldRecover;					// 护盾恢复
    public int mShieldCD;						// 护盾cd

    public int mTrainLife;						// 培养生命
    public int mTrainAttack;					// 培养攻击
    public int mTrainDefence;					// 培养防御
    public int mTrainCritical;               	// 培养暴击
    public int mTrainAntiCritical;				// 培养韧性
    public int mTrainHit;						// 培养命中
    public int mTrainDodge;						// 培养闪避

    public int mBattlePoint;					// 战力

	public int[] mEquipTalent = new int[(int)EquipTalent.ET_Count];						// 心法
	public List<SkillInfo> mSkillInfos = new List<SkillInfo>();
	public List<SkillInfo> mTalents = new List<SkillInfo>();
	public RolePartInfo[] mRolePartInfo = new RolePartInfo[(int)EquipmentPosition.EP_Count];  // 装备槽信息

	public RoleData()
	{
	}

	public RoleData(byte []data, ref int offset, bool isRobot = false)
	{
        mIndex = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mJob = (Job)data[offset]; offset += sizeof(byte);
        mLevel = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        mLife = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        mAttack = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mDefence = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mCritical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mAntiCritical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mHit = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mDodge = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mStrike = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mAntiStrike = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mShieldRecover = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mShieldCD = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        mTrainLife = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mTrainAttack = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mTrainDefence = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mTrainCritical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mTrainAntiCritical = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mTrainHit = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        mTrainDodge = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        mBattlePoint = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        int skillNum = (int)data[offset]; offset += sizeof(byte);
        int talentNum = (int)data[offset]; offset += sizeof(byte);
        int casketNum = (int)data[offset]; offset += sizeof(byte);
        int equipNum = (int)data[offset]; offset += sizeof(byte);
        int signLength = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);

        for (int i = 0; i < skillNum; i++)
        {
            int id = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            int lvl = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mSkillInfos.Add(new SkillInfo(id, lvl));
        }

        for (int i = 0; i < talentNum; i++)
        {
            int id = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            int lvl = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mTalents.Add(new SkillInfo(id, lvl));
        }

        for (int i = 0; i < casketNum; i++)
        {
            mRolePartInfo[i] = new RolePartInfo();
            mRolePartInfo[i].InitRolePartInfo(data, ref offset);
        }

        this.InitEquipment(equipNum, data, ref offset);

        string sign = System.Text.Encoding.UTF8.GetString(data, offset, signLength); offset += signLength;
        if (isRobot)
            mLife = mLife / 2;
	}
}