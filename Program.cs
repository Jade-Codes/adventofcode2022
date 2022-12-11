namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge11.txt");
            Challenge11.Part2(lines);
        }
    }
}