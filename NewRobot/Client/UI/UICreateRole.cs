using System.Collections;
using System.Text;
using System.Collections.Generic;
using NewRobot;

public class UICreateRoleData : UIData
{
	public string mName;
    public override void AnalyzeToData(string custom, byte[] data)
	{
		int offset = 1;
        byte type = data[offset]; ++offset;
		if (type == 1 )
		{
            ProtocolFuns.SelectPlayer(mName);
		}
	}
}
