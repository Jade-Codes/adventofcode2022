using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge7
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var linesArray = lines.ToArray();
            var scores = new Dictionary<string, int>();
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
                    currentDirectory = linesArray[i] + i;
                    currentDirectories.Add(currentDirectory);
                }
                else if (linesArray[i].Contains("$ ls"))
                {
                    var filesExist = true;
                    var counter = i + 1;
                    while (filesExist)
                    {
                        if (counter < linesArray.Length)
                        {
                            var splitLine = linesArray[counter].Split(' ');
                            var isNumber = Int32.TryParse(splitLine[0], out var number);

                            if (isNumber)
                            {
                                foreach (var directory in currentDirectories)
                                {
                                    var valueExists = scores.TryGetValue(directory, out var value);
                                    value += number;

                                    if (valueExists)
                                    {
                                        scores.Remove(directory);
                                    }
                                    scores.Add(directory, value);
                                }
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
            }

            Console.WriteLine(scores.Where(_ => _.Value <= 100000).Sum(x => x.Value));
        }

        public static void Part2(IEnumerable<string> lines)
        {
            var linesArray = lines.ToArray();
            var scores = new Dictionary<string, int>();
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
                    currentDirectory = linesArray[i] + i;
                    currentDirectories.Add(currentDirectory);
                }
                else if (linesArray[i].Contains("$ ls"))
                {
                    var filesExist = true;
                    var counter = i + 1;
                    while (filesExist)
                    {
                        if (counter < linesArray.Length)
                        {
                            var splitLine = linesArray[counter].Split(' ');
                            var isNumber = Int32.TryParse(splitLine[0], out var number);

                            if (isNumber)
                            {
                                foreach (var directory in currentDirectories)
                                {
                                    var valueExists = scores.TryGetValue(directory, out var value);
                                    value += number;

                                    if (valueExists)
                                    {
                                        scores.Remove(directory);
                                    }
                                    scores.Add(directory, value);
                                }
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
            }

            var usedSpace = scores.Max(_ => _.Value);
            var spaceAvailable = 70000000 - usedSpace;
            var spaceNeeded = 30000000 - spaceAvailable;

            var directoriesToDelete = scores.Where(_ => _.Value >= spaceNeeded).OrderBy(_ => _.Value);

            Console.WriteLine(spaceNeeded);
            foreach(var directoryToDelete in directoriesToDelete)
                Console.WriteLine(directoryToDelete);
        }

    }
}