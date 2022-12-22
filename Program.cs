namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge21.txt");
            Challenge21.Part2(lines);
        }
    }
}