namespace Challenges
{
    public class Challenge9
    {
        public static void Part1And2(IEnumerable<string> lines, Dictionary<int, (int x, int y)> dictCoords)
        {
            var tCoordsHistory = new List<(int x, int y)>();

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

        private static (int x, int y) GetInitialCoords(string direction, (int x, int y) coords)
        {
            switch (direction)
            {
                case "L":
                    coords.y--;
                    break;
                case "R":
                    coords.y++;
                    break;
                case "U":
                    coords.x--;
                    break;
                case "D":
                    coords.x++;
                    break;
                default:
                    break;
            }

            return coords;
        }

        private static (int x, int y) GetCurrentCoords(int index, (int x, int y) hCoords, (int x, int y) tCoords)
        {
            var distanceToMovement = new Dictionary<(int x, int y), (int x, int y)> {
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

            (int x, int y) distance = (hCoords.x - tCoords.x, hCoords.y - tCoords.y);

            if (Math.Abs(distance.x) < 2 && Math.Abs(distance.y) < 2)
            {
                return tCoords;
            }

            var movement = distanceToMovement[distance];

            return (tCoords.x + movement.x, tCoords.y + movement.y);
        }
    }
}
