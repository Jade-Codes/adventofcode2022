using Newtonsoft.Json.Linq;

namespace Challenges
{
    public class Challenge13
    {
        public static void Part1(IEnumerable<string> lines)
        {
            var pairs = new List<(dynamic part1, dynamic part2)>();
            dynamic currentPart1 = null;
            dynamic currentPart2 = null;

            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    currentPart1 = null;
                    currentPart2 = null;
                }
                else if (currentPart1 == null)
                {
                    currentPart1 = JArray.Parse(line);
                }
                else if (currentPart2 == null)
                {
                    currentPart2 = JArray.Parse(line);
                    pairs.Add((currentPart1, currentPart2));
                }
            }

            var pairsArray = pairs.ToArray();
            var indexDictionary = new Dictionary<int, int>();

            for (var i = 0; i < pairsArray.Length; i++)
            {
                var isRightOrder = CompareObjects(pairsArray[i].part1, pairsArray[i].part2);
                indexDictionary.Add(i + 1, isRightOrder);
            }

            Console.WriteLine(indexDictionary.Where(_ => _.Value < 0).Sum(_ => _.Key));
        }

        public static void Part2(IEnumerable<string> lines)
        {
            var arrays = new List<dynamic>();
            var divisors = new List<dynamic>()
            {
                JArray.Parse("[[2]]"),
                JArray.Parse("[[6]]")
            };

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    arrays.Add(JArray.Parse(line));
                }
            }

            arrays.AddRange(divisors);
            arrays.Sort(CompareObjects);

            Console.WriteLine((arrays.IndexOf(divisors[0]) + 1) * (arrays.IndexOf(divisors[1]) + 1));
        }

        private static int CompareObjects(dynamic leftObject, dynamic rightObject)
        {
            if (leftObject is JValue && rightObject is JValue)
            {
                return leftObject.ToObject<int>() - rightObject.ToObject<int>();
            }

            var leftObjectArray = leftObject as JArray ?? new JArray(leftObject.ToObject<int>());
            var rightObjectArray = rightObject as JArray ?? new JArray(rightObject.ToObject<int>());

            var tupleArray = leftObjectArray.Zip(rightObjectArray);

            foreach (var tuple in tupleArray)
            {
                var value = CompareObjects(tuple.First, tuple.Second);
                if (value != 0)
                {
                    return value;
                }
            }

            return leftObjectArray.Count - rightObjectArray.Count;
        }
    }
}
