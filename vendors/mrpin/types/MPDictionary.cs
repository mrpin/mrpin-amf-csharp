using System.Collections.Generic;

public class MPDictionary<TKey, TValue> : Dictionary<TKey, TValue>
{
	public new TValue this [TKey key] {
		get {
			TValue result;

			TryGetValue (key, out result);

			return result;
		}
		set { base [key] = value; }
	}
}
