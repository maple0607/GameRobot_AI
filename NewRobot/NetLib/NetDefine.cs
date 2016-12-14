using System;
using System.Text;

public enum eNetState
{
    net_none = 1,
    net_working,
    net_dropped,
}

public struct sBufferInfo 
{
    public ushort usBufferSize;
    public ushort usReadSize;
}

public class NetDefine
{
    public static int m_NetBufLen = 0xffff;
    public static int m_UnCompressedLen = 0x4ffff;
}
