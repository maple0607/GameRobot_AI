using System;
using System.Collections.Generic;
using System.Text;
using Json;
namespace NewRobot
{
    /// <summary>
    /// 测试内容：获取基本信息，刷新碎片目标（预设好的碎片中随机碎片），掠夺目标（随机目标）
    /// 因为不读取配置表，所以不能检测是否可以合成，暂时不测试合成
    /// </summary>
    public class PlunderTest : ActivityTest
    {
        public tStep mCurStep = tStep.apply;
        private bool bData = false;
        private int[] mPieceIds = { 55061011, 55061012, 55061031, 55061032 };
        private int mCurPiece;
        public override void Start()
        {
            base.Start();
            mCurStep = tStep.apply;
            bData = false;
            UIAcitivyData data = Robot.GetCurRobot().GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
            data.mPlunderData.OnReciviData += OnDelegate;
        }
        public override void End()
        {
            base.End();
            UIAcitivyData data = Robot.GetCurRobot().GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
            data.mPlunderData.OnReciviData -= OnDelegate;
        }
        public override void Loop()
        {
            base.Loop();
            if (mCurStep == tStep.apply)
            {
                TextProtocol protocol = new TextProtocol(ConstUIName.UIPlunderProtocol);
                JsonObject obj = new JsonObject();
                obj["Action"] = new JsonProperty("info");
                protocol.push(obj.ToString());
                ProtocolFuns.sendTextProtocol(protocol);
                mCurStep = tStep.data;
                bData = false;
            }
            else if (mCurStep == tStep.data)
            {
                if (bData)
                {
                    Robot robot = Robot.GetCurRobot();
                    UIAcitivyData data = Robot.GetCurRobot().GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
                    if (data.mPlunderData.mCostNum > robot.MyActorManager.mMyPlayerData.mVogir)
                    {
                        finish = true;
                        return;
                    }
                    Random rand = new Random();
                    mCurPiece = mPieceIds[rand.Next(mPieceIds.Length)];

                    TextProtocol protocol = new TextProtocol(ConstUIName.UIPlunderProtocol);
                    JsonObject obj = new JsonObject();
                    obj["Action"] = new JsonProperty("refresh");
                    obj["Param0"] = new JsonProperty(mCurPiece.ToString());
                    protocol.push(obj.ToString());
                    ProtocolFuns.sendTextProtocol(protocol);
                    bData = false;
                    mCurStep = tStep.enter;
                }
            }
            else if (mCurStep == tStep.enter)
            {
                if (bData)
                {
                    Robot robot = Robot.GetCurRobot();
                    UIAcitivyData data = Robot.GetCurRobot().GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;

                    Random rand = new Random();
                    PlunderData.PlunderTarget target = data.mPlunderData.mTargetName[rand.Next(data.mPlunderData.mTargetName.Count)];

                    TextProtocol protocol = new TextProtocol(ConstUIName.UIPlunderProtocol);
                    JsonObject obj = new JsonObject();
                    obj["Action"] = new JsonProperty("plunder");
                    obj["Param0"] = new JsonProperty(mCurPiece.ToString());
                    obj["Param1"] = new JsonProperty(target.isRobt);
                    obj["Param2"] = new JsonProperty(target.mUUID);
                    obj["Param3"] = new JsonProperty(false);
                    protocol.push(obj.ToString());
                    ProtocolFuns.sendTextProtocol(protocol);
                    bData = false;
                    mCurStep = tStep.finish;
                }
            }
            else if (mCurStep == tStep.finish)
            {
                if (bData)
                {
                    Robot robot = Robot.GetCurRobot();
                    UIAcitivyData data = Robot.GetCurRobot().GetUIData(ConstUIName.UIActivityPage) as UIAcitivyData;
                    if (data.mPlunderData.mCostNum > robot.MyActorManager.mMyPlayerData.mVogir)
                    {
                        finish = true;
                        return;
                    }
                    mCurStep = tStep.apply;
                }
            }
        }
        private void OnDelegate()
        {
            bData = true;
        }
    }
}
