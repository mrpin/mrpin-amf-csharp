using AMF;
using Mono.Xml.Xsl;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace AMF
{
    public class Serializer
    {
        /*
         * Static fields
         */

        private DateTime _dateTimeUtcZero = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /*
         * Fields
         */
        private ClassMapper _classMapper;

        private MemoryStream _stream;

        private int _depth;

        //caches

        private CacheStrings _cacheStrings;
        private CacheObjects _cacheObjects;
        private CacheStrings _cacheTraits;

        /*
         * Methods
         */

        public Serializer(ClassMapper classMapper)
        {
            _classMapper = classMapper;
            _stream = new MemoryStream();
            _depth = 0;
        }

        public byte[] serialize(object target)
        {
            //Initialize caches
            if (_depth == 0)
            {
                _cacheStrings = new CacheStrings();
                _cacheObjects = new CacheObjects();
                _cacheTraits = new CacheStrings();
            }

            _depth++;

            amf3Serialize(target);

            _depth--;

            //cleanup
            if (_depth == 0)
            {
                _cacheStrings = null;
                _cacheObjects = null;
                _cacheTraits = null;
            }

            return _stream.ToArray();
        }

        private void amf3Serialize(object target)
        {
            MethodInfo methodInfo = null;

            if (target != null)
            {
                methodInfo = target.GetType().GetMethod("encodeAmf", BindingFlags.Public);
            }

            if (methodInfo != null)
            {
                Object[] methodParams = new Object[]
                {
                    this
                };

                methodInfo.Invoke(target, methodParams);
            }
            else if (target == null)
            {
                amd3WriteNull();
            }
            else if (Boolean.Equals(true, target))
            {
                amd3WriteTrue() ;
            }
            else if (Boolean.Equals(false, target))
            {
                amf3WriteFalse();
            }
            else if (target.isNumeric())
            {
                amf3WriteNumeric(target);
            }
            else if (target is string)
            {
                amf3WriteString(target as string);
            }
            else if (target is DateTime)
            {
                amf3WriteDateTime((DateTime)target);
            }
            else if (target is byte[])
            {
                amf3WriteByteArray((byte[])target);
            }
            else if (target is ICollection)
            {
                amf3WriteArray(target as ICollection);
            }
            else if (target is Dictionary<string, object>)
            {
                amf3WriteDictionary(target as Dictionary<string, object>);
            }
            else if (target is object)
            {
                amf3WriteObject(target);
            }
            else
            {
                throw new AmfException("unknown type for serialize");
            }
        }

        // Helper for writing arrays inside encodeAmf
        public void writeArray(ICollection value)
        {
            amf3WriteArray(value);
        }

        // Helper for writing objects inside encodeAmf
        public void writeObject(object target, Dictionary<string, object> properties = null, ObjectTraits traits = null)
        {
            amf3WriteObject(target, properties, traits);
        }

        private void amf3WriteReference(int index)
        {
            int header = index << 1; //shift value left to leave a low bit of 0
            _stream.WriteInteger(header);
        }

        private void amd3WriteNull()
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_NULL);
        }

        private void amd3WriteTrue()
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_TRUE);
        }

        private void amf3WriteFalse()
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_FALSE);
        }

        private void amf3WriteNumeric(object value)
        {
            if (value is byte)
            {
                _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_INTEGER);
                _stream.WriteInteger(byte.Parse(value.ToString()));
            }
            else if (value is int)
            {
                int targetInteger = (int)value;

                if (AmfConstants.INTEGER_MIN < (int)value && (int)value < AmfConstants.INTEGER_MAX)
                {
                    _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_INTEGER);
                    _stream.WriteInteger(targetInteger);
                }
                else
                {
                    _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_INTEGER);
                    _stream.WriteDouble(targetInteger);
                }
            }
            else if (value is uint)
            {
                uint targetUint = uint.Parse(value.ToString());

                if (targetUint < AmfConstants.INTEGER_MAX)
                {
                    _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_INTEGER);
                    _stream.WriteInteger(int.Parse(targetUint.ToString()));
                }
                else
                {
                    _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_DOUBLE);
                    _stream.WriteDouble(targetUint);
                }
            }
            else if (value is float || value is double)
            {
                _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_DOUBLE);
                _stream.WriteDouble(double.Parse(value.ToString()));
            }
        }

        private void amf3WriteString(string value)
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_STRING);
            amf3WriteStringInternal(value);
        }

        private void amf3WriteStringInternal(string value)
        {
            int indexInCache = _cacheStrings[value];

            if (value.Length == 0)
            {
                _stream.WriteByte((byte)AmfConstants.AMF3_EMPTY_STRING);
            }
            else if (indexInCache != - 1)
            {
                amf3WriteReference(indexInCache);
            }
            else
            {
                _cacheStrings.addToCache(value);

                byte[] buffer = Encoding.UTF8.GetBytes(value);

                //write size
                int head = ((int)buffer.Length << 1) | 1;
                _stream.WriteInteger(head);

                //write string
                _stream.Write(buffer, 0, buffer.Length);
            }
        }

        private void amf3WriteDateTime(DateTime value)
        {
            int indexInCache = _cacheObjects[value];

            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_DATE);

            if (indexInCache != - 1)
            {
                amf3WriteReference(indexInCache);
            }
            else
            {
                _cacheObjects.addToCache(value);

                value = value.ToUniversalTime();

                TimeSpan diff = value.Subtract(_dateTimeUtcZero);

                _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_NULL);
                _stream.WriteDouble(diff.TotalMilliseconds);
            }
        }

        private void amf3WriteByteArray(byte[] value)
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_BYTE_ARRAY);

            int indexInCache = _cacheObjects[value];

            if (indexInCache != - 1)
            {
                amf3WriteReference(indexInCache);
            }
            else
            {
                _cacheObjects.addToCache(value);

                //add length
                _stream.WriteInteger(value.Length << 1 | 1);
                _stream.Write(value, 0, value.Length);
            }
        }

        private void amf3WriteArray(ICollection value)
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_ARRAY);

            int indexInCache = _cacheObjects[value];

            if (indexInCache != - 1)
            {
                amf3WriteReference(indexInCache);
                return;
            }

            _cacheObjects.addToCache(value);

            //make room for a low bit of 1
            int header = value.Count << 1;
            //set the low bit to 1
            header = header | 1;
            _stream.WriteInteger(header);

            _stream.WriteByte((byte)AmfConstants.AMF3_CLOSE_DYNAMIC_ARRAY);

            foreach (object item in value)
            {
                amf3Serialize(item);
            }
        }

        //todo:finish
        private void amf3WriteDictionary(Dictionary<string, object> value)
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_DICTIONARY);

            int indexInCache = _cacheObjects[value];

            if (indexInCache != - 1)
            {
                amf3WriteReference(indexInCache);
                return;
            }

            _cacheObjects.addToCache(value);

            //            int header = 0x0B;


            //                IDictionary dic = obj as IDictionary;
            //                uint head = 0x0B;
            //                WriteInt(stm, head);
            //                if (obj is CNameObjDict)
            //                    WriteString(stm, (obj as CNameObjDict).className);
            //                else
            //                    WriteString(stm, "");
            //                foreach (DictionaryEntry e in obj as IDictionary)
            //                {
            //                    if (e.Key.ToString() == "" && e.Value is string)    //解析时为了好看放进去的ClassName
            //                        continue;
            //                    WriteString(stm, e.Key.ToString());
            //                    WriteAmf(stm, e.Value);
            //                }
            //                WriteString(stm, "");
        }

        private void amf3WriteObject(object value, Dictionary<string, object> properties = null, ObjectTraits traits = null)
        {
            _stream.WriteByte((byte)AmfConstants.AMF3_MARKER_OBJECT);

            int indexInCache = _cacheObjects[value];

            if (indexInCache != - 1)
            {
                amf3WriteReference(indexInCache);
                return;
            }

            _cacheObjects.addToCache(value);

            // Calculate traits if not given
            bool useDefaultClassName = false;

            if (traits == null)
            {
                traits = new ObjectTraits();
                //todo:
                traits.className = _classMapper.getClassNameRemote("");

                if (string.IsNullOrEmpty(traits.className))
                {
                    useDefaultClassName = true;
                }
            }

            string className = useDefaultClassName ? "__default__" : traits.className;

            int indexInCacheTraits = _cacheTraits[className];

            if (className != null && indexInCacheTraits != - 1)
            {
                _stream.WriteInteger(indexInCacheTraits << 2 | 0x01);
            }
            else
            {
                if (className != null)
                {
                    _cacheTraits.addToCache(className);
                }
                //Write out trait header

                int header = 0x03;// Not object ref and not trait ref

                if (traits.isDynamic)
                {
                    header |= 0x02 << 2;
                }

                header |= traits.members.Count << 4;

                _stream.WriteInteger(header);


                // Write out class name
                amf3WriteStringInternal(useDefaultClassName ? "" : className);

                foreach (string memberName in traits.members)
                {
                    amf3WriteStringInternal(memberName);
                }
            }

            //Extract properties if not given
            if (properties == null)
            {
                properties = _classMapper.objectSerialize(value);
            }

            for (int j = traits.members.Count - 1; j >= 0 ; j--)
            {
                string memberName = traits.members[j];

                amf3Serialize(properties[memberName]);

                properties.Remove(memberName);
            }

            if (traits.isDynamic)
            {
                // Write dynamic properties
                foreach (KeyValuePair<string, object> kvPair in properties)
                {
                    amf3WriteStringInternal(kvPair.Key);
                    amf3Serialize(kvPair.Value);
                }
            }

            _stream.WriteByte((byte)AmfConstants.AMF3_CLOSE_DYNAMIC_OBJECT);
        }
    }
}
