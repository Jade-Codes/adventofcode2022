using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge5
    {
        public static void Part1(IEnumerable<string> lines, int rowIndex = 8, int columnIndex = 9)
        {
            GetArrayMatrix(lines, rowIndex, columnIndex, out var arrayMatrix);
            GetStackList(arrayMatrix, out var stackList);
            GetActionsList(lines, arrayMatrix, out var actionList);

            var stackArray = stackList.ToArray();
            foreach (var action in actionList)
            {
                var previousStack = stackArray[action[1] - 1];
                var currentStack = stackArray[action[2] - 1];

                for (var i = 0; i < action[0]; i++)
                {
                    var valueToMove = previousStack.Peek();
                    currentStack.Push(valueToMove);
                    previousStack.Pop();

                    stackArray[action[1] - 1] = previousStack;
                    stackArray[action[2] - 1] = currentStack;
                }
            }

            for (var i = 0; i < stackArray.Length; i++)
            {
                Console.Write(Regex.Replace(stackArray[i].FirstOrDefault() ?? "", @"\W+", ""));
            }
        }

        public static void Part2(IEnumerable<string> lines, int rowIndex = 8, int columnIndex = 9)
        {
            GetArrayMatrix(lines, rowIndex, columnIndex, out var arrayMatrix);
            GetStackList(arrayMatrix, out var stackList);
            GetActionsList(lines, arrayMatrix, out var actionList);

            var stackArray = stackList.ToArray();
            foreach (var action in actionList)
            {
                var previousStack = stackArray[action[1] - 1];
                var currentStack = stackArray[action[2] - 1];

                var valuesToMove = new List<string>();

                for (var i = 0; i < action[0]; i++)
                {
                    var valueToMove = previousStack.Peek();
                    valuesToMove.Add(valueToMove);
                    previousStack.Pop();

                    stackArray[action[1] - 1] = previousStack;
                }

                valuesToMove.Reverse();

                foreach (var valueToMove in valuesToMove)
                {
                    currentStack.Push(valueToMove);
                    stackArray[action[2] - 1] = currentStack;
                }
            }

            for (var i = 0; i < stackArray.Length; i++)
            {
                Console.Write(Regex.Replace(stackArray[i].FirstOrDefault() ?? "", @"\W+", ""));
            }
        }

        private static void GetArrayMatrix(IEnumerable<string> lines, int rowIndex, int columnIndex, out string[][] arrayMatrix)
        {
            var linesArray = lines.ToArray();
            arrayMatrix = new string[columnIndex][];

            for (var i = 0; i < rowIndex; i++)
            {
                var charArrayMatrix = linesArray[i].Chunk(4).ToArray();
                for (var j = 0; j < columnIndex; j++)
                {
                    var currentString = string.Join("", charArrayMatrix[j]);

                    if (!string.IsNullOrWhiteSpace(currentString))
                    {
                        if (arrayMatrix[j] is null)
                        {
                            arrayMatrix[j] = new string[8];
                        }
                        arrayMatrix[j][i] = currentString;
                    }
                }
            }
        }

        private static void GetStackList(string[][] arrayMatrix, out List<Stack<string>> stackList)
        {
            stackList = new List<Stack<string>>();

            for (var i = 0; i < arrayMatrix.Length; i++)
            {
                var list = arrayMatrix[i].Where(_ => !string.IsNullOrEmpty(_)).ToList();
                list.Reverse();
                stackList.Add(new Stack<string>(list));
            }
        }

        
        private static void GetActionsList(IEnumerable<string> lines, string[][] arrayMatrix, out List<int[]> actionList)
        {   
            var linesArray = lines.ToArray();
            actionList = new List<int[]>();
            
            for (var i = arrayMatrix.Length + 1; i < linesArray.Length; i++)
            {
                var numbers = Regex.Split(linesArray[i], @"\D+").Where(_ => !string.IsNullOrEmpty(_)).Select(Int32.Parse).ToList().ToArray();
                actionList.Add(numbers);
            }
        }
    }
}