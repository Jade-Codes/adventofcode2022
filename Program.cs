namespace Challenges
{
    class Program
    {
        static void Main(string[] args)
        {
            var lines = File.ReadAllLines("data\\challenge9.txt");
            var dictCoords = new Dictionary<int, (int, int)>{
                {0, (0, 0)},
                {1, (0, 0)},
                {2, (0, 0)},
                {3, (0, 0)},
                {4, (0, 0)},
                {5, (0, 0)},
                {6, (0, 0)},
                {7, (0, 0)},
                {8, (0, 0)},
                {9, (0, 0)}
            };
            Challenge9.Part1And2(lines, dictCoords);
        }
    }
}