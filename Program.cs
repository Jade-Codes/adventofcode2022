namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge7.txt");
            Challenge7.Part2(lines);
        }
    }
}