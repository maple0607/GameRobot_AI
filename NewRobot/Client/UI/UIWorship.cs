using System;
using System.Collections.Generic;
using NewRobot;
using Json;

/// <summary>
/// Add Description
/// </summary>
public class UIWorshipData : UIData
{
    public int worshipNum = -1;
    #region Interface
    /// <summary>
    /// Read From Proto
    /// </summary>
    /// <param name="custom"></param>
    /// <param name="data"></param>
    public override void AnalyzeToData(string custom, byte[] data)
    {
        base.AnalyzeToData(custom, data);
        int offset = 0;
        S2CProtocol protocol = (S2CProtocol)data[offset]; offset++;
        int actID = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        int actDataLength = System.BitConverter.ToInt32(data, offset); offset += sizeof(int);
        string actData = System.Text.Encoding.UTF8.GetString(data, offset, actDataLength); offset += actDataLength;
        JsonObject info = new JsonObject(actData);
        if (!info["Result"].Value.Equals("1"))
        {
            worshipNum = 0;
            return;
        }
        worshipNum = int.Parse(info["worshipnum"].Value);
        if (0 < worshipNum)
        {
            ProtocolFuns.DoActivityAction(48, string.Empty);
        }
    }
    #endregion
}
