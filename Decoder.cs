using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherThing
{
    internal class Decoder
    {
        private static readonly StringBuilder StringBuilder = new StringBuilder();

        private const string LetterFrequencies = "etaoinsrhdlucmfywgpbvkxqjz";

        private string _key;
        private string _input;
        private string _output;

        private readonly Dictionary<char, int> _frequencyMap = new Dictionary<char, int>();
        private readonly Dictionary<char, char> _charsMap = new Dictionary<char, char>();

        public void SetKey(string key)
        {
            if (key.Length != 26)
                Program.Error("Key length must be 26");

            _key = key;
        }

        public string GetKey()
        {
            return _key;
        }

        public void SetInput(string input)
        {
            _input = input;
        }

        public void SetOutput(string output)
        {
            _output = output;
        }

        public string GetOutput()
        {
            return _output;
        }

        public string DecodeText()
        {
            StringBuilder.Clear();

            SetFreqMap();
            SetCharsMap();

            foreach (char c in _input)
            {
                if (!char.IsLetter(c))
                {
                    StringBuilder.Append(c);
                    continue;
                }

                StringBuilder.Append(_charsMap[c]);
            }

            return StringBuilder.ToString();
        }

        private void SetFreqMap()
        {
            if (string.IsNullOrEmpty(_input))
                Program.Error("Decoder input is null or empty");

            foreach (char c in _input)
            {
                if (!char.IsLetter(c))
                    continue;

                if (_frequencyMap.ContainsKey(c))
                    _frequencyMap[c]++;
                else
                    _frequencyMap[c] = 1;
            }
        }

        private void SetCharsMap()
        {
            if (!string.IsNullOrEmpty(_key))
            {
                for (int j = 0; j < _key.Length; j++)
                    _charsMap.Add(Encoder.Letters[j], _key[j]);

                return;
            }

            StringBuilder.Clear();

            var reverseSorted = _frequencyMap.OrderByDescending(kvp => kvp.Key);

            int i = 0;
            foreach (var kvp in reverseSorted)
            {
                StringBuilder.Append(kvp.Key);
                _charsMap.Add(kvp.Key, Encoder.Letters[i]);
                i++;
            }

            SetKey(StringBuilder.ToString());
            StringBuilder.Clear();

            foreach (char c in Encoder.Letters)
            {
                if (_input.Contains(c))
                    continue;
                
                StringBuilder.Append(c);
                Program.Info($"{c} was not found in input, expect mistakes in output");
            }

            StringBuilder.Clear();
        }
    }
}
