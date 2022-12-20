namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge19.txt");
            Challenge19.Part1(lines);
        }
    }
}