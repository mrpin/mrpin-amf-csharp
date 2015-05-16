using System.Collections;

namespace AMF
{
    public static class Root
    {
        /*
         * Static properties
         */


        private static ClassMapper _classMapper = new ClassMapper();

        /*
         * Static methods
         */

        public static byte[] serialize(object target)
        {
            Serializer serializer = new Serializer(_classMapper);
            return serializer.serialize(target);
        }

        public static AmfResponse deserialize(byte[] data)
        {
            Deserializer deserializer = new Deserializer(_classMapper);
            return deserializer.deserialize(data);
        }
    }
}
	
