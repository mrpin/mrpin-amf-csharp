using System;
using System.IO;
using System.Linq;

namespace AMF
{
    public static class IOHelperRead
    {
        public static bool isEof(this MemoryStream stream)
        {
            return stream.Position == stream.Length;
        }

        public static int TryReadByte(this MemoryStream stream)
        {
            if (stream.isEof())
            {
                throw new AmfExceptionIncomplete();
            }

            return stream.ReadByte();
        }

        public static int TryReadWord(this MemoryStream stream)
        {
            if (stream.isEof())
            {
                throw new AmfExceptionIncomplete();
            }

            return stream.ReadByte();
        }

        public static double TryReadDouble(this MemoryStream stream)
        {
            byte bytesToRead = 8;

            if (stream.Position + bytesToRead > stream.Length)
            {
                throw new AmfExceptionIncomplete();
            }

            byte[] buffer = new byte[bytesToRead];
            stream.Read(buffer, 0, buffer.Length);

            return BitConverter.ToDouble(buffer.Reverse().ToArray(), 0);
        }
    }
}

