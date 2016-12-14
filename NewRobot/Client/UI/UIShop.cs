using System;
using System.Collections.Generic;
using System.Text;
using NewRobot;
using Json;

public class ShopItemInfo
{
    public int itemType;
    public int itemID;
    public int itemCount;

    public int mPrice;
    public int mOffPrice;
    public int mBeginTime;
    public int mEndTime;
    public string mUUID;
    public Dictionary<int, LimitInfo> CurLimitInfos = new Dictionary<int, LimitInfo>();
    public int FinalMaxNum;

    public int mLimitLevel;
    public int mLimitVipLevel;
}

public struct ItemCommonInfo
{
    public int itemStoreID;
    public int itemStoreType;
    public int itemStoreNum;

    public ItemCommonInfo(int id, int type, int num)
    {
        itemStoreID = id;
        itemStoreType = type;
        itemStoreNum = num;
    }
}

public struct ItemRoleInfo
{
    public int id;
    public int job;
    public int itemType;
}

public class LimitInfo
{
    public int CurLimit;
    public int MaxLimit;

    public LimitInfo()
    {
    }

    public LimitInfo(int cur, int max)
    {
        CurLimit = cur;
        MaxLimit = max;
    }
}


public class UIShopData : UIData
{
    public UIShopData()
    {
        ProtocolFuns.getMallInfo();
    }


    public int mItemID = -1;
    public Dictionary<int, List<ShopItemInfo>> mItemDict = new Dictionary<int, List<ShopItemInfo>>();

    public static bool bHas = true;

    private int HasItem(List<ShopItemInfo> lst, int itemID)
    {
        for (int idx = 0; idx < lst.Count; idx++)
        {
            if (lst[idx].itemID == itemID)
            {
                return idx / 4;
            }
        }

        return -1;
    }

    public int IsBuyCount(Dictionary<int, LimitInfo> dict, bool vip)
    {
        int count = 0;
        if (dict.Count == 0)
        {
            count = 9999999;
        }
        else
        {
            foreach (KeyValuePair<int, LimitInfo> pair in dict)
            {
                if (!vip)
                {
                    if (pair.Key != 0 && pair.Key != 1)
                    {
                        if (count == 0 || count > pair.Value.CurLimit)
                            count = pair.Value.CurLimit;
                    }
                }
                else
                {
                    if (pair.Key == 4)
                    {
                        count = pair.Value.CurLimit;
                        break;
                    }
                }
            }

        }
        return count;
    }

    ShopItemInfo addItemInfo(JsonProperty commodity)
    {
        ShopItemInfo info = new ShopItemInfo();
        JsonProperty itemInfo = commodity[0];
        info.itemType = int.Parse(itemInfo[0].Value);
        info.itemID = int.Parse(itemInfo[1].Value);
        info.itemCount = int.Parse(itemInfo[2].Value);

        info.mPrice = int.Parse(commodity[1].Value);
        info.mOffPrice = int.Parse(commodity[2].Value);
        info.mBeginTime = int.Parse(commodity[3].Value);
        info.mEndTime = int.Parse(commodity[4].Value);


        JsonProperty limitInfoJson = commodity[5];
        for (int i = 0; i < limitInfoJson.Count; i++)
        {
            JsonProperty curLimit = limitInfoJson[i];
            int limitType = int.Parse(curLimit[0].Value);
            if (limitType == 0)
            {
                info.mLimitLevel = int.Parse(curLimit[1].Value);
            }
            else if (limitType == 1)
            {
                info.mLimitVipLevel = int.Parse(curLimit[1].Value);
            }
            else
            {
                LimitInfo limitInfo = new LimitInfo(int.Parse(curLimit[1].Value), int.Parse(curLimit[2].Value));
                info.CurLimitInfos[limitType] = limitInfo;
            }
        }

        info.mUUID = commodity[6].Value;

        return info;
    }

    public override void AnalyzeTextProtocol(List<TextProtocolData> dataList)
    {
        string action = dataList[0].mValue as string;
        if (action == "mallSuccess")
        {
            int mallType = (int)dataList[1].mValue;
            if (mItemDict.ContainsKey(mallType))
            {
                JsonObject mallInfo = new JsonObject(dataList[2].mValue as string);
                JsonProperty commodity = mallInfo["Commodity"];
                ShopItemInfo info = this.addItemInfo(commodity);

                List<ShopItemInfo> lst = mItemDict[mallType];
                for (int idx = 0; idx < lst.Count; idx++)
                {
                    if (lst[idx].mUUID == info.mUUID)
                    {
                        lst[idx] = info;
                        break;
                    }
                }
            }
        }
        else if (action == "mallFailed")
        {
            int mallType = (int)dataList[2].mValue;
        }
        else if (action == "mall")
        {
            mItemDict.Clear();
            JsonObject mallInfo = new JsonObject(dataList[1].mValue as string);
            foreach (JsonProperty item in mallInfo["Mall"].Items)
            {
                int mallType = int.Parse(item["MallType"].Value);
                foreach (JsonProperty commodity in item["Commodities"].Items)
                {
                    if (!mItemDict.ContainsKey(mallType))
                        mItemDict[mallType] = new List<ShopItemInfo>();

                    ShopItemInfo info = this.addItemInfo(commodity);
                    if (info.mLimitLevel > Robot.GetCurRobot().MyActorManager.mMyPlayerData.mLevel)
                        continue;

                    mItemDict[mallType].Add(info);
                }
            }
        }
    }
}