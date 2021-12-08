using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge05
    {
        public static bool Run()
        {
            string challengeFile = @"C:\Code\CRYPTOPALS\Set01-Challenge05-SourceFile.txt";
            string source = File.ReadAllText(challengeFile).Replace("\r\n", "\n"); // convert line endings to get expected result
            int srclen = source.Length;

            string expectResult = "0b3637272a2b2e63622c2e69692a23693a2a3c6324202d623d63343c2a26226324272765272a282b2f20430a652e2c652a3124333a653e2b2027630c692b20283165286326302e27282f";

            char[] key = ("ICE").ToCharArray();

            byte[] decoded = RepeatingKeyXOR(Encoding.Default.GetBytes(source), key);
            string result = BitConverter.ToString(decoded).Replace("-", string.Empty).ToLowerInvariant();

            byte[] buf2 = RepeatingKeyXOR(decoded, key);
            string source2 = Encoding.Default.GetString(buf2);

            Console.WriteLine("> Using '{0}' as Repeating Key XOR to encrypt the string '{1}' yielding the cypher text:\n{2}", new string(key), source, result);

            return expectResult == result && source == source2;
        }

        public static byte[] RepeatingKeyXOR(byte[] source, char[] key)
        {
            int srclen = source.Length;

            byte[] result = new byte[srclen];
            source.CopyTo(result, 0);

            int keypos = 0;
            int keylen = key.Length;

            char x;

            for (int i = 0; i < srclen; i++)
            {
                x = key[keypos++ % keylen];
                result[i] ^= (byte)x;
            }

            return result;
        }
    }
}
