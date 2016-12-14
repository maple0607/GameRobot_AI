using System;
using System.Collections.Generic;
using System.Text;
using Json;

namespace NewRobot
{
    class RechargeActivity : TestBase
    {
        public int mCount = 0;
        public enum Progress
        {
            e_Fir,
            e_Lot,
            e_EvRe,
            e_toRe,
            e_upRe,
            e_vipRe,
            e_Promotion,

            e_7Day,
            e_toLogin,
            e_evLogin,
            e_levelUp,
            e_Kithen,
        }
        public Progress mCurStep = Progress.e_Fir;
        public override void Start()
        {
            //OnFinish += OnDoAction;
            base.Start();
            mCurStep = Progress.e_Fir;
            mCount = 12;
        }
        private void OnDoAction()
        {
            OnGetReward((int)Activity.eActivityID.AID_FirstRecharge, "Get", string.Empty);

        }
        private void OnGetReward(int actId, string key, string id)
        {
            JsonObject obj = new JsonObject();
            obj[key] = new JsonProperty(id);
            ProtocolFuns.DoActivityAction(actId, obj.ToString());
        }

        public void OnGetActInfo(int actId, string actData)
        {
            
            JsonObject obj = new JsonObject(actData);
            switch ((Activity.eActivityID)actId)
            {
                case Activity.eActivityID.AID_Lottery:

                    JsonProperty mGot = obj["Got"];
                    foreach (JsonProperty item in mGot.Items)
                    {
                        int state=int.Parse(item.Items[1].Value);
                        if (state==1)
                            OnGetReward((int)Activity.eActivityID.AID_Lottery, "Get", item.Items[0].Value);
                    }
 
                    break;
                case Activity.eActivityID.AID_FirstRecharge:
                    OnGetReward((int)Activity.eActivityID.AID_FirstRecharge, "Get", string.Empty);
                    break;
                case Activity.eActivityID.AID_GrowthFund:
 
                    break;
                case Activity.eActivityID.AID_VipReward:
                    List<JsonProperty> mVipRewards = obj["VipRewards"].Items;
                    foreach (JsonProperty item in mVipRewards)
                    {
                        string lv=item.Items[0].Value;
                        if (item.Items[2].IsTrue && !item.Items[3].IsTrue)
                            OnGetReward((int)Activity.eActivityID.AID_VipReward, "Level", lv);
                    }

                   
                    break;
                case Activity.eActivityID.AID_TodayRecharge:
                    List<JsonProperty> mTodayRewards = obj["Rewards"].Items;
                    foreach (JsonProperty jp in mTodayRewards)
                    {
                        int needIgnot;
                        bool canGet, geted;
                        int.TryParse(jp[0].ToString(), out needIgnot);
                        bool.TryParse(jp[1].ToString(), out canGet);
                        bool.TryParse(jp[2].ToString(), out geted);
                        if (canGet && !geted)
                        {
                            OnGetReward((int)Activity.eActivityID.AID_TodayRecharge, "Get", needIgnot.ToString());
                        }
                    }
                    break;
                case Activity.eActivityID.AID_TotalRecharge:
                    List<JsonProperty> mTotalRewards = obj["Rewards"].Items;
                    foreach (JsonProperty jp in mTotalRewards)
                    {
                        int needIgnot;
                        bool canGet, geted;
                        int.TryParse(jp[0].ToString(), out needIgnot);
                        bool.TryParse(jp[1].ToString(), out canGet);
                        bool.TryParse(jp[2].ToString(), out geted);
                        if (canGet && !geted)
                        {
                            OnGetReward((int)Activity.eActivityID.AID_TodayRecharge, "Get", needIgnot.ToString());
                        }
                    }
                
                    break;
                case Activity.eActivityID.AID_Promotion:
  	                List<JsonProperty> mRewards= obj["Rewards"].Items;
                    if (mRewards != null && mRewards.Count > 0)
                    {
                        List<JsonProperty> jp = mRewards[0].Items;

                        OnGetReward((int)Activity.eActivityID.AID_Promotion, "Get", jp[0].Value);
                      
                    }
                   
                    break;
                case Activity.eActivityID.AID_SevenDayHappy:
                    List<JsonProperty> mSevenDayRewards = obj["Rewards"].Items;
                    for (int i = 0; i < mSevenDayRewards.Count; i++)
                    {
                        if (mSevenDayRewards[i][2].IsTrue && !mSevenDayRewards[i][3].IsTrue)
                        {
                            OnGetReward((int)Activity.eActivityID.AID_SevenDayHappy, "Day", mSevenDayRewards[i][0].Value);
                            
                        }

                    }
                    break;
                case Activity.eActivityID.AID_TotalLogin:

                    List<JsonProperty> mTotalLoginRewards = obj["Rewards"].Items;
                    for (int i = 0; i < mTotalLoginRewards.Count; i++)
                    {
                        if (mTotalLoginRewards[i][3].IsTrue && !mTotalLoginRewards[i][4].IsTrue)
                        {
                            OnGetReward((int)Activity.eActivityID.AID_SevenDayHappy, "Day", mTotalLoginRewards[i][0].Value);
                        }
                    }

                    break;

                case Activity.eActivityID.AID_EveryDayLogin:
                    if (int.Parse(obj["Status"].Value) == 1)
                    {
                        OnGetReward((int)Activity.eActivityID.AID_EveryDayLogin, "Get", "15");
                    }
                        break;
                case Activity.eActivityID.AID_LevelGift:
                    List<JsonProperty> levelUpRewards = obj["Rewards"].Items;
                    for (int i = 0; i < levelUpRewards.Count; i++)
                    {
                        //Delete geted reward
                        if (levelUpRewards[i][2].IsTrue && !levelUpRewards[i][3].IsTrue)
                        {
                            OnGetReward((int)Activity.eActivityID.AID_LevelGift, "Get", levelUpRewards[i][0].Value); 
                        }

                    }


                    break;
            }
            
        }

        public override void Loop()
        {
            base.Loop();
            if (mCurStep ==Progress.e_Fir)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_FirstRecharge);
               
                mCurStep = Progress.e_Lot;
            }
            else if (mCurStep == Progress.e_Lot)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_Lottery);
                
                mCurStep = Progress.e_EvRe;
            }
            else if (mCurStep == Progress.e_EvRe)
            {

                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_TodayRecharge);
                mCurStep = Progress.e_toRe;
            }
            else if (mCurStep == Progress.e_toRe)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_TotalRecharge);
                mCurStep = Progress.e_upRe;
            }
            else if (mCurStep == Progress.e_upRe)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_GrowthFund);
                mCurStep = Progress.e_vipRe;
            }
            else if (mCurStep == Progress.e_vipRe)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_VipReward);
                mCurStep = Progress.e_Promotion;
            }
            else if (mCurStep == Progress.e_Promotion)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_Promotion);
                mCurStep = Progress.e_7Day;
            }
            else if (mCurStep == Progress.e_7Day)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_SevenDayHappy);
                mCurStep = Progress.e_toLogin;
            }
            else if (mCurStep == Progress.e_toLogin)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_TotalLogin);
                mCurStep = Progress.e_evLogin;
            }
             else if (mCurStep == Progress.e_evLogin)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_EveryDayLogin);
                mCurStep = Progress.e_levelUp;
            }
              else if (mCurStep == Progress.e_levelUp)
            {
                ProtocolFuns.GetKitchenInfo();
                mCurStep = Progress.e_Kithen;
            }  
            else if (mCurStep == Progress.e_Kithen)
            {
                ProtocolFuns.GetActivityInfo((int)Activity.eActivityID.AID_SevenDayHappy);
                mCurStep = Progress.e_Lot;
            }   
           
            mCount--;
            if (mCount <= 0)
                OnFinish();
        }
    }
}
