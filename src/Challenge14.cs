namespace Challenges
{
    public class Challenge14
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var edgeCoordinates = GetEdgeCoordinates(lines);
            var sandCoordinates = new HashSet<(int x, int y)>();

            int heighestYValue = edgeCoordinates.Max(_ => _.y);
            (int x, int y) currentPosition = (500, 0);

            while (currentPosition.y < heighestYValue)
            {
                CalculateNextPosition(heighestYValue + 1, edgeCoordinates, ref sandCoordinates, ref currentPosition);
            }

            Console.WriteLine(sandCoordinates.Count());
        }

        public static void Part2(IEnumerable<string> lines)
        {
            var edgeCoordinates = GetEdgeCoordinates(lines);
            var sandCoordinates = new HashSet<(int x, int y)>();

            int heighestYValue = edgeCoordinates.Max(_ => _.y) + 2;

            (int x, int y) currentPosition = (500, 0);

            while (!sandCoordinates.Contains((500, 0)))
            {
                CalculateNextPosition(heighestYValue, edgeCoordinates, ref sandCoordinates, ref currentPosition);
            }

            Console.WriteLine(sandCoordinates.Count());
        }

        public static HashSet<(int x, int y)> GetEdgeCoordinates(IEnumerable<string> lines)
        {
            var edgeCoordinates = new HashSet<(int x, int y)>();

            foreach (var line in lines)
            {
                var coordinatesArray = line.Split("->");

                for (var i = 1; i < coordinatesArray.Length; i++)
                {
                    var coordinates1 = coordinatesArray[i - 1].Split(',').Select(Int32.Parse).ToArray();
                    var coordinates2 = coordinatesArray[i].Split(',').Select(Int32.Parse).ToArray();

                    if (coordinates1[0] - coordinates2[0] != 0)
                    {
                        var start = Math.Min(coordinates1[0], coordinates2[0]);
                        var distance = Math.Abs(coordinates1[0] - coordinates2[0]) + 1;
                        var xCoordinates = Enumerable.Range(start, distance);
                        var yCoordinates = new int[] { }.Concat(Enumerable.Repeat(coordinates1[1], distance));
                        var coordinatesTuple = Enumerable.Zip(xCoordinates, yCoordinates);

                        edgeCoordinates.UnionWith(coordinatesTuple);
                    }
                    else if (coordinates1[1] - coordinates2[1] != 0)
                    {
                        var start = Math.Min(coordinates1[1], coordinates2[1]);
                        var distance = Math.Abs(coordinates1[1] - coordinates2[1]) + 1;
                        var xCoordinates = new int[] { }.Concat(Enumerable.Repeat(coordinates1[0], distance));
                        var yCoordinates = Enumerable.Range(start, Math.Abs(coordinates1[1] - coordinates2[1]) + 1);

                        edgeCoordinates.UnionWith(Enumerable.Zip(xCoordinates, yCoordinates));
                    }
                    else
                    {
                        edgeCoordinates.Add((coordinates1[0], coordinates1[1]));
                    }
                }
            }

            return edgeCoordinates;
        }

        public static void CalculateNextPosition(int heighestYValue, HashSet<(int x, int y)> edgeCoordinates, ref HashSet<(int x, int y)> sandCoordinates, ref (int x, int y) currentPosition)
        {
            (int x, int y) downPosition = (currentPosition.x, currentPosition.y + 1);
            (int x, int y) leftPosition = (currentPosition.x - 1, currentPosition.y + 1);
            (int x, int y) rightPosition = (currentPosition.x + 1, currentPosition.y + 1);

            if (!edgeCoordinates.Contains(downPosition) && !sandCoordinates.Contains(downPosition) && downPosition.y != heighestYValue)
            {
                currentPosition = downPosition;
            }
            else if (!edgeCoordinates.Contains(leftPosition) && !sandCoordinates.Contains(leftPosition) && leftPosition.y != heighestYValue)
            {
                currentPosition = leftPosition;
            }
            else if (!edgeCoordinates.Contains(rightPosition) && !sandCoordinates.Contains(rightPosition) && rightPosition.y != heighestYValue)
            {
                currentPosition = rightPosition;
            }
            else
            {
                sandCoordinates.Add(currentPosition);
                currentPosition = (500, 0);
            }
        }
    }
}