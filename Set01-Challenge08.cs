using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge08
    {
        public static bool Run()
        {
            string challengeFile = @"C:\Code\CRYPTOPALS\Set01-Challenge08-SourceFile.txt";
            string[] source = File.ReadAllLines(challengeFile);

            int len = source.Length;
            bool result = false;

            Console.WriteLine("> Detecting ECB by looking for repeating blocks...");

            for (int i=0; i<len; i++) {
                byte[] buf = Set01.Challenge01.ConvertHexStringToByteArray(source[i]);
                if (DetectBlockDuplication(buf, 16)) {
                    Console.WriteLine("> At line #{0} found block repeats", i+1);
                    result = true;
                }
            }

            return result;
        }

        public static bool DetectBlockDuplication(byte[] source, int blockSize)
        {
            byte[][] map = Set01.Challenge06.SplitIntoBlocks(source, blockSize);
            Dictionary<string, bool> dict = new Dictionary<string, bool>();

            string hexkey;

            for (int m = 0, l = map.Length; m < l; m++)
            {
                hexkey = Convert.ToHexString(map[m]);
                if (dict.ContainsKey(hexkey)) return true;
                else dict.Add(hexkey, false);
            }

            return false;
        }

    }
}
