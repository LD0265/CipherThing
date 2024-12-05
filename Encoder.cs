using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CipherThing
{
    internal class Encoder
    {
        private static readonly Random Random = new Random();

        public const string Letters = "abcdefghijklmnopqrstuvwxyz";

        private string _key;
        private string _input;
        private string _output;

        private readonly Dictionary<char, char> _charsMap = new Dictionary<char, char>();

        public Encoder()
        {
            foreach (char c in Letters)
                _charsMap.Add(c, ' ');

            GenerateRandomKey();
            SetMap();
        }

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

        public void SetOutput(string outputDir)
        {
            _output = outputDir;
        }

        public string GetOutput()
        {
            return _output;
        }

        public void SetMap()
        {
            for (int i = 0; i < _key.Length; i++)
                _charsMap[Letters[i]] = _key[i];
        }

        public void GenerateRandomKey()
        {
            char[] letters = Letters.ToCharArray();
            string key = string.Join("", ShuffleArray(letters));

            SetKey(key);
        }

        public string EncodeText()
        {
            if (string.IsNullOrEmpty(_input))
                Program.Error("Encoder input is null or empty");

            StringBuilder stringBuilder = new StringBuilder();

            _input = _input.ToLower();

            foreach (char c in _input)
            {
                if (!char.IsLetter(c))
                {
                    stringBuilder.Append(c);
                    continue;
                }

                stringBuilder.Append(_charsMap[c]);
            }

            return stringBuilder.ToString();
        }

        private static T[] ShuffleArray<T>(T[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                int n = Random.Next(array.Length);
                (array[i], array[n]) = (array[n], array[i]);
            }

            return array;
        }
    }
}