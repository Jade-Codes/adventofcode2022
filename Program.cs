namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge15.txt");
            Challenge15.Part2(lines);
        }
    }
}