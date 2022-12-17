using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge16
    {
        public static void Part1(IEnumerable<string> lines, int timeout = 30, string start = "AA")
        {
            var allValves = new Dictionary<string, Valve>();
            var shortestDistances = new Dictionary<(Valve, Valve), int>();

            Parse(lines, ref allValves);
            ShortestPaths(allValves.Values.ToHashSet(), ref shortestDistances);

            var startValve = allValves.First(_ => _.Key == start).Value;
            var unvisitedValves = allValves.Where(v => v.Value.FlowRate > 0).Select(_ => _.Value);
            var startValveTimes = new List<(int timeRemaining, Valve currentValve)>
            {
                (timeout, startValve)
            };

            Console.WriteLine(CalculatePressure(startValveTimes, unvisitedValves, shortestDistances));
        }

        public static void Part2(IEnumerable<string> lines, int timeout = 26, string start = "AA")
        {
            var allValves = new Dictionary<string, Valve>();
            var shortestDistances = new Dictionary<(Valve, Valve), int>();

            Parse(lines, ref allValves);
            ShortestPaths(allValves.Values.ToHashSet(), ref shortestDistances);

            var startValve = allValves.First(_ => _.Key == start).Value;
            var unvisitedValves = allValves.Where(v => v.Value.FlowRate > 0).Select(_ => _.Value);

            //If elephant is helping us out, have two of us starting
            var startValveTimes = new List<(int timeRemaining, Valve currentValve)>
            {
                (timeout, startValve),
                (timeout, startValve)
            };

            Console.WriteLine(CalculatePressure(startValveTimes, unvisitedValves, shortestDistances));
        }

        private static void Parse(IEnumerable<string> lines, ref Dictionary<string, Valve> allValves)
        {
            foreach (var line in lines)
            {
                var valves = Regex.Matches(line, @"[A-Z]{2}")
                    .Cast<Match>()
                    .Select(x => x.Value).ToArray();
                var currentValve = valves[0];
                var adjacentValves = new HashSet<string>(valves.Where(_ => _ != currentValve));

                var flowRate = Regex.Matches(line, @"(\-?)\d+")
                    .Cast<Match>()
                    .Where(_ => !string.IsNullOrWhiteSpace(_.Value))
                    .Select(x => Int32.Parse(x.Value)).FirstOrDefault();

                allValves.Add(currentValve, new Valve(currentValve, flowRate, adjacentValves));
            }

        }

        private static void ShortestPaths(HashSet<Valve> valves, ref Dictionary<(Valve, Valve), int> distances)
        {
            foreach (var valve in valves)
            {
                ShortestPath(valves, valve, ref distances);
            }
        }

        private static void ShortestPath(HashSet<Valve> valves, Valve sourceValve, ref Dictionary<(Valve, Valve), int> distances)
        {
            var queue = new Queue<(Valve valve, int distance)>();

            queue.Enqueue((sourceValve, 0));

            while (queue.TryDequeue(out var currentValve))
            {
                foreach (var adjacentValveValue in currentValve.valve.AdjacentValves)
                {
                    var adjacentValve = valves.FirstOrDefault(_ => _.Name == adjacentValveValue);
                    if (!distances.ContainsKey((sourceValve, adjacentValve)))
                    {
                        distances[(sourceValve, adjacentValve)] = currentValve.distance + 1;
                        queue.Enqueue((adjacentValve, currentValve.distance + 1));
                    }
                }
            }
        }

        private static int CalculatePressure(IEnumerable<(int timeRemaining, Valve currentValve)> valveTimes, IEnumerable<Valve> unvisitedValves, Dictionary<(Valve, Valve), int> shortestDistances)
        {
            var maxPressure = 0;
            var currentValveTime = valveTimes.OrderByDescending(_ => _.timeRemaining).First();

            foreach (var valve in unvisitedValves)
            {
                var timeRemaining = currentValveTime.timeRemaining - shortestDistances[(currentValveTime.currentValve, valve)] - 1;
                if (timeRemaining <= 0)
                {
                    continue;
                }

                var currentValveTimes = new List<(int timeRemaining, Valve valve)>
                {
                    (timeRemaining, valve),
                };

                // If elephant is helping us out add other current valve and time
                if (valveTimes.Count() > 1)
                {
                    currentValveTimes.Add(valveTimes.OrderByDescending(_ => _.timeRemaining).Last());

                }

                var currentUnvistedValves = unvisitedValves.Where(v => v.Name != valve.Name);
                var currentPressure = timeRemaining * valve.FlowRate + CalculatePressure(currentValveTimes, currentUnvistedValves, shortestDistances);
                maxPressure = Math.Max(currentPressure, maxPressure);
            }

            return maxPressure;
        }
    }
}

public record Valve
{
    public Valve(string currentValve, int flowRate, HashSet<string> adjacentValves)
    {
        Name = currentValve;
        FlowRate = flowRate;
        AdjacentValves = adjacentValves;
    }

    public string Name { get; init; }
    public int FlowRate { get; init; }
    public HashSet<string> AdjacentValves { get; init; }
}