using System.Collections;
using System.Collections.Generic;
using NewRobot;
using System.Threading;

public class RoleDataBase
{
	public int mIndex;                       	// 索引 
	public int mID;                          	// ID
	public Job mJob;                          	// 职业
	public int mLevel;                       	// 等级

	public RoleEquipmentInfo[] mEquipments = new RoleEquipmentInfo[(int)EquipmentPosition.EP_Count];

	public RoleDataBase ()
	{
		for ( int i = 0; i < mEquipments.Length; i++)
			mEquipments[i] = new RoleEquipmentInfo();
	}

	public void InitEquipment (int equipmentNum, byte []data, ref int offset)
	{
        for (int i = 0; i < equipmentNum; i++)
        {
            RoleEquipmentInfo info = new RoleEquipmentInfo();
            info.InitItemInfo(data, ref offset);

            mEquipments[info.mPosition] = info;
        }
	}

	//该方法只用于获取模型id
	public bool GetEquipID(EquipmentPosition pos, ref int itemID)
	{
        RoleEquipmentInfo info = null;
        bool find = false;
        if (pos == EquipmentPosition.EP_Clothes)
        {
            info = mEquipments[(int)EquipmentPosition.EP_Wrap];
            if (info.IsEnable())
                find = true;
        }
        if (!find)
            info = mEquipments[(int)pos];
        itemID = info.mID;
        return info.IsEnable();
	}
}
