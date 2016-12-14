using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.ComponentModel;
using System.Runtime.InteropServices;



public class NConnect
{
    private TcpClient m_TcpClient = null;
    private NetworkStream m_NetStrm = null;
    private IPAddress m_ServerIpAdd = null;
    private int m_nServerPort = 0;
    private int m_nBufSize = 0;

    private eNetState m_eCurNetState = eNetState.net_none;

    private byte[] m_byHeadLen = new byte[2];
    private byte[] m_byReceiveBuf = null;
    private byte[] m_byUnCompressBuf = null;

    private sBufferInfo m_BufferInfo = new sBufferInfo();

    public event EventHandler<NetEventHandle> m_NetEvent;

    private Compress mCompress = new Compress();
	private bool mConnectOver = false;

	public string mstrErrorInfo;
	
    public NConnect(string strIpAdd, int nPort)
    {
        if (!IPAddress.TryParse(strIpAdd, out m_ServerIpAdd))
        {
            IPAddress[] addrs = Dns.GetHostAddresses(strIpAdd);
            if (addrs.Length > 0)
                this.m_ServerIpAdd = addrs[0];
        }
        this.m_nServerPort = nPort;
        m_nBufSize = NetDefine.m_NetBufLen;
        m_byReceiveBuf = new byte[m_nBufSize];
        m_byUnCompressBuf = new byte[NetDefine.m_UnCompressedLen];
        if (this.m_ServerIpAdd == null)
        {
            mstrErrorInfo = "NConnect: " + strIpAdd;
        }
    }

    ~NConnect()
    {

    }

	public void IsNetDrop()
	{
		if ( m_TcpClient == null || !mConnectOver)
			return;

		if ( !m_TcpClient.Connected )
		{
			NetDrop("IsNetDrop");
		}
	}

    public eNetState CurNetState
    {
        get{return m_eCurNetState;}
    }

	
	void NetDrop(string except)
	{
		m_eCurNetState = eNetState.net_dropped;
		this.CloseConnect();
		mstrErrorInfo = "net_dropped: " + except;
	}
	
    public void StartConnect()
    {
		while(true)
		{
            try
            {
                if (m_TcpClient == null && m_ServerIpAdd != null)
                    m_TcpClient = new TcpClient(m_ServerIpAdd.AddressFamily);
				mConnectOver = false;
                m_TcpClient.BeginConnect(m_ServerIpAdd, m_nServerPort,
                            new AsyncCallback(ConnectCallback), null);
				
				break;
            }
            catch( Exception e)
	        {
				NetDrop(e.Message);
	        }
		}
    }

    public void CloseConnect()
    {
        try
        {
            if (m_TcpClient != null)
            {
                m_TcpClient.Close();
				
				m_TcpClient = null;
                m_NetStrm = null;
            }
        }
        catch( Exception e)
        {
			NetDrop(e.Message);
        }
    }

    private void ConnectCallback(IAsyncResult result)
    {
        try
        {
			if ( m_TcpClient != null)
			{
	            m_TcpClient.EndConnect(result);
				mConnectOver = true;
				
	            if (m_TcpClient.Connected)
	            {
	                m_NetStrm = m_TcpClient.GetStream();
	                m_eCurNetState = eNetState.net_working;
	            }
	            this.ReceiveHeadLen();
			}
        }
        catch ( Exception e)
        {
			NetDrop(e.Message);
        }
    }

    private void ReceiveHeadLen()
    {
        try
        {
            if (m_NetStrm != null && m_NetStrm.CanRead)
            {
                m_BufferInfo.usBufferSize = sizeof(ushort);
                m_BufferInfo.usReadSize = 0;

                m_NetStrm.BeginRead(m_byHeadLen, 0, m_byHeadLen.Length,
                                   new AsyncCallback(HeadLenCallBack), null);

            }
            else
            {
                NetDrop("NetStrm don't CanRead!");
            }
        }
        catch ( Exception e)
        {
			NetDrop(e.Message);
        }
    }

    private void HeadLenCallBack(IAsyncResult result)
    {
        try
        {
			if (m_NetStrm != null)
			{
	            m_BufferInfo.usReadSize += (ushort)m_NetStrm.EndRead(result);
	            if (m_BufferInfo.usReadSize < m_BufferInfo.usBufferSize)
	            {
	                m_NetStrm.BeginRead(m_byReceiveBuf, m_BufferInfo.usReadSize,
	                                    m_BufferInfo.usBufferSize - m_BufferInfo.usReadSize,
	                                    new AsyncCallback(HeadLenCallBack), null);
	            }
	            else
	            {
	                ushort sHeadLen = (ushort)(BitConverter.ToUInt16(m_byHeadLen, 0) - sizeof(ushort));
	                m_BufferInfo.usBufferSize = sHeadLen;
	                m_BufferInfo.usReadSize = 0;
	                if (sHeadLen > 0 && sHeadLen < m_nBufSize)
	                {
	                    m_NetStrm.BeginRead(m_byReceiveBuf, 0, sHeadLen,
	                                   new AsyncCallback(ReceiveCallBack), null);
	                }
	                else
	                {
	                    NetDrop("HeadLen = " + sHeadLen.ToString());
	                }
	            }
			}
        }
        catch ( Exception e)
        {
			NetDrop(e.Message);
        }
    }

    private void ReceiveCallBack(IAsyncResult result)
    {
        
        try
        {
            m_BufferInfo.usReadSize += (ushort)m_NetStrm.EndRead(result);
            if (m_BufferInfo.usReadSize < m_BufferInfo.usBufferSize)
            {
                m_NetStrm.BeginRead(m_byReceiveBuf, m_BufferInfo.usReadSize,
                                m_BufferInfo.usBufferSize - m_BufferInfo.usReadSize,
                                new AsyncCallback(ReceiveCallBack), null);
            }
            else
            {
                EventHandler<NetEventHandle> temp = m_NetEvent;
                if (temp != null)
                {
                    int nSize = mCompress.decompress(m_byReceiveBuf, m_BufferInfo.usBufferSize,
                                            m_byUnCompressBuf);
                    if ( nSize == -1 )
                    {
                        NetDrop("Decompress error!");
                        return;
                    }

                    m_NetEvent(this, new NetEventHandle(m_byUnCompressBuf, (UInt16)nSize));
                }

                this.ReceiveHeadLen();
            }
        }
        catch ( Exception e)
        {
			NetDrop(e.Message);
        }
    }

    public void SendToServer(byte[] byData)
    {
        try
        {
            ushort length = (ushort)(byData.Length + sizeof(ushort));
            if (length > sizeof(ushort) && length < 0xffff)
            {
                if (m_NetStrm.CanWrite)
                {
                    byte[] _data = new byte[length];
                    Array.Copy(BitConverter.GetBytes(length), 0, _data, 0, sizeof(ushort));
                    Array.Copy(byData, 0, _data, sizeof(ushort), byData.Length);
                    m_NetStrm.BeginWrite(_data, 0, length, null, null);
                }
                else
                {
                    NetDrop("NetStrm don't CanRead!");
                }
            }
            else
            {
                NetDrop("byData.Length error!");
            }
        }
        catch ( Exception e)
        {
			NetDrop(e.Message);
        }
    }
}
