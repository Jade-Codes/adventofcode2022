namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge18.txt");
            Challenge18.Part1(lines);
        }
    }
}