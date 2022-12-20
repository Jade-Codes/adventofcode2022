using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge16
    {
        public static void Part1(IEnumerable<string> lines, int timeRemaining = 30, string start = "AA")
        {
            var allValves = new Dictionary<string, Valve>();
            var shortestDistances = new Dictionary<(Valve, Valve), int>();

            Parse(lines, ref allValves);
            ShortestPaths(allValves.Values.ToHashSet(), ref shortestDistances);

            var startValve = allValves.First(_ => _.Key == start).Value;
            var unvisitedValves = allValves.Where(v => v.Value.FlowRate > 0).Select(_ => _.Value);
            var startValveTimes = new List<State>
            {
                new State(timeRemaining, startValve)
            };

            Console.WriteLine(CalculatePressure(startValveTimes, unvisitedValves, shortestDistances));
        }

        public static void Part2(IEnumerable<string> lines, int timeRemaining = 26, string start = "AA")
        {
            var allValves = new Dictionary<string, Valve>();
            var shortestDistances = new Dictionary<(Valve, Valve), int>();

            Parse(lines, ref allValves);
            ShortestPaths(allValves.Values.ToHashSet(), ref shortestDistances);

            var startValve = allValves.First(_ => _.Key == start).Value;
            var unvisitedValves = allValves.Where(v => v.Value.FlowRate > 0).Select(_ => _.Value);

            //If elephant is helping us out, have two of us starting
            var startStates = new List<State>
            {
                new State(timeRemaining, startValve),
                new State(timeRemaining, startValve)
            };

            Console.WriteLine(CalculatePressure(startStates, unvisitedValves, shortestDistances));
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

        private static int CalculatePressure(IEnumerable<State> states, IEnumerable<Valve> unvisitedValves, Dictionary<(Valve, Valve), int> shortestDistances)
        {
            var maxPressure = 0;
            var currentState = states.OrderByDescending(_ => _.TimeRemaining).First();

            foreach (var valve in unvisitedValves)
            {
                var timeRemaining = currentState.TimeRemaining - shortestDistances[(currentState.CurrentValve, valve)] - 1;
                if (timeRemaining <= 0)
                {
                    continue;
                }

                var currentStates = new List<State>
                {
                    new State(timeRemaining, valve),
                };

                // If elephant is helping us out add other current valve and time
                if (states.Count() > 1)
                {
                    currentStates.Add(states.OrderByDescending(_ => _.TimeRemaining).Last());

                }

                var currentUnvisitedValves = unvisitedValves.Where(v => v.Name != valve.Name);
                var currentPressure = timeRemaining * valve.FlowRate + CalculatePressure(currentStates, currentUnvisitedValves, shortestDistances);
                maxPressure = Math.Max(currentPressure, maxPressure);
            }

            return maxPressure;
        }

        internal record Valve
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

        internal record State
        {
            public State(int timeRemaining, Valve currentValve)
            {
                TimeRemaining = timeRemaining;
                CurrentValve = currentValve;
            }

            public int TimeRemaining { get; init; }
            public Valve CurrentValve { get; init; }
        }
    }
}

