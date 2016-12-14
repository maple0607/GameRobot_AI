using System.Collections.Generic;

public class ActorManager
{
	public PlayerData mMyPlayerData = null;
	public PlayerDataBase mCurRemoteData = null;
	
	public ActorManager()
	{
	}

	public void RemoveCurRemoteData ()
	{
		 
	}

	public void OnPlayerDetail(byte []data, ref int offset)
	{
		mMyPlayerData = new PlayerData(data, ref offset);
	}

	public void onEquipItem(Error result, int itemIndex, int roleIndex,ItemInfo info)
	{
		if (result == Error.Err_Ok)
		{
			ItemData data = ItemManager.Instance.GetItemDataById (info.mID);
			EquipmentPosition ep = RoleEquipmentInfo.getPosition (data.mMainType,data.mSubType);
			if(ep != EquipmentPosition.EP_Unknown)
			{
				RoleEquipmentInfo rinfo = new RoleEquipmentInfo();
				rinfo.InitItemInfo (info,ep);
				mMyPlayerData.mRoleData[roleIndex].mEquipments[rinfo.mPosition] = rinfo;
			}
		}
	}

	public void onSellItemReward(bool mResult)
	{
		if (mResult) 
		{
		}
	}
	
	public void onUnequipItem(bool isOk, int roleIndex, int equipPosition)
	{
		if(isOk)
		{
			RoleEquipmentInfo reinfo = mMyPlayerData.mRoleData[roleIndex].mEquipments[equipPosition];
			if(reinfo.mID == -1 || reinfo.mItemIndex == -1)
				return;
			mMyPlayerData.mRoleData[roleIndex].mEquipments[equipPosition] = new RoleEquipmentInfo();
		}
	}

	public void onGetRoleInfo( byte[] data, ref int offset)
	{
		int idx = System.BitConverter.ToInt32(data, offset);
		mMyPlayerData.mRoleData[idx] = new RoleData(data, ref offset);
	}

	public void onBagData( byte[] data, ref int offset)
	{
		mMyPlayerData.InitBagData(data, ref offset);
	}
}
