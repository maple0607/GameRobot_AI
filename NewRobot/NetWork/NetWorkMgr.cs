using System;
using System.Collections;

public class NetWorkMgr 
{
	public delegate void NetDroppedHandler();
	private event NetDroppedHandler OnNetDropped;
	
	private NConnect m_NetWork = null;
	private Protocol m_protocol = null;

    

	private string m_strIPAddress;
	public string IPAddress
	{
		set
		{
			m_strIPAddress = value;
		}
	}
	
	private int m_nPort;
	public int Port
	{
		set
		{
			m_nPort = value;
		}
	}
	
	public NetWorkMgr(NetDroppedHandler handler)
	{
		OnNetDropped = new NetDroppedHandler(handler);

        m_protocol =  new Protocol();
        m_protocol.InitProtocolEvery();
	}


	public void StartNetWork()
    {
		this.StopNetWork();
		
        if (m_NetWork == null)
        {
            m_NetWork = new NConnect(m_strIPAddress, m_nPort);
            m_NetWork.StartConnect();

            m_protocol.mLastProtocolTime = DateTime.Now.Ticks;
            
            m_NetWork.m_NetEvent += new EventHandler<NetEventHandle>(m_protocol.ReceiveProtocol);
        }
    }
	
	public void StopNetWork()
    {
        if ( m_NetWork != null)
        {
            m_NetWork.CloseConnect();
            m_NetWork = null;
        }
		
		if ( m_protocol != null)
        {
            m_protocol.Clear();
        }
    }
	
	public void SendData(byte[] byData)
    {
        if ( m_NetWork != null && byData != null)
        {
            m_NetWork.SendToServer(byData);
        }
    }
	
	public eNetState GetCurNetState ()
	{
		if ( m_NetWork != null)
			return m_NetWork.CurNetState;
		else
			return eNetState.net_none;
	}
	
	public ProtocolEvent GetMyEvent ()
	{
		return m_protocol.MyEvent;
	}
	
	public void OnPing()
	{
        try
        {
            byte[] protocol = new byte[1];
            protocol[0] = (byte)C2SProtocol.C2S_Ping;
            this.SendData(protocol);
        }
        catch
        {
        }
	}

	public void FixedUpdate()
	{
		if ( m_NetWork != null)
		{
			if ( m_NetWork.CurNetState == eNetState.net_dropped)
			{
				this.StopNetWork();
				this.OnNetDropped();
			}
            else if (m_protocol != null && m_NetWork.CurNetState == eNetState.net_working &&
                m_protocol.mLastProtocolTime > 0 && DateTime.Now.Ticks - m_protocol.mLastProtocolTime > 300000000)
            {
                this.StopNetWork();
                this.OnNetDropped();
            }
			else
			{
				m_NetWork.IsNetDrop();
				if ( m_protocol != null )
					m_protocol.AnalyzeProtocol();
			}
		}
	}
}
