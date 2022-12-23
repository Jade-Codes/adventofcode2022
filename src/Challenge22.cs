using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge22
    {
        private const string DIGIT_WORD_PATTERN = @"\D+\d+";
        private const string DIGIT_PATTERN = @"\d+";
        private const string NON_DIGIT_PATTERN = @"\D+";
        private static Dictionary<char, int> directions = new Dictionary<char, int>()
        {
            { 'R', 0},
            { 'D', 1},
            { 'L', 2},
            { 'U', 3}
        };
        private static Dictionary<char, char> rightDirections = new Dictionary<char, char>()
        {
            { 'L', 'U'},
            { 'U', 'R'},
            { 'R', 'D'},
            { 'D', 'L'}
        };
        private static Dictionary<char, char> leftDirections = new Dictionary<char, char>()
        {
            { 'L', 'D'},
            { 'D', 'R'},
            { 'R', 'U'},
            { 'U', 'L'}
        };

        public static void Part1(IEnumerable<string> lines)
        {
            var instructions = new List<(int distance, char direction)>();
            var pathCoordinates = new HashSet<(int x, int y)>();
            var blockedCoordinates = new HashSet<(int x, int y)>();
            var visitedPositions = new HashSet<(int x, int y)>();

            Parse(lines, ref instructions, ref pathCoordinates, ref blockedCoordinates);

            var currentPosition = pathCoordinates.OrderBy(_ => _.y).First();
            var allCoordinates =new HashSet<(int x, int y)>(pathCoordinates.Concat(blockedCoordinates));
            visitedPositions.Add(currentPosition);
            char? currentDirection = null;

            foreach (var instruction in instructions)
            {
                if (currentDirection == null)
                {
                    currentDirection = instruction.direction;
                }
                else
                {

                    switch (instruction.direction)
                    {
                        case 'L':
                            currentDirection = leftDirections[currentDirection.Value];
                            break;
                        case 'R':
                            currentDirection = rightDirections[currentDirection.Value];
                            break;
                    }
                }

                var movements = instruction.distance;
                while (movements > 0)
                {
                    (int x, int y) tempPostion = (currentPosition.x, currentPosition.y);
                    switch (currentDirection)
                    {
                        case 'L':
                            tempPostion = (currentPosition.x - 1, currentPosition.y);
                            break;
                        case 'U':
                            tempPostion = (currentPosition.x, currentPosition.y - 1);
                            break;
                        case 'R':
                            tempPostion = (currentPosition.x + 1, currentPosition.y);
                            break;
                        case 'D':
                            tempPostion = (currentPosition.x, currentPosition.y + 1);
                            break;
                    }
                    if (pathCoordinates.Contains(tempPostion))
                    {
                        currentPosition = tempPostion;
                    }
                    else if (blockedCoordinates.Contains(tempPostion))
                    {
                        movements = 0;
                        break;
                    }
                    else
                    {
                        switch (currentDirection)
                        {
                            case 'L':
                                var x = allCoordinates.Where(_ => _.y == currentPosition.y).Max(_ => _.x);
                                tempPostion = (x, currentPosition.y);
                                break;
                            case 'U':
                                var y = allCoordinates.Where(_ => _.x == currentPosition.x).Max(_ => _.y);
                                tempPostion = (currentPosition.x, y);
                                break;
                            case 'R':
                                x = allCoordinates.Where(_ => _.y == currentPosition.y).Min(_ => _.x);
                                tempPostion = (x, currentPosition.y);
                                break;
                            case 'D':
                                y = allCoordinates.Where(_ => _.x == currentPosition.x).Min(_ => _.y);
                                tempPostion = (currentPosition.x, y);
                                break;
                        }
                        if (blockedCoordinates.Contains(tempPostion))
                        {
                            movements = 0;
                            break;
                        }
                        else
                        {
                            currentPosition = tempPostion;
                        }
                    }

                    movements--;
                }

            }
            Console.WriteLine((1000*currentPosition.y) + (4*currentPosition.x) + directions[currentDirection.Value]);
        }

        public static void Part2(IEnumerable<string> lines)
        {
        }

        private static void Parse(IEnumerable<string> lines,
            ref List<(int distance, char direction)> instructions,
            ref HashSet<(int x, int y)> pathCoordinates,
            ref HashSet<(int x, int y)> blockedCoordinates)
        {
            var linesArray = lines.ToArray();
            for (var i = 1; i <= linesArray.Length; i++)
            {
                if (linesArray[i - 1].Contains('.'))
                {
                    var charArray = linesArray[i - 1].ToCharArray();
                    for (var j = 1; j <= charArray.Length; j++)
                    {
                        if (charArray[j - 1] == '.')
                        {
                            pathCoordinates.Add((j, i));
                        }
                        else if (charArray[j - 1] == '#')
                        {
                            blockedCoordinates.Add((j, i));
                        }
                    }
                }
                else
                {
                    var currentInstructions = Regex.Matches(linesArray[i - 1], DIGIT_WORD_PATTERN).Select(_ => _.Value).ToList();

                    if (currentInstructions.Count() > 0)
                    {
                        var firstDistance = Int32.Parse(Regex.Match(linesArray[i - 1], DIGIT_PATTERN).Value);
                        instructions.Add((firstDistance, 'R'));

                        foreach (var instruction in currentInstructions)
                        {
                            var distance = Int32.Parse(Regex.Match(instruction, DIGIT_PATTERN).Value);
                            var direction = Regex.Match(instruction, NON_DIGIT_PATTERN).Value.ToCharArray()[0]; ;
                            instructions.Add((distance, direction));
                        }
                    }
                }
            }
        }
    }
}
