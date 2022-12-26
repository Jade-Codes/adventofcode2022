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
        private static Dictionary<int, char> faces = new Dictionary<int, char>()
        {
            { 1, 'A'},
            { 2, 'B'},
            { 3, 'C'},
            { 4, 'D'},
            { 5, 'E'},
            { 6, 'F'}
        };
        
        private static Dictionary<(char face, int c1, int c2), (char face, int c1, int c2)> faceToFace = new Dictionary<(char face, int c1, int c2), (char, int c1, int c2)>()
        {
            {('A', 0, 1), ('F', 0, 2)},
            {('A', 0, 2), ('D', 2, 0)},
            {('A', 1, 3), ('B', 0, 2)},
            {('A', 2, 3), ('C', 0, 1)},

            {('B', 0, 1), ('F', 2, 3)},
            {('B', 0, 2), ('A', 1, 3)},
            {('B', 1, 3), ('E', 3, 1)},
            {('B', 2, 3), ('C', 1, 3)},

            {('C', 0, 1), ('A', 2, 3)},
            {('C', 0, 2), ('D', 0, 1)},
            {('C', 1, 3), ('B', 2, 3)},
            {('C', 2, 3), ('E', 0, 1)},

            {('D', 0, 1), ('C', 0, 2)},
            {('D', 0, 2), ('A', 2, 0)},
            {('D', 1, 3), ('E', 0, 2)},
            {('D', 2, 3), ('F', 0, 1)},

            {('E', 0, 1), ('C', 2, 3)},
            {('E', 0, 2), ('D', 1, 3)},
            {('E', 1, 3), ('B', 3, 1)},
            {('E', 2, 3), ('F', 1, 3)},

            {('F', 0, 1), ('D', 2, 3)},
            {('F', 0, 2), ('A', 0, 1)},
            {('F', 1, 3), ('E', 2, 3)},
            {('F', 2, 3), ('B', 0, 1)}
        };

        public static void Part1(IEnumerable<string> lines)
        {
            var instructions = new List<(int distance, char direction)>();
            var pathCoordinates = new HashSet<(int x, int y)>();
            var blockedCoordinates = new HashSet<(int x, int y)>();
            var allCoordinates = new HashSet<(int x, int y)>();
            var visitedPositions = new HashSet<(int x, int y)>();

            Parse(lines, ref instructions, ref pathCoordinates, ref blockedCoordinates, ref allCoordinates);

            var currentPosition = pathCoordinates.OrderBy(_ => _.y).First();
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
            Console.WriteLine((1000 * currentPosition.y) + (4 * currentPosition.x) + directions[currentDirection.Value]);
        }

        public static void Part2(IEnumerable<string> lines, int length = 50)
        {
            var instructions = new List<(int distance, char direction)>();
            var pathCoordinates = new HashSet<(int x, int y)>();
            var blockedCoordinates = new HashSet<(int x, int y)>();
            var allCoordinates = new HashSet<(int x, int y)>();
            var visitedPositions = new HashSet<(int x, int y)>();

            Parse(lines, ref instructions, ref pathCoordinates, ref blockedCoordinates, ref allCoordinates);

            var currentX = 1;
            var currentY = 1;
            var currentFace = 1;

            var tempAllCoorinates = new HashSet<(int x, int y)>(allCoordinates);
            var faceCoordinates = new Dictionary<char, HashSet<(int x, int y)>>();

            while (tempAllCoorinates.Count > 0)
            {
                var blockCoordinates = tempAllCoorinates.Where(_ =>
                    _.x >= currentX &&
                    _.x <= currentX + length - 1 &&
                    _.y >= currentY &&
                    _.y <= currentY + length - 1).ToHashSet();

                if (blockCoordinates.Any())
                {
                    faceCoordinates.Add(faces[currentFace], blockCoordinates);
                    tempAllCoorinates = tempAllCoorinates.Except(blockCoordinates).ToHashSet();
                    currentFace++;
                }

                currentX = (currentX + length > allCoordinates.Max(_ => _.x)) ? 1 : currentX + length;
                currentY = (currentX == 1) ? currentY + length : currentY;
            }

            char? currentDirection = null;
            var currentPosition = pathCoordinates.OrderBy(_ => _.y).First();

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
                        var tempDirection = currentDirection;

                        switch (currentDirection)
                        {
                            case 'L':
                                var previousFace = faceCoordinates.FirstOrDefault(_ => _.Value.Contains(currentPosition));
                                var adjoiningSide = faceToFace[(previousFace.Key, 0, 2)];
                                var nextFace = faceCoordinates.FirstOrDefault(_ => _.Key == adjoiningSide.face);
                                
                                CalculatePosition(adjoiningSide, previousFace.Value, nextFace.Value, currentPosition, ref tempPostion, ref tempDirection);
                                break;
                            case 'U':
                                previousFace = faceCoordinates.FirstOrDefault(_ => _.Value.Contains(currentPosition));
                                adjoiningSide = faceToFace[(previousFace.Key, 0, 1)];
                                nextFace = faceCoordinates.FirstOrDefault(_ => _.Key == adjoiningSide.face);
                                
                                CalculatePosition(adjoiningSide, previousFace.Value, nextFace.Value, currentPosition, ref tempPostion, ref tempDirection);
                                break;
                            case 'R':
                                previousFace = faceCoordinates.FirstOrDefault(_ => _.Value.Contains(currentPosition));
                                adjoiningSide = faceToFace[(previousFace.Key, 1, 3)];
                                nextFace = faceCoordinates.FirstOrDefault(_ => _.Key == adjoiningSide.face);
                                
                                CalculatePosition(adjoiningSide, previousFace.Value, nextFace.Value, currentPosition, ref tempPostion, ref tempDirection);
                                break;
                            case 'D':
                                previousFace = faceCoordinates.FirstOrDefault(_ => _.Value.Contains(currentPosition));
                                adjoiningSide = faceToFace[(previousFace.Key, 2, 3)];
                                nextFace = faceCoordinates.FirstOrDefault(_ => _.Key == adjoiningSide.face);
                                
                                CalculatePosition(adjoiningSide, previousFace.Value, nextFace.Value, currentPosition, ref tempPostion, ref tempDirection);
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
                            currentDirection = tempDirection;
                        }
                    }

                    movements--;
                }
            }
            
            Console.WriteLine((1000 * currentPosition.y) + (4 * currentPosition.x) + directions[currentDirection.Value]);
        }

        private static void CalculatePosition((char face, int c1, int c2) adjoiningSide, 
            HashSet<(int x, int y)> prevFaceCoordinates, 
            HashSet<(int x, int y)> currFaceCoordinates,
            (int x, int y) prevCoordinates,
            ref (int x, int y) nextCoordinates,
            ref char? currentDirection)
        {
            if (adjoiningSide.c1 == 0 && adjoiningSide.c2 == 1)
            {
                var x = currFaceCoordinates.Min(_ => _.x);
                var y = currFaceCoordinates.Min(_ => _.y);
                
                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    x =  x + (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    x =  x + (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }

                nextCoordinates = (x, y);
                currentDirection = 'D';
            }
            else if (adjoiningSide.c1 == 1 && adjoiningSide.c2 == 0)
            { 
                var x = currFaceCoordinates.Max(_ => _.x);
                var y = currFaceCoordinates.Min(_ => _.y);

                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    x =  x - (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    x =  x - (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }

                nextCoordinates = (x, y);
                currentDirection = 'D';
            }
            else if (adjoiningSide.c1 == 0 && adjoiningSide.c2 == 2)
            {
                var x = currFaceCoordinates.Min(_ => _.x);
                var y = currFaceCoordinates.Min(_ => _.y);

                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    y =  y + (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    y =  y + (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }

                nextCoordinates = (x, y);
                currentDirection = 'R';
            }
            else if (adjoiningSide.c1 == 2 && adjoiningSide.c2 == 0)
            {
                var x = currFaceCoordinates.Min(_ => _.x);
                var y = currFaceCoordinates.Max(_ => _.y);

                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    y =  y - (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    y =  y - (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }

                nextCoordinates = (x, y);
                currentDirection = 'R';

            }
            else if (adjoiningSide.c1 == 1 && adjoiningSide.c2 == 3)
            {
                var x = currFaceCoordinates.Max(_ => _.x);
                var y = currFaceCoordinates.Min(_ => _.y);

                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    y =  y + (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    y =  y + (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }

                nextCoordinates = (x, y);
                currentDirection = 'L';
            }
            else if (adjoiningSide.c1 == 3 && adjoiningSide.c2 == 1)
            {
                var x = currFaceCoordinates.Max(_ => _.x);
                var y = currFaceCoordinates.Max(_ => _.y);

                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    y =  y - (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    y =  y - (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }

                nextCoordinates = (x, y);
                currentDirection = 'L';
            }
            else if (adjoiningSide.c1 == 2 && adjoiningSide.c2 == 3)
            {
                var x = currFaceCoordinates.Min(_ => _.x);
                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    x =  x + (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    x =  x + (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }

                var y = currFaceCoordinates.Max(_ => _.y);
                nextCoordinates = (x, y);
                currentDirection = 'U';
            }
            else if (adjoiningSide.c1 == 3 && adjoiningSide.c2 == 2)
            {
                var x = currFaceCoordinates.Max(_ => _.x);
                if (currentDirection == 'L' || currentDirection == 'R')
                {
                    x =  x - (prevCoordinates.y - prevFaceCoordinates.Min(_ => _.y));
                }
                else
                {
                    x =  x - (prevCoordinates.x - prevFaceCoordinates.Min(_ => _.x));
                }
                var y = currFaceCoordinates.Max(_ => _.y);
                nextCoordinates = (x, y);
                currentDirection = 'U';
            }
        }

        private static void Parse(IEnumerable<string> lines,
            ref List<(int distance, char direction)> instructions,
            ref HashSet<(int x, int y)> pathCoordinates,
            ref HashSet<(int x, int y)> blockedCoordinates,
            ref HashSet<(int x, int y)> allCoordinates)
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
                            allCoordinates.Add((j, i));
                        }
                        else if (charArray[j - 1] == '#')
                        {
                            blockedCoordinates.Add((j, i));
                            allCoordinates.Add((j, i));
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
