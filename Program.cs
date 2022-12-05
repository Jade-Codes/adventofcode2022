namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadLines("data\\challenge5.txt");
            Challenge5.Part2(lines);
        }

    }
}