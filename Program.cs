namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge24.txt");
            Challenge24.Part2(lines);
        }
    }
}