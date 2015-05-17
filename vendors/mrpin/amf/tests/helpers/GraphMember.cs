using System.Collections.Generic;

namespace AMF
{
    public class GraphMember
    {
        /*
         * Fields
         */

        public List<GraphMember> children
        {
            get;
            private set;
        }

        public object parent
        {
            get;
            set;
        }


        /*
         * Methods
         */
        public GraphMember()
        {
            children = new List<GraphMember>();
        }

        public void AddChild(GraphMember child)
        {
            child.parent = this;
            children.Add(child);
        }
    }

}
