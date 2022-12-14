namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge14.txt");
            Challenge14.Part2(lines);
        }
    }
}