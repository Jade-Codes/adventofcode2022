namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge13.txt");
            Challenge13.Part2(lines);
        }
    }
}