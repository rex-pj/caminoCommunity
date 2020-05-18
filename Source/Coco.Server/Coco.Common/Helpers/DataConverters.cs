using System;
using System.Text;

namespace Coco.Common.Helpers
{
    public class DataConverters
    {
        public static string BytesToString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte byteData in bytes)
            {
                hex.AppendFormat("{0:x2}", byteData);
            }

            return hex.ToString();
        }

        public static byte[] StringToBytes(string hex)
        {
            var numberChars = hex.Length;
            var bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
            }

            return bytes;
        }
    }
}
