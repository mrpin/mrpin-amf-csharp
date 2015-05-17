using System;

namespace AMF
{
    public static class AmfExtensions
    {
        public static bool isNumeric(this object target)
        {
            return target is byte || target is int || target is uint || target is float || target is double;
        }

        public static double UnixTimestamp(this DateTime target)
        {
            return target.Subtract(AmfConstants.UnixEpocTime).TotalSeconds;
        }
    }
}
