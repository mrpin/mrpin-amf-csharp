namespace AMF
{
    public class AmfConstants
    {
        public const byte AMF3_MARKER_UNDEFINED = 0x00;
        public const byte AMF3_MARKER_NULL = 0x01;

        public const byte AMF3_MARKER_FALSE = 0x02;
        public const byte AMF3_MARKER_TRUE = 0x03;

        public const byte AMF3_MARKER_INTEGER = 0x04;
        public const byte AMF3_MARKER_DOUBLE = 0x05;
        public const byte AMF3_MARKER_STRING = 0x06;
        public const byte AMF3_MARKER_XML_DOC = 0x07; //not supported
        public const byte AMF3_MARKER_DATE = 0x08;
        public const byte AMF3_MARKER_ARRAY = 0x09;
        public const byte AMF3_MARKER_OBJECT = 0x0A;
        public const byte AMF3_MARKER_XML = 0x0B;
        public const byte AMF3_MARKER_BYTE_ARRAY = 0x0C;

        public const byte AMF3_MARKER_VECTOR_INT = 0x0D;//not supported
        public const byte AMF3_MARKER_VECTOR_UINT = 0x0E;//not supported
        public const byte AMF3_MARKER_VECTOR_DOUBLE = 0x0F;//not supported
        public const byte AMF3_MARKER_VECTOR_OBJECT = 0x10;//not supported

        public const byte AMF3_MARKER_DICTIONARY = 0x11;

        public const byte AMF3_EMPTY_STRING = 0x01;

        public const byte AMF3_CLOSE_DYNAMIC_ARRAY = 0x01;
        public const byte AMF3_CLOSE_DYNAMIC_OBJECT = 0x01;

        public const int INTEGER_MAX = 268435455;
        public const int INTEGER_MIN = - 268435456;

        public const uint MIN_INT_2_BYTE = 0x80;
        public const uint MIN_INT_3_BYTE = 0x4000;
        public const uint MIN_INT_4_BYTE = 0x200000;
    }
}