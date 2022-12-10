namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge10.txt");
            Challenge10.Part1And2(lines);
        }
    }
}