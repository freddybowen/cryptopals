using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge06
    {
        public static bool Run()
        {
            string testHammingA = "this is a test";
            string testHammingB = "wokka wokka!!!";
            int expectHamming = 37;
            int testHamming = CalculateHammingDistance(Encoding.Default.GetBytes(testHammingA), Encoding.Default.GetBytes(testHammingB));

            Console.WriteLine("> Found Hamming distance \"{0}\" between \"{1}\" and \"{2}\": {3}", testHamming, testHammingA, testHammingB, testHamming == expectHamming ? "PASS" : "FAIL");

            string challengeFile = @"C:\Code\CRYPTOPALS\Set01-Challenge06-SourceFile.txt";
            string challangeB64 = File.ReadAllText(challengeFile);

            byte[] source = Convert.FromBase64String(challangeB64);
            int srclen = source.Length;

            int minKeySize = 2;
            int maxKeySize = 40;

            byte[] a, b, c, d;
            int distance1, distance2, distance3;
            List<EvaluatedDistance> lstDistance = new List<EvaluatedDistance>(maxKeySize - minKeySize);
            float averageDistance;

            for (int i = minKeySize; i < maxKeySize; i++)
            {
                a = new byte[i];
                b = new byte[i];
                c = new byte[i];
                d = new byte[i];
                Buffer.BlockCopy(source, 0, a, 0, i);
                Buffer.BlockCopy(source, i*1, b, 0, i);
                Buffer.BlockCopy(source, i*2, c, 0, i);
                Buffer.BlockCopy(source, i*3, d, 0, i);

                distance1 = CalculateHammingDistance(a, b);
                distance2 = CalculateHammingDistance(b, c);
                distance3 = CalculateHammingDistance(c, d);

                averageDistance = (distance1 + distance2 + distance3) / 3;

                lstDistance.Add(new EvaluatedDistance(i, averageDistance, (float)averageDistance/i));
            }

            // take the 3 smallest (normalized) key sizes 
            int[] keysizes = lstDistance.OrderBy(x => x.NormalizedDistance).Select(x => x.KeySize).Take(3).ToArray();

            Console.WriteLine("> Trying top '{0}' key sizes based on Hamming distance...", keysizes.Length);

            byte[][] blockwise;
            byte[][] transposed;
            char[] guessKey;
            byte[] guessDecrypt;
            float guessScore;
            List<EvaluateResult> lstResult = new List<EvaluateResult>(keysizes.Length);

            for (int k=0, keySize; k<keysizes.Length; k++)
            {
                keySize = keysizes[k];
                blockwise = SplitIntoBlocks(source, keySize);
                transposed = TransposeBlocks(blockwise, keySize);
                guessKey = new char[keySize];

                //Console.WriteLine("> Evaluating key size \"{0}\" over \"{1}\" blocks...", keySize, blockwise.Length);

                for (int position = 0; position < keySize; position++){
                    guessKey[position] = Set01.Challenge04.OptimalKeyFromSingleCharXOR(transposed[position]).Key;
                }

                guessDecrypt = Set01.Challenge05.RepeatingKeyXOR(source, guessKey);
                guessScore = Set01.Challenge03.ScoreTextByCharFreq(Encoding.ASCII.GetChars(guessDecrypt));
                lstResult.Add(new EvaluateResult(guessKey, guessScore, guessDecrypt));

                Console.WriteLine(">> Evaluated key length '{0}' with the key '{1}' which yields a score of '{2}'", keySize, new string(guessKey), guessScore);
            }

            EvaluateResult result = lstResult.OrderByDescending(x => x.Score).First();

            Console.WriteLine("> Best result is key '{0}' with score '{1}' which decrypts as:\n\n{2}\n\n", new string(result.Key), result.Score, Encoding.ASCII.GetString(result.Result));

            return true;
        }


        public struct EvaluatedDistance
        {
            public int KeySize;
            public float Distance;
            public float NormalizedDistance;

            public EvaluatedDistance(int s, float d, float n)
            {
                this.KeySize = s;
                this.Distance = d;
                this.NormalizedDistance = n;
            }
        }

        public struct EvaluateResult
        {
            public char[] Key;
            public float Score;
            public byte[] Result;

            public EvaluateResult(char[] key, float score, byte[] result)
            {
                this.Key = key;
                this.Score = score;
                this.Result = result;
            }
        }

        public static byte[][] TransposeBlocks(byte[][] blocks, int blocklen)
        {
            int segments = blocks.Length;
            byte[][] transpose = new byte[blocklen][];

            for (int x = 0; x < blocklen; x++)
            {
                transpose[x] = new byte[segments];
                for (int y = 0; y < segments; y++)
                {
                    transpose[x][y] = blocks[y][x];
                }
            }

            return transpose;
        }

        public static byte[] ByteToBlock(byte b, int blocklen)
        {
            byte[] result = new byte[blocklen];

            for (int i=0; i<blocklen; i++) {
                result[i] = b;
            }

            return result;
        }

        public static byte[][] SplitIntoBlocks(byte[] source, int blocklen)
        {
            int srclen = source.Length;
            int segments = (srclen / blocklen) + (srclen % blocklen > 0 ? 1 : 0);
            byte[][] blocks = new byte[segments][];

            byte[] buf = new byte[blocklen];
            for (int s=0; s<segments; s++)
            {
                blocks[s] = new byte[blocklen];
                Buffer.BlockCopy(source, s * blocklen, blocks[s], 0, (s + 1 == segments ? srclen - (s * blocklen) : blocklen));
            }

            return blocks;
        }

        public static int CalculateHammingDistance(byte[] a, byte[] b)
        {
            if (a.Length != b.Length){
                throw new ArgumentException("Hamming distance cannot be calculated between byte arrays of unequal length.");
            }

            int len = a.Length, n = 0, x;
            
            for (int i=0; i<len; i++)
            {
                x = a[i] ^ b[i];

                while (x > 0)
                {
                    n += x & 1;
                    x >>= 1;
                }
            }

            return n;
        }

    }
}
