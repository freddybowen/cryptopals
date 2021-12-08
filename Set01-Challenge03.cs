using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CRYPTOPALS.Set01
{
    public static class Challenge03
    {
        // from : https://reusablesec.blogspot.com/2009/05/character-frequency-analysis-info.html
        private readonly static Dictionary<char, float> EnglishCharacterFreq = new Dictionary<char, float>()
        {
            {'a', 7.52766f}, {'e', 7.0925f}, {'o', 5.17f}, {'r', 4.96032f}, {'i', 4.69732f}, {'s', 4.61079f}, {'n', 4.56899f},
            {'1', 4.35053f}, {'t', 3.87388f}, {'l', 3.77728f}, {'2', 3.12312f}, {'m', 2.99913f}, {'d', 2.76401f}, {'0', 2.74381f},
            {'c', 2.57276f}, {'p', 2.45578f}, {'3', 2.43339f}, {'h', 2.41319f}, {'b', 2.29145f}, {'u', 2.10191f}, {'k', 1.96828f},
            {'4', 1.94265f}, {'5', 1.88577f}, {'g', 1.85331f}, {'9', 1.79558f}, {'6', 1.75647f}, {'8', 1.66225f}, {'7', 1.621f},
            {'y', 1.52483f}, {'f', 1.2476f}, {'w', 1.24492f}, {'j', 0.836677f}, {'v', 0.833626f}, {'z', 0.632558f}, {'x', 0.573305f},
            {'q', 0.346119f}, {'A', 0.130466f}, {'S', 0.108132f}, {'E', 0.0970865f}, {'R', 0.08476f}, {'B', 0.0806715f}, {'T', 0.0801223f},
            {'M', 0.0782306f}, {'L', 0.0775594f}, {'N', 0.0748134f}, {'P', 0.073715f}, {'O', 0.0729217f}, {'I', 0.070908f}, {'D', 0.0698096f},
            {'C', 0.0660872f}, {'H', 0.0544319f}, {'G', 0.0497332f}, {'K', 0.0460719f}, {'F', 0.0417393f}, {'J', 0.0363083f}, {'U', 0.0350268f},
            {'W', 0.0320367f}, {'.', 0.0316706f}, {'!', 0.0306942f}, {'Y', 0.0255073f}, {'*', 0.0241648f}, {'@', 0.0238597f}, {'V', 0.0235546f},
            {'-', 0.0197712f}, {'Z', 0.0170252f}, {'Q', 0.0147064f}, {'X', 0.0142182f}, {'_', 0.0122655f}, {'$', 0.00970255f}, {'#', 0.00854313f},
            {',', 0.00323418f}, {'/', 0.00311214f}, {'+', 0.00231885f}, {'?', 0.00207476f}, {';', 0.00207476f}, {'^', 0.00195272f},
            {' ', 0.00189169f},  {'%', 0.00170863f}, {'~', 0.00152556f}, {'=', 0.00140351f}, {'&', 0.00134249f}, {'`', 0.00115942f},
            {'\\', 0.00115942f}, {')', 0.00115942f}, {']', 0.0010984f}, {'[', 0.0010984f}, {':', 0.000549201f}, {'<', 0.000427156f},
            {'(', 0.000427156f}, {'æ', 0.000183067f}, {'>', 0.000183067f}, {'"', 0.000183067f}, {'ü', 0.000122045f}, {'|', 0.000122045f},
            {'{', 0.000122045f}, {'\'', 0.000122045f}, {'ö', 0.0000610223f}, {'ä', 0.0000610223f}, {'}', 0.0000610223f}
        };

        /*
        private readonly static Dictionary<char,float> EngCharFreq = new Dictionary<char, float>() {
            {'E', 12.7f}, {'T', 9.1f}, {'A', 8.2f}, {'O', 7.5f}, {'I', 7.0f},
            {'N', 6.7f}, {'S', 6.3f}, {'H', 6.1f}, {'R', 6.0f}, {'D', 4.3f},
            {'L', 4.0f}, {'C', 2.8f}, {'U', 2.8f}, {'M', 2.4f}, {'W', 2.4f}, 
            {'F', 2.2f}, {'G', 2.0f}, {'Y', 2.0f}, {'P', 1.9f}, {'B', 1.5f}, 
            {'V', 1.0f}, {'K', 0.8f}, {'J', 0.15f}, {'X', 0.15f}, {'Q', 0.10f}, {'Z', 0.07f}
        };
        */

        public static bool Run()
        {
            string hexString = @"1b37373331363f78151b7f2b783431333d78397828372d363c78373e783a393b3736";

            byte[] buf = Set01.Challenge01.ConvertHexStringToByteArray(hexString);
            Console.WriteLine("> Challenge string as text says: {0}", Encoding.Default.GetString(buf));

            List<CharacterFrequency> scoreBoard = new List<CharacterFrequency>();
            byte[] result;
            string tmp;
            float score;

            for (char key = ' '; key < '~'; key++)
            {
                result = SingleCharXOR(buf, key);
                tmp = Encoding.Default.GetString(result);
                score = ScoreTextByCharFreq(tmp.ToCharArray());
                //Console.WriteLine("> Result of XOR with '{0}' (score {1}) as text: {2}", key, score, tmp);
                scoreBoard.Add(new CharacterFrequency(key, score));
            }

            var finalScores = scoreBoard.OrderByDescending(x => x.Frequency).ToArray();

            char bestKey = finalScores[0].Character;
            float bestScore = finalScores[0].Frequency;

            result = SingleCharXOR(buf, bestKey);
            tmp = Encoding.Default.GetString(result);

            Console.WriteLine("> The key is '{0}' with the score '{1}' for the text: {2}", bestKey, bestScore, tmp);

            return true;
        }

        public struct CharacterFrequency
        {
            public char Character;
            public float Frequency;

            public CharacterFrequency(char c, float f)
            {
                this.Character = c;
                this.Frequency = f;
            }
        }


        public static float ScoreTextByCharFreq(char[] chars)
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();

            int len = chars.Length;
            char c;

            for (int i = 0; i < len; i++)
            {
                c = chars[i]; 
                if (charCount.ContainsKey(c)) charCount[c] += 1;
                else charCount.Add(c, 1);
            }

            float score = 0;
            float penalty = 0;
            foreach (var cx in charCount)
            {
                if (EnglishCharacterFreq.ContainsKey(cx.Key)) score += (EnglishCharacterFreq[cx.Key] + cx.Value) / 2;  // average the english char frequency with the frequncy in the sample
                else penalty += cx.Value;
            }

            return (penalty > score ? -1 : score);
        }


        public static byte[] SingleCharXOR(byte[] source, char c)
        {
            int len = source.Length;

            byte[] result = new byte[len];
            source.CopyTo(result, 0);

            byte x = Convert.ToByte(c);

            for (int i = 0; i < len; i++) {
                result[i] ^= x;
            }

            return result;
        }
    }
}
