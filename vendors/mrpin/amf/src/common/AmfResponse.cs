using System.Collections.Generic;
public class AmfResponse
{
    /*
     * Properties
     */

    public List<object> objects
    {
        get;
        private set;
    }

    public byte[] incompleteObject
    {
        get;
        set;
    }

    /*
     * Methods
     */
    public AmfResponse()
    {
        objects = new List<object>();
    }
}
