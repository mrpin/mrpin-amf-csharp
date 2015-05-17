using System.Collections.Generic;
namespace AMF
{
    public class ObjectDynamic
    {
        /*
         * Fields
         */
        private SortedDictionary<string, object> _data;

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
            _data = new SortedDictionary<string, object>();
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
    }
}