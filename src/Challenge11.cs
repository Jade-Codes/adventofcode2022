using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge11
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var currentMonkey = 0;
            var linesArray = lines.ToArray();
            var items = new Dictionary<int, LinkedList<int>>();
            var operations = new Dictionary<int, string>();
            var testOperations = new Dictionary<int, int>();
            var trueConditions = new Dictionary<int, int>();
            var falseConditions = new Dictionary<int, int>();
            var tally = new Dictionary<int, int>();
            var rounds = 20;

            for (var i = 0; i < linesArray.Length; i++)
            {
                GetMonkey(linesArray[i], ref currentMonkey);
                GetStartingPoints(linesArray[i], currentMonkey, ref items);
                GetOperation(linesArray[i], currentMonkey, ref operations);
                GetTest(linesArray[i], currentMonkey, ref testOperations);
                GetTrueCondition(linesArray[i], currentMonkey, ref trueConditions);
                GetFalseCondition(linesArray[i], currentMonkey, ref falseConditions);
            }

            foreach (var item in items)
            {
                tally[item.Key] = 0;
            }

            for (var i = 0; i < rounds; i++)
            {
                foreach (var item in items)
                {
                    foreach (var itemValue in item.Value.ToList())
                    {
                        var operation = operations[item.Key].Replace("old", itemValue.ToString());
                        var testOperation = testOperations[item.Key];
                        var trueCondition = trueConditions[item.Key];
                        var falseCondition = falseConditions[item.Key];
                        var newValue = 0;

                        if (operation.Contains('*'))
                        {
                            var operationSplit = operation.Split('*').Select(Int32.Parse).ToArray();
                            newValue = operationSplit[0] * operationSplit[1];
                        }
                        else if (operation.Contains('+'))
                        {
                            var operationSplit = operation.Split('+').Select(Int32.Parse).ToArray();
                            newValue = operationSplit[0] + operationSplit[1];
                        }

                        newValue = newValue / 3;

                        if (newValue % testOperation == 0)
                        {
                            items[item.Key].Remove(itemValue);
                            items[trueCondition].AddLast(newValue);
                            tally[item.Key]++;
                        }
                        else
                        {
                            items[item.Key].Remove(itemValue);
                            items[falseCondition].AddLast(newValue);
                            tally[item.Key]++;
                        }
                    }
                }
            }

            var orderTally = tally.OrderByDescending(_ => _.Value).Take(2);

            Console.WriteLine(orderTally.First().Value * orderTally.Last().Value);

        }

        public static void Part2(IEnumerable<string> lines)
        {
            var currentMonkey = 0;
            var linesArray = lines.ToArray();
            var items = new Dictionary<int, LinkedList<int>>();
            var operations = new Dictionary<int, string>();
            var testOperations = new Dictionary<int, int>();
            var trueConditions = new Dictionary<int, int>();
            var falseConditions = new Dictionary<int, int>();
            var tally = new Dictionary<int, int>();
            var rounds = 20;

            for (var i = 0; i < linesArray.Length; i++)
            {
                GetMonkey(linesArray[i], ref currentMonkey);
                GetStartingPoints(linesArray[i], currentMonkey, ref items);
                GetOperation(linesArray[i], currentMonkey, ref operations);
                GetTest(linesArray[i], currentMonkey, ref testOperations);
                GetTrueCondition(linesArray[i], currentMonkey, ref trueConditions);
                GetFalseCondition(linesArray[i], currentMonkey, ref falseConditions);
            }

            foreach (var item in items)
            {
                tally[item.Key] = 0;
            }

            for (var i = 0; i < rounds; i++)
            {
                foreach (var item in items)
                {
                    foreach (var itemValue in item.Value.ToList())
                    {
                        var operation = operations[item.Key].Replace("old", itemValue.ToString());
                        var trueCondition = trueConditions[item.Key];
                        var falseCondition = falseConditions[item.Key];

                        CalculateRemainder(operation, testOperations, out var remainders);
 
                        if (remainders[item.Key] == 0)
                        {
                            items[trueCondition].AddLast(remainders[trueCondition]);
                        }
                        else
                        {
                            items[falseCondition].AddLast(remainders[falseCondition]);
                        }

                        tally[item.Key]++;
                        items[item.Key].RemoveFirst();
                    }
                }
            }


            var orderTally = tally.OrderByDescending(_ => _.Value).Take(2);

            Console.WriteLine(orderTally.First().Value * orderTally.Last().Value);

        }

        public static void GetMonkey(string currentLine, ref int currentMonkey)
        {
            if (!currentLine.StartsWith("Monkey")) return;
            var monkey = Regex.Match(currentLine, @"\d+").Value;
            currentMonkey = Int32.Parse(monkey);
        }

        public static void GetStartingPoints(string currentLine, int currentMonkey, ref Dictionary<int, LinkedList<int>> startingItems)
        {
            if (!currentLine.Contains("Starting items")) return;
            var currentLineSplit = currentLine.Split(':');
            var numbers = new LinkedList<int>(currentLineSplit[1].Split(',').Select(Int32.Parse));
            startingItems.Add(currentMonkey, numbers);
        }

        public static void GetOperation(string currentLine, int currentMonkey, ref Dictionary<int, string> operations)
        {
            if (!currentLine.Contains("Operation")) return;
            var currentLineSplit = currentLine.Split(':');
            var condtions = currentLineSplit[1].Split('=').ToArray();
            operations.Add(currentMonkey, condtions[1]);

        }

        public static void GetTest(string currentLine, int currentMonkey, ref Dictionary<int, int> testOperation)
        {
            if (!currentLine.Contains("Test")) return;
            var divisible = Int32.Parse(Regex.Match(currentLine, @"\d+").Value);
            testOperation.Add(currentMonkey, divisible);
        }

        public static void GetTrueCondition(string currentLine, int currentMonkey, ref Dictionary<int, int> trueCondition)
        {
            if (!currentLine.Contains("If true")) return;
            var nextMonkey = Int32.Parse(Regex.Match(currentLine, @"\d+").Value);
            trueCondition.Add(currentMonkey, nextMonkey);
        }

        public static void GetFalseCondition(string currentLine, int currentMonkey, ref Dictionary<int, int> falseCondition)
        {
            if (!currentLine.Contains("If false")) return;
            var nextMonkey = Int32.Parse(Regex.Match(currentLine, @"\d+").Value);
            falseCondition.Add(currentMonkey, nextMonkey);
        }

        private static void CalculateRemainder(string operation, Dictionary<int, int> testOperations, out int[] remainders)
        {
            remainders = new int[testOperations.Count()];

            for (var i = 0; i < testOperations.Count(); i++)
            {
                if (operation.Contains('*'))
                {
                    var operationSplit = operation.Split('*').Select(Int32.Parse).ToArray();
                    var value = (operationSplit[0] * operationSplit[1]);
                    remainders[i] = value % testOperations[i];
                }
                else if (operation.Contains('+'))
                {
                    var operationSplit = operation.Split('+').Select(Int32.Parse).ToArray();
                    var value = (operationSplit[0] + operationSplit[1]);
                    remainders[i] = value % testOperations[i];
                }
                else
                {
                    Console.WriteLine();
                }
            }

        }

    }
}
