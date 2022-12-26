using System.Numerics;

namespace Challenges
{
    public class Challenge25
    {
        public static void Part1(IEnumerable<string> lines)
        {
            CalculateDecimal(lines, out var values);

            var total = values.Aggregate(BigInteger.Add);
            
            CalculateSnafu(total, out var snafuList);

            Console.Write(string.Join("", snafuList));
        }

        private static void CalculateDecimal(IEnumerable<string> lines, out List<BigInteger> values)
        {
            values = new List<BigInteger>();
            foreach (var line in lines)
            {
                var currentValue = new BigInteger(0);
                var charArray = line.ToCharArray().Reverse().ToArray();
                for (var i = 0; i < charArray.Length; i++)
                {
                    var number = BigInteger.Pow(5, i);
                    var isNumber = BigInteger.TryParse(charArray[i].ToString(), out var value);

                    if (isNumber)
                    {
                        currentValue += number * value;
                    }
                    else if (charArray[i] == '=')
                    {
                        currentValue -= number * 2;
                    }
                    else if (charArray[i] == '-')
                    {
                        currentValue -= number;
                    }
                }
                values.Add(currentValue);
            }
        }

        private static void CalculateSnafu(BigInteger total, out List<string> snafuList)
        {
            snafuList = new List<string>();
            var currentTotal = total;
            
            while (currentTotal > 0)
            {
                if (currentTotal % 5 == 3)
                {
                    snafuList.Add("=");
                    currentTotal += 5;
                }
                else if (currentTotal % 5 == 4)
                {
                    snafuList.Add("-");
                    currentTotal += 5;
                }
                else
                {
                    snafuList.Add((currentTotal % 5).ToString());
                }
                currentTotal /= 5;
            }

            snafuList.Reverse();
        }
    }
}
