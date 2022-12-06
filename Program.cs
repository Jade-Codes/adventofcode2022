namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("data\\challenge6.txt");
            Challenge6.Part1And2(input, 14);
        }

    }
}