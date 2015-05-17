using System.Collections.Generic;
namespace AMF
{
    public class AmfResponse
    {
        /*
         * Properties
         */

        public List<object> Objects
        {
            get;
            private set;
        }

        public byte[] IncompleteObject
        {
            get;
            set;
        }

        /*
         * Methods
         */
        public AmfResponse()
        {
            Objects = new List<object>();
        }
    }
}