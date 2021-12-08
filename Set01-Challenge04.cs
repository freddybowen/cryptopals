using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge04
    {
        private readonly static Dictionary<char,float> EngCharFreq = new Dictionary<char, float>() {
            {'E', 12.7f}, {'T', 9.1f}, {'A', 8.2f}, {'O', 7.5f}, {'I', 7.0f},
            {'N', 6.7f}, {'S', 6.3f}, {'H', 6.1f}, {'R', 6.0f}, {'D', 4.3f},
            {'L', 4.0f}, {'C', 2.8f}, {'U', 2.8f}, {'M', 2.4f}, {'W', 2.4f},
            {'F', 2.2f}, {'G', 2.0f}, {'Y', 2.0f}, {'P', 1.9f}, {'B', 1.5f},
            {'V', 1.0f}, {'K', 0.8f}, {'J', 0.15f}, {'X', 0.15f}, {'Q', 0.10f}, {'Z', 0.07f}
        };

        public static bool Run()
        {
            string challengeFile = @"C:\Code\CRYPTOPALS\Set01-Challenge04-SourceFile.txt";
            string[] haystack = File.ReadAllLines(challengeFile);
            int len = haystack.Length;

            byte[] buf; 
            OptimalKey score;
            List<OptimalKey> lstScore = new List<OptimalKey>();          

            for (int i=0; i<len; i++)
            {
                buf = Set01.Challenge01.ConvertHexStringToByteArray(haystack[i]);
                score = OptimalKeyFromSingleCharXOR(buf);
                lstScore.Add(score);
            }

            float bestScore = 0;
            char bestKey = ' ';
            int lineNbr = 0;

            for (int i=0; i<len; i++)
            {
                if (lstScore[i].Score > bestScore) {
                    bestKey = lstScore[i].Key;
                    bestScore = lstScore[i].Score;
                    lineNbr = i;
                }
            }

            buf = Set01.Challenge01.ConvertHexStringToByteArray(haystack[lineNbr]);
            byte[] result = Set01.Challenge03.SingleCharXOR(buf, bestKey);
            score = OptimalKeyFromSingleCharXOR(buf);

            Console.WriteLine("> The key is '{0}' with the score '{1}' for the text: '{2}'", bestKey, bestScore, Encoding.Default.GetString(result));

            return true;
        }

        public struct OptimalKey
        {
            public char Key;
            public float Score;

            public OptimalKey (char k, float s)
            {
                this.Key = k;
                this.Score = s;
            }
        }

        public static OptimalKey OptimalKeyFromSingleCharXOR(byte[] data)
        {
            List<OptimalKey> scoreBoard = new List<OptimalKey>();

            byte[] buf;
            float score;

            for (char key = ' '; key <= 'ÿ'; key++)
            {
                buf = Set01.Challenge03.SingleCharXOR(data, key);
                score = Set01.Challenge03.ScoreTextByCharFreq(Encoding.Default.GetChars(buf));
                scoreBoard.Add(new OptimalKey(key, score));
                //Console.WriteLine(">> Result of XOR with '{0}' (score {1}) as text: {2}", key, score, tmp);
            }

            float bestScore = 0;
            char bestKey = ' ';

            for (int i=0, l=scoreBoard.Count; i<l; i++) {
                if (scoreBoard[i].Score > bestScore) {
                    bestScore = scoreBoard[i].Score;
                    bestKey = scoreBoard[i].Key;
                }
            }

            return new OptimalKey(bestKey, bestScore);
        }
    }
}
