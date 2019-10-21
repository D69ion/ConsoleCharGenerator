using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CharGenerator;

namespace ConsoleCharGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = ""; //добавить пути
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

            Generator generator = new Generator(tuples);
            string result = generator.CreateString();


            Console.WriteLine(result.ToString());
            using(StreamWriter writer=new StreamWriter(path))
            {
                writer.Write(result.ToString());
            }
            Console.ReadLine();
        }

    }
}
