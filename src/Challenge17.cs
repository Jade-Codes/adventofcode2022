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
                else if (jetPattern[currentJetPattern] == '>' && currentRockColumn != columns-1 && rock.Max(_ => _.x) != columns -1)
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
                    currentRockType = currentRockType < 4 ? currentRockType+1 : 0;
                    currentRockRow = cache.Max(_ => _.y) + 4;
                    currentRockColumn = 2;
                }

                currentJetPattern++;
            }

            Console.WriteLine(cache.Max(_=> _.y));
        }

        public static void Part2(string input)
        {
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
            if(!cache.Intersect(newRock).Any())
            {
                rock = newRock;
                return true;
            }
            return false;
        }
    }

    public class Rock
    {
    }
}