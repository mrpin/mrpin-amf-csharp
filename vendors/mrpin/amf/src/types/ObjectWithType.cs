using System.Collections.Generic;

public class ObjectWithType : ObjectDynamic
{
    /*
     * Properties
     */

    public string className
    {
        get;
        private set;
    }

    /*
     * Methods
     */

    //! Default constructor
    public ObjectWithType(string className):base()
    {
        this.className = className;
    }
}
