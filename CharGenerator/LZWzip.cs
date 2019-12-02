using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharGenerator
{
    public class LZWzip
    {
        private StreamReader InputTextFile { get; set; }
        private BinaryReader InputBinFile { get; set; }
        private string Path { get; set; }

        public LZWzip(StreamReader input, string path)
        {
            InputTextFile = input;
            Path = path;
        }

        public LZWzip(BinaryReader input, string path)
        {
            InputBinFile = input;
            Path = path;
        }

        public void Zip()
        {
            using (BinaryWriter writer = new BinaryWriter(File.OpenWrite(Path)))
            {
                Dictionary<string, ushort> dictionary = new Dictionary<string, ushort>();
                for (ushort i = 0; i < 256; i++)
                {
                    dictionary.Add(((char)i).ToString(), i);
                }

                string w = string.Empty;
                List<ushort> compressed = new List<ushort>();

                foreach (var c in InputTextFile.ReadToEnd())
                {
                    string wc = w + c;
                    if (dictionary.ContainsKey(wc))
                    {
                        w = wc;
                    }
                    else
                    {
                        compressed.Add(dictionary[w]);
                        dictionary.Add(wc, (ushort)dictionary.Count);
                        w = c.ToString();
                    }
                }

                if (!string.IsNullOrEmpty(w))
                {
                    compressed.Add(dictionary[w]);
                }

                foreach (var item in compressed)
                {
                    writer.Write(item);
                }
            }

        }

        public void Unzip()
        {
            using (StreamWriter writer = new StreamWriter(Path))
            {
                Dictionary<ushort, string> dictionary = new Dictionary<ushort, string>();
                for (ushort i = 0; i < 256; i++)
                {
                    dictionary.Add(i, ((char)i).ToString());
                }

                ushort k = InputBinFile.ReadUInt16();
                string w = dictionary[k];
                StringBuilder decompressed = new StringBuilder(w);

                while(InputBinFile.BaseStream.Position != InputBinFile.BaseStream.Length)
                {
                    k = InputBinFile.ReadUInt16();
                    string entry = string.Empty;
                    if (dictionary.ContainsKey(k))
                    {
                        entry = dictionary[k];
                    }
                    else if (k == dictionary.Count)
                    {
                        entry = w + w[0];
                    }

                    decompressed.Append(entry);

                    dictionary.Add((ushort)dictionary.Count, w + entry[0]);
                    w = entry;

                    
                }

                writer.Write(decompressed.ToString());
            }

        }
    }
}
