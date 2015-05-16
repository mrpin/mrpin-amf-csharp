// AMF3协议格式的部分处理
//
// see: <<Action Message Format - AMF 3>>
//      https://code.google.com/p/amf3cplusplus/
//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AmfData
{
    class CAmf3Helper
    {


        protected class CObjTraits
        {
            public bool isDynamic;
            public string name;
            public string[] keys;
        }

        protected class CRefTable
        {
            public List<string> str = new List<string>();
            public List<CNameObjDict> obj = new List<CNameObjDict>();
            public List<CObjTraits> ot = new List<CObjTraits>();
        }

        const int MaxU29 = 268435455;

        // 无需构造实例
        private CAmf3Helper()
        {

        }

        public static object Read(Stream stm)
        {
            return ReadAmf(stm, new CRefTable());
        }

        protected static object ReadAmf(Stream stm, CRefTable rt)
        {
            int type = stm.ReadByte();

//            switch ((DataType)type)
//            {
//                case DataType.Integer:
//                    return ReadInt(stm);
//                case DataType.Double:
//                    return CDataHelper.BE_ReadDouble(stm);
//                case DataType.String:
//                    return ReadString(stm, rt);
//                case DataType.XmlDoc:
//                    break;
//                case DataType.Date:
//                    break;
//                case DataType.Array:
//                    return ReadArray(stm, rt);
//                case DataType.Object:
//                    return ReadHashObject(stm, rt);
//                case DataType.Xml:
//                    break;
//                case DataType.ByteArray:
//                    break;
//                default:
//                    break;
//            }

            Trace.Assert(false, "暂未处理的数据类型");
            return null;
        }


//        protected static string ReadString(Stream stm, CRefTable rt)
//        {
//            uint head = ReadInt(stm);
//            int len = (int)(head >> 1);
//            if (len <= 0)
//                return "";
//
//            if (IsRefrence(head))
//                return rt.str[len];
//
//            string str = CDataHelper.ReadUtfStr(stm, len);
//            rt.str.Add(str);
//
//            return str;
//        }

//        protected static CMixArray ReadArray(Stream stm, CRefTable rt)
//        {
//            uint head = ReadInt(stm);
//
//            Trace.Assert((head & 0x1) == 0x1);
//
//            int count = (int)(head >> 1);
//            CMixArray ary = new CMixArray(count);
//            for (string key = ReadString(stm, rt); key != ""; key = ReadString(stm, rt))
//                ary[key] = ReadAmf(stm, rt);
//
//            //读取子元素
//            for (int i = 0; i < count; i++)
//                ary[i] = ReadAmf(stm, rt);
//
//            return ary;
//        }

//        protected static CNameObjDict ReadHashObject(Stream stm, CRefTable rt)
//        {
//            uint head = ReadInt(stm);
//            CObjTraits ot = null;
//
//            if (IsRefrence(head))
//                return rt.obj[(int)(head >> 1)];
//
//            if (IsRefrence(head >> 1))
//            {
//                ot = rt.ot[(int)(head >> 2)];
//            }
//            else
//            {
//                ot = new CObjTraits();
//                Trace.Assert(((head >> 2) & 0x1) == 0, "暂不支持");
//                ot.isDynamic = ((head >> 3) & 0x1) != 0;
//                int count = (int)(head >> 4);
//                ot.name = ReadString(stm, rt);
//                ot.keys = new string[count];
//                for (int i = 0; i < count; i++)
//                    ot.keys[i] = ReadString(stm, rt);
//                rt.ot.Add(ot);
//            }
//
//            CNameObjDict obj = new CNameObjDict(ot.name);
//            for (int i = 0; i < ot.keys.Length; i++)
//                obj[ot.keys[i]] = ReadAmf(stm, rt);
//
//            //读取动态属性
//            if (ot.isDynamic)
//            {
//                while (true)
//                {
//                    string key = ReadString(stm, rt);
//                    if (key == "")
//                        break;
//
//                    obj[key] = ReadAmf(stm, rt);
//                }
//            }
//
//            rt.obj.Add(obj);
//
//            return obj;
//        }
//
//        protected static bool IsRefrence(uint header)
//        {
//            return (header & 0x1) == 0;
//        }
//
//        private static void Write(Stream stm, object obj)
//        {
//            //todo: review
////            WriteAmf(stm, obj);
//        }
//
//
//
//
//
//
//        public static object GetObject(byte[] buf)
//        {
//            return Read(new MemoryStream(buf));
//        }


    }
}
