﻿using System;
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
                Dictionary<string, int> dictionary = new Dictionary<string, int>();
                for (int i = 0; i < 256; i++)
                {
                    dictionary.Add(i.ToString(), i);
                }

                string w = string.Empty;
                List<int> compressed = new List<int>();

                foreach (var c in InputTextFile.ReadToEnd())
                {
                    string wc = w + c;
                    if (dictionary.ContainsKey(wc))
                    {
                        w = wc;
                    }
                    else
                    {
                        // write w to output
                        compressed.Add(dictionary[w]);
                        // wc is a new sequence; add it to the dictionary
                        dictionary.Add(wc, dictionary.Count);
                        w = c.ToString();
                    }
                }

                // write remaining output if necessary
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
                Dictionary<int, string> dictionary = new Dictionary<int, string>();
                for (int i = 0; i < 256; i++)
                {
                    dictionary.Add(i, i.ToString());
                }

                int k = InputBinFile.ReadInt32();
                string w = dictionary[k];
                StringBuilder decompressed = new StringBuilder(w);

                while(k > -1)
                {
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

                    // new sequence; add it to the dictionary
                    dictionary.Add(dictionary.Count, w + entry[0]);
                    w = entry;

                    k = InputBinFile.ReadInt32();
                }

                writer.Write(decompressed.ToString());
            }

        }
    }
}