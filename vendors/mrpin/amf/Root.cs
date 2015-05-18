using System.Collections;
using System.Collections.Generic;

namespace AMF
{
    /// <summary>
    /// Represents root object for acces to AMF3 serializer and deserializer.
    ///
    /// == serialize example ==
    /// var object = MyAwesomeRequest();
    /// object.request_id = 100500;
    /// object.data = "Some string";
    ///
    /// byte[] amf3Data = AMF.Root.Serialize(object);
    ///
    /// == deserialize example ==
    ///
    /// //get data from network, file, etc.
    /// byte[] amf3Data = ...;
    ///
    /// AmfResponse response = AMF.Root.deserialize(amf3Data)
    ///
    /// foreach(var item in response.Objects)
    /// {
    /// AMF.AmfLogger.Log("received object with type: " + item.GetType().FullName);
    /// }
    ///
    /// if(response.IncomcompleteObject != null)
    /// {
    /// AMF.AmfLogger.Log("Also this data contains incomplete object");
    /// }
    ///
    /// </summary>
    public static class Root
    {
        /*
         * Static properties
         */

        private static ClassMapper _classMapper;

        /*
         * Static methods
         */
        static Root()
        {
            _classMapper = new ClassMapper();
        }

        /// <summary>
        /// Use it for serialize C# objects to AMF3 data.
        /// </summary>
        /// <param name="target">Any object which you want to serialize</param>
        /// <returns>ByteArray with encoded AMF3 data</returns>
        public static byte[] Serialize(object target)
        {
            Serializer serializer = new Serializer(_classMapper);
            return serializer.Serialize(target);
        }

        /// <summary>
        /// Use it for deserialize AMF3 data.
        /// </summary>
        /// <param name="data">ByteArray with AMF3 encoded data</param>
        /// <returns>AmfResponse which contains received objects and incomplete response if it exist</returns>

        public static AmfResponse Deserialize(byte[] data)
        {
            Deserializer deserializer = new Deserializer(_classMapper);
            return deserializer.Deserialize(data);
        }

        /*
         * Mapper methods
         */

        public static void RegisterClassAlias(string classLocal, string classRemote)
        {
            _classMapper.RegisterClassAlias(classLocal, classRemote);
        }

        //! classMap - dictionary with key - class name local, value - class name remote
        public static void RegisterClasses(Dictionary<string, string> classMap)
        {
            _classMapper.RegisterClasses(classMap);
        }

        public static void ClearClassAliases()
        {
            _classMapper.Reset();
        }
    }
}
	
