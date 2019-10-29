using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CharGenerator
{
    public class Analyzer
    {
        private Dictionary<char, double> Alphabet { get; set; }
        private string AnalyzedText { get; set; }
        private Dictionary<char, int> AllChars { get; set; } // словарь символов и их кол-во появлений
        private List<char> UniqueChars { get; set; } // словарь уникальных символов
        private Dictionary<char, double> AllCharOF { get; set; } // частность появления для каждого символа
        private int Size { get; set; } //общий размер текста
        private int? UniqueSize { get; set; } //кол-во уникальных символов
        private double UniqueOccurrenceFrequency { get; set; } // частость появления уникальных
        private double UnconditionalEntropy { get; set; } // безусловная энтропия (добавить условную?)

        public Analyzer(StreamReader reader, Dictionary<char, double> tuples)
        { 
            Alphabet = tuples;
            AnalyzedText = reader.ReadToEnd();
            Size = AnalyzedText.Length;
            AllChars = ParseText();
            FindUniqueChars();
            if(UniqueChars != null)
            {
                UniqueSize = UniqueChars.Count;
            }
            AllCharOF = new Dictionary<char, double>();
        }

        public string Analyze()
        {
            CalcOccurenceFrequencies();
            CalcUnconditionalEntropy();
            StringBuilder builder = new StringBuilder();
            builder.Append("Формат записи: символ, вероятность появления, частость появления" + Environment.NewLine);
            foreach (var pair in AllChars)
            {
                if (UniqueChars != null)
                {
                    if (UniqueChars.Contains(pair.Key))
                    {
                        continue;
                    }
                }
                builder.Append(pair.Key).Append(' ').Append(Alphabet[pair.Key]).Append(' ').Append(AllCharOF[pair.Key]).Append(Environment.NewLine);                
            }
            if (UniqueChars != null)
            {
                CalcUniqueOccurrenceFrequency();
                builder.Append(Environment.NewLine).Append("Для уникальных символов: ").Append(Environment.NewLine)
                    .Append("Кол-во уникальных символов: ").Append(UniqueSize).Append(Environment.NewLine).Append("Уникальные символы: ");
                foreach (var item in UniqueChars)
                {
                    builder.Append(item).Append(' ');
                }
                builder.Append(Environment.NewLine).Append("Частость появления: ").Append(UniqueOccurrenceFrequency).Append(Environment.NewLine);
            }
            builder.Append(Environment.NewLine).Append("Безусловная энтропия: ").Append(UnconditionalEntropy);

            return builder.ToString();
        }

        private void CalcOccurenceFrequencies()
        {
            foreach (var pair in AllChars)
            {
                if (pair.Value == 1)
                {
                    continue;
                }
                AllCharOF.Add(pair.Key, (double)pair.Value / (double)Size);
            }
        }

        private void FindUniqueChars()
        {
            foreach (var pair in AllChars)
            {
                if (pair.Value == 1)
                {
                    UniqueChars.Add(pair.Key);
                }
            }
        }

        private void CalcUnconditionalEntropy()
        {
            foreach (var pair in AllChars)
            {
                UnconditionalEntropy -= pair.Value * Math.Log(pair.Value, 2);
            }
        }

        private void CalcUniqueOccurrenceFrequency()
        {
            UniqueOccurrenceFrequency = (int)UniqueSize / Size;
        }

        private Dictionary<char, int> ParseText()
        {
            Dictionary<char, int> pairs = new Dictionary<char, int>();
            foreach (char item in AnalyzedText)
            {
                if (pairs.ContainsKey(item))
                {
                    pairs[item]++; 
                }
                else
                {
                    pairs.Add(item, 1);
                }
            }
            return pairs;
        }
    }
}
