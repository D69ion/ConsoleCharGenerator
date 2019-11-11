using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CharGenerator;
using System.Collections;

namespace ConsoleCharGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                //!(буква комманды) (путь файла) (имя выходного файла) (длина генерации)
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
                            ZipFile(com);
                            break;
                        }

                    case "!uz":
                        {
                            UnzipFile(com);
                            break;
                        }
                    //!comp (путь файла оригинального) (путь файла сжатого)
                    case "!comp":
                        {
                            CompareFiles(com);
                            break;
                        }
                }
                Console.WriteLine("Done");
            }
        }

        private static void UnzipFile(string[] args)
        {
            string zipFilePath = args[1];
            string unzipFilePath = Path.Combine(Path.GetDirectoryName(args[1]), args[2], ".txt");
            using (BinaryReader reader = new BinaryReader(File.OpenRead(zipFilePath)))
            {
                LZWzip zip = new LZWzip(reader, unzipFilePath);
                zip.Unzip();
            }
        }

        private static void ZipFile(string[] args)
        {
            string origFilePath = args[1];
            string zipFilePath = Path.Combine(Path.GetDirectoryName(args[1]), args[2], ".bin");
            using (StreamReader reader = new StreamReader(origFilePath))
            {
                LZWzip zip = new LZWzip(reader, zipFilePath);
                zip.Zip();
            }
        }

        private static void CompareFiles(string[] args)
        {
            FileInfo originalFile = new FileInfo(args[1]);
            FileInfo zippedFile = new FileInfo(args[2]);
            double res = 1 - ((double)zippedFile.Length / (double)originalFile.Length);
            Console.WriteLine("Origin file size: " + originalFile.Length);
            Console.WriteLine("Zipped file size: " + zippedFile.Length);
            Console.WriteLine("Compression ratio: {0:0.000}" + res * 100 + '%');
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

            Generator generator = new Generator(tuples, int.Parse(args[3]));
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
                    analyzer.Analyze();
                    writerAnal.Write(analyzer.ToString());
                }
            }
        }
    }
}
