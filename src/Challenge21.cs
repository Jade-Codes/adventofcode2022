using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge21
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var numberDictionary = new Dictionary<string, long>();
            var letterDictionary = new Dictionary<string, string>();

            Parse(lines, ref numberDictionary, ref letterDictionary);

            while(letterDictionary.Count > 0) 
            {
                foreach(var letters in letterDictionary) 
                {
                    var currentLetters = Regex.Matches(letters.Value, @"(\w+)").Select(_ => _.Value).ToList();
                    var currentDigits = new List<long>();
                    var keyList = new List<string>(numberDictionary.Keys);
                    var currentLettersEumerable = new List<string>(currentLetters).AsEnumerable();
                    foreach(var letter in currentLettersEumerable) 
                    {
                        if(keyList.Contains(letter))
                        {
                            currentLetters.Remove(letter);
                            currentDigits.Add(numberDictionary[letter]);
                        }
                    }

                    if (!currentLetters.Any())
                    {
                        var currentDigitsArray = currentDigits.ToArray();
                        long currentValue = 0;
                        if (letters.Value.Contains('+'))
                        {
                            currentValue = currentDigitsArray[0] + currentDigitsArray[1];
                        }
                        else if (letters.Value.Contains('*'))
                        {
                            currentValue = currentDigitsArray[0] * currentDigitsArray[1];
                        }
                        else if (letters.Value.Contains('-'))
                        {
                            currentValue = currentDigitsArray[0] - currentDigitsArray[1];
                        }
                        else if (letters.Value.Contains('/'))
                        {
                            currentValue = currentDigitsArray[0] / currentDigitsArray[1];
                        }
                        numberDictionary[letters.Key] = currentValue;
                        letterDictionary.Remove(letters.Key);
                    }
                }
            }

            Console.WriteLine(numberDictionary["root"]);
        }

        public static void Part2() 
        {
        }

        private static void Parse(IEnumerable<string> lines, ref Dictionary<string, long> numberDictionary, ref Dictionary<string, string> letterDictionary)
        {
            foreach (var line in lines)
            {
                var keyValue = line.Split(':');
                var digits = Regex.Match(keyValue[1], @"(\d+)").Value;

                if (long.TryParse(digits, out var number))
                {
                    numberDictionary.Add(keyValue[0], number);
                }
                else
                {
                    letterDictionary.Add(keyValue[0], keyValue[1]);
                }
            }
        }
    }
}
