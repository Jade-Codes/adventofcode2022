namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge25.txt");
            Challenge25.Part1(lines);
        }
    }
}