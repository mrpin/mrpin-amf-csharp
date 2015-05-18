using System.Collections.Generic;

namespace AMF
{
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

        public void RegisterClassAlias(string classLocal, string classRemote)
        {
            _mappingsRemote[classRemote] = classLocal;
            _mappingsLocal[classLocal] = classRemote;
        }

        public string GetClassNameLocal(string classNameRemote)
        {
            return _mappingsRemote[classNameRemote];
        }

        public string GetClassNameRemote(string classNameLocal)
        {
            return _mappingsLocal[classNameLocal];
        }

    }
}