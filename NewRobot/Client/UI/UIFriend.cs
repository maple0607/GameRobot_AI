using System.Collections;
using System.Collections.Generic;
using System.Text;
using Json;
using NewRobot;

public class UIFriendListData : UIData
{

    public class PlayerFriendInfo
    {
        public int mIndex;                       // 玩家索引
        public bool mIsOnline;                   // 是否在线
        public string mUUID;					  // UUID
        public string mName;                     // 姓名
        public string mGuild;					//所在公会
        public byte mIcon;						// 头像
        public short mLevel;                     // 等级
        public short mVipLevel;                  // Vip等级
        public int mBattlePoint;					//战斗力
        public bool canSend;						//是否可以赠送体力
        //public bool mIsOtherFriend;              // 自己是否对方好友

    }

    public class FriendGiftItem
    {
        public string giverName;
        public int giftNum;
        public string giftTime;
    }

    public class FriendRequest
    {
        public string mName;					//玩家索引
        public int mTime;					//申请时间
        public short mLevel;					//等级
    }

    public class RecommendInfo
    {
        public int mIndex;						// 玩家索引
        public string mUUID;						// UUID
        public string mName;						// 姓名
        public string mGuildName;				// 公会名字
        public byte mIcon;							// 头像
        public short mLevel;						// 等级
        public short mVipLevel;					// Vip等级
        public int mBattlePoint;					// 战力
    }


    public int mMaxFriendNum;
    public int mOnlineFriendNum;
    public int mFriendNum;
    public int mRequesetNum;
    public int mRecommendNum;
    public int mRequest;
    public List<PlayerFriendInfo> mFriendInfoList = new List<PlayerFriendInfo>();
    public List<FriendRequest> mFriendRequestList = new List<FriendRequest>();
    public List<PlayerFriendInfo> mRecommendList = new List<PlayerFriendInfo>();
    public List<FriendGiftItem> mFriendGiftList = new List<FriendGiftItem>();
    public int mGiftTime = 3;
    public int mLeftGetTime = 0;

    public override void AnalyzeToData(string custom, byte[] data)
    {
        int offset = 0;
        S2CProtocol protocol = (S2CProtocol)data[offset];
        offset++;
        if (protocol == S2CProtocol.S2C_GetFriendList)
        {
            mMaxFriendNum = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mOnlineFriendNum = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mFriendNum = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mRequesetNum = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mRecommendNum = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
            mFriendInfoList.Clear();
            for (int i = 0; i < mFriendNum; i++)
            {
                PlayerFriendInfo friendInfo = new PlayerFriendInfo();
                friendInfo.mIndex = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                friendInfo.mIsOnline = System.BitConverter.ToBoolean(data, offset); offset += sizeof(bool);
                friendInfo.mUUID = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                friendInfo.mName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                friendInfo.mGuild = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                //friendInfo.mIcon = System.BitConverter.ToBoolean( data, offset );		
                offset += sizeof(byte);
                friendInfo.mLevel = System.BitConverter.ToInt16(data, offset); offset += sizeof(short);
                friendInfo.mVipLevel = System.BitConverter.ToInt16(data, offset); offset += sizeof(short);
                friendInfo.mBattlePoint = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                friendInfo.canSend = (int)data[offset] == 0; offset++;
                //friendInfo.mIsOtherFriend = System.BitConverter.ToBoolean( data, offset );
               // offset += sizeof(bool);
                mFriendInfoList.Add(friendInfo);
            }
            mFriendInfoList.Sort(FriendSortFun);
            mFriendRequestList.Clear();
            for (int i = 0; i < mRequesetNum; i++)
            {
                FriendRequest friendRequest = new FriendRequest();
                friendRequest.mName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                friendRequest.mTime = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                friendRequest.mLevel = System.BitConverter.ToInt16(data, offset); offset += sizeof(short);
                mFriendRequestList.Add(friendRequest);
            }
            mRecommendList.Clear();
            //			Debug.LogError( mRecommendNum );
            for (int i = 0; i < mRecommendNum; i++)
            {
                PlayerFriendInfo recommend = new PlayerFriendInfo();
                recommend.mIndex = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                recommend.mIsOnline = System.BitConverter.ToBoolean(data, offset); offset += sizeof(bool);
                recommend.mUUID = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                recommend.mName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                recommend.mGuild = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                //friendInfo.mIcon = System.BitConverter.ToBoolean( data, offset );		
                offset += sizeof(byte);
                recommend.mLevel = System.BitConverter.ToInt16(data, offset); offset += sizeof(short);
                recommend.mVipLevel = System.BitConverter.ToInt16(data, offset); offset += sizeof(short);
                recommend.mBattlePoint = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
                bool canSend = (int)data[offset] == 0; offset++;
                //friendInfo.mIsOtherFriend = System.BitConverter.ToBoolean( data, offset );
                mRecommendList.Add(recommend);
            }
        }
        else if (protocol == S2CProtocol.S2C_RemoveFriend)
        {
            Error err = (Error)data[offset]; offset++;
            string name = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
            if (err == Error.Err_Ok)
            {
                //UIManager.Instance.PushScreenTip(StringsMgr.sctr_DeleteFriendSucess);
                for (int i = 0; i < mFriendInfoList.Count; i++)
                {
                    if (mFriendInfoList[i].mName == name)
                    {
                        mFriendInfoList.RemoveAt(i);
                   //     mUI.RefreshCurPage();
                        return;
                    }
                }
            }
            else
            {
              //  UIManager.Instance.ShowMessageBox(StringsMgr.sctr_DeleteFriendFailed, eBtnType.Single, null);
            }

        }
        else if (protocol == S2CProtocol.S2C_AgreeFriend)
        {

            Error err = (Error)data[offset]; offset++;
            bool agree = System.BitConverter.ToBoolean(data, offset); offset++;
            if (err == Error.Err_Ok)
            {
                if (agree)
                {
                    string name = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                    //string content = string.Format(StringsMgr.sctr_AddFriendSucessed, name);
                    //	UIManager.Instance.ShowMessageBox (content, eBtnType.Single, null);
                    for (int i = 0; i < mFriendRequestList.Count; i++)
                    {
                        if (mFriendRequestList[i].mName == name)
                        {
                            mFriendRequestList.RemoveAt(i);
                          //  mUI.RefreshCurPage();
                          //  UIManager.Instance.PushScreenTip(content);
                            return;
                        }
                    }
                }
                else
                {
                    string name = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
                    //string content = string.Format(StringsMgr.sctr_refuseFriendSucessed, name);
                    //	UIManager.Instance.ShowMessageBox (content, eBtnType.Single, null);
                    for (int i = 0; i < mFriendRequestList.Count; i++)
                    {
                        if (mFriendRequestList[i].mName == name)
                        {
                            mFriendRequestList.RemoveAt(i);
                         //   mUI.RefreshCurPage();
                          //  UIManager.Instance.PushScreenTip(content);
                            return;
                        }
                    }
                }
            }
            else if (err == Error.Err_NotExist)
            {
                string name = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty); offset += 64;
               // string content = string.Format(StringsMgr.sctr_agreeFailed, name);
                for (int i = 0; i < mFriendRequestList.Count; i++)
                {
                    if (mFriendRequestList[i].mName == name)
                    {
                        mFriendRequestList.RemoveAt(i);
                    //    mUI.RefreshCurPage();
                    //    UIManager.Instance.PushScreenTip(content);
                        return;
                    }
                }
            }
        }
        else if (protocol == S2CProtocol.S2C_InviteJoinGuild)
        {
            Error err = (Error)data[offset]; offset++;
            if (err == Error.Err_Ok)
            {
             //   UIManager.Instance.PushScreenTip(StringsMgr.sctr_guildInviteSend);
            }
            else if (err == Error.Err_HaveThis)
            {
              //  UIManager.Instance.PushScreenTip(StringsMgr.sctr_guildJionedOther);
            }
            else if (err == Error.Err_Invalid)
            {
               // UIManager.Instance.PushScreenTip(StringsMgr.sctr_inviteFailed);
            }
            else
            {
                //UIManager.Instance.PushScreenTip(StringsMgr.sctr_inviteFailed);
            }
        }
        else if (protocol == S2CProtocol.S2C_GetActivityInfo)
        {
            string format = "yyyy年MM月dd日 HH:mm";
            int mId = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int length = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            string content = System.Text.Encoding.UTF8.GetString(data, offset, length).Replace("\0", string.Empty);
            if (mId == 46)
            {
                mFriendGiftList.Clear();
                HasFriendGift = false;
                JsonObject jobj = new JsonObject(content);
                for (int i = 0; i < jobj["rewards"].Count; i++)
                {
                    string name = jobj["rewards"][i][0].GetValue().ToString();
                    double time = double.Parse(jobj["rewards"][i][1].ToString());
                  //  string dateTime = Global.ConvertIntDateTime(time).ToString(format);
                    FriendGiftItem gift = new FriendGiftItem();
                    gift.giverName = name;
                  //  gift.giftTime = dateTime;
                    mFriendGiftList.Add(gift);
                }
                mGiftTime = int.Parse(jobj["onepoint"].ToString());
                mLeftGetTime = int.Parse(jobj["lefttime"].ToString());
             //   if (mUI != null)
           //     {
            //        mUI.RefreshFriendGift();
             //   }
                if (mFriendGiftList.Count > 0)
                    HasFriendGift = true;
            }

        }
        else if (protocol == S2CProtocol.S2C_DoActivityAction)
        {
            int mId = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int length = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            string content = System.Text.Encoding.UTF8.GetString(data, offset, length).Replace("\0", string.Empty);
            if (mId == 46)
            {
                JsonObject jobj = new JsonObject(content);
                if (jobj["do"].GetValue().ToString() == "give")
                {
                    string name = jobj["name"].GetValue().ToString();
                    for (int i = 0; i < mFriendInfoList.Count; i++)
                    {
                        if (mFriendInfoList[i].mName == name)
                        {
                            mFriendInfoList[i].canSend = false;
                        //    UIManager.Instance.PushScreenTip(StringsMgr.sctr_GiftSend);
                        //    if (mUI != null)
                        //    {
                        //        mUI.RefreshCurPage();
                       //     }
                            return;
                        }
                    }
                }
                else if (jobj["do"].GetValue().ToString() == "get")
                {
                    if (jobj["gettype"].GetValue().ToString() == "0")
                    {
                        string name = jobj["name"].GetValue().ToString();
                        for (int i = 0; i < mFriendGiftList.Count; i++)
                        {
                            if (mFriendGiftList[i].giverName == name)
                            {
                             //   UIManager.Instance.PushScreenTip(jobj["ResultDesc"].GetValue().ToString());
                                if (jobj["Result"].GetValue().ToString() == "0")
                                {
                                    mFriendGiftList.RemoveAt(i);
                                    ProtocolFuns.GetActivityInfo(46);
                                }
                                return;
                            }
                        }
                    }
                    else
                    {
                    //    UIManager.Instance.PushScreenTip(jobj["ResultDesc"].GetValue().ToString());
                        mFriendGiftList.Clear();
                        ProtocolFuns.GetActivityInfo(46);
                    }
                }
            }
        }
    }

    private int FriendSortFun(PlayerFriendInfo info1, PlayerFriendInfo info2)
    {
        int result = 0;
        if (info1.mIsOnline & !info2.mIsOnline)
        {
            result = -1;
        }
        if (!info1.mIsOnline & info2.mIsOnline)
        {
            result = 1;
        }
        return result;
    }

    public static bool HasFriendGift = false;

    //public static SystemState getState()
    //{
    //    if (HasFriendGift)
    //        return SystemState.State_CanHandled;
    //    if (UIManager.Instance.HasUIData("UIFriendList"))
    //    {
    //        UIFriendListData data = UIManager.Instance.GetUIData("UIFriendList") as UIFriendListData;

    //        if (data.mFriendRequestList.Count > 0)
    //        {
    //            return SystemState.State_CanHandled;
    //        }
    //    }
    //    return SystemState.State_None;
    //}
}


