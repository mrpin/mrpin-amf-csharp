using System.Collections.Generic;

public class MappingSet
{
   /*
    * Fields
    */
    private MPDictionary<string, string> _mappingsRemote;
    private MPDictionary<string, string> _mappingsLocal;


    /*
     * Methods
     */

    public MappingSet()
    {
        _mappingsRemote = new MPDictionary<string, string>();
        _mappingsLocal = new MPDictionary<string, string>();
    }

    public void registerClassAlias(string classLocal, string classRemote)
    {
        _mappingsRemote[classRemote] = classLocal;
        _mappingsLocal[classLocal] = classRemote;
    }

    public string getClassNameLocal(string classNameRemote)
    {
        return _mappingsRemote[classNameRemote];
    }

    public string getClassNameRemote(string classNameLocal)
    {
        return _mappingsLocal[classNameLocal];
    }

}
