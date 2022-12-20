using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge19
    {
        public static void Part1(IEnumerable<string> lines, int timeRemaining = 24)
        {
            var bluePrints = new Dictionary<int, BluePrint>();
            var startRobots = new Robots((1, 0), (0, 0), (0, 0), (0, 0));

            Parse(lines, ref bluePrints);

            Console.WriteLine(CalculateQualtyLevel(bluePrints, startRobots, timeRemaining));
        }

        public static void Part2(IEnumerable<string> lines, int timeRemaining = 32)
        {
            var bluePrints = new Dictionary<int, BluePrint>();
            var startRobots = new Robots((1, 0), (0, 0), (0, 0), (0, 0));

            Parse(lines, ref bluePrints);
            
            Console.WriteLine(CalculateQualtyLevel(bluePrints, startRobots, timeRemaining));
        }

        private static void Parse(IEnumerable<string> lines, ref Dictionary<int, BluePrint> bluePrints)
        {
            foreach (var line in lines)
            {
                var bluePrint = new BluePrint();
                var matches = Regex.Matches(line, @"(\d+)").Select(_ => Int32.Parse(_.Value)).ToArray();
                var bluePrintIndex = matches[0];

                bluePrint.OreOreCost = matches[1];
                bluePrint.ClayOreCost = matches[2];
                bluePrint.ObsidianOreCost = matches[3];
                bluePrint.ObsidianClayCost = matches[4];
                bluePrint.GeodeOreCost = matches[5];
                bluePrint.GeodeObsidianCost = matches[6];

                bluePrints.Add(bluePrintIndex, bluePrint);
            }
        }

        private static int CalculateQualtyLevel(Dictionary<int, BluePrint> blueprints, Robots startRobot, int timeRemaining)
        {
            List<(int Index, int QualityLevel)> qualityLevels = new List<(int index, int max)>();


            foreach (var bluePrint in blueprints)
            {
                var currentMax = CalculateMax(bluePrint.Value, startRobot, timeRemaining);
                qualityLevels.Add((bluePrint.Key, currentMax * bluePrint.Key));
                Console.WriteLine(bluePrint.Key + " " + currentMax);
            }

            return qualityLevels.Sum(_ => _.QualityLevel);
        }

        private static int CalculateMax(BluePrint bluePrint, Robots startRobots, int timeRemaining)
        {
            var incomingStates = new HashSet<State>();
            incomingStates.Add(new State(timeRemaining, startRobots));
            var processedStates = new HashSet<State>();
            
            while (incomingStates.Count() > 0)
            {
                var currentState = incomingStates.First();
                if (processedStates.Contains(currentState))
                {
                    incomingStates.Remove(currentState);
                    continue;
                }

                processedStates.Add(currentState);
                incomingStates.Remove(currentState);

                if (currentState.TimeRemaining <= 0)
                {
                    continue;
                }

                var tryBuyOre = TryBuyOre(bluePrint, currentState, out var nextOreState);
                var tryBuyClay = TryBuyClay(bluePrint, currentState, out var nextClayState);
                var tryBuyObsidian = TryBuyObsidian(bluePrint, currentState, out var nextObsidianState);
                var tryBuyGeode = TryBuyGeode(bluePrint, currentState, out var nextGeodeState);

                if (tryBuyGeode)
                {
                    incomingStates.Add(nextGeodeState);
                }
                else
                {
                    if (tryBuyOre)
                    {
                        incomingStates.Add(nextOreState);
                    }
                    if (tryBuyClay)
                    {
                        incomingStates.Add(nextClayState);
                    }
                    if (tryBuyObsidian)
                    {
                        incomingStates.Add(nextObsidianState);
                    }

                    var robots = new Robots(currentState.CurrentRobots.Ore, currentState.CurrentRobots.Clay, currentState.CurrentRobots.Obsidian, currentState.CurrentRobots.Geode);
                    robots.Ore = (currentState.CurrentRobots.Ore.RobotAmount, currentState.CurrentRobots.Ore.CurrentOreAmount + currentState.CurrentRobots.Ore.RobotAmount);
                    robots.Clay = (currentState.CurrentRobots.Clay.RobotAmount, currentState.CurrentRobots.Clay.CurrentClayAmount + currentState.CurrentRobots.Clay.RobotAmount);
                    robots.Obsidian = (currentState.CurrentRobots.Obsidian.RobotAmount, currentState.CurrentRobots.Obsidian.CurrentObsidianAmount + currentState.CurrentRobots.Obsidian.RobotAmount);
                    robots.Geode = (currentState.CurrentRobots.Geode.RobotAmount, currentState.CurrentRobots.Geode.CurrentGeodeAmount + currentState.CurrentRobots.Geode.RobotAmount);

                    incomingStates.Add(new State(currentState.TimeRemaining - 1, robots));
                }
            }

            return processedStates.Max(_ => _.CurrentRobots.Geode.CurrentGeodeAmount);
        }

        private static bool TryBuyOre(BluePrint bluePrint, State currentState, out State nextState)
        {
            var robots = new Robots(currentState.CurrentRobots.Ore, currentState.CurrentRobots.Clay, currentState.CurrentRobots.Obsidian, currentState.CurrentRobots.Geode);
            robots.Ore = (currentState.CurrentRobots.Ore.RobotAmount + 1, currentState.CurrentRobots.Ore.CurrentOreAmount + currentState.CurrentRobots.Ore.RobotAmount - bluePrint.OreOreCost);
            robots.Clay = (currentState.CurrentRobots.Clay.RobotAmount, currentState.CurrentRobots.Clay.CurrentClayAmount + currentState.CurrentRobots.Clay.RobotAmount);
            robots.Obsidian = (currentState.CurrentRobots.Obsidian.RobotAmount, currentState.CurrentRobots.Obsidian.CurrentObsidianAmount + currentState.CurrentRobots.Obsidian.RobotAmount);
            robots.Geode = (currentState.CurrentRobots.Geode.RobotAmount, currentState.CurrentRobots.Geode.CurrentGeodeAmount + currentState.CurrentRobots.Geode.RobotAmount);

            nextState = new State(currentState.TimeRemaining - 1, robots);

            if (currentState.CurrentRobots.Ore.CurrentOreAmount < bluePrint.OreOreCost ||
                (currentState.CurrentRobots.Ore.RobotAmount >= bluePrint.ClayOreCost &&
                    currentState.CurrentRobots.Ore.RobotAmount >= bluePrint.ObsidianOreCost &&
                    currentState.CurrentRobots.Ore.RobotAmount >= bluePrint.GeodeOreCost))
            {
                return false;
            }
            return true;
        }

        private static bool TryBuyClay(BluePrint bluePrint, State currentState, out State nextState)
        {
            var robots = new Robots(currentState.CurrentRobots.Ore, currentState.CurrentRobots.Clay, currentState.CurrentRobots.Obsidian, currentState.CurrentRobots.Geode);
            robots.Ore = (currentState.CurrentRobots.Ore.RobotAmount, currentState.CurrentRobots.Ore.CurrentOreAmount + currentState.CurrentRobots.Ore.RobotAmount - bluePrint.ClayOreCost);
            robots.Clay = (currentState.CurrentRobots.Clay.RobotAmount + 1, currentState.CurrentRobots.Clay.CurrentClayAmount + currentState.CurrentRobots.Clay.RobotAmount);
            robots.Obsidian = (currentState.CurrentRobots.Obsidian.RobotAmount, currentState.CurrentRobots.Obsidian.CurrentObsidianAmount + currentState.CurrentRobots.Obsidian.RobotAmount);
            robots.Geode = (currentState.CurrentRobots.Geode.RobotAmount, currentState.CurrentRobots.Geode.CurrentGeodeAmount + currentState.CurrentRobots.Geode.RobotAmount);

            nextState = new State(currentState.TimeRemaining - 1, robots);

            if (currentState.CurrentRobots.Ore.CurrentOreAmount < bluePrint.ClayOreCost ||
                currentState.CurrentRobots.Clay.RobotAmount >= bluePrint.ObsidianClayCost)
            {
                return false;
            }
            return true;
        }

        private static bool TryBuyObsidian(BluePrint bluePrint, State currentState, out State nextState)
        {
            var robots = new Robots(currentState.CurrentRobots.Ore, currentState.CurrentRobots.Clay, currentState.CurrentRobots.Obsidian, currentState.CurrentRobots.Geode);
            robots.Ore = (currentState.CurrentRobots.Ore.RobotAmount, currentState.CurrentRobots.Ore.CurrentOreAmount + currentState.CurrentRobots.Ore.RobotAmount - bluePrint.ObsidianOreCost);
            robots.Clay = (currentState.CurrentRobots.Clay.RobotAmount, currentState.CurrentRobots.Clay.CurrentClayAmount + currentState.CurrentRobots.Clay.RobotAmount - bluePrint.ObsidianClayCost);
            robots.Obsidian = (currentState.CurrentRobots.Obsidian.RobotAmount + 1, currentState.CurrentRobots.Obsidian.CurrentObsidianAmount + currentState.CurrentRobots.Obsidian.RobotAmount);
            robots.Geode = (currentState.CurrentRobots.Geode.RobotAmount, currentState.CurrentRobots.Geode.CurrentGeodeAmount + currentState.CurrentRobots.Geode.RobotAmount);

            nextState = new State(currentState.TimeRemaining - 1, robots);

            if (currentState.CurrentRobots.Ore.CurrentOreAmount < bluePrint.ObsidianOreCost ||
                currentState.CurrentRobots.Clay.CurrentClayAmount < bluePrint.ObsidianClayCost ||
                currentState.CurrentRobots.Obsidian.RobotAmount >= bluePrint.GeodeObsidianCost)
            {
                return false;
            }
            return true;
        }

        private static bool TryBuyGeode(BluePrint bluePrint, State currentState, out State nextState)
        {
            var robots = new Robots(currentState.CurrentRobots.Ore, currentState.CurrentRobots.Clay, currentState.CurrentRobots.Obsidian, currentState.CurrentRobots.Geode);
            robots.Ore = (currentState.CurrentRobots.Ore.RobotAmount, currentState.CurrentRobots.Ore.CurrentOreAmount + currentState.CurrentRobots.Ore.RobotAmount - bluePrint.GeodeOreCost);
            robots.Clay = (currentState.CurrentRobots.Clay.RobotAmount, currentState.CurrentRobots.Clay.CurrentClayAmount + currentState.CurrentRobots.Clay.RobotAmount);
            robots.Obsidian = (currentState.CurrentRobots.Obsidian.RobotAmount, currentState.CurrentRobots.Obsidian.CurrentObsidianAmount + currentState.CurrentRobots.Obsidian.RobotAmount - bluePrint.GeodeObsidianCost);
            robots.Geode = (currentState.CurrentRobots.Geode.RobotAmount + 1, currentState.CurrentRobots.Geode.CurrentGeodeAmount + currentState.CurrentRobots.Geode.RobotAmount);

            nextState = new State(currentState.TimeRemaining - 1, robots);

            if (currentState.CurrentRobots.Ore.CurrentOreAmount < bluePrint.GeodeOreCost ||
                currentState.CurrentRobots.Obsidian.CurrentObsidianAmount < bluePrint.GeodeObsidianCost)
            {
                return false;
            }
            return true;
        }

        internal record BluePrint
        {
            public BluePrint()
            {
            }

            public int OreOreCost { get; set; }
            public int ClayOreCost { get; set; }
            public int ObsidianOreCost { get; set; }
            public int ObsidianClayCost { get; set; }
            public int GeodeOreCost { get; set; }
            public int GeodeObsidianCost { get; set; }
        }

        internal record Robots
        {
            public Robots((int RobotAmount, int CurrentOreAmount) ore,
                (int RobotAmount, int CurrentClayAmount) clay,
                (int RobotAmount, int CurrentObsidianAmount) obsidian,
                (int RobotAmount, int CurrentGeodAmount) geode)
            {
                Ore = ore;
                Clay = clay;
                Obsidian = obsidian;
                Geode = geode;
            }

            public (int RobotAmount, int CurrentOreAmount) Ore { get; set; }

            public (int RobotAmount, int CurrentClayAmount) Clay { get; set; }

            public (int RobotAmount, int CurrentObsidianAmount) Obsidian { get; set; }

            public (int RobotAmount, int CurrentGeodeAmount) Geode { get; set; }
        }

        internal record State
        {
            public State(int timeRemaining, Robots currentRobot)
            {
                TimeRemaining = timeRemaining;
                CurrentRobots = currentRobot;
            }

            public int TimeRemaining { get; init; }
            public Robots CurrentRobots { get; init; }
        }
    }
}
