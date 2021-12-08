using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CRYPTOPALS.Set02
{
    public static class Challenge10
    {
        public static bool Run()
        {
            byte[] key = Encoding.Default.GetBytes("YELLOW SUBMARINE");
            byte[] iv = new byte[] { 0x0 };

            string test = "Lorem ipsum dolor sit amet, consectetur adipiscing elit! Suspendisse ac lorem sit amet ex vehicula laoreet. Pellentesque habitant morbi tristique senectus et netus et malesuada fames ac turpis egestas. Sed dui libero, consequat vitae lectus at, convallis egestas urna. Praesent vitae luctus felis. Aliquam a neque cursus, iaculis leo sit amet, sollicitudin odio. In purus felis, molestie eget turpis vitae, varius porttitor orci. Sed sit amet pretium justo, vel dignissim magna. In tincidunt ornare diam ullamcorper feugiat. In dignissim id nulla ac tempus. Donec blandit imperdiet nisi quis venenatis. Vivamus in magna dui. Duis varius lorem nulla, vitae molestie lorem mollis ac. Quisque venenatis malesuada ligula sit amet convallis. Morbi pulvinar risus ac ex interdum, vitae facilisis sem blandit. Suspendisse vitae justo vitae enim varius vestibulum non ut tortor.";

            byte[] encTest1 = EncryptAES128ECB(PadPKCS7(Encoding.Default.GetBytes(test), 16), key);
            string decTest1 = Encoding.Default.GetString(UnpadPKCS7(Set01.Challenge07.DecryptAES128ECB(encTest1, key)));
            Console.WriteLine("> Testing AES128 ECB Encryption and Decryption... {0}", test == decTest1 ? "PASS" : "FAIL");

            byte[] encTest2 = EncryptAES128CBC_Manual(PadPKCS7(Encoding.Default.GetBytes(test), 16), key, iv);
            string decTest2 = Encoding.Default.GetString(UnpadPKCS7(DecryptAES128CBC_Manual(encTest2, key, iv)));
            Console.WriteLine("> Testing AES128 CBC Encryption and Decryption... {0}", test == decTest2 ? "PASS" : "FAIL");

            string challengeFile = @"C:\Code\CRYPTOPALS\Set02-Challenge10-SourceFile.txt";
            string challangeB64 = File.ReadAllText(challengeFile);
            byte[] challenge = Convert.FromBase64String(challangeB64);

            string solution = Encoding.Default.GetString(DecryptAES128CBC_Manual(challenge, key, iv));
            Console.WriteLine("> Encrypted challenge plain text says:\n\n{0}\n", solution);

            return solution.StartsWith("I'm back and I'm ringin' the bell");
        }

        public static byte[] PadPKCS7(byte[] source, int blockSize)
        {
            int srclen = source.Length;
            int reslen = (srclen < blockSize) ? blockSize : srclen + (blockSize - (srclen % blockSize));

            byte[] result = new byte[reslen];

            Buffer.BlockCopy(source, 0, result, 0, srclen);

            for (int i = srclen; i < reslen; i++){
                result[i] = Convert.ToByte(reslen-srclen);
            }

            return result;
        }


        public static byte[] UnpadPKCS7(byte[] source)
        {
            int srclen = source.Length;
            int padlen = 0;

            byte pad = source[srclen-1];
            if (!(pad >= 0x0 && pad <= 0x15)) throw new Exception("Invalid PKCS7 padding encountered");
         
            for (int i=srclen-1; i>0; i--)
            {
                if (source[i] == pad) padlen++;
                else break;
            }

            byte[] result = new byte[srclen - padlen];
            Buffer.BlockCopy(source, 0, result, 0, srclen - padlen);

            return result;
        }

        public static byte[] EncryptAES128ECB(byte[] source, byte[] key)
        {
            int blockSize = 16;
            int srclen = source.Length;

            if (srclen % blockSize != 0) throw new Exception("Source length must be padded to blocksize.");

            byte[] result = new byte[srclen];
            int blocks = srclen / blockSize + (srclen % blockSize > 0 ? 1 : 0);

            using (var algo = AesManaged.Create())
            {
                algo.Mode = CipherMode.ECB;
                algo.Padding = PaddingMode.None;
                algo.Key = key;

                using (var encryptor = algo.CreateEncryptor())
                {
                    for (int i = 0; i < blocks; i++) {
                        encryptor.TransformBlock(source, i*blockSize, blockSize, result, i*blockSize);
                    }
                }
            }
            return result;
        }

        public static byte[] EncryptAES128CBC_Manual(byte[] source, byte[] key, byte[] iv)
        {
            int blockSize = 16;
            int srclen = source.Length;

            if (srclen % blockSize != 0) throw new Exception("Source length must be padded to blocksize.");

            byte[] result = new byte[srclen];
            int blocks = srclen / blockSize + (srclen % blockSize > 0 ? 1 : 0);

            byte[] buf = new byte[blockSize];
            byte[] lastBlock = new byte[blockSize];

            using (var algo = AesManaged.Create())
            {
                algo.Mode = CipherMode.ECB;
                algo.Padding = PaddingMode.None;
                algo.Key = key;

                using (var encryptor = algo.CreateEncryptor())
                {
                    for (int i = 0; i < blocks; i++)
                    {
                        Buffer.BlockCopy(source, i*blockSize, buf, 0, blockSize);
                        if (i == 0){
                            RepeatingXOR(buf, iv);
                        }
                        else {
                            Buffer.BlockCopy(result, (i-1)*blockSize, lastBlock, 0, blockSize);
                            RepeatingXOR(buf, lastBlock);
                        }
                        encryptor.TransformBlock(buf, 0, blockSize, result, i*blockSize);
                    }
                }
            }
            return result;
        }


        public static byte[] DecryptAES128CBC_Manual(byte[] source, byte[] key, byte[] iv)
        {
            int blockSize = 16;
            int srclen = source.Length;

            if (srclen % blockSize != 0) throw new Exception("Source length must be padded to blocksize.");

            byte[] result = new byte[srclen];
            int blocks = srclen / blockSize + (srclen % blockSize > 0 ? 1 : 0);

            byte[] buf = new byte[blockSize];
            byte[] lastBlock = new byte[blockSize];

            using (var algo = AesManaged.Create())
            {
                algo.Mode = CipherMode.ECB;
                algo.Padding = PaddingMode.None;
                algo.Key = key;

                using (var decryptor = algo.CreateDecryptor())
                {
                    for (int i = 0; i < blocks; i++)
                    {
                        decryptor.TransformBlock(source, i*blockSize, blockSize, buf, 0);
                        if (i == 0){
                            RepeatingXOR(buf, iv);
                        }
                        else {
                            Buffer.BlockCopy(source, (i-1)*blockSize, lastBlock, 0, blockSize);
                            RepeatingXOR(buf, lastBlock);
                        }
                        Buffer.BlockCopy(buf, 0, result, i*blockSize, blockSize);
                    }
                }
            }
            return result;
        }

        /*
        public static byte[] RepeatingXOR(byte[] source, byte[] key)
        {
            int srclen = source.Length;

            byte[] result = new byte[srclen];
            source.CopyTo(result, 0);

            int keypos = 0;
            int keylen = key.Length;

            int x;

            for (int i = 0; i < srclen; i++)
            {
                x = key[keypos++ % keylen];
                result[i] ^= (byte)x;
            }

            return result;
        }
        */


        public static void RepeatingXOR(byte[] source, byte[] key)
        {
            int srclen = source.Length;
            int keypos = 0;
            int keylen = key.Length;

            for (int i = 0; i < srclen; i++) {
                source[i] ^= key[keypos++ % keylen];
            }
        }
    }
}
