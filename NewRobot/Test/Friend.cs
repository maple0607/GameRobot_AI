using System;
using System.Collections.Generic;
using System.Text;
using Json;

namespace NewRobot
{
    public class Friend : TestBase
    {
        public enum tStep
        {
            getFriendInfo,
            waitFriendInfo,
            agreeFriend,
            applyFriend,
            getGift,
            sendGift,
        }
        public tStep mCurStep = tStep.agreeFriend;
        public long mTime;
        public int mDungeonID;
        public int mCount = 0;

        public Friend()
        {
        }

        public override void Start()
        {
            base.Start();
            mCurStep = tStep.getFriendInfo;
            mCount = 1;
        }

        public override void Loop()
        {
            base.Loop();

            Robot robot = Robot.GetCurRobot();

            if (robot.MyActorManager == null || robot.MyActorManager.mMyPlayerData == null)
                return;
            UIFriendListData friendData = robot.GetUIData("UIFriend") as UIFriendListData;
            if( friendData == null ) return;
            if (mCurStep == tStep.getFriendInfo)
            {
                ProtocolFuns.GetFriendList();
                mCurStep = tStep.waitFriendInfo;
                mTime = DateTime.Now.Ticks / 10000000;
            }
            else if (mCurStep == tStep.waitFriendInfo)
            {
                long time = DateTime.Now.Ticks / 10000000;
                if (time - mTime > 5)
                {
                    mCurStep = tStep.agreeFriend;
                }
            }
            if (mCurStep == tStep.agreeFriend)
            {
                extraInfo = friendData.mFriendRequestList.Count.ToString();
                foreach( UIFriendListData.FriendRequest requestData in friendData.mFriendRequestList )
                {
                  ProtocolFuns.AgreeFriend( requestData.mName, true );
                }
                mCurStep = tStep.applyFriend;
            }
            else if (mCurStep == tStep.applyFriend)
            {
                extraInfo = friendData.mRecommendList.Count.ToString();
                if( friendData.mFriendInfoList.Count < friendData.mMaxFriendNum )
                {
                    foreach( UIFriendListData.PlayerFriendInfo requestData in friendData.mRecommendList )
                    {
                        ProtocolFuns.AddFriend( requestData.mName );
                    }
                }
                mCurStep = tStep.getGift;
            }
            else if( mCurStep == tStep.getGift )
            {
  			    JsonObject request = new JsonObject ();
			    request ["do"] = new JsonProperty ("get");
			    request ["gettype"] = new JsonProperty("1");
			    ProtocolFuns.DoActivityAction(46, request.ToString());
                mCurStep = tStep.sendGift;
            }
            else if( mCurStep == tStep.sendGift )
            {
                foreach( UIFriendListData.PlayerFriendInfo friendInfo in friendData.mFriendInfoList )
                {
                    if (friendInfo.canSend)
                    {
                        JsonObject request = new JsonObject();
                        request["do"] = new JsonProperty("give");
                        request["name"] = new JsonProperty(friendInfo.mName);
                        ProtocolFuns.DoActivityAction(46, request.ToString());
                    }
                }
                mCount --;
                if( mCount <= 0 )
                {
                    OnFinish();
                }
            }
            testState = mCurStep.ToString();
        }
    }
}
