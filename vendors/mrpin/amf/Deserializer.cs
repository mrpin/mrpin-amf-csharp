using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AMF
{
    public class Deserializer
    {

        /*
         * Fields
         */
        private ClassMapper _classMapper;

        private MemoryStream _source;

        private List<string> _cacheStrings;
        private List<object> _cacheObjects;
        private List<ObjectTraits> _cacheTraits;

        /*
         * Methods
         */

        public Deserializer(ClassMapper classMapper)
        {
            _classMapper = classMapper;
        }

        public AmfResponse deserialize(byte[] data)
        {
            if (data == null)
            {
                throw new AmfException("no source to deserialize");
            }

            AmfResponse result = new AmfResponse();

            _source = new MemoryStream(data);

            while (!_source.isEof())
            {
                _cacheStrings = new List<string>();
                _cacheObjects = new List<object>();
                _cacheTraits = new List<ObjectTraits>();

                int sourcePosition = (int)_source.Position;

                try
                {
                    result.objects.Add(amf3Deserialize());
                }
                catch (AmfException e)
                {
                    int bytesToRead = (int)(_source.Length - sourcePosition);

                    byte[] incompleteObject = new byte[bytesToRead];

                    int bytesReaded = _source.Read(incompleteObject, sourcePosition, bytesToRead);

                    result.incompleteObject = incompleteObject;

                    //todo: review and remove if not needed
                    //                    string data = Encoding.UTF8.GetString(receivedData, 0, bytesReaded);

                    //                    receivedData = Encoding.UTF8.GetBytes(data);

                    break;
                }

            }

            return result;
        }

        private object amf3Deserialize()
        {
            object result = null;

            //todo: throw exception
            int type = _source.TryReadByte();

            switch (type)
            {
                case AmfConstants.AMF3_MARKER_NULL:
                {
                    result = null;
                    break;
                }
                case AmfConstants.AMF3_MARKER_UNDEFINED:
                {
                    result = null;
                    break;
                }
                case AmfConstants.AMF3_MARKER_FALSE:
                {
                    result = false;
                    break;
                }
                case AmfConstants.AMF3_MARKER_TRUE:
                {
                    result = true;
                    break;
                }
                case AmfConstants.AMF3_MARKER_INTEGER:
                {
                    result = amf3ReadInteger();
                    break;
                }
                case AmfConstants.AMF3_MARKER_DOUBLE:
                {
                    result = amf3ReadDouble();
                    break;
                }
                case AmfConstants.AMF3_MARKER_STRING:
                {
                    result = amf3ReadString();
                    break;
                }
                case AmfConstants.AMF3_MARKER_DATE:
                {
                    result = amf3ReadDate();
                    break;
                }
                case AmfConstants.AMF3_MARKER_ARRAY:
                {
                    result = amf3ReadArray();
                    break;
                }
                case AmfConstants.AMF3_MARKER_OBJECT:
                {
                    result = amf3ReadObject();
                    break;
                }
                case AmfConstants.AMF3_MARKER_BYTE_ARRAY:
                {
                    result = amf3ReadByteArray();
                    break;
                }
                case  AmfConstants.AMF3_MARKER_DICTIONARY:
                {
                    result = amf3ReadDictionary();
                    break;
                }
            }

            return result;
        }


        private bool getAsReferenceObject<T>(int type, ref T value)
        {
            bool result = false;

            if ((type & 0x01) == 0)
            {
                int reference = type >> 1;
                value = (T)_cacheObjects[reference];

                result = true;
            }

            return result;
        }

        private bool getAsReferenceString(int type, ref string value)
        {
            bool result = false;

            if ((type & 0x01) == 0)
            {
                int reference = type >> 1;
                value = _cacheStrings[reference];

                result = true;
            }

            return result;
        }

        private int amf3ReadInteger()
        {
            int result = 0 ;

            int n = 0;
            int b = _source.TryReadWord();

            while (((b & 0x80) != 0) && (n < 3))
            {
                result = result << 7;
                result = result | (b & 0x7F);

                b = _source.TryReadWord();
                n++;
            }

            if (n < 3)
            {
                result = result << 7;
                result = result | b;
            }
            else
            {
                //use all 8 bits from 4th byte
                result = result << 8;
                result = result | b;

                //Check if the integer should be negative
                if (result > AmfConstants.INTEGER_MAX)
                {
                    result -= (1 << 29);
                }
            }

            return result;
        }

        private double amf3ReadDouble()
        {
            return _source.TryReadDouble();
        }

        private string amf3ReadString()
        {
            string result = null;

            int type = amf3ReadInteger();

            if (getAsReferenceString(type, ref result))
            {
                return result;
            }

            int lenght = type >> 1;

            result = "";

            if (lenght > (_source.Length - _source.Position))
            {
                throw new AmfExceptionIncomplete();
            }

            byte[] resultData = new byte[lenght];

            _source.Read(resultData, 0, resultData.Length);

            result = Encoding.UTF8.GetString(resultData);

            _cacheStrings.Add(result);

            return result;
        }

        private DateTime amf3ReadDate()
        {
            DateTime result = default(DateTime);

            int type = amf3ReadInteger();

            if (getAsReferenceObject(type, ref result))
            {
                return result;
            }

            double seconds = _source.TryReadDouble() / 1000;

            result = new DateTime();

            result.AddSeconds(seconds);

            _cacheObjects.Add(result);

            return result;
        }

        private ICollection amf3ReadArray()
        {
            ICollection result = null;

            int type = amf3ReadInteger();

            if (getAsReferenceObject(type, ref result))
            {
                return result;
            }

            int length = type >> 1;
            string propertyName = amf3ReadString();

            bool isAssociativeArray = !string.IsNullOrEmpty(propertyName);

            if (isAssociativeArray)
            {
                Dictionary<object, object> resultDictionary = new Dictionary<object, object>();

                while (!string.IsNullOrEmpty(propertyName))
                {
                    object value = amf3Deserialize();
                    resultDictionary[propertyName] = value;
                    propertyName = amf3ReadString();
                }

                object[] objects = getObjects(length);

                for (int i = 0; i < objects.Length - 1; i++)
                {
                    resultDictionary.Add(i.ToString(), objects[i]);
                }

                result = resultDictionary;
            }
            else
            {
                result = getObjects(length);
            }

            _cacheObjects.Add(result);

            return result;
        }

        private object[] getObjects(int count)
        {
            object[] result = new object[count];

            for (int i = 0; i < count - 1; i++)
            {
                result[i] = amf3Deserialize();
            }

            return result;
        }

        private object amf3ReadObject()
        {
            object result = null;

            int type = amf3ReadInteger();

            if (getAsReferenceObject(type, ref result))
            {
                return result;
            }

            ObjectTraits traits = null;

            int classType = type >> 1;
            bool classIsReference = (classType & 0x01) == 0;

            if (classIsReference)
            {
                int reference = classType >> 1;
                traits = _cacheTraits[reference];
            }
            else
            {
                traits = new ObjectTraits();

                traits.isDynamic = (classType & 0x04) != 0;
                traits.className = amf3ReadString();

                int attributeCount = classType >> 3;

                for (int j = 0; j < attributeCount; j++)
                {
                    traits.members.Add(amf3ReadString());
                }

                _cacheTraits.Add(traits);
            }

            result = _classMapper.createObject(traits.className);
            _cacheObjects.Add(result);

            Dictionary<string, object> properties = new Dictionary<string, object>();

            foreach (string propertyName in traits.members)
            {
                object value = amf3Deserialize();
                properties[propertyName] = value;
            }

            if (traits.isDynamic)
            {
                string propertyName = amf3ReadString();

                while (!string.IsNullOrEmpty(propertyName))
                {
                    object value = amf3Deserialize();
                    properties[propertyName] = value;

                    propertyName = amf3ReadString();
                }
            }

            _classMapper.objectDeserialize(result, properties);

            return result;
        }

        private byte[] amf3ReadByteArray()
        {
            byte[] result = null;

            int type = amf3ReadInteger();

            if (getAsReferenceObject(type, ref result))
            {
                return result;
            }

            int length = type >> 1;

            if (length > (_source.Length - _source.Position))
            {
                throw new AmfExceptionIncomplete();
            }

            result = new byte[length];

            _source.Read(result, 0, length);

            _cacheObjects.Add(result);

            return result;
        }

        private Dictionary<object, object> amf3ReadDictionary()
        {
            Dictionary<object, object> result = null;

            int type = amf3ReadInteger();

            if (getAsReferenceObject(type, ref result))
            {
                return result;
            }

            result = new Dictionary<object, object>();
            _cacheObjects.Add(result);

            int length = type >> 1;

            // Ignore: Not supported in c#
            int weakKeys = _source.TryReadByte();

            for (int j = 0; j < length; j++)
            {
                result[amf3Deserialize()] = amf3Deserialize();
            }

            return result;
        }

    }
}
