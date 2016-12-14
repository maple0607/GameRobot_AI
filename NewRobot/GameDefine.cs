using System.Collections;

public enum CamShakeType
{
	None = 0,
	Always = 1,
	Hit = 2,
};

public enum Job
{
	J_Unknown = 0,					// 未知
	J_Monk,							// 棍僧
	J_Beauty,						// 玲珑
	J_Handsome,						// 逍遥
	J_Mage,							// 法师
	J_Priest,						// 牧师
	
	J_Count,
};

public enum Error
{
	Err_Unknown = -1,			//未知
	Err_Failed = 0,				//失败
	Err_Ok = 1,					//成功
	Err_LevelLimitation,		//等级不够2
	Err_VipLevelLimitation,		//VIP等级不够3
	Err_JobLimitation,			//职业限制4
	Err_Max,					//最高级了5
	Err_NotEnouthSpace,			//空间不足6
	Err_NotEnoughMoney,			//金币不足7
	Err_NotEnoughGoldMoney,		//钻石不足8
	Err_NotEnoughMaterial,		//材料不足9
	Err_NotEnoughCount,			//次数不足10
	Err_NotOwner,				//非所有者11
	Err_NotExist,				//不存在12
	Err_Used=13,					//已使用13
	Err_Repetition,				//重复14
	Err_Invalid,				//无效15
	Err_Double,					//双倍16
	Err_NoFee,					//免费17
	Err_Cannot,					//不能这么做18
	Err_NotOpen,				//未开放19
	Err_ColdDown,				//冷却中20
	Err_HaveThis,				//已存在21
	Err_PermissionLimitation,	//无权限22
	Err_NotEnoughRes,			//资源不够
};

public enum EquipmentPosition
{
	EP_Unknown      = -1,           // 未知
	EP_Weapon       = 0,            // 武器
	EP_Clothes,                     // 衣服
	EP_Pants,						// 裤子
	EP_Shoe,                        // 鞋子
	EP_Cuff,						// 护腕
	EP_Belt,                        // 腰带
	EP_Jewellery,					// 饰品
	EP_Wrap,                        // 外装
	EP_Count,
};

public enum EquipSkill
{
	ES_Unknown		= -1,
	ES_First		= 0,
	ES_Second,
	ES_Third,
	ES_Fourth,
	
	ES_Count,
};

enum EquipPassiveSkill
{
	EPS_Unknown		= -1,
	EPS_First		= 0,
	
	EPS_Count,
};

public enum EquipTalent
{
	ET_Unknown		= -1,
	ET_First		= 0,
	ET_Second,
	ET_Third,
	ET_Fourth,
	ET_Fifth,
	
	ET_Count,
};

public enum ItemMainType
{
	IMT_Unknown		= 0,
	IMT_Weapon		= 1,		//武器
	IMT_Armor		= 2,		//防具 衣服裤子等
	IMT_Box			= 3,		//箱子
	IMT_Consumeable	= 4,		//消耗品,点击可以使用
	IMT_Material	= 5,		//原材料
	IMT_Talent		= 6,		//心法
	IMT_Stone		= 7,		//宝石

	IMT_Count,
}

//元宝使用
public enum GoldMoneyUseType
{
    GMUT_Unknown = 0,
    GMUT_BuyBagNum,                      //购买背包格子
	GMUT_BuyEnergy,						//购买体力
	GMUT_BuyArenaOCount,				//购买排位赛挑战次数
    GMUT_Count,
};

public enum StoneAttribute
{
	SA_ICE = 1,							// 冰
	SA_FLAME,							// 火焰
	SA_BOLT,								// 雷电
	SA_SOUL,								// 心灵
}

public 	enum ItemSubType
{
	IST_Unknown				= -1,
	//武器
	IST_HandWeapon			= 101,   	//单手武器
	IST_HandsWeapon			= 102,		//双手武器
	IST_LongWeapon			= 103,		//长兵器

	//防具
	IST_Clothes				= 201,      //衣服
	IST_Pants				= 202,		//裤子
	IST_Shoe				= 203,      //鞋子
	IST_Cuff				= 204,      //护腕
	IST_Belt				= 205,		//腰带
	IST_Jewellery			= 206,		//饰品
	IST_Wrap				= 207,		//外装

	//箱子
	IST_Gift				= 301,		//宝箱
	IST_Box					= 302,		//礼包	
	//消耗品
	IST_Medecine			= 401,		//大红
	IST_Mana				= 402,		//大蓝备用
	IST_EXP					= 403,		//经验药水
	IST_Effect				= 404,		//特效药水（无敌等备用）
	IST_Task				= 405,		//任务物品 
	IST_RoleTrain1          = 411,  	//生命丹药，用来强化角色生命经脉	
	IST_RoleTrain2          = 412,  	//攻击丹药，用来强化角色攻击经脉
	IST_RoleTrain3          = 413,  	//防御丹药，用来强化角色防御经脉
	IST_RoleTrain4          = 414,  	//暴击丹药，用来强化角色暴击经脉
	IST_RoleTrain5          = 415,  	//闪避丹药，用来强化角色闪避经脉

	IST_HandWeapon_chip		= 420,   	//单手武器装备碎片
	IST_Clothes_chip		= 421,      //衣服装备碎片
	IST_Pants_chip			= 422,		//裤子装备碎片
	IST_Shoe_chip			= 423,      //鞋子装备碎片
	IST_Cuff_chip			= 424,      //护腕装备碎片
	IST_Belt_chip			= 425,		//腰带装备碎片
	IST_Jewellery_chip		= 426,		//饰品装备碎片
	//材料
	IST_Strength			= 501,		//装备强化材料
	IST_Material			= 502,		//装备强化材料
	IST_Recipe				= 503,		//装备部位强化图纸

	IST_SkillBook           = 504,      //技能书
	IST_SkillFragments      = 505,      //技能残卷
	//心法
	IST_Book				= 601,		//少林心法书
	IST_Book2				= 602,		//玲珑法书
	IST_Book3				= 603,		//逍遥心法书
	IST_Book_chip			= 430,		//心法书碎片
	//宝石
	IST_Stone_COMM			= 700,		// 通用
	IST_Stone_Weapon		= 701,      // 武器
	IST_Stone_Clothes		= 702,		// 衣服
	IST_Stone_Pants			= 703,		// 裤子
	IST_Stone_Shoe			= 704,		// 鞋子
	IST_Stone_Cuff			= 705,		// 护腕
	IST_Stone_Belt			= 706,		// 腰带
	IST_Stone_Jewellery		= 707,		// 饰品
	IST_Count,
};

public enum MagicAttribute
{
	MA_Unknown		= -1,				//
	MA_Life			= 0,				//生命
	MA_Attack		= 1,				//攻擊
	MA_Defense		= 2,				//防禦
	MA_Critical		= 3,				//暴擊
	MA_AntiCritical	= 4,				//韌性
	MA_Hit			= 5,				//命中
	MA_Dodge		= 6,				//闪避
	MA_Strike		= 7,				//打击力
	MA_AntiStrike	= 8,				//抗打击力
	MA_ShieldRecover= 9,				//护盾恢复
	MA_ShieldCD		= 10,				//护盾CD
	
	MA_Count,
}
	
public enum eSceneActorType
{
	myplayer = 0,
	player,
	npc,
	pet,
}

public enum eRewardType
{
	REWARD_Type_None				= 0,
	REWARD_Type_Exp					= 1,			//奖励经验
	REWARD_Type_Money				= 2,			//奖励金钱
	REWARD_Type_GoldMoney			= 3,			//奖励金币
	REWARD_Type_Item				= 4,			//奖励道具 不用 道具直接填id
	REWARD_Type_Empty				= 5,			//空
	REWARD_Type_Energy				= 6,			//体力
	REWARD_Type_Honor				= 7,			//荣誉
	REWARD_Type_Lore				= 8,			//阅历
	REWARD_Type_Buff				= 9,			//buff
	REWARD_Type_ArenaOCount			= 10,			//增加排位赛挑战次数
	REWARD_Type_ArenaCCount			= 11,			//增加擂台赛挑战次数
	REWARD_Type_ExpActivity			= 12,			//经验副本活动次数增加次数 玛雅秘境
	REWARD_Type_MoneyActivity		= 13,			//金币活动增加次数 运送物资
	REWARD_Type_StoneActivity		= 14,			//宝石活动增加次数 仙踪鱼乐
	REWARD_Type_LoreActivity		= 15,			//阅历活动增加次数 泰坦神殿
	REWARD_Type_PetSkillActivity	= 16,			//宠物技能书活动增加次数 宠物联盟
	REWARD_Type_FishActivity		= 17,			//未使用 
	REWARD_Type_DailyTaskCount		= 18,			//增加日常任务可接次数
	REWARD_Type_PetIntegral			= 19,			//宠物积分
	
	REWARD_Type_Max
};

public enum enTaskState
{
	ets_none = 0,
	ets_accepted,
	ets_completed,
	ets_over,
	ets_canaccept,
	ets_will_accept,
}

public enum eDungeonType
{
	DT_Common = 0,		//普通副本
	DT_Elite,			//精英副本
	DT_Boss,			//boss副本

	DT_Act_Boss,		//boss活动副本1
	DT_Act_Bronze,		//十八铜人4
	DT_Act_Convoy,		//护送

	DT_PVP_Arena,		//擂台赛
	DT_PVP_Job,			//职业赛2
	DT_PVP_Mul,			//多人pvp
	DT_PVE_Mul,			//多人打怪3

	DT_Max,
};

public enum eMapMainType
{
	MT_World_Map = 1,
	MT_Dng,
	MT_Act_Boss,		//boss活动
	MT_Act_Bronze,		//十八铜人
	MT_Act_Climb,		//爬塔
	MT_PVP,				//pvp
	MT_PVP_Mul,			//多人pvp
}

public enum eDungeonElement
{
	e_none = 0,
	e_fight,
	e_trigger,
	e_move,
	e_ovo,
	e_cvy,
	e_action,
}

public enum eRoleTrainAttr
{
	RTA_Life = 0,
	RTA_Attack,
	RTA_Defence,
	RTA_Critical,
	RTA_Dodge,
};

public enum eMulActType
{
	pvp = 0,
	mul_boss,
}
public enum ActivityPageTable
{
	None = -1,
	BossActivityPage,
	MulActivityPage,
	MulBossPage,
	EighteenPage,
	ProtectedHoard,
	EndlessRoad,
}

public enum ResAttributeType
{
    AT_Unknown = 0,
    AT_Money,	                // 银币
    AT_GoldMoney,               // 金币
    AT_GoldMoneyPurchased,      // 真金
    AT_Vitality = 4,                // 体力
    AT_Experience,              // 经验
    AT_Energy,					// 真气
    AT_Spar,                    //晶石
};

public enum GoldMoneyTarget : int
{
	T_Unknown = -1,             // 无效
	
	T_BuyVitality = 0,			// 购买体力
	T_BuyMoney = 1,					// 购买金币
	T_BuyArenaPoint = 2,            // 购买竞技场点数
	T_BuyFriend = 3,
	T_BuySparFree = 4,
	T_BuySingleBoss = 5,
	T_BuyMultiBoss = 6,
	T_BuyProtectAthena = 7,
	T_BuyEighteen = 8,
	T_BuyNormDungeon = 9,
	T_BuyHardDungeon = 10,
	T_BuyEricDungeon = 11,
	T_BuyBagSlot = 12,               // 购买背包
	
	T_Count = 13,
};

