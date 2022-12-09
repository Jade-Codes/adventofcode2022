namespace Challenges
{
    public class Challenge9
    {
        public static void Part1And2(IEnumerable<string> lines, Dictionary<int, (int, int)> dictCoords)
        {
            var tCoordsHistory = new List<(int, int)>();

            foreach (var line in lines)
            {
                var splitLine = line.Split(' ');
                var isNumber = Int32.TryParse(splitLine[1], out var number);
                for (int i = 0; i < number; i++)
                {
                    dictCoords[0] = GetInitialCoords(splitLine[0], dictCoords[0]);

                    for (var j = 1; j < dictCoords.Count; j++)
                    {
                        dictCoords[j] = GetCurrentCoords(j, dictCoords[j - 1], dictCoords[j]);

                        if (j == dictCoords.Count-1)
                        {
                            tCoordsHistory.Add(dictCoords[j]);
                        }
                    }
                }
            }

            Console.WriteLine(tCoordsHistory.Distinct().Count());
        }

        private static (int, int) GetInitialCoords(string direction, (int, int) coords)
        {
            switch (direction)
            {
                case "L":
                    coords.Item2--;
                    break;
                case "R":
                    coords.Item2++;
                    break;
                case "U":
                    coords.Item1--;
                    break;
                case "D":
                    coords.Item1++;
                    break;
                default:
                    break;
            }

            return coords;
        }

        private static (int, int) GetCurrentCoords(int index, (int, int) hCoords, (int, int) tCoords)
        {
            var distanceToMovement = new Dictionary<(int, int), (int, int)> {
                {(2,0), (1,0)},
                {(2,1), (1,1)},
                {(2,2), (1,1)},
                {(1,2), (1,1)},
                {(0,2), (0,1)},
                {(-1,2), (-1,1)},
                {(-2,2), (-1,1)},
                {(-2,1), (-1,1)},
                {(-2,0), (-1,0)},
                {(-2,-1), (-1,-1)},
                {(-2,-2), (-1,-1)},
                {(-1,-2), (-1,-1)},
                {(0,-2), (0,-1)},
                {(1,-2), (1,-1)},
                {(2,-2), (1,-1)},
                {(2,-1), (1,-1)},
            };

            var distance = (hCoords.Item1 - tCoords.Item1, hCoords.Item2 - tCoords.Item2);

            if (Math.Abs(distance.Item1) < 2 && Math.Abs(distance.Item2) < 2)
            {
                return tCoords;
            }

            var movement = distanceToMovement[distance];

            return (tCoords.Item1 + movement.Item1, tCoords.Item2 + movement.Item2);
        }
    }
}
