namespace Challenges
{
    public class Challenge1
    {
        public static void Part1And2(IEnumerable<string> lines)
        {
            var currentNumber = 0;

            var maxNumber = 0;

            var array = new List<int>();

            foreach (var line in lines)
            {
                var successfullyParsed = int.TryParse(line, out var newNumber);
                if (successfullyParsed)
                {
                    currentNumber += newNumber;
                }
                else
                {

                    array.Add(currentNumber);
                    if (currentNumber > maxNumber)
                    {
                        maxNumber = currentNumber;
                    }
                    currentNumber = 0;
                }
            }

            array.Sort();
            array.Reverse();
            array = array.Take(3).ToList();

            int total = array.Sum();

            foreach (var value in array)
            {
                Console.WriteLine("Elf Calories: " + value);
            }
            Console.WriteLine();
            Console.WriteLine("Total Calories: " + total);
        }

    }
}