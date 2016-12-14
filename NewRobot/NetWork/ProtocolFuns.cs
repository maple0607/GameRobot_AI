using System;
using System.Collections;
using System.Text;
using Json;
using System.Security.Cryptography;
using NewRobot;

public enum BagType
{
	T_Normal = 0,
	T_Destiny = 1,
	T_Stone = 2,
}

public class ProtocolFuns 
{
    public static void SelectPlayer(string rolename)
    {
        byte[] protocol = new byte[65];
        protocol[0] = (byte)C2SProtocol.C2S_SelectPlayer;
        byte[] roleNameData = Encoding.UTF8.GetBytes(rolename);
        Array.Copy(roleNameData, 0, protocol, 1, roleNameData.Length);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void GetDungeonList(int batID)
    {
        byte[] protocol = new byte[5];
        protocol[0] = (byte)C2SProtocol.C2S_GetDungeonList;
        Array.Copy(BitConverter.GetBytes(batID), 0, protocol, 1, sizeof(int));
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void GetTaskList()
    {
        byte[] protocol = new byte[1];
        protocol[0] = (byte)C2SProtocol.C2S_GetTaskList;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }
   

    public static bool sendTextProtocol(TextProtocol data)
    {
        try
        {
            byte[] protocol = new byte[1 + sizeof(Int16) + data.mOffset];
            protocol[0] = (byte)C2SProtocol.C2S_TextProtocol;
            Array.Copy(BitConverter.GetBytes((Int16)data.mOffset), 0, protocol, 1, sizeof(Int16));
            Array.Copy(data.mData, 0, protocol, 1 + sizeof(Int16), data.mOffset);
            Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
            return true;
        }
        catch (Exception err)
        {
           
            return false;
        }
    }
    public static void GetKitchenInfo()
    {
        JsonObject request = new JsonObject();
        request["Action"] = new JsonProperty("info");
        TextProtocol data = new TextProtocol("C2S_EatFeast");
        data.push(request.ToString());
        ProtocolFuns.sendTextProtocol(data);
    }
   
    public static void CompleteTask(int id)
    {
        byte[] protocol = new byte[5];
        protocol[0] = (byte)C2SProtocol.C2S_CompleteTask;
        Array.Copy(BitConverter.GetBytes(id), 0, protocol, 1, sizeof(int));
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void EnterMap(int mapID)
    {
        byte[] protocol = new byte[5];
        protocol[0] = (byte)C2SProtocol.C2S_EnterScene;
        Array.Copy(BitConverter.GetBytes(mapID), 0, protocol, 1, sizeof(int));
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void LeaveMap()
    {
        byte[] protocol = new byte[1];
        protocol[0] = (byte)C2SProtocol.C2S_LeaveScene;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void getMallInfo()
    {
        TextProtocol data = new TextProtocol("C2S_Mall");
        data.push("mall");
        ProtocolFuns.sendTextProtocol(data);
    }

    public static void buyStoreItem(int type, string szUUID, int num)
    {
        TextProtocol data = new TextProtocol("C2S_Mall");
        data.push("buy");
        data.push(type);
        data.push(szUUID);
        data.push(num);
        ProtocolFuns.sendTextProtocol(data);
    }
    
    public static void FinishDungeon(int dngID, byte star, int costTime,int sceneIdx = 0)
    {
        MD5 md5 = new MD5CryptoServiceProvider();
        string source = String.Format("{0:D4}{1:D4}{2:D4}{3:D4}", dngID, sceneIdx, costTime, star);
        byte[] bytes_md5_in = Encoding.Default.GetBytes(source);
        byte[] bytes_md5_out = md5.ComputeHash(bytes_md5_in);
        string sign = BitConverter.ToString(bytes_md5_out).Replace("-", "").ToLower();

        byte[] data = System.Text.Encoding.UTF8.GetBytes(sign);
        int offset = 0;
        byte[] protocol = new byte[14 + data.Length];
        protocol[0] = (byte)C2SProtocol.C2S_FinishScene; offset++;
        Array.Copy(BitConverter.GetBytes(dngID), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(costTime), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(star), 0, protocol, offset, sizeof(byte)); offset += sizeof(byte);
        Array.Copy(BitConverter.GetBytes(data.Length), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(data, 0, protocol, offset, data.Length); offset += data.Length;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }
    public static void GetActivityList()
    {
        byte[] protocol = new byte[1];
        protocol[0] = (byte)C2SProtocol.C2S_GetActivityList;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }
    public static bool GetActivityInfo(int actID)
    {
        try
        {
            byte[] protocol = new byte[5];
            int offset = 0;
            protocol[offset] = (byte)C2SProtocol.C2S_GetActivityInfo; ++offset;
            Array.Copy(BitConverter.GetBytes(actID), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
            Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
            return true;
        }
        catch (Exception err)
        {
            
            return false;
        }
    }

    public static void BugItem(int type, int id, int num)
    {
        int offset = 1;
        byte[] protocol = new byte[13];
        protocol[0] = (byte)C2SProtocol.C2S_BuyItem;
        Array.Copy(BitConverter.GetBytes(type), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(id), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(num), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void moveTo(int x, int y, int targetX, int targetY)
    {
        int offset = 0;
        byte[] protocol = new byte[1 + (sizeof(int) * 4)];
        protocol[offset] = (byte)C2SProtocol.C2S_MoveTo; offset++;
        Array.Copy(BitConverter.GetBytes(x), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(y), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(targetX), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(targetY), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void CombineItem(int mIndex, bool isAuto = false)
    {
        byte[] protocol = new byte[6];
        protocol[0] = (byte)C2SProtocol.C2S_CombineItem;
        Array.Copy(BitConverter.GetBytes(mIndex), 0, protocol, 1, sizeof(int));
        Array.Copy(BitConverter.GetBytes(isAuto), 0, protocol, 5, sizeof(byte));
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }


    public static void ApprovalGuildApplication(string name, bool agree)
    {
        int offset = 0;
        byte[] data = System.Text.Encoding.UTF8.GetBytes(name);
        byte[] protocol = new byte[1 + 64 + 1];
        protocol[0] = (byte)C2SProtocol.C2S_ApprovalGuildApplication; offset++;
        Array.Copy(data, 0, protocol, offset, data.Length); offset += 64;
        protocol[offset] = (byte)(agree ? 1 : 0); offset++;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void AgreeFriend(string name, bool agree)
    {
        byte[] nameData = Encoding.UTF8.GetBytes(name);
        byte[] protocol = new byte[66];
        protocol[0] = (byte)C2SProtocol.C2S_AgreeFriend;
        protocol[1] = (byte)(agree ? 1 : 0);
        System.Array.Copy(nameData, 0, protocol, 2, nameData.Length);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void AddFriend(string name)
    {
        byte[] nameData = Encoding.UTF8.GetBytes(name);
        byte[] protocol = new byte[65];
        protocol[0] = (byte)C2SProtocol.C2S_AddFriend;
        System.Array.Copy(nameData, 0, protocol, 1, nameData.Length);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void GetFriendList()
    {
        byte[] protocol = new byte[1];
        protocol[0] = (byte)C2SProtocol.C2S_GetFriendList;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static bool DoActivityAction(int actID, string actData)
    {
        try
        {
            byte[] _data = Encoding.UTF8.GetBytes(actData);
            byte[] protocol = new byte[9 + _data.Length];
            int offset = 0;
            protocol[offset] = (byte)C2SProtocol.C2S_DoActivityAction; ++offset;
            Array.Copy(BitConverter.GetBytes(actID), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
            Array.Copy(BitConverter.GetBytes((int)_data.Length), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
            Array.Copy(_data, 0, protocol, offset, _data.Length); offset += _data.Length;
            Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
            return true;
        }
        catch (Exception err)
        {
            return false;
        }
    }

    public static void CreateGuild(string guildName, bool useIngot)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(guildName);
        int offset = 0;
        byte[] protocol = new byte[2 + 64];
        protocol[offset] = (byte)C2SProtocol.C2S_CreateGuild; offset++;
        //Array.Copy(BitConverter.GetBytes((short)data.Length), 0, protocol, offset, 2); offset += sizeof(short);
        Array.Copy(data, 0, protocol, offset, data.Length); offset += 64;
        protocol[offset] = (byte)(useIngot ? 1 : 0); offset++;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void GetGuildList(int page, int itemPerPage)
    {
        byte[] protocol = new byte[5];
        protocol[0] = (byte)C2SProtocol.C2S_GetGuildList;
        Array.Copy(BitConverter.GetBytes(page * itemPerPage), 0, protocol, 1, sizeof(int));
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void GetApplicationList(int page, int itemPerPage)
    {
        byte[] protocol = new byte[5];
        protocol[0] = (byte)C2SProtocol.C2S_GetGuildApplicationList;
        Array.Copy(BitConverter.GetBytes(page * itemPerPage), 0, protocol, 1, sizeof(int));
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void DoGuildActivityAction(int id, JsonObject obj)
    {
        int offset = 0; 
        byte[] data = System.Text.Encoding.UTF8.GetBytes(obj.ToString());
        byte[] protocol = new byte[1 + 8 + data.Length];
        protocol[0] = (byte)C2SProtocol.C2S_DoGuildActivityAction; offset++;
        Array.Copy(BitConverter.GetBytes(id), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(BitConverter.GetBytes(data.Length), 0, protocol, offset, sizeof(int)); offset += sizeof(int);
        Array.Copy(data, 0, protocol, offset, data.Length);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void GetMyGuildInfo()
    {
        byte[] protocol = new byte[1];
        protocol[0] = (byte)C2SProtocol.C2S_GetGuildInfo;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void GetGuildMember(int page, int memberPerPage)
    {
        byte[] protocol = new byte[5];
        protocol[0] = (byte)C2SProtocol.C2S_GetGuildMemberList;
        Array.Copy(BitConverter.GetBytes(page * memberPerPage), 0, protocol, 1, sizeof(int));
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void JionGuild(string guildName)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(guildName);
        byte[] protocol = new byte[65];
        protocol[0] = (byte)C2SProtocol.C2S_ApplyJoinGuild;
        Array.Copy(data, 0, protocol, 1, data.Length);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void CancelJionGuild(string guildName)
    {
        byte[] data = System.Text.Encoding.UTF8.GetBytes(guildName);
        byte[] protocol = new byte[65];
        protocol[0] = (byte)C2SProtocol.C2S_CancelApplyJoinGuild;
        Array.Copy(data, 0, protocol, 1, data.Length);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }

    public static void QuitGuild()
    {
        byte[] protocol = new byte[1];
        protocol[0] = (byte)C2SProtocol.C2S_QuitGuild;
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }
    public static void GetFriendData(byte id, string sName)
    {
        byte[] nameBytes = Encoding.UTF8.GetBytes(sName);

        byte[] protocol = new byte[1 + 1 + 64];
        int offset = 0;
        protocol[offset] = (byte)C2SProtocol.C2S_FindPlayerInfo; offset++;
        protocol[offset] = id; offset++;
        Array.Copy(nameBytes, 0, protocol, offset, nameBytes.Length);
        Robot.GetCurRobot().MyNetWorkMgr.SendData(protocol);
    }
}
