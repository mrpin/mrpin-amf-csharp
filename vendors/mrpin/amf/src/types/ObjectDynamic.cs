using System.Collections.Generic;
public class ObjectDynamic : Dictionary<string, object>
{
    /*
     * Fields
     */

    /*
     * Methods
     */
    public ObjectDynamic()
    {
    }

    public void fillProperties(Dictionary<string, object> properties)
    {
        foreach (KeyValuePair<string, object> kvPair in properties)
        {
            this[kvPair.Key] = kvPair.Value;
        }
    }
}
