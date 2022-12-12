using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge12
    {
        public static Dictionary<char, int> NumbersMap = new Dictionary<char, int> {
                {'S', 1},
                {'a', 1},
                {'b', 2},
                {'c', 3},
                {'d', 4},
                {'e', 5},
                {'f', 6},
                {'g', 7},
                {'h', 8},
                {'i', 9},
                {'j', 10},
                {'k', 11},
                {'l', 12},
                {'m', 13},
                {'n', 14},
                {'o', 15},
                {'p', 16},
                {'q', 17},
                {'r', 18},
                {'s', 19},
                {'t', 20},
                {'u', 21},
                {'v', 22},
                {'w', 23},
                {'x', 24},
                {'y', 25},
                {'z', 26},
                {'E', 26},
            };

        public static void Part1(IEnumerable<string> lines)
        {
            PopulateMatrix(lines, out var matrix);
            AddStartPositions('S', matrix, out var checkedPositions, out var nextPositions);
            CalculateShortestDistance(matrix, checkedPositions, nextPositions, out var shortestDistance);

            Console.WriteLine(shortestDistance);
        }

        public static void Part2(IEnumerable<string> lines)
        {

            PopulateMatrix(lines, out var matrix);
            AddStartPositions('a', matrix, out var checkedPositions, out var nextPositions);
            CalculateShortestDistance(matrix, checkedPositions, nextPositions, out var shortestDistance);

            Console.WriteLine(shortestDistance);

        }

        private static void PopulateMatrix(IEnumerable<string> lines, out char[][] matrix)
        {
            var linesArray = lines.ToArray();
            matrix = new char[linesArray.Length][];

            for (var i = 0; i < linesArray.Length; i++)
            {
                matrix[i] = linesArray[i].ToCharArray();
            }
        }

        private static void AddStartPositions(char start, char[][] matrix, out List<(int x, int y)> checkedPositions, out List<(int x, int y)> nextPositions)
        {
            nextPositions = new List<(int x, int y)>();
            checkedPositions = new List<(int x, int y)>();

            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    if (matrix[i][j] == start)
                    {
                        AddPositions(i, j, matrix, ref checkedPositions, ref nextPositions);
                    }
                }
            }
        }

        private static void AddPositions(int i, int j, char[][] matrix, ref List<(int x, int y)> checkedPositions, ref List<(int x, int y)> nextPositions)
        {
            if (!checkedPositions.Contains((i, j)))
            {
                checkedPositions.Add((i, j));
            }
            else
            {
                return;
            }

            if (j < matrix[i].Length - 1 &&
                NumbersMap[matrix[i][j + 1]] <= NumbersMap[matrix[i][j]] + 1 &&
                !checkedPositions.Contains((i, j + 1)) &&
                !nextPositions.Contains((i, j + 1)))
            {
                nextPositions.Add((i, j + 1));
            }

            if (j > 0 &&
                NumbersMap[matrix[i][j - 1]] <= NumbersMap[matrix[i][j]] + 1 &&
                !checkedPositions.Contains((i, j - 1)) &&
                !nextPositions.Contains((i, j - 1)))
            {
                nextPositions.Add((i, j - 1));
            }

            if (i < matrix.Length - 1 &&
                NumbersMap[matrix[i + 1][j]] <= NumbersMap[matrix[i][j]] + 1 &&
                !checkedPositions.Contains((i + 1, j)) &&
                !nextPositions.Contains((i + 1, j)))
            {
                nextPositions.Add((i + 1, j));
            }

            if (i > 0 &&
                NumbersMap[matrix[i - 1][j]] <= NumbersMap[matrix[i][j]] + 1 &&
                !checkedPositions.Contains((i - 1, j)) &&
                !nextPositions.Contains((i - 1, j)))
            {
                nextPositions.Add((i - 1, j));
            }

            nextPositions.Remove((i, j));
        }

        public static void CalculateShortestDistance(char[][] matrix, List<(int x, int y)> checkedPositions, List<(int x, int y)> nextPositions, out int shortestDistance)
        {
            var stillGoing = true;
            shortestDistance = 0;

            while (stillGoing)
            {
                var currentPositions = new List<(int x, int y)>();
                shortestDistance++;
                
                foreach (var nextPosition in nextPositions)
                {
                    if (matrix[nextPosition.x][nextPosition.y] == 'E')
                    {
                        stillGoing = false;
                        break;
                    }
                    AddPositions(nextPosition.x, nextPosition.y, matrix, ref checkedPositions, ref currentPositions);
                }
                nextPositions = currentPositions;
            }
        }
    }
}
