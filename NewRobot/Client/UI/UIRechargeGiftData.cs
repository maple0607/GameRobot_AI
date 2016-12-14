 
using System.Collections;
using System.Collections.Generic;
using Json;
using NewRobot;
public class UIRechargeGiftData : UIData {
    RechargeActivity mUI = new RechargeActivity();

    public override void AnalyzeToData(string custom, byte[] data)
    {
        base.AnalyzeToData(custom, data);
        int offset = 0;
        S2CProtocol protocol = (S2CProtocol)data[offset]; offset++;
        int actID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        int actDataLength = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        string actData = System.Text.Encoding.UTF8.GetString(data, offset, actDataLength); offset += actDataLength;
        JsonObject info = new JsonObject(actData);
        switch (protocol)
        {
            case S2CProtocol.S2C_GetActivityInfo:
                mUI.OnGetActInfo(actID,actData);
                break;
            case S2CProtocol.S2C_DoActivityAction:    
               OnDoAction(actID,actData);
                break;

        }
    }

    public void OnDoAction(int actId, string actData)
    {
        JsonObject obj = new JsonObject(actData);
        this.OnDoActionBtn(obj, actId);
    }
    private void OnDoActionBtn(JsonObject obj, int actId)
    {
        int error = int.Parse(obj["Result"].Value);
        if ((Error)error == Error.Err_Ok)
        {
            ProtocolFuns.GetActivityInfo(actId);
			 
        }
     

    }
    public override void AnalyzeTextProtocol(List<TextProtocolData> dataList)
    {
        base.AnalyzeTextProtocol(dataList);
        int id = (int)dataList[0].mValue;
		 
        switch ((Activity.eActivityID)id)
        {
            case Activity.eActivityID.AID_LevelGift:
               
                break;
            case Activity.eActivityID.AID_FirstRecharge:
                
                break;
             case Activity.eActivityID.AID_EveryDayLogin:
      
                break;


        }
          
      
    }
    public void AnalyzeKitchenTextProtocol(List<TextProtocolData> dataList)
    {
       
        
    }
	 
}
