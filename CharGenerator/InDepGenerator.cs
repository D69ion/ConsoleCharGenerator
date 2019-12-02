using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharGenerator
{
    public class InDepGenerator
    {
        private static Random random;
        private List<Tuple<char, double>> Alphabet { get; set; }

        public InDepGenerator(List<Tuple<char, double>> tuples)
        {
            Alphabet = tuples;
            random = new Random();
        }

        public string CreateString(int length)
        {
            if (!CheckProbability())
            {
                return null;
            }

            double[] ranges = CalcRanges();

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                double q = random.NextDouble();

                if (q > ranges[Alphabet.Count - 1])
                {
                    result.Append(Alphabet[Alphabet.Count - 1].Item1);
                    continue;
                }
                if (q < ranges[0])
                {
                    result.Append(Alphabet[0].Item1);
                    continue;
                }

                for (int j = 1; j < Alphabet.Count; j++)
                {
                    if (q > ranges[j - 1] && q < ranges[j])
                    {
                        result.Append(Alphabet[j].Item1);
                        break;

                    }
                }
            }
            return result.ToString();
        }

        private double[] CalcRanges()
        {
            double[] vs = new double[Alphabet.Count];
            for (int i = 1; i < Alphabet.Count; i++)
            {
                vs[i] = vs[i - 1] + Alphabet[i - 1].Item2;
            }
            return vs;
        }

        public Dictionary<char, double> GetDictionary()
        {
            Dictionary<char, double> pairs = new Dictionary<char, double>();
            foreach (var item in Alphabet)
            {
                pairs.Add(item.Item1, item.Item2);
            }
            return pairs;
        }

        private bool CheckProbability()
        {
            double sum = 0.0;
            const double EPS = 0.0000000000000000000000000001;
            foreach (var pair in Alphabet)
            {
                sum += pair.Item2;
            }
            return sum - 1 < EPS;
        }
    }
}
