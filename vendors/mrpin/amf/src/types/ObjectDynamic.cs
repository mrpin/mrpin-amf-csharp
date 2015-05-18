using System;
using System.Collections.Generic;

namespace AMF
{
    public class ObjectDynamic
    {
        /*
         * Fields
         */
        private MPDictionarySorted<string, object> _data;

        /*
         * Properties
         */

        public object this[string key]
        {
            get
            {
                return _data[key];
            }
            set
            {
                _data[key] = value;
            }
        }

        /*
         * Methods
         */
        public ObjectDynamic()
        {
            _data = new MPDictionarySorted<string, object>();
        }

        public SortedDictionary<string, object> serialize()
        {
            return  _data;
        }

        public void deserialize(Dictionary<string, object> data)
        {
            foreach (KeyValuePair<string, object> kvPair in data)
            {
                this[kvPair.Key] = kvPair.Value;
            }
        }


        public override bool Equals(object obj)
        {
            if (obj == null || !(obj is ObjectDynamic))
            {
                return false;
            }

            ObjectDynamic otherObj = obj as ObjectDynamic;

            bool result = true;

            if (otherObj._data.Keys.Count != _data.Keys.Count)
            {
                result = false;
            }
            else
            {
                foreach(string key in _data.Keys)
                {
                    if (!Object.Equals(_data[key], otherObj._data[key]))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }
    }
}