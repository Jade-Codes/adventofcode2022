namespace Challenges
{
    public class Challenge23
    {

        private static Dictionary<int, char> rounds = new Dictionary<int, char>()
        {
            {0, 'N'},
            {1, 'S'},
            {2, 'W'},
            {3, 'E'}
        };

        public static void Part1(IEnumerable<string> lines)
        {
            var currentCoordinates = new HashSet<(int x, int y)>();
            var elvesMove = true;
            Parse(lines, ref currentCoordinates);

            for (var i = 0; i < 10; i++)
            {
                var currentRound = i;
                CalculateMove(lines, ref currentCoordinates, ref currentRound, ref elvesMove);
            }

            var height = currentCoordinates.Max(_ => _.y) - currentCoordinates.Min(_ => _.y) + 1;
            var width = currentCoordinates.Max(_ => _.x) - currentCoordinates.Min(_ => _.x) + 1;
            var total = height * width;
            var totalEmpty = total - currentCoordinates.Count();

            Console.WriteLine(totalEmpty);

        }

        public static void Part2(IEnumerable<string> lines)
        {
            var currentCoordinates = new HashSet<(int x, int y)>();
            var currentRound = 0;
            var elvesMove = true;

            Parse(lines, ref currentCoordinates);

            while (elvesMove)
            {
                CalculateMove(lines, ref currentCoordinates, ref currentRound, ref elvesMove);
            }

            Console.WriteLine(currentRound);
        }

        private static void Parse(IEnumerable<string> lines,
            ref HashSet<(int x, int y)> currentCoordinates)
        {
            var linesArray = lines.ToArray();
            for (var i = 1; i <= linesArray.Length; i++)
            {
                var charArray = linesArray[i - 1].ToCharArray();
                for (var j = 1; j <= charArray.Length; j++)
                {
                    if (charArray[j - 1].Equals('#'))
                    {
                        currentCoordinates.Add((j, i));
                    }
                }
            }

        }

        private static void CalculateMove(IEnumerable<string> lines,
            ref HashSet<(int x, int y)> currentCoordinates,
            ref int currentRound,
            ref bool elvesMove)
        {
            var newCoordinates = new Dictionary<(int x, int y), (int x, int y)>();

            foreach (var coordinate in currentCoordinates)
            {
                var elf = new Elf(coordinate.x, coordinate.y);

                if (!currentCoordinates.Intersect(elf.AllDirections).Any())
                {
                    newCoordinates.Add(coordinate, coordinate);
                    continue;
                }

                var currentTry = 0;
                while (currentTry <= 4)
                {
                    if (currentTry == 4)
                    {
                        newCoordinates[coordinate] = coordinate;
                        currentTry = 0;
                        goto NextElf;
                    }

                    var mod = (currentTry + currentRound) % 4;

                    var direction = rounds[mod];

                    switch (direction)
                    {
                        case 'N':
                            if (currentCoordinates.Intersect(elf.NorthDirections).Any())
                            {
                                goto NextDirection;
                            }
                            else if (newCoordinates.Values.Any(_ => _ == elf.NorthDirection))
                            {
                                var otherCoordinate = newCoordinates.FirstOrDefault(_ => _.Value == elf.NorthDirection);
                                newCoordinates[otherCoordinate.Key] = otherCoordinate.Key;
                                newCoordinates[coordinate] = coordinate;
                                currentTry = 1;
                                goto NextElf;
                            }
                            else
                            {
                                newCoordinates[elf.Coordinates] = elf.NorthDirection;
                                goto NextElf;
                            }
                        case 'S':
                            if (currentCoordinates.Intersect(elf.SouthDirections).Any())
                            {
                                goto NextDirection;
                            }
                            else if (newCoordinates.Values.Any(_ => _ == elf.SouthDirection))
                            {
                                var otherCoordinate = newCoordinates.FirstOrDefault(_ => _.Value == elf.SouthDirection);
                                newCoordinates[otherCoordinate.Key] = otherCoordinate.Key;
                                newCoordinates[coordinate] = coordinate;
                                currentTry = 1;
                                goto NextElf;
                            }
                            else
                            {
                                newCoordinates[elf.Coordinates] = elf.SouthDirection;
                                goto NextElf;
                            }
                        case 'W':
                            if (currentCoordinates.Intersect(elf.WestDirections).Any())
                            {
                                goto NextDirection;
                            }
                            else if (newCoordinates.Values.Any(_ => _ == elf.WestDirection))
                            {
                                var otherCoordinate = newCoordinates.FirstOrDefault(_ => _.Value == elf.WestDirection);
                                newCoordinates[otherCoordinate.Key] = otherCoordinate.Key;
                                newCoordinates[coordinate] = coordinate;
                                currentTry = 1;
                                goto NextElf;
                            }
                            else
                            {
                                newCoordinates[elf.Coordinates] = elf.WestDirection;
                                goto NextElf;
                            }
                        case 'E':
                            if (currentCoordinates.Intersect(elf.EastDirections).Any())
                            {
                                goto NextDirection;
                            }
                            else if (newCoordinates.Values.Any(_ => _ == elf.EastDirection))
                            {
                                var otherCoordinate = newCoordinates.FirstOrDefault(_ => _.Value == elf.EastDirection);
                                newCoordinates[otherCoordinate.Key] = otherCoordinate.Key;
                                newCoordinates[coordinate] = coordinate;
                                currentTry = 1;
                                goto NextElf;
                            }
                            else
                            {
                                newCoordinates[elf.Coordinates] = elf.EastDirection;
                                goto NextElf;
                            }
                    }
                NextDirection:
                    currentTry++;
                }
            NextElf:;
            }

            currentRound++;

            if (currentCoordinates.Except(newCoordinates.Values).Count() == 0)
            {
                elvesMove = false;
                return;
            }

            currentCoordinates = newCoordinates.Values.ToHashSet();
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

            public (int x, int y) NorthDirection => (_x, _y - 1);

            public (int x, int y) SouthDirection => (_x, _y + 1);

            public (int x, int y) WestDirection => (_x - 1, _y);

            public (int x, int y) EastDirection => (_x + 1, _y);
            public HashSet<(int x, int y)> NorthDirections =>
                new HashSet<(int x, int y)>
                {
                    (_x, _y-1),
                    (_x+1, _y-1),
                    (_x-1, _y-1),
                };
            public HashSet<(int x, int y)> WestDirections =>
                new HashSet<(int x, int y)>
                {
                    (_x-1, _y),
                    (_x-1, _y+1),
                    (_x-1, _y-1),
                };
            public HashSet<(int x, int y)> EastDirections =>
                new HashSet<(int x, int y)>
                {
                    (_x+1, _y),
                    (_x+1, _y+1),
                    (_x+1, _y-1),
                };
            public HashSet<(int x, int y)> SouthDirections =>
                new HashSet<(int x, int y)>
                {
                    (_x, _y+1),
                    (_x+1, _y+1),
                    (_x-1, _y+1),
                };
            public HashSet<(int x, int y)> AllDirections =>
                new HashSet<(int x, int y)>
                {
                    (_x+1, _y),
                    (_x-1, _y),
                    (_x, _y+1),
                    (_x, _y-1),
                    (_x+1, _y+1),
                    (_x+1, _y-1),
                    (_x-1, _y+1),
                    (_x-1, _y-1),
                };
        }
    }
}
