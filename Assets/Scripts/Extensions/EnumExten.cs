using System;

namespace Extensions.EnumExten {
    public static class EnumExten {
        public static byte Byte(this Enum @enum) {
            return Convert.ToByte(@enum);
        }
        public static short Short(this Enum @enum) {
            return Convert.ToInt16(@enum);
        }
        public static ushort UShort(this Enum @enum) {
            return Convert.ToUInt16(@enum);
        }
        public static int Int(this Enum @enum) {
            return Convert.ToInt32(@enum);
        }
        public static uint UInt(this Enum @enum) {
            return Convert.ToUInt32(@enum);
        }
        public static long Long(this Enum @enum) {
            return Convert.ToInt64(@enum);
        }
        public static ulong ULong(this Enum @enum) {
            return Convert.ToUInt64(@enum);
        }
    }
}