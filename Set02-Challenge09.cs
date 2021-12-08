using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CRYPTOPALS.Set02
{
    public static class Challenge09
    {
        public static bool Run()
        {
            string txt = "YELLOW SUBMARINE";
            byte[] buf = Encoding.Default.GetBytes(txt);

            byte[] res = PadToLength(buf, 20);
            string tmp = Encoding.Default.GetString(res);

            return res.Length==20;
        }

        public static byte[] PadToLength(byte[] source, int padToLength)
        {
            int srclen = source.Length;

            if (srclen > padToLength) throw new Exception("Source length exceeds requested length after padding.");

            byte[] result = new byte[padToLength];

            Buffer.BlockCopy(source, 0, result, 0, srclen);

            for (int i = srclen; i < padToLength; i++) {
                result[i] = 0x04;
            }

            return result;
        }

    }
}
