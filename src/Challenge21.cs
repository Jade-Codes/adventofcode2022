using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge21
    {
        private const string WORD_PATTERN = @"(\b[^\d\W]+\b)";
        private const string DIGIT_PATTERN = @"(\d+)";
        private const string SYMBOL_PATTERN = @"([*|+|\/|-])";

        public static void Part1(IEnumerable<string> lines)
        {
            var digitsDictionary = new Dictionary<string, long>();
            var lettersDictionary = new Dictionary<string, string>();

            Parse(lines, ref digitsDictionary, ref lettersDictionary);
            while (lettersDictionary.Count() > 0)
            {
                AddDigits(ref digitsDictionary, ref lettersDictionary);
            }
            Console.WriteLine(digitsDictionary["root"]);
        }

        public static void Part2(IEnumerable<string> lines, string rootKey = "root", string goalKey = "humn")
        {
            var digitsDictionary = new Dictionary<string, long>();
            var lettersDictionary = new Dictionary<string, string>();

            Parse(lines, ref digitsDictionary, ref lettersDictionary);

            lettersDictionary.Add("humn", "x");
            digitsDictionary.Remove("humn");

            for (var i = 0; i < digitsDictionary.Count(); i++)
            {
                AddDigits(ref digitsDictionary, ref lettersDictionary);
            }
            ReplaceLetters(digitsDictionary, ref lettersDictionary);
            CalculateYell(digitsDictionary, lettersDictionary, out var currentDigit, rootKey, goalKey);

            Console.WriteLine((long)currentDigit);
        }

        private static void Parse(IEnumerable<string> lines, ref Dictionary<string, long> digitsDictionary, ref Dictionary<string, string> lettersDictionary)
        {
            foreach (var line in lines)
            {
                var keyValue = line.Split(':');
                var digitsLetter = Regex.Match(keyValue[1], DIGIT_PATTERN).Value;

                if (long.TryParse(digitsLetter, out var digits))
                {
                    digitsDictionary.Add(keyValue[0], digits);
                }
                else
                {
                    lettersDictionary.Add(keyValue[0], keyValue[1]);
                }
            }
        }

        private static void AddDigits(ref Dictionary<string, long> digitsDictionary, ref Dictionary<string, string> lettersDictionary)
        {
            foreach (var letters in lettersDictionary)
            {
                var currentLetters = Regex.Matches(letters.Value, WORD_PATTERN).Select(_ => _.Value).ToList();
                var currentDigits = new List<long>();
                var keyList = new List<string>(digitsDictionary.Keys);

                foreach (var letter in new List<string>(currentLetters))
                {
                    if (keyList.Contains(letter))
                    {
                        currentLetters.Remove(letter);
                        currentDigits.Add(digitsDictionary[letter]);
                    }
                }

                if (!currentLetters.Any())
                {
                    var operation = Regex.Match(letters.Value, SYMBOL_PATTERN).Value;
                    var currentDigitsArray = currentDigits.ToArray();
                    long currentValue = 0;

                    switch (operation)
                    {
                        case "+":
                            currentValue = currentDigitsArray[0] + currentDigitsArray[1];
                            break;
                        case "*":
                            currentValue = currentDigitsArray[0] * currentDigitsArray[1];
                            break;
                        case "-":
                            currentValue = currentDigitsArray[0] - currentDigitsArray[1];
                            break;
                        case "/":
                            currentValue = currentDigitsArray[0] / currentDigitsArray[1];
                            break;
                        default:
                            throw new Exception($"Invalid operation when adding digit: {letters.Key} {letters.Value} {currentValue}");
                    }
                    digitsDictionary[letters.Key] = currentValue;
                    lettersDictionary.Remove(letters.Key);
                }
            }

        }

        private static void ReplaceLetters(Dictionary<string, long> digitsDictionary, ref Dictionary<string, string> lettersDictionary)
        {
            foreach (var letters in lettersDictionary)
            {
                var currentLetters = Regex.Matches(letters.Value, WORD_PATTERN).Select(_ => _.Value).ToList();

                foreach (var letter in currentLetters)
                {
                    if (digitsDictionary.TryGetValue(letter, out var value))
                    {
                        lettersDictionary[letters.Key] = letters.Value.Replace(letter, value.ToString());
                    }
                }
            }
        }

        private static void CalculateYell(Dictionary<string, long> digitsDictionary, Dictionary<string, string> lettersDictionary, out long currentDigit, string rootKey, string targetKey)
        {
            var notFound = true;
            var currentKey = rootKey;
            currentDigit = 0L;

            while (notFound)
            {
                var current = lettersDictionary[currentKey];
                if (currentKey == rootKey)
                {
                    currentDigit = long.Parse(Regex.Match(current, DIGIT_PATTERN).Value);
                    currentKey = Regex.Match(current, WORD_PATTERN).Value;
                    digitsDictionary[currentKey] = currentDigit;
                    continue;
                }
                else if (currentKey == targetKey)
                {
                    currentKey = Regex.Match(current, WORD_PATTERN).Value;
                    notFound = false;
                    continue;
                }

                var operation = Regex.Match(current, SYMBOL_PATTERN).Value;

                switch (operation)
                {
                    case "+":
                        var tempDigit = long.Parse(Regex.Match(current, DIGIT_PATTERN).Value);
                        currentKey = Regex.Match(current, WORD_PATTERN).Value;
                        currentDigit = currentDigit - tempDigit;
                        digitsDictionary[currentKey] = currentDigit;
                        break;
                    case "*":
                        tempDigit = long.Parse(Regex.Match(current, DIGIT_PATTERN).Value);
                        currentKey = Regex.Match(current, WORD_PATTERN).Value;
                        currentDigit = currentDigit / tempDigit;
                        digitsDictionary[currentKey] = currentDigit;
                        break;
                    case "/":
                        var values = current.Split('/');

                        if (long.TryParse(values[0], out var numerator))
                        {
                            currentKey = values[1].Trim();
                            currentDigit = numerator / currentDigit;
                            digitsDictionary[currentKey] = currentDigit;
                        }
                        else if (long.TryParse(values[1], out var denominator))
                        {
                            currentKey = values[0].Trim();
                            currentDigit = denominator * currentDigit;
                            digitsDictionary[currentKey] = currentDigit;
                        }
                        break;
                    case "-":
                        values = current.Split('-');
                        if (long.TryParse(values[0], out var value))
                        {
                            currentKey = values[1].Trim();
                            currentDigit = value - currentDigit;
                            digitsDictionary[currentKey] = currentDigit;
                        }
                        else if (long.TryParse(values[1], out value))
                        {
                            currentKey = values[0].Trim();
                            currentDigit = currentDigit + value;
                            digitsDictionary[currentKey] = currentDigit;
                        }
                        break;
                    default:
                        throw new Exception($"Invalid operation when calculating yell: {currentKey} {currentDigit} {current}");
                }
            }
        }
    }
}
