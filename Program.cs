namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge16.txt");
            Challenge16.Part2(lines);
        }
    }
}