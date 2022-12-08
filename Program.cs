namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge8.txt");
            Challenge8.Part2(lines);
        }
    }
}