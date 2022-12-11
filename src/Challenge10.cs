namespace Challenges
{
    public class Challenge10
    {
        private const string LIGHT_UP = "#";
        private const string NOOP_NAME = "noop";
        private const string ADD_X_NAME = "addx";
        private const int NOOP_CYCLES = 1;
        private const int ADD_X_CYCLES = 2;
        private const int FIRST_ROUND_SIZE = 20;
        private const int TOTAL_ROUNDS_SIZE = 240;
        private const int INCREMENT_SIZE = 40;

        public static void Part1And2(IEnumerable<string> lines)
        {
            var linesArray = lines.ToArray();
            var currentPosition = 1;
            var upcomingCommands = new LinkedList<(int key, string value)>();
            var renderArray = Enumerable.Repeat(" ", INCREMENT_SIZE).ToArray();
            var sprintPosition = new int[3] { 1, 2, 3 };
            var signalStrengths = new Dictionary<int, int>();

            for (var i = 0; i < linesArray.Length; i++)
            {
                AddCommand(linesArray[i], ref upcomingCommands);
            }

            for (var i = 0; i < TOTAL_ROUNDS_SIZE; i++)
            {
                var currentCycle = i + 1;

                AddSignalStrength(currentCycle, currentPosition, ref signalStrengths);

                RenderAndResetLine(currentCycle, ref renderArray);

                LightUpArray(currentCycle, sprintPosition, ref renderArray);

                UpdatePosition(ref upcomingCommands, ref sprintPosition, ref currentPosition);
            }

            Console.WriteLine(signalStrengths.Values.Sum());
        }

        private static void AddCommand(string incomingCommand, ref LinkedList<(int key, string value)> upcomingCommands)
        {
            var incomingCommandValue = incomingCommand.Split(' ');

            if (incomingCommandValue[0] == ADD_X_NAME)
            {
                upcomingCommands.AddLast((ADD_X_CYCLES, incomingCommand));
            }
            else if (incomingCommandValue[0] == NOOP_NAME)
            {
                upcomingCommands.AddLast((NOOP_CYCLES, incomingCommand));
            }
        }

        private static void AddSignalStrength(int currentCycle, int currentPosition, ref Dictionary<int, int> signalStrengths)
        {
            if (currentCycle == FIRST_ROUND_SIZE || (currentCycle - FIRST_ROUND_SIZE) % INCREMENT_SIZE == 0)
            {
                signalStrengths[currentCycle] = currentCycle * currentPosition;
            }
        }

        private static void RenderAndResetLine(int currentCycle, ref string[] renderArray)
        {
            if (currentCycle % INCREMENT_SIZE == 0)
            {
                Array.ForEach(renderArray, Console.Write);
                Console.WriteLine();
                renderArray = Enumerable.Repeat(" ", INCREMENT_SIZE).ToArray();
            }
        }

        private static void LightUpArray(int currentCycle, int[] sprintPosition, ref string[] renderArray)
        {
            var currentRow = currentCycle % INCREMENT_SIZE;
            if (sprintPosition.Contains(currentRow))
            {
                if (currentRow - 1 >= 0) renderArray[currentRow - 1] = LIGHT_UP;
            }
        }

        private static void UpdatePosition(ref LinkedList<(int key, string value)> upcomingCommands, ref int[] sprintPosition, ref int currentPosition)
        {
            var upcomingCommand = upcomingCommands.First();

            if (upcomingCommand.key == 1)
            {
                var upcomingCommandValues = upcomingCommand.value.Split(' ');

                if (upcomingCommandValues[0] == ADD_X_NAME)
                {
                    currentPosition += Int32.Parse(upcomingCommandValues[1]);
                    sprintPosition = new int[3] { currentPosition, currentPosition + 1, currentPosition + 2 };
                }

                upcomingCommands.RemoveFirst();
            }
            else
            {
                upcomingCommands.First.Value = (upcomingCommand.key - 1, upcomingCommand.value);
            }
        }
    }
}
