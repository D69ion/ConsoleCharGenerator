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
            while (true)
            {
                //!(буква комманды) (путь файла) (имя выходного файла)
                string[] com = Console.ReadLine().Split(' ');
                switch (com[0])
                {
                    case "!g":
                        {
                            GenerateCom(com);
                            break;
                        }

                    case "!z":
                        {

                            break;
                        }                           
                }
                Console.WriteLine("Done");
            }
        }

        private static Random random = new Random();
        private static void GenerateCom(string[] args)
        {
            string resFileName = string.Concat(args[2], ".txt"), 
                analFileName = string.Concat(args[2], '_', random.Next(), ".txt");
            string path = Path.GetDirectoryName(args[1]); 
            List<Tuple<char, double>> tuples = new List<Tuple<char, double>>();
            using (StreamReader reader = new StreamReader(args[1]))
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

            //запись и анализ
            using (StreamWriter writerRes = new StreamWriter(Path.Combine(path, resFileName)))
            {
                writerRes.Write(result);
                //Console.WriteLine(result);               
            }
            using (StreamWriter writerAnal = new StreamWriter(Path.Combine(path, analFileName)))
            {
                using (StreamReader reader = new StreamReader(Path.Combine(path, resFileName)))
                {
                    Analyzer analyzer = new Analyzer(reader, generator.GetDictionary());
                    writerAnal.Write(analyzer.Analyze());
                }
            }
        }
    }
}
