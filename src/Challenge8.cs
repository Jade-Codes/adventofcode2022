namespace Challenges
{
    public class Challenge8
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var linesArray = lines.ToArray();
            var matrix = new int[linesArray.Length][];
            var isVisibleCounter = 0;

            for (var i = 0; i < linesArray.Length; i++)
            {
                matrix[i] = linesArray[i].ToCharArray().Select(a => a - '0').ToArray();
            }

            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    if (i == 0 ||
                        j == 0 ||
                        i == matrix[i].Length - 1 ||
                        j == matrix[j].Length - 1)
                    {
                        isVisibleCounter++;
                    }
                    else if (CalculateTop(i, j, matrix).Item1 ||
                        CalculateBottom(i, j, matrix).Item1 ||
                        CalculateLeft(j, matrix[i]).Item1 ||
                        CalculateRight(j, matrix[i]).Item1)
                    {
                        isVisibleCounter++;
                    }
                }
            }

            Console.WriteLine(isVisibleCounter);
        }

        public static void Part2(IEnumerable<string> lines)
        {            
            var linesArray = lines.ToArray();
            var matrix = new int[linesArray.Length][];
            var highestScenicView = 0;

            for (var i = 0; i < linesArray.Length; i++)
            {
                matrix[i] = linesArray[i].ToCharArray().Select(a => a - '0').ToArray();
            }

            for (var i = 0; i < matrix.Length; i++)
            {
                for (var j = 0; j < matrix[i].Length; j++)
                {
                    if (i != 0 &&
                        j != 0 &&
                        i != matrix[i].Length - 1 &&
                        j != matrix[j].Length - 1)
                    {
                        var topScenicView = CalculateTop(i, j, matrix).Item2;
                        var bottomScenicView = CalculateBottom(i, j, matrix).Item2;
                        var leftScenicView = CalculateLeft(j, matrix[i]).Item2;
                        var rightScenicView = CalculateRight(j, matrix[i]).Item2;
                        var currentScenicView = topScenicView * bottomScenicView * leftScenicView * rightScenicView;

                        if(currentScenicView > highestScenicView) {
                            highestScenicView = currentScenicView;
                        }
                    }
                }
            }

            Console.WriteLine(highestScenicView);
        }

        public static Tuple<bool, int> CalculateTop(int x, int y, int[][] matrix)
        {
            var isVisible = true;
            var counter = x - 1;

            while (isVisible)
            {
                if (matrix[counter][y] >= matrix[x][y])
                {
                    isVisible = false;
                    break;
                }

                if (counter == 0)
                {
                    break;
                }
                counter--;
            }
            return new Tuple<bool, int>(isVisible, x - counter);
        }

        public static Tuple<bool, int> CalculateBottom(int x, int y, int[][] matrix)
        {
            var isVisible = true;
            var counter = x + 1;

            while (isVisible)
            {
                if (matrix[counter][y] >= matrix[x][y])
                {
                    isVisible = false;
                    break;
                }
                if (counter == matrix.Length - 1)
                {
                    break;
                }
                counter++;
            }

            return new Tuple<bool, int>(isVisible, counter - x);
        }


        public static Tuple<bool, int> CalculateLeft(int y, int[] array)
        {
            var isVisible = true;
            var counter = y - 1;

            while (isVisible)
            {
                if (array[counter] >= array[y])
                {
                    isVisible = false;
                    break;
                }
                if (counter == 0)
                {
                    break;
                }
                counter--;
            }

            return new Tuple<bool, int>(isVisible, y - counter);
        }

        public static Tuple<bool, int> CalculateRight(int y, int[] array)
        {
            var isVisible = true;
            var counter = y + 1;

            while (isVisible)
            {
                if (array[counter] >= array[y])
                {
                    isVisible = false;
                    break;
                }
                if (counter == array.Length - 1)
                {
                    break;
                }
                counter++;
            }
            return new Tuple<bool, int>(isVisible, counter - y);
        }
    }
}