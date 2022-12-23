namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge22.txt");
            Challenge22.Part1(lines);
        }
    }
}