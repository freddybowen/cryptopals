using System;
using System.Diagnostics;
using System.Globalization;
using System.Collections.Generic;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge01
    {
        public static bool Run()
        {
            string hexString = @"49276d206b696c6c696e6720796f757220627261696e206c696b65206120706f69736f6e6f7573206d757368726f6f6d";
            string expectResult = @"SSdtIGtpbGxpbmcgeW91ciBicmFpbiBsaWtlIGEgcG9pc29ub3VzIG11c2hyb29t";

            string result = ConvertHexToBase64(hexString);

            return (result == expectResult);
        }

        public static string ConvertHexToBase64(string hexString)
        {
            byte[] hex = null;
            try
            {
                hex = ConvertHexStringToByteArray(hexString);
            }
            catch (Exception ex)
            {
                Console.WriteLine("> Exception says: {0}", ex.ToString());
            }

            Console.WriteLine("> Hex string as text says: {0}", Encoding.Default.GetString(hex));

            return Convert.ToBase64String(hex);
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
