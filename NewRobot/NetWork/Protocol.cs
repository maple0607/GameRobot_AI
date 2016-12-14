using System;
using System.Text;
using System.Collections.Generic;
using System.Threading;

enum S2CProtocol
{
    S2C_Ping = 0,
    S2C_Waiting,                        // 排队中
    S2C_Login,                          // 登录协议
    S2C_ReLogin,                        // 断线重连
    S2C_DayChanged,                     // 通知跨天
    S2C_Create,                         // 创建协议返回（第一次登录游戏，创建主角色
    S2C_SelectPlayer,                   // 登录信息（同一帐号下多玩家选择用 类似wow
    S2C_UseRole,						// 切换角色    

    S2C_TextProtocol,                   // 文本协议

    S2C_IAPGetAllProducts,              // IAP 获得所有产品ID
    S2C_IAPPayForProduct,               // IAP 付费成功
    S2C_IAPGetOrderSerial,              // IAP 获得订单号
    S2C_IAPCanMakePayments,             // IAP 是否可以充值

    S2C_Chat,                           // 聊天系统协议
    S2C_MoveTo,                         // 移动到地图中某点协议返回（非副本地图
    S2C_PlayerMoveIn,                   // 玩家进入地图（非副本地图，由服务器同步发送
    S2C_PlayerMoveOut,                  // 玩家离开地图（非副本地图，由服务器同步发送
    S2C_PlayerMoveTo,                   // 玩家移动到地图中某点（非副本地图，由服务器同步发送

    S2C_EquipItem,                      // 装备道具协议返回（只能是背包中的道具
    S2C_UnequipItem,                    // 卸下道具协议返回（只是角色的装备
    S2C_UseItem,                        // 使用道具协议返回（只能是背包中的道具
    S2C_DropItem,                       // 丢弃道具协议返回（指定背包及道具索引
    S2C_SellItem,                       // 贩卖道具协议返回（指定道具索引
    S2C_BuyItem,                        // 购买道具协议返回（指定商店及商店中道具在商店中的索引

    S2C_UpgradeItem,					// 道具升级（指定道具索引
    S2C_AdvanceItem,					// 道具进阶（指定道具索引
    S2C_AtlasLoot,						// 道具掉落查询
    S2C_CombineItem,                    // 合成道具协议返回（指定道具索引
    S2C_GetCombineItemInfo,             // 获得合成道具信息协议返回

    S2C_EquipStone,						// 装备宝石
    S2C_UnequipStone,					// 卸载宝石
    S2C_CombineStone,					// 合成宝石

    S2C_GetMapList,                     // 获得地图列表协议（指定场景ID
    S2C_GetDungeonList,                 // 获得副本列表协议（指定地图ID
    S2C_GetBossDungeonList,             // 获得英雄副本列表
    S2C_GetBattleReward,				// 获取战役星级奖励

    S2C_GetPlayerInfo,                  // 玩家信息
    S2C_GetRoleInfo,                    // 角色信息
    S2C_GetPetInfo,						// 宠物信息
    S2C_GetBagData,                     // 获取背包
    S2C_ViewPlayerInfo,					// 查看玩家信息

    S2C_NotifyModified,                 // 通知客户端数据更新
    S2C_NotifyAddItem,                  // 通知客户端道具添加

    S2C_NotifyScene,					// 场景通知
    S2C_EnterScene,						// 进入场景
    S2C_LeaveScene,						// 离开场景
    S2C_FinishScene,					// 完成场景  
    S2C_DoSceneAction,					// 执行场景动作协议返回
    S2C_RaidsScene,						// 扫荡场景

    S2C_OpenMenu,                       // 通知客户端打开菜单界面
    S2C_ClickMenu,                      // 点击NPC菜单协议返回

    S2C_NotifyTask,                     // 任务通知协议（是否可接和可完成之类的信息
    S2C_GetTaskList,                    // 得到任务列表协议（可以接的和已经接的
    S2C_CompleteTask,					// 完成任务

    S2C_NotifySkill,					// 技能通知 （0 = 激活技能， 1 = 技能可升级， 2 = 技能可进阶
    S2C_UpgradeSkill,					// 技能升级
    S2C_AdvanceSkill,					// 技能进阶

    S2C_NotifyTalent,					// 天赋通知
    S2C_InvokeTalent,					// 激活心法
    S2C_EquipTalent,					// 装备天赋
    S2C_UnequipTalent,					// 卸载天赋
    S2C_UpgradeTalent,					// 升级天赋

    S2C_GetRoleTrainInfo,               // 获得易筋信息
    S2C_TrainRole,                      // 易筋训练

    S2C_NotifyPet,						// 宠物通知
    S2C_InvokePet,						// 激活宠物
    S2C_EquipPet,						// 装备宠物
    S2C_UnequipPet,						// 卸载宠物
    S2C_UpgradePet,						// 升级宠物
    S2C_AdvancePet,						// 进阶宠物

    S2C_HasGift,                        // 是否有礼包没有领
    S2C_GetGift,                        // 领取礼包
    S2C_GetGiftInfo,                    // 获得礼包信息

    S2C_GetArenaInfo,                   // 获得竞技场信息协议返回
    S2C_RefreshArena,					// 刷新对手
    S2C_AttackArenaPlayer,              // 竞技场打架协议返回
    S2C_GetArenaTop,					// 获得竞技场雕像

    S2C_GetFriendList,                  // 获得朋友列表
    S2C_AddFriend,                      // 添加好友
    S2C_AgreeFriend,					// 同意添加
    S2C_RemoveFriend,                   // 移除好友

    S2C_LotteryBase,					// 抽奖基础信息
    S2C_Lottery,						// 抽奖

    S2C_GetGuildList,                   // 获得公会列表
    S2C_GetGuildInfo,                   // 获得公会信息
    S2C_GetGuildMemberList,             // 获得公会成员列表
    S2C_GetGuildConstructInfo,          // 获得公会建设信息
    S2C_GetGuildActivityList,           // 获得公会活动列表
    S2C_GetGuildActivityInfo,           // 获得公会活动信息
    S2C_DoGuildActivityAction,          // 执行公会活动动作
    S2C_CancelApplyJoinGuild,			// 取消申请加入公会
    S2C_GetGuildApplicationList,        // 获得公会申请列表

    S2C_CreateGuild,                    // 创建公会
    S2C_ApplyJoinGuild,                 // 申请加入公会
    S2C_ModifyGuildAnnouncement,        // 修改公会宣言
    S2C_GetGuildReward,                 // 获得公会奖励
    S2C_QuitGuild,                      // 离开公会
    S2C_DismissGuildMember,             // 踢出公会
    S2C_ChangeGuildMaster,              // 转让会长
    S2C_BuildGuild,						// 建设工会
    S2C_ApprovalGuildApplication,       // 审批申请
    S2C_InviteJoinGuild,                // 邀请加入公会
    S2C_ReplyInviteJoinGuild,           // 响应邀请加入公会

    S2C_GuildNotice,                    // 公会通知

    S2C_GetAchievementList,             // 获得成就列表
    S2C_GetAchievementReward,           // 获得成就奖励

    S2C_GetNoticeList,                  // 获取公告列表

    S2C_GetActivityList,                // 获得运营活动列表
    S2C_GetActivityInfo,                // 获得运营活动信息
    S2C_DoActivityAction,               // 执行运营活动动作

    S2C_NotifyMail,						// 新邮件提示
    S2C_GetMailList,                    // 获取邮件列表
    S2C_RemoveMail,                     // 删除邮件
    S2C_GetRewardByMail,                // 获取邮件奖励
    S2C_SendMail,                       // 发送邮件

    S2C_GetGoldMoneyUsage,              // 获得金币使用信息
    S2C_UseGoldMoney,                   // 使用金币

    S2C_UseActiveKey,                   // 使用激活码
    S2C_ActiveKeyStatus,                // 能否使用激活码

    S2C_ShowInitAnimation,              // 显示初始动画

    S2C_UIClosed,                       // 通知客户端哪些界面是关闭了的
    S2C_Synchronize,					// 数据同步
    S2C_CreateRoom,						// 创建房间 
    S2C_GetRoomList,					// 获取房间信息
    S2C_JoinRoom,						// 加入房间
    S2C_LeaveRoom,						// 离开房间
    S2C_StartSyn,						// 开始加载
    S2C_EndSyn,							// 结束
    S2C_NotifyRoom,						// 房间更新
    S2C_JoinSearch,						// 查找
    S2C_CancelSearch,					// 取消查找
    S2C_SwitchHost,						// 切换主机

    S2C_BN_GetGames,                    // 获得联网游戏信息列表
    S2C_BN_GetGame,                     // 获得联网游戏信息
    S2C_BN_GetTeams,                    // 获得联网游戏队伍列表
    S2C_BN_GetTeam,                     // 获得联网游戏队伍信息
    S2C_BN_GetCurTeam,                  // 获得当前队伍
    S2C_BN_CreateTeam,                  // 创建联网队伍
    S2C_BN_JoinTeam,                    // 加入联网队伍
    S2C_BN_LeaveTeam,                   // 离开联网队伍
    S2C_BN_KickTeamMember,              // 踢队员
    S2C_BN_StartQueuing,                // 申请排队
    S2C_BN_StopQueuing,                 // 取消排队
    S2C_BN_GetRankInfo,                 // 获得排名信息
    S2C_BN_NotifyTeam,					// 队伍通知

    S2C_Kick,
    S2C_FindSingleGuild,				// 查找公会
    S2C_FindPlayerInfo,					// 查找玩家信息
    S2C_Count,
};

enum C2SProtocol
{
    C2S_Ping = 0,
    C2S_Waiting,                        // 排队中
    C2S_Login,                          // 登录协议
    C2S_ReLogin,                        // 断线重连
    C2S_DayChanged,                     // 通知跨天
    C2S_Create,                         // 创建协议返回（第一次登录游戏，创建主角色
    C2S_SelectPlayer,                   // 登录信息（同一帐号下多玩家选择用 类似wow
    C2S_UseRole,						// 切换角色    

    C2S_TextProtocol,                   // 文本协议

    C2S_IAPGetAllProducts,              // IAP 获得所有产品ID
    C2S_IAPPayForProduct,               // IAP 付费成功
    C2S_IAPGetOrderSerial,              // IAP 获得订单号
    C2S_IAPCanMakePayments,             // IAP 是否可以充值

    C2S_Chat,                           // 聊天系统协议
    C2S_MoveTo,                         // 移动到地图中某点协议返回（非副本地图
    C2S_PlayerMoveIn,                   // 玩家进入地图（非副本地图，由服务器同步发送
    C2S_PlayerMoveOut,                  // 玩家离开地图（非副本地图，由服务器同步发送
    C2S_PlayerMoveTo,                   // 玩家移动到地图中某点（非副本地图，由服务器同步发送

    C2S_EquipItem,                      // 装备道具协议返回（只能是背包中的道具
    C2S_UnequipItem,                    // 卸下道具协议返回（只是角色的装备
    C2S_UseItem,                        // 使用道具协议返回（只能是背包中的道具
    C2S_DropItem,                       // 丢弃道具协议返回（指定背包及道具索引
    C2S_SellItem,                       // 贩卖道具协议返回（指定道具索引
    C2S_BuyItem,                        // 购买道具协议返回（指定商店及商店中道具在商店中的索引

    C2S_UpgradeItem,					// 道具升级（指定道具索引
    C2S_AdvanceItem,					// 道具进阶（指定道具索引
    C2S_AtlasLoot,						// 道具掉落查询
    C2S_CombineItem,                    // 合成道具协议返回（指定道具索引
    C2S_GetCombineItemInfo,             // 获得合成道具信息协议返回

    C2S_EquipStone,						// 装备宝石
    C2S_UnequipStone,					// 卸载宝石
    C2S_CombineStone,					// 合成宝石

    C2S_GetMapList,                     // 获得地图列表协议（指定场景ID
    C2S_GetDungeonList,                 // 获得副本列表协议（指定地图ID
    C2S_GetBossDungeonList,             // 获得英雄副本列表
    C2S_GetBattleReward,				// 获取战役星级奖励

    C2S_GetPlayerInfo,                  // 玩家信息
    C2S_GetRoleInfo,                    // 角色信息
    C2S_GetPetInfo,						// 宠物信息
    C2S_GetBagData,                     // 获取背包
    C2S_ViewPlayerInfo,					// 查看玩家信息

    C2S_NotifyModified,                 // 通知客户端数据更新
    C2S_NotifyAddItem,                  // 通知客户端道具添加

    C2S_NotifyScene,					// 场景通知
    C2S_EnterScene,						// 进入场景
    C2S_LeaveScene,						// 离开场景
    C2S_FinishScene,					// 完成场景  
    C2S_DoSceneAction,					// 执行场景动作协议返回
    C2S_RaidsScene,						// 扫荡场景

    C2S_OpenMenu,                       // 通知客户端打开菜单界面
    C2S_ClickMenu,                      // 点击NPC菜单协议返回

    C2S_NotifyTask,                     // 任务通知协议（是否可接和可完成之类的信息
    C2S_GetTaskList,                    // 得到任务列表协议（可以接的和已经接的
    C2S_CompleteTask,					// 完成任务

    C2S_NotifySkill,					// 技能通知 （0 = 激活技能， 1 = 技能可升级， 2 = 技能可进阶
    C2S_UpgradeSkill,					// 技能升级
    C2S_AdvanceSkill,					// 技能进阶

    C2S_NotifyTalent,					// 天赋通知
    C2S_InvokeTalent,					// 激活心法
    C2S_EquipTalent,					// 装备天赋
    C2S_UnequipTalent,					// 卸载天赋
    C2S_UpgradeTalent,					// 升级天赋

    C2S_GetRoleTrainInfo,               // 获得易筋信息
    C2S_TrainRole,                      // 易筋训练

    C2S_NotifyPet,						// 宠物通知
    C2S_InvokePet,						// 激活宠物
    C2S_EquipPet,						// 装备宠物
    C2S_UnequipPet,						// 卸载宠物
    C2S_UpgradePet,						// 升级宠物
    C2S_AdvancePet,						// 进阶宠物

    C2S_HasGift,                        // 是否有礼包没有领
    C2S_GetGift,                        // 领取礼包
    C2S_GetGiftInfo,                    // 获得礼包信息

    C2S_GetArenaInfo,                   // 获得竞技场信息协议返回
    C2S_RefreshArena,					// 刷新对手
    C2S_AttackArenaPlayer,              // 竞技场打架协议返回
    C2S_GetArenaTop,					// 获得竞技场雕像

    C2S_GetFriendList,                  // 获得朋友列表
    C2S_AddFriend,                      // 添加好友
    C2S_AgreeFriend,					// 同意添加
    C2S_RemoveFriend,                   // 移除好友

    C2S_LotteryBase,					// 抽奖基础信息
    C2S_Lottery,						// 抽奖

    C2S_GetGuildList,                   // 获得公会列表
    C2S_GetGuildInfo,                   // 获得公会信息
    C2S_GetGuildMemberList,             // 获得公会成员列表
    C2S_GetGuildConstructInfo,          // 获得公会建设信息
    C2S_GetGuildActivityList,           // 获得公会活动列表
    C2S_GetGuildActivityInfo,           // 获得公会活动信息
    C2S_DoGuildActivityAction,          // 执行公会活动动作
    C2S_CancelApplyJoinGuild,			// 取消申请加入公会
    C2S_GetGuildApplicationList,        // 获得公会申请列表

    C2S_CreateGuild,                    // 创建公会
    C2S_ApplyJoinGuild,                 // 申请加入公会
    C2S_ModifyGuildAnnouncement,        // 修改公会宣言
    C2S_GetGuildReward,                 // 获得公会奖励
    C2S_QuitGuild,                      // 离开公会
    C2S_DismissGuildMember,             // 踢出公会
    C2S_ChangeGuildMaster,              // 转让会长
    C2S_BuildGuild,						// 建设工会
    C2S_ApprovalGuildApplication,       // 审批申请
    C2S_InviteJoinGuild,                // 邀请加入公会
    C2S_ReplyInviteJoinGuild,           // 响应邀请加入公会

    C2S_GuildNotice,                    // 公会通知

    C2S_GetAchievementList,             // 获得成就列表
    C2S_GetAchievementReward,           // 获得成就奖励

    C2S_GetNoticeList,                  // 获取公告列表

    C2S_GetActivityList,                // 获得运营活动列表
    C2S_GetActivityInfo,                // 获得运营活动信息
    C2S_DoActivityAction,               // 执行运营活动动作

    C2S_NotifyMail,						// 新邮件提示
    C2S_GetMailList,                    // 获取邮件列表
    C2S_RemoveMail,                     // 删除邮件
    C2S_GetRewardByMail,                // 获取邮件奖励
    C2S_SendMail,                       // 发送邮件

    C2S_GetGoldMoneyUsage,              // 获得金币使用信息
    C2S_UseGoldMoney,                   // 使用金币

    C2S_UseActiveKey,                   // 使用激活码
    C2S_ActiveKeyStatus,                // 能否使用激活码

    C2S_ShowInitAnimation,              // 显示初始动画

    C2S_UIClosed,                       // 通知客户端哪些界面是关闭了的
    C2S_Synchronize,					// 数据同步
    C2S_CreateRoom,						// 创建房间 
    C2S_GetRoomList,					// 获取房间信息
    C2S_JoinRoom,						// 加入房间
    C2S_LeaveRoom,						// 离开房间
    C2S_StartSyn,						// 开始加载
    C2S_EndSyn,							// 结束
    C2S_NotifyRoom,						// 房间更新
    C2S_JoinSearch,						// 查找
    C2S_CancelSearch,					// 取消查找
    C2S_SwitchHost,						// 切换主机

    C2S_BN_GetGames,                    // 获得联网游戏信息列表
    C2S_BN_GetGame,                     // 获得联网游戏信息
    C2S_BN_GetTeams,                    // 获得联网游戏队伍列表
    C2S_BN_GetTeam,                     // 获得联网游戏队伍信息
    C2S_BN_GetCurTeam,                  // 获得当前队伍
    C2S_BN_CreateTeam,                  // 创建联网队伍
    C2S_BN_JoinTeam,                    // 加入联网队伍
    C2S_BN_LeaveTeam,                   // 离开联网队伍
    C2S_BN_KickTeamMember,              // 踢队员
    C2S_BN_StartQueuing,                // 申请排队
    C2S_BN_StopQueuing,                 // 取消排队
    C2S_BN_GetRankInfo,                 // 获得排名信息
    C2S_BN_NotifyTeam,					// 队伍通知

    C2S_Kick,
    C2S_FindSingleGuild,				// 查找公会
    C2S_FindPlayerInfo,					// 查找玩家信息

    C2S_Count,
};

public class Protocol
{
    public const int ROLENAME_LEN = 64;

    private List<byte[]> m_ProtocolList = new List<byte[]>();
    private List<byte[]> m_PorProtocolList = new List<byte[]>();
    private ProtocolEvent mEvent = null;
    public double mLastProtocolTime = -1;

    public Protocol()
    {

    }

    public void InitProtocolEvery()
    {
        mEvent = new ProtocolEvent();
    }

    public ProtocolEvent MyEvent
    {
        get
        {
            return mEvent;
        }
    }

    public void Clear()
    {
        m_PorProtocolList.Clear();
        m_ProtocolList.Clear();
        mLastProtocolTime = -1;
    }

    //解析协议
    public void AnalyzeProtocol()
    {
        lock (m_ProtocolList)
        {
            if (m_ProtocolList.Count > 0)
            {
                foreach (byte[] data in m_ProtocolList)
                {
                    m_PorProtocolList.Add(data);
                }
                m_ProtocolList.Clear();
            }
        }

        int count = 20;
        while (m_PorProtocolList.Count > 0 && count > 0)
        {
            byte[] byData = m_PorProtocolList[0];
            m_PorProtocolList.RemoveAt(0);
            mEvent.OnEvent(byData);
            mLastProtocolTime = System.DateTime.Now.Ticks;
            --count;
        }
    }

    //接收协议
    public void ReceiveProtocol(object src, NetEventHandle mea)
    {
        ushort uDataSize = mea.BufSize;
        if (uDataSize <= 0 || uDataSize > NetDefine.m_NetBufLen)
            return;

        byte[] byData = new byte[(int)uDataSize];
        Array.Copy(mea.Buffer, byData, uDataSize);

        lock (m_ProtocolList)
        {
            //mLastProtocolTime = Time.time;
            m_ProtocolList.Add(byData);
        }
    }
}
