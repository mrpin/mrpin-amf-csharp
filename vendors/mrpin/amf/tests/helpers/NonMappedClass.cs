namespace AMF
{
    public class NonMappedClass
    {
        /*
         * Fields
         */

        public string nil_property;

        /*
         * Properties
         */

        public string property_one
        {
            get
            {
                return "foo";
            }
        }

        public string another_public_property
        {
            get
            {
                return "a_public_value";
            }
        }

        public string read_only_prop
        {
            set
            {
                //do nothing
            }
        }

        /*
         * Methods
         */
        public NonMappedClass()
        {
        }
    }
}

