using System;
namespace AMF
{
    public class AmfException : Exception
    {
        public AmfException(string message):base(message)
        {

        }
    }
}