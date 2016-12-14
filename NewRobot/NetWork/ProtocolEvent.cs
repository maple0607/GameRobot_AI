using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

//-------------------------------------------------文本协议信息---------------------------------------------//
public enum TextProtocolDataType
{
	TPDT_Integer = 0,                   // Int32
	TPDT_Float,                         // float
	TPDT_String,                        // String
};

public class TextProtocolData
{
	public TextProtocolDataType mType;
	public object mValue;
};

// 往服务器发送的       
public class TextProtocol
{
	public byte[] mData;
	public int mOffset;
	
	public TextProtocol(string name)
	{
		mData = new byte[1024 * 512];
		mOffset = 0;
		
		mData[mOffset] = (byte)(TextProtocolDataType.TPDT_String); mOffset += 1;
		byte[] nameData = Encoding.UTF8.GetBytes(name);
		Array.Copy(BitConverter.GetBytes(nameData.Length), 0, mData, mOffset, sizeof(Int32)); mOffset += sizeof(Int32);
		Array.Copy(nameData, 0, mData, mOffset, nameData.Length); mOffset += nameData.Length;
	}
	
	public void push(int value)
	{
		mData[mOffset] = (byte)(TextProtocolDataType.TPDT_Integer); mOffset += 1;
		Array.Copy(BitConverter.GetBytes(value), 0, mData, mOffset, sizeof(Int32)); mOffset += sizeof(Int32);
	}
	public void push(float value)
	{
		mData[mOffset] = (byte)(TextProtocolDataType.TPDT_Float); mOffset += 1;
		Array.Copy(BitConverter.GetBytes(value), 0, mData, mOffset, sizeof(float)); mOffset += sizeof(float);
	}
	public void push(string value)
	{
		mData[mOffset] = (byte)(TextProtocolDataType.TPDT_String); mOffset += 1;
		byte[] data = Encoding.UTF8.GetBytes(value);
		Array.Copy(BitConverter.GetBytes(data.Length), 0, mData, mOffset, sizeof(Int32)); mOffset += sizeof(Int32);
		Array.Copy(data, 0, mData, mOffset, data.Length); mOffset += data.Length;
	}
};


public class ProtocolEvent
{
	public const int ProtocolHeaderSize = 1;
	
    private delegate void ProtocolHandler(byte[] data);
    private Dictionary<S2CProtocol, ProtocolHandler> mProtocols; 
	private Dictionary<S2CProtocol, string> mEventToUIName; 
	private Dictionary<C2SProtocol, string> mResaultToUI;
	private Dictionary<Activity.eActivityID, string> mActEventToUI;

	public delegate void TextProtocolHandler(string name, List<TextProtocolData> dataList);

    public delegate void PPing();
    public delegate void PWaiting(int order);
	public delegate void PLogin(bool login, bool created, bool forbidden, string extraData, string sessionID);
	public delegate void PDayChanged();
	public delegate void PEventToUI(string uiname, string custom, byte[] data);
	public delegate void PEnterScene(byte result, int mapID,int sceneIdx);
	public delegate void PGetPlayerInfo(byte[] data, ref int offset);
    public delegate void PGetRoleInfo(byte[] data, ref int offset);
    public delegate void PGetPetInfo(byte[] data, ref int offset);
    public delegate void PGetBagData(byte[] data, ref int offset);
	public delegate void PGetDungeonList(byte[] data, ref int offset);
	public delegate void PGetBossDungeonList(byte[] data, ref int offset);
	public delegate void PEquipItem(Error result, int itemIndex, int roleIndex,ItemInfo info);
	public delegate void PUnequipItem(bool ok, int roleIndex, int equipPosition);

	public delegate void PFinishScene(byte[] data, ref int offset);
	public delegate void PRadisScene(byte[] data ,int offset);

	public delegate void PSellItem(bool mResult);
	public delegate void PGetTaskList(List<TaskInfoUpdate> info);
	public delegate void PCompleteTask(int id,bool complete);
	public delegate void PNotifyTask(TaskInfoUpdate info);

	public delegate void PNotifyModified(ModifiedAttributeType attrType, int value, int absValue);
	public delegate void PNotifyAddItem(byte[] data, ref int offset);
	public delegate void PGetGoldUseage(byte[] data);
	public delegate void PUseGoldMoney(byte[] data);

    public delegate void PIAPGetOrderSerial(string productID, string orderSerial);
    public delegate void PIAPCanMakePayments(bool canMakePayments);

	public delegate void PSynchronize(byte[] data, ref int offset);
	public delegate void PCreateRoom(byte[] data, ref int offset);
	public delegate void PGetRoomList(byte[] data, ref int offset);
	public delegate void PJoinRoom(byte[] data, ref int offset);
	public delegate void PLeaveRoom(byte[] data, ref int offset);
	public delegate void PStartSyn(byte[] data, ref int offset);
	public delegate void PEndSyn(byte[] data, ref int offset);
	public delegate void PNotifyRoom(byte[] data, ref int offset);
	public delegate void PJoinSearch(byte[] data, ref int offset);
	public delegate void PCancelSearch(byte[] data, ref int offset);

	public delegate void PGetActivityList(List<Activity> activities);
	public delegate void PGetActivityInfo(byte[] data);
    public delegate void PDoActivity(byte[] data);

	public delegate void PPlayerIn(byte[] data);
	public delegate void PPlayerOut(byte[] data);
	public delegate void PPlayerMoveTo(byte[] data);
	public delegate void PPlayerEnterScene(byte[] data);

	public delegate void PMoveTo(byte[] data);
	public delegate void PGuildNotice(byte[] data);

	public delegate void PMelting(byte[] data);

	public delegate void PGetArenaTop(byte[] data);
	public delegate void PKick();

	public delegate void PGetFriendData(byte[] data);
	public delegate void POnUIClosed (byte[] data);

    public event PPing OnPing;
    public event PWaiting OnWaiting;
    public event PLogin OnLogin;
	public event PDayChanged OnDayChanged;
	public event PEventToUI OnEventToUI;
	
	public event PGetPlayerInfo OnGetPlayerInfo;
    public event PGetRoleInfo OnGetRoleInfo;
    public event PGetPetInfo OnGetPetInfo;
	public event PGetDungeonList OnGetDungeonList;
	public event PGetBossDungeonList OnGetBossDungeonList;
	public event PGetBagData OnGetBagData;
	public event PEquipItem OnEquipItem;
	public event PUnequipItem OnUnequipItem;
    public event PEnterScene OnEnterScene;
    public event PFinishScene OnFinishScene;
	public event PRadisScene OnRadisScene;
    public event PSellItem OnSellItem;

	public event PGetTaskList OnGetTaskList;
	public event PCompleteTask OnCompleteTask;
	public event PNotifyTask OnNotifyTask;

	public event PNotifyModified OnNotifyModified;
	public event PNotifyAddItem OnNotifyAddItem;
	public event PGetGoldUseage OnGetGoldUseage;
	public event PUseGoldMoney OnUseGoldMoney;

    
    public event PIAPGetOrderSerial OnIAPGetOrderSerial;
    public event PIAPCanMakePayments OnIAPCanMakePayments;

	public event PSynchronize OnSynchronize;
	public event PCreateRoom OnCreateRoom;
	public event PGetRoomList OnGetRoomList;
	public event PJoinRoom OnJoinRoom;
	public event PLeaveRoom OnLeaveRoom;
	public event PStartSyn OnStartSyn;
	public event PEndSyn OnEndSyn;
	public event PNotifyRoom OnNotifyRoom;
	public event PJoinSearch OnJoinSearch;
	public event PCancelSearch OnCancelSearch;
	
	public event PGetActivityList OnGetActivityList;
	public event PGetActivityInfo OnGetActivityInfo;
    public event PDoActivity OnDoActivityAction;


	public event PPlayerIn OnPlayerIn;
	public event PPlayerOut OnPlayerOut;
	public event PPlayerMoveTo OnPlayerMoveTo;
	public event PMoveTo OnMoveTo;
	public event PPlayerEnterScene OnPlayerEnterScene;

	public event PGuildNotice OnGuildNotice;

	public event PMelting OnMeltingItem;

	public event PGetArenaTop OnGetAranaTop;
	public event PKick OnKick;

	public event PGetFriendData OnGetFriendData;

	public event POnUIClosed OnClosedUI;

    public event TextProtocolHandler OnTextProtocol;


  //  public event PLottery OnLottery;
    public ProtocolEvent()
    {
        mProtocols = new Dictionary<S2CProtocol, ProtocolHandler>();
		mEventToUIName = new Dictionary<S2CProtocol, string>();
		mResaultToUI = new Dictionary<C2SProtocol, string>();
		mActEventToUI = new Dictionary<Activity.eActivityID, string> ();

        mProtocols[S2CProtocol.S2C_Ping] = new ProtocolHandler(onPing);
		mProtocols[S2CProtocol.S2C_Waiting] = new ProtocolHandler(onWaiting);
        mProtocols[S2CProtocol.S2C_Login] = new ProtocolHandler(onLogin);
		mProtocols[S2CProtocol.S2C_ReLogin] = new ProtocolHandler(onReLogin);
		mProtocols[S2CProtocol.S2C_DayChanged] = new ProtocolHandler(onDayChanged);
        mProtocols[S2CProtocol.S2C_Create] = new ProtocolHandler(onCreat);
		mProtocols[S2CProtocol.S2C_SelectPlayer] = new ProtocolHandler(onSelectPlayer);
		mProtocols[S2CProtocol.S2C_UseRole] = new ProtocolHandler(onUseRole);

		mProtocols[S2CProtocol.S2C_UseGoldMoney] = new ProtocolHandler(onUseGoldMoney);

		mProtocols[S2CProtocol.S2C_TextProtocol] = new ProtocolHandler(onTextProtocol);
		
		mProtocols[S2CProtocol.S2C_IAPCanMakePayments] = new ProtocolHandler(onIAPCanMakePayments);

		mProtocols[S2CProtocol.S2C_Chat] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_MoveTo] = new ProtocolHandler(onMoveTo);
		mProtocols[S2CProtocol.S2C_PlayerMoveIn] = new ProtocolHandler(onPlayerMoveIn);
		mProtocols[S2CProtocol.S2C_PlayerMoveOut] = new ProtocolHandler(onPlayerMoveOut);
		mProtocols[S2CProtocol.S2C_PlayerMoveTo] = new ProtocolHandler(onPlayerMoveTo);
		
		mProtocols[S2CProtocol.S2C_DropItem] = new ProtocolHandler(onDropItem);
		mProtocols[S2CProtocol.S2C_SellItem] = new ProtocolHandler(onSellItem);
		mProtocols[S2CProtocol.S2C_BuyItem] = new ProtocolHandler(onBuyItem);
		
		mProtocols[S2CProtocol.S2C_UpgradeItem] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_AdvanceItem] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_AtlasLoot] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_CombineItem] = new ProtocolHandler(onCombineItem);
		mProtocols[S2CProtocol.S2C_GetCombineItemInfo] = new ProtocolHandler(onGeneralDataDeal);

        mProtocols[S2CProtocol.S2C_EquipStone] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_UnequipStone] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_CombineStone] = new ProtocolHandler(onGeneralDataDeal);

		mProtocols[S2CProtocol.S2C_GetMapList] = new ProtocolHandler(onGetMapList);
		mProtocols[S2CProtocol.S2C_GetDungeonList] = new ProtocolHandler(onGetDungeonList);
		mProtocols[S2CProtocol.S2C_GetBossDungeonList] = new ProtocolHandler(onGetBossDungeonList);
		mProtocols[S2CProtocol.S2C_GetBattleReward] = new ProtocolHandler(onGetBattleReward);
		
		mProtocols[S2CProtocol.S2C_GetBagData] = new ProtocolHandler(onGetBagData);
		
		mProtocols[S2CProtocol.S2C_GetPlayerInfo] = new ProtocolHandler(onGetPlayerInfo);
		mProtocols[S2CProtocol.S2C_GetRoleInfo] = new ProtocolHandler(onGetRoleInfo);
        mProtocols[S2CProtocol.S2C_GetPetInfo] = new ProtocolHandler(onGetPetInfo);
		mProtocols[S2CProtocol.S2C_ViewPlayerInfo] = new ProtocolHandler(onViewPlayerInfo);

		mProtocols[S2CProtocol.S2C_NotifyModified] = new ProtocolHandler(onNotifyModified);
		mProtocols[S2CProtocol.S2C_NotifyAddItem] = new ProtocolHandler(onNotifyAddItem);

		mProtocols[S2CProtocol.S2C_GetGoldMoneyUsage] = new ProtocolHandler(onGetGoldMoneyUsage);

		mProtocols[S2CProtocol.S2C_NotifyScene] = new ProtocolHandler(onNotifyScene);
        mProtocols[S2CProtocol.S2C_EnterScene] = new ProtocolHandler(onEnterScene);
		mProtocols[S2CProtocol.S2C_LeaveScene] = new ProtocolHandler(onLeaveScene);
		mProtocols[S2CProtocol.S2C_FinishScene] = new ProtocolHandler(onFinishScene);
		mProtocols [S2CProtocol.S2C_RaidsScene] = new ProtocolHandler (onRadisScene);

		mProtocols[S2CProtocol.S2C_OpenMenu] = new ProtocolHandler(onOpenMenu);
		mProtocols[S2CProtocol.S2C_ClickMenu] = new ProtocolHandler(onClickMenu);
		
		mProtocols[S2CProtocol.S2C_NotifyTask] = new ProtocolHandler(onNotifyTask);
		mProtocols[S2CProtocol.S2C_GetTaskList] = new ProtocolHandler(onGetTaskList);
		mProtocols[S2CProtocol.S2C_CompleteTask] = new ProtocolHandler(onCompleteTask);

		mProtocols[S2CProtocol.S2C_NotifySkill] = new ProtocolHandler(onNotifySkill);
		mProtocols[S2CProtocol.S2C_UpgradeSkill] = new ProtocolHandler(onUpgradeSkill);
		mProtocols[S2CProtocol.S2C_AdvanceSkill] = new ProtocolHandler(onAdvanceSkill);

		mProtocols[S2CProtocol.S2C_NotifyTalent] = new ProtocolHandler(onNotifyTalentUpdate);
		mProtocols[S2CProtocol.S2C_InvokeTalent] = new ProtocolHandler(onNotifyTalentUpdate);
		mProtocols[S2CProtocol.S2C_EquipTalent] = new ProtocolHandler(onNotifyTalentUpdate);
		mProtocols[S2CProtocol.S2C_UnequipTalent] = new ProtocolHandler(onNotifyTalentUpdate);
		mProtocols[S2CProtocol.S2C_UpgradeTalent] = new ProtocolHandler(onNotifyTalentUpdate);
		
		mProtocols[S2CProtocol.S2C_GetRoleTrainInfo] = new ProtocolHandler(onGetRoleTrainInfo);
		mProtocols[S2CProtocol.S2C_TrainRole] = new ProtocolHandler(onTrainRole);
        mProtocols[S2CProtocol.S2C_GetGuildList] = new ProtocolHandler(OnGetGuildList);
        mProtocols[S2CProtocol.S2C_GetFriendList] = new ProtocolHandler(onGetFriendList);

        mProtocols[S2CProtocol.S2C_NotifyPet] = new ProtocolHandler(onNotifyPetUpdate);
        mProtocols[S2CProtocol.S2C_InvokePet] = new ProtocolHandler(onNotifyPetUpdate);
        mProtocols[S2CProtocol.S2C_EquipPet] = new ProtocolHandler(onNotifyPetUpdate);
        mProtocols[S2CProtocol.S2C_UnequipPet] = new ProtocolHandler(onNotifyPetUpdate);
        mProtocols[S2CProtocol.S2C_UpgradePet] = new ProtocolHandler(onNotifyPetUpdate);
        mProtocols[S2CProtocol.S2C_AdvancePet] = new ProtocolHandler(onNotifyPetUpdate);

		mProtocols[S2CProtocol.S2C_GetArenaInfo] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_RefreshArena] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_AttackArenaPlayer] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_GetArenaTop] = new ProtocolHandler (onGetArenaTop);

		mProtocols[S2CProtocol.S2C_GetFriendList] = new ProtocolHandler(onGetFriendList);
		mProtocols[S2CProtocol.S2C_AddFriend] = new ProtocolHandler(onAddFriend);
        mProtocols[S2CProtocol.S2C_AgreeFriend] = new ProtocolHandler(onAgreeFriend);
		mProtocols[S2CProtocol.S2C_RemoveFriend] = new ProtocolHandler(onRemoveFriend);

		mProtocols[S2CProtocol.S2C_GetGuildConstructInfo] = new ProtocolHandler(OnGetGuildInfo);
		mProtocols[S2CProtocol.S2C_GetGuildList] = new ProtocolHandler(OnGetGuildList);
		mProtocols[S2CProtocol.S2C_CreateGuild] = new ProtocolHandler(OnCreateGuild);
		mProtocols[S2CProtocol.S2C_ApplyJoinGuild] = new ProtocolHandler(OnCreateGuild);
		mProtocols[S2CProtocol.S2C_CancelApplyJoinGuild] = new ProtocolHandler(OnCreateGuild);
		mProtocols[S2CProtocol.S2C_DismissGuildMember] = new ProtocolHandler(OnCreateGuild);
		mProtocols[S2CProtocol.S2C_ChangeGuildMaster] = new ProtocolHandler(OnCreateGuild);
		mProtocols[S2CProtocol.S2C_QuitGuild] = new ProtocolHandler(OnCreateGuild);
		mProtocols[S2CProtocol.S2C_BuildGuild] = new ProtocolHandler(OnGetGuildInfo);
		mProtocols[S2CProtocol.S2C_GetGuildReward] = new ProtocolHandler(OnGetGuildInfo);
		mProtocols[S2CProtocol.S2C_ModifyGuildAnnouncement] = new ProtocolHandler(OnGetGuildInfo);
		mProtocols[S2CProtocol.S2C_DoGuildActivityAction] = new ProtocolHandler(OnGetGuildInfo);
		mProtocols[S2CProtocol.S2C_GetGuildActivityInfo] = new ProtocolHandler(OnGetGuildInfo);
		mProtocols[S2CProtocol.S2C_GuildNotice] = new ProtocolHandler(OnNoticeGuild);
		mProtocols[S2CProtocol.S2C_InviteJoinGuild] = new ProtocolHandler(onGetFriendList);
		mProtocols[S2CProtocol.S2C_GetGuildInfo] = new ProtocolHandler(OnGetGuildInfo);
		mProtocols[S2CProtocol.S2C_GetGuildMemberList] = new ProtocolHandler(OnGetGuildMemberInfo);
		mProtocols[S2CProtocol.S2C_GetGuildApplicationList] = new ProtocolHandler(OnGetApplicationInfo);
		mProtocols[S2CProtocol.S2C_ApprovalGuildApplication] = new ProtocolHandler(OnGetApprovalGuildApplication);
		
		mProtocols[S2CProtocol.S2C_LotteryBase] = new ProtocolHandler (onLotteryBase);
        mProtocols[S2CProtocol.S2C_Lottery] = new ProtocolHandler(onLottery);
		
		mProtocols[S2CProtocol.S2C_GetAchievementList] = new ProtocolHandler(onGetAchievementList);
		mProtocols[S2CProtocol.S2C_GetAchievementReward] = new ProtocolHandler(onGetAchievementReward);

		mProtocols[S2CProtocol.S2C_GetNoticeList] = new ProtocolHandler(onGetNoticeList);

		mProtocols[S2CProtocol.S2C_GetActivityList] = new ProtocolHandler(onGetActivityList);
		mProtocols[S2CProtocol.S2C_GetActivityInfo] = new ProtocolHandler(onGetActivityInfo);
		mProtocols[S2CProtocol.S2C_DoActivityAction] = new ProtocolHandler(onDoActivityAction);
	
		mProtocols[S2CProtocol.S2C_NotifyMail] = new ProtocolHandler(onNewMailTip);
		mProtocols[S2CProtocol.S2C_GetMailList] = new ProtocolHandler (onGetMailList);
		mProtocols[S2CProtocol.S2C_RemoveMail] = new ProtocolHandler (onRemoveMail);
		mProtocols[S2CProtocol.S2C_GetRewardByMail] = new ProtocolHandler (onGetRewardByMail);
		mProtocols[S2CProtocol.S2C_SendMail] = new ProtocolHandler (onSendMail);
		
        mProtocols[S2CProtocol.S2C_ActiveKeyStatus] = new ProtocolHandler(onGeneralDataDeal);

        mProtocols[S2CProtocol.S2C_ShowInitAnimation] = new ProtocolHandler(onGeneralDataDeal);
		mProtocols[S2CProtocol.S2C_UIClosed] = new ProtocolHandler(onUIClosed);
		
		mProtocols[S2CProtocol.S2C_Synchronize] = new ProtocolHandler(onSynchronize);
		mProtocols[S2CProtocol.S2C_EndSyn] = new ProtocolHandler(onSynEnd);
		mProtocols[S2CProtocol.S2C_JoinSearch] = new ProtocolHandler(onJoinSearch);
		mProtocols[S2CProtocol.S2C_CancelSearch] = new ProtocolHandler(onCancelSearch);
		mProtocols[S2CProtocol.S2C_NotifyRoom] = new ProtocolHandler(onNotityRoom);

		mProtocols[S2CProtocol.S2C_NotifyRoom] = new ProtocolHandler(onNotityRoom);
		mProtocols[S2CProtocol.S2C_Kick] = new ProtocolHandler(onKick);

		mProtocols [S2CProtocol.S2C_FindPlayerInfo] = new ProtocolHandler (onGetFriendData);

		mEventToUIName[S2CProtocol.S2C_Create] = "UICreateRole";
        mEventToUIName[S2CProtocol.S2C_Chat] = "UIChat";
		mEventToUIName[S2CProtocol.S2C_GetDungeonList] = "UIBattle";
        mEventToUIName[S2CProtocol.S2C_GetBossDungeonList] = "UIActivityBackground";
		mEventToUIName[S2CProtocol.S2C_GetBattleReward] = "UIBattle";
		mEventToUIName [S2CProtocol.S2C_RaidsScene] = "UIBattle";


		mEventToUIName[S2CProtocol.S2C_NotifyScene] = "UIBattle";
		mEventToUIName[S2CProtocol.S2C_GetRoleTrainInfo] = "UIVein";
		mEventToUIName[S2CProtocol.S2C_TrainRole] = "UIVein";

		mEventToUIName[S2CProtocol.S2C_UpgradeSkill] = "UISkill";
        mEventToUIName[S2CProtocol.S2C_AdvanceSkill] = "UISkill";

		mEventToUIName[S2CProtocol.S2C_UpgradeItem] = "UIEquipStrengthen";
		mEventToUIName[S2CProtocol.S2C_AdvanceItem] = "UIEquipStar";
		mEventToUIName[S2CProtocol.S2C_EquipStone] = "UIEquipSlot";
		mEventToUIName[S2CProtocol.S2C_UnequipStone] = "UIEquipSlot";
		mEventToUIName[S2CProtocol.S2C_CombineStone] = "UIEquipSlot";

		mEventToUIName[S2CProtocol.S2C_GetArenaInfo] = "UIArena";
		mEventToUIName[S2CProtocol.S2C_RefreshArena] = "UIArena";
		mEventToUIName[S2CProtocol.S2C_AttackArenaPlayer] = "UIArena";

        mEventToUIName[S2CProtocol.S2C_LotteryBase] = "UILottery";
		mEventToUIName[S2CProtocol.S2C_Lottery] = "UILottery";

        mEventToUIName[S2CProtocol.S2C_NotifyMail] = ConstUIName.UIMainData;
		mEventToUIName[S2CProtocol.S2C_GetGoldMoneyUsage] = ConstUIName.UIMainData;

		mEventToUIName[S2CProtocol.S2C_GetMailList] = "UIMail";
		mEventToUIName[S2CProtocol.S2C_RemoveMail] = "UIMail";
		mEventToUIName[S2CProtocol.S2C_GetRewardByMail] = "UIMail";
		mEventToUIName[S2CProtocol.S2C_SendMail] = "UIMail";
        mEventToUIName[S2CProtocol.S2C_CombineItem] = "UITips";
        mEventToUIName[S2CProtocol.S2C_AtlasLoot] = "UITips";

		mEventToUIName[S2CProtocol.S2C_JoinSearch] = "UIActivityBackground";
		mEventToUIName[S2CProtocol.S2C_CancelSearch] = "UIActivityBackground";
		mEventToUIName[S2CProtocol.S2C_NotifyRoom] = "UIActivityBackground";
        mEventToUIName[S2CProtocol.S2C_EndSyn] = "UIActivityBackground";

		
		mEventToUIName [S2CProtocol.S2C_GetFriendList] = "UIFriendList";
		mEventToUIName [S2CProtocol.S2C_AddFriend] = "UIFriendList";
		mEventToUIName [S2CProtocol.S2C_RemoveFriend] = "UIFriendList";
		mEventToUIName [S2CProtocol.S2C_AgreeFriend] = "UIFriendList";

		mEventToUIName [S2CProtocol.S2C_GetGuildList] = "UIFactionList";
		mEventToUIName [S2CProtocol.S2C_CreateGuild] = "UIFactionList";
		mEventToUIName [S2CProtocol.S2C_ApplyJoinGuild] = "UIFactionList";
		mEventToUIName [S2CProtocol.S2C_CancelApplyJoinGuild] = "UIFactionList";
		mEventToUIName [S2CProtocol.S2C_GetGuildInfo] = "UIFaction";
        mEventToUIName[S2CProtocol.S2C_GuildNotice] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_GetPetInfo] = "UIPets";
		mEventToUIName [S2CProtocol.S2C_InvokePet] = "UIPets";
		mEventToUIName [S2CProtocol.S2C_EquipPet] = "UIPets";
		mEventToUIName [S2CProtocol.S2C_AdvancePet] = "UIPets";
		mEventToUIName [S2CProtocol.S2C_UpgradePet] = "UIPets";
		mEventToUIName [S2CProtocol.S2C_UnequipPet] = "UIPets";
		mEventToUIName [S2CProtocol.S2C_GetGuildConstructInfo] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_GetGuildMemberList] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_GetGuildApplicationList] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_ApprovalGuildApplication] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_DismissGuildMember] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_ChangeGuildMaster] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_QuitGuild] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_ModifyGuildAnnouncement] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_DoGuildActivityAction] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_GetGuildActivityInfo] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_InviteJoinGuild] = "UIFriendList";
	
		mEventToUIName [S2CProtocol.S2C_BuildGuild] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_GetGuildReward] = "UIFaction";
		mEventToUIName [S2CProtocol.S2C_ViewPlayerInfo] = "UIOtherRole";
        mEventToUIName [S2CProtocol.S2C_DropItem] = "UIMelting";//熔炼
        mEventToUIName[S2CProtocol.S2C_GetNoticeList] = "UIAnnouncement";//公告
        #region Boss活动
        mActEventToUI [Activity.eActivityID.AID_Eighteen] = "UIActivityBackground";
		mActEventToUI [Activity.eActivityID.AID_ProtectedAthena] = "UIActivityBackground";
		mActEventToUI [Activity.eActivityID.AID_MulBoss] = "UIActivityBackground";
        #endregion
        mActEventToUI [Activity.eActivityID.AID_DailyActivity] = "UIDayMissionPage";//日常
        mActEventToUI [Activity.eActivityID.AID_SparShop] = "UIMelting";//神秘商店
        #region //礼包兑换
        mActEventToUI[Activity.eActivityID.AID_TotalLogin] = "UIRechargeGift";//累积登陆
        mActEventToUI[Activity.eActivityID.AID_SevenDayHappy] = "UIRechargeGift";//新玩家七天乐
        mActEventToUI[Activity.eActivityID.AID_Kicthen] = "UIRechargeGift";//用膳
        mActEventToUI [Activity.eActivityID.AID_OnlineGift] = "UIGiftOnLine";//在线礼包

        mActEventToUI[Activity.eActivityID.AID_LevelGift] = "UIRechargeGift";//升级礼包
        mActEventToUI [Activity.eActivityID.AID_NextDayGift] = "UINextDayGift";//次日礼包
        mActEventToUI[Activity.eActivityID.AID_EveryDayLogin] = "UIRechargeGift";//每日登录
        mActEventToUI [Activity.eActivityID.AID_7DayTargetGift] = "UISevenDaysTargetGift";//七日目标
        mActEventToUI [Activity.eActivityID.AID_ContinueLogin] = "UIContinuousLogin";//连续签到
        #endregion
        #region //充值活动
        mActEventToUI [Activity.eActivityID.AID_EndlessRoad] = "UIActivityBackground";//无尽之塔
		mActEventToUI [Activity.eActivityID.AID_Lottery] = "UIRechargeGift";//秘藏豪礼
        mActEventToUI [Activity.eActivityID.AID_FirstRecharge] = "UIRechargeGift";//首充大礼
        mActEventToUI [Activity.eActivityID.AID_VipReward] = "UIRechargeGift";//VIP大礼
		mActEventToUI [Activity.eActivityID.AID_RedPacket] = "UIGrabRedPacket";//红包
		mActEventToUI [Activity.eActivityID.AID_GrowthFund] = "UIRechargeGift";//成长基金
		mActEventToUI [Activity.eActivityID.AID_TodayRecharge] = "UIRechargeGift";//今日累计充值
		mActEventToUI [Activity.eActivityID.AID_TotalRecharge] = "UIRechargeGift";//累积充值
		mActEventToUI [Activity.eActivityID.AID_Promotion] = "UIRechargeGift";//充值促销
        #endregion

        mActEventToUI [Activity.eActivityID.AID_DoubleRecharge] = "UIVip";
        mActEventToUI [Activity.eActivityID.AID_MoneyDungeon] = ConstUIName.UIActivityPage;
        mActEventToUI[Activity.eActivityID.AID_ProtectedAthena] = ConstUIName.UIActivityPage;
		mActEventToUI [Activity.eActivityID.AID_FriendGift] = "UIFriendList";
        mActEventToUI[Activity.eActivityID.AID_WorShip] = "UIWorship";
		mActEventToUI [Activity.eActivityID.AID_Carnival] = "UICrazyPage";
		mActEventToUI [Activity.eActivityID.AID_ArenaScore] = "UIArena";



    }

    public void OnEvent( byte[] byData)
    {
		S2CProtocol protocol = (S2CProtocol)byData[0];
        if (mProtocols.ContainsKey(protocol))
        {
			if(mProtocols[protocol]==null)
			{
				 
			}
			else
            	mProtocols[protocol].Invoke(byData);
        }
    }

	public void RegisterUIEvent(Activity.eActivityID actId,string name){
		mActEventToUI[actId] = name;
	}
    
    private void onPing(byte[] data)
    {
		this.OnPing();
    }

    private void onWaiting(byte[] data)
    {
        int offset = 1;
        int order = BitConverter.ToInt32(data, offset); offset += sizeof(int);
		if(OnWaiting != null)
        	this.OnWaiting(order);
    }

	private void onLogin(byte[] data)
    {
        int offset = ProtocolHeaderSize;
		int tmp = data[offset]; ++offset;
		bool login = false;
		bool forbidden = false;
		
		switch (tmp)
		{
		case 0:
			{
				login = false;
				forbidden = false;
			}
			break;
		case 1:
			{
				login = true;
				forbidden = false;
			}
			break;
		case 2:
			{
				login = false;
				forbidden = true;
			}
			break;
		}

		bool created = (data[offset] == 1); ++offset;
        //int Maxnum = BitConverter.ToInt32(data, offset); offset += sizeof(int);
		int extraDataLength = BitConverter.ToInt32(data, offset); offset += sizeof(int);
		int sessionIDLength = BitConverter.ToInt32(data, offset); offset += sizeof(int);
		string extraData = "";
		string sessionID = "";
		
		if (extraDataLength > 0)
		{
			extraData = Encoding.UTF8.GetString(data, offset, extraDataLength); offset += extraDataLength;
		}
		
		if (sessionIDLength > 0)
		{
			sessionID = Encoding.UTF8.GetString(data, offset, sessionIDLength); offset += sessionIDLength;
		}
		
		if (OnLogin != null)
		{
			this.OnLogin(login, created, forbidden, extraData, sessionID);
		}
    }
	
    private void onReLogin(byte[] data)
	{
		
	}
	private void onDayChanged(byte[] data)
	{
		if (OnDayChanged != null)
			OnDayChanged ();
	}
	private void onCreat(byte[] data)
    {
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	public void onSelectPlayer(byte[] data)
	{
		
	}

	private void onUseRole (byte[] data)
	{
		
	}

	private void onTextProtocol(byte[] data)
	{
		int offset = 1;
		
		string name;
		List<TextProtocolData> dataList = new List<TextProtocolData>();
		
		// 解析协议
		/*int size = BitConverter.ToInt16(data, offset);*/ offset += sizeof(Int16);
		
		// 解析头部
		TextProtocolDataType dataType = (TextProtocolDataType)(data[offset]); offset += 1;
		int length = BitConverter.ToInt32(data, offset); offset += sizeof(int);
		name = Encoding.UTF8.GetString(data, offset, length); offset += length;
		
		// 解析数据
		while (offset < data.Length)
		{
			dataType = (TextProtocolDataType)(data[offset]); offset += 1;
			switch (dataType)
			{
			case TextProtocolDataType.TPDT_Float:
			{
				TextProtocolData tpData = new TextProtocolData();
				tpData.mType = dataType;
				tpData.mValue = BitConverter.ToSingle(data, offset); offset += sizeof(float);
				dataList.Add(tpData);
			} break;
			case TextProtocolDataType.TPDT_Integer:
			{
				TextProtocolData tpData = new TextProtocolData();
				tpData.mType = dataType;
				tpData.mValue = BitConverter.ToInt32(data, offset); offset += sizeof(int);
				dataList.Add(tpData);
			} break;
			case TextProtocolDataType.TPDT_String:
			{
				TextProtocolData tpData = new TextProtocolData();
				tpData.mType = dataType;
				length = BitConverter.ToInt32(data, offset); offset += sizeof(int);
				tpData.mValue = Encoding.UTF8.GetString(data, offset, length); offset += length;
				dataList.Add(tpData);
			} break;
			}
		}

        if (OnTextProtocol != null)
        {
            OnTextProtocol(name, dataList);
        }
       
	}

    

     
 
    private void onIAPCanMakePayments(byte[] data)
    {
        int offset = 1;

        bool canMakePayments = (data[offset] == 1); ++offset;

        if (OnIAPCanMakePayments != null)
        {
            OnIAPCanMakePayments(canMakePayments);
        }
    }

	private void onChat (byte[] data)
	{
		
	}

	private void onMoveTo (byte[] data)
	{
		if(OnMoveTo != null)
			this.OnMoveTo (data);
	}
	
	private void onPlayerMoveIn (byte[] data)
	{
		if(OnPlayerIn != null)
			this.OnPlayerIn (data);
	}
	
	private void onPlayerMoveOut (byte[] data)
	{
		if(OnPlayerOut != null)
			this.OnPlayerOut (data);
	}
	
	private void onPlayerMoveTo (byte[] data)
	{
		if(OnPlayerMoveTo != null)
			this.OnPlayerMoveTo (data);
	}

	private void onMoveItem (byte[] data)
	{
		
	}
	
 
	private void onDropItem (byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
		if (OnMeltingItem != null)
			OnMeltingItem (data);
	}
	
	private void onSellItem(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		bool mResult = (data[offset] == 1);
		OnSellItem(mResult);
	}
	
	private void onGetPlayerInfo(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		this.OnGetPlayerInfo(data, ref offset);
		//this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	
	private void onGetRoleInfo(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		OnGetRoleInfo(data, ref offset);
	}

    private void onGetPetInfo(byte[] data)
    {
        int offset = ProtocolHeaderSize;
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
       // OnGetPetInfo(data, ref offset);
    }

	private void onViewPlayerInfo(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private void onGetDungeonList(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	
	private void onGetBossDungeonList(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	
	private void onGetBattleReward(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}
	
	private void onDungeonNotify(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	
	private void onGetBagData(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		this.OnGetBagData(data, ref offset);
	}
	
	private void onSortBag (byte[] data)
	{
		
	}
	 
	private void onGetTaskList(byte[] data){
		int offset = ProtocolHeaderSize;
		short num = BitConverter.ToInt16 (data,offset);  offset += sizeof(short);
		List<TaskInfoUpdate> list = new List<TaskInfoUpdate>();
		for(int i = 0; i != num; ++i){
			TaskInfoUpdate info = new TaskInfoUpdate();
			info.Init (data,ref offset);
			list.Add (info);
		}
		if(this.OnGetTaskList != null)
			this.OnGetTaskList(list);
	}
	
	private void onCompleteTask(byte[] data){
		int offset = ProtocolHeaderSize;
		int id = BitConverter.ToInt32 (data,offset);offset += sizeof(int);
		byte complete = data[offset];
		if(this.OnCompleteTask != null){
			if(complete == 1){
				this.OnCompleteTask(id,true);
			}
			else{
				this.OnCompleteTask(id,false);
			}
		}
	}

	private void onNotifyTask(byte[] data){
		int offset = ProtocolHeaderSize;
		TaskInfoUpdate info = new TaskInfoUpdate();
		info.Init (data,ref offset);
		if(this.OnNotifyTask != null)
			this.OnNotifyTask(info);
	}

	private void onGeneralDataDeal(byte[] data)
	{
		if(mEventToUIName.ContainsKey((S2CProtocol)data[0]))
			this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	private void onUIClosed(byte[] data)
	{
		if (OnClosedUI != null)
			OnClosedUI (data);
	}


	private void onGetArenaTop(byte[] data)
	{
		if (OnGetAranaTop != null)
			OnGetAranaTop (data);
	}

	private	void onGetNoticeList (byte[] data)
	{
        this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}

	private void onCombineStone (byte[] data)
	{
		
	}

	private void onGetMapList (byte[] data)
	{
		
	}

	private void OnGetGuildList( byte[] data )
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	 
	}

	private void OnCreateGuild( byte[] data )
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
		 
	}

	private void OnNoticeGuild( byte[] data )
	{
	 
        this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private void OnGetOtherRoleInfo( byte[] data )
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}

	private void OnGetGuildInfo( byte[] data )
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}

	private void OnGetGuildMemberInfo( byte[] data )
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}

	private void OnGetApprovalGuildApplication( byte[] data )
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}

	private void OnGetApplicationInfo( byte[] data )
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}

	private	void onGetFriendList (byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}
	
	private	void onAddFriend (byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
		 
	}

    private void onAgreeFriend(byte[] data)
    {
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
    }

	private	void onRemoveFriend (byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);	
	}
	
	private void onGetRoleTrainInfo (byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private void onTrainRole(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private	void onNotifySkill (byte[] data)
	{
		
	}
    private void onUpgradeSkill(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	private void onAdvanceSkill(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
    private void onOpenShop(byte[] data)
    {
        this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
    }
	private void onNotifyModified(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		ModifiedAttributeType mod = (ModifiedAttributeType)BitConverter.ToInt16 (data,offset);  offset += sizeof(short);
		int Value = BitConverter.ToInt32 (data,offset);                                         offset += sizeof(int);
		int absValue = BitConverter.ToInt32 (data,offset);
		if(OnNotifyModified != null){
			OnNotifyModified(mod,Value,absValue);
		}
	}

	private void onGetGoldMoneyUsage(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
		 
	}
	
	private void onNotifyAddItem(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		OnNotifyAddItem(data, ref offset);
	}

	
	
	private void onNotifyScene (byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

    private void onEnterScene(byte[] data)
    {
        int offset = ProtocolHeaderSize;
        byte result = data[offset]; offset++;
		int mapID = System.BitConverter.ToInt32(data, offset); 	offset += sizeof(int);
        int sceneIndex = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
		if (OnPlayerEnterScene != null)
			OnPlayerEnterScene (data);

        this.OnEnterScene(result, mapID,sceneIndex);
    }

	private void onLeaveScene (byte[] data)
	{
		
	}

    private void onFinishScene(byte[] data)
    {
        int offset = ProtocolHeaderSize;
        this.OnFinishScene(data, ref offset);
    }

	public void onRadisScene(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private	void onOpenMenu (byte[] data)
	{
		
	}
	
	private	void onClickMenu (byte[] data)
	{
		
	}

	private void onSynchronize(byte[] data)
	{
		int offset = ProtocolHeaderSize;
		OnSynchronize(data, ref offset);
	}

	private void onJoinSearch(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	
	private void onCancelSearch(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private void onNotityRoom(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	
	private void onNotifyTalentUpdate(byte[] data){
		 
	}

    private void onNotifyPetUpdate(byte[] data)
    {
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
    }


	private void onLottery(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private void onLotteryBase(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private void onGetMailList(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
		
	}
	private void onRemoveMail(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
		
	}
	private void onGetRewardByMail(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
		
	}

	private void onSendMail(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	private void onGetActivityList(byte[] data)
	{
		int offset = 1;
		
		List<Activity> activities = new List<Activity>();
		
		int actNum = BitConverter.ToInt32(data, offset); offset += sizeof(int);
		for (int i = 0; i < actNum; i++)
		{
			activities.Add(new Activity(data, ref offset));
		}
		
		if (OnGetActivityList != null)
		{
			OnGetActivityList(activities);
		}
	}

	private void onGetActivityInfo(byte[] data)
	{		
		int actID = BitConverter.ToInt32 (data, 1);
		if(mActEventToUI.ContainsKey((Activity.eActivityID)actID))
			this.OnEventToUI (mActEventToUI [(Activity.eActivityID)actID], string.Empty, data);
		if (OnGetActivityInfo != null)
			OnGetActivityInfo (data);
	}
	
	private void onDoActivityAction(byte[] data)
	{
		int actID = BitConverter.ToInt32(data, 1);
		if(mActEventToUI.ContainsKey((Activity.eActivityID)actID))
			this.OnEventToUI (mActEventToUI [(Activity.eActivityID)actID], string.Empty, data);
        if (OnDoActivityAction != null)
            OnDoActivityAction(data);
	}

	private void onNewMailTip(byte[] data)
	{
		this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}
	private void onGetAchievementList(byte[] data)
	{
		this.OnEventToUI (mEventToUIName [(S2CProtocol)data [0]], string.Empty, data);
	}
	private void onGetAchievementReward(byte[] data)
	{
		this.OnEventToUI (mEventToUIName [(S2CProtocol)data [0]], string.Empty, data);
	}
	private void onAtlasLoot(byte[] data)
	{
		this.OnEventToUI (mEventToUIName [(S2CProtocol)data [0]], string.Empty, data);
	}

	private void onGetFriendData(byte[] data)
	{
		 
        this.OnEventToUI(mEventToUIName[(S2CProtocol)data[0]], string.Empty, data);
	}

	 
	private void onCombineItem(byte[] data)
	{
		int offset = 1;
		byte mResult = data [offset];		offset++;
		 
	}

	
	private void onBuyItem(byte[] data){
		// the detial of buy Result;
		int offset = 1;
		byte mResult = data [offset];		offset++;
		 
	}

	private void onUseGoldMoney(byte[] data){
		// the detial of buy Result;
		int offset = 1;
		int target = data [offset];                                         offset ++;
		int Result = data [offset];                                         offset ++;
		 
		if (OnUseGoldMoney != null)
			OnUseGoldMoney (data);		
	}

	private void onSynEnd(byte[] data)
	{
		int offset = 1;
		this.OnEndSyn(data, ref offset);
	}

	private void onKick(byte[] data)
	{
		int offset = 1;
		//只有协议头
		this.OnKick();
	}
	
}