using System.Collections.Generic;

public class ObjectTraits
{
    /*
     * Fields
     */
    public string className;
    public bool isDynamic;
    public List<string> members;

    /*
     * Methods
     */
    public ObjectTraits()
    {
        members = new  List<string>();
        isDynamic = true;
    }
}
