namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge12.txt");
            Challenge12.Part2(lines);
        }
    }
}