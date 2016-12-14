using System;
using System.Collections.Generic;
using System.Text;

public class NetEventHandle : EventArgs
{
    private byte[] m_byReceive = null;
    private ushort m_usSize = 0;

    public NetEventHandle(byte[] byData, ushort usSize)
    {
        m_byReceive = byData;
        m_usSize = usSize;
    }

    public byte[] Buffer
    {
        get { return m_byReceive; }
    }

    public ushort BufSize
    {
        get { return m_usSize; }
    }
}
