using System;
using System.Collections.Generic;
using System.Text;
using Json;
using NewRobot;

public enum FactionEventType
{
    None,
}

enum GuildNoticeType
{
    Unknown = 0,
    JoinGuild,                 // 某人加入公会（公会内群发
    QuitGuild,                 // 离开公会（某人离开了之类的，公会内群发
    DismissGuildMember,        // 某人被踢出公会（公会内群发
    ChangeGuildMaster,         // 会长发生了变化（公会内群发
    ApprovalGuildApplication,  // 审批了公会申请（只针对事发当事人
    InviteJoinGuild,           // 邀请加入公会（只针对被邀请人
    EditGuildAnnouncement,		//修改公会宣言
    GuildBuild,					//公会建设
    GuildSomeOne,				//新请求
};

public enum FactionIdentity
{
    Master = 5, //会长
    Deputy = 4,	//副会长
    Elder = 3,  //长老
    SubElder = 2, //执事
    Normal = 1,		//会员
}

public class FactionBriefInfo
{
    public int mRank;
    public int mLevel;
    public string factionName;
    public string bossName;
    public int memberCount;
    public int maxCount;
    public bool isApplyed;
}

public class FactionInfo
{
    public string mFactionName; 			    // 公会名
    public string mBossName;          		// 公会会长名
    public int mLevel;  					// 公会等级
    public int mExp;				       // 贡献值（公会经验
    public int mMaxExperience;           // 当前最大贡献值（最大公会经验
    public int mMemberNum;               // 成员个数
    public int mMemberMaxNum;            // 最大成员数量
    public int mFactionMoney;              // 可以获得的奖励
    public bool mCanGetMoney;              // 是否可以获得奖励
    public int mAnnouncementLength;      // 宣言长度（后跟宣言
    public string mAccounce;
    public int mFactionEventNum;           // 公会事件个数（后跟mFactionEventNum * FactionEventInfo
    public FactionIdentity mSelfPost;                 // 自己的职位		
}


public class FactionEventInfo
{
    public string mName;                 // 事件玩家名称
    public short mLevel;                   // 事件玩家等级
    public short mVipLevel;                // 事件玩家VIP等级
    public Job mJob;                      // 事件玩家职业

    public FactionEventType mType;                     // 事件类型
    public int[] mParams = new int[3];               // 事件参数
    public int mDescriptionLength;       				// 事件描述长度
    public string mDescription;							//事件描述
}

public class GuildMemberInfo
{
    public FactionIdentity mPost;                     // 职位
    public short mLevel;                   // 等级
    public Job mJob;                      // 职业
    public int mMeritorious;             // 贡献
    public string mPlayerName;           // 玩家名称
    public int mLastLogoutTime;          // 上次登出时间，如果在线，则是0
}

public class GuildApplicationInfo
{
    public string mName;                          	// 名字
    public short mLevel;                           // 等级
    public int mApplyTime;                       // 申请时间
}

public class GuildConstructInfo
{
    public int mLevel;                           // 公会等级
    public int mExperience;                      // 公会贡献（公会经验
    public int mMaxExperience;                   // 最大公会贡献（最大公会经验
    public int mGuildRewardStatus;               // 奖励状态
    public int mBuildGuildProgress;
    public int mFreeBuildTime;
    public int mGoldBuildTime;
    public int mFreeBuildExp;
    public int mGoldBuildExp;
    public int mGoldBuildCost;						// 元宝建设消耗
    public int mBuildPlayerInfo;					// 最近建设公会信息
}

public class BuildGuildInfo
{
    public string mplayerName;
    public int mgetExp;
    public int mTime;
}

public class TreasureReward
{
    public int needMin;
    public int needMax;

    public List<TreasureRewardData> rewardItems = new List<TreasureRewardData>();
}

public class TreasureInfo
{
    public int openTime;
    public int maxopenTime;
    public int myContribute;
    public int guildContribute;
    public int guildMaxContribute = 1;
    public int isOpen;
    public int openMoney;
    public int doubleMoney;
    public List<TreasureReward> rewardList = new List<TreasureReward>();
    public TreasureReward curTreasureReward = null;
    public List<int> rewardIds = new List<int>();
    public List<int> rewardResTypes = new List<int>();
}

public class TreasureRewardData
{
    public int type;
    public int id;
    public int num;
}

public class GuildCopyInfo
{
    public int id;
    public int hardLvl;
    public int contriVal;
}

public class UIFactionData : UIData
{
    public int myContribution;
    public int maxMemberNum;
    public int maxApplicationNum;
    public int maxGuildNum;
    public FactionInfo mFactionInfo = new FactionInfo();
    public List<FactionEventInfo> mFactionEventList = new List<FactionEventInfo>();
    public List<GuildMemberInfo> mGuildMemberList = new List<GuildMemberInfo>();
    public List<GuildApplicationInfo> mApplicationList = new List<GuildApplicationInfo>();
    public List<FactionBriefInfo> mFactionInfoList = new List<FactionBriefInfo>();
    public GuildConstructInfo mGuildConstructInfo;
    public List<BuildGuildInfo> mGuildBuildInfo = new List<BuildGuildInfo>();
    public List<GuildCopyInfo> mGuildCopyInfo = new List<GuildCopyInfo>();
    public float recordTime = 0;
    public TreasureInfo mTreasureInfo = new TreasureInfo();
    public RoleData remotePlayerRole = null;

    public static List<int> mStateList = new List<int>();

    public override void AnalyzeToData(string custom, byte[] data)
    {
        int offset = 0;
        S2CProtocol protocol = (S2CProtocol)data[offset];
        offset++;
        if (protocol == S2CProtocol.S2C_GetGuildInfo)
        {
            mFactionInfo.mFactionName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);
            offset += 64;
            mFactionInfo.mBossName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);
            offset += 64;
            mFactionInfo.mLevel = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mExp = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mMaxExperience = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mMemberNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mMemberMaxNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mFactionMoney = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mCanGetMoney = System.BitConverter.ToBoolean(data, offset);
            offset++;
            mFactionInfo.mAnnouncementLength = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mFactionEventNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mFactionInfo.mSelfPost = (FactionIdentity)data[offset];
            offset++;
            if (mFactionInfo.mAnnouncementLength > 0)
            {
                mFactionInfo.mAccounce = System.Text.Encoding.UTF8.GetString(data, offset, mFactionInfo.mAnnouncementLength).Replace("\0", string.Empty);
                offset += mFactionInfo.mAnnouncementLength;
            }
            //else
            //{
            //    mFactionInfo.mAccounce = StringsMgr.sctr_announceNull;
            //}
            //			for( int i = 0; i < mFactionInfo.mFactionEventNum; i++ )
            //			{
            //				FactionEventInfo eventInfo = new FactionEventInfo ();
            //				eventInfo.mName = System.Text.Encoding.UTF8.GetString (data, offset, 64).Replace ("\0", string.Empty);				offset += 64;
            //				eventInfo.mLevel = System.BitConverter.ToInt16( data, offset ); 		offset += sizeof(short);
            //				eventInfo.mVipLevel  = System.BitConverter.ToInt16( data, offset ); 		offset += sizeof(short);
            //				eventInfo.mJob = (Job)data[offset];											offset ++;
            //				eventInfo.mType = (FactionEventType)data[offset];							offset ++;
            //				for( int j = 0; j < 3; j++ )
            //				{
            //					eventInfo.mParams[j] =  System.BitConverter.ToInt32( data, offset ); 		offset += sizeof(int);
            //				}
            //				eventInfo.mDescriptionLength = System.BitConverter.ToInt32( data, offset ); 		offset += sizeof(int);
            //				if( eventInfo.mDescriptionLength > 0)
            //				{
            //					eventInfo.mDescription = System.Text.Encoding.UTF8.GetString (data, offset, eventInfo.mDescriptionLength).Replace ("\0", string.Empty);	offset += eventInfo.mDescriptionLength;
            //				}
            //				mFactionEventList.Add( eventInfo );
            //			}
            //if (mUI != null)
            //{
            //    //UIManager.Instance.mUIBaseWindow.CloseSelf = true;
            //    mUI.InitGuild();
            //}
        }
        else if (protocol == S2CProtocol.S2C_GetGuildMemberList)
        {
            int startIndex = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            maxMemberNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);					//最大成员数
            int mNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);                 // 个数（后跟mNum * GuildMemberInfo
            for (int i = 0; i < mNum; i++)
            {
                GuildMemberInfo memberInfo = new GuildMemberInfo();
                memberInfo.mPost = (FactionIdentity)data[offset];
                offset++;
                memberInfo.mLevel = System.BitConverter.ToInt16(data, offset);
                offset += sizeof(short);
                memberInfo.mJob = (Job)data[offset];
                offset++;
                memberInfo.mMeritorious = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                memberInfo.mPlayerName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);
                offset += 64;
                memberInfo.mLastLogoutTime = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                if (startIndex + i >= mGuildMemberList.Count)
                {
                    mGuildMemberList.Add(memberInfo);
                }
                else
                {
                    mGuildMemberList[startIndex + i] = memberInfo;
                }
            }
            //if (mUI != null)
            //{
            //    mUI.ResetMemberPageNum(maxMemberNum);
            //    mUI.initMemberPage(startIndex / 5);
            //    mUI.mMemberList.SwitchToPage(0,true);
            //}
        }
        else if (protocol == S2CProtocol.S2C_GetGuildApplicationList)
        {
            int startIndex = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            maxApplicationNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int num = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int i = 0;
            for (; i < num; i++)
            {
                GuildApplicationInfo applicationInfo = new GuildApplicationInfo();
                applicationInfo.mName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);
                offset += 64;
                applicationInfo.mLevel = System.BitConverter.ToInt16(data, offset);
                offset += sizeof(short);
                applicationInfo.mApplyTime = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                if (startIndex + i >= mApplicationList.Count)
                {
                    mApplicationList.Add(applicationInfo);
                }
                else
                {
                    mApplicationList[startIndex + i] = applicationInfo;
                }
                if (!mStateList.Contains(2) && mFactionInfo.mSelfPost != FactionIdentity.Normal)
                    mStateList.Add(2);
            }
            if (num == 0 && startIndex == 0)
                mStateList.Remove(2);

            for (int j = i; j < 5; j++)
            {
                if (j >= 0)
                {


                    int index = startIndex + j;
                    if (index < mApplicationList.Count)
                    {
                        mApplicationList.RemoveAt(index);
                    }
                }
            }
            //if (mUI != null)
            //{
            //    if (mUI.curSelectIndex == 2 || mUI.curSelectIndex == 3)
            //    {
            //        mUI.mNewMemberTip.SetActive(UIFactionData.mStateList.Contains(2));
            //    }
            //    mUI.initApplyPage(startIndex / 5);
            //}
        }
        else if (protocol == S2CProtocol.S2C_ApprovalGuildApplication)
        {
            Error err = (Error)data[offset];
            offset++;
            bool agree = System.BitConverter.ToBoolean(data, offset);
            offset += sizeof(bool);
            string name = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);

            if (err == Error.Err_NotExist)
            {
                //  UIManager.Instance.PushScreenTip(StringsMgr.sctr_applicationNotExist);
            }
            else if (err == Error.Err_Ok)
            {
                if (agree)
                {
                    //        string content = string.Format(StringsMgr.sctr_addMemberSucess, name);
                    //        UIManager.Instance.PushScreenTip(content);
                }
                else
                {
                    //    string content = string.Format(StringsMgr.sctr_refuseMemberSucess, name);
                    //   UIManager.Instance.PushScreenTip(content);
                }
            }
            else if (err == Error.Err_HaveThis)
            {
                //  UIManager.Instance.PushScreenTip(StringsMgr.sctr_haveGuild);
            }
            else if (err == Error.Err_Max)
            {
                // UIManager.Instance.PushScreenTip(StringsMgr.sctr_memberMax);
            }
        }
        else if (protocol == S2CProtocol.S2C_GetGuildList)
        {
            int mCreateMoney = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int mCreateGoldMoney = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int mCreateLevelLimitation = System.BitConverter.ToInt16(data, offset);
            offset += sizeof(short);
            int beginIndex = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int mFactionNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int protoFactionNum = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            if (protoFactionNum > 5)
            {
                protoFactionNum = 5;
            }
            maxGuildNum = mFactionNum;
            for (int i = 0; i < protoFactionNum; i++)
            {
                FactionBriefInfo briefInfo = new FactionBriefInfo();
                briefInfo.mRank = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                briefInfo.mLevel = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                briefInfo.factionName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);
                offset += 64;
                briefInfo.bossName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);
                offset += 64;
                briefInfo.memberCount = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                briefInfo.maxCount = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                briefInfo.isApplyed = System.BitConverter.ToBoolean(data, offset);
                offset++;
                if (beginIndex + i >= mFactionInfoList.Count)
                {
                    mFactionInfoList.Add(briefInfo);
                }
                else if (beginIndex + i > 0)
                {
                    mFactionInfoList[beginIndex + i] = briefInfo;
                }
            }
            //if (mUI != null)
            //{
            //    mUI.ResetFactionPageNum(mFactionNum);
            //    mUI.initFactionPage(beginIndex / 5);
            //}
        }
        else if (protocol == S2CProtocol.S2C_DismissGuildMember)
        {
            Error err = (Error)data[offset];
            if (err == Error.Err_Ok)
            {
                //     mGuildMemberList.RemoveAt(mUI.curSelectIndex);
                //   ProtocolFuns.GetGuildMember(mUI.mMemberList.CurPage, 5);
                //  mUI.selectEffect.SetActive(false);
            }
            else
            {
                //   UIManager.Instance.PushScreenTip(StringsMgr.sctr_dismissTips);
            }
        }
        else if (protocol == S2CProtocol.S2C_ChangeGuildMaster)
        {
            Error err = (Error)data[offset];
            if (err == Error.Err_Invalid)
            {
                //     UIManager.Instance.PushScreenTip(StringsMgr.sctr_transferFailed);
                return;
            }
            //if (mUI != null)
            //{
            //    mUI.OpenPanel(0);
            //}
        }
        else if (protocol == S2CProtocol.S2C_QuitGuild)
        {
            Error err = (Error)data[offset];
            if (err == Error.Err_Ok)
            {
                UIFactionData.mStateList.Clear();
                //   GameClient.Instance.MyActorManager.SetGuildName("");
                //    UIManager.Instance.CloseUI(mUI.name);
            }
        }
        else if (protocol == S2CProtocol.S2C_GetGuildConstructInfo)
        {
            mGuildConstructInfo = new GuildConstructInfo();
            mGuildConstructInfo.mLevel = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mExperience = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mMaxExperience = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mGuildRewardStatus = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mBuildGuildProgress = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mFreeBuildTime = System.BitConverter.ToInt32(data, offset);
            if (mGuildConstructInfo.mFreeBuildTime > 0)
            {
                if (!mStateList.Contains(4))
                    mStateList.Add(4);
            }
            else
                mStateList.Remove(4);
            offset += sizeof(int);
            mGuildConstructInfo.mGoldBuildTime = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mFreeBuildExp = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mGoldBuildExp = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mGoldBuildCost = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildConstructInfo.mBuildPlayerInfo = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            mGuildBuildInfo.Clear();
            for (int i = 0; i < mGuildConstructInfo.mBuildPlayerInfo; i++)
            {
                BuildGuildInfo buildInfo = new BuildGuildInfo();
                buildInfo.mplayerName = System.Text.Encoding.UTF8.GetString(data, offset, 64).Replace("\0", string.Empty);
                offset += 64;
                buildInfo.mgetExp = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                buildInfo.mTime = System.BitConverter.ToInt32(data, offset);
                offset += sizeof(int);
                mGuildBuildInfo.Add(buildInfo);
            }
            //if (mUI != null)
            //{
            //    mUI.UpdateGuildContributionInfo();
            //}
        }
        else if (protocol == S2CProtocol.S2C_BuildGuild)
        {
            Error err = (Error)data[offset];
            if (err == Error.Err_Ok)
            {
                //ProtocolFuns.GetGuildConstructInfo ();
            }
            else if (err == Error.Err_NotEnoughGoldMoney)
            {
                //    UIManager.Instance.PushScreenTip(StringsMgr.sctr_notEnoughBuildMoney);
            }
            else if (err == Error.Err_NotEnoughCount)
            {
                //    UIManager.Instance.PushScreenTip(StringsMgr.sctr_maxTime);
            }
            else
            {
                //       UIManager.Instance.PushScreenTip(err.ToString());
            }
        }
        else if (protocol == S2CProtocol.S2C_GetGuildReward)
        {
            Error err = (Error)data[offset];
            if (err == Error.Err_Ok)
            {
                //  mUI.getRewardpanel.SetActive(false);
                //   ProtocolFuns.GetGuildConstructInfo();
                //   UIManager.Instance.PushScreenTip(StringsMgr.sctr_getRewardSucess);
            }
            else if (err == Error.Err_Used)
            {
                //  UIManager.Instance.PushScreenTip(StringsMgr.sctr_rewardUsed);
            }
            else
            {
                //     UIManager.Instance.PushScreenTip(err.ToString());
            }
        }
        else if (protocol == S2CProtocol.S2C_ModifyGuildAnnouncement)
        {
            Error err = (Error)data[offset];
            if (err == Error.Err_Ok)
            {
                //     mUI.inputPanel.SetActive(false);
                //    UIManager.Instance.PushScreenTip(StringsMgr.sctr_modifySucess);
                //     ProtocolFuns.GetMyGuildInfo();
            }
            else if (err == Error.Err_PermissionLimitation)
            {
                //    mUI.inputPanel.SetActive(false);
                //   UIManager.Instance.PushScreenTip(StringsMgr.sctr_permissionLimitation);
            }
            else if (err == Error.Err_Invalid)
            {
                //      mUI.inputPanel.SetActive(false);
                //      UIManager.Instance.PushScreenTip(StringsMgr.sctr_unknownWord);
            }
            else
            {
                //       UIManager.Instance.PushScreenTip(err.ToString());
            }
        }
        else if (protocol == S2CProtocol.S2C_FindPlayerInfo)
        {
            //  Activity.eActivityID id = (Activity.eActivityID)data[offset]; offset++;
            //  if (id != Activity.eActivityID.AID_GuildCopy) return;
            PlayerDataBase remotePlayer = new PlayerDataBase(data, ref offset, false, false);
            RoleData role = null;
            int batPoint = 0;
            foreach (int key in remotePlayer.mRoleData.Keys)
            {
                if (remotePlayer.mRoleData[key].mBattlePoint > batPoint)
                {
                    role = remotePlayer.mRoleData[key];
                    batPoint = role.mBattlePoint;
                }
            }
            remotePlayerRole = role;
        }
        else if (protocol == S2CProtocol.S2C_DoGuildActivityAction)
        {
            int mId = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int length = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            string content = System.Text.Encoding.UTF8.GetString(data, offset, length).Replace("\0", string.Empty);
            if (mId == 10)
            {
                JsonObject jObj = new JsonObject(content);
                if (jObj["Result"].ToString() == "0")
                {
                    if (int.Parse(jObj["type"].ToString()) == 1)
                    {
                        //       int itemId = int.Parse(jObj["Reward"][1].ToString());
                        //       SetSprintInfo.SetItemIcon(mUI.prayitem, itemId, false);
                        //       mUI.prayRewardPanel1.SetActive(true);
                        //       mUI.prayRewardPanel2.SetActive(false);
                    }
                    else
                    {
                        for (int i = 0; i < jObj["Reward"].Count && i < 10; i++)
                        {
                            int itemId = int.Parse(jObj["Reward"][i][1].ToString());
                            //      SetSprintInfo.SetItemIcon(mUI.prayitems[i], itemId, false);
                        }
                        //    mUI.prayRewardPanel2.SetActive(true);
                        //    mUI.prayRewardPanel1.SetActive(false);
                    }
                }
                else if (jObj["Result"].ToString() == "5")
                {
                    //  UIManager.Instance.PushScreenTip(StringsMgr.sctr_bagMax);
                }
                else if (jObj["Result"].ToString() == "3")
                {
                    //     UIManager.Instance.PushScreenTip(StringsMgr.cstr_notEnoughContribution);
                }
                // ProtocolFuns.GetActivityActionInfo(10);
            }
            else if (mId == 7)
            {
                JsonObject jObj = new JsonObject(content);
                //   UIManager.Instance.PushScreenTip(jObj["ResultDesc"].ToString());
                //    ProtocolFuns.GetActivityActionInfo(7);
            }
            else if (mId == 44)
            {
                JsonObject jObj = new JsonObject(content);
                if (jObj["Result"].ToString() == "0")
                {
                    if (jObj["do"].ToString().Contains("open"))
                    {
                        //       ProtocolFuns.GetActivityActionInfo(44);
                    }
                    else if (jObj["do"].ToString().Contains("double"))
                    {
                        //        mUI.IsDoule = true;
                    }
                    else
                    {
                        //        if (mUI != null)
                        //         {
                        //             mUI.EnterCopy();
                        //         }
                    }
                }
                else
                {
                    //     UIManager.Instance.PushScreenTip(jObj["ResultDesc"].Value.ToString());
                }
            }
        }
        else if (protocol == S2CProtocol.S2C_GetGuildActivityInfo)
        {
            int mId = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            int length = System.BitConverter.ToInt32(data, offset);
            offset += sizeof(int);
            string content = System.Text.Encoding.UTF8.GetString(data, offset, length).Replace("\0", string.Empty);
            if (mId == 10)
            {
                JsonObject jObj = new JsonObject(content);
                myContribution = int.Parse(jObj["contribute"].ToString());

                string record = "";
                if (jObj["records"].Count > 0)
                {
                    for (int i = 0; i < jObj["records"].Count; i++)
                    {
                        record += jObj["records"][i] + "\n";
                    }
                }
                record = record.Replace("\"", "");
                //if (mUI != null)
                //{
                //    mUI.prayEvent.text = record;
                //    //					if(int.Parse(jObj["contribute"].Value) > 100)
                //    //					{
                //    //						if(!mStateList.Contains(5))
                //    //							mStateList.Add(5);
                //    //					}
                //    //					else
                //    //						mStateList.Remove(5);

                //    mUI.myContribution.text = string.Format(StringsMgr.sctr_curContribution, jObj["contribute"]);

                //}
            }
            else if (mId == 44)
            {
                JsonObject jobj = new JsonObject(content);
                //if (jobj["Result"].ToString() != "0")
                //{
                //    UIManager.Instance.PushScreenTip(jobj["ResultDesc"].Value.ToString());
                //    if (mUI != null)
                //    {
                //        mUI.bOPenTreasure = false;
                //    }
                //    return;
                //}
                mTreasureInfo.curTreasureReward = null;
                mTreasureInfo.openTime = int.Parse((jobj["jointime"][0]).ToString());
                mTreasureInfo.maxopenTime = int.Parse((jobj["jointime"][1]).ToString());
                mTreasureInfo.myContribute = int.Parse(jobj["myContribute"].ToString());
                mTreasureInfo.guildContribute = int.Parse(jobj["guildContribute"].ToString());
                mTreasureInfo.isOpen = int.Parse(jobj["isOpen"].ToString());
                if (mTreasureInfo.isOpen > 0)
                {
                    if (!mStateList.Contains(3))
                        mStateList.Add(3);
                }
                else
                    mStateList.Remove(3);

                mTreasureInfo.rewardList.Clear();
                if (jobj["rewards"].Count > 0)
                {
                    for (int i = 0; i < jobj["rewards"].Count; i++)
                    {
                        TreasureReward tr = new TreasureReward();
                        tr.needMin = int.Parse(jobj["rewards"][i][0].ToString());
                        tr.needMax = int.Parse(jobj["rewards"][i][1].ToString());
                        if (tr.needMax > mTreasureInfo.guildMaxContribute)
                        {
                            mTreasureInfo.guildMaxContribute = tr.needMax;
                        }
                        for (int j = 0; j < jobj["rewards"][i][2].Count; j++)
                        {
                            TreasureRewardData trd = new TreasureRewardData();
                            trd.type = int.Parse(jobj["rewards"][i][2][j][0].ToString());
                            trd.id = int.Parse(jobj["rewards"][i][2][j][1].ToString());
                            trd.num = int.Parse(jobj["rewards"][i][2][j][2].ToString());
                            tr.rewardItems.Add(trd);
                            if (!mTreasureInfo.rewardIds.Contains(trd.id))
                            {
                                mTreasureInfo.rewardIds.Add(trd.id);
                            }
                        }
                        for (int k = 0; k < jobj["rewards"][i][3].Count; k++)
                        {
                            TreasureRewardData trd = new TreasureRewardData();
                            trd.id = 0;
                            trd.type = int.Parse(jobj["rewards"][i][3][k][0].ToString());
                            trd.num = int.Parse(jobj["rewards"][i][3][k][1].ToString());
                            tr.rewardItems.Add(trd);
                            if (!mTreasureInfo.rewardResTypes.Contains(trd.type))
                            {
                                mTreasureInfo.rewardResTypes.Add(trd.type);
                            }
                        }
                        if (mTreasureInfo.guildContribute >= tr.needMin && mTreasureInfo.guildContribute <= tr.needMax)
                        {
                            mTreasureInfo.curTreasureReward = tr;
                        }
                        mTreasureInfo.rewardList.Add(tr);
                    }
                }

                mTreasureInfo.openMoney = int.Parse(jobj["openMoney"].ToString());
                mTreasureInfo.doubleMoney = int.Parse(jobj["doubleMoney"].ToString());

                mGuildCopyInfo.Clear();
                for (int i = 0; i < jobj["dungeon"].Count; i++)
                {
                    GuildCopyInfo copyInfo = new GuildCopyInfo();
                    copyInfo.id = int.Parse(jobj["dungeon"][i][0].ToString());
                    copyInfo.hardLvl = int.Parse(jobj["dungeon"][i][1].ToString());
                    copyInfo.contriVal = int.Parse(jobj["dungeon"][i][2].ToString());
                    mGuildCopyInfo.Add(copyInfo);
                }
                //if (mUI != null)
                //{
                //    if (mUI.bOPenTreasure)
                //    {
                //        mUI.OpenPanel(6);
                //        mUI.bOPenTreasure = false;
                //    }
                //    mUI.RefreshTreasurePanel();
                //}
            }
        }
    }
}
