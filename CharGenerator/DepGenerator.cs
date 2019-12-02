using System;
using System.Collections.Generic;
using System.Text;

namespace CharGenerator
{
    public class DepGenerator
    {
        private static Random random;
        private string Alphabet { get; set; }
        private double[,] Probabilities { get; set; }

        public DepGenerator(string chars, double[,] prob)
        {
            random = new Random();
            Alphabet = chars;
            Probabilities = prob;
        }

        public String CreateString(int length)
        {
            if (!CheckProbability())
            {
                return null;
            }

            StringBuilder builder = new StringBuilder();
            char previous = Alphabet[random.Next(Alphabet.Length)];
            double[,] ranges = CalcRanges();
            builder.Append(previous);

            for (int i = 0; i < length - 1; i++)
            {
                double q = random.NextDouble();
                if (q < ranges[Alphabet.IndexOf(previous), 0])
                {
                    builder.Append(Alphabet[0]);
                    previous = Alphabet[0];
                    continue;
                }
                if (q > ranges[Alphabet.IndexOf(previous), Alphabet.Length - 1])
                {
                    builder.Append(Alphabet[Alphabet.Length - 1]);
                    previous = Alphabet[Alphabet.Length - 1];
                    continue;
                }

                for (int j = 1; j < Alphabet.Length; j++)
                {
                    if (q >= ranges[Alphabet.IndexOf(previous), j - 1] && q <= ranges[Alphabet.IndexOf(previous), j])
                    {
                        builder.Append(Alphabet[j - 1]);
                        previous = Alphabet[j - 1];
                        break;
                    }
                }
            }

            return builder.ToString();
        }

        private double[,] CalcRanges()
        {
            double[,] vs = new double[Alphabet.Length, Alphabet.Length];
            for (int i = 0; i < Alphabet.Length; i++)
            {
                for (int j = 1; j < Alphabet.Length; j++)
                {
                    vs[i, j] = vs[i, j - 1] + Probabilities[i, j - 1];
                }
            }
            return vs;
        }

        private bool CheckProbability()
        {
            double sum = 0.0;
            const double EPS = 0.0000000000000000000000000001;
            for (int i = 0; i < Alphabet.Length; i++)
            {
                sum = 0.0;
                for (int j = 0; j < Alphabet.Length; j++)
                {
                    sum += Probabilities[i, j];
                }

                if (sum - 1 < EPS)
                {
                    continue;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
    }
}
