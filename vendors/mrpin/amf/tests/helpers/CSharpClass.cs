namespace AMF
{
    public class CSharpClass
    {
        /*
         * Fields
         */

        public string foo;
        public string baz;

        /*
         * Methods
         */
        public CSharpClass()
        {
        }

        public void EncodeAmf(Serializer serializer)
        {
            ObjectTraits objectTraits = new  ObjectTraits();
            objectTraits.className = "org.amf.ASClass";
            objectTraits.isDynamic = false;
            objectTraits.members.Add("baz");
            objectTraits.members.Add("foo");

            serializer.WriteObject(this, null, objectTraits);
        }
    }

}
