using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace ConsoleCharGenerator
{
    class Program
    {
        private static Random random = new Random();
        static void Main(string[] args)
        {
            string path = "";
            //Tuple<char, double>[] tuple = new Tuple<char, double>[10];
            List<Tuple<char, double>> tuples = new List<Tuple<char, double>>();
            using (StreamReader reader = new StreamReader(path))
            {
                string[] temp;
                while (!reader.EndOfStream)
                {
                    temp = reader.ReadLine().Split(' ');
                    tuples.Add(new Tuple<char, double>(Convert.ToChar(temp[0]), Convert.ToDouble(temp[1])));
                }
            }
            tuples.TrimExcess();
            QSort(ref tuples, 0, tuples.Count - 1);

            StringBuilder result = new StringBuilder();
            for (int i = 0; i < random.Next(60, 200); i++)
            {
                double q = random.NextDouble();
                for (int j = 1; j < tuples.Count; j++)
                {
                    if (q > tuples[j - 1].Item2 && q < tuples[j].Item2)
                    {
                        result.Append(tuples[j].Item1);
                    }
                }
            }

            Console.WriteLine(result.ToString());
            Console.ReadLine();
        }

        private static void QSort(ref List<Tuple<char, double>> tuples, int i, int j)
        {
            if (i < j)
            {
                int q = Partition(ref tuples, i, j);
                QSort(ref tuples, i, q);
                QSort(ref tuples, q + 1, j);
            }
        }

        private static int Partition(ref List<Tuple<char, double>> tuples, int p, int r)
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

        private static double[] ToDoubleArray(string[] src)
        {
            double[] res = new double[src.Length];
            for (int i = 0; i < src.Length; i++)
            {
                res[i] = Convert.ToDouble(src[i]);
            }
            return res;
        }
    }
}
