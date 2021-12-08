using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge07
    {
        public static bool Run()
        {
            string challengeFile = @"C:\Code\CRYPTOPALS\Set01-Challenge07-SourceFile.txt";
            string challangeB64 = File.ReadAllText(challengeFile);

            string challengeKey = "YELLOW SUBMARINE";

            byte[] key = Encoding.Default.GetBytes(challengeKey);
            byte[] source = Convert.FromBase64String(challangeB64);
            byte[] result = DecryptAES128ECB(source, key);

            Console.WriteLine("> Decrypted challenge cyper text to:\n\n{0}\n\n", Encoding.Default.GetString(result));

            return true;
        }


        public static byte[] DecryptAES128ECB(byte[] source, byte[] key)
        {
            int blockSize = 16;
            int srclen = source.Length;

            if (srclen % blockSize != 0) throw new Exception("Source length must be padded to blocksize.");

            byte[] result = new byte[srclen];
            int blocks = srclen / blockSize + (srclen % blockSize > 0 ? 1 : 0);

            using (var algo = Aes.Create())
            {
                algo.Mode = CipherMode.ECB;
                algo.BlockSize = 128;
                algo.Padding = PaddingMode.None;

                using (var decryptor = algo.CreateDecryptor(key, algo.IV))
                {
                    for (int i = 0; i < blocks; i++) {
                        decryptor.TransformBlock(source, i * blockSize, blockSize, result, i * blockSize);
                    }
                }
            }
            return result;
        }

    }
}
