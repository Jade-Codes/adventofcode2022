using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge7
    {
        public static void Part1(IEnumerable<string> lines)
        {
            CalculateTotals(lines, out var totals);
            Console.WriteLine(totals.Where(_ => _.Value <= 100000).Sum(x => x.Value));
        }

        public static void Part2(IEnumerable<string> lines, int totalSpace = 70000000, int totalSpaceNeeded = 30000000)
        {
            CalculateTotals(lines, out var totals);
            CalculateSpaceNeeded(totals, totalSpace, totalSpaceNeeded, out var spaceNeeded);
            var directoryToDelete = totals.Where(_ => _.Value >= spaceNeeded).OrderBy(_ => _.Value).First();
            Console.WriteLine(directoryToDelete.Value);
        }

        private static void CalculateTotals(IEnumerable<string> lines, out Dictionary<string, int> totals)
        {
            totals = new Dictionary<string, int>();
            var linesArray = lines.ToArray();
            var currentDirectories = new List<string>();
            var currentDirectory = string.Empty;

            for (int i = 0; i < linesArray.Length; i++)
            {
                if (linesArray[i].Contains("$ cd .."))
                {
                    currentDirectories.RemoveAt(currentDirectories.Count - 1);
                }
                else if (linesArray[i].Contains("$ cd "))
                {
                    currentDirectory = linesArray[i] + '-' + i;
                    currentDirectories.Add(currentDirectory);
                }
                else if (linesArray[i].Contains("$ ls"))
                {
                    CalculateTotals(linesArray, currentDirectories, i, ref totals);
                }
            }
        }

        private static void CalculateTotals(string[] linesArray, IEnumerable<string> directories, int index, ref Dictionary<string, int> totals)
        {
            var filesExist = true;
            var counter = index + 1;
            while (filesExist)
            {
                if (counter < linesArray.Length)
                {
                    var splitLine = linesArray[counter].Split(' ');
                    var isNumber = Int32.TryParse(splitLine[0], out var number);

                    if (isNumber)
                    {
                        UpdateTotals(directories, number, ref totals);
                    }
                    else if (splitLine[0] != "dir")
                    {
                        filesExist = false;
                    }

                    counter++;
                }
                else
                {
                    filesExist = false;
                }
            }
        }

        private static void UpdateTotals(IEnumerable<string> directories, int number, ref Dictionary<string, int> totals)
        {
            foreach (var directory in directories)
            {
                UpdateTotal(directory, number, ref totals);
            }
        }

        private static void UpdateTotal(string directory, int number, ref Dictionary<string, int> totals)
        {
            var valueExists = totals.TryGetValue(directory, out var value);
            value += number;

            if (valueExists)
            {
                totals.Remove(directory);
            }
            totals.Add(directory, value);
        }

        public static void CalculateSpaceNeeded(Dictionary<string, int> totals, int totalSpace, int totalSpaceNeeded, out int spaceNeeded)
        {
            var usedSpace = totals.Max(_ => _.Value);
            var spaceAvailable = totalSpace - usedSpace;
            spaceNeeded = totalSpaceNeeded - spaceAvailable;
        }
    }
}