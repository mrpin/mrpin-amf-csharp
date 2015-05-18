using System.Collections.Generic;

public class MPDictionarySorted<TKey, TValue> : SortedDictionary<TKey, TValue>
{
    /*
     * Properties
     */

    public new TValue this [TKey key]
    {
        get
        {
            TValue result;

            TryGetValue(key, out result);

            return result;
        }
        set
        {
            base[key] = value;
        }
    }

    /*
     * Methods
     */
    public MPDictionarySorted()
    {
    }
}