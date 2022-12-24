namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge23.txt");
            Challenge23.Part2(lines);
        }
    }
}