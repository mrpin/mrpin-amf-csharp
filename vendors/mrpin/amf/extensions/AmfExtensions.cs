public static class AmfExtensions
{
    public static bool isNumeric(this object target)
    {
        return target is byte || target is int || target is uint || target is float || target is double;
    }
}
