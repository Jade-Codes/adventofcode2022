using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge6
    {
        public static void Part1And2(string input, int queueSize = 4)
        {
            var inputCharArray = input.ToCharArray();
            var queue = new Queue<char>();
            var currentIndex = 0;

            for (var i = 0; i < inputCharArray.Length; i++)
            {
                queue.Enqueue(inputCharArray[i]);

                if (queue.Count() > queueSize)
                {
                    queue.Dequeue();
                }

                currentIndex++;

                var distinctValues = queue.Distinct();

                if (distinctValues.Count() == queueSize)
                {
                    break;
                }
            }

            Console.WriteLine(currentIndex);
        }
    }
}