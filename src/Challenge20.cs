using System.Text.RegularExpressions;

namespace Challenges
{
    public class Challenge20
    {
        public static void Part1(IEnumerable<string> lines)
        {
            Parse(lines, out var data, 1);   
            Decode(ref data, 1);
            Print(data);
        }

        public static void Part2(IEnumerable<string> lines)
        {
            Parse(lines, out var data, 811589153);   
            Decode(ref data, 10);
            Print(data);
        }

        private static void Parse(IEnumerable<string> lines, out List<Data> data, int multiplier)
        {
            var integers = lines.Select(long.Parse).ToArray();
            data = new List<Data>();

            for (var i = 0; i < integers.Count(); i++)
            {
                data.Add(new Data(integers[i]*multiplier, i));
            }   
        }

        private static void Decode(ref List<Data> data, int iterations)
        {
            for (var i = 0; i < iterations; i++)
            {
                for (var j = 0; j < data.Count(); j++)
                {
                    var currentIndex = data.FindIndex(x => x.OriginalIndex == j);
                    var currentValue = data[currentIndex];
                    var nextIndex = (currentIndex + currentValue.Value) % (data.Count() - 1);

                    if (nextIndex < 0)
                    {
                        nextIndex += data.Count() - 1;
                    }

                    data.RemoveAt(currentIndex);
                    data.Insert((int)nextIndex, currentValue);
                }
            }
        }

        private static void Print(List<Data> data)
        {
            var zeroIndex = data.FindIndex(x => x.Value == 0);

            var afterOneThousand = data[(zeroIndex + 1000) % data.Count].Value;
            var afterTwoThousand = data[(zeroIndex + 2000) % data.Count].Value;
            var afterThreeThousand = data[(zeroIndex + 3000) % data.Count].Value;

            Console.WriteLine(afterOneThousand + afterTwoThousand + afterThreeThousand);
        }

        internal class Data
        {
            public int OriginalIndex { get; set; }
            public long Value { get; set; }

            public Data(long value, int originalIndex)
            {
                Value = value;
                OriginalIndex = originalIndex;
            }
        }
    }
}
