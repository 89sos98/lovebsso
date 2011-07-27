namespace CYQ.Data.Table
{
    using System;

    internal class ValueFormat
    {
        public static string formatString = "{0}";
        public static bool IsFormat = false;

        public static string Format(object value)
        {
            return string.Format(formatString, value);
        }

        public static void Reset()
        {
            IsFormat = false;
            formatString = "{0}";
        }
    }
}

