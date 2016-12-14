using System.Collections;
using System.Collections.Generic;


public enum buyTarget
{
	T_Unknown = -1,						//无效
	T_BuyVitality = 0,					//体力
	
	T_BuyMoney = 1,							//金币
	T_BuyArenaPoint = 2,					//竞技场点数
	T_BuyBagSlot = 3,						//背包
	T_ResetDungeon = 4,						//副本
	
	T_count,
}


public class ItemBase
{
	public byte mType;
	public int mID;
	public int mNum;
	public int mIdx = -1;
	public virtual void InitItemBase(byte[] data, ref int offset) 
	{
		mType = data[offset];  offset++;
		mID = System.BitConverter.ToInt32(data,offset);  offset += sizeof(int);
		mNum = System.BitConverter.ToInt32(data,offset); offset += sizeof(int);
		mIdx = System.BitConverter.ToInt32 (data, offset); offset += sizeof(int);
	}
}
public class SuitData
{
	public int mId;
	public int mSuitNum;
	public int mSuitBattlePoint;
	public MagicAttribute mSuitAttr;
	public int mSuitAttrPer;
	public int mSuitEffectId;
	public string mSuitName;
	public string mDesc;
	public SuitData(string[] lines)
	{
		mId = int.Parse (lines [0]);
		mSuitNum = string.IsNullOrEmpty (lines[1])?0:int.Parse (lines[1]);
		mSuitBattlePoint = string.IsNullOrEmpty (lines[2])?0:int.Parse (lines[2]);
		mSuitAttr = string.IsNullOrEmpty (lines[3])?MagicAttribute.MA_Unknown : (MagicAttribute)int.Parse (lines[3]);
		mSuitAttrPer = string.IsNullOrEmpty (lines[4])?0 : int.Parse (lines[4]);
		mSuitEffectId = string.IsNullOrEmpty (lines[5])?0 : int.Parse (lines[5]);
		mSuitName = lines[6];
		mDesc = lines[7];
	}
}

public class EquipPercent{
	public float      mLowest;
	public float      mHighest;
}

public class ItemManager
{
	private static ItemManager m_self = null;
	public static ItemManager Instance
	{
		get
		{
			if ( m_self == null ) 
			{
				m_self = new ItemManager();
			}
			return m_self;
		}
	}
	
	private Dictionary<ItemMainType, Dictionary<int, ItemData>> mItemDataDict;
	private Dictionary<int,Dictionary<int,SuitData>> mSuitsDataDict;
	private Dictionary<Job,Dictionary<MagicAttribute,float>> mBattlePointNum = new Dictionary<Job, Dictionary<MagicAttribute, float>>();
	private Dictionary<int,EquipPercent> mEquipPercentDict;
	public ItemManager ()
	{
		mItemDataDict = new Dictionary<ItemMainType, Dictionary<int, ItemData>>();
		mSuitsDataDict = new Dictionary<int, Dictionary<int, SuitData>>();
		mEquipPercentDict = new Dictionary<int, EquipPercent>();
	}

	public static ItemMainType getItemMainTypeById(int itemid)
	{
		if (itemid / 10000000 > 10) 
		{
			return (ItemMainType)(itemid / 100000000);
		}
		return (ItemMainType)(itemid / 10000000);
	}

	public void InitItemWeaponTable(List<string[]> lines)
	{
		Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
		for ( int idx = 1; idx < lines.Count; idx++)
		{
			int offset = 0;
			WeaponInfo info = new WeaponInfo();
			info.InitBaseInfo(lines[idx], ref offset);
			dict[info.mItemId] = info;
		}
		mItemDataDict[ItemMainType.IMT_Weapon] = dict;
	}

	public void InitItemArmorTable(List<string[]> lines)
	{
		Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
		for ( int idx = 1; idx < lines.Count; idx++)
		{
			int offset = 0;
			ArmorInfo info = new ArmorInfo();
			info.InitBaseInfo(lines[idx], ref offset);
			dict[info.mItemId] = info;
		}
		mItemDataDict[ItemMainType.IMT_Armor] = dict;
	}

	public void InitItemBoxTable(List<string[]> lines)
	{
		Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
		for ( int idx = 1; idx < lines.Count; idx++)
		{
			int offset = 0;
			BoxInfo info = new BoxInfo();
			info.InitBaseInfo(lines[idx], ref offset);
			dict[info.mItemId] = info;
		}
		mItemDataDict[ItemMainType.IMT_Box] = dict;
	}

	public void InitItemMaterialTable(List<string[]> lines)
	{
		Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
		for ( int idx = 1; idx < lines.Count; idx++)
		{
			int offset = 0;
			MaterialInfo info = new MaterialInfo();
			info.InitBaseInfo(lines[idx], ref offset);
			
			dict[info.mItemId] = info;
		}
		mItemDataDict[ItemMainType.IMT_Material] = dict;
	}

	public void InitItemTalentTable(List<string[]> lines)
	{
		Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
		for ( int idx = 1; idx < lines.Count; idx++)
		{
			int offset = 0;
			TalentInfo info = new TalentInfo();
			info.InitBaseInfo(lines[idx], ref offset);
			dict[info.mItemId] = info;
		}
		mItemDataDict[ItemMainType.IMT_Talent] = dict;
	}

	public void InitItemStoneTable(List<string[]> lines)
	{
		Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
		for ( int idx = 1; idx < lines.Count; idx++)
		{
			int offset = 0;
			StoneInfo info = new StoneInfo();
			info.InitBaseInfo(lines[idx], ref offset);
			dict[info.mItemId] = info;
		}
		mItemDataDict[ItemMainType.IMT_Stone] = dict;
	}

	public void InitItemConsumeableTable(List<string[]> lines)
	{
		Dictionary<int, ItemData> dict = new Dictionary<int, ItemData>();
		for ( int idx = 1; idx < lines.Count; idx++)
		{
			int offset = 0;
			Consumeable info = new Consumeable();
			info.InitBaseInfo(lines[idx], ref offset);
			
			dict[info.mItemId] = info;
		}
		mItemDataDict[ItemMainType.IMT_Consumeable] = dict;
	}

	public void InitSuitTable(List<string[]> lines)
	{
		lines.RemoveAt (0);
		for (int i=0,max=lines.Count; i<max; i++)
		{
			SuitData sd = new SuitData(lines[i]);
			if(!mSuitsDataDict.ContainsKey (sd.mId))
				mSuitsDataDict[sd.mId] = new Dictionary<int, SuitData>();
			mSuitsDataDict[sd.mId][sd.mSuitNum] = sd;
		}
	}

	public void InitEquipPercentTable(List<string[]> lines){
		for(int i = 1; i < lines.Count; ++i){
			EquipPercent ep = new EquipPercent();
			int id = int.Parse (lines[i][0]);
			ep.mLowest = float.Parse (lines[i][1])/100f;
			ep.mHighest = float.Parse (lines[i][2])/100f;
			mEquipPercentDict[id] = ep;
		}
	}

	public ItemInfo GetRoleUsedItemInfo(ItemInfo info)
	{
		ItemInfo cur = null;
		/*PlayerData curPlayer = GameClient.Instance.MyActorManager.mMyPlayerData;
		foreach(RoleData role in curPlayer.mRoleData.Values)
		{
			ItemData mItemData = ItemManager.Instance.GetItemDataById(info.mID);
			int position = ItemManager.Instance.GetEquipmentPosition(mItemData);
			if(role.mJob == mItemData.mJob)
			{
				cur = role.mEquipments[position];
			}
		}*/
		return cur;
	}
	


    public ItemData GetItemDataById(int itemid)
	{
		if ( itemid <= 0)
			return null;

		ItemMainType itemType = getItemMainTypeById(itemid);
		if ( itemType == ItemMainType.IMT_Unknown)
			return null;
		
		Dictionary<int, ItemData> dict = mItemDataDict[itemType];
		ItemData info = null;
		if ( dict.ContainsKey(itemid))
			info = dict[itemid];

		return info;
	}
		
	public int GetModelResID (int itemID)
	{
		ItemData data = this.GetItemDataById(itemID);
		if ( data == null)
			return 0;

		return data.mModuleResId;
	}

	public RoleData GetRoleIndexByItemData(int id)
	{
		RoleData curNoticeRole = null; 
		/*ItemData curData = ItemManager.Instance.GetItemDataById(id);
		int ePosition = ItemManager.Instance.GetEquipmentPosition(curData);
		Job curJob = curData.mJob;
		foreach(RoleData role in  GameClient.Instance.MyActorManager.mMyPlayerData.mRoleData.Values)
		{
			if(role.mJob == curJob)
			{
				curNoticeRole = role;
			}
		}*/
		return curNoticeRole;
	}

    public int GetEquipmentPosition(ItemData itemData)
    {
        int ePosition = -1;
        switch (itemData.mSubType)
        {
            case ItemSubType.IST_HandsWeapon:
            case ItemSubType.IST_HandWeapon:
            case ItemSubType.IST_LongWeapon:
                {
                    ePosition = (int)EquipmentPosition.EP_Weapon;
                }
                break;
            case ItemSubType.IST_Clothes:
                {
                    ePosition = (int)EquipmentPosition.EP_Clothes;
                }
                break;
            case ItemSubType.IST_Pants:
                {
                    ePosition = (int)EquipmentPosition.EP_Pants;
                }
                break;
            case ItemSubType.IST_Shoe:
                {
                    ePosition = (int)EquipmentPosition.EP_Shoe;
                }
                break;
            case ItemSubType.IST_Cuff:
                {
                    ePosition = (int)EquipmentPosition.EP_Cuff;
                }
                break;
            case ItemSubType.IST_Belt:
                {
                    ePosition = (int)EquipmentPosition.EP_Belt;
                }
                break;
            case ItemSubType.IST_Jewellery:
                {
                    ePosition = (int)EquipmentPosition.EP_Jewellery;
                }
                break;
			case ItemSubType.IST_Wrap:
				{
					ePosition = (int)EquipmentPosition.EP_Wrap;
				}
					break;
		}
		return ePosition;
	}
	
    public int ItemQuality(int QualityID)
    {
        int quality = 0;
        if (QualityID < 0 || QualityID > 11)
        {
        }
        else
        {
            quality = QualityID / 3 + 1;
        
        }
        return quality;
    }

	public SuitData GetSuitData(int suitId,int suitNum){
		if (mSuitsDataDict.ContainsKey (suitId) && mSuitsDataDict[suitId].ContainsKey (suitNum)){
			return mSuitsDataDict[suitId][suitNum];
		}
		return null;
	}

	public List<SuitData> GetSuitDatasBySuitId(int suitId){
		if(mSuitsDataDict.ContainsKey (suitId)){
			List<SuitData> datas = new List<SuitData>();
			Dictionary<int,SuitData>.ValueCollection.Enumerator vc = mSuitsDataDict[suitId].Values.GetEnumerator ();
			while(vc.MoveNext ()){
				datas.Add (vc.Current);
			}
			return datas;
		}
		return null;
	}

	
	static Dictionary<int,ItemAdvanceInfo>   ItemAdvanceInfos = new Dictionary<int, ItemAdvanceInfo>();

	static public void InitAdvanceInfos(List<string[]> lines){
		ItemAdvanceInfos.Clear ();
		for(int i = 1; i < lines.Count; ++i){
			ItemAdvanceInfo info = new ItemAdvanceInfo();
			info.Init (lines[i]);
			ItemAdvanceInfos[info.mFromItemId] = info;
		}
	}

	static public ItemAdvanceInfo GetAdvanceInfo(int fromId){
		if(ItemAdvanceInfos.ContainsKey (fromId))
			return ItemAdvanceInfos[fromId];
		return null;
	}

	public void InitBattlePoint(List<string[]> lines){
		for(int i = 1; i < lines.Count; ++i){
			string[] strs = lines[i];
			int index = 0;
			Job job = (Job)int.Parse (strs[index++]);
			mBattlePointNum[job] = new Dictionary<MagicAttribute, float>();
			for(int j = (int)MagicAttribute.MA_Life; j <= (int)MagicAttribute.MA_Dodge; ++j,++index){
				mBattlePointNum[job][(MagicAttribute)j] = string.IsNullOrEmpty (strs[index]) ? 0 : float.Parse (strs[index]);
			}
		}
	}
}
