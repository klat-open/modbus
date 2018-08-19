using System;

namespace TQElectronic.Utils.Types
{
    public static class BytesUtils
    {
        public static string ToHexString(this byte[] bytes, int offset, int length, char split = ' ')
        {
            return BitConverter.ToString(bytes, offset, length).Replace('-', split);
        }

        public static string ToHexString(this byte[] bytes, char split = ' ')
        {
            return bytes.ToHexString(0, bytes.Length, split);
        }

        public static int ToInt32(this byte[] bytes, int offset = 0, bool srcIsLittleEndian = false)
        {
            return bytes.ToNumber(offset, srcIsLittleEndian, 4, BitConverter.ToInt32);
        }

        public static uint ToUInt32(this byte[] bytes, int offset = 0, bool srcIsLittleEndian = false)
        {
            return bytes.ToNumber(offset, srcIsLittleEndian, 4, BitConverter.ToUInt32);
        }

        public static short ToInt16(this byte[] bytes, int offset = 0, bool srcIsLittleEndian = false)
        {
            return bytes.ToNumber(offset, srcIsLittleEndian, 4, BitConverter.ToInt16);
        }

        public static ushort ToUInt16(this byte[] bytes, int offset = 0, bool srcIsLittleEndian = false)
        {
            return bytes.ToNumber(offset, srcIsLittleEndian, 4, BitConverter.ToUInt16);
        }

        public static long ToInt64(this byte[] bytes, int offset = 0, bool srcIsLittleEndian = false)
        {
            return bytes.ToNumber(offset, srcIsLittleEndian, 4, BitConverter.ToInt64);
        }

        public static ulong ToUInt64(this byte[] bytes, int offset = 0, bool srcIsLittleEndian = false)
        {
            return bytes.ToNumber(offset, srcIsLittleEndian, 4, BitConverter.ToUInt64);
        }

        public static T ToNumber<T>(this byte[] bytes, int offset, bool srcIsLittleEndian, int byteCount, Func<byte[], int, T> convertFunc)
        {
            if (srcIsLittleEndian == BitConverter.IsLittleEndian)
                return convertFunc(bytes, offset);

            var temp = new byte[byteCount];
            Array.Copy(bytes, offset, temp, 0, temp.Length);
            Array.Reverse(temp);
            return convertFunc(temp, 0);
        }

        public static byte[] ToBytes(this int value, bool destIsLittleEndian = false)
        {
            return ToBytes<int>(value, destIsLittleEndian);
        }

        public static byte[] ToBytes(this uint value, bool destIsLittleEndian = false)
        {
            return ToBytes<uint>(value, destIsLittleEndian);
        }

        public static byte[] ToBytes(this short value, bool destIsLittleEndian = false)
        {
            return ToBytes<short>(value, destIsLittleEndian);
        }

        public static byte[] ToBytes(this ushort value, bool destIsLittleEndian = false)
        {
            return ToBytes<ushort>(value, destIsLittleEndian);
        }

        public static byte[] ToBytes(this long value, bool destIsLittleEndian = false)
        {
            return ToBytes<long>(value, destIsLittleEndian);
        }

        public static byte[] ToBytes(this ulong value, bool destIsLittleEndian = false)
        {
            return ToBytes<ulong>(value, destIsLittleEndian);
        }

        public static byte[] ToBytes<T>(this T value, bool destIsLittleEndian = false)
        {
            byte[] bytes = null;

            if (value is int intValue)
                bytes = BitConverter.GetBytes(intValue);
            if (value is uint uintValue)
                bytes = BitConverter.GetBytes(uintValue);
            if (value is short shortValue)
                bytes = BitConverter.GetBytes(shortValue);
            if (value is ushort ushortValue)
                bytes = BitConverter.GetBytes(ushortValue);
            if (value is long longValue)
                bytes = BitConverter.GetBytes(longValue);
            if (value is ulong ulongValue)
                bytes = BitConverter.GetBytes(ulongValue);

            if (bytes == null)
                throw new InvalidCastException("T must be int, uint, short, ushort, long or ulong.");

            if (destIsLittleEndian != BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            return bytes;
        }
    }
}