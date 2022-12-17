using System.Numerics;
using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge15
    {
        public static void Part1(IEnumerable<string> lines, int currentRow = 2000000)
        {
            var sensorBeacons = new List<SensorBeacon>();
            var noBeaconsRow = new HashSet<(int x, int y)>();
            foreach (var line in lines)
            {
                var coordinates = line.Split('=')
                    .Select(x => Regex.Match(x, @"(\-?)\d+"))
                    .Cast<Match>()
                    .Where(_ => !string.IsNullOrWhiteSpace(_.Value))
                    .Select(x => int.Parse(x.Value)).ToArray();

                sensorBeacons.Add(new SensorBeacon((coordinates[0], coordinates[1]), (coordinates[2], coordinates[3])));
            }

            foreach (var sb in sensorBeacons)
            {
                IEnumerable<int> yRange;
                bool isInRange = false;

                if (sb.Sensor.y >= currentRow)
                {
                    yRange = Enumerable.Range(currentRow, Math.Abs(sb.Radius) + 1);
                    isInRange = yRange.Contains(sb.Sensor.y);
                }
                else
                {
                    yRange = Enumerable.Range(sb.Sensor.y, Math.Abs(sb.Radius) + 1);
                    isInRange = yRange.Contains(currentRow);
                }

                if (isInRange)
                {
                    var currentYDistance = Math.Abs(sb.Sensor.y - currentRow);
                    var currentXDistance = Math.Abs(sb.Radius - currentYDistance);

                    var xCoordinates = Enumerable.Range(sb.Sensor.x - currentXDistance, currentXDistance * 2 + 1);
                    var yCoordinates = new int[] { }.Concat(Enumerable.Repeat(currentRow, xCoordinates.Count() + 1));

                    var noBeacons = Enumerable.Zip(xCoordinates, yCoordinates).Where(_ => _ != (sb.Beacon.x, sb.Beacon.y));

                    noBeaconsRow.UnionWith(noBeacons);
                }

            }

            Console.WriteLine(noBeaconsRow.Count());
        }

        public static void Part2(IEnumerable<string> lines, int minSize = 0, int maxSize = 4000000)
        {
            var sensorBeacons = new List<SensorBeacon>();

            foreach (var line in lines)
            {
                var coordinates = line.Split('=')
                    .Select(x => Regex.Match(x, @"(\-?)\d+"))
                    .Cast<Match>()
                    .Where(_ => !string.IsNullOrWhiteSpace(_.Value))
                    .Select(x => int.Parse(x.Value)).ToArray();

                sensorBeacons.Add(new SensorBeacon((coordinates[0], coordinates[1]), (coordinates[2], coordinates[3])));
            }

            (int x, int y) lostBeacon = (0, 0);

            for (var i = minSize; i <= maxSize; i++)
            {
                var intersections = new List<(int start, int end)>();
                var ends = new List<int>();

                foreach (var sb in sensorBeacons)
                {
                    var currentDistance = Calculate.ManhattanDistance(sb.Sensor, (sb.Sensor.x, i));

                    if (currentDistance > sb.Radius)
                    {
                        continue;
                    }

                    var start = Calculate.RowDistanceStart(sb.Sensor.x, sb.Radius, currentDistance, minSize);
                    var end = Calculate.RowDistanceEnd(sb.Sensor.x, sb.Radius, currentDistance, maxSize);

                    intersections.Add((start, end));
                }

                var intersectionsArray = intersections.OrderBy(_ => _.start).ToArray();

                var minStart = intersectionsArray[0].start;
                if (minStart > 0)
                {
                    lostBeacon = (minStart + 1, i);
                    goto End;
                }

                var maxEnd = intersectionsArray[0].end;

                for (var j = 1; j < intersectionsArray.Length; j++)
                {
                    var currentStart = intersectionsArray[j].start;
                    if (intersectionsArray[j].start - maxEnd == 2)
                    {
                        lostBeacon = (maxEnd + 1, i);
                        goto End;
                    }
                    if (intersectionsArray[j].end > maxEnd)
                    {
                        maxEnd = intersectionsArray[j].end;
                    }
                }
            }

        End:
            Console.WriteLine(new BigInteger(lostBeacon.x) * new BigInteger(maxSize) + new BigInteger(lostBeacon.y));
        }
    }

    public record SensorBeacon
    {
        public SensorBeacon((int, int) sensor, (int, int) beacon)
        {
            Sensor = sensor;
            Beacon = beacon;
        }

        public (int x, int y) Sensor { get; init; }
        public (int x, int y) Beacon { get; init; }
        public int Radius => Calculate.ManhattanDistance(Sensor, Beacon);

    }

    public static class Calculate
    {
        public static int ManhattanDistance((int x, int y) coordinates1, (int x, int y) coordinates2) => Math.Abs(coordinates1.x - coordinates2.x) + Math.Abs(coordinates1.y - coordinates2.y);

        public static int RowDistanceStart(int sensorX, int radius, int distance, int minStart) => Math.Max(sensorX - radius + distance, minStart);

        public static int RowDistanceEnd(int sensorX, int radius, int distance, int maxEnd) => Math.Min(sensorX + radius - distance, maxEnd);
    }
}