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
        static Random random = new Random();
        static void Main(string[] args)
        {
            string path = "";
            char[] alphabet;
            double[] probabilities;
            using (StreamReader reader = new StreamReader(path))
            {
                alphabet = reader.ReadLine().Remove(' ').ToCharArray();
                probabilities = ToDoubleArray(reader.ReadLine().Split(' '));
            }



            //SortedList<char, double> valuePairs = new SortedList<char, double>();//попытка пофиксить неупорядоченность, но вероятности у символов могут быть одинаковыми
            //for (int i = 0; i < alphabet.Length; i++)
            //{
            //    valuePairs.Add(alphabet[i], probabilities[i]);
            //}



            StringBuilder result = new StringBuilder();
            for (int i = 0; i < 100; i++)
            {
                double q = random.NextDouble();
                for (int j = 1; j < alphabet.Length; j++)
                {
                    if (q > probabilities[j - 1] && q < probabilities[j])//не факт что оно упорядочено
                    {
                        result.Append(alphabet[j]);
                    }
                }
            }

            Console.WriteLine(result.ToString());
            Console.ReadLine();
        }

        static double[] ToDoubleArray(string[] src)
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
