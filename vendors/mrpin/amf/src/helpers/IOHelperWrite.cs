using AMF;
using System;
using System.IO;
using System.Linq;

public static class IOHelperWrite
{
    /*
     * Fields
     */

    /*
     * Methods
     */
    public static void WriteInteger(this MemoryStream stream, int value)
    {
        value &= 0x1FFFFFFF;

        if (value < AmfConstants.MIN_INT_2_BYTE)
        {
            stream.WriteByte((byte)value);
        }
        else if (value < AmfConstants.MIN_INT_3_BYTE)
        {
            stream.WriteByte((byte)((value >> 7) | 0x80));
            stream.WriteByte((byte)(value & 0x7F));
        }
        else if (value < AmfConstants.MIN_INT_4_BYTE)
        {
            stream.WriteByte((byte)((value >> 14) | 0x80));
            stream.WriteByte((byte)(((value >> 7) & 0x7F) | 0x80));
            stream.WriteByte((byte)(value & 0x7F));
        }
        else
        {
            stream.WriteByte((byte)((value >> 22) | 0x80));
            stream.WriteByte((byte)(((value >> 15) & 0x7F) | 0x80));
            stream.WriteByte((byte)(((value >> 8) & 0x7F) | 0x80));
            stream.WriteByte((byte)(value & 0xFF));
        }
    }

    public static void WriteDouble(this MemoryStream stream, double value)
    {
        byte[] buffer = BitConverter.GetBytes(value).Reverse().ToArray();
        stream.Write(buffer, 0, buffer.Length);
    }

}
