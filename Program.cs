namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge9.txt");
            Challenge9.Part2(lines);
        }
    }
}