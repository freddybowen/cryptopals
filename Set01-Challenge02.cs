using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge02
    {
        public static bool Run()
        {
            string hexString1 = @"1c0111001f010100061a024b53535009181c";
            string hexString2 = @"686974207468652062756c6c277320657965";
            string expectResult = @"746865206b696420646f6e277420706c6179";

            byte[] buf1 = Set01.Challenge01.ConvertHexStringToByteArray(hexString1);
            Console.WriteLine("> Hex string 1 as text says: {0}", Encoding.Default.GetString(buf1));

            byte[] buf2 = Set01.Challenge01.ConvertHexStringToByteArray(hexString2);
            Console.WriteLine("> Hex string 2 as text says: {0}", Encoding.Default.GetString(buf2));

            byte[] expect = Set01.Challenge01.ConvertHexStringToByteArray(expectResult);
            Console.WriteLine("> Expected result string as text says: {0}", Encoding.Default.GetString(expect));

            Console.WriteLine("> Performing XOR on string 1 with string 2...");
            byte[] result = FixedXOR(buf1, buf2);
            Console.WriteLine("> Result string as text says: {0}", Encoding.Default.GetString(result));

            string resultString = BitConverter.ToString(result).Replace("-", string.Empty);
            Console.WriteLine("> Result string as hex is: {0}", resultString);

            return (expectResult.ToLowerInvariant() == resultString.ToLowerInvariant());
        }

        public static byte[] FixedXOR(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "Fixed XOR requires equal length buffers:\n\t{0}\n\t{1} ", a, b));

            for (int i=0; i<a.Length; i++)
            {
                a[i] ^= b[i];
            }

            return a;

        }

        public static byte[] ConvertHexStringToByteArray(string hexString)
        {
            if (hexString.Length % 2 != 0) {
                throw new ArgumentException(String.Format(CultureInfo.InvariantCulture, "The binary key cannot have an odd number of digits: {0}", hexString));
            }

            byte[] data = new byte[hexString.Length / 2];
            for (int index = 0; index < data.Length; index++)
            {
                string byteValue = hexString.Substring(index * 2, 2);
                data[index] = byte.Parse(byteValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture);
            }

            return data;
        }
    }
}
