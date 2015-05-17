using System.Collections.Generic;
namespace AMF
{
    public class CacheBase <TKey>
    {
        /*
          * Fields
          */

        private int _cacheIndex;
        private Dictionary<TKey, int> _cache;

        /*
         * Properties
         */
        public int this[TKey key]
        {
            get
            {
                int result;

                if (!_cache.TryGetValue(key, out result))
                {
                    result = - 1;
                }

                return result;
            }
        }

        /*
         * Methods
         */


        public CacheBase()
        {
            _cache = new Dictionary<TKey, int>();
            _cacheIndex = 0;
        }

        public void addToCache(TKey value)
        {
            _cache[value] = _cacheIndex;
            _cacheIndex++;
        }
    }
}
