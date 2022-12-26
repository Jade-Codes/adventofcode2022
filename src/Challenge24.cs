using System.Linq;
using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge24
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var wallCoordinates = new HashSet<(int x, int y)>();
            var currentBlizzardCoordinates = new Dictionary<int, HashSet<(int x, int y, char direction)>>();
            var currentStandingCoordinates = new Dictionary<int, HashSet<(int x, int y)>>();
            var minute = 0;

            Parse(lines, ref wallCoordinates, ref currentBlizzardCoordinates, ref currentStandingCoordinates);

            Move(ref minute, wallCoordinates, ref currentBlizzardCoordinates, ref currentStandingCoordinates, 'v');

            Console.WriteLine(minute);
        }

        public static void Part2(IEnumerable<string> lines)
        {
            var wallCoordinates = new HashSet<(int x, int y)>();
            var currentBlizzardCoordinates = new Dictionary<int, HashSet<(int x, int y, char direction)>>();
            var currentStandingCoordinates = new Dictionary<int, HashSet<(int x, int y)>>();
            var minute = 0;

            Parse(lines, ref wallCoordinates, ref currentBlizzardCoordinates, ref currentStandingCoordinates);

            Move(ref minute, wallCoordinates, ref currentBlizzardCoordinates, ref currentStandingCoordinates, 'v');

            Move(ref minute, wallCoordinates, ref currentBlizzardCoordinates, ref currentStandingCoordinates, '^');

            Move(ref minute, wallCoordinates, ref currentBlizzardCoordinates, ref currentStandingCoordinates, 'v');

            Console.WriteLine(minute);
        }

        private static void Move(ref int minute,
            HashSet<(int x, int y)> wallCoordinates,
            ref Dictionary<int, HashSet<(int x, int y, char direction)>> currentBlizzardCoordinates,
            ref Dictionary<int, HashSet<(int x, int y)>> currentStandingCoordinates,
            char direction)
        {
            var reached = false;
            while (!reached)
            {
                var nextBlizzardCoordinates = new HashSet<(int x, int y, char direction)>();
                var nextStandingCoordinates = new HashSet<(int x, int y)>();
                foreach (var blizzardCoordinates in currentBlizzardCoordinates[minute])
                {
                    (int x, int y, char direction) newCoordinates;

                    switch (blizzardCoordinates.direction)
                    {
                        case 'v':
                            newCoordinates = (blizzardCoordinates.x, blizzardCoordinates.y + 1, blizzardCoordinates.direction);
                            if (wallCoordinates.Contains((newCoordinates.x, newCoordinates.y)))
                            {
                                newCoordinates.y = wallCoordinates.Min(_ => _.y) + 1;
                            }

                            nextBlizzardCoordinates.Add(newCoordinates);
                            break;
                        case '>':
                            newCoordinates = (blizzardCoordinates.x + 1, blizzardCoordinates.y, blizzardCoordinates.direction);
                            if (wallCoordinates.Contains((newCoordinates.x, newCoordinates.y)))
                            {
                                newCoordinates.x = wallCoordinates.Min(_ => _.x) + 1;
                            }

                            nextBlizzardCoordinates.Add(newCoordinates);
                            break;
                        case '<':
                            newCoordinates = (blizzardCoordinates.x - 1, blizzardCoordinates.y, blizzardCoordinates.direction);
                            if (wallCoordinates.Contains((newCoordinates.x, newCoordinates.y)))
                            {
                                newCoordinates.x = wallCoordinates.Max(_ => _.x) - 1;
                            }

                            nextBlizzardCoordinates.Add(newCoordinates);
                            break;
                        case '^':
                            newCoordinates = (blizzardCoordinates.x, blizzardCoordinates.y - 1, blizzardCoordinates.direction);
                            if (wallCoordinates.Contains((newCoordinates.x, newCoordinates.y)))
                            {
                                newCoordinates.y = wallCoordinates.Max(_ => _.y) - 1;
                            }

                            nextBlizzardCoordinates.Add(newCoordinates);
                            break;
                    }
                }

                var currentBlizzardCoordinatee = currentStandingCoordinates[minute];

                foreach (var currentStandingCoordinate in currentBlizzardCoordinatee)
                {
                    var blizzardCoordinates = nextBlizzardCoordinates.Select(_ => new { _.x, _.y }).AsEnumerable().Select(_ => (_.x, _.y));
                    var elf = new Elf(currentStandingCoordinate.x, currentStandingCoordinate.y);

                    var availableCoordinates = elf.AllDirections.Except(blizzardCoordinates).Except(wallCoordinates).Where(_ => _.y <= wallCoordinates.Max(_ => _.y) && _.y >= wallCoordinates.Min(_ => _.y));

                    nextStandingCoordinates.UnionWith(availableCoordinates);
                    if (availableCoordinates.Any() && direction == 'v' && availableCoordinates.Max(_ => _.y) == wallCoordinates.Max(_ => _.y))
                    {
                        var availableCoordinate = availableCoordinates.OrderByDescending(_ => _.y).First();
                        nextStandingCoordinates = new HashSet<(int x, int y)>();
                        nextStandingCoordinates.Add(availableCoordinate);
                        minute++;
                        currentBlizzardCoordinates.Add(minute, nextBlizzardCoordinates);
                        currentStandingCoordinates.Add(minute, nextStandingCoordinates);
                        return;
                    }
                    if (availableCoordinates.Any() && direction == '^' && availableCoordinates.Min(_ => _.y) == wallCoordinates.Min(_ => _.y))
                    {
                        var availableCoordinate = availableCoordinates.OrderBy(_ => _.y).First();
                        nextStandingCoordinates = new HashSet<(int x, int y)>();
                        nextStandingCoordinates.Add(availableCoordinate);
                        minute++;
                        currentBlizzardCoordinates.Add(minute, nextBlizzardCoordinates);
                        currentStandingCoordinates.Add(minute, nextStandingCoordinates);
                        return;
                    }
                }
                minute++;
                currentBlizzardCoordinates.Add(minute, nextBlizzardCoordinates);
                currentStandingCoordinates.Add(minute, nextStandingCoordinates);
            }
        }

        private static void Parse(IEnumerable<string> lines,
            ref HashSet<(int x, int y)> wallCoordinates,
            ref Dictionary<int, HashSet<(int x, int y, char direction)>> currentBlizzardCoordinates,
            ref Dictionary<int, HashSet<(int x, int y)>> currentStandingCoordinates
            )
        {
            currentStandingCoordinates[0] = new HashSet<(int x, int y)>();
            currentBlizzardCoordinates[0] = new HashSet<(int x, int y, char direction)>();

            var linesArray = lines.ToArray();
            for (var i = 0; i < linesArray.Length; i++)
            {
                var charArray = linesArray[i].ToCharArray();
                for (var j = 0; j < charArray.Length; j++)
                {
                    if (charArray[j].Equals('#'))
                    {
                        wallCoordinates.Add((j, i));
                    }
                    else if (charArray[j].Equals('.'))
                    {
                    }
                    else if (charArray[j].Equals('E'))
                    {
                        currentStandingCoordinates[0].Add((j, i));
                    }
                    else
                    {
                        currentBlizzardCoordinates[0].Add((j, i, charArray[j]));
                    }
                }
            }

        }


        internal class Elf
        {
            private readonly int _x;
            private readonly int _y;

            public Elf(int x, int y)
            {
                _x = x;
                _y = y;
            }

            public (int x, int y) Coordinates => (_x, _y);

            public HashSet<(int x, int y)> AllDirections =>
                new HashSet<(int x, int y)>
                {
                    (_x+1, _y),
                    (_x-1, _y),
                    (_x, _y+1),
                    (_x, _y-1),
                    (_x, _y)
                };
        }
    }
}
