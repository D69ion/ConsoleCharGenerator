using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharGenerator
{
    public class Generator
    {
        private Random random;
        private List<Tuple<char, double>> Alphabet { get; set; }
        private int Length { get; set; }

        public Generator(List<Tuple<char, double>> tuples, int length)
        {
            Alphabet = tuples;
            random = new Random();
            Length = length;
        }

        public string CreateString()
        {
            if (!CheckProbability())
            {
                return null;
            }

            QSort(Alphabet, 0, Alphabet.Count - 1);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < Length; i++)
            {
                double q = random.NextDouble();

                if (q > Alphabet[Alphabet.Count - 1].Item2)
                {
                    result.Append(Alphabet[Alphabet.Count - 1].Item1);
                    continue;
                }
                if (q < Alphabet[0].Item2)
                {
                    result.Append(Alphabet[0].Item1);
                    continue;
                }

                for (int j = 1; j < Alphabet.Count; j++)
                {
                    if (q > Alphabet[j - 1].Item2 && q < Alphabet[j].Item2)
                    {
                        result.Append(Alphabet[j].Item1);
                        continue;
                    }
                }
            }
            return result.ToString();
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

        private void QSort(List<Tuple<char, double>> tuples, int i, int j)
        {
            if (i < j)
            {
                int q = Partition(ref tuples, i, j);
                QSort(tuples, i, q);
                QSort(tuples, q + 1, j);
            }
        }

        private int Partition(ref List<Tuple<char, double>> tuples, int p, int r)
        {
            double x = tuples[p].Item2;
            int i = p - 1;
            int j = r + 1;
            while (true)
            {
                do
                {
                    j--;
                }
                while (tuples[j].Item2 > x);
                do
                {
                    i++;
                }
                while (tuples[i].Item2 < x);
                if (i < j)
                {
                    (tuples[i], tuples[j]) = (tuples[j], tuples[i]);
                }
                else
                {
                    return j;
                }
            }
        }

    }
}
