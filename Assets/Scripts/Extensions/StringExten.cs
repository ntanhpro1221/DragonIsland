using System;

namespace Extensions.StringExten {
    public static class StringExten {
        public static T Enum<T>(this string @string) {
            return (T)System.Enum.Parse(typeof(T), @string);
        }
        public static byte Byte(this string @string) {
            return Convert.ToByte(@string);
        }
        public static short Short(this string @string) {
            return Convert.ToInt16(@string);
        }
        public static ushort UShort(this string @string) {
            return Convert.ToUInt16(@string);
        }
        public static int Int(this string @string) {
            return Convert.ToInt32(@string);
        }
        public static uint UInt(this string @string) {
            return Convert.ToUInt32(@string);
        }
        public static long Long(this string @string) {
            return Convert.ToInt64(@string);
        }
        public static ulong ULong(this string @string) {
            return Convert.ToUInt64(@string);
        }
    }
}