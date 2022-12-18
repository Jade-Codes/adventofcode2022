namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var input = File.ReadAllText("data\\challenge17.txt");
            Challenge17.Part2(input);
        }
    }
}