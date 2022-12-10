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
                    else if (CalculateTop(i, j, matrix).visible ||
                        CalculateBottom(i, j, matrix).visible ||
                        CalculateLeft(j, matrix[i]).visible ||
                        CalculateRight(j, matrix[i]).visible)
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
                        var topScenicView = CalculateTop(i, j, matrix).view;
                        var bottomScenicView = CalculateBottom(i, j, matrix).view;
                        var leftScenicView = CalculateLeft(j, matrix[i]).view;
                        var rightScenicView = CalculateRight(j, matrix[i]).view;
                        var currentScenicView = topScenicView * bottomScenicView * leftScenicView * rightScenicView;

                        if(currentScenicView > highestScenicView) {
                            highestScenicView = currentScenicView;
                        }
                    }
                }
            }

            Console.WriteLine(highestScenicView);
        }

        public static (bool visible, int view) CalculateTop(int x, int y, int[][] matrix)
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
            return (isVisible, x - counter);
        }

        public static (bool visible, int view) CalculateBottom(int x, int y, int[][] matrix)
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

            return (isVisible, counter - x);
        }


        public static (bool visible, int view) CalculateLeft(int y, int[] array)
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

            return (isVisible, y - counter);
        }

        public static (bool visible, int view) CalculateRight(int y, int[] array)
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
            return (isVisible, counter - y);
        }
    }
}