using System.Numerics;

namespace Challenges
{
    public class Challenge25
    {
        public static void Part1(IEnumerable<string> lines)
        {
            List<BigInteger> values = new List<BigInteger>();
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

            var total = values.Aggregate(BigInteger.Add);
            var currentTotal = total;
            var snafulist = new List<string>();

            while (currentTotal > 0)
            {

                if (currentTotal % 5 == 3)
                {
                    snafulist.Add("=");
                    currentTotal += 5;
                }
                else if (currentTotal % 5 == 4)
                {
                    snafulist.Add("-");
                    currentTotal += 5;
                }
                else
                {

                    snafulist.Add((currentTotal % 5).ToString());
                }
                currentTotal /= 5;

            }

            snafulist.Reverse();

            Console.Write(string.Join("", snafulist));
        }
    }
}
