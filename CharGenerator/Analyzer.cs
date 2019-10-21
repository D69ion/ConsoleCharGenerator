using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CharGenerator
{
    public class Analyzer
    {
        private List<Tuple<char, double>> Alphabet { get; set; }
        private string AnalyzedText { get; set; }
        private Dictionary<char, int> AllChars { get; set; } // словарь символов и их кол-во появлений
        private List<char> UniqueChars { get; set; } // словарь уникальных символов
        private Dictionary<char, double> AllCharOF { get; set; } // частность появления для каждого символа
        private int Size { get; set; } //общий размер
        private int UniqueSize { get; set; } //кол-во уникальных символов
        private double UniqueOccurrenceFrequency { get; set; } // частость появления уникальных
        private double UnconditionalEntropy { get; set; } // безусловная энтропия (добавить условную?)

        public Analyzer(string text, List<Tuple<char, double>> tuples)
        {
            Alphabet = tuples;
            AnalyzedText = text;
            Size = AnalyzedText.Length;
            AllChars = ParseText();
            FindUniqueChars();
            UniqueSize = UniqueChars.Count;
        }

        public string Analyze()
        {
            CalcOccurenceFrequencies();
            CalcUniqueOccurrenceFrequency();
            CalcUnconditionalEntropy();
            StringBuilder builder = new StringBuilder();
            builder.Append("Формат записи: символ, вероятность появления, частость появления" + Environment.NewLine);
            foreach (var pair in Alphabet)
            {
                builder.Append(pair.Item1).Append(' ').Append(pair.Item2).Append(' ').Append()                
            }
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
                AllCharOF.Add(pair.Key, pair.Value / Size);
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
            UniqueOccurrenceFrequency = UniqueSize / Size;
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
