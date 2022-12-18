using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge17
    {
        public static void Part1(string input, long totalRocks = 2022, int columns = 7)
        {
            var jetPattern = input.ToCharArray();
            var currentJetPattern = 0;
            long currentRock = 0;
            var currentRockType = 0;
            long currentRockColumn = 2;
            long currentRockRow = 4;

            var cache = new HashSet<(long x, long y)>()
            {
                (0,0),
                (1,0),
                (2,0),
                (3,0),
                (4,0),
                (5,0),
                (6,0)
            };

            while (currentRock < totalRocks)
            {
                if (currentJetPattern >= jetPattern.Length)
                {
                    currentJetPattern = 0;
                }

                var rock = GenerateRock(currentRockType, currentRockRow, currentRockColumn);

                if (jetPattern[currentJetPattern] == '<' && currentRockColumn != 0 && rock.Max(_ => _.x) != 0)
                {
                    var newRockColumn = currentRockColumn - 1;
                    currentRockColumn = Move(cache, currentRockType, currentRockRow, newRockColumn, ref rock) ? newRockColumn : currentRockColumn;
                }
                else if (jetPattern[currentJetPattern] == '>' && currentRockColumn != columns - 1 && rock.Max(_ => _.x) != columns - 1)
                {
                    var newRockColumn = currentRockColumn + 1;
                    currentRockColumn = Move(cache, currentRockType, currentRockRow, newRockColumn, ref rock) ? newRockColumn : currentRockColumn;
                }

                var newRockRow = currentRockRow - 1;
                var canMove = Move(cache, currentRockType, newRockRow, currentRockColumn, ref rock);
                currentRockRow = canMove ? newRockRow : currentRockRow;

                if (!canMove)
                {
                    cache.UnionWith(rock);
                    currentRock++;
                    currentRockType = currentRockType < 4 ? currentRockType + 1 : 0;
                    currentRockRow = cache.Max(_ => _.y) + 4;
                    currentRockColumn = 2;
                }

                currentJetPattern++;
            }

            Console.WriteLine(cache.Max(_ => _.y));
        }

        public static void Part2(string input, long totalRocks = 1000000000000, int columns = 7)
        {
            var jetPattern = input.ToCharArray();
            var currentJetPattern = 0;
            long currentRock = 0;
            var currentRockType = 0;
            long currentRockColumn = 2;
            long currentRockRow = 4;

            var fallenRocks = new HashSet<(long x, long y)>()
            {
                (0,0),
                (1,0),
                (2,0),
                (3,0),
                (4,0),
                (5,0),
                (6,0)
            };

            var jetCache = new Dictionary<(long, string), (long rocks, long rows)>();

            while (currentRock < totalRocks)
            {
                if (currentJetPattern >= jetPattern.Length)
                {
                    currentJetPattern = 0;
                }

                var rock = GenerateRock(currentRockType, currentRockRow, currentRockColumn);

                if (jetPattern[currentJetPattern] == '<' && currentRockColumn != 0 && rock.Max(_ => _.x) != 0)
                {
                    var newRockColumn = currentRockColumn - 1;
                    currentRockColumn = Move(fallenRocks, currentRockType, currentRockRow, newRockColumn, ref rock) ? newRockColumn : currentRockColumn;
                }
                else if (jetPattern[currentJetPattern] == '>' && currentRockColumn != columns - 1 && rock.Max(_ => _.x) != columns - 1)
                {
                    var newRockColumn = currentRockColumn + 1;
                    currentRockColumn = Move(fallenRocks, currentRockType, currentRockRow, newRockColumn, ref rock) ? newRockColumn : currentRockColumn;
                }

                var newRockRow = currentRockRow - 1;
                var canMove = Move(fallenRocks, currentRockType, newRockRow, currentRockColumn, ref rock);
                currentRockRow = canMove ? newRockRow : currentRockRow;

                if (!canMove)
                {
                    var values = "";

                    fallenRocks.UnionWith(rock);

                    var maxRow = fallenRocks.Max(_ => _.y);

                    foreach (var pattern in fallenRocks.OrderByDescending(_ => _.y).Take(50))
                    {
                        values += (pattern.x, pattern.y - currentRockRow);
                    }

                    currentRock++;

                    if (jetCache.TryGetValue((currentJetPattern, values), out var value))
                    {
                        var distanceBetweenRocks = currentRock-value.rocks;
                        var distanceBetweenRows = maxRow - value.rows;


                        var modulus = ((totalRocks - value.rocks) % distanceBetweenRocks);

                        var value2 = (totalRocks - value.rocks) / distanceBetweenRocks;

                        var index = jetCache.Values.ToList().IndexOf(value);
                        var nextValue = jetCache.Values.ToList().ElementAt(index + (int)modulus);
                        var nextValueRow = nextValue.rows - value.rows;

                        var rocksd = ((totalRocks-value.rocks) / distanceBetweenRocks);

                        var calculate =  rocksd * distanceBetweenRows + value.rows + nextValueRow;

                        Console.WriteLine(calculate);
                        return;
                    }

                    jetCache[(currentJetPattern, values)] = (currentRock, maxRow);

                    currentRockType = currentRockType < 4 ? currentRockType + 1 : 0;
                    currentRockRow = maxRow + 4;
                    currentRockColumn = 2;

                    if (currentRock % 10000 == 0) Console.WriteLine(currentRock);
                }

                currentJetPattern++;
            }

            Console.WriteLine(fallenRocks.Max(_ => _.y));
        }

        public static HashSet<(long x, long y)> GenerateRock(int rockType, long currentRow, long currentColumn)
        {
            var rock = new HashSet<(long x, long y)>();

            switch (rockType)
            {
                case 0:
                    rock.Add((currentColumn, currentRow));
                    rock.Add((currentColumn + 1, currentRow));
                    rock.Add((currentColumn + 2, currentRow));
                    rock.Add((currentColumn + 3, currentRow));
                    break;
                case 1:
                    rock.Add((currentColumn, currentRow + 1));
                    rock.Add((currentColumn + 1, currentRow + 1));
                    rock.Add((currentColumn + 1, currentRow));
                    rock.Add((currentColumn + 1, currentRow + 2));
                    rock.Add((currentColumn + 2, currentRow + 1));
                    break;
                case 2:
                    rock.Add((currentColumn, currentRow));
                    rock.Add((currentColumn + 1, currentRow));
                    rock.Add((currentColumn + 2, currentRow));
                    rock.Add((currentColumn + 2, currentRow + 1));
                    rock.Add((currentColumn + 2, currentRow + 2));
                    break;
                case 3:
                    rock.Add((currentColumn, currentRow));
                    rock.Add((currentColumn, currentRow + 1));
                    rock.Add((currentColumn, currentRow + 2));
                    rock.Add((currentColumn, currentRow + 3));
                    break;
                case 4:
                    rock.Add((currentColumn, currentRow));
                    rock.Add((currentColumn + 1, currentRow));
                    rock.Add((currentColumn + 1, currentRow + 1));
                    rock.Add((currentColumn, currentRow + 1));
                    break;
                default:
                    // code block
                    break;
            }

            return rock;
        }

        public static bool Move(HashSet<(long x, long y)> cache, int rockType, long row, long column, ref HashSet<(long x, long y)> rock)
        {
            var newRock = GenerateRock(rockType, row, column);
            if (!cache.Intersect(newRock).Any())
            {
                rock = newRock;
                return true;
            }
            return false;
        }
    }
}