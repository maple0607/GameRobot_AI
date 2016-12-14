using System;
using System.Collections;
using System.Collections.Generic;

public class ItemRealAttribute
{
    public MagicAttribute mItemAttribute;
    public int mValue;
    public int mGrowth;
    public int mPercent;

    public ItemRealAttribute(MagicAttribute attrType, int value, int growth, int percent)
    {
        mItemAttribute = attrType;
        mValue = value;
        mGrowth = growth;
        mPercent = percent;
    }
}

public class ItemInfo
{
	public int mItemIndex;                   	// 道具服务器索引
	public int mType;                        	// 道具类型
	public int mID;                          	// 道具id
	public int mQuality;						// 品质
    public int mSpar;							// 晶石
    public int mSuitID;						    // 套装ID
    public int mSpecialID;						// 特效ID
	public int mAvailableTime;              	// 有效时间（-1为始终有效
	public ushort mItemNum;                    	// 道具个数
	public ushort mPosition;                   	// 道具在背包中的位置
    public List<ItemRealAttribute> mItemRealAttribute = new List<ItemRealAttribute>();

	public ItemInfo()
	{
		mType = -1;
		mID = -1;
		mItemIndex = -1;
	}

	public virtual void InitItemInfo(byte[] data, ref int offset)
	{
		mItemIndex = System.BitConverter.ToInt32(data, offset); 									offset += sizeof(int);
		mType = System.BitConverter.ToInt32(data, offset); 											offset += sizeof(int);
		mID = System.BitConverter.ToInt32(data, offset); 											offset += sizeof(int);
        mQuality = System.BitConverter.ToInt32(data, offset);                                       offset += sizeof(int);
		mSpar = System.BitConverter.ToInt32(data, offset); 										    offset += sizeof(int);
        mSuitID = System.BitConverter.ToInt32(data, offset);                                        offset += sizeof(int);
        mSpecialID = System.BitConverter.ToInt32(data, offset);                                     offset += sizeof(int);
		mAvailableTime = System.BitConverter.ToInt32(data, offset); 								offset += sizeof(int);
		mItemNum = System.BitConverter.ToUInt16(data, offset); 										offset += sizeof(ushort);
		mPosition = System.BitConverter.ToUInt16(data, offset); 									offset += sizeof(ushort);
        int attrNum = (int)data[offset];                                                            offset += sizeof(byte);
        for (int i = 0; i < attrNum; i++)
        {
            MagicAttribute attrType = (MagicAttribute)data[offset];                                 offset += sizeof(byte);
            int value = System.BitConverter.ToInt32(data, offset);                                  offset += sizeof(int);
			int growth = System.BitConverter.ToInt32(data, offset);                                 offset += sizeof(int);
            int percent = System.BitConverter.ToInt32(data, offset);                                offset += sizeof(int);
            mItemRealAttribute.Add(new ItemRealAttribute(attrType, value, growth, percent));
        }
	}

	public bool IsEnable()
	{
		if ( mType == -1 || mID == -1)
			return false;
		else
			return true;
	}
};

public class RoleEquipmentInfo : ItemInfo
{
    private int mPosition;
	public RoleEquipmentInfo()
	{

	}

	public override void InitItemInfo(byte[] data, ref int offset)
	{
		mPosition = data[offset];					    offset++;
		base.InitItemInfo(data, ref offset);
	}

	public void InitItemInfo(ItemInfo info,EquipmentPosition pos){
		mItemIndex = info.mItemIndex;
		mType = info.mType;
		mID = info.mID;
		mSpar = info.mSpar;
		mQuality = info.mQuality;
        mSuitID = info.mSuitID;
        mSpecialID = info.mSpecialID;
		mAvailableTime = info.mAvailableTime;
		mItemNum = info.mItemNum;
		base.mPosition = info.mPosition;
        mItemRealAttribute = info.mItemRealAttribute;
		this.mPosition = (byte)pos;
	}

	public static EquipmentPosition getPosition(ItemMainType mainType,ItemSubType subType)
    {
		switch(mainType)
        {
		case ItemMainType.IMT_Weapon:
            {
			    return EquipmentPosition.EP_Weapon;
		    }
		case ItemMainType.IMT_Armor:
            {
			    return (EquipmentPosition)((int)subType%200);
		    }
		}
		return EquipmentPosition.EP_Unknown;
	}
}

public class ItemAdvanceInfo{
	public int       mFromItemId;
	public int       mToItemId;
	public int       mNeedExp;

	public void Init(string[] line){
		mFromItemId = int.Parse (line[0]);
		mToItemId = int.Parse (line[1]);
		mNeedExp = int.Parse (line[2]);
	}
}

public class ItemAttribute{
	public MagicAttribute  mItemAttribute;
	public int             mBasic;
	public int             mWeight;
	public int mGrowthValue;

	public void Init(string[] line,ref int offset){
		mItemAttribute = line[offset].CompareTo (string.Empty) == 0? MagicAttribute.MA_Unknown:(MagicAttribute)(int.Parse (line[offset]));         offset++;
		mBasic = line[offset].CompareTo (string.Empty) == 0?0:int.Parse (line[offset]);                                                            offset++;
		mWeight = line[offset].CompareTo (string.Empty) == 0?0:int.Parse (line[offset]);                                                           offset++;
	}
}

public class SimpleItemData{
	public int          mId;
	public int          mNum;
}

public class ItemData
{
	public int 				mItemId;
	public ItemMainType 	mMainType;
	public ItemSubType 		mSubType;
	public string 			mName;
	public int 				mSpar;
	public int  			mQlt;
	public byte 			mUseLevel;
	public Job				mJob;
	public int              mMaxNum;
	public int 				mSellPrice;
	public int				mEffectiveTime;
    public int              mSuitID;
    public int              mSpecialType;
	public string 			mIconName;
	public string 			mDescribe;
	public int 				mModuleResId;

	public ItemAttribute    mFirstAttribute = new ItemAttribute();
	public ItemAttribute    mSecondAttribute = new ItemAttribute();
	public ItemAttribute    mThridAttribute = new ItemAttribute();
	public ItemAttribute    mFourthAttribute = new ItemAttribute();

	void sort(){
		if(mFirstAttribute.mItemAttribute > mSecondAttribute.mItemAttribute){
			ItemAttribute item = mFirstAttribute;
			mFirstAttribute = mSecondAttribute;
			mSecondAttribute = item;
		}
	}

	public virtual void InitBaseInfo (string[] line, ref int offset)
	{
		mItemId = int.Parse(line[offset]);							offset++;
		mMainType = (ItemMainType)byte.Parse(line[offset]);			offset++;
		mSubType = (ItemSubType)int.Parse(line[offset]);			offset++;
		mName = line[offset];										offset++;
		mSpar = int.Parse(line[offset]);							offset++;
		mQlt = int.Parse(line[offset]);								offset++;
		mUseLevel = byte.Parse(line[offset]);						offset++;
		mJob = (Job)(int.Parse(line[offset]));						offset++;
		mMaxNum = int.Parse(line[offset]);							offset++;
		mSellPrice = int.Parse(line[offset]);						offset++;
		mEffectiveTime = int.Parse(line[offset]);					offset++;
		mFirstAttribute.Init (line,ref offset);
		mSecondAttribute.Init (line,ref offset);
		mThridAttribute.Init (line,ref offset);
		mFourthAttribute.Init (line,ref offset);
        mSuitID = line[offset] == "" ?0:int.Parse(line[offset]);                                offset++;
        mSpecialType = line[offset] == "" ?0:int.Parse(line[offset]);                           offset++;
		mIconName = line[offset];									offset++;
		mDescribe = line[offset];									offset++;
		if ( line[offset] != "" )
			mModuleResId = int.Parse(line[offset]);					offset++;
		sort ();
	}
}

public class WeaponInfo : ItemData
{
	public override void InitBaseInfo (string[] line, ref int offset)
	{
		base.InitBaseInfo(line, ref offset);
	}
}

public class ArmorInfo : ItemData
{
	public override void InitBaseInfo (string[] line, ref int offset)
	{
		base.InitBaseInfo(line, ref offset);
	}
}

public class BoxInfo : ItemData
{
	public override void InitBaseInfo (string[] line, ref int offset)
	{
		base.InitBaseInfo(line, ref offset);
	}
}

public class Consumeable : ItemData
{
	public override void InitBaseInfo (string[] line, ref int offset)
	{
		base.InitBaseInfo(line, ref offset);
	}
}

public class MaterialInfo : ItemData
{
	public override void InitBaseInfo (string[] line, ref int offset)
	{
		base.InitBaseInfo(line, ref offset);
	}
}

public class TalentInfo : ItemData
{
	public override void InitBaseInfo (string[] line, ref int offset)
	{
		base.InitBaseInfo(line, ref offset);
	}
}

public class StoneInfo : ItemData
{
	public override void InitBaseInfo (string[] line, ref int offset)
	{
		base.InitBaseInfo(line, ref offset);
	}
}


